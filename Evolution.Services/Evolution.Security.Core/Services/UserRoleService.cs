using AutoMapper;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Interfaces.Validations;
using DomainModel = Evolution.Security.Domain.Models.Security;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Enums;
using System.Linq.Expressions;
using Evolution.Common.Extensions;
using System.Linq;
using Evolution.Common.Models.Messages;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Enums;
using System.Transactions;

namespace Evolution.Security.Core.Services
{
    //TEST GKUMAR
    public class UserRoleService : IUserRoleService
    {
        private readonly IAppLogger<UserRoleService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IUserRoleRepository _repository = null;
        private readonly IApplicationRepository _applicationRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IUserService _userService = null;
        private readonly IRoleService _roleService = null;
        private readonly IUserRoleValidationService _validationService = null;
        private readonly ICompanyService _companyService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor
        public UserRoleService(IMapper mapper,
                                IUserRoleRepository repository,
                                IApplicationRepository applicationRepository,
                                IAppLogger<UserRoleService> logger,
                                IUserService userService,
                                IRoleService roleService,
                                ICompanyService companyService,
                                IUserRoleValidationService validationService,
                                JObject messgaes,
                                IAuditSearchService auditSearchService)
        {
            this._repository = repository;
            this._applicationRepository = applicationRepository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._companyService = companyService;
            this._userService = userService;
            this._roleService = roleService;
            this._messageDescriptions = messgaes;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(DomainModel.UserRoleInfo searchModel)
        {
            IList<DomainModel.UserRoleInfo> result = null;
            Exception exception = null;
            try
            {
                Expression<Func<DbModel.UserRole, bool>> predicate = null;

                if (!string.IsNullOrEmpty(searchModel.UserLogonName))
                    predicate = x => x.User.SamaccountName == searchModel.UserLogonName;

                if (!string.IsNullOrEmpty(searchModel.RoleName))
                    predicate = predicate.And(x => x.Role.Name == searchModel.RoleName);

                if (!string.IsNullOrEmpty(searchModel.CompanyCode))
                    predicate = predicate.And(x => x.Company.Code == searchModel.CompanyCode);

                result = _mapper.Map<IList<DomainModel.UserRoleInfo>>(this._repository.FindBy(predicate, x => x.Company, x1 => x1.User, x2 => x2.Role).ToList());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> userIds)
        {
            IList<DomainModel.UserRoleInfo> result = null;
            Exception exception = null;
            try
            {
                if (userIds?.Count > 0)
                    result = _mapper.Map<IList<DomainModel.UserRoleInfo>>(this._repository.FindBy(x => userIds.Contains(x.UserId)).ToList());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(DomainModel.UserRoleInfo searchModel, params string[] includes)
        {
            IList<DbModel.UserRole> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.Get(searchModel, includes).ToList();
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

        public Response IsUserRoleExistInDb(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                            ref IList<DomainModel.UserRoleInfo> userRoleNotExists,
                                            ref IList<DbModel.UserRole> dbUserRoles)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                result = IsUserRoleExistInDb(userRoleInfos, ref userRoleNotExists, ref dbUserRoles, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userRoleInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<DomainModel.UserRoleInfo> userRoleInfos,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.UserRole> dbUserRoles = null;
            IList<DbModel.Role> dbRoles = null;
            IList<DbModel.Company> dbCompany = null;
            IList<DbModel.Application> dbApplications = null;

            return AddUserRole(userRoleInfos, ref dbUserRoles, ref dbApplications, ref dbUsers, ref dbRoles, ref dbCompany, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.UserRoleInfo> userRoleInfos,
                            ref IList<DbModel.UserRole> dbUserRoles,
                            ref IList<DbModel.Application> dbApplications,
                            ref IList<DbModel.User> dbUsers,
                            ref IList<DbModel.Role> dbRoles,
                            ref IList<DbModel.Company> dbCompany,
                            ref long? eventId, // Manage Security Audit changes
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddUserRole(userRoleInfos, ref dbUserRoles, ref dbApplications, ref dbUsers, ref dbRoles, ref dbCompany, ref eventId, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify

        //public Response Modify(IList<DbModel.UserRole> dbUserRoles, string modifiedBy)
        //{
        //    Exception exception = null;
        //    try
        //    {
        //        dbUserRoles?.ToList().ForEach(dbUserRole =>
        //        {
        //            dbUserRole.IsAllowedDuringLock = true;
        //            dbUserRole.LastModification = DateTime.UtcNow;
        //            dbUserRole.UpdateCount = dbUserRole.UpdateCount.CalculateUpdateCount();
        //            dbUserRole.ModifiedBy = modifiedBy;
        //        });
        //        _repository.AutoSave = false;
        //        _repository.Update(dbUserRoles);
        //        _repository.ForceSave();
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbUserRoles?.ToList());
        //    }
        //    finally
        //    {
        //        _repository.AutoSave = true;
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        //}

        #endregion

        #region Delete
        public Response Delete(IList<DomainModel.UserRoleInfo> userRoleInfo,
                                ref long? eventId, // Manage Security Audit changes
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.UserRole> dbUserRoles = null;
            IList<DbModel.Application> dbApplications = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Role> dbRoles = null;
            IList<DbModel.Company> dbCompany = null;

            return this.RemoveUserRole(userRoleInfo,
                                        ref dbApplications,
                                        ref dbUserRoles,
                                        ref dbUsers,
                                        ref dbRoles,
                                        ref dbCompany,
                                        ref eventId, // Manage Security Audit changes
                                        commitChange,
                                        isDbValidationRequire);
        }

        public Response Delete(IList<DomainModel.UserRoleInfo> userRoleInfo,
                                ref IList<DbModel.UserRole> dbUserRoles,
                                ref IList<DbModel.Application> dbApplications,
                                ref IList<DbModel.User> dbUsers,
                                ref IList<DbModel.Role> dbRoles,
                                ref IList<DbModel.Company> dbCompany,
                                ref long? eventId, // Manage Security Audit changes
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            return this.RemoveUserRole(userRoleInfo,
                                        ref dbApplications,
                                        ref dbUserRoles,
                                        ref dbUsers,
                                        ref dbRoles,
                                        ref dbCompany,
                                        ref eventId, // Manage Security Audit changes
                                        commitChange,
                                        isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check    
        public Response IsRecordValidForProcess(IList<DomainModel.UserRoleInfo> userRoleInfos, ValidationType validationType)
        {
            IList<DomainModel.UserRoleInfo> filteredUsers = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.UserRole> dbUserRoles = null;
            IList<DbModel.Role> dbRoles = null;
            IList<DbModel.Company> dbCompany = null;
            IList<DbModel.Application> dbApplications = null;
            return this.CheckRecordValidForProcess(userRoleInfos, validationType, ref filteredUsers, ref dbApplications, ref dbUserRoles, ref dbUsers, ref dbRoles, ref dbCompany);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.UserRole> dbUserRoles,
                                                ref IList<DbModel.Application> dbApplications,
                                                ref IList<DbModel.User> dbUsers,
                                                ref IList<DbModel.Role> dbRoles,
                                                ref IList<DbModel.Company> dbCompany)
        {
            IList<DomainModel.UserRoleInfo> filteredUsers = null;
            return this.CheckRecordValidForProcess(userRoleInfos, validationType, ref filteredUsers, ref dbApplications, ref dbUserRoles, ref dbUsers, ref dbRoles, ref dbCompany);
        }
        #endregion

        #endregion

        #region Private Metods

        #region Record Exists
        private bool IsUserRoleExistInDb(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                            ref IList<DomainModel.UserRoleInfo> userRoleNotExists,
                                            ref IList<DbModel.UserRole> dbUserRoles,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            if (userRoleInfos?.Count > 0)
            {
                var userLogonNames = userRoleInfos.Select(x => x.UserLogonName).Distinct().ToList();
                var userRoleNames = userRoleInfos.Select(x => x.RoleName).Distinct().ToList();
                var compCodes = userRoleInfos.Select(x => x.CompanyCode).Distinct().ToList();
                if (dbUserRoles == null || dbUserRoles.Count <= 0)
                    dbUserRoles = _repository.FindBy(x => userLogonNames.Contains(x.User.SamaccountName) &&
                                                          userRoleNames.Contains(x.Role.Name) &&
                                                          compCodes.Contains(x.Company.Code))
                                             .Include(x => x.Company)
                                             .Include(x => x.User)
                                             .Include(x => x.Role)
                                             .ToList();

                var dbURoles = dbUserRoles;
                userRoleNotExists = userRoleInfos.Where(x => !dbURoles.Any(x1 => x1.Company.Code == x.CompanyCode &&
                                                                                    x1.User.SamaccountName == x.UserLogonName &&
                                                                                    x1.Role.Name == x.RoleName))
                                                 .Select(x => x).ToList();

                userRoleNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.UserRoleNotExist, x.UserLogonName, x.RoleName, x.CompanyCode);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                         ref IList<DomainModel.UserRoleInfo> filteredUserRoles,
                                         ref IList<DbModel.Application> dbApplications,
                                         ref IList<DbModel.UserRole> dbUserRoles,
                                         ref IList<DbModel.User> dbUsers,
                                         ref IList<DbModel.Role> dbRoles,
                                         ref IList<DbModel.Company> dbCompany,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (userRoleInfos != null && userRoleInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUserRoles == null || filteredUserRoles.Count <= 0)
                    filteredUserRoles = FilterRecord(userRoleInfos, validationType);

                if (filteredUserRoles?.Count > 0 && IsValidPayload(filteredUserRoles, validationType, ref validationMessages))
                {
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    var valdResponse = this.IsMasterDataValid(filteredUserRoles, ref dbApplications, ref dbUsers, ref dbRoles, ref dbCompany, ref validationMessages);
                    if (valdResponse.Code == ResponseType.Success.ToId())
                    {
                        IList<DomainModel.UserRoleInfo> userRoleNotExists = null;
                        var response = (!IsUserRoleExistInDb(filteredUserRoles, ref userRoleNotExists, ref dbUserRoles, ref messages) && messages.Count == filteredUserRoles.Count);
                        if (!response)
                        {
                            var userRoleAlreadyExists = filteredUserRoles.Where(x => !userRoleNotExists.Any(x1 => x1.CompanyCode == x.CompanyCode && x1.UserLogonName == x.UserLogonName && x1.RoleName == x.RoleName)).ToList();
                            userRoleAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messageDescriptions, x, MessageType.UserRoleAlreadyExist, x.RoleName, x.UserLogonName);
                            });

                            if (messages.Count > 0)
                            {
                                result = false;
                                validationMessages.AddRange(messages);
                            }
                        }
                    }
                    else
                    {
                        validationMessages = valdResponse.ValidationMessages;
                        result = false;
                    }
                }
                else
                    result = false;
            }

            return result;
        }

        private Response AddUserRole(IList<DomainModel.UserRoleInfo> userRoleInfo,
                                        ref IList<DbModel.UserRole> dbUserRoles,
                                        ref IList<DbModel.Application> dbApplications,
                                        ref IList<DbModel.User> dbUsers,
                                        ref IList<DbModel.Role> dbRoles,
                                        ref IList<DbModel.Company> dbCompany,
                                        ref long? eventId,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                Response valdResponse = null;
                IList<DomainModel.UserRoleInfo> recordToBeAdd = null;
                eventID = userRoleInfo?.FirstOrDefault()?.EventId; // Manage Security Audit changes
                if (userRoleInfo?.Count > 0)
                {
                    if (isDbValidationRequire)
                        valdResponse = CheckRecordValidForProcess(userRoleInfo, ValidationType.Add, ref recordToBeAdd, ref dbApplications, ref dbUserRoles, ref dbUsers, ref dbRoles, ref dbCompany);

                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                    {
                        IList<DbModel.User> dbUser = dbUsers;
                        IList<DbModel.Role> dbRole = dbRoles;
                        IList<DbModel.Company> dbComps = dbCompany;
                        IList<DbModel.Application> dbApps = dbApplications;
                        var mappedRecordToBeAdd = _mapper.Map<IList<DbModel.UserRole>>(recordToBeAdd, opt =>
                        {
                            opt.Items["dbApplication"] = dbApps;
                            opt.Items["dbUser"] = dbUser;
                            opt.Items["dbRole"] = dbRole;
                            opt.Items["dbCompany"] = dbComps;
                        });
                        mappedRecordToBeAdd = mappedRecordToBeAdd
                                              .Select(x =>
                                              {
                                                  x.Id = 0;
                                                  x.LastModification = null;
                                                  x.UpdateCount = null;
                                                  x.ModifiedBy = null;
                                                  return x;
                                              }).ToList();
                        //FInding distinct record.
                        mappedRecordToBeAdd = mappedRecordToBeAdd.GroupBy(x => new { x.ApplicationId, x.CompanyId, x.UserId, x.RoleId })
                                                                 .Select(grp => grp.FirstOrDefault()).ToList();
                        _repository.AutoSave = false;
                        _repository.Add(mappedRecordToBeAdd);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            dbUserRoles = mappedRecordToBeAdd;

                            // Manage Secuirty Audit changes - starts.

                            if(value > 0 && mappedRecordToBeAdd?.Count > 0 && recordToBeAdd?.Count > 0)
                            {
                                /* Doubt: In contract exchange rate - they are using dbRecord for loop and sending recordsToBe add in audit log first and third param. but in contract rate schedule it is different. why?
                                 * Need to test: On user role add how it is going to work?
                                 */
                                mappedRecordToBeAdd?.ToList().ForEach(x => _auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventID, recordToBeAdd.FirstOrDefault()?.ActionByUser,
                                                                                                        null,
                                                                                                        ValidationType.Add.ToAuditActionType(),
                                                                                                        SqlAuditModuleType.UserUserRole,
                                                                                                        null,
                                                                                                        _mapper.Map<DomainModel.UserRoleInfo>(x)));
                                eventId = eventID;
                            }
                            // Manage Security Audit chages - ends.
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userRoleInfo);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                            ref IList<DomainModel.UserRoleInfo> filteredUserRoles,
                                            ref IList<DbModel.Application> dbApplications,
                                            ref IList<DbModel.UserRole> dbUserRoles,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Role> dbRoles,
                                            ref IList<DbModel.Company> dbCompany,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (userRoleInfos != null && userRoleInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUserRoles == null || filteredUserRoles.Count <= 0)
                    filteredUserRoles = FilterRecord(userRoleInfos, validationType);

                if (filteredUserRoles?.Count > 0 && IsValidPayload(filteredUserRoles, validationType, ref validationMessages))
                {
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    var valdResponse = this.IsMasterDataValid(filteredUserRoles, ref dbApplications, ref dbUsers, ref dbRoles, ref dbCompany, ref validationMessages);
                    if (valdResponse.Code == ResponseType.Success.ToId())
                    {
                        IList<DomainModel.UserRoleInfo> userRoleNotExists = null;
                        result = IsUserRoleExistInDb(filteredUserRoles, ref userRoleNotExists, ref dbUserRoles, ref validationMessages);
                    }
                    else
                    {
                        validationMessages = valdResponse.ValidationMessages;
                        result = false;
                    }
                }
                else
                    result = false;
            }

            return result;
        }


        private Response RemoveUserRole(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                        ref IList<DbModel.Application> dbApplications,
                                        ref IList<DbModel.UserRole> dbUserRoles,
                                        ref IList<DbModel.User> dbUsers,
                                        ref IList<DbModel.Role> dbRoles,
                                        ref IList<DbModel.Company> dbCompany,
                                        ref long? eventId, // Manage Security Audit changes
                                        bool commitChange,
                                        bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.UserRoleInfo> filteredUserRoles = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventID = userRoleInfos?.FirstOrDefault()?.EventId; // Manage Security Audit changes

                Response response = null;
                if (isDbValidationRequire)
                    response = CheckRecordValidForProcess(userRoleInfos,
                                                          ValidationType.Delete,
                                                          ref filteredUserRoles,
                                                          ref dbApplications,
                                                          ref dbUserRoles,
                                                          ref dbUsers,
                                                          ref dbRoles,
                                                          ref dbCompany);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result)))
                {
                    _repository.AutoSave = false;
                    dbUserRoles = dbUserRoles.GroupBy(x => new { x.ApplicationId, x.CompanyId, x.UserId, x.RoleId })
                                             .Select(grp => grp.FirstOrDefault()).ToList();
                    _repository.Delete(dbUserRoles);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        // Manage Security Audit changes - starts
                        if(value > 0 && dbUserRoles?.Count > 0 && filteredUserRoles?.Count > 0)
                        {
                            filteredUserRoles?.ToList().ForEach(x => _auditSearchService.AuditLog(x, ref eventID, x.ActionByUser,
                                                                                                  null,
                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.UserUserRole,
                                                                                                  x,
                                                                                                  null));
                            eventId = eventID;
                        }
                        // Manage Security Audit changes - ends
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userRoleInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        #endregion

        #region Common
        private Response IsMasterDataValid(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                            ref IList<DbModel.Application> dbApplications,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Role> dbRoles,
                                            ref IList<DbModel.Company> dbCompany,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Response result = new Response().ToPopulate(ResponseType.Success);
            if (userRoleInfos?.Count() > 0)
            {
                IList<string> userNotExists = null;
                IList<string> roleNotExists = null;

                var appNames = userRoleInfos.Select(x => x.ApplicationName).ToList();
                var appUserLogonNames = userRoleInfos.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.UserLogonName)).ToList();
                var appRoleNames = userRoleInfos.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.RoleName)).ToList();
                var compCodes = userRoleInfos.Select(x => x.CompanyCode).ToList();

                result = this._userService.IsRecordExistInDb(appUserLogonNames, ref dbUsers, ref userNotExists);
                if (result.Code == ResponseType.Success.ToId() && Convert.ToBoolean(result.Result) == true)
                {
                    result = this._roleService.IsRecordExistInDb(appRoleNames, ref dbRoles, ref roleNotExists);
                    if (result.Code == ResponseType.Success.ToId() && Convert.ToBoolean(result.Result) == true)
                    {
                        if (!this._companyService.IsValidCompany(compCodes, ref dbCompany, ref validationMessages))
                            result = result.ToPopulate(ResponseType.Validation, validationMessages, null);
                        else if (!this.IsApplicationExistInDb(appNames, ref dbApplications, ref validationMessages))
                            result = result.ToPopulate(ResponseType.Validation, validationMessages, null);
                    }
                }
            }
            return result;
        }

        private IList<DomainModel.UserRoleInfo> FilterRecord(IList<DomainModel.UserRoleInfo> userRoles, ValidationType filterType)
        {
            IList<DomainModel.UserRoleInfo> filteredUserRoles = null;

            if (filterType == ValidationType.Add)
                filteredUserRoles = userRoles?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            //else if (filterType == ValidationType.Update)
            //    filteredUsers = users?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredUserRoles = userRoles?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredUserRoles;
        }

        private Response CheckRecordValidForProcess(IList<DomainModel.UserRoleInfo> userRoleInfo,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.UserRoleInfo> filteredUsers,
                                                    ref IList<DbModel.Application> dbApplications,
                                                    ref IList<DbModel.UserRole> dbUserRoles,
                                                    ref IList<DbModel.User> dbUsers,
                                                    ref IList<DbModel.Role> dbRoles,
                                                    ref IList<DbModel.Company> dbCompany)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(userRoleInfo, ref filteredUsers, ref dbApplications, ref dbUserRoles, ref dbUsers, ref dbRoles, ref dbCompany, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(userRoleInfo, ref filteredUsers, ref dbApplications, ref dbUserRoles, ref dbUsers, ref dbRoles, ref dbCompany, ref validationMessages);
                //else if (validationType == ValidationType.Update)
                //    result = IsRecordValidForUpdate(userRoleInfo, ref filteredUsers, ref dbUsers, ref dbRoles, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userRoleInfo);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<DomainModel.UserRoleInfo> userRoles,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(userRoles), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsApplicationExistInDb(IList<string> appNames,
                                            ref IList<DbModel.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbApplications == null)
                dbApplications = this._applicationRepository.Get(appNames);

            var validMessages = validationMessages;
            var dbApps = dbApplications;

            if (appNames?.Count > 0)
            {
                var appNotExists = appNames.Where(x => !dbApps.Any(x1 => x1.Name == x)).Select(x => x).ToList();
                appNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ApplicationNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
        #endregion

        #endregion
    }
}