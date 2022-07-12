using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
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

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistStampInfoService : ITechnicalSpecialistStampInfoService
    {
        private readonly IAppLogger<TechnicalSpecialistStampInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistStampInfoRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistStampValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IDocumentService _documentService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ITechnicalSpecialistStampCountryCodeService _countryService = null;

        #region Constructor
        public TechnicalSpecialistStampInfoService(IMapper mapper,
                                                    ITechnicalSpecialistStampInfoRepository repository,
                                                    IAppLogger<TechnicalSpecialistStampInfoService> logger,
                                                    ITechnicalSpecialistStampValidationService validationService,
                                                    JObject messages,
                                                    //ITechnicalSpecialistService technSpecServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                     IDocumentService documentService,
                                                    ITechnicalSpecialistStampCountryCodeService countryService, IAuditSearchService auditSearchService
                                                    )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _countryService = countryService;
            _documentService = documentService;
            _auditSearchService = auditSearchService;


        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistStampInfo searchModel)
        {
            IList<TechnicalSpecialistStampInfo> result = null;
            Exception exception = null;
            try
            {

                var tsStamp = _mapper.Map<IList<TechnicalSpecialistStampInfo>>(_repository.Search(searchModel));
                result = PopulateTsStampsDocument(tsStamp);
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
            IList<TechnicalSpecialistStampInfo> result = null;
            Exception exception = null;
            try
            {
                var tsStamp = _mapper.Map<IList<TechnicalSpecialistStampInfo>>(GetStampInfoByPin(pins));
                result = PopulateTsStampsDocument(tsStamp);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> stampIds)
        {
            IList<TechnicalSpecialistStampInfo> result = null;
            Exception exception = null;
            try
            {
                var tsStamp = _mapper.Map<IList<TechnicalSpecialistStampInfo>>(GetStampInfotById(stampIds));
                result = PopulateTsStampsDocument(tsStamp);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), stampIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> stampNumbers)
        {
            IList<TechnicalSpecialistStampInfo> result = null;
            Exception exception = null;
            try
            {
                var tsStamp = _mapper.Map<IList<TechnicalSpecialistStampInfo>>(GetStampInfotByStampNumber(stampNumbers));
                result = PopulateTsStampsDocument(tsStamp);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), stampNumbers);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumber,
                                            ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<KeyValuePair<string, string>> tsPinAndStampNumberNotExists = null;
            return IsRecordExistInDb(tsPinAndStampNumber, ref dbTsStampInfos, ref tsPinAndStampNumberNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumbers,
                                            ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                            ref IList<KeyValuePair<string, string>> tsPinAndStampNumberNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsStampInfos == null && tsPinAndStampNumbers?.Count > 0)
                    dbTsStampInfos = GetStampInfoByPin(tsPinAndStampNumbers.Select(x => x.Key).ToList());

                result = IsStampInfoExistInDb(tsPinAndStampNumbers, dbTsStampInfos, ref tsPinAndStampNumberNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPinAndStampNumbers);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbTsStampCountries = null;
            return AddTechSpecialistStamp(tsStampInfos, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbTsStampCountries, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                            ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                            ref IList<DbModel.Data> dbTsStampCountries,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddTechSpecialistStamp(tsStampInfos, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbTsStampCountries, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbTsStampCountries = null;
            return UpdateTechSpecialistStamp(tsStampInfos, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbTsStampCountries, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                ref IList<DbModel.Data> dbTsStampCountries,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            return UpdateTechSpecialistStamp(tsStampInfos, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbTsStampCountries, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete
        public Response Delete(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            return RemoveTechSpecialistStamp(tsStampInfos, ref dbTsStampInfos, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<TechnicalSpecialistStampInfo> tsStampInfos,
               ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                             bool commitChange = true,
                             bool isDbValidationRequire = true)
        {
            return RemoveTechSpecialistStamp(tsStampInfos, ref dbTsStampInfos, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCountries = null;
            return IsRecordValidForProcess(tsStampInfos, validationType, ref dbTechnicalSpecialists, ref dbCountries, ref dbTsStampInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCountries,
                                                ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistStampInfo> filteredTechSpecialist = null;
            return CheckRecordValidForProcess(tsStampInfos, validationType, ref filteredTechSpecialist, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbCountries, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCountries = null;
            return IsRecordValidForProcess(tsStampInfos, validationType, ref dbTechnicalSpecialists, ref dbCountries, ref dbTsStampInfos);
        }
        #endregion

        #endregion

        #region Private Metods

        #region Get
        private IList<DbModel.TechnicalSpecialistStamp> GetStampInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            if (pins?.Count > 0)
            {
                dbTsStampInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsStampInfos;
        }

        private IList<DbModel.TechnicalSpecialistStamp> GetStampInfotById(IList<int> tsStampIds)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            if (tsStampIds?.Count > 0)
                dbTsStampInfos = _repository.FindBy(x => tsStampIds.Contains(x.Id)).ToList();

            return dbTsStampInfos;
        }

        private IList<DbModel.TechnicalSpecialistStamp> GetStampInfotByStampNumber(IList<string> tsStampNumbers)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos = null;
            if (tsStampNumbers?.Count > 0)
                dbTsStampInfos = _repository.FindBy(x => tsStampNumbers.Contains(x.StampNumber.ToString())).ToList();

            return dbTsStampInfos;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                         ref IList<TechnicalSpecialistStampInfo> filteredTechSpecialistStamp,
                                         ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbCountries,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsStampInfos != null && tsStampInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTechSpecialistStamp == null || filteredTechSpecialistStamp.Count <= 0)
                    filteredTechSpecialistStamp = FilterRecord(tsStampInfos, validationType);

                if (filteredTechSpecialistStamp?.Count > 0 && IsValidPayload(filteredTechSpecialistStamp, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTechSpecialistStamp.Select(x => x.Epin.ToString()).ToList();

                    if (tsEpin?.Count > 0)
                    {
                        //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    }
                    if (result)
                        result = IsMasterDataValid(filteredTechSpecialistStamp, ref dbCountries, ref validationMessages);
                    if (result)
                    {
                        result = IsTSStampUnique(filteredTechSpecialistStamp, ref validationMessages);
                    }
                    if (result)
                    {
                        result = IsStampNumberValidation(filteredTechSpecialistStamp, ref validationMessages);
                    }

                }
            }
            return result;
        }

        private Response AddTechSpecialistStamp(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                           ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.Data> dbTsStampCountries,
                                           bool commitChange = true,
                                           bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbTsStampCountryCodes = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistStampInfo> recordToBeAdd = null;
                eventId = tsStampInfos?.FirstOrDefault().EventId;

                if (tsStampInfos?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsStampInfos, ValidationType.Add);
                }
                if (recordToBeAdd?.Count > 0)
                {
                    if (isDbValidationRequire)
                    {
                        valdResponse = CheckRecordValidForProcess(tsStampInfos, ValidationType.Add, ref recordToBeAdd, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbTsStampCountries);
                    }
                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                    {
                        _repository.AutoSave = false;
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbTsStampCountryCodes = dbTsStampCountries;

                        var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistStamp>>(recordToBeAdd, opt =>
                        {
                            opt.Items["isAssignId"] = false;
                            opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                            opt.Items["DBTsStampCountryCodes"] = dbTsStampCountryCodes;

                        });
                        _repository.Add(mappedRecords);
                        if (commitChange)
                        {
                            var savedCnt = _repository.ForceSave();
                            dbTsStampInfos = mappedRecords;
                            if (savedCnt > 0)
                            {
                                ProcessTsStampDocuments(recordToBeAdd, mappedRecords, ref validationMessages);
                            }

                            if (mappedRecords?.Count > 0 && savedCnt > 0)
                            {
                                int i = 0;
                                recordToBeAdd?.ToList().ForEach(x1 =>
                                {
                                    var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                    if (newDocuments != null && newDocuments.Count > 0)
                                        x1.DocumentName = string.Join(",", newDocuments);

                                    x1.Id = mappedRecords[i++].Id;// def1035 test 2
                                    _auditSearchService.AuditLog(x1, ref eventId, tsStampInfos.FirstOrDefault().ActionByUser, null,
                                                              ValidationType.Add.ToAuditActionType(),
                                                             SqlAuditModuleType.TechnicalSpecialistStamp,
                                                             null,
                                                            x1);
                                });
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStampInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateTechSpecialistStamp(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                              ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                              ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                              ref IList<DbModel.Data> dbTsStampCountries,
                                              bool commitChange = true,
                                              bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbTsStampCountryCodes = null;
            var recordToBeModify = FilterRecord(tsStampInfos, ValidationType.Update);
            Response valdResponse = null;
            bool valdResult = false;
            long? eventId = null;

            try
            {
                eventId = tsStampInfos?.FirstOrDefault().EventId;

                if (tsStampInfos?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsStampInfos, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (isDbValidationRequire)
                    {
                        valdResponse = CheckRecordValidForProcess(tsStampInfos, ValidationType.Update, ref recordToBeModify, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbTsStampCountries);
                        valdResult = Convert.ToBoolean(valdResponse.Result);
                    }
                     
                    if ((dbTsStampInfos == null || dbTsStampInfos?.Count <= 0) && recordToBeModify?.Count > 0)
                    dbTsStampInfos = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());

                    if ((dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult)) && recordToBeModify?.Count > 0)
                    {
                        //valdResult = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }
                    if ((dbTsStampCountries == null || (dbTsStampCountries?.Count <= 0 && !valdResult)) && recordToBeModify?.Count > 0)
                    {
                        _countryService.IsValidTechnicalSpecialistStampCountryCode(recordToBeModify.Select(x => x.CountryName).ToList(), ref dbTsStampCountries, ref validationMessages);
                    }

                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbTsStampInfos?.Count > 0))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbTsStampCountryCodes = dbTsStampCountries;
                        IList<TechnicalSpecialistStampInfo> domExsitanceTsStampInfos = new List<TechnicalSpecialistStampInfo>();
                        TechnicalSpecialistStampInfo technicalSpecialistStampInfo = new TechnicalSpecialistStampInfo();

                        dbTsStampInfos.ToList().ForEach(dbStampInfos =>
                        {

                            technicalSpecialistStampInfo = _mapper.Map<TechnicalSpecialistStampInfo>(dbStampInfos);
                            var oldDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == dbStampInfos.Id)?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.DocumentName)?.ToList();
                            if (oldDocuments != null && oldDocuments.Count > 0)
                                technicalSpecialistStampInfo.DocumentName = string.Join(",", oldDocuments);

                            domExsitanceTsStampInfos.Add(technicalSpecialistStampInfo);

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == dbStampInfos.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, dbStampInfos, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                    opt.Items["DBTsStampCountryCodes"] = dbTsStampCountryCodes;

                                });
                                dbStampInfos.LastModification = DateTime.UtcNow;
                                dbStampInfos.UpdateCount = dbStampInfos?.UpdateCount.CalculateUpdateCount();
                                dbStampInfos.ModifiedBy = tsToBeModify?.ModifiedBy;
                            }

                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTsStampInfos);
                        if (commitChange)
                        {
                            var savedCnt = _repository.ForceSave();
                            if (savedCnt > 0)
                            {
                                ProcessTsStampDocuments(recordToBeModify, dbTsStampInfos, ref validationMessages);
                                if (recordToBeModify?.Count > 0)
                                {
                                    recordToBeModify?.ToList().ForEach(x1 =>
                                    {
                                        var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                        if (newDocuments != null && newDocuments.Count > 0)
                                            x1.DocumentName = string.Join(",", newDocuments);

                                        _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser, null,
                                                                   ValidationType.Update.ToAuditActionType(),
                                                                    SqlAuditModuleType.TechnicalSpecialistStamp,
                                                                    domExsitanceTsStampInfos?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                        x1);
                                    });
                                }
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStampInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                            ref IList<TechnicalSpecialistStampInfo> filteredTsInfos,
                                            ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCountries,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {


            bool result = false;
            if (tsStampInfos != null && tsStampInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInfos == null || filteredTsInfos.Count <= 0)
                    filteredTsInfos = FilterRecord(tsStampInfos, validationType);

                if (filteredTsInfos?.Count > 0 && IsValidPayload(filteredTsInfos, validationType, ref messages))
                {
                    GetTsStampDbInfo(filteredTsInfos, true, ref dbTsStampInfos);
                    IList<int> tsIds = filteredTsInfos.Select(x => x.Id).ToList();
                    IList<int> tsDbStampIds = filteredTsInfos.Select(x => x.Id).ToList();
                    if (tsIds.Any(x => !tsDbStampIds.Contains(x))) //Invalid TechSpecialist Id found.
                    {
                        var dbTsStampInfosByIds = dbTsStampInfos;
                        var idNotExists = tsIds.Where(id => !dbTsStampInfosByIds.Any(ts => ts.Id == id)).ToList();
                        var tsStampList = filteredTsInfos;
                        idNotExists?.ForEach(tsId =>
                        {
                            var ts = tsStampList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, ts, MessageType.TsStampUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsInfos, dbTsStampInfos, ref messages);
                        if (result)
                            result = IsTechSpecialistEpinUnique(filteredTsInfos, ref validationMessages);
                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                    IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsStampInfos.Where(x => !dbTsStampInfos.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.StampNumber, MessageType.TsUpdatedByOther, x.StampNumber);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTechSpecialistEpinUnique(IList<TechnicalSpecialistStampInfo> filteredTsStampInfos,
                                                ref IList<ValidationMessage> validationMessages)
        {
            if (filteredTsStampInfos?.Count > 0)
            {
                List<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                var dbTsStampInfos = _repository.Get(filteredTsStampInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.StampNumber)).ToList());
                if (dbTsStampInfos?.Count > 0)
                {
                    var dbPins = dbTsStampInfos.Select(x1 => x1.TechnicalSpecialist.Pin).ToList();
                    var dbStampNumbers = dbTsStampInfos.Select(x1 => x1.StampNumber).ToList();
                    var tsAlreadyExist = filteredTsStampInfos.Where(x => !dbPins.Contains(x.Epin) && !dbStampNumbers.Contains(x.StampNumber));
                    tsAlreadyExist?.ToList().ForEach(x =>
                    {
                        messages.Add(_messages, x.StampNumber, MessageType.TsStampAlreadyExist, x.StampNumber);
                    });
                }

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);

                return messages?.Count <= 0;
            }
            else
                return false;
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                            ref IList<TechnicalSpecialistStampInfo> filteredTsStampInfos,
                                            ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                            ref IList<ValidationMessage> validationMessages,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCountries)
        {
            bool result = false;
            if (tsStampInfos != null && tsStampInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsStampInfos == null || filteredTsStampInfos.Count <= 0)
                    filteredTsStampInfos = FilterRecord(tsStampInfos, validationType);

                if (filteredTsStampInfos?.Count > 0 && IsValidPayload(filteredTsStampInfos, validationType, ref validationMessages))
                {
                    GetTsStampDbInfo(filteredTsStampInfos, false, ref dbTsStampInfos);

                    //IList<KeyValuePair<string, string>> tsStampNotExists = null;
                    //var tsPinAndStamp = filteredTsStampInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.StampNumber))
                    //                                        .Distinct()
                    //                                        .ToList();
                    //result = IsStampInfoExistInDb(tsPinAndStamp, dbTsInfos, ref tsStampNotExists, ref validationMessages);

                    IList<int> tsStampIdNotExists = null;
                    var tsStampIds = filteredTsStampInfos.Select(x => x.Id).Distinct().ToList();
                    result = IsStampInfoExistInDb(tsStampIds, dbTsStampInfos, ref tsStampIdNotExists, ref validationMessages);
                    if (result)
                        result = IsTsStampCanBeRemove(dbTsStampInfos, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsTsStampCanBeRemove(IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsStampInfos?.Where(x => x.IsAnyCollectionPropertyContainValue())
                            .ToList()
                            .ForEach(x =>
                            {
                                messages.Add(_messages, x.StampNumber, MessageType.TsStampNumberIsBeingUsed, x.StampNumber);
                            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private void GetTsStampDbInfo(IList<TechnicalSpecialistStampInfo> filteredTsStampInfos,
                                        bool isTsStampInfoById,
                                        ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos)
        {
            IList<DbModel.TechnicalSpecialistStamp> dbTsStamps = null;
            dbTsStampInfos = dbTsStampInfos ?? new List<DbModel.TechnicalSpecialistStamp>();
            var tsPins = !isTsStampInfoById ?
                            filteredTsStampInfos.Select(x => x.Epin.ToString()).Distinct().ToList() :
                            null;
            IList<int> tsIds = isTsStampInfoById ?
                                filteredTsStampInfos.Select(x => x.Id).Distinct().ToList() :
                                null;

            if (tsPins?.Count > 0 && (dbTsStampInfos.Count <= 0 || dbTsStampInfos.Any(x => !tsPins.Contains(x.TechnicalSpecialist.Pin.ToString()))))
            {
                dbTsStamps = GetStampInfoByPin(tsPins).ToList();
            }
            if (tsIds?.Count > 0 && (dbTsStampInfos.Count <= 0 || dbTsStampInfos.Any(x => !tsIds.Contains(x.Id))))
            {
                dbTsStamps = GetStampInfotById(tsIds).ToList();
            }
            if (dbTsStamps != null && dbTsStamps.Any())
            {
                dbTsStampInfos.AddRange(dbTsStamps);
            }
        }

        private IList<TechnicalSpecialistStampInfo> FilterRecord(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                                    ValidationType filterType)
        {
            IList<TechnicalSpecialistStampInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsStampInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsStampInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsStampInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsStampInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumbers,
                                            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                            ref IList<KeyValuePair<string, string>> tsStampNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsStampInfos == null)
                dbTsStampInfos = new List<DbModel.TechnicalSpecialistStamp>();

            var validMessages = validationMessages;

            if (tsPinAndStampNumbers?.Count > 0)
            {
                tsStampNotExists = tsPinAndStampNumbers.Where(info => !dbTsStampInfos.Any(x1 => x1.TechnicalSpecialist.Pin == Convert.ToInt64(info.Key) && x1.StampNumber == info.Value))
                                                        .Select(x => x).ToList();

                tsStampNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsStampNumDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsStampInfoExistInDb(IList<int> tsStampIds,
                                            IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                            ref IList<int> tsStampIdNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsStampInfos == null)
                dbTsStampInfos = new List<DbModel.TechnicalSpecialistStamp>();

            var validMessages = validationMessages;

            if (tsStampIds?.Count > 0)
            {
                tsStampIdNotExists = tsStampIds.Where(x => !dbTsStampInfos.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                tsStampIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response RemoveTechSpecialistStamp(IList<TechnicalSpecialistStampInfo> tsStampInfos,
            ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                                    bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCountries = null;
            IList<TechnicalSpecialistStampInfo> recordToBeDeleted = null;
            long? eventId = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsStampInfos?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsStampInfos, ValidationType.Delete, ref dbTechnicalSpecialists, ref dbCountries, ref dbTsStampInfos);

                if (tsStampInfos?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsStampInfos, ValidationType.Delete);
                }
                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsStampInfos?.Count > 0)
                {
                    var dbTsStampToBeDeleted = dbTsStampInfos?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsStampToBeDeleted);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value > 0)

                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                            null,
                                                                                                             ValidationType.Delete.ToAuditActionType(),
                                                                                                             SqlAuditModuleType.TechnicalSpecialistStamp,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStampInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                    ValidationType validationType,
                                                    ref IList<TechnicalSpecialistStampInfo> filteredTsInfos,
                                                    ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.Data> dbCountries,
                                                    bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsStampInfos, ref filteredTsInfos, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbCountries, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsStampInfos, ref filteredTsInfos, ref dbTsStampInfos, ref validationMessages, ref dbTechnicalSpecialists, ref dbCountries);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsStampInfos, ref filteredTsInfos, ref dbTsStampInfos, ref dbTechnicalSpecialists, ref dbCountries, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStampInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsValidPayload(IList<TechnicalSpecialistStampInfo> ts,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(ts), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsMasterDataValid(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                       ref IList<DbModel.Data> dbCountries,
                                       ref IList<ValidationMessage> validationMessages)

        {
            bool result = false;
            IList<string> countryNames = tsStampInfos.Select(x => x.CountryName).ToList();

            if (countryNames?.Count > 0)
                result = _countryService.IsValidTechnicalSpecialistStampCountryCode(countryNames, ref dbCountries, ref validationMessages);

            return result;
        }

        private bool IsTSStampUnique(IList<TechnicalSpecialistStampInfo> filteredTsStamp,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var tsStamp = filteredTsStamp.Select(x => new { x.Epin, x.StampNumber, x.CountryName, x.CountryCode, x.Id, x.IsSoftStamp });
            var dbTsStamp = _repository.FindBy(x => tsStamp.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.StampNumber == x.StampNumber
                                              && x1.CountryName == x.Country.Name && x1.CountryCode == x.Country.Code
                                              && x1.Id != x.Id)).ToList();
            if (dbTsStamp?.Count > 0)
            {
                var tsStampAlreadyExist = filteredTsStamp.Where(x => dbTsStamp.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin
                                                                   && x.StampNumber == x1.StampNumber && x.CountryName == x1.Country.Name
                                                                   && x.CountryCode == x1.Country.Code));
                tsStampAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.TsStampAlreadyExist, x.StampNumber);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsStampNumberValidation(IList<TechnicalSpecialistStampInfo> filteredTsStamp,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var tsStampNumber = filteredTsStamp?.Select(x => new { x.StampNumber, x.IsSoftStamp });

            var HardStamp = tsStampNumber.Where(x => x.IsSoftStamp == false && x.StampNumber.Length < 3).ToList();
            HardStamp?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.Hard_Stamp_Number_Validation, x.StampNumber);
            });
            var SoftStamp = tsStampNumber.Where(x => x.IsSoftStamp == true && x.StampNumber.Length < 4).ToList();
            SoftStamp?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.Soft_Stamp_Number_Validation, x.StampNumber);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private IList<TechnicalSpecialistStampInfo> PopulateTsStampsDocument(IList<TechnicalSpecialistStampInfo> tsStamps)
        {
            try
            {
                if (tsStamps?.Count > 0)
                {
                    var epins = tsStamps.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var certificationIds = tsStamps.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsCertificationDocs = _documentService.Get(ModuleCodeType.TS, epins, certificationIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsCertificationDocs?.Count > 0)
                    {
                        return tsStamps.GroupJoin(tsCertificationDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsCert = tsc, doc }).Select(x =>
                            {
                                x.tsCert.Documents = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_Stamp.ToString()).ToList();
                                return x.tsCert;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStamps);
            }

            return tsStamps;
        }
        private Response ProcessTsStampDocuments(IList<TechnicalSpecialistStampInfo> tsStamps, IList<DbModel.TechnicalSpecialistStamp> dbTsStamps, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tsStamps?.Count > 0)
                {
                    tsStamps = tsStamps.Join(dbTsStamps,
                          tsc => new { stampnumber = tsc.StampNumber.ToString(), stampType = tsc.IsSoftStamp.ToString() },
                          dbtsc => new { stampnumber = dbtsc.StampNumber.ToString(), stampType = dbtsc.IsSoftStamp.ToString() },
                          (tsc, dbtsc) => new { tsCert = tsc, dbtsc }).Select(x =>
                          {
                              x.tsCert?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                              {
                                  if (x1.RecordStatus.IsRecordStatusDeleted())
                                      x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                  else
                                  {
                                      x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                      x1.ModifiedBy = x?.tsCert?.ModifiedBy;
                                  }
                                  x1.SubModuleRefCode = x?.dbtsc?.Id.ToString();
                                  x1.ModuleRefCode = x?.tsCert?.Epin.ToString();
                                  x1.DocumentType = DocumentType.TS_Stamp.ToString();
                              });
                              return x.tsCert;
                          }).ToList();

                    var tsDocToBeProcess = tsStamps?.Where(x => x.Documents != null &&
                                                                                      x.Documents.Any(x1 => x1.RecordStatus != null))
                                                                          .SelectMany(x => x.Documents)
                                                                          .ToList();
                    if (tsDocToBeProcess?.Count > 0)
                    {
                        var docToModify = tsDocToBeProcess.Where(x1 => x1.RecordStatus.IsRecordStatusModified()).ToList();
                        var docToDelete = tsDocToBeProcess.Where(x1 => x1.RecordStatus.IsRecordStatusDeleted()).ToList();
                        if (docToDelete.Count > 0)
                            _documentService.Delete(docToDelete);

                        if (docToModify?.Count > 0)
                            return _documentService.Modify(docToModify);
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStamps);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
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

        #endregion
    }
}
