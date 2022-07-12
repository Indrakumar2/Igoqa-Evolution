using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Evolution.ResourceSearch.Domain.Interfaces.Validations;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.ResourceSearch.Core.Services
{
    public class OverrideResourceService : IOverrideResourceService
    {
        private readonly IAppLogger<OverrideResourceService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMapper _mapper = null;
        public readonly IOverrideResourceRepository _overrideResourceRepository = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly IOverrideResourceValidation _validationService = null;
        private readonly IAuditLogger _auditLogger = null;

        public OverrideResourceService(JObject messages,
                                    IAppLogger<OverrideResourceService> logger,
                                    IMapper mapper,
                                    IOverrideResourceRepository overrideResourceRepository,
                                    ITechnicalSpecialistService technicalSpecialistService,
                                    IAuditLogger auditLogger,
                                    IOverrideResourceValidation validationService)
        {
            _logger = logger;
            _mapper = mapper;
            _messageDescriptions = messages;
            _overrideResourceRepository = overrideResourceRepository;
            _technicalSpecialistService = technicalSpecialistService;
            _validationService = validationService;
            _auditLogger = auditLogger;
        }

        public Response Get(IList<int> resourceSearchIds)
        {
            IList<OverridenPreferredResource> result = null;
            Exception exception = null;
            try
            {
                result = this._overrideResourceRepository.FindBy(x => resourceSearchIds.Contains(x.Id)).ProjectTo<OverridenPreferredResource>().ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearchIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Modify(IList<OverridenPreferredResource> resources, bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.OverrideResource> dbOverrideResources = null;
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            try
            {
                IList<OverridenPreferredResource> recordToBeUpdated = null;
                
                if (IsRecordValidForProcess(resources, ValidationType.Update, ref recordToBeUpdated, ref dbOverrideResources, ref dbTsInfos, ref validationMessages))
                {
                    var actionUser = resources?.FirstOrDefault()?.ActionByUser;
                    IList<OverridenPreferredResource> domExistingOverridenPreferredResource = new List<OverridenPreferredResource>();
                    dbOverrideResources.ToList().ForEach(x =>
                    {
                        domExistingOverridenPreferredResource.Add(ObjectExtension.Clone(_mapper.Map<OverridenPreferredResource>(x)));
                    });
                    dbOverrideResources.ToList().ForEach(overRes =>
                    {
                        var tsToBeModify = recordToBeUpdated.FirstOrDefault(x => x.Id == overRes.Id);
                        _mapper.Map(tsToBeModify, overRes, opt =>
                        {
                            opt.Items["isAssignId"] = true;
                            opt.Items["dbTechnicalSpecialist"] = dbTsInfos;
                        });
                        overRes.LastModification = DateTime.UtcNow;
                        overRes.ModifiedBy = actionUser;
                    });

                    _overrideResourceRepository.AutoSave = false;
                    _overrideResourceRepository.Update(dbOverrideResources);

                    if (commitChange)
                    {
                        int value=_overrideResourceRepository.ForceSave();
                        if (value > 0)
                            resources?.ToList().ForEach(x => AuditLog(x,
                                                               ValidationType.Update.ToAuditActionType(),
                                                               SqlAuditModuleType.OverrideResource,
                                                               domExistingOverridenPreferredResource?.FirstOrDefault(x1 => x1.Id == x.Id),
                                                               x));
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resources);

            }
            finally
            {
                _overrideResourceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), null, exception);
        }

        public Response Modify(IList<OverridenPreferredResource> resources, ref IList<DbModel.OverrideResource> dbOverrideResources,out IList<DbModel.TechnicalSpecialist> dbTsInfos, bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            dbTsInfos = new List<DbModel.TechnicalSpecialist>();
            try
            {
                IList<OverridenPreferredResource> recordToBeUpdated = null;
                
                if (IsRecordValidForProcess(resources, ValidationType.Update, ref recordToBeUpdated, ref dbOverrideResources, ref dbTsInfos, ref validationMessages))
                {
                    var tsData = dbTsInfos;
                    IList<OverridenPreferredResource> domExistingOverridenPreferredResource = new List<OverridenPreferredResource>();
                    dbOverrideResources.ToList().ForEach(x =>
                    {
                        domExistingOverridenPreferredResource.Add(ObjectExtension.Clone(_mapper.Map<OverridenPreferredResource>(x)));
                    });
                    dbOverrideResources.ToList().ForEach(overRes =>
                    { 
                        var tsToBeModify = recordToBeUpdated.FirstOrDefault(x => x.Id == overRes.Id);
                        _mapper.Map(tsToBeModify, overRes, opt =>
                        {
                            opt.Items["isAssignId"] = true;
                            opt.Items["dbTechnicalSpecialist"] = tsData;
                        });
                        overRes.LastModification = DateTime.UtcNow;
                    });

                    _overrideResourceRepository.AutoSave = false;
                    _overrideResourceRepository.Update(dbOverrideResources);

                    if (commitChange)
                    {
                       int value= _overrideResourceRepository.ForceSave();
                        if (value > 0)
                            resources?.ToList().ForEach(x => AuditLog(x,
                                                               ValidationType.Update.ToAuditActionType(),
                                                               SqlAuditModuleType.OverrideResource,
                                                               domExistingOverridenPreferredResource?.FirstOrDefault(x1 => x1.Id == x.Id),
                                                               x));
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resources);

            }
            finally
            {
                _overrideResourceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), null, exception);
        }

        public Response Save(IList<OverridenPreferredResource> resources, bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.OverrideResource> dbOverrideResources = null;
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            try
            {
                IList<OverridenPreferredResource> recordToBeInserted = null;

                if (IsRecordValidForProcess(resources, ValidationType.Add, ref recordToBeInserted, ref dbOverrideResources, ref dbTsInfos, ref validationMessages))
                {
                    var actionUser = resources?.FirstOrDefault()?.ActionByUser;
                    var dbOverRideResourceToBeInserted = _mapper.Map<IList<DbModel.OverrideResource>>(recordToBeInserted, opt =>
                          {
                              opt.Items["isAssignId"] = false;
                              opt.Items["dbTechnicalSpecialist"] = dbTsInfos;
                          });

                    _overrideResourceRepository.AutoSave = false;
                    _overrideResourceRepository.Add(dbOverRideResourceToBeInserted);

                    if (commitChange)
                    {
                        int value=_overrideResourceRepository.ForceSave();
                        if (value > 0)
                            resources?.ToList().ForEach(x => AuditLog(x,
                                                                      ValidationType.Add.ToAuditActionType(),
                                                                      SqlAuditModuleType.OverrideResource,
                                                                      null,
                                                                       _mapper.Map<IList<OverridenPreferredResource>>(dbOverRideResourceToBeInserted)));
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resources);

            }
            finally
            {
                _overrideResourceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), null, exception);
        }

        public Response Save(IList<OverridenPreferredResource> resources,
                            out IList<DbModel.OverrideResource> dbOverrideResources,
                            out IList<DbModel.TechnicalSpecialist> dbTsInfos, 
                            bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<OverridenPreferredResource> recordToBeInserted = null;
            dbOverrideResources = new List<DbModel.OverrideResource>();
            dbTsInfos = new List<DbModel.TechnicalSpecialist>();
            try
            {  
                if (IsRecordValidForProcess(resources, ValidationType.Add, ref recordToBeInserted, ref dbOverrideResources, ref dbTsInfos, ref validationMessages))
                {
                    var dbTsdata = dbTsInfos;
                    var actionUser = resources?.FirstOrDefault()?.ActionByUser;
                    var dbOverRideResourceToBeInserted = _mapper.Map<IList<DbModel.OverrideResource>>(recordToBeInserted, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["dbTechnicalSpecialist"] = dbTsdata;
                    });

                    _overrideResourceRepository.AutoSave = false;
                    _overrideResourceRepository.Add(dbOverRideResourceToBeInserted);

                    if (commitChange)
                    {
                        int value = _overrideResourceRepository.ForceSave();
                        if (value > 0)
                            resources?.ToList().ForEach(x => AuditLog(x,
                                                                      ValidationType.Add.ToAuditActionType(),
                                                                      SqlAuditModuleType.OverrideResource,
                                                                      null,
                                                                      _mapper.Map<IList<OverridenPreferredResource>>(dbOverRideResourceToBeInserted)));
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resources);

            }
            finally
            {
                _overrideResourceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), null, exception);
        }

        public bool IsRecordExistInDb(IList<int> overrideResourceId, ref IList<DbModel.OverrideResource> dbOverrideResources, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbOverrideResources == null && overrideResourceId?.Count > 0)
                {
                    dbOverrideResources = GetOverrideResourceById(overrideResourceId);
                }
                result = IsOverrideResourceExistInDb(overrideResourceId, dbOverrideResources, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), overrideResourceId);
            }
            return result;
        }


        private bool IsRecordValidForProcess(IList<OverridenPreferredResource> resources,
                                           ValidationType validationType,
                                           ref IList<OverridenPreferredResource> filteredResources,
                                           ref IList<DbModel.OverrideResource> dbOverrideResources,
                                           ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                           ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = false;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(resources, ref filteredResources, ref dbOverrideResources, ref dbTsInfos, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(resources, ref filteredResources, ref dbOverrideResources, ref dbTsInfos, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resources);
            }
            return result;
        }

        private bool IsRecordValidForAdd(IList<OverridenPreferredResource> resources,
                                           ref IList<OverridenPreferredResource> filteredResources,
                                           ref IList<DbModel.OverrideResource> dbOverrideResources,
                                           ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (resources != null && resources.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredResources == null || filteredResources.Count <= 0)
                    filteredResources = FilterRecord(resources, validationType);

                if (filteredResources?.Count > 0 && IsValidPayload(filteredResources, validationType, ref validationMessages))
                {
                    var epins = filteredResources.Select(x => x.TechSpecialist?.Epin.ToString()).Distinct().ToList();
                    var valResponse = _technicalSpecialistService.IsRecordExistInDb(epins, ref dbTsInfos, ref validationMessages);
                    result = Convert.ToBoolean(valResponse.Result);
                    if (!result)
                    {
                        validationMessages.AddRange(valResponse.ValidationMessages);
                    }

                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<OverridenPreferredResource> resources,
                                           ref IList<OverridenPreferredResource> filteredResources,
                                           ref IList<DbModel.OverrideResource> dbOverrideResources,
                                           ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (resources != null && resources.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredResources == null || filteredResources.Count <= 0)
                    filteredResources = FilterRecord(resources, validationType);

                if (filteredResources?.Count > 0 && IsValidPayload(filteredResources, validationType, ref messages))
                {
                    var overrideResourceId = filteredResources.Select(x => (int)x.Id).ToList();
                    if (IsRecordExistInDb(overrideResourceId, ref dbOverrideResources, ref validationMessages))
                    {
                        var epins = filteredResources.Select(x => x.TechSpecialist?.Epin.ToString()).Distinct().ToList();
                        var valResponse = _technicalSpecialistService.IsRecordExistInDb(epins, ref dbTsInfos, ref validationMessages);
                        result = Convert.ToBoolean(valResponse.Result);
                        if (!result)
                        {
                            validationMessages.AddRange(valResponse.ValidationMessages);
                        }
                    }

                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private IList<OverridenPreferredResource> FilterRecord(IList<OverridenPreferredResource> resources, ValidationType filterType)
        {
            IList<OverridenPreferredResource> filterResources = null;

            if (filterType == ValidationType.Add)
                filterResources = resources?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterResources = resources?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterResources = resources?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterResources;
        }

        private bool IsValidPayload(IList<OverridenPreferredResource> resources,
                      ValidationType validationType,
                      ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(resources), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.ResourceSearch, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<DbModel.OverrideResource> GetOverrideResourceById(IList<int> overrideResourceId)
        {
            IList<DbModel.OverrideResource> dbOverrideResources = null;
            if (overrideResourceId?.Count > 0)
                dbOverrideResources = _overrideResourceRepository.FindBy(x => overrideResourceId.Contains(x.Id)).ToList();

            return dbOverrideResources;
        }

        private bool IsOverrideResourceExistInDb(IList<int> overrideResourceId, IList<DbModel.OverrideResource> dbOverrideResources, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbOverrideResources == null)
                dbOverrideResources = new List<DbModel.OverrideResource>();

            var validMessages = validationMessages;

            if (overrideResourceId?.Count > 0)
            {
                var overrideResourceIdNotExists = overrideResourceId.Where(id => !dbOverrideResources.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                overrideResourceIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.OverrideResourceIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response AuditLog(OverridenPreferredResource overrideSearch,
                                   SqlAuditActionType sqlAuditActionType,
                                   SqlAuditModuleType sqlAuditModuleType,
                                   object oldData,
                                   object newData)
        {
            LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
            Exception exception = null;
            long? eventId = 0;
            if (overrideSearch != null && !string.IsNullOrEmpty(overrideSearch.ActionByUser))
            {
                string actionBy = overrideSearch.ActionByUser;
                if (overrideSearch.EventId > 0)
                    eventId = overrideSearch.EventId;
                else
                    eventId = logEventGeneration.GetEventLogId(eventId,
                                                                  sqlAuditActionType,
                                                                  actionBy,
                                                                  overrideSearch.ResourceSearchId.ToString(),
                                                                  "OverrideSearch");

                return _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

    }
}
