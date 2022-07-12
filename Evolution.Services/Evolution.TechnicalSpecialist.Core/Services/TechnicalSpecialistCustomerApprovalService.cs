using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces;
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
    public class TechnicalSpecialistCustomerApprovalService : ITechnicalSpecialistCustomerApprovalService
    {
        private readonly IAppLogger<TechnicalSpecialistCustomerApprovalService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistCustomerApprovalRepository _repository = null;
        private readonly ITechnicalSpecialistCustomerService _customerService = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IDocumentService _documentService = null;
        private readonly ITechnicalSpecialistNoteService _tsNoteService = null;
        private readonly ITechnicalSpecialistCustomerApprovalValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService = null;


        #region Constructor
        public TechnicalSpecialistCustomerApprovalService(IMapper mapper,
                                                    ITechnicalSpecialistCustomerApprovalRepository repository,
                                                    IAppLogger<TechnicalSpecialistCustomerApprovalService> logger,
                                                    ITechnicalSpecialistCustomerApprovalValidationService validationService,
                                                    JObject messages, ITechnicalSpecialistCustomerService customerService,
                                                    ITechnicalSpecialistNoteService tsNoteService,
                                                    IDocumentService documentService,
                                                    //ITechnicalSpecialistService technicalSpecialistInfoServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                 IAuditSearchService auditSearchService

                                                    )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            _customerService = customerService;
            //_tsInfoServices = technicalSpecialistInfoServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _tsNoteService = tsNoteService;
            _documentService = documentService;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistCustomerApprovalInfo searchModel)
        {
            IList<TechnicalSpecialistCustomerApprovalInfo> result = null;
            Exception exception = null;
            try
            {
                var tsCustApp = _mapper.Map<IList<TechnicalSpecialistCustomerApprovalInfo>>(_repository.Search(searchModel));
                var tsCustAppDocResult = PopulateTsCustApprovalDocuments(tsCustApp);
                result = PopulateTsCustApprovalNotes(tsCustAppDocResult);
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
            IList<TechnicalSpecialistCustomerApprovalInfo> result = null;
            Exception exception = null;
            try
            {
                var tsCustApp = _mapper.Map<IList<TechnicalSpecialistCustomerApprovalInfo>>(GetCustomerApprovalInfoByPin(pins));
                var tsCustAppDocResult = PopulateTsCustApprovalDocuments(tsCustApp);
                result = PopulateTsCustApprovalNotes(tsCustAppDocResult);
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
            IList<TechnicalSpecialistCustomerApprovalInfo> result = null;
            Exception exception = null;
            try
            {
                var tsCustApp = _mapper.Map<IList<TechnicalSpecialistCustomerApprovalInfo>>(GetCustomerApprovalById(Ids));
                var tsCustAppDocResult = PopulateTsCustApprovalDocuments(tsCustApp);
                result = PopulateTsCustApprovalNotes(tsCustAppDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), Ids);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> CustomerName)
        {
            IList<TechnicalSpecialistCustomerApprovalInfo> result = null;
            Exception exception = null;
            try
            {
                var tsCustApp = _mapper.Map<IList<TechnicalSpecialistCustomerApprovalInfo>>(GetCustomerApprovalInfotByCustomerName(CustomerName));
                var tsCustAppDocResult = PopulateTsCustApprovalDocuments(tsCustApp);
                result = PopulateTsCustApprovalNotes(tsCustAppDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), CustomerName);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCustomerName,
                                           ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                                           ref IList<ValidationMessage> validationMessages)
        {
            IList<KeyValuePair<string, string>> tsPinAndStampNumberNotExists = null;
            return IsRecordExistInDb(tsPinAndCustomerName, ref dbTsCustomerApprovalInfos, ref tsPinAndStampNumberNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCustomerName,
                                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                                            ref IList<KeyValuePair<string, string>> tsPinAndCustomerNameNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsCustomerApprovalInfos == null && tsPinAndCustomerName?.Count > 0)
                    dbTsCustomerApprovalInfos = GetCustomerApprovalInfoByPin(tsPinAndCustomerName.Select(x => x.Key).ToList());

                result = IsCustomerApprovalInfoExistInDb(tsPinAndCustomerName, dbTsCustomerApprovalInfos, ref tsPinAndCustomerNameNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPinAndCustomerName);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCustomers> Dbcustomers = null;
            return AddTechSpecialistCustomerApproval(tsCustomerApprovalInfos, ref dbTsCustomerApprovalInfos, ref dbTechnicalSpecialists, ref Dbcustomers, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                            ref IList<DbModel.TechnicalSpecialistCustomers> Dbcustomers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddTechSpecialistCustomerApproval(tsCustomerApprovalInfos, ref dbTsCustomerApprovalInfos, ref dbTechnicalSpecialists, ref Dbcustomers, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,

                                                                 bool commitChange = true,
                                bool isDbValidationRequire = true
            )
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos = null;

            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCustomers> Dbcustomers = null;
            return UpdateTsCustomerApproval(tsCustomerApprovalInfos, ref dbTsCustomerApprovalInfos, ref dbTechnicalSpecialists, ref Dbcustomers, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
            ref IList<DbModel.TechnicalSpecialistCustomers> Dbcustomers,
            bool commitChange = true, bool isDbValidationRequire = true)
        {
            return UpdateTsCustomerApproval(tsCustomerApprovalInfos, ref dbTsCustomerApprovalInfos, ref dbTechnicalSpecialists, ref Dbcustomers, commitChange, isDbValidationRequire);

        }
        #endregion

        #region Delete
        public Response Delete(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos = null;
            return RemoveTechSpecialistCustomerApproval(tsCustomerApprovalInfos, ref dbTsCustApprInfos, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos,
                         bool commitChange = true,
                         bool isDbValidationRequire = true)
        {
            return RemoveTechSpecialistCustomerApproval(tsCustomerApprovalInfos, ref dbTsCustApprInfos, commitChange, isDbValidationRequire);
        }
        #endregion


        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos = null;
            return IsRecordValidForProcess(tsCustomerApprovalInfos, validationType, ref dbTsCustomerApprovalInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos)
        {
            IList<TechnicalSpecialistCustomerApprovalInfo> filteredTechSpecialist = null;
            IList<DbModel.TechnicalSpecialistCustomers> dbCustomer = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist = null;

            return CheckRecordValidForProcess(tsCustomerApprovalInfos, validationType, ref filteredTechSpecialist, ref dbTsCustomerApprovalInfos, ref dbTechnicalpecialist, ref dbCustomer);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                                               ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomer,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist,
                                               bool isDraft = false)
        {
            IList<TechnicalSpecialistCustomerApprovalInfo> filteredTechSpecialist = null;

            return CheckRecordValidForProcess(tsCustomerApprovalInfos, validationType, ref filteredTechSpecialist, ref dbTsCustomerApprovalInfos, ref dbTechnicalpecialist, ref dbCustomer, isDraft);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsStampInfos,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsStampInfos)
        {
            return IsRecordValidForProcess(tsStampInfos, validationType, ref dbTsStampInfos);
        }
        #endregion

        #region Get
        private IList<DbModel.TechnicalSpecialistCustomerApproval> GetCustomerApprovalInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbtsCustomerApprovalInfos = null;
            if (pins?.Count > 0)
            {
                dbtsCustomerApprovalInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsCustomerApprovalInfos;
        }

        private IList<DbModel.TechnicalSpecialistCustomerApproval> GetCustomerApprovalById(IList<int> tsCustomerApprovalIds)
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbtsCustomerApprovalInfos = null;
            if (tsCustomerApprovalIds?.Count > 0)
                dbtsCustomerApprovalInfos = _repository.FindBy(x => tsCustomerApprovalIds.Contains(x.Id)).ToList();

            return dbtsCustomerApprovalInfos;
        }

        private IList<DbModel.TechnicalSpecialistCustomerApproval> GetCustomerApprovalInfotByCustomerName(IList<string> tsCodeAndStandard)
        {
            IList<DbModel.TechnicalSpecialistCustomerApproval> dbtsCustomerInfos = null;
            if (tsCodeAndStandard?.Count > 0)
                dbtsCustomerInfos = _repository.FindBy(x => tsCodeAndStandard.Contains(x.Customer.Name)).ToList();

            return dbtsCustomerInfos;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustApprInfos,
                                         ref IList<TechnicalSpecialistCustomerApprovalInfo> filteredTsCustApprInfos,
                                         ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos,
                                         ref IList<DbModel.TechnicalSpecialistCustomers> dbTsCustomer,
                                         ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCustApprInfos != null && tsCustApprInfos.Count > 0)
            {
                dbTsCustApprInfos = dbTsCustApprInfos ?? new List<DbModel.TechnicalSpecialistCustomerApproval>();
                dbTsCustomer = dbTsCustomer ?? new List<DbModel.TechnicalSpecialistCustomers>();
                dbTsInfos = dbTsInfos ?? new List<DbModel.TechnicalSpecialist>();
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsCustApprInfos == null || filteredTsCustApprInfos.Count <= 0)
                    filteredTsCustApprInfos = FilterRecord(tsCustApprInfos, validationType);

                if (filteredTsCustApprInfos?.Count > 0 && IsValidPayload(filteredTsCustApprInfos, validationType, ref validationMessages))
                {
                    IList<KeyValuePair<string, string>> tsCustAprovalNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();

                    IList<string> CustomerCode = filteredTsCustApprInfos.Select(x => x.CustomerCode).ToList();
                    IList<string> epins = filteredTsCustApprInfos.Select(x => x.Epin.ToString()).ToList();

                    // var dbMaster = this.GetMasterData(filteredTechSpecialistCustomerApproval, ref dbCustomerName);

                    result = _customerService.IsValidCustomer(CustomerCode, ref dbTsCustomer, ref validationMessages);
                    if (result && epins?.Count > 0)
                    {
                        //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(epins, ref dbTsInfos, ref validationMessages, tsca => tsca.TechnicalSpecialistCustomerApproval).Result);
                        result = IsTechSpecialistExistInDb(epins, ref dbTsInfos, ref validationMessages, tsca => tsca.TechnicalSpecialistCustomerApproval);
                    }

                    if (result)
                    {
                        //this.GetTsCustomerApprovalDbInfo(filteredTsCustApprInfos, false, ref dbTsCustApprInfos);
                        dbTsCustApprInfos = dbTsInfos.SelectMany(x => x.TechnicalSpecialistCustomerApproval).ToList();
                        var tsPinAndCodes = tsCustApprInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.CustomerName))
                                                          .Distinct()
                                                          .ToList();
                        result = !IsCustomerApprovalInfoExistInDb(tsPinAndCodes, dbTsCustApprInfos, ref tsCustAprovalNotExists, ref messages) && messages.Count == tsPinAndCodes.Count;
                        if (tsCustAprovalNotExists == null || tsCustAprovalNotExists?.Count == 0)
                        {
                            result = true;
                        }
                        if (!result && tsCustAprovalNotExists?.Count > 0)
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



        private Response AddTechSpecialistCustomerApproval(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                     ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTscustApproval,
                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                     ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomers,
                                     bool commitChange = true,
                                     bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                IList<DbModel.TechnicalSpecialistCustomers> dbCustomerName = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalspecialist = null;
                IList<DbModel.CustomerCommodity> dbCustomerCommodity = null;
                IList<TechnicalSpecialistCustomerApprovalInfo> recordToBeAdd = null;
                eventId = tsCustomerApprovalInfos?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = CheckRecordValidForProcess(tsCustomerApprovalInfos, ValidationType.Add, ref recordToBeAdd, ref dbTscustApproval, ref dbTechnicalSpecialists, ref dbCustomers);

                if (tsCustomerApprovalInfos?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsCustomerApprovalInfos, ValidationType.Add);
                }

                if (!isDbValidationRequired || (recordToBeAdd?.Count > 0 && Convert.ToBoolean(valdResponse.Result)))
                {
                    dbCustomerName = dbCustomers;
                    dbCustomerCommodity = dbCustomers?.ToList().SelectMany(x => x.CustomerCommodity).ToList();
                    dbTechnicalspecialist = dbTechnicalSpecialists;
                    _repository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistCustomerApproval>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbCustomerCode"] = dbCustomerName;
                        opt.Items["DbTechnicalspecialists"] = dbTechnicalspecialist;
                        opt.Items["DbCustomerCommodity"] = dbCustomerCommodity;
                    });

                    _repository.Add(mappedRecords);

                    if (commitChange)
                    {
                        var savedCnt = _repository.ForceSave();
                        dbTscustApproval = mappedRecords;
                        if (savedCnt > 0)
                        {
                            ProcessTsCustAppNotes(recordToBeAdd, mappedRecords, ValidationType.Add, ref validationMessages);
                            ProcessTsCustAppDocuments(recordToBeAdd, mappedRecords, ref validationMessages);

                        }

                        if (recordToBeAdd?.Count > 0)
                        {
                            int i = 0;
                            recordToBeAdd?.ToList().ForEach(x1 =>
                            {
                                var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                if (newDocuments != null && newDocuments.Count > 0)
                                    x1.DocumentName = string.Join(",", newDocuments);

                                x1.Id = mappedRecords[i++].Id;// def1035 test 2
                                _auditSearchService.AuditLog(x1, ref eventId, tsCustomerApprovalInfos?.FirstOrDefault()?.ActionByUser,
                                                                                           null,
                                                                                             ValidationType.Add.ToAuditActionType(),
                                                                                             SqlAuditModuleType.TechnicalSpecialistCustomerApproval,
                                                                                             null,
                                                                                            x1
                                                                                            );
                            });
                        }



                    }


                }
                else
                    return valdResponse;
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

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        #endregion

        #region Modify

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApproval,
                                           ref IList<TechnicalSpecialistCustomerApprovalInfo> filteredTsCustApproval,
                                           ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApproval,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomers,
                                           ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {
            bool result = false;
            if (tsCustomerApproval != null && tsCustomerApproval.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsCustApproval == null || filteredTsCustApproval.Count <= 0)
                    filteredTsCustApproval = FilterRecord(tsCustomerApproval, validationType);

                if (filteredTsCustApproval?.Count > 0 && IsValidPayload(filteredTsCustApproval, validationType, ref messages))
                {
                    GetTsCustomerApprovalDbInfo(filteredTsCustApproval, ref dbTsCustApproval);
                    IList<int> tsCustAppIds = filteredTsCustApproval.Select(x => x.Id).ToList();
                    IList<int> tsDbCustAppIds = dbTsCustApproval.Select(x => x.Id).ToList();
                    if (tsCustAppIds.Any(x => !tsDbCustAppIds.Contains(x))) //Invalid TechSpecialist PaySchedule Id found.
                    {
                        var dbTsInfosByIds = dbTsCustApproval;
                        var idNotExists = tsCustAppIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistPaySchedulesList = filteredTsCustApproval;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tscustApp = techSpecialistPaySchedulesList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tscustApp, MessageType.TsCustomerApprovalUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft || IsRecordUpdateCountMatching(filteredTsCustApproval, dbTsCustApproval, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsCustApproval.Select(x => x.Epin.ToString()).ToList();
                            IList<string> customerCodes = filteredTsCustApproval.Where(x => !string.IsNullOrEmpty(x.CustomerCode)).Select(x => x.CustomerCode).ToList();
                            //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            if (result && customerCodes?.Count > 0)
                                result = _customerService.IsValidCustomer(customerCodes, ref dbCustomers, ref validationMessages);
                            //    if (result)
                            //        //result = IsTSPayScheduleUnique(filteredTsCustApproval, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }


        private void GetTsCustomerApprovalDbInfo(IList<TechnicalSpecialistCustomerApprovalInfo> filteredTsCustApproval,
                                        ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApproval)
        {
            dbTsCustApproval = dbTsCustApproval ?? new List<DbModel.TechnicalSpecialistCustomerApproval>();
            IList<int> tslangCapabilityIds = filteredTsCustApproval?.Select(x => x.Id).Distinct().ToList();
            if (tslangCapabilityIds?.Count > 0 && (dbTsCustApproval.Count <= 0 || dbTsCustApproval.Any(x => !tslangCapabilityIds.Contains(x.Id))))
            {
                var tsCustomerApprovals = GetCustomerApprovalById(tslangCapabilityIds);
                if (tsCustomerApprovals != null && tsCustomerApprovals.Any())
                {
                    dbTsCustApproval.AddRange(tsCustomerApprovals);
                }
            }
        }


        //private bool IsTSPayScheduleUnique(IList<TechnicalSpecialistCustomerApprovalInfo> filteredTslangCapability,
        //                                ref IList<ValidationMessage> validationMessages)
        //{
        //    List<ValidationMessage> messages = new List<ValidationMessage>();
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    var tslangCapabilities = filteredTslangCapability.Select(x => new { x.Epin, x.CustomerCode, x.Id });
        //    var dbTslangCapabilities = _repository.FindBy(x => tslangCapabilities.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.CustomerCode == x.Customer.Code & x1.Id != x.Id)).ToList();
        //    if (dbTslangCapabilities?.Count > 0)
        //    {
        //        var tslangcapabilityExist = filteredTslangCapability.Where(x => dbTslangCapabilities.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin && x.CustomerCode == x1.Customer.Code));
        //        tslangcapabilityExist?.ToList().ForEach(x =>
        //        {
        //            messages.Add(_messages, x, MessageType.langCapabilitiesAlreadyExist, x.Epin, x.CustomerName);
        //        });
        //    }

        //    if (messages.Count > 0)
        //        validationMessages.AddRange(messages);

        //    return messages?.Count <= 0;
        //}

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApproval,
                                               IList<DbModel.TechnicalSpecialistCustomerApproval> dbCustApproval,
                                               ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsCustomerApproval.Where(x => !dbCustApproval.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsCustomerApprovalUpdatedByOther, x.CustomerCode);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response UpdateTsCustomerApproval(IList<TechnicalSpecialistCustomerApprovalInfo> tscustApproval,
                                     ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTscustApproval,
                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                     ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomers,
                                     bool commitChange = true,
                                     bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.TechnicalSpecialistCustomers> customers = null;
            IList<DbModel.CustomerCommodity> dbCustomerCommodity = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistCustomerApprovalInfo> recordToBeModify = null;
            long? eventId = 0;

            bool valdResult = false;
            try
            {
                eventId = tscustApproval?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tscustApproval, ValidationType.Update, ref recordToBeModify, ref dbTscustApproval, ref dbTechSpecialists, ref dbCustomers);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (!isDbValidationRequired && tscustApproval?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tscustApproval, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTscustApproval == null || (dbTscustApproval?.Count <= 0 && !valdResult))
                    {
                        dbTscustApproval = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult))
                    {
                        //valdResult = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if (dbCustomers == null || (dbCustomers?.Count <= 0 && !valdResult))
                    {
                        valdResult = _customerService.IsValidCustomer(recordToBeModify.Select(x => x.CustomerCode).ToList(), ref dbCustomers, ref validationMessages);
                    }

                    if (!isDbValidationRequired || (valdResult && dbTscustApproval?.Count > 0))
                    {
                        IList<TechnicalSpecialistCustomerApprovalInfo> domExistanceTechSplCustomerApproval = new List<TechnicalSpecialistCustomerApprovalInfo>();

                        dbTechSpecialists = dbTechnicalSpecialists;
                        customers = dbCustomers;
                        dbCustomerCommodity = dbCustomers?.ToList().SelectMany(x => x.CustomerCommodity).ToList();

                        TechnicalSpecialistCustomerApprovalInfo technicalSpecialistCustomerApprovalInfo = new TechnicalSpecialistCustomerApprovalInfo();

                        dbTscustApproval.ToList().ForEach(tsPayInfo =>
                        {

                            technicalSpecialistCustomerApprovalInfo = _mapper.Map<TechnicalSpecialistCustomerApprovalInfo>(tsPayInfo);
                            var oldDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsPayInfo.Id)?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.DocumentName)?.ToList();
                            if (oldDocuments != null && oldDocuments.Count > 0)
                                technicalSpecialistCustomerApprovalInfo.DocumentName = string.Join(",", oldDocuments);

                            domExistanceTechSplCustomerApproval.Add(ObjectExtension.Clone(technicalSpecialistCustomerApprovalInfo));
                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsPayInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsPayInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbCustomerCode"] = customers;
                                    opt.Items["DbTechnicalspecialists"] = dbTechSpecialists;
                                    opt.Items["DbCustomerCommodity"] = dbCustomerCommodity;
                                });
                                tsPayInfo.LastModification = DateTime.UtcNow;
                                tsPayInfo.UpdateCount = tsPayInfo.UpdateCount.CalculateUpdateCount();
                                tsPayInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTscustApproval);
                        if (commitChange)
                        {
                            var savedCnt = _repository.ForceSave();
                            if (savedCnt > 0)
                            {
                                ProcessTsCustAppNotes(recordToBeModify, dbTscustApproval, ValidationType.Update, ref validationMessages);
                                ProcessTsCustAppDocuments(recordToBeModify, dbTscustApproval, ref validationMessages);
                                if (recordToBeModify?.Count > 0)
                                {
                                    recordToBeModify?.ToList().ForEach(x1 =>
                                    {
                                        var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                        if (newDocuments != null && newDocuments.Count > 0)
                                            x1.DocumentName = string.Join(",", newDocuments);

                                        _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                 null,
                                                                 ValidationType.Update.ToAuditActionType(),
                                                                  SqlAuditModuleType.TechnicalSpecialistCustomerApproval,
                                                                  domExistanceTechSplCustomerApproval?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                     x1
                                                                    );
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tscustApproval);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                            ref IList<TechnicalSpecialistCustomerApprovalInfo> filteredTsCustomerApprovalInfos,
                                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsInfos,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCustomerApprovalInfos != null && tsCustomerApprovalInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsCustomerApprovalInfos == null || filteredTsCustomerApprovalInfos.Count <= 0)
                    filteredTsCustomerApprovalInfos = FilterRecord(tsCustomerApprovalInfos, validationType);

                if (filteredTsCustomerApprovalInfos?.Count > 0 && IsValidPayload(filteredTsCustomerApprovalInfos, validationType, ref validationMessages))
                {
                    GetTsCustomerApprovalDbInfo(filteredTsCustomerApprovalInfos, false, ref dbTsInfos);

                    IList<KeyValuePair<string, string>> tsCustomerApprovalNotExists = null;
                    var tsPinAndCustomerName = filteredTsCustomerApprovalInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.CustomerName))
                                                            .Distinct()
                                                            .ToList();
                    result = IsCustomerApprovalInfoExistInDb(tsPinAndCustomerName, dbTsInfos, ref tsCustomerApprovalNotExists, ref validationMessages);
                    if (result)
                        result = IsTsCustomerNameCanBeRemove(dbTsInfos, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsTsCustomerNameCanBeRemove(IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerNameInfos,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsCustomerNameInfos?.Where(x => x.IsAnyCollectionPropertyContainValue())
                            .ToList()
                            .ForEach(x =>
                            {
                                messages.Add(_messages, x.Customer.Name, MessageType.TsCustomerNameIsBeingUsed, x.Customer.Name);
                            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private void GetTsCustomerApprovalDbInfo(IList<TechnicalSpecialistCustomerApprovalInfo> filteredTsCustomerApprovalInfos,
                                        bool isTsCustomerApprovalInfoById,
                                        ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos)
        {
            var tsPins = !isTsCustomerApprovalInfoById ?
                            filteredTsCustomerApprovalInfos.Select(x => x.Epin.ToString()).Distinct().ToList() :
                            null;
            IList<int> tsIds = isTsCustomerApprovalInfoById ?
                                filteredTsCustomerApprovalInfos.Select(x => x.Id).Distinct().ToList() :
                                null;

            if (dbTsCustomerApprovalInfos == null || dbTsCustomerApprovalInfos.Count <= 0)
                dbTsCustomerApprovalInfos = isTsCustomerApprovalInfoById ?
                                    GetCustomerApprovalById(tsIds).ToList() :
                                    GetCustomerApprovalInfoByPin(tsPins).ToList();
        }

        private IList<TechnicalSpecialistCustomerApprovalInfo> FilterRecord(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                                                    ValidationType filterType)
        {
            IList<TechnicalSpecialistCustomerApprovalInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsCustomerApprovalInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndCustomerName,
                                                     IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos,
                                                     ref IList<KeyValuePair<string, string>> tsCustApprNotExists,
                                                     ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsCustApprInfos = dbTsCustApprInfos ?? new List<DbModel.TechnicalSpecialistCustomerApproval>();
            if (tsPinAndCustomerName?.Count > 0 && dbTsCustApprInfos?.Count > 0)
            {
                tsCustApprNotExists = tsPinAndCustomerName.Where(info => !dbTsCustApprInfos.Any(x1 => x1.TechnicalSpecialist.Pin.ToString() == info.Key &&
                                                                                                      x1?.Customer?.Name == info.Value))
                                                          .Select(x => x).ToList();

                tsCustApprNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCustomerApprovalDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        //private bool IsCustomerApprovalInfoExistInDb(IList<int> tsStampIds,
        //                                    IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsStampInfos,
        //                                    ref IList<int> tsStampIdNotExists,
        //                                    ref IList<ValidationMessage> validationMessages)
        //{
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    if (dbTsStampInfos == null)
        //        dbTsStampInfos = new List<DbModel.TechnicalSpecialistCustomerApproval>();

        //    var validMessages = validationMessages;

        //    if (tsStampIds?.Count > 0)
        //    {
        //        tsStampIdNotExists = tsStampIds.Where(x => !dbTsStampInfos.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
        //        tsStampIdNotExists?.ToList().ForEach(x =>
        //        {
        //            validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
        //        });
        //    }

        //    if (validMessages.Count > 0)
        //        validationMessages = validMessages;

        //    return validationMessages.Count <= 0;
        //}

        private Response RemoveTechSpecialistCustomerApproval(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos,
                                                    bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<TechnicalSpecialistCustomerApprovalInfo> recordToBeDeleted = null;
            long? eventId = 0;

            try
            {
                eventId = tsCustomerApprovalInfos?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsCustomerApprovalInfos, ValidationType.Delete, ref dbTsCustApprInfos);

                if (tsCustomerApprovalInfos?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsCustomerApprovalInfos, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsCustApprInfos?.Count > 0)
                {
                    var dbTsCustApprToBeDeleted = dbTsCustApprInfos?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();

                    _repository.AutoSave = false;
                    _repository.Delete(dbTsCustApprToBeDeleted);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();

                        if (recordToBeDeleted.Count > 0 && value > 0)
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                    null,
                                                                                                    ValidationType.Delete.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.TechnicalSpecialistCustomerApproval,
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
        private Response CheckRecordValidForProcess(
                                            IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistCustomerApprovalInfo> filteredTsCustApprInfos,
                                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomers,
                                            bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsCustomerApprovalInfos,
                                                 ref filteredTsCustApprInfos,
                                                 ref dbTsCustApprInfos,
                                                 ref dbCustomers,
                                                 ref dbTechnicalSpecialists,
                                                 ref validationMessages
                                                 );
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsCustomerApprovalInfos, ref filteredTsCustApprInfos, ref dbTsCustApprInfos, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsCustomerApprovalInfos,
                                                 ref filteredTsCustApprInfos,
                                                 ref dbTsCustApprInfos,
                                                 ref dbTechnicalSpecialists,
                                                 ref dbCustomers,
                                                 ref validationMessages,
                                                 isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustomerApprovalInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsValidPayload(IList<TechnicalSpecialistCustomerApprovalInfo> ts,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(ts), validationType);
            foreach (var item in ts)
            {
                if (item.FromDate != null && item.ToDate != null)
                    if (item.ToDate < item.FromDate)
                        messages.Add(_messages, item.FromDate, MessageType.TsPayRateExpectedFrom, item.ToDate);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);
            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private IList<TechnicalSpecialistCustomerApprovalInfo> PopulateTsCustApprovalDocuments(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustApprovals)
        {
            try
            {
                if (tsCustApprovals?.Count > 0)
                {
                    var epins = tsCustApprovals.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var custAppIds = tsCustApprovals.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsCustAppDocs = _documentService.Get(ModuleCodeType.TS, epins, custAppIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsCustAppDocs?.Count > 0)
                    {
                        return tsCustApprovals.GroupJoin(tsCustAppDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsCert = tsc, doc }).Select(x =>
                            {
                                x.tsCert.Documents = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_CustomerApproval.ToString()).ToList();
                                return x.tsCert;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustApprovals);
            }

            return tsCustApprovals;
        }

        private Response ProcessTsCustAppDocuments(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustApproval, IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprovals, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tsCustApproval?.Count > 0)
                {
                    
                    for (int comCnt = 0; comCnt < tsCustApproval?.Count; comCnt++) //Changes for Live D656
                    {
                        if (dbTsCustApprovals[comCnt] != null && tsCustApproval[comCnt].Documents != null)
                        {
                            tsCustApproval[comCnt]?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = tsCustApproval[comCnt]?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = dbTsCustApprovals[comCnt].Id.ToString();
                                x1.ModuleRefCode = tsCustApproval[comCnt]?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_CustomerApproval.ToString();
                            });
                        }
                    }

                    //tsCustApproval = tsCustApproval.Join(dbTsCustApprovals,
                    //    tsc => new { customerCode = tsc.CustomerCode },
                    //    dbtsc => new { customerCode = dbtsc?.Customer?.Code },
                    //    (tsc, dbtsc) => new { tsCert = tsc, dbtsc }).Select(x =>
                    //    {
                    //        x.tsCert?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                    //        {
                    //            if (x1.RecordStatus.IsRecordStatusDeleted())
                    //                x1.RecordStatus = RecordStatus.Delete.FirstChar();
                    //            else
                    //            {
                    //                x1.RecordStatus = RecordStatus.Modify.FirstChar();
                    //                x1.ModifiedBy = x.tsCert?.ModifiedBy;
                    //            }
                    //            x1.SubModuleRefCode = x.dbtsc.Id.ToString();
                    //            x1.ModuleRefCode = x?.tsCert?.Epin.ToString();
                    //            x1.DocumentType = DocumentType.TS_CustomerApproval.ToString();
                    //        });
                    //        return x.tsCert;
                    //    }).ToList();


                    var tsDocToBeProcess = tsCustApproval?.Where(x => x.Documents != null &&
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustApproval);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistCustomerApprovalInfo> PopulateTsCustApprovalNotes(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustApproval)
        {
            try
            {
                if (tsCustApproval?.Count > 0)
                {
                    var epins = tsCustApproval.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var certificationIds = tsCustApproval.Select(x => x.Id).Distinct().ToList();
                    var tsCustApprovalComments = _tsNoteService.Get(NoteType.CED, true, epins, certificationIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                    if (tsCustApprovalComments?.Count > 0)
                    {
                        return tsCustApproval.GroupJoin(tsCustApprovalComments,
                            tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                            note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                            (tsc, note) => new { tsCert = tsc, note }).Select(x =>
                            {
                                //x.tsCert.Comments = x?.note?.FirstOrDefault()?.Note; //Commented for Hot Fix D656
                                return x.tsCert;
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustApproval);
            }

            return tsCustApproval;
        }

        private Response ProcessTsCustAppNotes(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustApprovals, IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprovals, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistNoteInfo> tsNewCertNotes = null;
            try
            {
                if (tsCustApprovals?.Count > 0)
                {
                    if (validationType == ValidationType.Add)
                    {
                        tsNewCertNotes = tsCustApprovals.Join(dbTsCustApprovals,
                             tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.CustomerName },
                             dbtsc => new { epin = dbtsc.TechnicalSpecialistId.ToString(), RefId = dbtsc.Customer.Name },
                             (tsc, dbtsc) => new { tsc, dbtsc }).Select(x =>
                             new TechnicalSpecialistNoteInfo
                             {
                                 Epin = x.tsc.Epin,
                                 RecordType = NoteType.CED.ToString(),
                                 RecordRefId = x.dbtsc.Id,
                                 Note = x.tsc.Comments,
                                 RecordStatus = RecordStatus.New.FirstChar(),
                                 CreatedBy = x.tsc.ActionByUser,
                                 CreatedDate = DateTime.UtcNow,
                                 EventId = x.tsc.EventId,
                                 ActionByUser = x.tsc.ActionByUser

                             }).ToList();

                    }
                    else if (validationType == ValidationType.Update)
                    {
                        var epins = tsCustApprovals.Select(x => x.Epin.ToString()).Distinct().ToList();
                        var certificationIds = tsCustApprovals.Select(x => x.Id).Distinct().ToList();
                        var tsCertificationNotes = _tsNoteService.Get(NoteType.CED, true, epins, certificationIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                        tsNewCertNotes = tsCertificationNotes.Join(tsCustApprovals,
                             note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                             tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                             (note, tsc) => new { note, tsCert = tsc }).Where(x => !string.Equals(x.note.Note, x.tsCert.Comments)).Select(x =>
                             {
                                 x.note.Note = x.tsCert.Comments;
                                 x.note.RecordStatus = RecordStatus.New.FirstChar();
                                 x.note.CreatedBy = x.tsCert.ActionByUser;
                                 x.note.CreatedDate = DateTime.UtcNow;
                                 x.note.EventId = x.tsCert.EventId;
                                 x.note.ActionByUser = x.tsCert.ActionByUser;
                                 return x.note;
                             }).ToList();

                    }
                    return _tsNoteService.Add(tsNewCertNotes);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustApprovals);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
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

        #endregion


        #endregion
    }
}










