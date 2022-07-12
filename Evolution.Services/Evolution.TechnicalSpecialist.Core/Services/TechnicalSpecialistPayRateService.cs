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
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistPayRateInfoService : ITechnicalSpecialistPayRateService
    {
        private readonly IAppLogger<TechnicalSpecialistPayRateInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistPayRateRepository _tsPayRateRepository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistPayRateValidationService _validationService = null;
        private readonly IExpenseType _expenseTypeService = null;
        //private readonly ITechnicalSpecialistService _technSpecService = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ITechnicalSpecialistPayScheduleService _tsPayScheduleService = null;
       private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor

        public TechnicalSpecialistPayRateInfoService(IMapper mapper,
                                                    ITechnicalSpecialistPayRateRepository repository,
                                                    IExpenseType expenseTypeService,
                                                    //ITechnicalSpecialistService technSpecService,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    ITechnicalSpecialistPayScheduleService tsPayScheduleService,
                                                    IAppLogger<TechnicalSpecialistPayRateInfoService> logger,
                                                    ITechnicalSpecialistPayRateValidationService validationService,
                                                    JObject messages, IAuditSearchService auditSearchService)
        {
            _tsPayRateRepository = repository;
            _expenseTypeService = expenseTypeService;
           // _technSpecService = technSpecService;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _tsPayScheduleService = tsPayScheduleService;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            _auditSearchService = auditSearchService;
        }

        #endregion

        public Response Get(DomainModel.TechnicalSpecialistPayRateInfo searchModel)
        {
            IList<TechnicalSpecialistPayRateInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistPayRateInfo>>(_tsPayRateRepository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> payRateIds)
        {
            IList<TechnicalSpecialistPayScheduleInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistPayScheduleInfo>>(GetPayRateInfoById(payRateIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), payRateIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<DbModel.TechnicalSpecialistPayRate> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DbModel.TechnicalSpecialistPayRate>>(GetPayRateInfoByPin(tsPins));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> payScheduleNames)
        {
            IList<DbModel.TechnicalSpecialistPayRate> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DbModel.TechnicalSpecialistPayRate>>(GetPayRateInfoByScheduleNames(payScheduleNames));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), payScheduleNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPayRate(IList<string> expenseTypes)
        {
            IList<DbModel.TechnicalSpecialistPayRate> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DbModel.TechnicalSpecialistPayRate>>(GetPayRateInfoByPayRate(expenseTypes));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), expenseTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }


        #region Get

        private IList<DbModel.TechnicalSpecialistPayRate> GetPayRateInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRateInfos = null;
            if (pins?.Count > 0)
            {
                dbTsPayRateInfos = _tsPayRateRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsPayRateInfos;
        }

        private IList<DbModel.TechnicalSpecialistPayRate> GetPayRateInfoById(IList<int> tsPayRateIds)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRateInfos = null;
            if (tsPayRateIds?.Count > 0)
                dbTsPayRateInfos = _tsPayRateRepository.FindBy(x => tsPayRateIds.Contains((int)x.Id)).ToList();

            return dbTsPayRateInfos;
        }

        private IList<DbModel.TechnicalSpecialistPayRate> GetPayRateInfoByScheduleNames(IList<string> tspayScheduleNames)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRateInfos = null;
            if (tspayScheduleNames?.Count > 0)
                dbTsPayRateInfos = _tsPayRateRepository.FindBy(x => tspayScheduleNames.Contains(x.PaySchedule.PayScheduleName)).ToList();

            return dbTsPayRateInfos;
        }

        private IList<DbModel.TechnicalSpecialistPayRate> GetPayRateInfoByPayRate(IList<string> expenseTypes)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRateInfos = null;
            if (expenseTypes?.Count > 0)
                dbTsPayRateInfos = _tsPayRateRepository.FindBy(x => expenseTypes.Contains(x.ExpenseType.Name)).ToList();

            return dbTsPayRateInfos;
        }

        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistPayRateInfo> tsPayRates, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules = null;
            IList<DbModel.Data> dbExpenseTypes = null;
            return AddTechSpecialistPayRate(tsPayRates,
                                            ref dbTsPayRates,
                                            ref dbTechnicalSpecialists,
                                            ref dbTsPaySchedules,
                                            ref dbExpenseTypes,
                                            commitChange,
                                            isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, ref IList<DbModel.Data> dbExpenseTypes, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialistPayRate(tsPayRates,
                                          ref dbTsPayRates,
                                          ref dbTechnicalSpecialists,
                                          ref dbTsPaySchedules,
                                          ref dbExpenseTypes,
                                          commitChange,
                                          isDbValidationRequired);
        }

        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistPayRateInfo> tsPayRates, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules = null;
            IList<DbModel.Data> dbExpenseTypes = null;
            return UpdateTechSpecialistPayRate(tsPayRates,
                                            ref dbTsPayRates,
                                            ref dbTechnicalSpecialists,
                                            ref dbTsPaySchedules,
                                            ref dbExpenseTypes,
                                            commitChange,
                                            isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, ref IList<DbModel.Data> dbExpenseTypes, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistPayRate(tsPayRates,
                                       ref dbTsPayRates,
                                       ref dbTechnicalSpecialists,
                                       ref dbTsPaySchedules,
                                       ref dbExpenseTypes,
                                       commitChange,
                                       isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistPayRateInfo> tsPayRates, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates = null;
            return RemoveTechSpecialistPayRate(tsPayRates, ref dbTsPayRates, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistPayRate(tsPayRates, ref dbTsPayRates, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates = null;
            return IsRecordValidForProcess(tsPayRates, validationType, ref dbTsPayRates);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates)
        {

            IList<TechnicalSpecialistPayRateInfo> filteredTSPayRates = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTSPaySchedules = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbExpenseTypes = null;

            return CheckRecordValidForProcess(tsPayRates, validationType, ref filteredTSPayRates, ref dbTsPayRates, ref dbTechnicalSpecialists, ref dbTSPaySchedules, ref dbExpenseTypes);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                                ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTSPaySchedules,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbExpenseTypes,
                                                bool isDraft = false)
        {

            IList<TechnicalSpecialistPayRateInfo> filteredTSPayRates = null;
            return CheckRecordValidForProcess(tsPayRates, validationType, ref filteredTSPayRates, ref dbTsPayRates, ref dbTechnicalSpecialists, ref dbTSPaySchedules, ref dbExpenseTypes, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ValidationType validationType, IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates)
        {
            return IsRecordValidForProcess(tsPayRates, validationType, ref dbTsPayRates);
        }

        public Response IsRecordExistInDb(IList<int> tsPayRateIds, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsPayRateIdNotExists = null;
            return IsRecordExistInDb(tsPayRateIds, ref dbTsPayRates, ref tsPayRateIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsPayRateIds, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates, ref IList<int> tsPayRateIdNotExists, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsPayRates == null && tsPayRateIds?.Count > 0)
                    dbTsPayRates = GetPayRateInfoById(tsPayRateIds);

                result = IsTSPayRateExistInDb(tsPayRateIds, dbTsPayRates, ref tsPayRateIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayRateIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #region Private Methods

        private Response AddTechSpecialistPayRate(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                                    ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                    ref IList<DbModel.Data> dbExpenseTypes,
                                                    bool commitChange = true,
                                                    bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistPayRateInfo> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                IList<DbModel.TechnicalSpecialistPaySchedule> dbTechSpecPaySchedules = null;
                IList<DbModel.Data> dbMasteExpenseTypes = null;
             
                eventId = tsPayRates?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsPayRates, ValidationType.Add, ref recordToBeAdd, ref dbTsPayRates, ref dbTechnicalSpecialists, ref dbTsPaySchedules, ref dbExpenseTypes);
                }

                if (!isDbValidationRequired && tsPayRates?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsPayRates, ValidationType.Add);
                }

                if ((!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result)) && recordToBeAdd?.Count > 0)
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbTechSpecPaySchedules = dbTsPaySchedules;
                    dbMasteExpenseTypes = dbExpenseTypes;

                    _tsPayRateRepository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistPayRate>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTSPaySchedule"] = dbTechSpecPaySchedules;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                        opt.Items["DbExpenseType"] = dbMasteExpenseTypes;
                    });
                    _tsPayRateRepository.Add(mappedRecords);

                    if (commitChange) 
                        _tsPayRateRepository.ForceSave();

                    dbTsPayRates = mappedRecords;
                    if (mappedRecords?.Count > 0)
                    {

                        mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsPayRates?.FirstOrDefault()?.ActionByUser,
                                                                                            null,
                                                                                             ValidationType.Add.ToAuditActionType(),
                                                                                           SqlAuditModuleType.TechnicalSpecialistPayRate,
                                                                                              null,
                                                                                                _mapper.Map<TechnicalSpecialistPayRateInfo>(x1)
                                                                                              ));
                    }


                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayRates);
            }
            finally
            {
                _tsPayRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response UpdateTechSpecialistPayRate(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                                    ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                    ref IList<DbModel.Data> dbExpenseTypes,
                                                    bool commitChange = true,
                                                    bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistPayRateInfo> recordToBeModify = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTechSpecPaySchedules = null;
            IList<DbModel.Data> dbMasterExpenseTypes = null;
            IList<DbModel.TechnicalSpecialistPayRate> dbPayRates = null;
            bool valdResult = true;
            long? eventId = 0;
        
            try
            {
                eventId = tsPayRates?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsPayRates, ValidationType.Update, ref recordToBeModify, ref dbTsPayRates, ref dbTechnicalSpecialists, ref dbTsPaySchedules, ref dbExpenseTypes);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (!isDbValidationRequired && tsPayRates?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsPayRates, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTsPayRates?.Count <= 0)
                    {
                        dbTsPayRates = _tsPayRateRepository.Get(recordToBeModify?.Select(x => (int)x.Id).ToList());
                    }
                    else
                    {
                        dbPayRates = dbTsPayRates.Where(x=> recordToBeModify.Select(x1=>x1.Id).Contains(x.Id)).ToList();
                    }

                    if (dbTechnicalSpecialists?.Count <= 0)
                    {
                        //valdResult = Convert.ToBoolean(_technSpecService.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if (dbTsPaySchedules == null || (dbTsPaySchedules?.Count <= 0 && !valdResult))
                    {
                        valdResult = Convert.ToBoolean(_tsPayScheduleService.IsRecordExistInDb(recordToBeModify.Select(x => (int)x.PayScheduleId).ToList(), ref dbTsPaySchedules, ref validationMessages).Result);
                    }

                    if (dbExpenseTypes?.Count <= 0)
                    {
                        valdResult = _expenseTypeService.IsValidExpenseType(recordToBeModify.Select(x => x.ExpenseType).ToList(), ref dbExpenseTypes, ref validationMessages);
                    }

                    if (!isDbValidationRequired || (valdResult && dbTsPaySchedules?.Count > 0))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbTechSpecPaySchedules = dbTsPaySchedules;
                        dbMasterExpenseTypes = dbExpenseTypes;
                        IList<TechnicalSpecialistPayRateInfo> domExsistancePayRateInfo = new List<TechnicalSpecialistPayRateInfo>();
                        
                        dbPayRates.ToList().ForEach(tsRateInfo =>
                        {
                            domExsistancePayRateInfo.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistPayRateInfo>(tsRateInfo)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsRateInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsRateInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                    opt.Items["DbTSPaySchedule"] = dbTechSpecPaySchedules;
                                    opt.Items["DbExpenseType"] = dbMasterExpenseTypes;
                                });
                                tsRateInfo.LastModification = DateTime.UtcNow;
                                tsRateInfo.UpdateCount = tsRateInfo.UpdateCount.CalculateUpdateCount();
                                tsRateInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            } 
                        });
                        _tsPayRateRepository.AutoSave = false;
                        _tsPayRateRepository.Update(dbPayRates);
                        if (commitChange)
                        {
                          int value=  _tsPayRateRepository.ForceSave();
                            if (recordToBeModify?.Count > 0 && value>0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                     null,
                                                                    ValidationType.Update.ToAuditActionType(),
                                                                     SqlAuditModuleType.TechnicalSpecialistPayRate,
                                                                    domExsistancePayRateInfo?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayRates);
            }
            finally
            {
                _tsPayRateRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialistPayRate(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
             ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                                        bool commitChange = true,
                                                        bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<TechnicalSpecialistPayRateInfo> recordToBeDeleted = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
           
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsPayRates?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(tsPayRates, ValidationType.Delete, ref dbTsPayRates);

                if (tsPayRates?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsPayRates, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsPayRates?.Count > 0)
                {

                    var dbTsPayRateToBeDeleted = dbTsPayRates?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _tsPayRateRepository.AutoSave = false;
                    _tsPayRateRepository.Delete(dbTsPayRateToBeDeleted);
                    if (commitChange)
                    {
                       int value= _tsPayRateRepository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value > 0)
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                  null,
                                                                                                 ValidationType.Delete.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.TechnicalSpecialistPayRate,
                                                                                                 x1,
                                                                                                 null
                                                                                                 ));
                        }
                    }

                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayRates);
            }
            finally
            {
                _tsPayRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                                    ValidationType validationType,
                                                    ref IList<TechnicalSpecialistPayRateInfo> filteredTsPayRates,
                                                    ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                    ref IList<DbModel.Data> dbExpenseTypes,
                                                    bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsPayRates, ref filteredTsPayRates, ref dbTsPayRates, ref dbTechnicalSpecialists, ref dbTsPaySchedules, ref dbExpenseTypes, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsPayRates, ref filteredTsPayRates, ref dbTsPayRates, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsPayRates, ref filteredTsPayRates, ref dbTsPayRates, ref dbTechnicalSpecialists, ref dbTsPaySchedules, ref dbExpenseTypes, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayRates);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsTSPayRateExistInDb(IList<int> tsPayScheduleIds,
                                               IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                               ref IList<int> tsPayRateIdNotExists,
                                               ref IList<ValidationMessage> validationMessages)
        { 
                validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTsPayRates == null)
                dbTsPayRates = new List<DbModel.TechnicalSpecialistPayRate>();

            var validMessages = validationMessages;

            if (tsPayScheduleIds?.Count > 0)
            {
                tsPayRateIdNotExists = tsPayScheduleIds.Where(id => !dbTsPayRates.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsPayRateIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsPayRateIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private IList<TechnicalSpecialistPayRateInfo> FilterRecord(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ValidationType filterType)
        {
            IList<TechnicalSpecialistPayRateInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsPayRates?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsPayRates?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsPayRates?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                       ref IList<TechnicalSpecialistPayRateInfo> filteredTsPayRates,
                                       ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                       ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                       ref IList<DbModel.Data> dbExpenseTypes,
                                       ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsPayRates != null && tsPayRates.Count > 0)
            {
                ValidationType validationType = ValidationType.Add; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsPayRates == null || filteredTsPayRates.Count <= 0)
                    filteredTsPayRates = FilterRecord(tsPayRates, validationType);

                if (filteredTsPayRates?.Count > 0 && IsValidPayload(filteredTsPayRates, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsPayRates.Select(x => x.Epin.ToString()).ToList();
                    IList<int> tsPayScheduleIds = filteredTsPayRates.Where(x => x.PayScheduleId > 0)?.Select(x => (int)x.PayScheduleId).ToList();
                    IList<string> expenseTypes = filteredTsPayRates.Select(x => x.ExpenseType).ToList();

                    //result = Convert.ToBoolean(_technSpecService.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    if (result && tsPayScheduleIds?.Count > 0)
                        result = Convert.ToBoolean(_tsPayScheduleService.IsRecordExistInDb(tsPayScheduleIds, ref dbTsPaySchedules, ref validationMessages).Result);
                    if (result && expenseTypes?.Count > 0)
                        result = _expenseTypeService.IsValidExpenseType(expenseTypes, ref dbExpenseTypes, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                       ref IList<TechnicalSpecialistPayRateInfo> filteredTsPayRates,
                                       ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                       ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                       ref IList<DbModel.Data> dbExpenseTypes,
                                       ref IList<ValidationMessage> validationMessages,
                                       bool isDraft = false)
        {
            bool result = false;
            if (tsPayRates != null && tsPayRates.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsPayRates == null || filteredTsPayRates.Count <= 0)
                    filteredTsPayRates = FilterRecord(tsPayRates, validationType);

                if (filteredTsPayRates?.Count > 0 && IsValidPayload(filteredTsPayRates, validationType, ref messages))
                {
                    GetTsPayRateDbInfo(filteredTsPayRates, ref dbTsPayRates);
                    IList<int> tsPayRateIds = filteredTsPayRates.Select(x => (int)x.Id).ToList();
                    IList<int> tsDBPayRateIds = dbTsPayRates.Select(x => x.Id).ToList();
                    if (tsPayRateIds.Any(x=> !tsDBPayRateIds.Contains(x))) //Invalid TechSpecialist Pay Rate Id found.
                    {
                        var dbTsInfosByIds = dbTsPayRates;
                        var idNotExists = tsPayRateIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistPayRateList = filteredTsPayRates;
                        idNotExists?.ForEach(tsrId =>
                        {
                            var tsPayRate = techSpecialistPayRateList.FirstOrDefault(x1 => x1.Id == tsrId);
                            messages.Add(_messages, tsPayRate, MessageType.TsPayRateUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsPayRates, dbTsPayRates, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsPayRates.Select(x => x.Epin.ToString()).ToList();
                            IList<int> tsPayScheduleIds = filteredTsPayRates.Select(x => (int)x.PayScheduleId).ToList();
                            IList<string> expenseTypes = filteredTsPayRates.Select(x => x.ExpenseType).ToList();

                            //result = Convert.ToBoolean(_technSpecService.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            if (result && tsPayScheduleIds?.Count > 0)
                                result = Convert.ToBoolean(_tsPayScheduleService.IsRecordExistInDb(tsPayScheduleIds, ref dbTsPaySchedules, ref validationMessages).Result);
                            if (result && expenseTypes?.Count > 0)
                                result = _expenseTypeService.IsValidExpenseType(expenseTypes, ref dbExpenseTypes, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }


        private bool IsRecordValidForRemove(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                            ref IList<TechnicalSpecialistPayRateInfo> filteredTsPayRates,
                                            ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsPayRates != null && tsPayRates.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsPayRates == null || filteredTsPayRates.Count <= 0)
                    filteredTsPayRates = FilterRecord(tsPayRates, validationType);

                if (filteredTsPayRates?.Count > 0 && IsValidPayload(filteredTsPayRates, validationType, ref validationMessages))
                {
                    GetTsPayRateDbInfo(filteredTsPayRates, ref dbTsPayRates);
                    IList<int> tsPayRateIdNotExists = null;
                    var tsPayRateIds = filteredTsPayRates.Select(x => (int)x.Id).Distinct().ToList();
                    result = IsTSPayRateExistInDb(tsPayRateIds, dbTsPayRates, ref tsPayRateIdNotExists, ref validationMessages);
                    if (result)
                        result = IsTechSpecialistPayRateCanBeRemove(dbTsPayRates, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                      ValidationType validationType,
                      ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsPayRates), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            foreach (var item in tsPayRates)
            {
                if (item.EffectiveFrom != null && item.EffectiveTo != null)
                   if(item.EffectiveTo < item.EffectiveFrom)
                        messages.Add(_messages, item.EffectiveFrom, MessageType.TsPayRateExpectedFrom, item.EffectiveTo);
            }

            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private void GetTsPayRateDbInfo(IList<TechnicalSpecialistPayRateInfo> filteredTsPayRates,
                                    ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates)
        {
            dbTsPayRates = dbTsPayRates ?? new List<DbModel.TechnicalSpecialistPayRate>();
            IList<int> tsPayRateIds = filteredTsPayRates?.Select(x => (int)x.Id).Distinct().ToList();
            if (tsPayRateIds?.Count > 0 && (dbTsPayRates.Count <= 0 || dbTsPayRates.Any(x=> !tsPayRateIds.Contains(x.Id))))
            {
                var tsPayRates = GetPayRateInfoById(tsPayRateIds);
                if (tsPayRates != null && tsPayRates.Any())
                {
                    dbTsPayRates.AddRange(tsPayRates);
                }
            }     
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                        IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                        ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var notMatchedRecords = tsPayRates.Where(x => !dbTsPayRates.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsPayRateUpdatedByOther, x.ExpenseType);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTechSpecialistPayRateCanBeRemove(IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            dbTsPayRates?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x.ExpenseType.Name, MessageType.TsPayRateIsBeingUsed, x.ExpenseType.Name);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages)
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

    }
}