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
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistLanguageCapabilityService : ITechnicalSpecialistLanguageCapabilityService
    {

        private readonly IAppLogger<TechnicalSpecialistLanguageCapabilityService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistLanguageCapabilityRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly IMasterService _masterService = null;
        private readonly ILanguageService _langService = null;
        //private readonly ITechnicalSpecialistService _tsInfoServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ITechnicalSpecialistLanguageCapabilityValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor
        public TechnicalSpecialistLanguageCapabilityService(IMapper mapper,
                                                    ITechnicalSpecialistLanguageCapabilityRepository repository,
                                                    IAppLogger<TechnicalSpecialistLanguageCapabilityService> logger,
                                                    ITechnicalSpecialistLanguageCapabilityValidationService validationService,
                                                    //ITechnicalSpecialistService tsInfoServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    IMasterService masterService,
                                                    ILanguageService langService,
                                                    JObject messages, IAuditSearchService auditSearchService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            _masterService = masterService;
            //_tsInfoServices = tsInfoServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _langService = langService;
            _auditSearchService = auditSearchService;

        }


        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistLanguageCapabilityInfo searchModel)
        {
            IList<TechnicalSpecialistLanguageCapabilityInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistLanguageCapabilityInfo>>(_repository.Search(searchModel));
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
            IList<TechnicalSpecialistLanguageCapabilityInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistLanguageCapabilityInfo>>(GetLanguageCapabilityInfoByPin(pins));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> Ids)
        {
            IList<TechnicalSpecialistLanguageCapabilityInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistLanguageCapabilityInfo>>(GetLanguageCapabilityInfoById(Ids));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), Ids);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> Language)
        {
            IList<TechnicalSpecialistLanguageCapabilityInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistLanguageCapabilityInfo>>(GetLanguageCapabilityByLanguage(Language));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), Language);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndLanguage,
                                         ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<KeyValuePair<string, string>> tsPinAndlanguageNotExists = null;
            return IsRecordExistInDb(tsPinAndLanguage, ref dbTslangCapabilityInfos, ref tsPinAndlanguageNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndLanguage,
                                            ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                            ref IList<KeyValuePair<string, string>> tsPinAndCustomerNameNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTslangCapabilityInfos == null && tsPinAndLanguage?.Count > 0)
                    dbTslangCapabilityInfos = GetLanguageCapabilityByLanguage(tsPinAndLanguage.Select(x => x.Key).ToList());

                result = IslangCapabilityInfoExistInDb(tsPinAndLanguage, dbTslangCapabilityInfos, ref tsPinAndCustomerNameNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPinAndLanguage);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbLanguages = null;
            return AddTsLanguageCapability(tslangCapabilityInfos, ref dbTslangCapabilityInfos, ref dbTechnicalSpecialists, ref dbLanguages, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                       ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Data> dbLanguages,
                                       bool commitChange = true,
                                       bool isDbValidationRequire = true)
        {
            return AddTsLanguageCapability(tslangCapabilityInfos, ref dbTslangCapabilityInfos, ref dbTechnicalSpecialists, ref dbLanguages, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapability, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilities = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbLanguages = null;

            return UpdateTSLanguageCapability(tslangCapability, ref dbTslangCapabilities, ref dbTechnicalSpecialists, ref dbLanguages, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                        ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbLanguages,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true)
        {
            return UpdateTSLanguageCapability(tslangCapabilityInfos, ref dbTslangCapabilityInfos, ref dbTechnicalSpecialists, ref dbLanguages, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Delete
        public Response Delete(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos = null;
            return RemoveTechSpecialistCustomerApproval(tsCustomerApprovalInfos, ref dbTslangCapabilityInfos, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
              ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                              bool commitChange = true,
                              bool isDbValidationRequire = true)
        {
            return RemoveTechSpecialistCustomerApproval(tsCustomerApprovalInfos, ref dbTslangCapabilityInfos, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsCustomerApprovalInfos = null;
            return IsRecordValidForProcess(tsCustomerApprovalInfos, validationType, ref dbTsCustomerApprovalInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsCustomerApprovalInfos)
        {
            IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTechSpecialist = null;
            IList<DbModel.Data> dbCustomer = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist = null;

            return CheckRecordValidForProcess(tsCustomerApprovalInfos, validationType, ref filteredTechSpecialist, ref dbTsCustomerApprovalInfos, ref dbCustomer, ref dbTechnicalpecialist);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsLanguageCapabilities,
                                                ref IList<DbModel.Data> dbLanguages,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialists,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTechSpecialist = null;
            return CheckRecordValidForProcess(tsCustomerApprovalInfos, validationType, ref filteredTechSpecialist, ref dbTsLanguageCapabilities, ref dbLanguages, ref dbTechnicalpecialists, isDraft);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tsStampInfos,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsStampInfos)
        {
            return IsRecordValidForProcess(tsStampInfos, validationType, ref dbTsStampInfos);
        }
        #endregion

        #region Add

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                         ref IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapabilityInfos,
                                         ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                         ref IList<DbModel.Data> dbTsLanguage,
                                         ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tslangCapabilityInfos != null && tslangCapabilityInfos.Count > 0)
            {
                dbTslangCapabilityInfos = dbTslangCapabilityInfos ?? new List<DbModel.TechnicalSpecialistLanguageCapability>();
                dbTsLanguage = dbTsLanguage ?? new List<DbModel.Data>();
                dbTsInfos = dbTsInfos ?? new List<DbModel.TechnicalSpecialist>();

                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTslangCapabilityInfos == null || filteredTslangCapabilityInfos.Count <= 0)
                    filteredTslangCapabilityInfos = FilterRecord(tslangCapabilityInfos, validationType);

                if (filteredTslangCapabilityInfos?.Count > 0 && IsValidPayload(filteredTslangCapabilityInfos, validationType, ref validationMessages))
                {
                    IList<KeyValuePair<string, string>> tsCustAprovalNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();

                    IList<string> language = filteredTslangCapabilityInfos.Select(x => x.Language).ToList();
                    IList<string> epins = filteredTslangCapabilityInfos.Select(x => x.Epin.ToString()).ToList();

                    var dbMaster = GetMasterData(filteredTslangCapabilityInfos, ref dbTsLanguage);

                    result = _langService.IsValidLanguage(language, ref dbTsLanguage, ref validationMessages);

                    if (result && epins?.Count > 0)
                    {
                        //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(epins, ref dbTsInfos, ref validationMessages, tsca => tsca.TechnicalSpecialistLanguageCapability).Result);
                        result = IsTechSpecialistExistInDb(epins, ref dbTsInfos, ref validationMessages, tsca => tsca.TechnicalSpecialistLanguageCapability);
                    }

                    if (result)
                    {
                        //this.GetTsCustomerApprovalDbInfo(filteredTsCustApprInfos, false, ref dbTsCustApprInfos);
                        dbTslangCapabilityInfos = dbTsInfos.SelectMany(x => x.TechnicalSpecialistLanguageCapability).ToList();
                        var tsPinAndCodes = tslangCapabilityInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.Language))
                                                          .Distinct()
                                                          .ToList();
                        result = !IslangCapabilityInfoExistInDb(tsPinAndCodes, dbTslangCapabilityInfos, ref tsCustAprovalNotExists, ref messages) && messages.Count == tsPinAndCodes.Count;
                        if (!result)
                        {
                            var tsAlreadyExists = tsPinAndCodes.Where(x => tsCustAprovalNotExists.Contains(x)).ToList();
                            tsAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messages, x, MessageType.TsEPinAlreadyExist, x);
                            });

                            if (messages.Count > 0)
                                validationMessages.AddRange(messages);
                        }
                    }

                }
            }
            return result;
        }

        private object GetMasterData(IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapabilityInfos, ref IList<DbModel.Data> dbTsLanguage)
        {
            IList<string> CodeStandardName = filteredTslangCapabilityInfos.Select(x => x.Language).ToList();

            var masterNames = CodeStandardName.ToList();
            var masterTypes = new List<MasterType>()
                    {
                        MasterType.Language

                    };
            var dbMaster = _masterService.Get(masterTypes, null, masterNames);
            if (dbMaster?.Count > 0)
            {
                var tsLanguage = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.Language).ToList();
                if (tsLanguage?.Count > 0)
                {
                    dbTsLanguage.AddRange(tsLanguage);
                    dbTsLanguage = dbTsLanguage.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
                }
            }

            return dbMaster;


        }

        private Response AddTsLanguageCapability(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                        ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbLanguages,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                IList<DbModel.Data> dbMstLanguages = null;
                IList<DbModel.TechnicalSpecialist> dbTechspecialist = null;
                IList<TechnicalSpecialistLanguageCapabilityInfo> recordToBeAdd = null;
                eventId = tslangCapabilityInfos?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tslangCapabilityInfos, ValidationType.Add, ref recordToBeAdd, ref dbTslangCapabilityInfos, ref dbMstLanguages, ref dbTechspecialist);

                if (tslangCapabilityInfos?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tslangCapabilityInfos, ValidationType.Add);
                }

                if (!isDbValidationRequire || (recordToBeAdd?.Count > 0 && Convert.ToBoolean(valdResponse.Result)))
                {
                    if(dbMstLanguages != null && dbMstLanguages.Any()) //IGOQC D842  Issue 13
                    {
                        dbMstLanguages.AddRange(dbLanguages);
                    }
                    else { dbMstLanguages = dbLanguages; }
                    if(dbTechspecialist != null && dbTechspecialist.Any()) //IGOQC D842  Issue 13
                    {
                        dbTechnicalSpecialists.AddRange(dbTechspecialist);
                    } else { dbTechspecialist = dbTechnicalSpecialists;  }
                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistLanguageCapability>>(recordToBeAdd, opt =>
                    {
                        opt.Items["DbLanguage"] = dbMstLanguages;
                        opt.Items["DbTechnicalspecialists"] = dbTechspecialist;
                    });

                    _repository.Add(mappedRecords);

                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        dbTslangCapabilityInfos = mappedRecords;
                        if (mappedRecords?.Count > 0 && value > 0)
                        {
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tslangCapabilityInfos.FirstOrDefault().ActionByUser,
                                                                                              null,
                                                                                             ValidationType.Add.ToAuditActionType(),
                                                                                             SqlAuditModuleType.TechnicalSpecialistLanguageCapability,
                                                                                               null,
                                                                                               _mapper.Map<TechnicalSpecialistLanguageCapabilityInfo>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tslangCapabilityInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }



        #endregion

        #region ModifyDetaisl

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistLanguageCapabilityInfo> tslanguageCapabilities,
                                           ref IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapability,
                                           ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsLangcapbility,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.Data> dblanguages,
                                           ref IList<ValidationMessage> validationMessages,
                                           bool isDraft = false)
        {
            bool result = false;
            if (tslanguageCapabilities != null && tslanguageCapabilities.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTslangCapability == null || filteredTslangCapability.Count <= 0)
                    filteredTslangCapability = FilterRecord(tslanguageCapabilities, validationType);

                if (filteredTslangCapability?.Count > 0 && IsValidPayload(filteredTslangCapability, validationType, ref messages))
                {
                    GetTslangCapabilityDbInfo(filteredTslangCapability, ref dbTsLangcapbility);
                    IList<int> tsPayScheduleIds = filteredTslangCapability.Select(x => x.Id).ToList();
                    IList<int> tsDbPayScheduleIds = dbTsLangcapbility.Select(x => x.Id).ToList();
                    if (tsPayScheduleIds.Any(x => !tsDbPayScheduleIds.Contains(x))) //Invalid TechSpecialist PaySchedule Id found.
                    {
                        var dbTsInfosByIds = dbTsLangcapbility;
                        var idNotExists = tsPayScheduleIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistPaySchedulesList = filteredTslangCapability;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsPaySchedule = techSpecialistPaySchedulesList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsPaySchedule, MessageType.TsPayScheduleUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTslangCapability, dbTsLangcapbility, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTslangCapability.Select(x => x.Epin.ToString()).ToList();
                            IList<string> languages = filteredTslangCapability.Select(x => x.Language).ToList();
                            //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            if (result && languages?.Count > 0)
                                result = _langService.IsValidLanguage(languages, ref dblanguages, ref validationMessages);
                            if (result)
                                result = IsTSPayScheduleUnique(filteredTslangCapability, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }


        private bool IsTSPayScheduleUnique(IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapability,
                                        ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var tslangCapabilities = filteredTslangCapability.Select(x => new { x.Epin, x.Language, x.Id });
            var dbTslangCapabilities = _repository.FindBy(x => tslangCapabilities.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.Language == x.Language.Name & x1.Id != x.Id)).ToList();
            if (dbTslangCapabilities?.Count > 0)
            {
                var tslangcapabilityExist = filteredTslangCapability.Where(x => dbTslangCapabilities.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin && x.Language == x1.Language.Name));
                tslangcapabilityExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.langCapabilitiesAlreadyExist, x.Epin, x.Language);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }



        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistLanguageCapabilityInfo> tsPaySchedules,
                                               IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsPaySchedules,
                                               ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsPaySchedules.Where(x => !dbTsPaySchedules.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsPayScheduleUpdatedByOther, x.Language);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response UpdateTSLanguageCapability(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapability,
                                     ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapability,
                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                     ref IList<DbModel.Data> dblanguages,
                                     bool commitChange = true,
                                     bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dblanguage = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistLanguageCapabilityInfo> recordToBeModify = null;
            bool valdResult = false;
            long? eventId = 0;


            try
            {
                eventId = tslangCapability?.Select(x => x.EventId).FirstOrDefault();
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tslangCapability, ValidationType.Update, ref recordToBeModify, ref dbTslangCapability, ref dblanguages, ref dbTechSpecialists);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (tslangCapability?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tslangCapability, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTslangCapability == null || (dbTslangCapability?.Count <= 0 && valdResult == false))
                    {
                        dbTslangCapability = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && valdResult == false))
                    {
                        //valdResult = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if (dblanguages == null || (dblanguages?.Count <= 0 && valdResult == false))
                    {
                        valdResult = _langService.IsValidLanguage(recordToBeModify.Select(x => x.Language).ToList(), ref dblanguages, ref validationMessages);
                    }

                    if (!isDbValidationRequired || (valdResult && dbTslangCapability?.Count > 0))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dblanguage = dblanguages;
                        IList<TechnicalSpecialistLanguageCapabilityInfo> domexsistanceTslangCapability = new List<TechnicalSpecialistLanguageCapabilityInfo>();

                        dbTslangCapability.ToList().ForEach(tsPayInfo =>
                        {
                            domexsistanceTslangCapability.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistLanguageCapabilityInfo>(tsPayInfo)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsPayInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsPayInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbLanguage"] = dblanguage;
                                    opt.Items["DbTechnicalspecialists"] = dbTechSpecialists;
                                });
                                tsPayInfo.LastModification = DateTime.UtcNow;
                                tsPayInfo.UpdateCount = tsPayInfo.UpdateCount.CalculateUpdateCount();
                                tsPayInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTslangCapability);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            if (recordToBeModify?.Count > 0 && value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                      null,
                                                                                                      ValidationType.Update.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.TechnicalSpecialistLanguageCapability,
                                                                                                       domexsistanceTslangCapability?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tslangCapability);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion


        #region Common
        private void GetTsCustomerApprovalDbInfo(IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTsCustomerApprovalInfos,
                                    bool isTsCustomerApprovalInfoById,
                                    ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsCustomerApprovalInfos)
        {
            var tsPins = !isTsCustomerApprovalInfoById ?
                            filteredTsCustomerApprovalInfos.Select(x => x.Epin.ToString()).Distinct().ToList() :
                            null;
            IList<int> tsIds = isTsCustomerApprovalInfoById ?
                                filteredTsCustomerApprovalInfos.Select(x => (int)x.Id).Distinct().ToList() :
                                null;

            if (dbTsCustomerApprovalInfos == null || dbTsCustomerApprovalInfos.Count <= 0)
                dbTsCustomerApprovalInfos = isTsCustomerApprovalInfoById ?
                                    GetLanguageCapabilityInfoById(tsIds).ToList() :
                                    GetLanguageCapabilityInfoByPin(tsPins).ToList();
        }

        private IList<TechnicalSpecialistLanguageCapabilityInfo> FilterRecord(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
                                                                    ValidationType filterType)
        {
            IList<TechnicalSpecialistLanguageCapabilityInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IslangCapabilityInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndLanguage,
                                                     IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                                     ref IList<KeyValuePair<string, string>> tslangCapabilityNotExists,
                                                     ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTslangCapabilityInfos == null)
                dbTslangCapabilityInfos = new List<DbModel.TechnicalSpecialistLanguageCapability>();

            var validMessages = validationMessages;

            if (tsPinAndLanguage?.Count > 0)
            {
                tslangCapabilityNotExists = tsPinAndLanguage.Where(info => !dbTslangCapabilityInfos.Any(x1 => x1.TechnicalSpecialist.Pin.ToString() == info.Key &&
                                                                                                      x1.Language.Name == info.Value))
                                                          .Select(x => x).ToList();

                tslangCapabilityNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsLanguageDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IslangagugeCapabilityInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndLanguage,
                                                    IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                                    ref IList<KeyValuePair<string, string>> tslangcapabilityNotExists,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTslangCapabilityInfos == null)
                dbTslangCapabilityInfos = new List<DbModel.TechnicalSpecialistLanguageCapability>();

            var validMessages = validationMessages;

            if (tsPinAndLanguage?.Count > 0)
            {
                tslangcapabilityNotExists = tsPinAndLanguage.Where(info => !dbTslangCapabilityInfos.Any(x1 => x1.TechnicalSpecialist.Pin.ToString() == info.Key &&
                                                                                                      x1.Language.Name == info.Value))
                                                          .Select(x => x).ToList();

                tslangcapabilityNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsLanguageDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private Response RemoveTechSpecialistCustomerApproval(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
            ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                                    bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<TechnicalSpecialistLanguageCapabilityInfo> recordToBeDeleted = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsCustomerApprovalInfos?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsCustomerApprovalInfos, ValidationType.Delete, ref dbTslangCapabilityInfos);

                if (tsCustomerApprovalInfos?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsCustomerApprovalInfos, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTslangCapabilityInfos?.Count > 0)
                {
                    var dbTslangCapabilityToBeDeleted = dbTslangCapabilityInfos?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTslangCapabilityToBeDeleted);
                    if (commitChange)
                    {
                        _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0)
                        {



                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                   null,
                                                                                                 ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.TechnicalSpecialistLanguageCapability,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustomerApprovalInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                                  ValidationType validationType,
                                                  ref IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapabilityInfos,
                                                  ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                                  ref IList<DbModel.Data> dbTsLanguage,
                                                  ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                  bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tslangCapabilityInfos,
                                                 ref filteredTslangCapabilityInfos,
                                                 ref dbTslangCapabilityInfos,
                                                 ref dbTsLanguage,
                                                 ref dbTsInfos,
                                                 ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tslangCapabilityInfos,
                                                     ref filteredTslangCapabilityInfos,
                                                     ref dbTslangCapabilityInfos,
                                                     ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tslangCapabilityInfos,
                                                     ref filteredTslangCapabilityInfos,
                                                     ref dbTslangCapabilityInfos,
                                                     ref dbTsInfos,
                                                      ref dbTsLanguage,
                                                     ref validationMessages,
                                                     isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tslangCapabilityInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapability,
                                           ref IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapability,
                                           ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapability,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tslangCapability != null && tslangCapability.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTslangCapability == null || filteredTslangCapability.Count <= 0)
                    filteredTslangCapability = FilterRecord(tslangCapability, validationType);

                if (filteredTslangCapability?.Count > 0 && IsValidPayload(filteredTslangCapability, validationType, ref validationMessages))
                {
                    GetTslangCapabilityDbInfo(filteredTslangCapability, ref dbTslangCapability);
                    IList<int> tslangCapabilityIdNotExists = null;
                    var tsLangCapabilityIds = filteredTslangCapability.Select(x => x.Id).Distinct().ToList();
                    result = IslangCapabilityExistInDb(tsLangCapabilityIds, dbTslangCapability, ref tslangCapabilityIdNotExists, ref validationMessages);
                    //if (result)
                    //   // result = IsTechSpecialistPayScheduleCanBeRemove(dbTslangCapability, ref validationMessages);
                }
            }
            return result;
        }
        private bool IslangCapabilityExistInDb(IList<int> tslangCapabilityIds,
                                            IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangcapabilities,
                                            ref IList<int> tslangCapabilityIdNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTslangcapabilities == null)
                dbTslangcapabilities = new List<DbModel.TechnicalSpecialistLanguageCapability>();

            var validMessages = validationMessages;

            if (tslangCapabilityIds?.Count > 0)
            {
                tslangCapabilityIdNotExists = tslangCapabilityIds.Where(id => !dbTslangcapabilities.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tslangCapabilityIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsPayScheduleIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;

        }

        private void GetTslangCapabilityDbInfo(IList<TechnicalSpecialistLanguageCapabilityInfo> filteredTslangCapabilities,
                                            ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilites)
        {
            dbTslangCapabilites = dbTslangCapabilites ?? new List<DbModel.TechnicalSpecialistLanguageCapability>();
            IList<int> tslangCapabilityIds = filteredTslangCapabilities?.Select(x => x.Id).Distinct().ToList();
            if (tslangCapabilityIds?.Count > 0 && (dbTslangCapabilites.Count <= 0 || dbTslangCapabilites.Any(x => !tslangCapabilityIds.Contains(x.Id))))
            {
                var tsLanguageCapabs = GetLanguageCapabilityInfoById(tslangCapabilityIds);
                if (tsLanguageCapabs != null && tsLanguageCapabs.Any())
                {
                    dbTslangCapabilites.AddRange(tsLanguageCapabs);
                }
            }
        }


        private bool IsValidPayload(IList<TechnicalSpecialistLanguageCapabilityInfo> ts,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(ts), validationType);

            if (ts != null && ts.Any(x => x.Language == null))
                messages.Add(_messages, "Language", MessageType.TsLanguage);
            else if (ts.Select(x => x.Language).Any(x => string.IsNullOrEmpty(x)))
                messages.Add(_messages, "Language", MessageType.TsLanguage);
            if (ts != null && ts.Any(x => x.WritingCapabilityLevel == null))
                messages.Add(_messages, "WritingCapabilityLevel", MessageType.TsWritingCapabilityLevel);
            else if (ts.Select(x => x.WritingCapabilityLevel).Any(x => string.IsNullOrEmpty(x)))
                messages.Add(_messages, "WritingCapabilityLevel", MessageType.TsWritingCapabilityLevel);
            if (ts != null && ts.Any(x => x.SpeakingCapabilityLevel == null))
                messages.Add(_messages, "SpeakingCapabilityLevel", MessageType.TsSpeakingCapabilityLevel);
            else if (ts.Select(x => x.SpeakingCapabilityLevel).Any(x => string.IsNullOrEmpty(x)))
                messages.Add(_messages, "SpeakingCapabilityLevel", MessageType.TsSpeakingCapabilityLevel);
            if (ts != null && ts.Any(x => x.ComprehensionCapabilityLevel == null))
                messages.Add(_messages, "ComprehensionCapabilityLevel", MessageType.TsComprehensionCapabilityLevel);
            else if (ts.Select(x => x.ComprehensionCapabilityLevel).Any(x => string.IsNullOrEmpty(x)))
                messages.Add(_messages, "ComprehensionCapabilityLevel", MessageType.TsComprehensionCapabilityLevel);

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        #endregion

        #region Get
        private IList<DbModel.TechnicalSpecialistLanguageCapability> GetLanguageCapabilityInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbtsLangCapbInfos = null;
            if (pins?.Count > 0)
            {
                dbtsLangCapbInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsLangCapbInfos;
        }

        private IList<DbModel.TechnicalSpecialistLanguageCapability> GetLanguageCapabilityInfoById(IList<int> tsLangCapabilityIds)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbtsCodeAndStandardInfos = null;
            if (tsLangCapabilityIds?.Count > 0)
                dbtsCodeAndStandardInfos = _repository.FindBy(x => tsLangCapabilityIds.Contains((int)x.Id)).ToList();

            return dbtsCodeAndStandardInfos;
        }

        private IList<DbModel.TechnicalSpecialistLanguageCapability> GetLanguageCapabilityByLanguage(IList<string> tslangCapability)
        {
            IList<DbModel.TechnicalSpecialistLanguageCapability> dbtslangCapabilityInfos = null;
            if (tslangCapability?.Count > 0)
                dbtslangCapabilityInfos = _repository.FindBy(x => tslangCapability.Contains(x.Language.Name.ToString())).ToList();

            return null;
        }

        public Response Modify(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos, ref IList<DbModel.TechnicalSpecialistLanguageCapability> dblangCapabilityInfos, ref IList<DbModel.Data> Languages, bool commitChange = true, bool isDbValidationRequire = true)
        {
            throw new NotImplementedException();
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages, params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
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

    }
}
#endregion

#endregion

