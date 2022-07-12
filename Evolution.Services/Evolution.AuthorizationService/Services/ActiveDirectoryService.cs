using Evolution.AuthorizationService.Interfaces; 
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Logging.Interfaces;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;

namespace Evolution.AuthorizationService.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";
        private readonly IAppLogger<ActiveDirectoryService> _logger = null;

        private readonly ADConfiguration _config;
        private readonly LdapConnection _connection;

        public ActiveDirectoryService(IOptions<ADConfiguration> config, IAppLogger<ActiveDirectoryService> logger)
        {
            _logger = logger;
            _config = config.Value;
            if (!_config.IsSandBoxEnvironment)
            {
                _config.LdapSearchFilter = "(&(objectClass=user)(objectClass=person)(|(userprincipalname={0}@*)(sAMAccountName={0})))";
                _config.LdapServerPort = _config.LdapServerPort <= 0 ? LdapConnection.DEFAULT_SSL_PORT : _config.LdapServerPort;
                _connection = new LdapConnection
                {
                    SecureSocketLayer =_config.EnableSecureSocketLayer
                };
            }
        }

        //public ActiveDirectoryService()
        //{
        //    _config = new ADConfiguration();
        //    _config.LdapServerPort = LdapConnection.DEFAULT_SSL_PORT;
        //    _config.LdapUrl = "netserv.local";
        //    _config.LdapDomainName = @"netserv";
        //    _config.LdapPswd = "gajendra82";
        //    _config.LdapSearchBase = string.Format("DC={0},DC=local", _config.LdapUrl);
        //    _config.LdapSearchFilter = "(&(objectClass=user)(objectClass=person)(sAMAccountName={0}))";
        //    _connection = new LdapConnection
        //    {
        //        SecureSocketLayer = true
        //    };
        //}

        public bool ConnectToLDAPServer()
        {
            try
            {
                if (_config.IsSandBoxEnvironment)
                    return true;

                if (string.IsNullOrEmpty(_config.LdapUrl))
                    throw new Exception("LDAPServer URL is empty");
                else
                    _connection.Connect(_config.LdapUrl, _config.LdapServerPort);

                if (string.IsNullOrEmpty(_config.LdapServiceAccountDn))
                    throw new Exception("LDAP ServiceAccountDn is empty");
                else if(string.IsNullOrEmpty(_config.LdapUser))
                    throw new Exception("LDAP UserName is empty");
                else if (string.IsNullOrEmpty(_config.LdapPswd))
                    throw new Exception("LDAP password is empty");
                else if (!string.IsNullOrEmpty(_config.LdapServiceAccountDn) && !string.IsNullOrEmpty(_config.LdapUser) && !string.IsNullOrEmpty(_config.LdapPswd))
                     _connection.Bind(string.Format("cn={0},{1}", _config.LdapUser,_config.LdapServiceAccountDn), _config.LdapPswd); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                throw new Exception("System is not able to connect to LDAP Server.", ex);
            }
            finally
            {
                if (_connection != null && _connection.Connected)
                    _connection.Disconnect();
            }
            return true;
        }

        public bool ValidateLogin(string username, string password,out bool isDisabledAdAccount, bool validatePswd = false)
        {
            bool result = false;
            isDisabledAdAccount = false;
            try
            {
                if (_config.IsSandBoxEnvironment)
                    result = true;
                else
                {
                    if (ConnectToLDAPServer())
                    {
                        _connection.Connect(_config.LdapUrl, _config.LdapServerPort);
                        _connection.Bind(string.Format("cn={0},{1}", _config.LdapUser, _config.LdapServiceAccountDn), _config.LdapPswd); 

                        var searchFilter = string.Format(_config.LdapSearchFilter, username);
                        var searchResult = _connection.Search(
                            _config.LdapSearchBase,
                            LdapConnection.SCOPE_SUB,
                            searchFilter,
                            new[] {  MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute , "useraccountcontrol" },
                            false
                        );

                        var user = searchResult.Next();
                        if (user != null)
                        {
                            // changed as requested by ITK on feb092021 (DEF 1476)
                            long accStatus = Convert.ToInt64(user.getAttribute("useraccountcontrol")?.StringValue);
                            if (accStatus == 514 || accStatus == 66050)//514 = Disabled,66050 = Disabled, password never expires
                            {
                                isDisabledAdAccount = true;
                            }
                            if (accStatus == 512 || accStatus== 66048) //512 = Enabled , 66048 = Enabled, password never expires
                            {
                                isDisabledAdAccount = false;
                            } 
                            _connection.Bind(user.DN, password);
                            result = validatePswd ? _connection.Bound : true ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "Invalid Credentials" || ex.Message == "Referral") // "Referral" : this error value will come when AD account not present in LDAP with difined filter condition
                {
                    return false;
                } //Invalid Credential Fix (password wrong time)
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                throw new Exception("System is not able to connect to LDAP Server.", ex);
            }
            finally
            {
                if (_connection !=null && _connection.Connected)
                    _connection.Disconnect();
            }
            return result;
        }
    }
}

