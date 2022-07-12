using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Security.Core.Services
{
    public class UserDetailService : IUserDetailService
    {
        private readonly IAppLogger<UserDetailService> _logger = null;
        private readonly IUserService _userService = null;
        private readonly IRoleService _roleService = null;
        private readonly IMapper _mapper = null;
        private readonly IUserDetailRepository _repository = null;
        private readonly IRoleDetailService _roleDetailService = null;
        private readonly IModuleDetailService _moduleDetailService = null;
        private readonly IUserRoleService _userRoleService = null;
        private readonly IUserTypeService _userTypeService = null;
        private readonly ICompanyService _companyService = null;
        public readonly IApplicationMenuRepository _menuRepository = null;
        private readonly JObject _messages = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly IActiveDirectoryService _activeDirectoryService = null;

        #region Constructor
        public UserDetailService(IMapper mapper,
                                    IUserDetailRepository repository,
                                    IApplicationMenuRepository menuRepository,
                                    IAppLogger<UserDetailService> logger,
                                    IRoleDetailService roleDetailService,
                                    IModuleDetailService moduleDetailService,
                                    IUserService userService,
                                    IUserRoleService userRoleService,
                                    IUserTypeService userTypeService,
                                    IRoleService roleService,
                                    ICompanyService companyService,
                                    IActiveDirectoryService activeDirectoryService,
                                    JObject messages,
                                    IMemoryCache memoryCache,
                                    IOptions<AppEnvVariableBaseModel> environment)
        {
            _repository = repository;
            _menuRepository = menuRepository;
            _roleDetailService = roleDetailService;
            _moduleDetailService = moduleDetailService;
            _userService = userService;
            _userRoleService = userRoleService;
            _userTypeService = userTypeService;
            _roleService = roleService;
            _companyService = companyService;
            _activeDirectoryService = activeDirectoryService;
            _logger = logger;
            _mapper = mapper;
            _messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }
        #endregion

        #region Public Methods
        public Response Get(UserInfo searchModel)
        {
            IList<UserDetail> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repository.Search(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(UserInfo searchModel, string[] excludeUserTypes)
        {
            IList<UserDetail> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repository.Search(searchModel, excludeUserTypes);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        public Response GetUserMenu(string applicationName, string userLogonName, string companyCode)
        {
            IList<UserMenuPermissionInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repository.GetMenuList(applicationName, userLogonName, companyCode);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { UserName = userLogonName, CompanyCode = companyCode });
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        //public Response GetUserMenu(string applicationName, string userLogonName, string companyCode)
        //{
        //    IList<UserMenuPermissionInfo> result = null;
        //    IList<UserDetail> userDetails = null;
        //    IList<ApplicationMenuInfo> menuList = null;
        //    Exception exception = null;
        //    try
        //    {
        //        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
        //        {
        //            menuList = _menuRepository.Search(applicationName);
        //            userDetails = _repository.Search(new UserInfo()
        //            {
        //                ApplicationName = applicationName,
        //                LogonName = userLogonName,
        //                IsActive = true //Assuming in-active user will be not allow to access this service
        //            });
        //            tranScope.Complete();
        //        }

        //        var roleDetails = GetRoleDetail(companyCode, userDetails);
        //        var moduleActivities = ExtractModuleActivity(roleDetails);

        //        result = PopulatMenuPermission(moduleActivities, menuList);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { UserName = userLogonName, CompanyCode = companyCode });
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        //}

        public Response GetUserType(string userLogonName, string companyCode)
        {
            IList<UserTypeInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    var dbUserTypes = _userTypeService.Get(companyCode, userLogonName).Result.Populate<IList<DbModel.UserType>>();
                    if (dbUserTypes?.Count > 0)
                    {
                        result = _mapper.Map<IList<UserTypeInfo>>(dbUserTypes);
                    }
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { UserName = userLogonName, CompanyCode = companyCode });
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response GetUserPermission(string applicationName, string userLogonName, int companyId, string moduleName)
        {
            return PopulateUserPermission(companyId, userLogonName, moduleName);
            //return PopulateUserPermission(applicationName, userLogonName, companyCode, moduleName);
        }

        public Response GetUserPermission(string applicationName, string userLogonName, string companyCode)
        {
            return PopulateUserPermission(applicationName, userLogonName, companyCode, null);
        }

        public bool GetUserPermission(int companyId, string userLogonName, string moduleName, List<string> activities)
        {
            bool result = false;
            Exception exception = null;
            try
            {
                int IsAllowed = _repository.UserPermission(companyId, userLogonName, moduleName, activities);
                if (IsAllowed == activities.Count)
                    result = true;
            }
            catch (Exception ex)
            {
                exception = ex;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), string.Format("{0}", "{1}", "{2}", companyId, userLogonName, moduleName));
            }
            return result;
        }

        public Response Add(IList<UserDetail> userDetails)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.UserRole> dbUserRoles = null;
            IList<DbModel.Role> dbRoles = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.Company> dbCompany = null;
            IList<UserInfo> userInfos = null;
            IList<RoleInfo> roleInfos = null;
            IList<UserRoleInfo> userRoleInfos = null;
            IList<UserTypeInfo> userTypeInfos = null;
            IList<DbModel.UserType> dbUserTypes = null;
            Response response = null;
            Exception exception = null;
            long? eventId = null; // Manage Security Audit changes
            try
            { 
                //Populate default role for some User Type
                PopulateDefaultRoleForUser(ref userDetails);

                //Check if additional company has role assigned
                var roles = userDetails.SelectMany(x => x.CompanyRoles).Where(x => x.Roles?.Count == 0).ToList();

                //Checking, Whether provided user and activity exists in DB or not
                response = Validate(userDetails,
                                         ValidationType.Add,
                                         ref userInfos,
                                         ref roleInfos,
                                         ref userRoleInfos,
                                         ref dbUsers,
                                         ref dbUserRoles,
                                         ref dbRoles,
                                         ref dbApplications,
                                         ref dbCompany,
                                         ref userTypeInfos,
                                         ref dbUserTypes);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                {
                    return response;
                }

                if (dbUsers?.Count <= 0)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        response = _userService.Add(userInfos, ref dbUsers, ref dbApplications, ref dbCompany, ref eventId, true, true); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        AppendUserRoleEvent(ref userRoleInfos, eventId); // Manage Security Audit changes

                        response = AddUserRole(userRoleInfos, dbApplications, dbUsers, dbRoles, dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        AppendUserTypeEvent(ref userTypeInfos, eventId); // Manage Security Audit changes

                        response = AddUserType(userTypeInfos, dbUsers, dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userDetails);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response Delete(IList<UserInfo> userInfos)
        {
            Exception exception = null;
            Response response = null;
            IList<ValidationMessage> valdMsg = null;
            try
            {
                if (userInfos != null)
                {
                    response = _userService.IsRecordValidForProcess(userInfos, ValidationType.Delete, nameof(DbModel.User.UserRole));
                    if (response.Code == MessageType.Success.ToId())
                    {
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            var count = _repository.DeleteUser(userInfos.Select(x => x.LogonName).ToList());
                            if (count > 0)
                            {
                                tranScope.Complete();
                            }
                        }
                    }
                    else
                    {
                        return response;
                    }
                }
            }
            catch (SqlException)
            {
                var moduleType = ModuleType.Security;
                var msgTypeId = MessageType.UserIsBeingUsed.ToId();
                var msg = _messages[msgTypeId].ToString().Replace("({0})", "");
                var msgDetail = new List<MessageDetail>() { new MessageDetail(moduleType, msgTypeId, msg) };
                valdMsg = new List<ValidationMessage>()
                {
                    new ValidationMessage(userInfos,msgDetail)
                };
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, valdMsg?.ToList(), null, exception);
        }

        public Response Modify(IList<UserDetail> userDetails)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.UserRole> dbUserRoles = null;
            IList<DbModel.Role> dbRoles = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.Company> dbCompany = null;
            IList<UserInfo> users = null;
            IList<RoleInfo> roles = null;
            IList<UserRoleInfo> userRoleInfos = null;
            IList<UserTypeInfo> userTypeInfos = null;
            IList<DbModel.UserType> dbUserTypes = null;
            Response response = null;
            Exception exception = null;
            long? eventId = null; // Manage Security Audit changes
            try
            { 
                //Checking, Whether provided user and activity exists in DB or not
                response = Validate(userDetails,
                                    ValidationType.Update,
                                    ref users,
                                    ref roles,
                                    ref userRoleInfos,
                                    ref dbUsers,
                                    ref dbUserRoles,
                                    ref dbRoles,
                                    ref dbApplications,
                                    ref dbCompany,
                                    ref userTypeInfos,
                                    ref dbUserTypes);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                {
                    return response;
                }

                if (dbUsers?.Count > 0)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        _repository.AutoSave = false;
                        response = _userService.Modify(users, ref dbUsers, ref dbApplications, ref dbCompany, ref eventId, true, false); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        AppendUserRoleEvent(ref userRoleInfos, eventId); // Manage Security Audit changes

                        response = DeleteUserRole(userRoleInfos, ref dbUsers, ref dbUserRoles, ref dbRoles, ref dbApplications, ref dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        response = AddUserRole(userRoleInfos, dbApplications, dbUsers, dbRoles, dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        AppendUserTypeEvent(ref userTypeInfos, eventId); // Manage Security Audit changes

                        response = ModifyUserType(userTypeInfos, dbUsers, dbUserTypes, dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }


                        response = DeleteUserType(userTypeInfos, ref dbUsers, ref dbUserTypes, ref dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        response = AddUserType(userTypeInfos, dbUsers, dbCompany, ref eventId); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                        {
                            return response;
                        }

                        _repository.ForceSave();
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userDetails);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response ChangeUserStatus(string applicationName, string userSamaAccount, bool newStatus)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Company> dbCompany = null;
            IList<DbModel.Application> dbApplication = null;
            IList<UserInfo> users = null;
            IList<string> userNotExists = null;
            Response response = null;
            Exception exception = null;
            long? eventId = null; // Manage Security Audit changes
            try
            {
                //Checking, Whether provided user exists in DB or not
                var appUser = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(applicationName,userSamaAccount)
                };
                response = _userService.IsRecordExistInDb(appUser, ref dbUsers, ref userNotExists);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                {
                    return response;
                }

                if (dbUsers?.Count > 0)
                {
                    users = _mapper.Map<IList<UserInfo>>(dbUsers);
                    if (users?.Count > 0)
                    {
                        users = users.Select(x => { x.IsActive = newStatus; x.RecordStatus = RecordStatus.Modify.FirstChar(); return x; }).ToList();
                        response = _userService.Modify(users, ref dbUsers, ref dbApplication, ref dbCompany, ref eventId);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new KeyValuePair<string, string>(applicationName, userSamaAccount));
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetUserRoleCompany(IList<KeyValuePair<string, string>> appUserLogonNames)
        {
            Exception exception = null;
            IList<UserCompanyInfo> userComps = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    userComps = _repository.GetUserRoleCompany(appUserLogonNames);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), appUserLogonNames);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, userComps, exception);
        }
        #endregion

        #region Private Methods
        private bool ValidateUserNameExistesInActiveDirectory(List<KeyValuePair<string, string>> userNames,out List<ValidationMessage>  validationMessage)
        {
            validationMessage= new List<ValidationMessage>();
            if (userNames?.Count <= 0)
            {
                validationMessage.Add(_messages, null, MessageType.InvalidADUser);
                return false;
            }
            else {
                return _activeDirectoryService.IsValidADUsers(userNames, out validationMessage);
            } 
        }

        private Response Validate(IList<UserDetail> userDetails,
                                    ValidationType type,
                                    ref IList<UserInfo> userInfos,
                                    ref IList<RoleInfo> roleInfos,
                                    ref IList<UserRoleInfo> userRoleInfos,
                                    ref IList<DbModel.User> dbUsers,
                                    ref IList<DbModel.UserRole> dbUserRoles,
                                    ref IList<DbModel.Role> dbRoles,
                                    ref IList<DbModel.Application> dbApplications,
                                    ref IList<DbModel.Company> dbCompany,
                                    ref IList<UserTypeInfo> userTypeInfos,
                                    ref IList<DbModel.UserType> dbUserTypes)
        {
            Response response = null;
             
            response = IsDefaultCompanyRoleAndUsertypeAssigned(userDetails, type);
            if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
            {
                string status = Utility.GetRecordStatus(type).FirstChar();
                var recordToBeProcess = userDetails?.Where(x => (x.User.RecordStatus == status) == true);
                userInfos = recordToBeProcess.Select(x => x.User).ToList();

                if (userInfos.Any(x =>!(x?.AuthenticationMode == "AD" || x?.AuthenticationMode == "UP")))
                {
                    return new Response().ToPopulate(ResponseType.Validation, null, null, new List<ValidationMessage> { new ValidationMessage("AuthenticationMode", new List<MessageDetail> { new MessageDetail(ModuleType.Security, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }

                List<KeyValuePair<string, string>> userNames = userInfos.Where(x=>x.AuthenticationMode=="AD")?.Select(x => new KeyValuePair<string,string>( x.LogonName, x.Email ) ).ToList();
                if (userNames?.Count > 0 && !ValidateUserNameExistesInActiveDirectory(userNames, out List<ValidationMessage> validationMessages))
                {
                    return new Response().ToPopulate(ResponseType.Validation, null, null, validationMessages?.ToList(), false, null);
                }

                if (dbCompany == null || dbCompany?.Count <= 0)
                {
                    var companycodes = recordToBeProcess.Select(x => x.User.CompanyCode).ToList()
                                       .Union(recordToBeProcess.Where(x => x.CompanyRoles != null).SelectMany(x => x.CompanyRoles.Select(x1 => x1.CompanyCode)))
                                       .Union(recordToBeProcess.Where(x => x.CompanyUserTypes != null).SelectMany(x => x.CompanyUserTypes.Select(x1 => x1.CompanyCode)))
                                       .ToList();
                    IList<ValidationMessage> validationMessage = new List<ValidationMessage>();
                    var result = _companyService.IsValidCompany(companycodes, ref dbCompany, ref validationMessage, x => x.CompanyOffice);
                    if (!result)
                    {
                        return new Response().ToPopulate(ResponseType.Success, validationMessage, false, null);
                    }
                }

                response = _userService.IsRecordValidForProcess(userInfos, type, ref dbUsers, ref dbApplications, ref dbCompany);
                if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                {
                    var newStatus = Utility.GetRecordStatus(ValidationType.Add).FirstChar();
                    var deleteStatus = Utility.GetRecordStatus(ValidationType.Delete).FirstChar();

                    roleInfos = userDetails.Where(x => x.CompanyRoles != null &&
                                                       x.CompanyRoles.Count > 0 &&
                                                       x.CompanyRoles.Any(x1 => x1.Roles != null &&
                                                                                x1.Roles.Count > 0))
                                           .SelectMany(x2 => x2.CompanyRoles.SelectMany(x3 => x3.Roles))
                                                                            .Where(x1 => x1.RecordStatus == newStatus ||
                                                                                         x1.RecordStatus == deleteStatus).ToList();

                    IList<string> roleNotExists = null;
                    var appRoleNames = roleInfos.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.RoleName)).ToList();
                    response = _roleService.IsRecordExistInDb(appRoleNames, ref dbRoles, ref roleNotExists);
                    if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                    {
                        IList<UserRoleInfo> userRoleNotExists = null;
                        response = IsValidUserRole(type, userDetails, ref userRoleInfos, ref userRoleNotExists, ref dbUserRoles);
                    }

                    if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                    {
                        IList<UserTypeInfo> userTypeNotExists = null;
                        response = IsValidUserType(type, userDetails, ref userTypeInfos, ref userTypeNotExists, ref dbUserTypes);
                    }
                }
            }
            return response;
        }

        private Response ModifyUserType(IList<UserTypeInfo> userTyepInfos,
                                  IList<DbModel.User> dbUsers,
                                  IList<DbModel.UserType> dbUserType,
                                  IList<DbModel.Company> dbCompany,
                                  ref long? eventId) // Manage Security Audit changes
        {
            if (userTyepInfos?.Count > 0 && dbUsers?.Count > 0 && dbCompany?.Count > 0)
            {
                return _userTypeService.Modify(userTyepInfos,
                                                 ref dbUserType,
                                                 ref dbUsers,
                                                 ref dbCompany,
                                                 ref eventId, // Manage Security Audit changes
                                                 true,
                                                 true);
            }
            else
            {
                return new Response().ToPopulate(ResponseType.Success, true);
            }
        }

        private Response AddUserRole(IList<UserRoleInfo> userRoleInfos,
                                     IList<DbModel.Application> dbApplication,
                                     IList<DbModel.User> dbUsers,
                                     IList<DbModel.Role> dbRoles,
                                     IList<DbModel.Company> dbCompany,
                                     ref long? eventId) // Manage Security Audit changes
        {
            if (userRoleInfos?.Count > 0 && dbUsers?.Count > 0 && dbApplication?.Count > 0)
            {
                IList<DbModel.UserRole> dbUSerRole = null;
                return _userRoleService.Add(userRoleInfos,
                                                 ref dbUSerRole,
                                                 ref dbApplication,
                                                 ref dbUsers,
                                                 ref dbRoles,
                                                 ref dbCompany,
                                                 ref eventId);
            }
            else
            {
                return new Response().ToPopulate(ResponseType.Success, true);
            }
        }

        private Response IsValidUserRole(ValidationType validationType,
                                            IList<UserDetail> userDetails,
                                            ref IList<UserRoleInfo> userRoleInfos,
                                            ref IList<UserRoleInfo> userRoleNotExists,
                                            ref IList<DbModel.UserRole> dbUserRoles)
        {
            var messages = new List<ValidationMessage>();
            if (userDetails?.Count > 0)
            {
                var recordStatus = Utility.GetRecordStatus(validationType).FirstChar();
                var newStatus = Utility.GetRecordStatus(ValidationType.Add).FirstChar();
                var deleteStatus = Utility.GetRecordStatus(ValidationType.Delete).FirstChar();

                userRoleInfos = userDetails.Where(x => x.User.RecordStatus == recordStatus &&
                                                        x.CompanyRoles != null &&
                                                        x.CompanyRoles.Count > 0 &&
                                                        x.CompanyRoles.Any(x1 => x1.Roles != null && x1.Roles.Count > 0))
                                            .SelectMany(x2 => x2.CompanyRoles.SelectMany(x3 => x3.Roles.Where(x4 => x4.RecordStatus == newStatus ||
                                                                                                                    x4.RecordStatus == deleteStatus)
                                                                                                        .Select(x5 => new UserRoleInfo()
                                                                                                        {
                                                                                                            UserLogonName = x2.User.LogonName,
                                                                                                            ApplicationName = x2.User.ApplicationName,
                                                                                                            CompanyCode = x3.CompanyCode,
                                                                                                            RoleName = x5.RoleName,
                                                                                                            RecordStatus = x5.RecordStatus,
                                                                                                            ActionByUser = x5.ActionByUser // Manage Security Audit changes
                                                                                                        }))).ToList();

                IList<UserRoleInfo> invalidUserRoles = null;
                var response = _userRoleService.IsUserRoleExistInDb(userRoleInfos, ref userRoleNotExists, ref dbUserRoles);
                if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                {
                    if (validationType == ValidationType.Add)
                    {
                        invalidUserRoles = userRoleInfos;
                    }
                }
                else
                {
                    if (validationType == ValidationType.Add)
                    {
                        var userRoleNotFound = userRoleNotExists;
                        invalidUserRoles = userRoleInfos.Where(x => !userRoleNotFound.Any(x1 => x1.ApplicationName == x.ApplicationName &&
                                                                                                x1.UserLogonName == x.UserLogonName &&
                                                                                                x1.RoleName == x.RoleName &&
                                                                                                x1.CompanyCode == x.CompanyCode)).ToList();
                    }
                    else //Delete & Modify
                    {
                        //bool valdMsg =
                        invalidUserRoles = null;
                        messages = response.ValidationMessages
                                           .Where(x =>
                                           {
                                               var valdMsg = ((UserRoleInfo)x.EntityValue);
                                               return valdMsg.RecordStatus.IsRecordStatusDeleted() ||
                                                      valdMsg.RecordStatus.IsRecordStatusModified();
                                           }).ToList();
                    }
                }

                invalidUserRoles?.ToList()?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.UserRoleAlreadyExist, x.RoleName, x.UserLogonName);
                });
            }
            return new Response().ToPopulate(ResponseType.Success, messages, messages.Count <= 0);
        }

        private Response DeleteUserRole(IList<UserRoleInfo> userRoleInfos,
                                        ref IList<DbModel.User> dbUsers,
                                        ref IList<DbModel.UserRole> dbUserRoles,
                                        ref IList<DbModel.Role> dbRoles,
                                        ref IList<DbModel.Application> dbApplications,
                                        ref IList<DbModel.Company> dbCompany,
                                        ref long? eventId) // Manage Security Audit changes
        {
            if (userRoleInfos?.Count > 0 && dbUserRoles?.Count > 0)
            {
                IList<DbModel.UserRole> dbUSerRole = null;
                return _userRoleService.Delete(userRoleInfos,
                                                    ref dbUSerRole,
                                                    ref dbApplications,
                                                    ref dbUsers,
                                                    ref dbRoles,
                                                    ref dbCompany,
                                                    ref eventId); // Manage Security Audit changes
            }
            else
            {
                return new Response().ToPopulate(ResponseType.Success);
            }
        }

        private IList<RoleDetail> GetRoleDetail(string compCode, IList<UserDetail> userDetails)
        {
            IList<string> roleNames = userDetails?.Where(x => x.CompanyRoles != null)
                                                  .SelectMany(x => x.CompanyRoles)
                                                  .Where(x => x.CompanyCode == compCode && x.Roles != null)
                                                  .SelectMany(x => x.Roles.Select(x1 => x1.RoleName))
                                                  .ToList();
            if (roleNames?.Count > 0)
            {
                var response = _roleDetailService.Get(roleNames);
                if (response.Code == ResponseType.Success.ToId() && response.Result != null)
                {
                    return response.Result.Populate<IList<RoleDetail>>();
                }
            }

            return new List<RoleDetail>();
        }

        private IList<RoleModuleActivity> ExtractModuleActivity(IList<RoleDetail> roleDetails)
        {
            return roleDetails?
                    .SelectMany(x => x.Modules)
                    .GroupBy(x => x.Module.ModuleName)
                    .Select(x => new RoleModuleActivity()
                    {
                        Module = x.FirstOrDefault().Module,
                        Activities = x.SelectMany(x1 => x1.Activities).ToList()
                    })
                    .ToList();
        }

        private IList<UserMenuPermissionInfo> PopulatMenuPermission(IList<RoleModuleActivity> moduleActivities,
                                                                    IList<ApplicationMenuInfo> applicationMenuList)
        {
            var activities = moduleActivities?.SelectMany(x => x.Activities.Select(x1 => new
            {
                x.Module.ModuleName,
                x1.ActivityName,
                x1.ActivityCode
            }));

            return applicationMenuList?
                            .Where(x => x.IsActive == true)
                            .Select(appMenu => new UserMenuPermissionInfo()
                            {
                                Module = appMenu.ModuleName,
                                MenuName = appMenu.MenuName,
                                IsVisible = appMenu.ActivityCodes.Split('-').ToList()
                                                    .Any(actCode => activities == null ?
                                                                    false :
                                                                    activities.Where(x2 => x2.ModuleName.Trim() == appMenu.ModuleName.Trim())
                                                                              .Select(x4 => x4.ActivityCode)
                                                                              .ToList()
                                                                              .Contains(actCode))
                            })
                            .Where(x => x.IsVisible == true)
                            .OrderBy(x => x.MenuName).ToList();
        }

        private Response PopulateUserPermission(string applicationName, string userLogonName, string companyCode, string moduleName)
        {
            IList<UserPermissionInfo> result = null;
            Exception exception = null;
            try
            {
                //var cacheKey = userLogonName + companyCode + moduleName;
                //if (!_memoryCache.TryGetValue(cacheKey, out result))
                //{
                IList<DbModel.UserRole> dbUserRole = null;
                var searchModel = new UserRoleInfo()
                {
                    ApplicationName = applicationName,
                    UserLogonName = userLogonName,
                    CompanyCode = companyCode
                };
                string[] includes = new string[] { "Role.RoleActivity", "Role.RoleActivity.Module", "Role.RoleActivity.Activity" };

                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    dbUserRole = _userRoleService.Get(searchModel, includes).Result.Populate<IList<DbModel.UserRole>>();
                    tranScope.Complete();
                }

                if (dbUserRole?.Count > 0)
                {

                    var roleActivities = dbUserRole.SelectMany(x1 => x1.Role
                                                                        .RoleActivity
                                                                        .Select(x2 => new UserPermissionInfo()
                                                                        {
                                                                            Module = x2.Module.Name,
                                                                            Activity = x2.Activity.Code,
                                                                            HasPermission = true
                                                                        })).ToList();
                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        roleActivities = roleActivities?.Where(x => x.Module == moduleName).ToList();
                    }

                    var groupedBy = roleActivities.GroupBy(x => new { x.Module, x.Activity });

                    result = groupedBy.Select(x => new UserPermissionInfo()
                    {
                        Module = x.Key.Module,
                        Activity = x.Key.Activity,
                        HasPermission = true
                    }).ToList();

                    // _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    // }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { UserName = userLogonName, CompanyCode = companyCode });
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        private Response PopulateUserPermission(int companyId, string userLogonName, string moduleName)
        {
            IList<UserPermissionInfo> result = null;
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                result = _repository.UserPermission(companyId, userLogonName, moduleName);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), string.Format("{0}", "{1}", "{2}", companyId, userLogonName, moduleName));
            }

            return new Response().ToPopulate(responseType, null, null, null, result, exception, result?.Count);
        }

        private void PopulateDefaultRoleForUser(ref IList<UserDetail> userDetails)
        {
            if (userDetails?.Count > 0)
            {
                foreach (var userDetail in userDetails)
                {
                    if (userDetail.CompanyRoles == null && userDetail.CompanyUserTypes != null && userDetail.User != null && !String.IsNullOrEmpty(userDetail.User.CompanyCode))
                    {
                        var compUserTypes = userDetail?.CompanyUserTypes?.FirstOrDefault(x => x.CompanyCode == userDetail.User.CompanyCode)?.UserTypes?.Select(x => x.UserType).ToList();
                        var defaultRoleName = GetUserDefaultRole(compUserTypes);
                        if (!string.IsNullOrEmpty(defaultRoleName))
                        {
                            var companyRole = new CompanyRole()
                            {
                                CompanyCode = userDetail.User.CompanyCode,
                                Roles = new List<RoleInfo>()
                                {
                                    new RoleInfo()
                                    {
                                        ApplicationName=userDetail.User.ApplicationName,
                                        RoleName=defaultRoleName,
                                        RecordStatus=RecordStatus.New.FirstChar()
                                    }
                                }
                            };
                            userDetail.CompanyRoles = new List<CompanyRole>() { companyRole };
                        }
                    }
                }
            }
        }

        private string GetUserDefaultRole(IList<string> userTypes)
        {
            string result = string.Empty;
            if (userTypes?.Count > 0)
            {
                if (userTypes.Contains(UserType.TechnicalSpecialist.ToString()))
                {
                    result = "TechnicalSpecialist";
                }
                else if (userTypes.Contains(UserType.Customer.ToString()))
                {
                    result = "Customer";
                }
            }
            return result;
        }


        private Response IsValidUserType(ValidationType validationType,
                                    IList<UserDetail> userDetails,
                                    ref IList<UserTypeInfo> userTypeInfos,
                                    ref IList<UserTypeInfo> userTypeNotExists,
                                    ref IList<DbModel.UserType> dbUserTypes)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (userDetails?.Count > 0)
            {
                var recordStatus = Utility.GetRecordStatus(validationType).FirstChar();
                var newStatus = Utility.GetRecordStatus(ValidationType.Add).FirstChar();
                var deleteStatus = Utility.GetRecordStatus(ValidationType.Delete).FirstChar();
                var modifyStatus = Utility.GetRecordStatus(ValidationType.Update).FirstChar();

                userTypeInfos = userDetails.Where(x => x.User.RecordStatus == recordStatus &&
                                                        x.CompanyUserTypes != null &&
                                                        x.CompanyUserTypes.Count > 0 &&
                                                        x.CompanyUserTypes.Any(x1 => x1.UserTypes != null && x1.UserTypes.Count > 0))
                                            .SelectMany(x2 => x2.CompanyUserTypes.SelectMany(x3 => x3.UserTypes.Where(x4 => x4.RecordStatus == newStatus ||
                                                                                                                    x4.RecordStatus == deleteStatus ||
                                                                                                                    x4.RecordStatus == modifyStatus)
                                                                                                        .Select(x5 => new UserTypeInfo()
                                                                                                        {
                                                                                                            UserLogonName = x2.User.LogonName,
                                                                                                            CompanyCode = x3.CompanyCode,
                                                                                                            UserType = x5.UserType,
                                                                                                            IsActive = x5.IsActive,
                                                                                                            RecordStatus = x5.RecordStatus,
                                                                                                            ActionByUser = x5.ActionByUser // Manage Security Audit changes
                                                                                                        }))).ToList();

                IList<UserTypeInfo> invalidUserTypes = null;
                var response = _userTypeService.IsUserTypeExistInDb(userTypeInfos, ref userTypeNotExists, ref dbUserTypes);
                if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                {
                    if (validationType == ValidationType.Add)
                    {
                        invalidUserTypes = userTypeInfos;
                    }
                    if (validationType == ValidationType.Update)
                    {
                        return _userTypeService.IsRecordUpdateCountMatching(userTypeInfos, dbUserTypes, ref messages);

                    }
                }
                else
                {
                    if (validationType == ValidationType.Add)
                    {
                        var userTypeNotFound = userTypeNotExists;
                        invalidUserTypes = userTypeInfos.Where(x => !userTypeNotFound.Any(x1 => x1.UserLogonName == x.UserLogonName &&
                                                                                                x1.UserType == x.UserType &&
                                                                                                x1.CompanyCode == x.CompanyCode)).ToList();
                    }
                    else //Delete & Modify
                    {
                        invalidUserTypes = null;
                        messages = response.ValidationMessages
                                           .Where(x =>
                                           {
                                               var valdMsg = ((UserTypeInfo)x.EntityValue);
                                               return valdMsg.RecordStatus.IsRecordStatusDeleted() ||
                                                      valdMsg.RecordStatus.IsRecordStatusModified();
                                           }).ToList();
                    }
                }

                invalidUserTypes?.ToList()?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.UserTypeAlreadyExist, x.UserType, x.UserLogonName);
                });
            }
            return new Response().ToPopulate(ResponseType.Success, messages, messages.Count <= 0);
        }

        private Response AddUserType(IList<UserTypeInfo> userTyepInfos,
                                   IList<DbModel.User> dbUsers,
                                   IList<DbModel.Company> dbCompany,
                                   ref long? eventId) // Manage Security Audit changes
        {
            if (userTyepInfos?.Count > 0 && dbUsers?.Count > 0 && dbCompany?.Count > 0)
            {
                IList<DbModel.UserType> dbUserType = null;
                return _userTypeService.Add(userTyepInfos,
                                                 ref dbUserType,
                                                 ref dbUsers,
                                                 ref dbCompany,
                                                 ref eventId); // Manage Security Audit changes
            }
            else
            {
                return new Response().ToPopulate(ResponseType.Success, true);
            }
        }

        private Response DeleteUserType(IList<UserTypeInfo> userTypeInfos,
                                ref IList<DbModel.User> dbUsers,
                                ref IList<DbModel.UserType> dbUserTypes,
                                ref IList<DbModel.Company> dbCompany,
                                ref long? eventId) // Manage Security Audit changes
        {
            if (userTypeInfos?.Count > 0 && dbUserTypes?.Count > 0)
            {
                // IList<DbModel.UserType> dbUserType = null;
                return _userTypeService.Delete(userTypeInfos,
                                                    ref dbUserTypes,
                                                    ref dbUsers,
                                                    ref dbCompany,
                                                    ref eventId); // Manage Security Audit changes
            }
            else
            {
                return new Response().ToPopulate(ResponseType.Success);
            }
        }

        private Response IsDefaultCompanyRoleAndUsertypeAssigned(IList<UserDetail> userDetails, ValidationType validationType)
        {
            var messages = new List<ValidationMessage>();
            IList<UserTypeInfo> userTypeInfos = null;
            IList<UserRoleInfo> userRoleInfos = null;
            if (userDetails?.Count > 0 && (validationType == ValidationType.Add || validationType == ValidationType.Update))
            {
                var recordStatus = Utility.GetRecordStatus(validationType).FirstChar();
                var newStatus = Utility.GetRecordStatus(ValidationType.Add).FirstChar();
                var deleteStatus = Utility.GetRecordStatus(ValidationType.Delete).FirstChar();
                var modifyStatus = Utility.GetRecordStatus(ValidationType.Update).FirstChar();

                userTypeInfos = userDetails.Where(x => x.User.RecordStatus == recordStatus &&
                                                        x.CompanyUserTypes != null &&
                                                        x.CompanyUserTypes.Count > 0 &&
                                                        x.CompanyUserTypes.Any(x1 => x1.UserTypes != null && x1.UserTypes.Count > 0))
                                                .SelectMany(x2 => x2.CompanyUserTypes.SelectMany(x3 => x3.UserTypes.Where(x4 => x4.RecordStatus == newStatus || x4.RecordStatus == modifyStatus || x4.RecordStatus == null)
                                                                            .Select(x5 => new UserTypeInfo()
                                                                            {
                                                                                UserLogonName = x2.User.LogonName,
                                                                                CompanyCode = x3.CompanyCode,
                                                                                UserType = x5.UserType,
                                                                                RecordStatus = x5.RecordStatus
                                                                            }))).ToList();

                var usrWithoutDefaultCompUserType = userDetails.Where(x => x.User.RecordStatus == recordStatus)
                                                    .GroupJoin(
                                                    userTypeInfos,
                                                    usr => usr.User.CompanyCode,
                                                    usrTyp => usrTyp.CompanyCode,
                                                    (usr, usrTyp) => new { usr, usrTyp })
                                                    .Where(x => x?.usrTyp?.Count() <= 0).Select(x => x.usr.User);
                //Commented for D669(as per mail conversation b/w francina & sumit)
                //usrWithoutDefaultCompUserType?.ToList()?.ForEach(x =>
                //{
                //    messages.Add(_messages, x, MessageType.UserTypeNotAssignedForDefaultCompany, x.CompanyName, x.UserName);
                //});

                //Check if additional company has role assigned
                var roles = userDetails.SelectMany(x => x.CompanyRoles).Where(x => x.Roles?.Count == 0).ToList();
                //roles?.ToList()?.ForEach(x =>
                //{
                //    messages.Add(_messages, x, MessageType.UserTypeNotAssignedForAdditionalCompany);
                //});

                userRoleInfos = userDetails.Where(x => x.User.RecordStatus == recordStatus &&
                                                        x.CompanyRoles != null &&
                                                        x.CompanyRoles.Count > 0 &&
                                                        x.CompanyRoles.Any(x1 => x1.Roles != null && x1.Roles.Count > 0))
                                            .SelectMany(x2 => x2.CompanyRoles.SelectMany(x3 => x3.Roles.Where(x4 => x4.RecordStatus == newStatus || x4.RecordStatus == null)
                                                                                                        .Select(x5 => new UserRoleInfo()
                                                                                                        {
                                                                                                            UserLogonName = x2.User.LogonName,
                                                                                                            ApplicationName = x2.User.ApplicationName,
                                                                                                            CompanyCode = x3.CompanyCode,
                                                                                                            RoleName = x5.RoleName,
                                                                                                            RecordStatus = x5.RecordStatus
                                                                                                        }))).ToList();

                var usrWithoutDefaultCompUserRole = userDetails.Where(x => x.User.RecordStatus == recordStatus)
                                                              .GroupJoin(
                                                              userRoleInfos,
                                                              usr => usr.User.CompanyCode,
                                                              usrRl => usrRl.CompanyCode,
                                                              (usr, usrRl) => new { usr, usrRl })
                                                              .Where(x => x?.usrRl?.Count() <= 0).Select(x => x.usr.User);

                //Commented for D669(as per mail conversation b/w francina & sumit)
                //if (validationType == ValidationType.Add)
                //usrWithoutDefaultCompUserRole?.ToList()?.ForEach(x =>
                //{
                //    messages.Add(_messages, x, MessageType.UserRoleNotAssignedForDefaultCompany, x.CompanyName, x.UserName);
                //});

                //if (validationType == ValidationType.Update)
                //    if (usrWithoutDefaultCompUserRole?.ToList()?.Count > 0 || roles?.Count > 0)
                //        messages.Add(_messages, null, MessageType.UserTypeNotAssignedForAdditionalCompany);

            }
            return new Response().ToPopulate(ResponseType.Success, messages, messages.Count <= 0);
        }

        // Manage Security Audit changes
        public void AppendUserRoleEvent(ref IList<UserRoleInfo> userRoleInfo, long? eventId)
        {
            ObjectExtension.SetPropertyValue(userRoleInfo, "EventId", eventId);
        }

        // Manage Security Audit changes
        public void AppendUserTypeEvent(ref IList<UserTypeInfo> userTypeInfo, long? eventId)
        {
            ObjectExtension.SetPropertyValue(userTypeInfo, "EventId", eventId);
        }

        #endregion
    }
}