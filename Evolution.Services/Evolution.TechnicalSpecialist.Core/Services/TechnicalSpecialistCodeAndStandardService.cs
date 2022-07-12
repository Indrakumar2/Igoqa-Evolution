using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
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
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistCodeAndStandardService : ITechnicalSpecialistCodeAndStandardService
    {

        private readonly IAppLogger<ITechnicalSpecialistCodeAndStandardRepository> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistCodeAndStandardRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistCodeAndStandardValidationService _validationService = null;
        private readonly IMasterService _masterService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ICodeStandardService _codeStandardService = null;
        //private readonly ITechnicalSpecialistService _technicalSpecialistInfoServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;

        #region Constructor
        public TechnicalSpecialistCodeAndStandardService(IMapper mapper,
                                                    ITechnicalSpecialistCodeAndStandardRepository repository,
                                                    IAppLogger<ITechnicalSpecialistCodeAndStandardRepository> logger,
                                                    ITechnicalSpecialistCodeAndStandardValidationService validationService,
                                                    IMasterService masterservice,
                                                    ICodeStandardService codeStandardService,
                                                    IAuditSearchService auditSearchService,
                                                    //ITechnicalSpecialistService technicalSpecialistInfoServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    JObject messages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            _masterService = masterservice;
            _codeStandardService = codeStandardService;
            //_technicalSpecialistInfoServices = technicalSpecialistInfoServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _auditSearchService = auditSearchService;

        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistCodeAndStandardinfo searchModel)
        {
            IList<TechnicalSpecialistCodeAndStandardinfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCodeAndStandardinfo>>(_repository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinId(IList<string> pinIds)
        {
            IList<TechnicalSpecialistCodeAndStandardinfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCodeAndStandardinfo>>(GetCodeAndStandardInfoByPin(pinIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pinIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> CodeAndStandardIds)
        {
            IList<TechnicalSpecialistCodeAndStandardinfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCodeAndStandardinfo>>(GetCodeAndStandardById(CodeAndStandardIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), CodeAndStandardIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> CodeAndStandard)
        {
            IList<TechnicalSpecialistCodeAndStandardinfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCodeAndStandardinfo>>(GetCodeAndStandardInfotByCodeStandardName(CodeAndStandard));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), CodeAndStandard);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCodeAndStandard,
                                    ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<KeyValuePair<string, string>> tsPinAndCodeAndStandardsNotExists = null;
            return IsRecordExistInDb(tsPinAndCodeAndStandard, ref dbtsCodeAndStandardInfos, ref tsPinAndCodeAndStandardsNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCodeAndStandard,
                                   ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                                   ref IList<KeyValuePair<string, string>> tsPinAndCodeAndStandardsNotExists,
                                   ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbtsCodeAndStandardInfos == null && (tsPinAndCodeAndStandard != null && tsPinAndCodeAndStandard.Any()))
                    dbtsCodeAndStandardInfos = GetCodeAndStandardInfoByPin(tsPinAndCodeAndStandard.Select(x => x.Key).ToList());

                result = IsCodeAndStandardInfoExistInDb(tsPinAndCodeAndStandard, dbtsCodeAndStandardInfos, ref tsPinAndCodeAndStandardsNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPinAndCodeAndStandard);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCodes = null;

            return AddTechSpecialistCodes(tsCodeAndStandardInfos, ref dbTsCodeAndStandardInfos, ref dbTechnicalSpecialists, ref dbCodes, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                        ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCodes,
                        bool commitChange = true,
                        bool isDbValidationRequire = true)
        {
            return AddTechSpecialistCodes(tsCodeAndStandardInfos, ref dbtsCodeAndStandardInfos, ref dbTechnicalSpecialists, ref dbCodes, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCodes = null;

            return UpdateTechSpecialistCodes(tsCodeAndStandardInfos, ref dbTsCodeAndStandardInfos, ref dbTechnicalSpecialists, ref dbCodes, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                               ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                               ref IList<DbModel.Data> dbCodes,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            return UpdateTechSpecialistCodes(tsCodeAndStandardInfos, ref dbtsCodeAndStandardInfos, ref dbTechnicalSpecialists, ref dbCodes, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                              bool commitChange = true,
                              bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTstCodeAndStandards = null;
            return RemoveTechSpecialistCodes(tsCodeAndStandardInfos, ref dbTstCodeAndStandards, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
              ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTstCodeAndStandards,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            return RemoveTechSpecialistCodes(tsCodeAndStandardInfos, ref dbTstCodeAndStandards, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeInfos,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCodes = null;
            return IsRecordValidForProcess(tsCodeInfos, validationType, ref dbTechnicalSpecialists, ref dbCodes, ref dbTsCodeInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCodes,
                                                ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeInfos,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistCodeAndStandardinfo> filteredTechSpecialist = null;

            return CheckRecordValidForProcess(tsCodeInfos, validationType, ref filteredTechSpecialist, ref dbTsCodeInfos, ref dbCodes, ref dbTechnicalSpecialists, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeInfos,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeInfos)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCodes = null;
            return IsRecordValidForProcess(tsCodeInfos, validationType, ref dbTechnicalSpecialists, ref dbCodes, ref dbTsCodeInfos);
        }
        #endregion


        #endregion

        #region Private Metods

        #region Get

        private IList<DbModel.TechnicalSpecialistCodeAndStandard> GetCodeAndStandardInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos = null;
            if (pins != null && pins.Any())
            {
                dbtsCodeAndStandardInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsCodeAndStandardInfos;
        }

        private IList<DbModel.TechnicalSpecialistCodeAndStandard> GetCodeAndStandardById(IList<int> tsCodeAndStandardIds)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos = null;
            if (tsCodeAndStandardIds != null && tsCodeAndStandardIds.Any())
                dbtsCodeAndStandardInfos = _repository.FindBy(x => tsCodeAndStandardIds.Contains(x.Id)).ToList();

            return dbtsCodeAndStandardInfos;
        }

        private IList<DbModel.TechnicalSpecialistCodeAndStandard> GetCodeAndStandardInfotByCodeStandardName(IList<string> tsCodeAndStandard)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos = null;
            if (tsCodeAndStandard != null && tsCodeAndStandard.Any())
                dbtsCodeAndStandardInfos = _repository.FindBy(x => tsCodeAndStandard.Contains(x.CodeStandard.Name.ToString())).ToList();

            return dbtsCodeAndStandardInfos;
        }

        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                         ref IList<TechnicalSpecialistCodeAndStandardinfo> filteredTechSpecialistCodeAndStandard,
                                         ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbCodeAndStandardInfos,
                                         ref IList<DbModel.Data> dbCodeAndStandard,
                                         ref IList<DbModel.TechnicalSpecialist> dbtechnicalspecialist,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCodeAndStandardInfos != null && (tsCodeAndStandardInfos != null && tsCodeAndStandardInfos.Any()))
            {
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTechSpecialistCodeAndStandard == null || filteredTechSpecialistCodeAndStandard.Count <= 0)
                    filteredTechSpecialistCodeAndStandard = FilterRecord(tsCodeAndStandardInfos, validationType);

                if (filteredTechSpecialistCodeAndStandard != null && filteredTechSpecialistCodeAndStandard.Any() && IsValidPayload(filteredTechSpecialistCodeAndStandard, validationType, ref validationMessages))
                {
                    IList<KeyValuePair<string, string>> tsCodesNotExists = null;

                    IList<string> codeStandardName = filteredTechSpecialistCodeAndStandard.Select(x => x.CodeStandardName).ToList();
                    IList<string> epins = filteredTechSpecialistCodeAndStandard.Select(x => x.Epin.ToString()).ToList();
                    GetMasterData(filteredTechSpecialistCodeAndStandard, ref dbCodeAndStandard);
                    result = _codeStandardService.IsValidCodeAndStandardName(codeStandardName, ref dbCodeAndStandard, ref validationMessages);
                    if (result && (epins != null && epins.Any()))
                    {
                        //result = Convert.ToBoolean(_technicalSpecialistInfoServices.IsRecordExistInDb(epins, ref dbtechnicalspecialist, ref validationMessages, tsca => tsca.TechnicalSpecialistCodeAndStandard).Result);
                        result = IsTechSpecialistExistInDb(epins, ref dbtechnicalspecialist, ref validationMessages, tsca => tsca.TechnicalSpecialistCodeAndStandard);
                    }

                    if (result)
                    {
                        dbCodeAndStandardInfos = dbtechnicalspecialist.SelectMany(x => x.TechnicalSpecialistCodeAndStandard).ToList();
                        var tsPinAnCodes = tsCodeAndStandardInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.CodeStandardName))
                                                        .Distinct()
                                                        .ToList();
                        result = IsCodeAndStandardInfoExistInDb(tsPinAnCodes, dbCodeAndStandardInfos, ref tsCodesNotExists, ref validationMessages);

                    }
                }
            }
            return result;
        }

        private Response AddTechSpecialistCodes(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                                ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodesInfos,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCodes,
                                                bool commitChange = true,
                                                bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Data> dbCodeAndStandard = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalspecialist = null;
                IList<TechnicalSpecialistCodeAndStandardinfo> recordToBeAdd = null;
                eventId = tsCodeAndStandardInfos?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsCodeAndStandardInfos, ValidationType.Add, ref recordToBeAdd, ref dbTsCodesInfos, ref dbCodes, ref dbTechnicalSpecialists);

                if (!isDbValidationRequire && tsCodeAndStandardInfos != null && tsCodeAndStandardInfos.Any())
                {
                    recordToBeAdd = FilterRecord(tsCodeAndStandardInfos, ValidationType.Add);
                }

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse?.Result))
                {
                    dbCodeAndStandard = dbCodes;
                    dbTechnicalspecialist = dbTechnicalSpecialists;
                    _repository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistCodeAndStandard>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbCodeAndStandards"] = dbCodeAndStandard;
                        opt.Items["DbTechnicalspecialists"] = dbTechnicalspecialist;
                    });

                    _repository.Add(mappedRecords);

                    if (commitChange)
                        _repository.ForceSave();
                    dbTsCodesInfos = mappedRecords;
                    if (mappedRecords?.Count > 0 && recordToBeAdd != null && recordToBeAdd.Any())
                    {
                        mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsCodeAndStandardInfos?.FirstOrDefault()?.ActionByUser,
                                                                                           null,
                                                                                           ValidationType.Add.ToAuditActionType(),
                                                                                           SqlAuditModuleType.TechnicalSpecialistCodeAndStandard,
                                                                                             null,
                                                                                          _mapper.Map<TechnicalSpecialistCodeAndStandardinfo>(x1)
                                                                                           ));
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCodeAndStandardInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateTechSpecialistCodes(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                                   ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodesInfos,
                                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                   ref IList<DbModel.Data> dbCodes,
                                                   bool commitChange = true,
                                                   bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Data> dbCodeAndStandard = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalspecialist = null;
                var recordToBeModify = FilterRecord(tsCodeAndStandardInfos, ValidationType.Update);
                bool valdResult = false;
                eventId = tsCodeAndStandardInfos?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsCodeAndStandardInfos, ValidationType.Update, ref recordToBeModify, ref dbTsCodesInfos, ref dbCodeAndStandard, ref dbTechnicalspecialist);
                if (!isDbValidationRequire && tsCodeAndStandardInfos != null && tsCodeAndStandardInfos.Any())
                {
                    recordToBeModify = FilterRecord(tsCodeAndStandardInfos, ValidationType.Update);
                }
                if (recordToBeModify != null && recordToBeModify.Any())
                {
                    if (dbTsCodesInfos == null || dbTsCodesInfos?.Count <= 0)
                        dbTsCodesInfos = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult))
                    {
                        //valdResult = Convert.ToBoolean(_technicalSpecialistInfoServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }
                    if (dbCodes == null || (dbCodes?.Count <= 0 && !valdResult))
                    {
                        _codeStandardService.IsValidCodeAndStandardName(recordToBeModify.Select(x => x.CodeStandardName).ToList(), ref dbCodes, ref validationMessages);
                    }

                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbTsCodesInfos != null && dbTsCodesInfos.Any()))
                    {
                        dbTechnicalspecialist = dbTechnicalSpecialists;
                        dbCodeAndStandard = dbCodes;
                        IList<TechnicalSpecialistCodeAndStandardinfo> domExsistingTechSplCodeAndStandaradInfo = new List<TechnicalSpecialistCodeAndStandardinfo>();
                        
                        dbTsCodesInfos.ToList().ForEach(dbTsCodeInfos =>
                        {
                            domExsistingTechSplCodeAndStandaradInfo.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistCodeAndStandardinfo>(dbTsCodeInfos)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == dbTsCodeInfos.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, dbTsCodeInfos, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbCodeAndStandards"] = dbCodeAndStandard;
                                    opt.Items["DbTechnicalspecialists"] = dbTechnicalspecialist;

                                });
                                dbTsCodeInfos.LastModification = DateTime.UtcNow;
                                dbTsCodeInfos.UpdateCount = dbTsCodeInfos.UpdateCount.CalculateUpdateCount();
                                dbTsCodeInfos.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTsCodesInfos);
                        if (commitChange)
                        {
                            _repository.ForceSave();
                            recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                 null,
                                                                                                 ValidationType.Update.ToAuditActionType(),
                                                                                                SqlAuditModuleType.TechnicalSpecialistCodeAndStandard,
                                                                                                domExsistingTechSplCodeAndStandaradInfo?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                                                x1
                                                                                                ));
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbTsCodesInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                            ref IList<TechnicalSpecialistCodeAndStandardinfo> filteredTsInfos,
                                            ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodesInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbcodes,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {


            bool result = false;
            if (tsCodeAndStandardInfos != null && tsCodeAndStandardInfos.Any())
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInfos == null || filteredTsInfos.Count <= 0)
                    filteredTsInfos = FilterRecord(tsCodeAndStandardInfos, validationType);

                if (filteredTsInfos != null && filteredTsInfos.Any() && IsValidPayload(filteredTsInfos, validationType, ref messages))
                {
                    GetTsCodesDbInfo(filteredTsInfos, true, ref dbTsCodesInfos);
                    IList<int> tsIds = filteredTsInfos.Select(x => x.Id).ToList();
                    IList<int> tsDBCodesIds = dbTsCodesInfos.Select(x => x.Id).ToList();
                    if (tsIds.Any(x => !tsDBCodesIds.Contains(x))) //Invalid Id found.
                    {
                        var dbTsCodeInfosByIds = dbTsCodesInfos;
                        var idNotExists = tsIds.Where(id => !dbTsCodeInfosByIds.Any(ts => ts.Id == id)).ToList();
                        var tsCodeList = filteredTsInfos;
                        idNotExists?.ForEach(tsId =>
                        {
                            var ts = tsCodeList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, ts, MessageType.TsCodeUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsInfos, dbTsCodesInfos, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsInfos.Select(x => x.Epin.ToString()).ToList();
                            IList<string> codes = filteredTsInfos.Select(x => x.CodeStandardName).ToList();

                            if (tsEpin != null && tsEpin.Any())
                            {
                                //result = Convert.ToBoolean(_technicalSpecialistInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages, tsca => tsca.TechnicalSpecialistCodeAndStandard).Result);
                                result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages, tsca => tsca.TechnicalSpecialistCodeAndStandard);
                            }
                            if (result && codes != null && codes.Any())
                                result = _codeStandardService.IsValidCodeAndStandardName(codes, ref dbcodes, ref validationMessages);
                            if (result)
                            {
                                IList<DbModel.TechnicalSpecialistCodeAndStandard> dbCodeAndStandardInfos = null;
                                IList<KeyValuePair<string, string>> tsCodesNotExists = null;
                                dbCodeAndStandardInfos = dbTechnicalSpecialists.SelectMany(x => x.TechnicalSpecialistCodeAndStandard).ToList();
                                var tsPinAnCodes = filteredTsInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.CodeStandardName))
                                                                .Distinct()
                                                                .ToList();
                                result = IsCodeAndStandardInfoExistInDb(tsPinAnCodes, dbCodeAndStandardInfos, ref tsCodesNotExists, ref validationMessages);
                            }

                        }

                    }
                }
                if (isDraft && result==false) //To handle reject TS changes and duplicate value validation
                {
                    result = true;
                    validationMessages.Clear();
                    messages?.Clear();
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages, params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString()), includes).ToList();

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

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                                    IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodesInfos,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsCodeAndStandardInfos.Where(x => !dbTsCodesInfos.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.CodeStandardName, MessageType.TsUpdatedByOther, x.CodeStandardName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodenfos,
                                            ref IList<TechnicalSpecialistCodeAndStandardinfo> filteredTsCodeInfos,
                                            ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsInfos,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCodenfos != null && tsCodenfos.Any())
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsCodeInfos == null || filteredTsCodeInfos.Count <= 0)
                    filteredTsCodeInfos = FilterRecord(tsCodenfos, validationType);

                if (filteredTsCodeInfos != null && filteredTsCodeInfos.Any() && IsValidPayload(filteredTsCodeInfos, validationType, ref validationMessages))
                {
                    GetTsCodesDbInfo(filteredTsCodeInfos, false, ref dbTsInfos);

                    IList<int> tsIdNotExists = null;
                    IList<int> tsids = filteredTsCodeInfos.Select(x => x.Id).Distinct().ToList();

                    result = IsCodeAndStandardInfoExistInDb(tsids, dbTsInfos, ref tsIdNotExists, ref validationMessages);
                    if (result)
                        result = IsTsCodeCanBeRemove(dbTsInfos, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsTsCodeCanBeRemove(IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeInfos,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsCodeInfos?.Where(x => x.IsAnyCollectionPropertyContainValue())
                            .ToList()
                            .ForEach(x =>
                            {
                                messages.Add(_messages, x.CodeStandard.Name, MessageType.TsCoderIsBeingUsed, x.CodeStandard.Name);
                            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion     

        #region Common

        private bool IsCodeAndStandardInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndCodeAndStandard,
                                          IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos,
                                          ref IList<KeyValuePair<string, string>> tsCodeExists,
                                          ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsCodeAndStandardInfos = dbTsCodeAndStandardInfos ?? new List<DbModel.TechnicalSpecialistCodeAndStandard>();

            var validMessages = validationMessages;

            if (tsPinAndCodeAndStandard != null && tsPinAndCodeAndStandard?.Count > 0)
            {
                tsCodeExists = tsPinAndCodeAndStandard.Where(info => dbTsCodeAndStandardInfos.Any(x1 => x1.TechnicalSpecialist.Pin == Convert.ToInt64(info.Key) && x1.CodeStandard.Name == info.Value))
                                                        .Select(x => x).ToList();

                tsCodeExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCodeAndStandardExist, x);
                });
            }

            if (validMessages.Any())
                validationMessages = validMessages;

            return validationMessages?.Count <= 0;
        }


        private bool IsCodeAndStandardInfoExistInDb(IList<int> tsCodeIds,
                                           IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos,
                                           ref IList<int> tsCodeIdNotExists,
                                           ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsCodeAndStandardInfos = dbTsCodeAndStandardInfos ?? new List<DbModel.TechnicalSpecialistCodeAndStandard>();

            var validMessages = validationMessages;

            if (tsCodeIds != null && tsCodeIds.Any())
            {
                tsCodeIdNotExists = tsCodeIds.Where(x => !dbTsCodeAndStandardInfos.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                tsCodeIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCodesDoesNotExist, x);
                });
            }

            if (validMessages.Any())
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void GetTsCodesDbInfo(IList<TechnicalSpecialistCodeAndStandardinfo> filteredTsCodesInfos,
                                       bool isTsCodeInfoById,
                                       ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsCodeAndStandardInfos)
        {
            IList<DbModel.TechnicalSpecialistCodeAndStandard> tsCodeAndStands = null;
            dbTsCodeAndStandardInfos = dbTsCodeAndStandardInfos ?? new List<DbModel.TechnicalSpecialistCodeAndStandard>();
            var tsPins = !isTsCodeInfoById ?
                            filteredTsCodesInfos.Select(x => x.Epin.ToString()).Distinct().ToList() :
                            null;
            IList<int> tsIds = isTsCodeInfoById ?
                                filteredTsCodesInfos.Select(x => x.Id).Distinct().ToList() :
                                null;
            if (tsPins != null && tsPins.Any() && (dbTsCodeAndStandardInfos.Count <= 0 || dbTsCodeAndStandardInfos.Any(x => !tsPins.Contains(x.TechnicalSpecialist.Pin.ToString()))))
            {
                tsCodeAndStands = GetCodeAndStandardInfoByPin(tsPins).ToList();
            }
            if (tsIds != null && tsIds.Any() && (dbTsCodeAndStandardInfos.Count <= 0 || dbTsCodeAndStandardInfos.Any(x => !tsIds.Contains(x.Id))))
            {
                tsCodeAndStands = GetCodeAndStandardById(tsIds).ToList();
            }
            if (tsCodeAndStands != null && tsCodeAndStands.Any())
            {
                dbTsCodeAndStandardInfos.AddRange(tsCodeAndStands);
            }
        }

        private IList<TechnicalSpecialistCodeAndStandardinfo> FilterRecord(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodesInfos,
                                                                 ValidationType filterType)
        {
            IList<TechnicalSpecialistCodeAndStandardinfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsCodesInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsCodesInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsCodesInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistCodeAndStandardinfo> ts,
                                 ValidationType validationType,
                                 ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(ts), validationType);
            if (validationResults != null && validationResults.Any())
            {
                messages.Add(_messages, ModuleType.Security, validationResults);
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;
        }

        private Response RemoveTechSpecialistCodes(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeInfos,
            ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTstCodeAndStandards,
                                                   bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCodes = null;
            long? eventId = 0;
            IList<TechnicalSpecialistCodeAndStandardinfo> recordToBeDeleted = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsCodeInfos?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsCodeInfos, ValidationType.Delete, ref dbTechnicalSpecialists, ref dbCodes, ref dbTstCodeAndStandards);

                if (tsCodeInfos != null && tsCodeInfos.Any())
                {
                    recordToBeDeleted = FilterRecord(tsCodeInfos, ValidationType.Delete);
                }

                if (recordToBeDeleted != null && recordToBeDeleted.Any() && (response == null || Convert.ToBoolean(response.Result)) && dbTstCodeAndStandards != null && dbTstCodeAndStandards.Any())
                {
                    var dbTsCodeToBeDeleted = dbTstCodeAndStandards?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsCodeToBeDeleted);
                    if (commitChange)
                    {
                        _repository.ForceSave();

                        recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                        null,
                                                                                                         ValidationType.Delete.ToAuditActionType(),
                                                                                                         SqlAuditModuleType.TechnicalSpecialistCodeAndStandard,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCodeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeInfos,
                                                   ValidationType validationType,
                                                   ref IList<TechnicalSpecialistCodeAndStandardinfo> filteredTsInfos,
                                                   ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTsInfos,
                                                   ref IList<DbModel.Data> dbcodeAndStandards,
                                                   ref IList<DbModel.TechnicalSpecialist> dbtechnicalspecialists,
                                                   bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsCodeInfos, ref filteredTsInfos, ref dbTsInfos, ref dbcodeAndStandards, ref dbtechnicalspecialists, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsCodeInfos, ref filteredTsInfos, ref dbTsInfos, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsCodeInfos, ref filteredTsInfos, ref dbTsInfos, ref dbtechnicalspecialists, ref dbcodeAndStandards, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCodeInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private void GetMasterData(IList<TechnicalSpecialistCodeAndStandardinfo> tsInfos,
                                                 ref IList<DbModel.Data> codeAndStandards)

        {
            var dbMaster = _masterService.Get(new List<MasterType>()
                    {
                        MasterType.CodeStandard
                    }, null, tsInfos?.Select(x => x.CodeStandardName)?.ToList());

            if (dbMaster != null && dbMaster.Any())
            {
                codeAndStandards = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.CodeStandard).ToList();
            }
        }

        #endregion



        #endregion

    }
}

