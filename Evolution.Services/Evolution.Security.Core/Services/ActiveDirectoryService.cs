using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;

namespace Evolution.Security.Core.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";
        private readonly IAppLogger<ActiveDirectoryService> _logger = null;

        private readonly ADConfiguration _config;
        private readonly LdapConnection _connection;
        private readonly JObject _messages = null;

        public ActiveDirectoryService(IOptions<ADConfiguration> config, IAppLogger<ActiveDirectoryService> logger, JObject messages)
        {
            _logger = logger;
            _config = config.Value;
            _messages = messages;
            if (!_config.IsSandBoxEnvironment)
            {
                _config.LdapSearchFilter = "(&(objectClass=user)(objectClass=person)(|(userprincipalname={0}@*)(sAMAccountName={0})))";
                _config.LdapServerPort = _config.LdapServerPort <= 0 ? LdapConnection.DefaultSslPort : _config.LdapServerPort;
                _connection = new LdapConnection
                {
                    SecureSocketLayer = _config.EnableSecureSocketLayer
                };
            }
        }

        public bool IsValidADUsers(List<KeyValuePair<string, string>> usernames, out List<ValidationMessage> validationMessage)
        {
            validationMessage = new List<ValidationMessage>();
            try
            {
                if (_config.IsSandBoxEnvironment)
                    return true;
                else
                {
                    _connection.Connect(_config.LdapUrl, _config.LdapServerPort);
                    _connection.Bind(string.Format("cn={0},{1}", _config.LdapUser, _config.LdapServiceAccountDn), _config.LdapPswd);

                    foreach (KeyValuePair<string, string> user in usernames)
                    {
                        if (!IsUserNameExists(_connection, user))
                        {
                            validationMessage.Add(_messages, user, MessageType.InvalidADUser);
                        }
                    }
                    return validationMessage?.Count <= 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                throw;
            }
            finally
            {
                if (_connection != null && _connection.Connected)
                    _connection.Disconnect();
            }
        }


        public bool IsUserNameExists(LdapConnection _connection, KeyValuePair<string, string> username)
        {
            try
            {
                if (string.IsNullOrEmpty(username.Key)|| string.IsNullOrEmpty(username.Value)) return false;
                else
                {
                    var searchFilter = string.Format(_config.LdapSearchFilter, username.Key);
                    var searchResult = _connection.Search(
                        _config.LdapSearchBase,
                        LdapConnection.ScopeSub,
                        searchFilter,
                        new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute, "useraccountcontrol" , "mail" },
                        false
                    );

                    var user = searchResult.Next();
                    if (user != null)
                    {
                        // changed as requested by ITK on feb092021 (DEF 1476)
                        long accStatus = Convert.ToInt64(user?.GetAttribute("useraccountcontrol")?.StringValue);
                        string email = user?.GetAttribute("mail")?.StringValue;
                        if ((accStatus == 514 || accStatus == 66050 || accStatus == 512 || accStatus == 66048) && string.Equals(email, username.Value,StringComparison.InvariantCultureIgnoreCase))//512 = Enabled , 66048 = Enabled, password never expires, 514 = Disabled,66050 = Disabled, password never expires
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Referral") // "Referral" : this error value will come when AD account not present in LDAP with difined filter condition
                {
                    return false;
                }
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                throw new Exception("System is not able to connect to LDAP Server.", ex);
            }
            finally
            {
                if (_connection != null && _connection.Connected)
                    _connection.Disconnect();
            }
            return false;
        }
    }
}

