using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Interfaces.Validations;
using Evolution.Security.Domain.Models.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IAppLogger<RoleService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IRoleRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IRoleValidationService _validationService = null;
        private readonly IApplicationRepository _applicationRepository = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor
        public RoleService(IMapper mapper,
                             IRoleRepository repository,
                             IApplicationRepository applicationRepository,
                             IAppLogger<RoleService> logger,
                             IRoleValidationService validationService,
                             JObject messgaes,
                             IAuditSearchService auditSearchService)
        {
            this._applicationRepository = applicationRepository;
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messageDescriptions = messgaes;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(RoleInfo searchModel)
        {
            IList<DomainModel.RoleInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.Search(searchModel)?
                                         .AsQueryable()
                                         .ProjectTo<DomainModel.RoleInfo>()
                                         .ToList();
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

        public Response Get(string applicationName, IList<string> roleNames)
        {
            IList<DomainModel.RoleInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this.GetRoleByName(roleNames?.Select(x => new KeyValuePair<string, string>(applicationName, x)).ToList())
                              .AsQueryable()
                              .ProjectTo<DomainModel.RoleInfo>()
                              .ToList();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roleNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> roleIds)
        {
            IList<DomainModel.RoleInfo> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this.GetRoleById(roleIds)
                                    .AsQueryable()
                                    .ProjectTo<DomainModel.RoleInfo>()
                                    .ToList();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roleIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> appRoleNames,
                                          ref IList<DbModel.Role> dbRoles,
                                          ref IList<string> roleNotExists)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (dbRoles == null && appRoleNames?.Count > 0)
                    dbRoles = GetRoleByName(appRoleNames);

                result = IsRoleExistInDb(appRoleNames, dbRoles, ref roleNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), appRoleNames);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<RoleInfo> roles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true) // Manage Security Audit changes
        {
            IList<DbModel.Role> dbRoles = null;
            return AddRole(roles, ref dbRoles, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }

        public Response Add(IList<RoleInfo> roles, ref IList<DbModel.Role> dbRoles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true) // Manage Security Audit changes
        {
            return AddRole(roles, ref dbRoles, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }
        #endregion

        #region Modify
        public Response Modify(IList<RoleInfo> roles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.Role> dbRoles = null;
            return UpdateRoles(roles, ref dbRoles, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<RoleInfo> roles, ref IList<DbModel.Role> dbRoles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return UpdateRoles(roles, ref dbRoles, ref eventId, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete
        public Response Delete(IList<RoleInfo> roleInfo, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return this.RemoveRole(roleInfo, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<RoleInfo> roles, ValidationType validationType)
        {
            IList<DbModel.Role> dbRoles = null;
            return IsRecordValidForProcess(roles, validationType, ref dbRoles);
        }

        public Response IsRecordValidForProcess(IList<RoleInfo> roles, ValidationType validationType, ref IList<DbModel.Role> dbRoles)
        {
            IList<DomainModel.RoleInfo> filteredRoles = null;
            IList<DbModel.Application> dbApplications = null;
            return this.CheckRecordValidForProcess(roles, validationType, ref filteredRoles, ref dbRoles, ref dbApplications);
        }

        public Response IsRecordValidForProcess(IList<RoleInfo> roles, ValidationType validationType, IList<DbModel.Role> dbRoles)
        {
            return IsRecordValidForProcess(roles, validationType, ref dbRoles);
        }
        #endregion

        #endregion

        #region Private Metods
        #region Get
        private IList<DbModel.Role> GetRoleByName(IList<KeyValuePair<string, string>> appRoleNames)
        {
            IList<DbModel.Role> dbRoles = null;
            if (appRoleNames?.Count > 0)
            {
                var appNames = appRoleNames.Select(x => x.Key);
                var roleNames = appRoleNames.Select(x => x.Value);
                dbRoles = _repository.FindBy(x => roleNames.Contains(x.Name) && appNames.Contains(x.Application.Name)).ToList();
            }
            return dbRoles;
        }

        private IList<DbModel.Role> GetRoleById(IList<int> roleIds)
        {
            IList<DbModel.Role> dbRoles = null;
            if (roleIds?.Count > 0)
                dbRoles = _repository.FindBy(x => roleIds.Contains((int)x.Id)).ToList();

            return dbRoles;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.RoleInfo> roles,
                                         ref IList<DomainModel.RoleInfo> filteredRoles,
                                         ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles,
                                         ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (roles != null && roles.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredRoles == null || filteredRoles.Count <= 0)
                    filteredRoles = FilterRecord(roles, validationType);

                if (filteredRoles?.Count > 0 && IsValidPayload(filteredRoles, validationType, ref validationMessages))
                {
                    //IList<int> roleIds = null;
                    IList<string> roleNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    this.GetRoleAndApplicationDbInfo(filteredRoles, false, ref dbRoles, ref dbApplications);
                    var applications = roles.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        var appRoleNames = roles.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.RoleName)).Distinct().ToList();
                        var response = (!IsRoleExistInDb(appRoleNames, dbRoles, ref roleNotExists, ref messages) && messages.Count == appRoleNames.Count);
                        if (!response)
                        {
                            var mdouleAlreadyExists = appRoleNames.Where(x => !roleNotExists.Contains(x.Value)).ToList();
                            mdouleAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messageDescriptions, x, MessageType.RoleAlreadyExist, x);
                            });

                            if (messages.Count > 0)
                            {
                                result = false;
                                validationMessages.AddRange(messages);
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

        private Response AddRole(IList<RoleInfo> roles, ref IList<DbModel.Role> dbRoles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                Response valdResponse = null;
                IList<DbModel.Application> dbApplications = null;
                IList<RoleInfo> recordToBeAdd = null;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(roles, ValidationType.Add, ref recordToBeAdd, ref dbRoles, ref dbApplications);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.RoleId = null; return x; }).ToList();
                    _repository.Add(_mapper.Map<IList<DbModel.Role>>(recordToBeAdd));
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        dbRoles = _repository.Get(recordToBeAdd.Select(x => x.RoleName).ToList());
                        if(value > 0 && recordToBeAdd?.Count > 0)
                        {
                            dbRoles?.ToList().ForEach(x => 
                            recordToBeAdd?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                              "{"+ AuditSelectType.Id + ":"+x.Id + "}${"+ AuditSelectType.Name + ":" + x.Name?.Trim()+"}",
                                                                                              ValidationType.Add.ToAuditActionType(),
                                                                                              SqlAuditModuleType.UserRole,
                                                                                              null,
                                                                                              _mapper.Map<DomainModel.RoleInfo>(x))));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roles);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateRoles(IList<RoleInfo> roles, ref IList<DbModel.Role> dbRoles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                Response valdResponse = null;
                IList<DbModel.Application> dbApplications = null;
                var recordToBeModify = FilterRecord(roles, ValidationType.Update);

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(roles, ValidationType.Update, ref recordToBeModify, ref dbRoles, ref dbApplications);
                else if ((dbRoles == null || dbRoles?.Count <= 0) && recordToBeModify?.Count > 0)
                    dbRoles = _repository.Get(recordToBeModify?.Select(x => (int)x.RoleId).ToList());

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbRoles?.Count > 0))
                {
                    // Manage Security Audit changes - starts
                    IList<RoleInfo> domRoleInfo = new List<RoleInfo>();
                    dbRoles?.ToList().ForEach(x =>
                    {
                        domRoleInfo.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.RoleInfo>(x)));
                    });
                    // Manage Security Audit changes - ends
                    dbRoles.ToList().ForEach(dbRole =>
                    {
                        var roleToBeModify = recordToBeModify.FirstOrDefault(x => x.RoleId == dbRole.Id);
                        dbRole.ApplicationId = dbApplications.FirstOrDefault(x => x.Name == roleToBeModify.ApplicationName).Id;
                        dbRole.Name = roleToBeModify.RoleName;
                        dbRole.Description = roleToBeModify.Description;
                        dbRole.IsAllowedDuringLock = roleToBeModify.IsAllowedDuringLock;
                        dbRole.LastModification = DateTime.UtcNow;
                        dbRole.UpdateCount = roleToBeModify.UpdateCount.CalculateUpdateCount();
                        dbRole.ModifiedBy = roleToBeModify.ModifiedBy;
                    });
                    _repository.AutoSave = false;
                    _repository.Update(dbRoles);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if(value > 0 && dbRoles?.Count > 0 && recordToBeModify?.Count > 0)
                        {
                            dbRoles?.ToList().ForEach(x =>
                            recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                  "{" + AuditSelectType.Id + ":" + x.Id + "}${" + AuditSelectType.Name + ":" + x.Name?.Trim() + "}",
                                                                                                  ValidationType.Update.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.UserRole,
                                                                                                  _mapper.Map<RoleInfo>(domRoleInfo?.FirstOrDefault(x2 => x2.RoleId == x1.RoleId)),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roles);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.RoleInfo> roles,
                                            ref IList<DomainModel.RoleInfo> filteredRoles,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (roles != null && roles.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredRoles == null || filteredRoles.Count <= 0)
                    filteredRoles = FilterRecord(roles, validationType);

                if (filteredRoles?.Count > 0 && IsValidPayload(filteredRoles, validationType, ref messages))
                {
                    this.GetRoleAndApplicationDbInfo(filteredRoles, true, ref dbRoles, ref dbApplications);
                    IList<int> roleIds = filteredRoles.Select(x => (int)x.RoleId).ToList();
                    if (dbRoles?.Count != roleIds?.Count) //Invalid Role Id found.
                    {
                        var dbRoleByIds = dbRoles;
                        var idNotExists = roleIds.Where(id => !dbRoleByIds.Any(role => role.Id == id)).ToList();
                        var roleList = filteredRoles;
                        idNotExists?.ForEach(x =>
                        {
                            var role = roleList.FirstOrDefault(x1 => x1.RoleId == x);
                            messages.Add(_messageDescriptions, role, MessageType.RequestedUpdateRoleNotExists);
                        });
                    }
                    else
                    {
                        var applications = filteredRoles.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                        if (this.IsApplicationExistInDb(applications, dbApplications, ref messages))
                        {
                            result = IsRecordUpdateCountMatching(filteredRoles, dbRoles, ref messages);
                            if (result)
                                result = this.IsRoleNameUnique(filteredRoles, ref validationMessages);
                        }
                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.RoleInfo> roles, IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = roles.Where(x => !dbRoles.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.RoleId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.RoleName, MessageType.RoleUpdateCountMismatch, x.RoleName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRoleNameUnique(IList<DomainModel.RoleInfo> filteredRoles, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var roleNames = filteredRoles.Select(x => x.RoleName);
            var appNames = filteredRoles.Select(x => x.ApplicationName);
            var appRoles = _repository.FindBy(x => appNames.Contains(x.Application.Name))
                                        .Join(roleNames,
                                              role => role.Name,
                                              roleName => roleName,
                                              (role, roleName) => role)
                                        .ToList();
            if (appRoles?.Count > 0)
            {
                var roleAlreadyExist = filteredRoles.Where(x => !appRoles.Select(x1 => x1.Id)
                                                                 .ToList()
                                                                 .Contains((int)x.RoleId));
                roleAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x.RoleName, MessageType.RoleAlreadyExist, x.RoleName);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.RoleInfo> roles,
                                            ref IList<DomainModel.RoleInfo> filteredRoles,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (roles != null && roles.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredRoles == null || filteredRoles.Count <= 0)
                    filteredRoles = FilterRecord(roles, validationType);

                if (filteredRoles?.Count > 0 && IsValidPayload(filteredRoles, validationType, ref validationMessages))
                {
                    this.GetRoleAndApplicationDbInfo(filteredRoles, false, ref dbRoles, ref dbApplications);
                    var applications = filteredRoles.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        IList<string> roleNotExists = null;
                        var appRoleNames = filteredRoles.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.RoleName)).Distinct().ToList();
                        result = IsRoleExistInDb(appRoleNames, dbRoles, ref roleNotExists, ref validationMessages);
                        if (result)
                            result = IsRoleCanBeRemove(dbRoles, ref validationMessages);
                    }
                }
            }
            return result;
        }

        private bool IsRoleCanBeRemove(IList<DbModel.Role> dbRoles, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbRoles?.Where(x => x.IsAnyCollectionPropertyContainValue())
                    .ToList()
                    .ForEach(x =>
                    {
                        messages.Add(_messageDescriptions, x.Name, MessageType.RoleIsBeingUsed, x.Name);
                    });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private void GetRoleAndApplicationDbInfo(IList<RoleInfo> filteredRoles,
                                                 bool isRoleInfoById,
                                                 ref IList<DbModel.Role> dbRoles,
                                                 ref IList<DbModel.Application> dbAppications)
        {
            var roleNames = !isRoleInfoById ?
                            filteredRoles.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.RoleName))
                                         .Distinct()
                                         .ToList() : null;

            IList<int> roleIds = isRoleInfoById ? filteredRoles.Select(x => (int)x.RoleId).Distinct().ToList() : null;
            IList<string> appNames = filteredRoles.Select(x => x.ApplicationName).Distinct().ToList();
            if (dbRoles == null || dbRoles.Count <= 0)
                dbRoles = isRoleInfoById ? this.GetRoleById(roleIds).ToList() : this.GetRoleByName(roleNames).ToList();
            if (dbAppications == null || dbAppications.Count <= 0)
                dbAppications = _applicationRepository.Get(appNames).ToList();
        }

        private IList<DomainModel.RoleInfo> FilterRecord(IList<DomainModel.RoleInfo> roles,
                                                            ValidationType filterType)
        {
            IList<DomainModel.RoleInfo> filteredRoles = null;

            if (filterType == ValidationType.Add)
                filteredRoles = roles?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredRoles = roles?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredRoles = roles?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredRoles;
        }

        private bool IsRoleExistInDb(IList<KeyValuePair<string, string>> appRoleNames,
                                        IList<DbModel.Role> dbRoles,
                                        ref IList<string> roleNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbRoles == null)
                dbRoles = new List<DbModel.Role>();

            var validMessages = validationMessages;

            if (appRoleNames?.Count > 0)
            {
                roleNotExists = appRoleNames
                                  .Where(x => !dbRoles
                                              .Any(x1 => x1.Application.Name == x.Key &&
                                                         x1.Name == x.Value))
                                  .Select(x => x.Value)
                                  .ToList();

                roleNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.RoleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response RemoveRole(IList<RoleInfo> roles, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Role> dbRoles = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(roles, ValidationType.Delete, ref dbRoles);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() &&
                                                Convert.ToBoolean(response.Result) &&
                                                dbRoles?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbRoles);
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roles);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<DomainModel.RoleInfo> roles,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.RoleInfo> filteredRoles,
                                                    ref IList<DbModel.Role> dbRoles,
                                                    ref IList<DbModel.Application> dbAppications)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(roles, ref filteredRoles, ref dbRoles, ref dbAppications, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(roles, ref filteredRoles, ref dbRoles, ref dbAppications, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(roles, ref filteredRoles, ref dbRoles, ref dbAppications, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roles);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRoleExistInDb(IList<int> roleIds,
                                        IList<DbModel.Role> dbRoles,
                                        ref IList<int> roleNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbRoles == null)
                dbRoles = new List<DbModel.Role>();

            var validMessages = validationMessages;

            if (roleIds?.Count > 0)
            {
                roleNotExists = roleIds.Where(x => !dbRoles.Select(x1 => (int)x1.Id).ToList().Contains(x)).ToList();
                roleNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.RoleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsValidPayload(IList<DomainModel.RoleInfo> roles,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(roles), validationType);
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

        #endregion
        #endregion
    }
}