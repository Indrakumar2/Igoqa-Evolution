using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistTrainingAndCompetencyTypeService : ITechnicalSpecialistTrainingAndCompetancyTypeService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistTrainingAndCompetencyTypeService> _logger = null;
        private readonly ITechnicalSpecialistTrainingAndCompetencyTypeRepository _tsTrainingAndCompetencyTypeRepository = null;
        private readonly IInternalTrainingService _internalTrainingService = null;
        private readonly ICompetencyService _competencyService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public TechnicalSpecialistTrainingAndCompetencyTypeService(IMapper mapper,
            JObject messages,
            IAppLogger<TechnicalSpecialistTrainingAndCompetencyTypeService> logger,
            ITechnicalSpecialistTrainingAndCompetencyTypeRepository tsTrainingAndCompetencyTypeRepository,
            IInternalTrainingService internalTrainingService,
            ICompetencyService competencyService, IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _messages = messages;
            _logger = logger;
            _tsTrainingAndCompetencyTypeRepository = tsTrainingAndCompetencyTypeRepository;
            _internalTrainingService = internalTrainingService;
            _competencyService = competencyService;
            _auditSearchService = auditSearchService;

        }

        public Response Add(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys = null;
            IList<DbModel.Data> dbMasterIntTrainingAndCompetencys = null;
            return AddInternalTrainingAndCompetencyType(tsTypes, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys, ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddInternalTrainingAndCompetencyType(tsTypes, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes = null;
            return RemoveInternalTrainingAndCompetencyType(tsTypes, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveInternalTrainingAndCompetencyType(tsTypes, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, commitChange, isDbValidationRequired);
        }


        public Response Get(IList<int> tsIds)
        {
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistInternalTrainingAndCompetencyType>>(GetTsInternalTrainingAndCompetencyTypeById(tsIds));

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(TechnicalSpecialistInternalTrainingAndCompetencyType searchModel)
        {
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistInternalTrainingAndCompetencyType>>(_tsTrainingAndCompetencyTypeRepository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> typeNames)
        {
            IList<TechnicalSpecialistCertification> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCertification>>(GetTsInternalTrainingAndCompetencyTypeByTypeNames(typeNames));

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), typeNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<int> tsTypeIds, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsTypeIdNotExists = null;
            return IsRecordExistInDb(tsTypeIds, ref dbTsIntTrainingAndCompetencyTypes, ref tsTypeIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsTypeIds,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<int> tsIntTrainingAndCompetencyTypeIdNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsIntTrainingAndCompetencyTypes == null && tsTypeIds?.Count > 0)
                    dbTsIntTrainingAndCompetencyTypes = GetTsInternalTrainingAndCompetencyTypeById(tsTypeIds);

                result = IsTsIntTrainingAndCompetencyTypeExistInDb(tsTypeIds, dbTsIntTrainingAndCompetencyTypes, ref tsIntTrainingAndCompetencyTypeIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIntTrainingAndCompetencyTypeIdNotExists);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, ValidationType validationType, CompCertTrainingType triningOrCompetacyType)
        {
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTSTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys = null;
            IList<DbModel.Data> dbMasterIntTrainingAndCompetencys = null;

            return IsRecordValidForProcess(tsTypes, validationType, triningOrCompetacyType, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, ValidationType validationType, CompCertTrainingType triningOrCompetacyType, ref IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTSTypes, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys, ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys)
        {
            return CheckRecordValidForProcess(tsTypes,
                                             validationType,
                                             triningOrCompetacyType,
                                             ref filteredTSTypes,
                                             ref dbTsIntTrainingAndCompetencyTypes,
                                             ref dbTsIntTrainingAndCompetencys,
                                             ref dbMasterIntTrainingAndCompetencys);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, ValidationType validationType, CompCertTrainingType triningOrCompetacyType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys, ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys)
        {
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTSTypes = null;
            return CheckRecordValidForProcess(tsTypes,
                                           validationType,
                                           triningOrCompetacyType,
                                           ref filteredTSTypes,
                                           ref dbTsIntTrainingAndCompetencyTypes,
                                           ref dbTsIntTrainingAndCompetencys,
                                           ref dbMasterIntTrainingAndCompetencys);
        }

        public Response Modify(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys = null;
            IList<DbModel.Data> dbMasterIntTrainingAndCompetencys = null;
            return UpdateInternalTrainingAndCompetencyType(tsTypes, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys, ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateInternalTrainingAndCompetencyType(tsTypes, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, commitChange, isDbValidationRequired);
        }


        private IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> GetTsInternalTrainingAndCompetencyTypeById(IList<int> tsIds)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsTrainingAndCompetencyTypes = null;
            if (tsIds?.Count > 0)
                dbTsTrainingAndCompetencyTypes = _tsTrainingAndCompetencyTypeRepository.FindBy(x => tsIds.Contains(x.Id)).ToList();

            return dbTsTrainingAndCompetencyTypes;
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> GetTsInternalTrainingAndCompetencyTypeByTypeNames(IList<string> tsTypeNames)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsTrainingAndCompetencyTypes = null;
            if (tsTypeNames?.Count > 0)
                dbTsTrainingAndCompetencyTypes = _tsTrainingAndCompetencyTypeRepository.FindBy(x => tsTypeNames.Contains(x.TrainingOrCompetencyData.Name)).ToList();

            return dbTsTrainingAndCompetencyTypes;
        }

        private Response AddInternalTrainingAndCompetencyType(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                        CompCertTrainingType triningOrCompetacyType,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                        ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys,
                                        bool commitChange = true,
                                        bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.Data> dbMasterTrainingAndCompetencys = null;
            long? eventId = 0;
            Response valdResponse = null;
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> recordToBeAdd = null;

            try
            {
                eventId = tsIntTrainingAndCompetencyTypes?.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsIntTrainingAndCompetencyTypes, ValidationType.Add, triningOrCompetacyType, ref recordToBeAdd, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                }

                if (!isDbValidationRequired && tsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsIntTrainingAndCompetencyTypes, ValidationType.Add);
                }

                if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    dbMasterTrainingAndCompetencys = dbMasterIntTrainingAndCompetencys;
                    _tsTrainingAndCompetencyTypeRepository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DBMasterTrainingAndCompetency"] = dbMasterTrainingAndCompetencys;
                    });
                    _tsTrainingAndCompetencyTypeRepository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _tsTrainingAndCompetencyTypeRepository.ForceSave();
                        dbTsIntTrainingAndCompetencyTypes = mappedRecords;

                        if (mappedRecords?.Count > 0 && savedCnt > 0)
                        {
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsIntTrainingAndCompetencyTypes.FirstOrDefault().ActionByUser,
                                                                                                      null,
                                                                                                       ValidationType.Add.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.TechnicalSpecialistTrainingAndCompetencyType,
                                                                                                        null,
                                                                                                         _mapper.Map<TechnicalSpecialistInternalTrainingAndCompetencyType>(x1)
                                                                                                        ));
                        }
                    }

                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIntTrainingAndCompetencyTypes);
            }
            finally
            {
                _tsTrainingAndCompetencyTypeRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateInternalTrainingAndCompetencyType(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                        CompCertTrainingType triningOrCompetacyType,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                        ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys,
                                        bool commitChange = true,
                                        bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> recordToBeModify = null;
            IList<DbModel.Data> dbMasterTrainingAndCompetencys = null;
            Response valdResponse = null;
            long? eventId = 0;

            try
            {
                eventId = tsIntTrainingAndCompetencyTypes?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsIntTrainingAndCompetencyTypes, ValidationType.Add, triningOrCompetacyType, ref recordToBeModify, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                }

                if (!isDbValidationRequired && tsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsIntTrainingAndCompetencyTypes, ValidationType.Update);
                }
                if (dbTsIntTrainingAndCompetencyTypes?.Count <= 0 && recordToBeModify?.Count > 0)
                {
                    dbTsIntTrainingAndCompetencyTypes = _tsTrainingAndCompetencyTypeRepository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                }

                if (recordToBeModify?.Count > 0)
                {
                    if ((valdResponse == null || Convert.ToBoolean(valdResponse.Result)) && dbTsIntTrainingAndCompetencyTypes?.Count > 0)
                    {
                        dbMasterTrainingAndCompetencys = dbMasterIntTrainingAndCompetencys;
                        IList<TechnicalSpecialistInternalTrainingAndCompetencyType> domExsistanceIntTrainingAndCompetencyTypes = new List<TechnicalSpecialistInternalTrainingAndCompetencyType>();
                       
                        dbTsIntTrainingAndCompetencyTypes.ToList().ForEach(tsTypeInfo =>
                        {
                            domExsistanceIntTrainingAndCompetencyTypes.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistInternalTrainingAndCompetencyType>(tsTypeInfo)));
                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsTypeInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsTypeInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DBMasterTrainingAndCompetency"] = dbMasterTrainingAndCompetencys;
                                });
                            }

                        });
                        _tsTrainingAndCompetencyTypeRepository.AutoSave = false;
                        _tsTrainingAndCompetencyTypeRepository.Update(dbTsIntTrainingAndCompetencyTypes);
                        if (commitChange)
                        {
                            var savedCnt = _tsTrainingAndCompetencyTypeRepository.ForceSave();
                            if (recordToBeModify?.Count > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                     null,
                                                                    ValidationType.Update.ToAuditActionType(),
                                                                    SqlAuditModuleType.TechnicalSpecialistTrainingAndCompetencyType,
                                                                      domExsistanceIntTrainingAndCompetencyTypes?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                    x1
                                                                    ));
                            }

                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIntTrainingAndCompetencyTypes);
            }
            finally
            {
                _tsTrainingAndCompetencyTypeRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveInternalTrainingAndCompetencyType(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                                                CompCertTrainingType triningOrCompetacyType,
                                                                ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                                                bool commitChange = true,
                                                                bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys = null;
            IList<DbModel.Data> dbMasterIntTrainingAndCompetencys = null;
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> recordToBeDeleted = null;
            long? eventId = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsIntTrainingAndCompetencyTypes?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(tsIntTrainingAndCompetencyTypes, ValidationType.Delete, triningOrCompetacyType, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);

                if (!isDbValidationRequired && tsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsIntTrainingAndCompetencyTypes, ValidationType.Delete);
                }

                if (dbTsIntTrainingAndCompetencyTypes == null || dbTsIntTrainingAndCompetencyTypes?.Count <= 0 && recordToBeDeleted?.Count > 0)
                {
                    dbTsIntTrainingAndCompetencyTypes = _tsTrainingAndCompetencyTypeRepository.Get(recordToBeDeleted?.Select(x => x.Id).ToList());
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response?.Result)) && dbTsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    var dbTsIntTrainingAndCompetencyTypeToBeDeleted = dbTsIntTrainingAndCompetencyTypes?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _tsTrainingAndCompetencyTypeRepository.AutoSave = false;
                    _tsTrainingAndCompetencyTypeRepository.Delete(dbTsIntTrainingAndCompetencyTypeToBeDeleted);
                    if (commitChange)
                    {

                        int value = _tsTrainingAndCompetencyTypeRepository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value > 0)
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                   null,
                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.TechnicalSpecialistTrainingAndCompetencyType,
                                                                                                    x1,
                                                                                                    null
                                                                                                    ));
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIntTrainingAndCompetencyTypes);
            }
            finally
            {
                _tsTrainingAndCompetencyTypeRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                    ValidationType validationType,
                                    CompCertTrainingType triningOrCompetacyType,
                                    ref IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTsIntTrainingAndCompetencyTypes,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                    ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsIntTrainingAndCompetencyTypes, triningOrCompetacyType, ref filteredTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsIntTrainingAndCompetencyTypes, ref filteredTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencyTypes, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsIntTrainingAndCompetencyTypes, triningOrCompetacyType, ref filteredTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIntTrainingAndCompetencyTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<TechnicalSpecialistInternalTrainingAndCompetencyType> FilterRecord(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes, ValidationType filterType)
        {
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filterTsIntTrainingAndCompetencyTypes = null;

            if (filterType == ValidationType.Add)
                filterTsIntTrainingAndCompetencyTypes = tsIntTrainingAndCompetencyTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsIntTrainingAndCompetencyTypes = tsIntTrainingAndCompetencyTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsIntTrainingAndCompetencyTypes = tsIntTrainingAndCompetencyTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsIntTrainingAndCompetencyTypes;
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                        CompCertTrainingType triningOrCompetacyType,
                                        ref IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                        ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsIntTrainingAndCompetencyTypes != null && tsIntTrainingAndCompetencyTypes.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsIntTrainingAndCompetencyTypes == null || filteredTsIntTrainingAndCompetencyTypes.Count <= 0)
                    filteredTsIntTrainingAndCompetencyTypes = FilterRecord(tsIntTrainingAndCompetencyTypes, validationType);

                if (filteredTsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    IList<string> typeNames = filteredTsIntTrainingAndCompetencyTypes.Select(x => x.TypeName).ToList();

                    if (typeNames?.Count > 0)
                    {
                        if (CompCertTrainingType.IT == triningOrCompetacyType)
                        {
                            result = _internalTrainingService.IsValidInternalTraining(typeNames, ref dbMasterIntTrainingAndCompetencys, ref validationMessages);
                        }
                        else if (CompCertTrainingType.Co == triningOrCompetacyType)
                        {
                            result = _competencyService.IsValidCompetency(typeNames, ref dbMasterIntTrainingAndCompetencys, ref validationMessages);
                        }
                    }

                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                        CompCertTrainingType triningOrCompetacyType,
                                        ref IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                        ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            IList<ValidationMessage> messages = new List<ValidationMessage>();

            if (tsIntTrainingAndCompetencyTypes != null && tsIntTrainingAndCompetencyTypes.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;

                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsIntTrainingAndCompetencyTypes == null || filteredTsIntTrainingAndCompetencyTypes.Count <= 0)
                    filteredTsIntTrainingAndCompetencyTypes = FilterRecord(tsIntTrainingAndCompetencyTypes, validationType);

                if (filteredTsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    GetIntTrainingAndCompetencyTypeDbInfo(filteredTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencyTypes);
                    IList<int> tsCertificationIds = filteredTsIntTrainingAndCompetencyTypes.Select(x => x.Id).ToList();
                    IList<int> tsDBCertificationIds = dbTsIntTrainingAndCompetencyTypes.Select(x => x.Id).ToList();
                    if (tsCertificationIds.Any(x => !tsDBCertificationIds.Contains(x))) //Invalid TechSpecialist  IntTrainingAndCompetencyTypes Id found.
                    {
                        var dbTsCertificateInfosByIds = dbTsIntTrainingAndCompetencyTypes;
                        var idNotExists = tsCertificationIds.Where(id => !dbTsCertificateInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistCertificateList = filteredTsIntTrainingAndCompetencyTypes;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsCertificate = techSpecialistCertificateList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsCertificate, MessageType.TsIntTrainingAndCompetencyTypeUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        IList<string> typeNames = filteredTsIntTrainingAndCompetencyTypes.Select(x => x.TypeName).ToList();

                        if (typeNames?.Count > 0)
                        {
                            if (CompCertTrainingType.IT == triningOrCompetacyType)
                            {
                                result = _internalTrainingService.IsValidInternalTraining(typeNames, ref dbMasterIntTrainingAndCompetencys, ref validationMessages);
                            }
                            else if (CompCertTrainingType.Co == triningOrCompetacyType)
                            {
                                result = _competencyService.IsValidCompetency(typeNames, ref dbMasterIntTrainingAndCompetencys, ref validationMessages);
                            }
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsIntTrainingAndCompetencyTypes,
                                        ref IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsIntTrainingAndCompetencyTypes != null && tsIntTrainingAndCompetencyTypes.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsIntTrainingAndCompetencyTypes == null || filteredTsIntTrainingAndCompetencyTypes.Count <= 0)
                    filteredTsIntTrainingAndCompetencyTypes = FilterRecord(tsIntTrainingAndCompetencyTypes, validationType);

                if (filteredTsIntTrainingAndCompetencyTypes?.Count > 0)
                {
                    GetIntTrainingAndCompetencyTypeDbInfo(filteredTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencyTypes);
                    IList<int> tsTypeIdNotExists = null;
                    var tsTypeIds = filteredTsIntTrainingAndCompetencyTypes.Select(x => x.Id).Distinct().ToList();
                    result = IsTsIntTrainingAndCompetencyTypeExistInDb(tsTypeIds, dbTsIntTrainingAndCompetencyTypes, ref tsTypeIdNotExists, ref validationMessages);
                    if (result)
                    {
                        result = IsIntTrainingAndCompetencyTypeCanBeRemove(dbTsIntTrainingAndCompetencyTypes, ref validationMessages);
                    }
                }
            }
            return result;
        }

        private void GetIntTrainingAndCompetencyTypeDbInfo(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTsIntTrainingAndCompetencyTypes,
                            ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes)
        {
            dbTsIntTrainingAndCompetencyTypes = dbTsIntTrainingAndCompetencyTypes ?? new List<DbModel.TechnicalSpecialistTrainingAndCompetencyType>();
            IList<int> tsCertificationIds = filteredTsIntTrainingAndCompetencyTypes?.Select(x => x.Id).Distinct().ToList();
            if (tsCertificationIds?.Count > 0 && (dbTsIntTrainingAndCompetencyTypes.Count <= 0 || dbTsIntTrainingAndCompetencyTypes.Any(x => !tsCertificationIds.Contains(x.Id))))
            {
                var tsInternalTrainingAndCompetencies = GetTsInternalTrainingAndCompetencyTypeById(tsCertificationIds);
                if (tsInternalTrainingAndCompetencies != null && tsInternalTrainingAndCompetencies.Any())
                {
                    dbTsIntTrainingAndCompetencyTypes.AddRange(tsInternalTrainingAndCompetencies);
                }
            }
        }

        private bool IsTsIntTrainingAndCompetencyTypeExistInDb(IList<int> tsTypeIds,
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                ref IList<int> tsTypeIdNotExists,
                                ref IList<ValidationMessage> validationMessages)
        {

            validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTsIntTrainingAndCompetencyTypes == null)
                dbTsIntTrainingAndCompetencyTypes = new List<DbModel.TechnicalSpecialistTrainingAndCompetencyType>();

            var validMessages = validationMessages;

            if (tsTypeIds?.Count > 0)
            {
                tsTypeIdNotExists = tsTypeIds.Where(id => !dbTsIntTrainingAndCompetencyTypes.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsTypeIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsIntTrainingAndCompetencyTypeIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsIntTrainingAndCompetencyTypeCanBeRemove(IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsIntTrainingAndCompetencyTypes?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x, MessageType.TsIntTrainingAndCompetencyTypeIsBeingUsed);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

    }

}
