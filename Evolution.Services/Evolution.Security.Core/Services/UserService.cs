using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Interfaces.Validations;
using Evolution.Security.Domain.Models.Security;
using Evolution.Security.Model.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IAppLogger<UserService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IUserRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IUserValidationService _validationService = null;
        private readonly IApplicationRepository _applicationRepository = null;
        private readonly ICompanyService _companyService = null;
        private readonly ICompanyOfficeService _companyOfficeService = null;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ICustomerUserProjectRepository _customerUserProjectRepository = null;

        #region Constructor
        public UserService(IMapper mapper,
                             IUserRepository repository,
                             IApplicationRepository applicationRepository,
                             ICompanyService companyService,
                             ICompanyOfficeService companyOfficeService,
                             IAppLogger<UserService> logger,
                             IUserValidationService validationService,
                             JObject messgaes,
                             IPasswordHasher<string> passwordHasher,
                             IAuditSearchService auditSearchService,
                             ICustomerUserProjectRepository customerUserProjectRepository)
        {
            this._applicationRepository = applicationRepository;
            this._companyService = companyService;
            this._companyOfficeService = companyOfficeService;
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messageDescriptions = messgaes;
            this._passwordHasher = passwordHasher;
            _auditSearchService = auditSearchService;
            _customerUserProjectRepository = customerUserProjectRepository;
        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(UserInfo searchModel, string[] excludeUserTypes = null, bool isGetAllUser = false)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.Search(searchModel, excludeUserTypes, isGetAllUser);
                    tranScope.Complete();
                }
                //var dbApplications = dbUsers.Select(x => x.Application).ToList();
                //var dbCompany = dbUsers.Select(x => x.Company).ToList();
                //var dbCompanyOffices = dbUsers.Select(x => x.CompanyOffice).ToList();
                //var dbCompanyUserTypes = dbUsers.SelectMany(x => x.UserType).ToList();
                //result = _mapper.Map<IList<DomainModel.UserInfo>>(dbUsers, config =>
                //{
                //    config.Items.Add("dbApplication", dbApplications);
                //    config.Items.Add("dbCompany", dbCompany);
                //    config.Items.Add("dbCompanyOffice", dbCompanyOffices);
                //    config.Items.Add("dbCompanyUserType", dbCompanyUserTypes);
                //});
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetUser(string companyCode, string userType, bool isActive = true)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                result = _repository.GetUser(companyCode, new List<string> { userType }, isActive);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { companyCode,  userType , isActive });
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(UserInfo searchModel, ref IList<DbModel.User> dbUsers, params string[] includes)
        {
            Exception exception = null;
            IList<UserInfo> result = null;
            try
            {
                Expression<Func<DbModel.User, bool>> whereClause = null;
                if (searchModel != null)
                {
                    var dbSearchModel = this._mapper.Map<DbModel.User>(searchModel);
                    whereClause = dbSearchModel.ToExpression(new List<string> {
                        "LockoutEnabled",
                        "EmailConfirmed",
                        "PhoneNumberConfirmed",
                        "IsPasswordNeedToBeChange",
                        "IsShowNewVisit"
                    });
                }

                var users = _repository.FindBy(whereClause, new string[] { nameof(DbModel.User.UserType) });
                if (includes?.Count() > 0)
                {
                    includes.ToList().ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x))
                            users = users.Include(x);
                    });
                }
                dbUsers = users.ToList();
                result = _mapper.Map<IList<DomainModel.UserInfo>>(dbUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> userNames)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DomainModel.UserInfo>>(this.GetUserByName(userNames).ToList());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> userNames, bool IsUserTypeRequired)
        {
            IList<DomainModel.UserInfos> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _mapper.Map<IList<DomainModel.UserInfos>>(this.GetUserByName(userNames).ToList());
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(string applicationName, IList<string> userNames)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{// Commented : as this is giving  error "Connection currently has transaction enlisted.  Finish current transaction and retry." please re check in all flow.
                result = _mapper.Map<IList<DomainModel.UserInfo>>(this.GetUserByName(userNames?.Select(x => new KeyValuePair<string, string>(applicationName, x)).ToList()));
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByUserType(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DomainModel.UserInfo>>(this.GetUserByUserTypeAndCompany(companyCode, userTypes, isFilterCompanyActiveCoordinators));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByUserType(IList<string> loginNames, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                //Getting Exception - Email log is not created. It is only used in Visit/Timesheet
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _mapper.Map<IList<DomainModel.UserInfo>>(this.GetUserByUserTypeAndCompany(loginNames, userTypes, isFilterCompanyActiveCoordinators));
                    //tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByUserType(string companyCode, IList<string> userTypes, bool isUserTypeRequired, bool isFilterCompanyActiveCoordinators = false)
        {
            IList<DomainModel.UserInfos> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    var data = this.GetUserByUserTypeAndCompany(companyCode, userTypes, isFilterCompanyActiveCoordinators, isUserTypeRequired);
                    result = _mapper.Map<IList<DomainModel.UserInfos>>(data);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        //Added for ITK Defect 908(Ref ALM Document 14-05-2020)
        public Response GetUserType(string companyCode, IList<string> userTypes)
        {
            IList<DomainModel.UserTypeInfo> result = null;
            Exception exception = null;
            try
            {
                if (!string.IsNullOrEmpty(companyCode) && userTypes?.Count > 0) {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {

                        result = _repository.Get(companyCode, userTypes);
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        //Added for ITK Defect 908(Ref ALM Document 14-05-2020)

        // Added for Email Notification
        public IList<DbModel.UserType> GetUsersByTypeAndCompany(string companyCode, IList<string> userTypes)
        {
            IList<DbModel.UserType> result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                result = this.GetUserByTypeAndCompany(companyCode, userTypes);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypes);
            }
            return result;
        }

        public IList<DbModel.User> GetUsersByCompanyAndName(string companyCode, string userName)
        {
            IList<DbModel.User> result = null;
            Exception exception = null;
            try
            {
                if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(userName))
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        result = _repository.GetUserByCompanyAndName(companyCode, userName);
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName, companyCode);
            }
            return result;
        }

        public Response Get(IList<int> userIds)
        {
            IList<DomainModel.UserInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this.GetUserById(userIds)
                                    .AsQueryable()
                                    .ProjectTo<DomainModel.UserInfo>()
                                    .ToList();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetUserCompanyRoles(string userName, string menuName)
        {
            IList<DomainModel.UserCompanyRole> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    result = _repository.GetUserCompanyRoles(userName, menuName);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetViewAllAssignments(string samAccountName)
        {
            IList<ViewAllRights> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    result = _repository.GetViewAllAssignments(samAccountName);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> appUserNames,
                                          ref IList<DbModel.User> dbUsers,
                                          ref IList<string> userNotExists)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (dbUsers == null && appUserNames?.Count > 0)
                    dbUsers = GetUserByName(appUserNames);

                result = IsUserExistInDb(appUserNames, dbUsers, ref userNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), appUserNames);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<UserInfo> users, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.Company> dbCompany = null;
            return AddUser(users, ref dbUsers, ref dbApplications, ref dbCompany, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<UserInfo> users,
                            ref IList<DbModel.User> dbUsers,
                            ref IList<DbModel.Application> dbApplications,
                            ref IList<DbModel.Company> dbCompany,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddUser(users, ref dbUsers, ref dbApplications, ref dbCompany, ref eventId, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<UserInfo> users,
                                ref IList<DbModel.User> dbUsers,
                                ref IList<DbModel.Application> dbApplications,
                                ref IList<DbModel.Company> dbCompany,
                                ref long? eventId,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            return UpdateUsers(users, ref dbUsers, ref dbApplications, ref dbCompany, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<UserInfo> users, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.Company> dbCompany = null;
            return Modify(users, ref dbUsers, ref dbApplications, ref dbCompany, ref eventId, commitChange, isDbValidationRequire);
        }

        //public Response Modify(IList<DbModel.User> dbUsers, string modifiedBy)
        //{
        //    Exception exception = null;
        //    try
        //    {
        //        dbUsers?.ToList().ForEach(dbUser =>
        //        {
        //            dbUser.IsAllowedDuringLock = true;
        //            dbUser.IsActive = true;
        //            dbUser.LastModification = DateTime.UtcNow;
        //            dbUser.UpdateCount = dbUser.UpdateCount.CalculateUpdateCount();
        //            dbUser.ModifiedBy = modifiedBy;
        //        });
        //        _repository.AutoSave = false;
        //        _repository.Update(dbUsers);
        //        _repository.ForceSave();
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbUsers?.ToList());
        //    }
        //    finally
        //    {
        //        _repository.AutoSave = true;
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        //}

        #endregion

        #region Delete
        public Response Delete(IList<UserInfo> userInfo, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return this.RemoveUser(userInfo, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check    
        public Response IsRecordValidForProcess(IList<UserInfo> users, ValidationType validationType, params string[] childTableToBeExcludes)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DomainModel.UserInfo> filteredUsers = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.Company> dbCompany = null;
            return this.CheckRecordValidForProcess(users,
                                                    validationType,
                                                    ref filteredUsers,
                                                    ref dbUsers,
                                                    ref dbApplications,
                                                    ref dbCompany,
                                                    childTableToBeExcludes);
        }

        public Response IsRecordValidForProcess(IList<UserInfo> users,
                                                ValidationType validationType,
                                                ref IList<DbModel.User> dbUsers,
                                                ref IList<DbModel.Application> dbApplications,
                                                ref IList<DbModel.Company> dbCompany,
                                                 params string[] childTableToBeExcludes)
        {
            IList<DomainModel.UserInfo> filteredUsers = null;
            return this.CheckRecordValidForProcess(users,
                                                    validationType,
                                                    ref filteredUsers,
                                                    ref dbUsers,
                                                    ref dbApplications,
                                                    ref dbCompany,
                                                    childTableToBeExcludes);
        }
        #endregion

        #endregion

        #region Private Metods
        #region Get

        private IList<DbModel.User> GetUserByName(IList<KeyValuePair<string, string>> appUserNames)
        {
            IList<DbModel.User> dbUsers = null;
            if (appUserNames?.Count > 0)
            {
                var appNames = appUserNames.Select(x => x.Key);
                var userNames = appUserNames.Select(x => x.Value);
                dbUsers = _repository.FindBy(x => userNames.Contains(x.SamaccountName) && appNames.Contains(x.Application.Name), new string[] { nameof(DbModel.User.UserType) }).ToList();
            }
            return dbUsers;
        }

        private IList<DbModel.User> GetUserByName(IList<string> appUserNames)
        {
            IList<DbModel.User> dbUsers = null;
            if (appUserNames?.Count > 0)
                dbUsers = _repository.FindBy(x => appUserNames.Contains(x.SamaccountName), new string[] { nameof(DbModel.User.CompanyOffice) }).ToList();
            return dbUsers;
        }

        private IList<DbModel.User> GetUserByUserTypeAndCompany(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false, bool isUserTypeRequired = false)
        {
            IList<DbModel.User> dbUsers = null;
            if (!string.IsNullOrEmpty(companyCode) && userTypes?.Count > 0)
            {
                dbUsers = _repository.Get(companyCode, userTypes, isFilterCompanyActiveCoordinators, isUserTypeRequired);
            }
            return dbUsers;
        }

        private IList<DbModel.User> GetUserByUserTypeAndCompany(IList<string> loginNames, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false, bool isUserTypeRequired = false)
        {
            IList<DbModel.User> dbUsers = null;
            if (userTypes?.Count > 0)
            {
                dbUsers = _repository.Get(loginNames, userTypes, isFilterCompanyActiveCoordinators, isUserTypeRequired);
            }
            return dbUsers;
        }

        //908
        /** We make the _repository.Get call on Responce Method Itself on 12-02-2021*/
        //private IList<DbModel.UserType> GetUsersByUserTypeAndCompany(string companyCode, IList<string> userTypes)
        //{
        //    IList<DbModel.UserType> dbUsers = null;
        //    if (!string.IsNullOrEmpty(companyCode) && userTypes?.Count > 0)
        //    {
        //        dbUsers = _repository.Get(companyCode, userTypes);
        //    }
        //    return dbUsers;
        //}
        //908

        // Added for Email Notification
        private IList<DbModel.UserType> GetUserByTypeAndCompany(string companyCode, IList<string> userTypes)
        {
            IList<DbModel.UserType> dbUsers = null;
            if (!string.IsNullOrEmpty(companyCode) && userTypes?.Count > 0)
            {
                dbUsers = _repository.GetUserByType(companyCode, userTypes);
            }
            return dbUsers;
        }

        private IList<DbModel.User> GetUserById(IList<int> userIds)
        {
            IList<DbModel.User> dbUsers = null;
            if (userIds?.Count > 0)
                dbUsers = _repository.FindBy(x => userIds.Contains((int)x.Id)).ToList();

            return dbUsers;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.UserInfo> users,
                                         ref IList<DomainModel.UserInfo> filteredUsers,
                                         ref IList<DbModel.User> dbUsers,
                                         ref IList<DbModel.Application> dbApplications,
                                         ref IList<DbModel.Company> dbCompany,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (users != null && users.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUsers == null || filteredUsers.Count <= 0)
                    filteredUsers = FilterRecord(users, validationType);

                if (filteredUsers?.Count > 0 && IsValidPayload(filteredUsers, validationType, ref validationMessages))
                {
                    //IList<int> userIds = null;
                    IList<string> userNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    this.GetUserAndApplicationDbInfo(filteredUsers, false, ref dbUsers, ref dbApplications);
                    var applications = users.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        var appUserNames = users.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.LogonName)).Distinct().ToList();
                        var response = (!IsUserExistInDb(appUserNames, dbUsers, ref userNotExists, ref messages) && messages.Count == appUserNames.Count);
                        if (!response)
                        {
                            var userAlreadyExists = appUserNames.Where(x => !userNotExists.Contains(x.Value)).ToList();
                            userAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messageDescriptions, x, MessageType.UserAlreadyExist, x.Value);
                            });

                            if (messages.Count > 0)
                            {
                                result = false;
                                validationMessages.AddRange(messages);
                            }
                        }
                        else
                        {
                            result = _companyService.IsValidCompany(users.Select(x => x.CompanyCode).ToList(), ref dbCompany, ref validationMessages, x => x.CompanyOffice);
                            if (result)
                            {
                                if (users.Any(x => !string.IsNullOrEmpty(x.CompanyOfficeName)))
                                {
                                    var compCodeAndOfficeName = users.Select(x => new KeyValuePair<string, string>(x.CompanyCode, x.CompanyOfficeName)).ToList();
                                    IList<DbModel.CompanyOffice> compOffices = dbCompany?.SelectMany(x => x.CompanyOffice).ToList();
                                    result = _companyOfficeService.IsValidCompanyAddress(compCodeAndOfficeName, ref compOffices, ref validationMessages);
                                }
                            }
                        }
                    }
                    else
                        result = false;
                }
                else
                    result = false;
            }

            return result;
        }

        private Response AddUser(IList<UserInfo> users,
                                    ref IList<DbModel.User> dbUsers,
                                    ref IList<DbModel.Application> dbApplications,
                                    ref IList<DbModel.Company> dbCompany,
                                    ref long? eventId,
                                    bool commitChange = true,
                                    bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0;
            try
            {
                Response valdResponse = null;
                IList<UserInfo> recordToBeAdd = null;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(users, ValidationType.Add, ref recordToBeAdd, ref dbUsers, ref dbApplications, ref dbCompany);
                else
                    recordToBeAdd = FilterRecord(users, ValidationType.Add);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.UserId = null; return x; }).ToList();

                    IList<DbModel.Application> dbApps = dbApplications;
                    IList<DbModel.Company> dbComps = dbCompany;
                    IList<DbModel.CompanyOffice> dbCompOffices = dbCompany?.SelectMany(x => x.CompanyOffice).ToList();
                    var mappedRecordToBeAdd = _mapper.Map<IList<DbModel.User>>(recordToBeAdd, opt =>
                     {
                         opt.Items["dbApplication"] = dbApps;
                         opt.Items["dbCompany"] = dbComps;
                         opt.Items["dbCompanyOffice"] = dbCompOffices;
                     });

                    //HashPassword(recordToBeAdd, ref mappedRecordToBeAdd);
                    mappedRecordToBeAdd = mappedRecordToBeAdd
                                            .Select(x =>
                                            {
                                                x.Id = 0;
                                                x.IsPasswordNeedToBeChange = (LogonMode.UP.ToString() == x.AuthenticationMode.ToUpper());
                                                x.UpdateCount = null;
                                                x.ModifiedBy = null;
                                                x.LastModification = null;
                                                x.CreatedDate = DateTime.UtcNow;
                                                return x;
                                            }).ToList();

                    _repository.Add(mappedRecordToBeAdd);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        dbUsers = mappedRecordToBeAdd;
                        ProcessCustomerUserProjectAccess(recordToBeAdd, true);
                        if (value > 0 && mappedRecordToBeAdd?.Count > 0 && recordToBeAdd?.Count > 0)
                        {
                            mappedRecordToBeAdd?.ToList().ForEach(x =>
                            recordToBeAdd?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                "{" + AuditSelectType.Id + ":" + x.Id + "}${" + AuditSelectType.Name + ":" + x.Name?.Trim() + "}${" + AuditSelectType.CompanyName + ":" + x.Company?.Name?.Trim() + "}",
                                                                                               ValidationType.Add.ToAuditActionType(),
                                                                                               SqlAuditModuleType.User,
                                                                                               null,
                                                                                               _mapper.Map<DomainModel.UserInfo>(x))));
                            eventId = eventID;
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), users);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateUsers(IList<UserInfo> users,
                                        ref IList<DbModel.User> dbUsers,
                                        ref IList<DbModel.Application> dbApplications,
                                        ref IList<DbModel.Company> dbCompany,
                                        ref long? eventId,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(users, ValidationType.Update);

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(users, ValidationType.Update, ref recordToBeModify, ref dbUsers, ref dbApplications, ref dbCompany);
                else if ((dbUsers == null || dbUsers?.Count <= 0) && recordToBeModify?.Count > 0)
                    dbUsers = _repository.Get(recordToBeModify?.Select(x => (int)x.UserId).ToList());

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbUsers?.Count > 0))
                {
                    // Manage Security Audit changes - starts
                    IList<UserInfo> domUserInfo = new List<UserInfo>();
                    dbUsers?.ToList().ForEach(x =>
                    {
                        domUserInfo.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.UserInfo>(x)));
                    });
                    // Manage Security Audit changes - ends
                    IList<DbModel.Application> dbApps = dbApplications;
                    IList<DbModel.Company> dbComp = dbCompany;
                    IList<DbModel.CompanyOffice> dbCompOff = dbComp?.SelectMany(x => x.CompanyOffice).ToList();
                    dbUsers.ToList().ForEach(dbUser =>
                    {
                        var userToBeModify = recordToBeModify.FirstOrDefault(x => x.UserId == dbUser.Id);
                        var authMode = dbUser?.AuthenticationMode;//cyber security fix not allowed to change auth mode
                        ////  bool IsAllowedDuringLock=dbUser?.UserRole?.Select(x => x.Role)?.Where(x1 => x1.IsAllowedDuringLock == true).Select(x2=>x2.IsAllowedDuringLock);
                        //dbUser.ApplicationId = dbApps.FirstOrDefault(x => x.Name == userToBeModify.ApplicationName).Id;
                        //dbUser.SamaccountName = userToBeModify.LogonName; //User Module - Ability to change user name  CR Changes
                        //dbUser.AuthenticationMode = userToBeModify.AuthenticationMode;
                        //dbUser.CompanyId = dbComp?.FirstOrDefault(x => x.Code == userToBeModify.CompanyCode).Id;
                        //dbUser.CompanyOfficeId = dbComp?.FirstOrDefault(x => x.Code == userToBeModify.CompanyCode)?.CompanyOffice.FirstOrDefault(x => x.OfficeName == userToBeModify.CompanyOfficeName)?.Id;
                        //dbUser.Culture = userToBeModify.Culture;
                        //dbUser.Email = userToBeModify.Email;
                        ////dbUser.EmailConfirmed = userToBeModify.IsEmailConfirmed;
                        //dbUser.IsActive = userToBeModify.IsActive;
                        //dbUser.IsPasswordNeedToBeChange = userToBeModify.IsPasswordNeedToBeChange;
                        //dbUser.Name = userToBeModify.UserName;
                        //dbUser.PhoneNumber = userToBeModify.PhoneNumber;
                        //// dbUser.IsAllowedDuringLock=  ? true : null
                        ////dbUser.PhoneNumberConfirmed = userToBeModify.IsPhoneNumberConfirrmed;
                        ////dbUser.UserType = userToBeModify.UserType; 
                        //dbUser.SecurityQuestion1 = userToBeModify.SecurityQuestion1;
                        //dbUser.SecurityQuestion1Answer = userToBeModify.SecurityQuestion1Answer;
                        //dbUser.LastModification = DateTime.UtcNow;
                        //dbUser.UpdateCount = userToBeModify.UpdateCount.CalculateUpdateCount();
                        //dbUser.ModifiedBy = userToBeModify.ModifiedBy;
                        _mapper.Map(userToBeModify, dbUser, opt =>
                        {
                            opt.Items["dbApplication"] = dbApps;
                            opt.Items["dbCompany"] = dbComp;
                            opt.Items["dbCompanyOffice"] = dbCompOff;
                        });
                        if (Convert.ToBoolean(dbUser.IsActive) && !dbUser.LockoutEnabled) //cyber security issue fix : lock user account on invalid login attempt in EVO2 
                        {
                            dbUser.LockoutEndDateUtc = null;
                            dbUser.AccessFailedCount = null; 
                           // dbUser.LockoutEnabled = false;
                        }
                        dbUser.AuthenticationMode = authMode;
                        dbUser.LastModification = DateTime.UtcNow;
                        dbUser.UpdateCount = userToBeModify.UpdateCount.CalculateUpdateCount();
                        dbUser.CompanyOfficeId = dbComp?.FirstOrDefault(x => x.Code == userToBeModify.CompanyCode)?.CompanyOffice.FirstOrDefault(x => x.OfficeName == userToBeModify.CompanyOfficeName)?.Id; //Changes for Live D728
                    });

                    _repository.AutoSave = false;
                    _repository.Update(dbUsers);

                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0 && dbUsers?.Count > 0 && recordToBeModify?.Count > 0)
                        {
                            ProcessCustomerUserProjectAccess(recordToBeModify);
                            dbUsers?.ToList().ForEach(x =>
                            recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                  "{" + AuditSelectType.Id + ":" + x.Id + "}${" + AuditSelectType.Name + ":" + x.Name?.Trim() + "}${" + AuditSelectType.CompanyName + ":" + x.Company?.Name?.Trim() + "}",
                                                                                                  ValidationType.Update.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.User,
                                                                                                  _mapper.Map<UserInfo>(domUserInfo?.FirstOrDefault(x2 => x2.UserId == x1.UserId)),
                                                                                                  x1
                                                                                                  )));
                            eventId = eventID;
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), users);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        private Response ProcessCustomerUserProjectAccess(IList<UserInfo> userInfo, bool isFetchUserIds = false)
        {
            Exception exception = null;
            List<CustomerUserProject> customerUserProjects = new List<CustomerUserProject>();
            var dbUserProjects = new List<DbModel.CustomerUserProjectAccess>();
            List<int> projectNos = new List<int>();
            try
            {
                if (userInfo?.Count > 0)
                {
                    if (isFetchUserIds) //For Processing New Users Project Data
                    {
                        var logonNames = userInfo.Where(x => x.AuthenticationMode.ToString() == LogonMode.UP.ToString() && !string.IsNullOrEmpty(x.LogonName)).Select(x => x.LogonName);
                        var userIds = _repository.FindBy(x => logonNames.Contains(x.SamaccountName)).Select(x => new { x.SamaccountName, x.Id });
                        customerUserProjects = userInfo.Where(x => x.CustomerUserProjectNumbers != null && x.CustomerUserProjectNumbers.Any())
                                                   ?.Join(userIds,
                                                   custPrj => new { SamaccountName = custPrj.LogonName },
                                                   usr => new { usr.SamaccountName },
                                                   (custPrj, usr) => new { custPrj, usr })
                                                  .SelectMany(x =>
                                                  {
                                                      x.custPrj.CustomerUserProjectNumbers = x.custPrj.CustomerUserProjectNumbers.Select(x1 => { x1.UserId = x.usr.Id; return x1; }).ToList();
                                                      return x.custPrj.CustomerUserProjectNumbers;
                                                  }).ToList();
                    }
                    else //For Processing Existing Users Project Data
                    {
                        customerUserProjects = userInfo.Where(x => x.AuthenticationMode.ToString() == LogonMode.UP.ToString() && x.CustomerUserProjectNumbers != null && x.CustomerUserProjectNumbers.Any())
                            .SelectMany(x1 => x1.CustomerUserProjectNumbers).ToList();
                        projectNos = customerUserProjects?.Where(x1 => x1.RecordStatus.IsRecordStatusDeleted()).Select(x => x.ProjectNumber).ToList(); //Changes for D-726
                        List<int?> usrIds = customerUserProjects?.Where(x => x.UserId > 0)?.Select(x => x.UserId).Distinct().ToList();
                        dbUserProjects = _customerUserProjectRepository?.FindBy(x => usrIds.Contains(x.UserId)).ToList() ?? new List<DbModel.CustomerUserProjectAccess>(); // fetch the db Customer User Projects based on UserId
                    }
                    if (customerUserProjects != null && customerUserProjects?.Count > 0)
                    {
                        if (dbUserProjects != null)
                        {
                            var newProjectData = customerUserProjects
                                .GroupJoin(dbUserProjects,
                                   cust => cust.UserId,
                                   prj => prj.UserId,
                                  (cust, prj) => new { cust, prj })
                                .Where(x => (x.prj != null && !x.prj.Any(x1 => x1.ProjectId == x.cust.ProjectNumber)) || x.prj.Count() == 0)
                                .Select(x => x.cust).ToList(); // Comparing the ProjectIds to get the newly Authorised projects
                            var deleteUserProjects = dbUserProjects?.Where(x => projectNos.Contains(x.ProjectId)).Select(x1 => x1).ToList(); //Comparing the ProjectIds to get the deleted Authorised Project

                            if (newProjectData != null && newProjectData.Any())
                            {
                                _customerUserProjectRepository.Add(_mapper.Map<IList<DbModel.CustomerUserProjectAccess>>(newProjectData));
                            }
                            if (deleteUserProjects != null & deleteUserProjects.Any())
                            {
                                _customerUserProjectRepository.Delete(_mapper.Map<IList<DbModel.CustomerUserProjectAccess>>(deleteUserProjects));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerUserProjects);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
        private bool IsRecordValidForUpdate(IList<DomainModel.UserInfo> users,
                                            ref IList<DomainModel.UserInfo> filteredUsers,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.User> dbUsers,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<DbModel.Company> dbCompany,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (users != null && users.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUsers == null || filteredUsers.Count <= 0)
                    filteredUsers = FilterRecord(users, validationType);

                if (filteredUsers?.Count > 0 && IsValidPayload(filteredUsers, validationType, ref messages))
                {
                    this.GetUserAndApplicationDbInfo(filteredUsers, true, ref dbUsers, ref dbApplications);
                    IList<int> userIds = filteredUsers?.Select(x => (int)x.UserId)?.ToList();
                    if (dbUsers?.Count != userIds?.Count) //Invalid User Id found.
                    {
                        var dbUserByIds = dbUsers;
                        var idNotExists = userIds?.Where(id => !dbUserByIds.Any(user => user.Id == id))?.ToList();
                        var userList = filteredUsers;
                        idNotExists?.ForEach(x =>
                        {
                            var user = userList?.FirstOrDefault(x1 => x1.UserId == x);
                            messages.Add(_messageDescriptions, user, MessageType.RequestedUpdateUserNotExists);
                        });
                    }
                    else
                    {
                        var applications = filteredUsers?.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName))?.Distinct()?.ToList();
                        if (this.IsApplicationExistInDb(applications, dbApplications, ref messages))
                        {
                            result = IsRecordUpdateCountMatching(filteredUsers, dbUsers, ref messages);
                            if (result)
                            {
                                result = this.IsUserNameUnique(filteredUsers, ref validationMessages);
                                if (result)
                                {
                                    var compCodes = filteredUsers?.Where(x1 => !string.IsNullOrEmpty(x1.CompanyCode))?.Select(x => x.CompanyCode)?.ToList();    //Added Where Condition for D-695(issue 5)
                                    if (compCodes?.Count > 0)
                                        result = _companyService.IsValidCompany(compCodes, ref dbCompany, ref validationMessages, x => x.CompanyOffice);
                                }
                            }
                        }
                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return result;
        }



        private bool IsRecordUpdateCountMatching(IList<DomainModel.UserInfo> users, IList<DbRepository.Models.SqlDatabaseContext.User> dbUsers, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = users.Where(x => dbUsers.ToList().Any(x1 => x1.UpdateCount.ToInt() != x.UpdateCount.ToInt() && x1.Id == x.UserId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.LogonName, MessageType.UserUpdateCountMismatch, x.LogonName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsUserNameUnique(IList<DomainModel.UserInfo> filteredUsers, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var userNames = filteredUsers.Select(x => x.LogonName);
            var appNames = filteredUsers.Select(x => x.ApplicationName);
            //var appUsers = _repository.FindBy(x => appNames.Contains(x.Application.Name))
            //                            .Join(userNames,
            //                                  user => user.SamaccountName,
            //                                  userName => userName,
            //                                  (user, userName) => user)
            //                            .ToList();
            var appUsers = _repository.FindBy(x => appNames.Contains(x.Application.Name) &&
                                                   userNames.Contains(x.SamaccountName)).ToList();
            if (appUsers?.Count > 0)
            {
                var userAlreadyExist = filteredUsers.Where(x => !appUsers.Select(x1 => x1.Id)
                                                                 .ToList()
                                                                 .Contains((int)x.UserId));
                userAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x.LogonName, MessageType.UserAlreadyExist, x.LogonName);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.UserInfo> users,
                                            ref IList<DomainModel.UserInfo> filteredUsers,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.User> dbUsers,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages,
                                            params string[] childTableToBeExcludes)
        {
            bool result = false;
            if (users != null && users.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUsers == null || filteredUsers.Count <= 0)
                    filteredUsers = FilterRecord(users, validationType);

                if (filteredUsers?.Count > 0 && IsValidPayload(filteredUsers, validationType, ref validationMessages))
                {
                    this.GetUserAndApplicationDbInfo(filteredUsers, false, ref dbUsers, ref dbApplications);
                    var applications = filteredUsers.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        IList<string> userNotExists = null;
                        var appUserNames = filteredUsers.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.LogonName)).Distinct().ToList();
                        result = IsUserExistInDb(appUserNames, dbUsers, ref userNotExists, ref validationMessages);
                        if (result)
                            result = IsUserCanBeRemove(dbUsers, ref validationMessages, childTableToBeExcludes);
                    }
                }
            }
            return result;
        }

        private bool IsUserCanBeRemove(IList<DbModel.User> dbUsers, ref IList<ValidationMessage> validationMessages, params string[] childTableToBeExcludes)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbUsers?.Where(x => x.IsAnyCollectionPropertyContainValue(childTableToBeExcludes))
                    .ToList()
                    .ForEach(x =>
                    {
                        messages.Add(_messageDescriptions, x.SamaccountName, MessageType.UserIsBeingUsed, x.SamaccountName);
                    });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private void GetUserAndApplicationDbInfo(IList<UserInfo> filteredUsers,
                                                 bool isUserInfoById,
                                                 ref IList<DbModel.User> dbUsers,
                                                 ref IList<DbModel.Application> dbAppications)
        {
            var userNames = !isUserInfoById ?
                            filteredUsers?.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.LogonName))?
                                         .Distinct()?
                                         .ToList() : null;

            IList<int> userIds = isUserInfoById ? filteredUsers?.Select(x => (int)x.UserId)?.Distinct()?.ToList() : null;
            IList<string> appNames = filteredUsers?.Select(x => x.ApplicationName)?.Distinct()?.ToList();
            if (dbUsers == null || dbUsers.Count <= 0)
                dbUsers = isUserInfoById ? this.GetUserById(userIds)?.ToList() : this.GetUserByName(userNames)?.ToList();
            if (dbAppications == null || dbAppications.Count <= 0)
                dbAppications = _applicationRepository.Get(appNames).ToList();
        }

        private IList<DomainModel.UserInfo> FilterRecord(IList<DomainModel.UserInfo> users,
                                                            ValidationType filterType)
        {
            IList<DomainModel.UserInfo> filteredUsers = null;

            if (filterType == ValidationType.Add)
                filteredUsers = users?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredUsers = users?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredUsers = users?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredUsers;
        }

        private bool IsUserExistInDb(IList<KeyValuePair<string, string>> appUserNames,
                                        IList<DbModel.User> dbUsers,
                                        ref IList<string> userNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbUsers == null)
                dbUsers = new List<DbModel.User>();

            var validMessages = validationMessages;

            appUserNames = appUserNames?.Where(x => !string.IsNullOrEmpty(x.Value))?.ToList();

            if (appUserNames?.Count > 0)
            {
                userNotExists = appUserNames
                                  .Where(x => !dbUsers
                                              .Any(x1 => x1.Application.Name == x.Key &&
                                                         x1.SamaccountName.ToLower() == x.Value.ToLower()))
                                  .Select(x => x.Value)
                                  .ToList();

                userNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.UserNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response RemoveUser(IList<UserInfo> users, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.Company> dbCompany = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(users, ValidationType.Delete, ref dbUsers, ref dbApplications, ref dbCompany);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result) && dbUsers?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbUsers);
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), users);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<DomainModel.UserInfo> users,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.UserInfo> filteredUsers,
                                                    ref IList<DbModel.User> dbUsers,
                                                    ref IList<DbModel.Application> dbAppications,
                                                    ref IList<DbModel.Company> dbCompany,
                                                    params string[] childTableToBeExcludes)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(users, ref filteredUsers, ref dbUsers, ref dbAppications, ref dbCompany, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(users, ref filteredUsers, ref dbUsers, ref dbAppications, ref validationMessages, childTableToBeExcludes);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(users, ref filteredUsers, ref dbUsers, ref dbAppications, ref dbCompany, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), users);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsUserExistInDb(IList<int> userIds,
                                        IList<DbModel.User> dbUsers,
                                        ref IList<int> userNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbUsers == null)
                dbUsers = new List<DbModel.User>();

            var validMessages = validationMessages;

            if (userIds?.Count > 0)
            {
                userNotExists = userIds.Where(x => !dbUsers.Select(x1 => (int)x1.Id).ToList().Contains(x)).ToList();
                userNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.UserNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsValidPayload(IList<DomainModel.UserInfo> users,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(users), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsApplicationExistInDb(IList<KeyValuePair<int, string>> applications,
                                            IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbApplications == null)
                dbApplications = new List<DbModel.Application>();

            var validMessages = validationMessages;

            if (applications?.Count > 0)
            {
                var appNotExists = applications
                                  .Where(x => dbApplications
                                              .Count(x1 =>
                                              {
                                                  var result = false;
                                                  if (x.Key > 0 && !string.IsNullOrEmpty(x.Value))
                                                      result = (x1.Id == x.Key && x1.Name == x.Value);
                                                  else if (x.Key > 0)
                                                      result = (x1.Id == x.Key);
                                                  else if (!string.IsNullOrEmpty(x.Value))
                                                      result = (x1.Name == x.Value);

                                                  return result;
                                              }) <= 0)
                                  .Select(x => x.Value)
                                  .ToList();

                appNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ApplicationNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void HashPassword(IList<DomainModel.UserInfo> userInfos, ref IList<DbModel.User> dbUsers)
        {
            foreach (var dbUser in dbUsers)
            {
                var domainUser = userInfos?.FirstOrDefault(x => x.LogonName == dbUser.SamaccountName);
                if (domainUser != null && !string.IsNullOrEmpty(domainUser.Password))
                {
                    dbUser.PasswordHash = this._passwordHasher.HashPassword(domainUser.LogonName, domainUser.Password);
                }
            }
        }


        #endregion

        #endregion
    }
}