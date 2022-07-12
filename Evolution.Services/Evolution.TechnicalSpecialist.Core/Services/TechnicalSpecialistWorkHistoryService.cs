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
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistWorkHistoryService : ITechnicalSpecialistWorkHistoryService
    {

        private readonly IAppLogger<TechnicalSpecialistWorkHistoryService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistWorkHistoryRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistWorkHistoryValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService=null;
        //private readonly ITechnicalSpecialistService _tsInfoServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;

        #region Constructor
        public TechnicalSpecialistWorkHistoryService(IMapper mapper,
                                                    ITechnicalSpecialistWorkHistoryRepository repository,
                                                    IAppLogger<TechnicalSpecialistWorkHistoryService> logger,
                                                    ITechnicalSpecialistWorkHistoryValidationService validationService,
                                                    //ITechnicalSpecialistService tsInfoServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    JObject messages,IAuditSearchService auditSearchService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            //_tsInfoServices = tsInfoServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistWorkHistoryInfo searchModel)
        {
            IList<TechnicalSpecialistWorkHistoryInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistWorkHistoryInfo>>(_repository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinId(IList<string> pins)
        {
            IList<TechnicalSpecialistWorkHistoryInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistWorkHistoryInfo>>(GetWorkHistoryInfoByPin(pins));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> WorkHistoryIds)
        {
            IList<TechnicalSpecialistWorkHistoryInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistWorkHistoryInfo>>(GetWorkHistoryById(WorkHistoryIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), WorkHistoryIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }


        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistWorkHistory> dbTsInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return AddTsWorkHistory(tsWorkHistoryInfos, ref dbTsInfos,ref dbTechnicalSpecialists, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistWorkHistoryInfo> tstsWorkHistory, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTsWorkHistory(tstsWorkHistory, ref dbtsWorkHistory, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }
        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            //IList<DbModel.Data> dbCurrencies = null;

            return UpdateTechSpecialistWorkHistory(tsWorkHistory, ref dbtsWorkHistory, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistWorkHistoryInfo> tstsWorkHistory, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistWorkHistory(tstsWorkHistory, ref dbtsWorkHistory, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory = null;
            return RemoveTsWorkHistory(tsWorkHistory,ref dbtsWorkHistory, commitChange, isDbValidationRequired);
        }


        public Response Delete(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTsWorkHistory(tsWorkHistory,ref dbtsWorkHistory, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
              IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkhistory = null;
                return IsRecordValidForProcess(tsWorkHistory, validationType, ref dbTsWorkhistory, ref dbTechnicalSpecialists);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,ValidationType validationType, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool isDraft = false)
        {
            IList<TechnicalSpecialistWorkHistoryInfo> filteredTSPaySchedule = null;
            //IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkhistory = null;

            return CheckRecordValidForProcess(tsWorkHistory, validationType, ref filteredTSPaySchedule, ref dbtsWorkHistory, ref dbTechnicalSpecialists, isDraft);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, ValidationType validationType, IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            
             return IsRecordValidForProcess(tsWorkHistory, validationType,  ref dbtsWorkHistory, ref dbTechnicalSpecialists);

        }

        //public Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, ValidationType validationType)
        //{
        //    IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
        //    IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkhistory = null;
        //    return IsRecordValidForProcess(tsWorkHistory, validationType, ref dbTsWorkhistory, ref dbTechnicalSpecialists);
        //}

        //public Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbWorkhistory,ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists )
        //{
        //    IList<TechnicalSpecialistWorkHistoryInfo> filteredTSPaySchedule = null;           
        //    // IList<DbModel.Data> dbCurrencies = null;

        //    return this.CheckRecordValidForProcess(tsWorkHistory, validationType, ref filteredTSPaySchedule, ref dbWorkhistory, ref dbTechnicalSpecialists);
        //}

        //public Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkhistory, ValidationType validationType, IList<DbModel.TechnicalSpecialistWorkHistory> dbWorkhistory)
        //{
        //    IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
        //    //IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkhistory = null;
        //    return IsRecordValidForProcess(tsWorkhistory, validationType,  ref dbWorkhistory,ref dbTechnicalSpecialists);
        //}

        public Response IsRecordExistInDb(IList<int> tsPayScheduleIds, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsPaySchedules, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsPayScheduleIdNotExists = null;
            return IsRecordExistInDb(tsPayScheduleIds, ref dbTsPaySchedules, ref tsPayScheduleIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsWorkhistoryIds, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsPaySchedules, ref IList<int> tsPayScheduleIdNotExists, ref IList<ValidationMessage> validationMessages)
        {

            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsPaySchedules == null && tsWorkhistoryIds?.Count > 0)
                    dbTsPaySchedules = GetWorkHistoryInfoById(tsWorkhistoryIds);

                result = IsTSWorkHistoryExistInDb(tsWorkhistoryIds, dbTsPaySchedules, ref tsPayScheduleIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsWorkhistoryIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins,
                                              ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                              ref IList<ValidationMessage> validationMessages)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString())).ToList();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;
            var dbTechSpecs = dbTsInfos;

            if (tsPins?.Count > 0)
            {
                IList<string> tsPinNotExists = tsPins.Where(pin => !dbTechSpecs.Any(x1 => x1.Pin.ToString() == pin))
                                        .Select(pin => pin)
                                        .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        #endregion

        #region Get
        private IList<DbModel.TechnicalSpecialistWorkHistory> GetWorkHistoryInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistoryInfos = null;
            if (pins?.Count > 0)
            {
                dbtsWorkHistoryInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsWorkHistoryInfos;
        }

        private IList<DbModel.TechnicalSpecialistWorkHistory> GetWorkHistoryById(IList<int> tsWorkHistoryIds)
        {
            IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistoryInfos = null;
            if (tsWorkHistoryIds?.Count > 0)
                dbtsWorkHistoryInfos = _repository.FindBy(x => tsWorkHistoryIds.Contains((int)x.Id)).ToList();

            return dbtsWorkHistoryInfos;
        }

        #endregion

        #region Add


        private Response AddTsWorkHistory(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                                            ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistoryInfos,
                                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            bool commitChange = true,
                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
        
            try
            {
                eventId = tsWorkHistoryInfos?.FirstOrDefault().EventId;
                Response valdResponse = null;
                IList<TechnicalSpecialistWorkHistoryInfo> recordToBeAdd = null;

                IList<DbModel.TechnicalSpecialist> dbts = dbTechnicalSpecialists ?? new List<DbModel.TechnicalSpecialist>() ;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsWorkHistoryInfos,
                                                                ValidationType.Add,
                                                                ref recordToBeAdd,
                                                                ref dbTsWorkHistoryInfos,
                                                                ref dbts);
                if (tsWorkHistoryInfos?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsWorkHistoryInfos, ValidationType.Add);
                }

                if (!isDbValidationRequire ||(recordToBeAdd?.Count>0 && Convert.ToBoolean(valdResponse.Result)))
                {
                    _repository.AutoSave = false;  
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistWorkHistory>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechnicalspecialists"] = dbts;
                    });
                    _repository.Add(mappedRecords);
                    if (commitChange)
                    {

                       int value= _repository.ForceSave();
                        dbTsWorkHistoryInfos = mappedRecords;
                        if (mappedRecords?.Count > 0 && value>0)
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsWorkHistoryInfos?.FirstOrDefault().ActionByUser,
                                                                                               null,
                                                                                               ValidationType.Add.ToAuditActionType(),
                                                                                               SqlAuditModuleType.TechnicalSpecialistWorkHistory,
                                                                                               null,
                                                                                               _mapper.Map<TechnicalSpecialistWorkHistoryInfo>(x1)
                                                                                               ));
                    }
                     }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsWorkHistoryInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                                                    ValidationType validationType,
                                                    ref IList<TechnicalSpecialistWorkHistoryInfo> recordToBeAdd,
                                                    ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistoryInfos,
                                                    ref IList<DbModel.TechnicalSpecialist> dbts,
                                                    bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsWorkHistoryInfos, ref recordToBeAdd,
                                                    ref dbTsWorkHistoryInfos, ref dbts,
                                                    ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsWorkHistoryInfos,
                                                    ref recordToBeAdd,
                                                    ref dbTsWorkHistoryInfos,
                                                    ref dbts,
                                                    ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsWorkHistoryInfos,
                                                    ref recordToBeAdd,
                                                    ref dbTsWorkHistoryInfos,
                                                    ref dbts,
                                                    ref validationMessages,
                                                    isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsWorkHistoryInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsRecordValidForAdd(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                                        ref IList<TechnicalSpecialistWorkHistoryInfo> filteredtsWorkHistory,
                                        ref IList<DbModel.TechnicalSpecialistWorkHistory> dbWorkHistoryInfos, 
                                        ref IList<DbModel.TechnicalSpecialist> dbtechnicalspecialist,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsWorkHistoryInfos != null && tsWorkHistoryInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredtsWorkHistory == null || filteredtsWorkHistory.Count <= 0)
                    filteredtsWorkHistory = FilterRecord(tsWorkHistoryInfos, validationType);

                if (filteredtsWorkHistory?.Count > 0 && IsValidPayload(filteredtsWorkHistory, validationType, ref validationMessages))
                { 
                    IList<string> epins = filteredtsWorkHistory.Select(x => x.Epin.ToString()).ToList(); 
                    //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(epins, ref dbtechnicalspecialist, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(epins, ref dbtechnicalspecialist, ref validationMessages);
                }
            }
            return result;
        }
        #endregion

        #region  Delete
        private Response RemoveTsWorkHistory(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                                      ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory,
                                                       bool commitChange = true,
                                                       bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null; 
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecilist = null;
            IList<TechnicalSpecialistWorkHistoryInfo> recordToBeDeleted = null;
            long? eventId = 0;
           
            try
            {
                eventId = tsWorkHistory?.FirstOrDefault().EventId;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsWorkHistory, ValidationType.Delete, ref dbtsWorkHistory,ref dbTechnicalSpecilist);

                if (tsWorkHistory?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsWorkHistory, ValidationType.Delete);
                }
                 
                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbtsWorkHistory?.Count > 0) 
                {
                    var dbTsWorkHistoryToBeDeleted = dbtsWorkHistory?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsWorkHistoryToBeDeleted);
                    if (commitChange)
                    {
                      int value=  _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value>0)
                           
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                               null,
                                                                                                ValidationType.Delete.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.TechnicalSpecialistWorkHistory,
                                                                                                 x1,
                                                                                                 null));

                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsWorkHistory);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                           ref IList<TechnicalSpecialistWorkHistoryInfo> filteredTsWorkHistory,
                                           ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistory,
                                           ref IList<DbModel.TechnicalSpecialist> dbts,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsWorkHistory != null && tsWorkHistory.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsWorkHistory == null || filteredTsWorkHistory.Count <= 0)
                    filteredTsWorkHistory = FilterRecord(tsWorkHistory, validationType);

                if (filteredTsWorkHistory?.Count > 0 && IsValidPayload(filteredTsWorkHistory, validationType, ref validationMessages))
                {
                    GetTsWorkHistoryDbInfo(filteredTsWorkHistory, ref dbTsWorkHistory);
                    IList<int> tsPayScheduleIdNotExists = null;
                    var tsPayScheduleIds = filteredTsWorkHistory.Select(x => x.Id).Distinct().ToList();
                    result = IsTSWorkHistoryExistInDb(tsPayScheduleIds, dbTsWorkHistory, ref tsPayScheduleIdNotExists, ref validationMessages);

                    //if (result)
                    //    result = IsTechSpecialistPayScheduleCanBeRemove(dbTsWorkHistory, ref validationMessages);
                }
            }
            return result;
        }


        #endregion

        #region Common
        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                           IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistory,
                                           ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = tsWorkHistory.Where(x => !dbTsWorkHistory.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsWorkHistoryUpdatedByOther, x.Epin);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private bool IsCodeAndStandardInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndCodeAndStandard,
                                              IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos,
                                              ref IList<KeyValuePair<string, string>> tsCodeExists,
                                              ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsCodeAndStandardInfos == null)
                dbTsCodeAndStandardInfos = new List<DbModel.TechnicalSpecialistCodeAndStandard>();

            var validMessages = validationMessages;

            if (tsPinAndCodeAndStandard?.Count > 0)
            {
                tsCodeExists = tsPinAndCodeAndStandard.Where(info => dbTsCodeAndStandardInfos.Any(x1 => x1.TechnicalSpecialist.Pin == Convert.ToInt64(info.Key) && x1.CodeStandard.Name == info.Value))
                                                        .Select(x => x).ToList();

                tsCodeExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCodeAndStandardExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private bool IsCodeAndStandardInfoExistInDb(IList<int> tsCodeIds,
                                           IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos,
                                           ref IList<int> tsCodeIdNotExists,
                                           ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsCodeAndStandardInfos == null)
                dbTsCodeAndStandardInfos = new List<DbModel.TechnicalSpecialistCodeAndStandard>();

            var validMessages = validationMessages;

            if (tsCodeIds?.Count > 0)
            {
                tsCodeIdNotExists = tsCodeIds.Where(x => !dbTsCodeAndStandardInfos.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                tsCodeIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCodesDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
        private bool IsTSWorkHistoryExistInDb(IList<int> tsWorkHistoryIds,
                                         IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistory,
                                         ref IList<int> tsWorkHistoryIdNotExists,
                                         ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsWorkHistory == null)
                dbTsWorkHistory = new List<DbModel.TechnicalSpecialistWorkHistory>();

            var validMessages = validationMessages;

            if (tsWorkHistoryIds?.Count > 0)
            {
                tsWorkHistoryIdNotExists = tsWorkHistoryIds.Where(id => !dbTsWorkHistory.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsWorkHistoryIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsPayScheduleIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void GetTsWorkHistoryDbInfo(IList<TechnicalSpecialistWorkHistoryInfo> filteredWorkHistory,
                                        ref IList<DbModel.TechnicalSpecialistWorkHistory> dbWorkHistory)
        {
            dbWorkHistory = dbWorkHistory ?? new List<DbModel.TechnicalSpecialistWorkHistory>();
            IList<int> tsWorkHistoryIds = filteredWorkHistory?.Select(x => x.Id).Distinct().ToList();
            if (tsWorkHistoryIds?.Count > 0 && (dbWorkHistory.Count <= 0 || dbWorkHistory.Any(x=> !tsWorkHistoryIds.Contains(x.Id))))
            {
                var tsWorkHistories = GetWorkHistoryInfoById(tsWorkHistoryIds);
                if (tsWorkHistories != null && tsWorkHistories.Any())
                {
                    dbWorkHistory.AddRange(tsWorkHistories);
                }
            }    
        }

        private IList<DbModel.TechnicalSpecialistWorkHistory> GetWorkHistoryInfoById(IList<int> tsWorkhistoryIds)
        {

            IList<DbModel.TechnicalSpecialistWorkHistory> dbWorkHistoryInfos = null;
            if (tsWorkhistoryIds?.Count > 0)
                dbWorkHistoryInfos = _repository.FindBy(x => tsWorkhistoryIds.Contains((int)x.Id)).ToList();

            return dbWorkHistoryInfos;
        }


        private IList<TechnicalSpecialistWorkHistoryInfo> FilterRecord(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkhistoryInfos,
                                                                 ValidationType filterType)
        {
            IList<TechnicalSpecialistWorkHistoryInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsWorkhistoryInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsWorkhistoryInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsWorkhistoryInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                 ValidationType validationType,
                                 ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsWorkHistory), validationType);
            foreach (var item in tsWorkHistory)
            {
                if (!item.IsCurrentCompany && item.ToDate == null && item.RecordStatus != "D") //RecordStatus Condition Added for ITK QA D1335
                {
                    messages.Add(_messages, new { item.ToDate }, MessageType.TsPayRateExpectedToDate, item.ToDate);
                }
                if (item.FromDate != null && item.ToDate != null && item.RecordStatus != "D") //RecordStatus Condition Added for ITK QA D1335
                    if (item.ToDate < item.FromDate)
                        messages.Add(_messages, item.FromDate, MessageType.TsPayRateExpectedFrom, item.ToDate);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }
        #endregion

        #region modify
        private Response UpdateTechSpecialistWorkHistory(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                      ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkhistory,
                                      ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                      bool commitChange = true,
                                      bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistWorkHistoryInfo> recordToBeModify = null;
            long? eventId = 0;
           
            bool valdResult = true;
            try
            {
                eventId = tsWorkHistory?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsWorkHistory, ValidationType.Update, ref recordToBeModify, ref dbTsWorkhistory, ref dbTechnicalSpecialists);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (!isDbValidationRequired && tsWorkHistory?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsWorkHistory, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTsWorkhistory?.Count <= 0 )
                    {
                        dbTsWorkhistory = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists?.Count <= 0)
                    {
                        //valdResult = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }
                     
                    if (valdResult && dbTsWorkhistory?.Count > 0)
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        IList<TechnicalSpecialistWorkHistoryInfo> domExsistanceTsWorkHistory = new List<TechnicalSpecialistWorkHistoryInfo>();
                        
                        dbTsWorkhistory.ToList().ForEach(tsWorkHistoryInfo =>
                        {
                            domExsistanceTsWorkHistory.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistWorkHistoryInfo>(tsWorkHistoryInfo)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsWorkHistoryInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsWorkHistoryInfo, opt =>
                                {
                                    opt.Items["DbTechnicalspecialists"] = dbTechSpecialists;
                                });
                                tsWorkHistoryInfo.LastModification = DateTime.UtcNow;
                                tsWorkHistoryInfo.UpdateCount = tsWorkHistoryInfo.UpdateCount.CalculateUpdateCount();
                                tsWorkHistoryInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                           
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTsWorkhistory);
                        if (commitChange)
                        {
                          int value=  _repository.ForceSave();
                            if (recordToBeModify?.Count > 0 && value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                      null,
                                                                                                      ValidationType.Update.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.TechnicalSpecialistWorkHistory,
                                                                                                     domExsistanceTsWorkHistory?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsWorkHistory);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                           ref IList<TechnicalSpecialistWorkHistoryInfo> filteredTsWorkHistory,
                                           ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistory,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<ValidationMessage> validationMessages,
                                           bool isDraft = false)
        {
            bool result = false;
            if (tsWorkHistory != null && tsWorkHistory.Count > 0)
            {
                ValidationType validationType = ValidationType.Update; 
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsWorkHistory == null || filteredTsWorkHistory.Count <= 0)
                    filteredTsWorkHistory = FilterRecord(tsWorkHistory, validationType);

                if (filteredTsWorkHistory?.Count > 0 && IsValidPayload(filteredTsWorkHistory, validationType, ref validationMessages))
                {
                    GetTsWorkHistoryDbInfo(filteredTsWorkHistory, ref dbTsWorkHistory);

                    result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsWorkHistory, dbTsWorkHistory, ref validationMessages);
                    if (result)
                    {
                        IList<string> tsEpin = filteredTsWorkHistory.Select(x => x.Epin.ToString()).ToList();

                        //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    } 
                } 

            }
            return result;


            #endregion



        }
     }
}

#endregion


