using AutoMapper;
using Evolution.AuthorizationService.Interfaces;
using Evolution.AuthorizationService.Models;
using Evolution.AuthorizationService.Models.Tokens;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuthorizationService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserReposiotry _repository = null;
        private readonly IAuthClientRepository _authClientRepository = null;
        private readonly IRefreshTokenReposiotry _refreshRepository = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<AccountService> _logger = null;
        private readonly AppEnvVariableBaseModel _appEnvVariableBaseModel = null;
        private readonly IEmailQueueService _emailQueueService = null;
        private readonly JObject _message = null;
        private readonly IActiveDirectoryService _activeDirectoryService = null;

        public AccountService(IUserReposiotry repository,
                                IRefreshTokenReposiotry refreshRepository,
                                IAuthClientRepository authClientRepository,
                                IEmailQueueService emailQueueService,
                                IAppLogger<AccountService> logger,
                                IActiveDirectoryService activeDirectoryService,
                                IMapper mapper,
                                JObject messages,
                                IJwtHandler jwtHandler,
                                IOptions<AppEnvVariableBaseModel> appEnvVariableBaseModel)
        {
            _message = messages;
            _logger = logger;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _repository = repository;
            _authClientRepository = authClientRepository;
            _refreshRepository = refreshRepository;
            _appEnvVariableBaseModel = appEnvVariableBaseModel.Value;
            _emailQueueService = emailQueueService;
            _activeDirectoryService = activeDirectoryService;
        }

        public Response SignIn(string username, string password, string requestedIp, string clientCode, string audienceCode)
        {
            JsonWebToken result = null;
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            List<ValidationMessage> validationMessage = null;
            Exception exception = null;
            ResponseType responseType = ResponseType.Error;
            User userDetail = null;
            DbModel.Client clientDetail = null;
            try
            {
                messages = ValidateUser(username, password, ref userDetail, ref validationMessage, true);
                if (messages.Count <= 0)
                {
                    if (string.IsNullOrEmpty(clientCode) || string.IsNullOrEmpty(audienceCode) || !IsValidClientAndAudience(clientCode, audienceCode, ref clientDetail))
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidClientOrAudienceCode, ModuleType.Authentication));
                    }
                    else
                    {
                        TokenOption tokenOption = new TokenOption()
                        {
                            AccessTokenExpMins = clientDetail.AccessTokenExpMins,
                            Audience = audienceCode,
                            DefaultLangCulture = userDetail.Culture,
                            DefualtCompCode = userDetail.DefaultCompanyCode,
                            DefaultCompanyId = userDetail.DefaultCompanyId,
                            RefreshTokenExpMins = clientDetail.RefreshTokenExpMins,
                            SecretKey = clientDetail.SeckretKey,
                            Subject = userDetail.DisplayName,
                            TokenIssuer = clientDetail.TokenIssuer,
                            UniqueName = userDetail.Username,
                            UserType = userDetail.UserType,
                            Application = userDetail.Application,

                        };

                        long refreshTokenExprAsServerLocal = 0;
                        result = _jwtHandler.Create(tokenOption, ref refreshTokenExprAsServerLocal);
                        result.isAuthTokenAlreadyExistsForUser = IsAuthTokenExistsForUSer(userDetail.Username, out DbModel.RefreshToken refreshToken);
                        //if (result.isAuthTokenAlreadyExistsForUser)//TODO: This should be removed once session related issues are resolved
                        //{
                        //    _logger.LogError(ResponseType.Exception.ToId(), "Multiple login detected using same UserName", refreshToken);
                        //}
                        //Add Refresh Token to DB
                        AddRefreshToken(new RefreshToken()
                        {
                            AccessToken = result.AccessToken,
                            Username = username,
                            Token = result.RefreshToken,
                            RequestedIp = requestedIp,
                            TokenExpiretime = refreshTokenExprAsServerLocal,
                            Application = userDetail.Application
                        });
                        responseType = ResponseType.Success;

                        if (userDetail.UserId > 0)
                            _repository.Update(new DbModel.User { Id = userDetail.UserId, LastLoginDate = DateTime.UtcNow }, a => a.LastLoginDate);

                        this.ManageLockout(userDetail.Username, false);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, messages, null, validationMessage, result, exception, null);
        }

        public Response SignInByQuestionAnswer(UserQuestionAnswer userQuestionAnswer, string requestedIp, string clientCode, string audienceCode)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            bool result = false;
            try
            {
                Response response = ValidateUserAnswer(userQuestionAnswer);
                if (response.Code == ResponseType.Success.ToId())
                {
                    var userInfo = _repository.FindBy(x => x.Application.Name == userQuestionAnswer.Application &&
                                                               x.SamaccountName == userQuestionAnswer.UserLogonName &&
                                                               x.Email == userQuestionAnswer.UserEmail)
                                                   .Select(x => new { x.Name, x.PasswordHash })
                                                   .FirstOrDefault();
                    if (userInfo != null)
                    {
                        return SignIn(userQuestionAnswer.UserLogonName, userInfo.PasswordHash, requestedIp, clientCode, audienceCode);
                    }
                    else
                    {
                        throw new Exception("Invalid User Information.");
                    }
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                exception = ex;
                _logger.LogError(responseType.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, messages, null, null, result, exception, null);
        }

        public Response RefreshAccessToken(string token, string userName, string requestedIp, string clientCode, string audienceCode)
        {
            JsonWebToken result = null;
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            List<ValidationMessage> validationMessage = null;
            Exception exception = null;
            ResponseType responseType = ResponseType.Error;
            User userDetail = null;
            DbModel.Client clientDetail = null;
            try
            {
                messages = ValidateRefreshToken(token, userName, requestedIp, ref userDetail, ref validationMessage);
                if (messages.Count <= 0)
                {
                    if (string.IsNullOrEmpty(clientCode) || string.IsNullOrEmpty(audienceCode) || !IsValidClientAndAudience(clientCode, audienceCode, ref clientDetail))
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidClientOrAudienceCode, ModuleType.Authentication));
                    }
                    else
                    {
                        TokenOption tokenOption = new TokenOption()
                        {
                            AccessTokenExpMins = clientDetail.AccessTokenExpMins,
                            Audience = audienceCode,
                            DefaultLangCulture = userDetail.Culture,
                            DefualtCompCode = userDetail.DefaultCompanyCode,
                            RefreshTokenExpMins = clientDetail.RefreshTokenExpMins,
                            SecretKey = clientDetail.SeckretKey,
                            Subject = userDetail.DisplayName,
                            TokenIssuer = clientDetail.TokenIssuer,
                            UniqueName = userDetail.Username
                        };

                        long refreshTokenExprAsServerLocal = 0;
                        result = _jwtHandler.Create(tokenOption, ref refreshTokenExprAsServerLocal);

                        //Add Refresh Token to DB
                        RefreshToken newRefreshToken = new RefreshToken()
                        {
                            Username = userName,
                            Token = result.RefreshToken,
                            AccessToken = result.AccessToken,
                            RequestedIp = requestedIp,
                            TokenExpiretime = refreshTokenExprAsServerLocal
                        };
                        AddRefreshToken(newRefreshToken);
                        responseType = ResponseType.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, messages, null, validationMessage, result, exception, null);
        }

        public Response RevokeRefreshToken(string token, string userName, string requestedIp)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            List<ValidationMessage> validationMessage = null;
            ResponseType responseType = ResponseType.Error;
            Exception exception = null;
            User userDetail = null;
            try
            {
                messages = ValidateRefreshToken(token, userName, requestedIp, ref userDetail, ref validationMessage);
                if (messages.Count <= 0 && validationMessage.Count == 0)
                {
                    if (_appEnvVariableBaseModel.IsIpValidationRequire)
                    {
                        RemoveRefreshToken(token, userName, requestedIp);
                    }
                    else
                    {
                        RemoveRefreshToken(token, userName);
                    }

                    responseType = ResponseType.Success;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, messages, null, validationMessage, null, exception, null);
        }

        public Response CancelAccessToken(string token, string userName, string requestedIp)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            ResponseType responseType = ResponseType.Error;
            Exception exception = null;
            User userDetail = null;
            try
            {
                messages = ValidateAccessToken(token, userName, requestedIp, ref userDetail);
                if (messages.Count <= 0)
                {
                    if (_appEnvVariableBaseModel.IsIpValidationRequire)
                    {
                        RemoveAccessToken(token, userName, requestedIp);
                    }
                    else
                    {
                        RemoveAccessToken(token, userName);
                    }

                    responseType = ResponseType.Success;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, messages, null, null, null, exception, null);
        }

        public Response GetUserQuestion(string applicationName, string userLogonName, string userEmail)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            IList<string> result = null;
            try
            {
                result = _repository.FindBy(x => x.Application.Name == applicationName && x.SamaccountName == userLogonName && x.Email == userEmail)
                                         .Select(x => x.SecurityQuestion1).ToList();
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                exception = ex;
                _logger.LogError(responseType.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, messages, null, null, result, exception, result?.Count);
        }

        public Response ValidateUserAnswer(UserQuestionAnswer userQuestionAnswer)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            bool result = false;
            try
            {
                messages = ValidateUserAnswerModel(userQuestionAnswer);
                if (messages?.Count == 0)
                {
                    messages = new List<KeyValuePair<MessageType, ModuleType>>();
                    var dbUsers = _repository.FindBy(x => x.Application.Name == userQuestionAnswer.Application &&
                                                         x.SamaccountName == userQuestionAnswer.UserLogonName &&
                                                         x.Email == userQuestionAnswer.UserEmail).ToList();
                    var dbUser = dbUsers.FirstOrDefault();
                    var questionAnswers = dbUsers.Select(x => new { x.SecurityQuestion1, x.SecurityQuestion1Answer, x.AuthenticationMode }).ToList();
                    if (IsUserLocked(userQuestionAnswer.UserLogonName, ref dbUser))
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_User_Locked, ModuleType.Authentication));
                    else if (questionAnswers?.Count <= 0)
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_Security_Question_NotSetup, ModuleType.Authentication));
                    else if (questionAnswers.FirstOrDefault()?.AuthenticationMode == LogonMode.AD.ToString())
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_ForgetPassword_Not_For_User, ModuleType.Authentication));
                    else
                    {
                        bool answerMatch = true;
                        userQuestionAnswer.QuestionAnswers.ToList().ForEach(x =>
                        {
                            if (answerMatch)
                                answerMatch = questionAnswers.Any(x1 => x1.SecurityQuestion1.ToLower().Trim() == x.Question.ToLower().Trim() &&
                                                                        x1.SecurityQuestion1Answer.ToLower().Trim() == x.Answer.ToLower().Trim());

                        });

                        if (!answerMatch)
                        {
                            messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_Security_Question_Answer_Vald_Failed, ModuleType.Authentication));
                            this.ManageLockout(userQuestionAnswer.UserLogonName, true, dbUser);
                        }

                        result = answerMatch;
                    }
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                exception = ex;
                _logger.LogError(responseType.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, messages, null, null, result, exception, null);
        }

        public Response SendPassword(UserQuestionAnswer userQuestionAnswer)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            bool result = false;
            try
            {
                Response response = ValidateUserAnswer(userQuestionAnswer);
                if (response.Code == ResponseType.Success.ToId())
                {
                    var userInfo = _repository.FindBy(x => x.Application.Name == userQuestionAnswer.Application &&
                                                               x.SamaccountName == userQuestionAnswer.UserLogonName &&
                                                               x.Email == userQuestionAnswer.UserEmail)
                                                   .Select(x => new { x.Name, x.PasswordHash })
                                                   .ToList();
                    if (userInfo != null)
                    {
                        List<EmailBodyPlaceHolder> emailContentPlaceholders = new List<EmailBodyPlaceHolder>()
                        {
                            new EmailBodyPlaceHolder("[NAME]",userInfo.FirstOrDefault().Name,userQuestionAnswer.UserEmail),
                            new EmailBodyPlaceHolder("[PASSWORD]",userInfo.FirstOrDefault().PasswordHash,userQuestionAnswer.UserEmail)
                        };

                        string subject = _message[MessageType.ForgetPasswordEmailSubject.ToId()].ToString();
                        List<Email.Models.EmailAddress> toAddr = new List<Email.Models.EmailAddress>() { new Email.Models.EmailAddress() { Address = userQuestionAnswer.UserEmail } };
                        EmailQueueMessage emailQueueMessage = _emailQueueService.PopulateEmailQueueMessage(ModuleType.Authentication,
                                                                                             ModuleCodeType.AUTH,
                                                                                             userQuestionAnswer.UserLogonName,
                                                                                             EmailTemplate.EmailForgetPassword,
                                                                                             EmailType.Notification,
                                                                                             subject, emailContentPlaceholders, toAddr, null, null, false, true);
                        return _emailQueueService.Add(new List<EmailQueueMessage>() { emailQueueMessage });
                    }
                    else
                    {
                        throw new Exception("Invalid User Information.");
                    }
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                exception = ex;
                _logger.LogError(responseType.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, messages, null, null, result, exception, null);
        }

        public Response resetPassword(User user)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            bool result = false;
            try
            {
                var userInfoData = _repository.FindBy(x => x.Application.Name == user.Application &&
                                                            x.SamaccountName == user.Username)
                                                .FirstOrDefault();
                if (userInfoData != null)
                {
                    if (userInfoData.PasswordHash != user.PasswordHash)
                    {
                        userInfoData.PasswordHash = user.PasswordHash;
                        userInfoData.LastModification = DateTime.Now;
                        userInfoData.ModifiedBy = user.Username;
                        _repository.Update(userInfoData);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    throw new Exception("Invalid User Information.");
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                exception = ex;
                _logger.LogError(responseType.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, messages, null, null, result, exception, null);
        }

        private List<KeyValuePair<MessageType, ModuleType>> ValidateUserAnswerModel(UserQuestionAnswer userQuestionAnswer)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            try
            {
                messages = new List<KeyValuePair<MessageType, ModuleType>>();
                if (string.IsNullOrEmpty(userQuestionAnswer?.Application))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.ApplicationNameInvalid, ModuleType.Authentication));
                }
                else if (string.IsNullOrEmpty(userQuestionAnswer?.UserLogonName))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.UserLogonNameInvalid, ModuleType.Authentication));
                }
                else if (string.IsNullOrEmpty(userQuestionAnswer?.UserEmail))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.UserEmailInvalid, ModuleType.Authentication));
                }
                else if (userQuestionAnswer.QuestionAnswers == null || userQuestionAnswer.QuestionAnswers.Count <= 0 || userQuestionAnswer.QuestionAnswers.Any(x => string.IsNullOrEmpty(x.Question) || string.IsNullOrEmpty(x.Answer)))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_User_Question_Answer_Not_Passed, ModuleType.Authentication));
                }
            }
            catch (Exception ex)
            {
                messages?.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.UnableToProcess,
                    ModuleType.Authentication));
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return messages;
        }

        private List<KeyValuePair<MessageType, ModuleType>> ValidateUser(string userName, string password, ref User user, ref List<ValidationMessage> validationMessage, bool validatePswd = false)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            bool isDisabledAdAccount = false;
            try
            {
                messages = new List<KeyValuePair<MessageType, ModuleType>>();
                DbModel.User dbUser = null;
                if (string.IsNullOrEmpty(userName) || (validatePswd && string.IsNullOrEmpty(password)))
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidCredential, ModuleType.Authentication));

                if (user == null) 
                    user = GetUserData(userName, ref dbUser);

                if (user != null)
                {
                    bool IsEvolocked = false;
                    DbModel.Announcement announcement = _repository.CheckEvoutionLocked().FirstOrDefault();
                    if (announcement != null)
                        IsEvolocked = announcement.IsEvolutionLocked == true ? true : false;
                    if (IsEvolocked)
                    {
                        if (validationMessage == null)
                            validationMessage = new List<ValidationMessage>();
                        //if (_repository.GetRoles(dbUser?.UserRole.ToList())?.Count == 0)
                        int count = _repository.GetRoleCount(dbUser?.UserRole.ToList());
                        if (count == 0)
                        {
                            validationMessage.Add(_message, ModuleType.Authentication, MessageType.User_Denied_During_Evolution_Lock, announcement.EvolutionLockMessage);
                            messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.User_Denied_During_Evolution_Lock, ModuleType.Authentication));
                            if (messages?.Count > 0)
                                return messages;
                        }
                    }
                }
                if (user == null)
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidCredential, ModuleType.Authentication));
                else if (user.UserType.Split(',').Contains(UserType.Customer.ToString())) // Temp Fix : To resolve cyber security fix to avoid customer login as suggested in email . this should be removed after implementing Extranet module in EVO2.
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidCredential, ModuleType.Authentication));
                else if (IsUserLocked(user.Username, ref dbUser))
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_Account_Locked, ModuleType.Authentication));
                else if (user.AuthenticationType == LogonMode.UP.ToString() && user.PasswordHash != password?.Trim() && validatePswd)
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidCredential, ModuleType.Authentication));
                    this.ManageLockout(userName, true, dbUser);
                }
                else if (user.AuthenticationType == LogonMode.AD.ToString() && !_activeDirectoryService.ValidateLogin(userName, password, out isDisabledAdAccount, validatePswd))
                {
                    if (isDisabledAdAccount)//cyber security issue fix : to inform disabled AD user account on login attempt in EVO2
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_Account_Locked, ModuleType.Authentication));
                    else
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidCredential, ModuleType.Authentication));

                    this.ManageLockout(userName, true, dbUser);
                }
                else if (user.AuthenticationType == LogonMode.AD.ToString() && isDisabledAdAccount)//cyber security issue fix : to inform disabled AD user account on login attempt in EVO2
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_Account_Locked, ModuleType.Authentication));
                    this.ManageLockout(userName, true, dbUser);
                }
                else if (user.IsActive.HasValue && !Convert.ToBoolean(user.IsActive.Value))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_UserIsNotActive, ModuleType.Authentication));
                }
                else if (!user.IsRoleAssigned)
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_User_Role_Not_Associated, ModuleType.Authentication));
                }
            }
            catch (Exception ex)
            {
                messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.UnableToProcess, ModuleType.Authentication));
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName);
            }

            return messages;
        }

        private List<KeyValuePair<MessageType, ModuleType>> ValidateRefreshToken(string token, string userName, string requestedIp, ref User user, ref List<ValidationMessage> validationMessage)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            try
            {
                messages = new List<KeyValuePair<MessageType, ModuleType>>();

                if (string.IsNullOrEmpty(token))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidRefreshToken, ModuleType.Authentication));
                }
                else
                {
                    RefreshToken tokenDetail = null;
                    if (_appEnvVariableBaseModel.IsIpValidationRequire)
                    {
                        tokenDetail = GetRefreshToken(token, userName, requestedIp);
                    }
                    else
                    {
                        tokenDetail = GetRefreshToken(token, userName);
                    }

                    if (tokenDetail == null)
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidRefreshToken, (ModuleType.Authentication)));
                    }
                    else if (tokenDetail.TokenExpiretime <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_ExpiredRefreshToken, (ModuleType.Authentication)));
                    }
                    else if (tokenDetail.Username.Trim().ToLower() != userName.Trim().ToLower() || (_appEnvVariableBaseModel.IsIpValidationRequire && tokenDetail.RequestedIp.Trim() != requestedIp.Trim()))
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_TokenAssocFailed, (ModuleType.Authentication)));
                    }
                    else
                    {
                        List<KeyValuePair<MessageType, ModuleType>> userVald = ValidateUser(userName, string.Empty, ref user, ref validationMessage, false);
                        if (userVald != null && userVald.Count > 0)
                        {
                            messages.AddRange(userVald);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messages?.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.UnableToProcess, ModuleType.Authentication));
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName);
            }

            return messages;
        }

        private List<KeyValuePair<MessageType, ModuleType>> ValidateAccessToken(string token, string userName, string requestedIp, ref User user)
        {
            List<KeyValuePair<MessageType, ModuleType>> messages = null;
            try
            {
                messages = new List<KeyValuePair<MessageType, ModuleType>>();

                if (string.IsNullOrEmpty(token))
                {
                    messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidAccessToken, ModuleType.Authentication));
                }
                else
                {
                    RefreshToken tokenDetail = null;
                    if (_appEnvVariableBaseModel.IsIpValidationRequire)
                    {
                        tokenDetail = GetAccessToken(token, userName, requestedIp);
                    }
                    else
                    {
                        tokenDetail = GetAccessToken(token, userName);
                    }

                    if (tokenDetail == null)
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_InvalidAccessToken, (ModuleType.Authentication)));
                    }
                    else if (!tokenDetail.Username.Trim().Equals(userName.Trim(),StringComparison.InvariantCultureIgnoreCase) || (_appEnvVariableBaseModel.IsIpValidationRequire && tokenDetail.RequestedIp.Trim() != requestedIp.Trim()))
                    {
                        messages.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.Auth_TokenAssocFailed, (ModuleType.Authentication)));
                    }
                }
            }
            catch (Exception ex)
            {
                messages?.Add(new KeyValuePair<MessageType, ModuleType>(MessageType.UnableToProcess, ModuleType.Authentication));
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName);
            }

            return messages;
        }

        private User GetUserData(string username, ref DbModel.User dbUser)
        {
            var dbUserData = _repository.FindBy(x => x.SamaccountName == username && (x.AuthenticationMode == "AD" || x.AuthenticationMode == "UP"))?.Select(x => new DbModel.User
            {
                Id = x.Id,
                Name = x.Name,
                SamaccountName = x.SamaccountName,
                Culture = x.Culture,
                CompanyId = x.CompanyId,
                IsActive = x.IsActive,
                LockoutEnabled = x.LockoutEnabled,
                LockoutEndDateUtc = x.LockoutEndDateUtc,
                LastLoginDate = x.LastLoginDate,
                Company = new DbModel.Company { Code = x.Company.Code, Name = x.Company.Name },
                ApplicationId = x.ApplicationId,
                PasswordHash = x.PasswordHash,
                AuthenticationMode = x.AuthenticationMode,
                AccessFailedCount=x.AccessFailedCount,
                Application = new DbModel.Application { Name = x.Application.Name },
                UserRole = x.UserRole.Select(x1 => new DbModel.UserRole { Id = x1.Id, RoleId = x1.RoleId }).ToList(),
                UserType = x.UserType.Select(x1 => new DbModel.UserType { CompanyId = x1.CompanyId, UserTypeName = x1.UserTypeName }).ToList()

            })?.FirstOrDefault();

            dbUser = dbUserData;
            return _mapper.Map<User>(dbUser);
        }


        private User GetUser(string username, ref DbModel.User dbUser)
        {
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<DbModel.User, ICollection<DbModel.UserRole>> dbUsers = _repository.FindBy(x => x.SamaccountName == username)
                                     .Include(x => x.Application)
                                     .Include(x => x.Company)
                                     .Include(x => x.UserType)
                                     .Include(x => x.UserRole);
            dbUser = dbUsers?.FirstOrDefault();
            return _mapper.Map<User>(dbUser);
        }

        private bool IsValidClientAndAudience(string clientCode, string audienceCode, ref DbModel.Client client)
        {
            client = _authClientRepository.FindBy(x => x.ClientCode == clientCode &&
                                                        x.ClientAudience.Any(x1 => x1.Audience.AudienceCode == audienceCode)).FirstOrDefault();

            return client != null;
        }

        private RefreshToken GetRefreshToken(string token, string userName, string requestedIp)
        {
            return _mapper.Map<DbModel.RefreshToken, RefreshToken>(_refreshRepository.FindBy(x => x.Token == token &&
                                                                                                            x.Username == userName &&
                                                                                                            x.RequestedIp == requestedIp).FirstOrDefault());
        }

        private RefreshToken GetRefreshToken(string token, string userName)
        {
            return _mapper.Map<DbModel.RefreshToken, RefreshToken>(_refreshRepository.FindBy(x => x.Token == token &&
                                                                                                            x.Username == userName).FirstOrDefault());
        }

        private RefreshToken GetAccessToken(string token, string userName, string requestedIp)
        {
            return _mapper.Map<DbModel.RefreshToken, RefreshToken>(_refreshRepository.FindBy(x => x.AccessToken == token &&
                                                                                                            x.Username == userName &&
                                                                                                            x.RequestedIp == requestedIp).FirstOrDefault());
        }

        private RefreshToken GetAccessToken(string token, string userName)
        {
            return _mapper.Map<DbModel.RefreshToken, RefreshToken>(_refreshRepository.FindBy(x => x.AccessToken == token &&
                                                                                                            x.Username == userName).FirstOrDefault());
        }

        private void AddRefreshToken(RefreshToken token)
        {
            try
            {
                if (string.IsNullOrEmpty(token.Application))
                {
                    token.Application = "Dummy";//TODO : Need to remove later once everything finalize.
                }

                _refreshRepository.AutoSave = true;
                if (_refreshRepository.FindBy(x => x.Username == token.Username)?.Select(x => x.Id)?.Count() > 0)
                {
                    //Deleting existing Token.
                    _refreshRepository.Delete(x => x.Username == token.Username);
                }
                _mapper.Map<RefreshToken>(_refreshRepository.Add(_mapper.Map<DbRepository.Models.SqlDatabaseContext.RefreshToken>(token)));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        private bool IsAuthTokenExistsForUSer(string userName, out DbModel.RefreshToken refreshToken)
        {
            refreshToken = null;
            try
            {
                refreshToken = _refreshRepository.FindBy(x => x.Username == userName)?.FirstOrDefault();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return !string.IsNullOrEmpty(refreshToken?.AccessToken);
        }

        private void RemoveRefreshToken(string refreshToken, string userName, string requestedIp)
        {
            _refreshRepository.Delete(x => x.Token == refreshToken && x.Username == userName && x.RequestedIp == requestedIp);
        }

        private void RemoveRefreshToken(string refreshToken, string userName)
        {
            _refreshRepository.Delete(x => x.Token == refreshToken && x.Username == userName);
        }

        private void RemoveAccessToken(string accessToken, string userName, string requestedIp)
        {
            _refreshRepository.Delete(x => x.AccessToken == accessToken && x.Username == userName && x.RequestedIp == requestedIp);
        }

        private void RemoveAccessToken(string accessToken, string userName)
        {
            _refreshRepository.Delete(x => x.AccessToken == accessToken && x.Username == userName);
        }

        private bool IsUserLocked(string samaAccountName, ref DbModel.User dbUser)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(samaAccountName) && dbUser == null)
                    dbUser = _repository.FindBy(x => x.SamaccountName == samaAccountName).FirstOrDefault();

                if (dbUser?.AuthenticationMode == LogonMode.AD.ToString())
                {
                    result = !Convert.ToBoolean(dbUser.IsActive);//cyber security issue fix : lock user account on invalid login attempt in EVO2
                }
                else
                    result = dbUser.LockoutEnabled;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return result;
        }

        private void ManageLockout(string samaAccountName, bool isFailedAttempt, DbModel.User dbUserInfo = null)
        {
            try
            {
                DbModel.User dbUser = dbUserInfo;
                Int32? accessFailedCount = null;
                DateTime? lockoutEndDateUtc = null;

                if (dbUser == null && !string.IsNullOrEmpty(samaAccountName))
                    dbUser = _repository.FindBy(x => x.SamaccountName == samaAccountName).Select(x => new DbModel.User
                    {
                        Id = x.Id,
                        LockoutEnabled = x.LockoutEnabled,
                        AccessFailedCount = x.AccessFailedCount,
                        LockoutEndDateUtc = x.LockoutEndDateUtc,
                        AuthenticationMode = x.AuthenticationMode,
                        IsActive = x.IsActive,
                    }).FirstOrDefault();

                if (dbUser == null) return; //cyber security issue fix : to lock  user in EVO2 on multiple failed login attempt.
                if (isFailedAttempt)
                {
                    accessFailedCount = Convert.ToInt32(dbUser.AccessFailedCount) + 1;
                    if (accessFailedCount > _appEnvVariableBaseModel.LockUserAfterFailedAttemptCount)
                    {
                        dbUser.LockoutEnabled = true; //cyber security issue fix : to lock  user in EVO2 on multiple failed login attempt.
                        lockoutEndDateUtc = DateTime.UtcNow.AddYears(100);

                        if (dbUser.AuthenticationMode == LogonMode.AD.ToString())
                        {
                            dbUser.IsActive = false;//cyber security issue fix : to lock  user in EVO2 on multiple failed login attempt.
                        }
                    }
                }
                else
                {
                    dbUser.LockoutEnabled = false;
                    dbUser.IsActive = true;
                }

                dbUser.AccessFailedCount = accessFailedCount;
                dbUser.LockoutEndDateUtc = lockoutEndDateUtc;
                _repository.Update(dbUser, x => x.AccessFailedCount, x1 => x1.LockoutEndDateUtc, x2 => x2.IsActive, x3 => x3.LockoutEnabled);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

    }
}