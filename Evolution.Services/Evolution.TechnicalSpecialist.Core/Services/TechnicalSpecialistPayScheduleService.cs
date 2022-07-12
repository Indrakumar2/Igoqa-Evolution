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
    public class TechnicalSpecialistPayScheduleInfoService : ITechnicalSpecialistPayScheduleService
    {
        private readonly IAppLogger<TechnicalSpecialistPayScheduleInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistPayScheduleRepository _technSpecPayScheduleRepository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistPayScheduleValidationService _validationService = null;
        private readonly INativeCurrencyService _currencyService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IAuditSearchService _auditSearchService = null;
        
        #region Constructor

        public TechnicalSpecialistPayScheduleInfoService(IMapper mapper,
                                                    ITechnicalSpecialistPayScheduleRepository repository,
                                                    INativeCurrencyService currencyService,
                                                    //ITechnicalSpecialistService technSpecServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    IAppLogger<TechnicalSpecialistPayScheduleInfoService> logger,
                                                    ITechnicalSpecialistPayScheduleValidationService validationService,
                                                    JObject messages,
                                                   IAuditSearchService auditSearchService
                                                    )
        {
            _technSpecPayScheduleRepository = repository;
            _currencyService = currencyService;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            _auditSearchService = auditSearchService;
            
        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(DomainModel.TechnicalSpecialistPayScheduleInfo searchModel)
        {
            IList<TechnicalSpecialistPayScheduleInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistPayScheduleInfo>>(_technSpecPayScheduleRepository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> payscheduleIds)
        {
            IList<TechnicalSpecialistPayScheduleInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistPayScheduleInfo>>(GetPayScheduleInfoById(payscheduleIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), payscheduleIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<TechnicalSpecialistPayScheduleInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistPayScheduleInfo>>(GetPayScheduleInfoByPin(tsPins));
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
            IList<TechnicalSpecialistPayScheduleInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistPayScheduleInfo>>(GetPayScheduleInfoByScheduleNames(payScheduleNames));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), payScheduleNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Add 

        public Response Add(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCurrencies = null;
            return AddTechSpecialistPaySchedule(tsPaySchedules, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Data> dbCurrencies, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialistPaySchedule(tsPaySchedules, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCurrencies = null;

            return UpdateTechSpecialistPaySchedule(tsPaySchedules, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Data> dbCurrencies, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistPaySchedule(tsPaySchedules, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules = null;
            return RemoveTechSpecialistPaySchedule(tsPaySchedules,ref dbTsPaySchedules, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistPaySchedule(tsPaySchedules, ref dbTsPaySchedules,commitChange, isDbValidationRequired);
        }


        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules = null;
            return IsRecordValidForProcess(tsPaySchedules, validationType, ref dbTsPaySchedules);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules)
        {
            IList<TechnicalSpecialistPayScheduleInfo> filteredTSPaySchedule = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCurrencies = null;

            return CheckRecordValidForProcess(tsPaySchedules, validationType, ref filteredTSPaySchedule, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCurrencies, bool isDraft = false)
        {
            IList<TechnicalSpecialistPayScheduleInfo> filteredTSPaySchedule = null;
            return CheckRecordValidForProcess(tsPaySchedules, validationType, ref filteredTSPaySchedule, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ValidationType validationType, IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules)
        {
            return IsRecordValidForProcess(tsPaySchedules, validationType, ref dbTsPaySchedules);
        }

        public Response IsRecordExistInDb(IList<int> tsPayScheduleIds, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsPayScheduleIdNotExists = null;
            return IsRecordExistInDb(tsPayScheduleIds, ref dbTsPaySchedules, ref tsPayScheduleIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsPayScheduleIds, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules, ref IList<int> tsPayScheduleIdNotExists, ref IList<ValidationMessage> validationMessages)
        {

            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsPaySchedules == null && tsPayScheduleIds?.Count > 0)
                    dbTsPaySchedules = GetPayScheduleInfoById(tsPayScheduleIds);

                result = IsTSPayScheduleExistInDb(tsPayScheduleIds, dbTsPaySchedules, ref tsPayScheduleIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayScheduleIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #endregion

        #region Private Methods

        private IList<DbModel.TechnicalSpecialistPaySchedule> GetPayScheduleInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPayScheduleInfos = null;
            if (pins?.Count > 0)
            {
                dbTsPayScheduleInfos = _technSpecPayScheduleRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsPayScheduleInfos;
        }

        private IList<DbModel.TechnicalSpecialistPaySchedule> GetPayScheduleInfoById(IList<int> tsPayScheduleIds)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPayScheduleInfos = null;
            if (tsPayScheduleIds?.Count > 0)
                dbTsPayScheduleInfos = _technSpecPayScheduleRepository.FindBy(x => tsPayScheduleIds.Contains((int)x.Id)).ToList();

            return dbTsPayScheduleInfos;
        }

        private IList<DbModel.TechnicalSpecialistPaySchedule> GetPayScheduleInfoByScheduleNames(IList<string> tspayScheduleNames)
        {
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPayScheduleInfos = null;
            if (tspayScheduleNames?.Count > 0)
                dbTsPayScheduleInfos = _technSpecPayScheduleRepository.FindBy(x => tspayScheduleNames.Contains(x.PayScheduleName)).ToList();

            return dbTsPayScheduleInfos;
        }

        private Response AddTechSpecialistPaySchedule(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCurrencies,
                                            bool commitChange = true,
                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistPayScheduleInfo> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                eventId = tsPaySchedules?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                {
                    valdResponse = CheckRecordValidForProcess(tsPaySchedules, ValidationType.Add, ref recordToBeAdd, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies);
                }

                if (!isDbValidationRequire && tsPaySchedules?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsPaySchedules, ValidationType.Add);
                }

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;

                    _technSpecPayScheduleRepository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistPaySchedule>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                    });
                    _technSpecPayScheduleRepository.Add(mappedRecords);
                    if (commitChange)
                    {

                       int value= _technSpecPayScheduleRepository.ForceSave();
                       //D756,404
                        dbTsPaySchedules = dbTsPaySchedules ?? new List<DbModel.TechnicalSpecialistPaySchedule>();
                        dbTsPaySchedules.AddRange(mappedRecords);
                        if (mappedRecords?.Count > 0 && value > 0)
                        {
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsPaySchedules.FirstOrDefault().ActionByUser, null,
                                                                                               ValidationType.Add.ToAuditActionType(),
                                                                                               SqlAuditModuleType.TechnicalSpecialistPaySchedule,
                                                                                                null,
                                                                                                _mapper.Map<TechnicalSpecialistPayScheduleInfo>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPaySchedules);
            }
            finally
            {
                _technSpecPayScheduleRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistPaySchedule(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                       ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                       ref IList<DbModel.Data> dbCurrencies,
                                       bool commitChange = true,
                                       bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistPayScheduleInfo> recordToBeModify = null;
            bool valdResult = false;
            long? eventId = 0;
            IList <DbModel.TechnicalSpecialistPaySchedule> dbRecordtobeModified = new List<DbModel.TechnicalSpecialistPaySchedule>();
            try
            {
                eventId = tsPaySchedules?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsPaySchedules, ValidationType.Update, ref recordToBeModify, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (!isDbValidationRequired && tsPaySchedules?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsPaySchedules, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if ((dbTsPaySchedules == null || (dbTsPaySchedules?.Count <= 0 && valdResult == false)) && recordToBeModify?.Count > 0)
                    {
                        dbTsPaySchedules = _technSpecPayScheduleRepository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if ((dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && valdResult == false)) && recordToBeModify?.Count > 0)
                    {
                        //valdResult = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if ((dbCurrencies == null || (dbCurrencies?.Count <= 0 && valdResult == false)) && recordToBeModify?.Count > 0)
                    {
                        valdResult = _currencyService.IsValidCurrency(recordToBeModify.Select(x => x.PayCurrency).ToList(), ref dbCurrencies, ref validationMessages);
                    }

                    if (!isDbValidationRequired || (valdResult && dbTsPaySchedules?.Count > 0))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                         IList<TechnicalSpecialistPayScheduleInfo> domExsitanceTsPaySchedules = new List<TechnicalSpecialistPayScheduleInfo>();
                        
                        dbTsPaySchedules.ToList().ForEach(tsPayInfo =>
                        {
                            domExsitanceTsPaySchedules.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistPayScheduleInfo>(tsPayInfo)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsPayInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsPayInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                });
                                tsPayInfo.LastModification = DateTime.UtcNow;
                                tsPayInfo.UpdateCount = tsPayInfo.UpdateCount.CalculateUpdateCount();
                                tsPayInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                                dbRecordtobeModified.Add(tsPayInfo);
                            } 
                        });
                        _technSpecPayScheduleRepository.AutoSave = false;
                        if(dbRecordtobeModified?.Count > 0)
                        _technSpecPayScheduleRepository.Update(dbRecordtobeModified);
                        if (commitChange)
                        {
                           int value= _technSpecPayScheduleRepository.ForceSave();
                            if (recordToBeModify?.Count > 0 && value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                      null,
                                                                                                       ValidationType.Update.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.TechnicalSpecialistPaySchedule,
                                                                                                     domExsitanceTsPaySchedules?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPaySchedules);
            }
            finally
            {
                _technSpecPayScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialistPaySchedule(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                        bool commitChange = true,
                                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistPayScheduleInfo> recordToBeDeleted = null;
            long? eventId = 0;
            
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsPaySchedules?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsPaySchedules, ValidationType.Delete, ref dbTsPaySchedules);

                if (tsPaySchedules?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsPaySchedules, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsPaySchedules?.Count > 0)
                {
                    var dbTsPaySchedToBeDeleted = dbTsPaySchedules?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _technSpecPayScheduleRepository.AutoSave = false;
                    _technSpecPayScheduleRepository.Delete(dbTsPaySchedToBeDeleted);
                    if (commitChange)
                    {
                      int value =  _technSpecPayScheduleRepository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value>0)
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                 null,
                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.TechnicalSpecialistPaySchedule,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPaySchedules);
            }
            finally
            {
                _technSpecPayScheduleRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistPayScheduleInfo> filteredTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCurrencies,
                                            bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsPaySchedules, ref filteredTsPaySchedules, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsPaySchedules, ref filteredTsPaySchedules, ref dbTsPaySchedules, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsPaySchedules, ref filteredTsPaySchedules, ref dbTsPaySchedules, ref dbTechnicalSpecialists, ref dbCurrencies, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPaySchedules);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsRecordValidForAdd(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                       ref IList<TechnicalSpecialistPayScheduleInfo> filteredTsPaySchedules,
                                       ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                       ref IList<DbModel.Data> dbCurrencies,
                                       ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsPaySchedules != null && tsPaySchedules.Count > 0)
            {
                ValidationType validationType = ValidationType.Add; 
                    validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsPaySchedules == null || filteredTsPaySchedules.Count <= 0)
                    filteredTsPaySchedules = FilterRecord(tsPaySchedules, validationType);

                if (filteredTsPaySchedules?.Count > 0 && IsValidPayload(filteredTsPaySchedules, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsPaySchedules.Select(x => x.Epin.ToString()).ToList();
                    IList<string> currencies = filteredTsPaySchedules.Select(x => x.PayCurrency).ToList();
                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    if (result && currencies?.Count > 0)
                        result = _currencyService.IsValidCurrency(currencies, ref dbCurrencies, ref validationMessages);
                    //d980 - Validation is already handled in UI
                    //if (result)
                    //    result = IsTSPayScheduleUnique(filteredTsPaySchedules, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ref IList<TechnicalSpecialistPayScheduleInfo> filteredTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCurrencies,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {
            bool result = false;
            if (tsPaySchedules != null && tsPaySchedules.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                    validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsPaySchedules == null || filteredTsPaySchedules.Count <= 0)
                    filteredTsPaySchedules = FilterRecord(tsPaySchedules, validationType);

                if (filteredTsPaySchedules?.Count > 0 && IsValidPayload(filteredTsPaySchedules, validationType, ref messages))
                {
                    GetTsPayScheduleDbInfo(filteredTsPaySchedules, ref dbTsPaySchedules);
                    IList<int> tsPayScheduleIds = filteredTsPaySchedules.Select(x => x.Id).ToList();
                    IList<int> tsDbPayScheduleIds = dbTsPaySchedules.Select(x => x.Id).Distinct().ToList();
                    if (tsPayScheduleIds.Any(x=> !tsDbPayScheduleIds.Contains(x))) //Invalid TechSpecialist PaySchedule Id found.
                    {
                        var dbTsInfosByIds = dbTsPaySchedules;
                        var idNotExists = tsPayScheduleIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistPaySchedulesList = filteredTsPaySchedules;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsPaySchedule = techSpecialistPaySchedulesList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsPaySchedule, MessageType.TsPayScheduleUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsPaySchedules, dbTsPaySchedules, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsPaySchedules.Select(x => x.Epin.ToString()).ToList();
                            IList<string> currencies = filteredTsPaySchedules.Select(x => x.PayCurrency).ToList();
                            //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            if (result && currencies?.Count > 0)
                                result = _currencyService.IsValidCurrency(currencies, ref dbCurrencies, ref validationMessages);
                            // d980- Validation is already handled in UI
                            //if (result)
                            //    result = IsTSPayScheduleUnique(filteredTsPaySchedules, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }


        private bool IsRecordValidForRemove(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ref IList<TechnicalSpecialistPayScheduleInfo> filteredTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsPaySchedules != null && tsPaySchedules.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsPaySchedules == null || filteredTsPaySchedules.Count <= 0)
                    filteredTsPaySchedules = FilterRecord(tsPaySchedules, validationType);

                if (filteredTsPaySchedules?.Count > 0 && IsValidPayload(filteredTsPaySchedules, validationType, ref validationMessages))
                {
                    GetTsPayScheduleDbInfo(filteredTsPaySchedules, ref dbTsPaySchedules);
                    IList<int> tsPayScheduleIdNotExists = null;
                    var tsPayScheduleIds = filteredTsPaySchedules.Select(x => x.Id).Distinct().ToList();
                    result = IsTSPayScheduleExistInDb(tsPayScheduleIds, dbTsPaySchedules, ref tsPayScheduleIdNotExists, ref validationMessages);
                    if (result && tsPayScheduleIds?.Count() > 0)
                    { 
                        result = IsTechSpecialistPayScheduleCanBeRemove(dbTsPaySchedules?.Where(x=> tsPayScheduleIds.Contains(x.Id)).ToList(), ref validationMessages);
                    }
                }
            }
            return result;
        }

        private bool IsTSPayScheduleExistInDb(IList<int> tsPayScheduleIds,
                                                IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                ref IList<int> tsPayScheduleIdNotExists,
                                                ref IList<ValidationMessage> validationMessages)
        { 
                validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTsPaySchedules == null)
                dbTsPaySchedules = new List<DbModel.TechnicalSpecialistPaySchedule>();

            var validMessages = validationMessages;

            if (tsPayScheduleIds?.Count > 0)
            {
                tsPayScheduleIdNotExists = tsPayScheduleIds.Where(id => !dbTsPaySchedules.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsPayScheduleIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsPayScheduleIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialistPayScheduleCanBeRemove(IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            dbTsPaySchedules?.Where(x => x.IsAnyCollectionPropertyContainValue(new string[]{ "TechnicalSpecialistPayRate"}))
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x.PayScheduleName, MessageType.TsPayScheduleIsBeingUsed, x.PayScheduleName, x.PayCurrency);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private void GetTsPayScheduleDbInfo(IList<TechnicalSpecialistPayScheduleInfo> filteredTsPaySchedules,
                                            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules)
        {
            dbTsPaySchedules = dbTsPaySchedules ?? new List<DbModel.TechnicalSpecialistPaySchedule>();
            IList<int> tsPayScheduleIds = filteredTsPaySchedules?.Select(x => x.Id).Distinct().ToList();
            if (tsPayScheduleIds?.Count > 0 && (dbTsPaySchedules.Count <= 0 || !dbTsPaySchedules.Any(x=>tsPayScheduleIds.Contains(x.Id))))
            {
                var tsPaySchedules = GetPayScheduleInfoById(tsPayScheduleIds);
                if (tsPaySchedules != null && tsPaySchedules.Any())
                {
                    dbTsPaySchedules.AddRange(tsPaySchedules);
                }
            }
                
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                                IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var notMatchedRecords = tsPaySchedules.Where(x => !dbTsPaySchedules.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsPayScheduleUpdatedByOther, x.PayScheduleName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTSPayScheduleUnique(IList<TechnicalSpecialistPayScheduleInfo> filteredTsPaySchedules,
                                            ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ?? new List<ValidationMessage>();

            var tsPaySchedules = filteredTsPaySchedules.Select(x => new { x.Epin, x.PayScheduleName, x.PayCurrency, x.Id });
            var dbTsPaySchedules = _technSpecPayScheduleRepository.FindBy(x => tsPaySchedules.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.PayCurrency == x.PayCurrency && x1.PayScheduleName == x.PayScheduleName & x1.Id != x.Id)).ToList();
            if (dbTsPaySchedules?.Count > 0)
            {
                var tsPayScheduleAlreadyExist = filteredTsPaySchedules.Where(x => dbTsPaySchedules.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin && x.PayCurrency == x1.PayCurrency && x.PayScheduleName == x1.PayScheduleName));
                tsPayScheduleAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.TsPayScheduleAlreadyExist, x.Epin, x.PayScheduleName, x.PayCurrency);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private IList<TechnicalSpecialistPayScheduleInfo> FilterRecord(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, ValidationType filterType)
        {
            IList<TechnicalSpecialistPayScheduleInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsPaySchedules?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsPaySchedules?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsPaySchedules?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                              ValidationType validationType,
                              ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsPaySchedules), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        public bool IsValidPaySchedule(IList<int> payScheduleIds, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var dbPaySchedules = _technSpecPayScheduleRepository.FindBy(x => payScheduleIds.Contains(x.Id)).ToList();
            var invalidPaySchedules = payScheduleIds.Where(x => !dbPaySchedules.Any(x1 => x1.Id == x));
            invalidPaySchedules.ToList().ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsPayScheduleIsInvalid, x);
            });

            dbPaySchedule = dbPaySchedules;
            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
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