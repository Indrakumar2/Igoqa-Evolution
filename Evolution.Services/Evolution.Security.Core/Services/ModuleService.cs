using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
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
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Core.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IAppLogger<ModuleService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IModuleRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IModuleValidationService _validationService = null;
        private readonly IApplicationRepository _applicationRepository = null;

        #region Constructor
        public ModuleService(IMapper mapper,
                             IModuleRepository repository,
                             IApplicationRepository applicationRepository,
                             IAppLogger<ModuleService> logger,
                             IModuleValidationService validationService,
                             JObject messgaes)
        {
            this._applicationRepository = applicationRepository;
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messageDescriptions = messgaes;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(ModuleInfo searchModel)
        {
            IList<DomainModel.ModuleInfo> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(searchModel)?
                                         .AsQueryable()
                                         .ProjectTo<DomainModel.ModuleInfo>()
                                         .ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(string applicationName, IList<string> moduleNames)
        {
            IList<DomainModel.ModuleInfo> result = null;
            Exception exception = null;
            try
            {
                result = this.GetModuleByName(moduleNames?.Select(x => new KeyValuePair<string, string>(applicationName, x)).ToList())
                                .AsQueryable()
                                .ProjectTo<DomainModel.ModuleInfo>()
                                .ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> moduleIds)
        {
            IList<DomainModel.ModuleInfo> result = null;
            Exception exception = null;
            try
            {
                result = this.GetModuleById(moduleIds)
                                    .AsQueryable()
                                    .ProjectTo<DomainModel.ModuleInfo>()
                                    .ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        #endregion

        #region Add
        public Response Add(IList<ModuleInfo> modules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.Module> dbModules = null;
            return AddModule(modules, ref dbModules, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<ModuleInfo> modules, ref IList<DbModel.Module> dbModules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return AddModule(modules, ref dbModules, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<ModuleInfo> modules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.Module> dbModules = null;
            return UpdateModules(modules, ref dbModules, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<ModuleInfo> modules, ref IList<DbModel.Module> dbModules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return UpdateModules(modules, ref dbModules, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete
        public Response Delete(IList<ModuleInfo> moduleInfo, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return this.RemoveModule(moduleInfo, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<ModuleInfo> modules, ValidationType validationType)
        {
            IList<DbModel.Module> dbModules = null;
            return IsRecordValidForProcess(modules, validationType, ref dbModules);
        }

        public Response IsRecordValidForProcess(IList<ModuleInfo> modules, ValidationType validationType, ref IList<DbModel.Module> dbModules)
        {
            IList<DomainModel.ModuleInfo> filteredModules = null;
            IList<DbModel.Application> dbApplications = null;
            return this.CheckRecordValidForProcess(modules, validationType, ref filteredModules, ref dbModules, ref dbApplications);
        }

        public Response IsRecordValidForProcess(IList<ModuleInfo> modules, ValidationType validationType, IList<DbModel.Module> dbModules)
        {
            return IsRecordValidForProcess(modules, validationType, ref dbModules);
        }
        #endregion

        #endregion

        #region Private Metods

        #region Get
        private IList<DbModel.Module> GetModuleByName(IList<KeyValuePair<string, string>> appModuleNames)
        {
            IList<DbModel.Module> dbModules = null;
            if (appModuleNames?.Count > 0)
            {
                var appNames = appModuleNames.Select(x => x.Key);
                var moduleNames = appModuleNames.Select(x => x.Value);
                dbModules = _repository.FindBy(x => moduleNames.Contains(x.Name) &&
                                                    appNames.Contains(x.Application.Name)).ToList();
            }
            return dbModules;
        }

        private IList<DbModel.Module> GetModuleById(IList<int> roleIds)
        {
            IList<DbModel.Module> dbModules = null;
            if (roleIds?.Count > 0)
                dbModules = _repository.FindBy(x => roleIds.Contains((int)x.Id)).ToList();

            return dbModules;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.ModuleInfo> modules,
                                         ref IList<DomainModel.ModuleInfo> filteredModules,
                                         ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules,
                                         ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (modules != null && modules.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredModules == null || filteredModules.Count <= 0)
                    filteredModules = FilterRecord(modules, validationType);

                if (filteredModules?.Count > 0 && IsValidPayload(filteredModules, validationType, ref validationMessages))
                {
                    //IList<int> moduleIds = null;
                    IList<string> moduleNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    this.GetModuleAndApplicationDbInfo(filteredModules, validationType, false, ref dbModules, ref dbApplications);
                    var applications = modules.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        var appModuleNames = modules.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ModuleName)).Distinct().ToList();
                        var response = (!IsModuleExistInDb(appModuleNames, dbModules, ref moduleNotExists, ref messages) && messages.Count == appModuleNames.Count);
                        if (!response)
                        {
                            var mdouleAlreadyExists = appModuleNames.Where(x => !moduleNotExists.Contains(x.Value)).ToList();
                            mdouleAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messageDescriptions, x, MessageType.ModuleAlreadyExist, x);
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

        private Response AddModule(IList<ModuleInfo> modules, ref IList<DbModel.Module> dbModules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Application> dbApplications = null;
                IList<ModuleInfo> recordToBeAdd = null;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(modules, ValidationType.Add, ref recordToBeAdd, ref dbModules, ref dbApplications);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.ModuleId = 0; return x; }).ToList();
                    _repository.Add(_mapper.Map<IList<DbModel.Module>>(recordToBeAdd));
                    if (commitChange)
                    {
                        _repository.ForceSave();
                        dbModules = _repository.Get(recordToBeAdd.Select(x => x.ModuleName).ToList());
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), modules);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateModules(IList<ModuleInfo> modules, ref IList<Module> dbModules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Application> dbApplications = null;
                var recordToBeModify = FilterRecord(modules, ValidationType.Update);

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(modules, ValidationType.Update, ref recordToBeModify, ref dbModules, ref dbApplications);
                else if ((dbModules == null || dbModules?.Count <= 0) && recordToBeModify?.Count > 0)
                    dbModules = _repository.Get(recordToBeModify?.Select(x => (int)x.ModuleId).ToList());

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbModules?.Count > 0))
                {
                    dbModules.ToList().ForEach(dbModule =>
                    {
                        var moduleToBeModify = recordToBeModify.FirstOrDefault(x => x.ModuleId == dbModule.Id);
                        dbModule.ApplicationId = dbApplications.FirstOrDefault(x => x.Name == moduleToBeModify.ApplicationName).Id;
                        dbModule.Name = moduleToBeModify.ModuleName;
                        dbModule.Description = moduleToBeModify.Description;
                        dbModule.LastModification = DateTime.UtcNow;
                        dbModule.UpdateCount = moduleToBeModify.UpdateCount.CalculateUpdateCount();
                        dbModule.ModifiedBy = moduleToBeModify.ModifiedBy;
                    });
                    _repository.AutoSave = false;
                    _repository.Update(dbModules);
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), modules);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.ModuleInfo> modules,
                                            ref IList<DomainModel.ModuleInfo> filteredModules,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (modules != null && modules.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredModules == null || filteredModules.Count <= 0)
                    filteredModules = FilterRecord(modules, validationType);

                if (filteredModules?.Count > 0 && IsValidPayload(filteredModules, validationType, ref messages))
                {
                    this.GetModuleAndApplicationDbInfo(filteredModules, validationType, true, ref dbModules, ref dbApplications);
                    IList<int> moduleIds = filteredModules.Select(x => (int)x.ModuleId).ToList();
                    if (dbModules?.Count != moduleIds?.Count) //Invalid Module Id found.
                    {
                        var dbModuleByIds = dbModules;
                        var idNotExists = moduleIds.Where(id => !dbModuleByIds.Any(module => module.Id == id)).ToList();
                        var moduleList = filteredModules;
                        idNotExists?.ForEach(x =>
                        {
                            var module = moduleList.FirstOrDefault(x1 => x1.ModuleId == x);
                            messages.Add(_messageDescriptions, module, MessageType.RequestedUpdateModuleNotExists);
                        });
                    }
                    else
                    {
                        var applications = filteredModules.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                        if (this.IsApplicationExistInDb(applications, dbApplications, ref messages))
                        {
                            result = IsRecordUpdateCountMatching(filteredModules, dbModules, ref messages);
                            if (result)
                                result = this.IsModuleNameUnique(filteredModules, ref validationMessages);
                        }
                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.ModuleInfo> modules, IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = modules.Where(x => !dbModules.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ModuleId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.ModuleName, MessageType.ModuleUpdateCountMismatch, x.ModuleName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsModuleNameUnique(IList<DomainModel.ModuleInfo> filteredModules, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var moduleNames = filteredModules.Select(x => x.ModuleName);
            var appNames = filteredModules.Select(x => x.ApplicationName);
            var appModules = _repository.FindBy(x => appNames.Contains(x.Application.Name))
                                        .Join(moduleNames,
                                              module => module.Name,
                                              moduleName => moduleName,
                                              (module, moduleName) => module)
                                        .ToList();
            if (appModules?.Count > 0)
            {
                var moduleAlreadyExist = filteredModules.Where(x => !appModules.Select(x1 => x1.Id)
                                                                 .ToList()
                                                                 .Contains((int)x.ModuleId));
                moduleAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x.ModuleName, MessageType.ModuleAlreadyExist, x.ModuleName);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.ModuleInfo> modules,
                                            ref IList<DomainModel.ModuleInfo> filteredModules,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (modules != null && modules.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredModules == null || filteredModules.Count <= 0)
                    filteredModules = FilterRecord(modules, validationType);

                if (filteredModules?.Count > 0 && IsValidPayload(filteredModules, validationType, ref validationMessages))
                {
                    this.GetModuleAndApplicationDbInfo(filteredModules, validationType, false, ref dbModules, ref dbApplications);
                    var applications = filteredModules.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        IList<string> moduleNotExists = null;
                        var appModuleNames = filteredModules.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ModuleName)).Distinct().ToList();
                        result = IsModuleExistInDb(appModuleNames, dbModules, ref moduleNotExists, ref validationMessages);
                        if (result)
                            result = IsModuleCanBeRemove(dbModules, ref validationMessages);
                    }
                }
            }
            return result;
        }

        private bool IsModuleCanBeRemove(IList<DbModel.Module> dbModules, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbModules?.Where(x => x.IsAnyCollectionPropertyContainValue())
                      .ToList()
                      .ForEach(x =>
                      {
                          if (x.ModuleActivity?.Count > 0)
                              messages.Add(_messageDescriptions, x.Name, MessageType.ModuleIsBeingUsed, x.Name);
                      });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private void GetModuleAndApplicationDbInfo(IList<DomainModel.ModuleInfo> filteredModules,
                                                    ValidationType validationType,
                                                    bool isModuleInfoById,
                                                    ref IList<DbModel.Module> dbModules,
                                                    ref IList<DbModel.Application> dbAppications)
        {
            var appModuleNames = !isModuleInfoById ?
                                    filteredModules.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ModuleName))
                                                    .Distinct()
                                                    .ToList() : null;

            IList<int> moduleIds = isModuleInfoById ? filteredModules.Select(x => (int)x.ModuleId).Distinct().ToList() : null;
            IList<string> appNames = filteredModules.Select(x => x.ApplicationName).Distinct().ToList();

            if (dbModules == null || dbModules.Count <= 0)
                dbModules = isModuleInfoById ?
                            this.GetModuleById(moduleIds).ToList() :
                            this.GetModuleByName(appModuleNames).ToList();
            if (dbAppications == null || dbAppications.Count <= 0)
                dbAppications = _applicationRepository.Get(appNames).ToList();
        }

        private IList<DomainModel.ModuleInfo> FilterRecord(IList<DomainModel.ModuleInfo> modules,
                                                            ValidationType filterType)
        {
            IList<DomainModel.ModuleInfo> filteredModules = null;

            if (filterType == ValidationType.Add)
                filteredModules = modules?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredModules = modules?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredModules = modules?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredModules;
        }

        private bool IsModuleExistInDb(IList<KeyValuePair<string, string>> appModuleNames,
                                        IList<DbModel.Module> dbModules,
                                        ref IList<string> moduleNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbModules == null)
                dbModules = new List<DbModel.Module>();

            var validMessages = validationMessages;

            if (appModuleNames?.Count > 0)
            {
                moduleNotExists = appModuleNames
                                  .Where(x => !dbModules
                                              .Any(x1 => x1.Application.Name == x.Key &&
                                                         x1.Name == x.Value))
                                  .Select(x => x.Value)
                                  .ToList();

                moduleNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ModuleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response RemoveModule(IList<ModuleInfo> modules, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Module> dbModules = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(modules, ValidationType.Delete, ref dbModules);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() &&
                                                Convert.ToBoolean(response.Result) &&
                                                dbModules?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbModules);
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), modules);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<DomainModel.ModuleInfo> modules,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.ModuleInfo> filteredModules,
                                                    ref IList<DbModel.Module> dbModules,
                                                    ref IList<DbModel.Application> dbAppications)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(modules, ref filteredModules, ref dbModules, ref dbAppications, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(modules, ref filteredModules, ref dbModules, ref dbAppications, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(modules, ref filteredModules, ref dbModules, ref dbAppications, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), modules);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsModuleExistInDb(IList<int> moduleIds,
                                        IList<DbModel.Module> dbModules,
                                        ref IList<int> moduleNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbModules == null)
                dbModules = new List<DbModel.Module>();

            var validMessages = validationMessages;

            if (moduleIds?.Count > 0)
            {
                moduleNotExists = moduleIds.Where(x => !dbModules.Select(x1 => (int)x1.Id).ToList().Contains(x)).ToList();
                moduleNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ModuleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsValidPayload(IList<DomainModel.ModuleInfo> modules,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(modules), validationType);
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