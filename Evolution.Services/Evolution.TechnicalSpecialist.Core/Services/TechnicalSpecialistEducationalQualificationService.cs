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
    public class TechnicalSpecialistEducationalQualificationService : ITechnicalSpecialistEducationalQualificationService
    {

        private readonly IAppLogger<TechnicalSpecialistEducationalQualificationInfo> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistEducationalQualificationRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistQualificationValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _tsInfoServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IDocumentService _documentService = null;
        private readonly ICountryService _countryService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        


        #region Constructor
        public TechnicalSpecialistEducationalQualificationService(IMapper mapper,
                                                    ITechnicalSpecialistEducationalQualificationRepository repository,
                                                    IAppLogger<TechnicalSpecialistEducationalQualificationInfo> logger,
                                                    ITechnicalSpecialistQualificationValidationService validationService,
                                                    //ITechnicalSpecialistService tsInfoServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    ICountryService countryService,
                                                    IDocumentService documentService,
                                                    JObject messages,
                                                   IAuditSearchService auditSearchService
                                                    )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            //_tsInfoServices = tsInfoServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _documentService = documentService;
            _auditSearchService = auditSearchService;
            
        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistEducationalQualificationInfo searchModel)
        {
            IList<TechnicalSpecialistEducationalQualificationInfo> result = null;
            Exception exception = null;
            try
            {
                var tsEduQulification = _mapper.Map<IList<TechnicalSpecialistEducationalQualificationInfo>>(_repository.Search(searchModel));
                result = PopulateTsEducationalDocument(tsEduQulification);
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
            IList<TechnicalSpecialistEducationalQualificationInfo> result = null;
            Exception exception = null;
            try
            {
                var tsEduQulification = _mapper.Map<IList<TechnicalSpecialistEducationalQualificationInfo>>(GetEduQulificationInfoByPin(pins));
                result = PopulateTsEducationalDocument(tsEduQulification);
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
            IList<TechnicalSpecialistEducationalQualificationInfo> result = null;
            Exception exception = null;
            try
            {
                var tsEduQulification = _mapper.Map<IList<TechnicalSpecialistEducationalQualificationInfo>>(GetEduQulificationById(WorkHistoryIds));
                result = PopulateTsEducationalDocument(tsEduQulification);
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

        public Response Add(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Country> Country = null;
            return AddTechSpecialisttsEduQulification(tsEduQulification, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Country> Country, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialisttsEduQulification(tsEduQulification, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Country> Country = null;

            return UpdateTechSpecialisttsEduQulification(tsEduQulification, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Country> Country, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialisttsEduQulification(tsEduQulification, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification = null;
            return RemoveTechSpecialisttsEduQulification(tsEduQulification,ref dbtsEduQulification, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialisttsEduQulification(tsEduQulification, ref dbtsEduQulification,commitChange, isDbValidationRequired);
        }


        #endregion
         
        #region Validation
         
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulificationInfos = null;
            return IsRecordValidForProcess(tsEduQulificationInfos, validationType, ref dbTsEduQulificationInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulificationInfos)
        {
            IList<TechnicalSpecialistEducationalQualificationInfo> filteredTechSpecialist = null;
            IList<DbModel.Country> dbCountry = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist = null;

            return CheckRecordValidForProcess(tsEduQulificationInfos, validationType, ref filteredTechSpecialist, ref dbTsEduQulificationInfos, ref dbTechnicalpecialist, ref dbCountry);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulification, ref IList<DbModel.Country> dbCountry, ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialists, bool isDraft = false)
        {
            IList<TechnicalSpecialistEducationalQualificationInfo> filteredTechSpecialist = null;
            return CheckRecordValidForProcess(tsEduQulificationInfos, validationType, ref filteredTechSpecialist, ref dbTsEduQulification, ref dbTechnicalpecialists, ref dbCountry, isDraft);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
            ValidationType validationType,
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulificationInfos)
        {
            return IsRecordValidForProcess(tsEduQulificationInfos, validationType, ref dbTsEduQulificationInfos);
        }

        public Response IsRecordExistInDb(IList<int> tstsEduQulificationIds, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tstsEduQulificationIdNotExists = null;
            return IsRecordExistInDb(tstsEduQulificationIds, ref dbtsEduQulification, ref tstsEduQulificationIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tstsEduQulificationIds, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification, ref IList<int> tstsEduQulificationIdNotExists, ref IList<ValidationMessage> validationMessages)
        {

            Exception exception = null;
            bool result = true;
            try
            {
                if (dbtsEduQulification == null && tstsEduQulificationIds?.Count > 0)
                    dbtsEduQulification = GettsEduQulificationInfoById(tstsEduQulificationIds);

                result = IsTStsEduQulificationExistInDb(tstsEduQulificationIds, dbtsEduQulification, ref tstsEduQulificationIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tstsEduQulificationIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
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

        #region Get
        private IList<DbModel.TechnicalSpecialistEducationalQualification> GetEduQulificationInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulificationInfos = null;
            if (pins?.Count > 0)
            {
                dbtsEduQulificationInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsEduQulificationInfos;
        }

        private IList<DbModel.TechnicalSpecialistEducationalQualification> GetEduQulificationById(IList<int> tsEduQulificationIds)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulificationInfos = null;
            if (tsEduQulificationIds?.Count > 0)
                dbtsEduQulificationInfos = _repository.FindBy(x => tsEduQulificationIds.Contains((int)x.Id)).ToList();

            return dbtsEduQulificationInfos;
        }

        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<TechnicalSpecialistEducationalQualificationInfo> tsCustApprInfos,
                                         ref IList<TechnicalSpecialistEducationalQualificationInfo> filteredTsCustApprInfos,
                                         ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsCustApprInfos,
                                         ref IList<DbModel.Data> dbCountry,
                                         ref IList<DbModel.Data> dbCounty,
                                         ref IList<DbModel.Data> dbCity,
                                         ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCustApprInfos != null && tsCustApprInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsCustApprInfos == null || filteredTsCustApprInfos.Count <= 0)
                    filteredTsCustApprInfos = FilterRecord(tsCustApprInfos, validationType);

                if (filteredTsCustApprInfos?.Count > 0 && IsValidPayload(filteredTsCustApprInfos, validationType, ref validationMessages))
                {
                    //IList<KeyValuePair<string, string>> tsCustAprovalNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();

                    // IList<string> CustomerCode = filteredTsCustApprInfos.Select(x => x.CustomerCode).ToList();
                    IList<string> epins = filteredTsCustApprInfos.Select(x => x.Epin.ToString()).ToList();

                    // var dbMaster = this.GetMasterData(filteredTechSpecialistCustomerApproval, ref dbCustomerName);

                    // result = _customerService.IsValidCustomer(CustomerCode, ref dbTsCustomer, ref validationMessages);

                    // if (result && epins?.Count > 0)
                    //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(epins, ref dbTsInfos, ref validationMessages, tsca => tsca.TechnicalSpecialistEducationalQualification).Result);
                    result = IsTechSpecialistExistInDb(epins, ref dbTsInfos, ref validationMessages, tsca => tsca.TechnicalSpecialistEducationalQualification);
                    if (!result)
                    {

                        messages.Add(_messages, null, MessageType.TsEPinAlreadyExist, result);

                        if (messages.Count > 0)
                            validationMessages.AddRange(messages);
                    }
                }

            }
            return result;
        }

        private Response AddTsEducationalQulificationApproval(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulificationInfos,
                                            bool commitChange = true,
                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Country> DbCountry = null;
                //IList<DbModel.Data> county = null;
                //IList<DbModel.Data> city = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalspecialist = null;
                IList<TechnicalSpecialistEducationalQualificationInfo> recordToBeAdd = null;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsEduQulificationInfos, ValidationType.Add, ref recordToBeAdd, ref dbTsEduQulificationInfos, ref dbTechnicalspecialist, ref DbCountry);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistEducationalQualification>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechnicalspecialists"] = dbTechnicalspecialist;
                    });

                    _repository.Add(mappedRecords);

                    if (commitChange)
                    {
                        _repository.ForceSave();
                        dbTsEduQulificationInfos = _repository.Get(recordToBeAdd.Select(x => x.Id).ToList());
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulificationInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        #endregion
         
        #region Remove
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
                                            ref IList<TechnicalSpecialistEducationalQualificationInfo> filteredTsEduQulificationInfos,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsInfos,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsEduQulificationInfos != null && tsEduQulificationInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsEduQulificationInfos == null || filteredTsEduQulificationInfos.Count <= 0)
                    filteredTsEduQulificationInfos = FilterRecord(tsEduQulificationInfos, validationType);

                if (filteredTsEduQulificationInfos?.Count > 0 && IsValidPayload(filteredTsEduQulificationInfos, validationType, ref validationMessages))
                {
                    GetTsEduApprovalDbInfo(filteredTsEduQulificationInfos, ref dbTsInfos);

                    //IList< string> tsStampNotExists = null;
                    ////var tsPinAndCustomerName = filteredTsEduQulificationInfos.Select(x => (x.Id))
                    ////                                        .Distinct()
                    ////                                        .ToList();
                    ////result = IsEduApprovalInfoExistInDb(tsPinAndCustomerName, dbTsInfos, ref tsStampNotExists, ref validationMessages);
                    //if (result)
                    //{

                    //}

                    result = IsTsEduQulificationCanBeRemove(dbTsInfos, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsTsEduQulificationCanBeRemove(IList<DbModel.TechnicalSpecialistEducationalQualification>  DbTsQualifications,
                                        ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ?? new List<ValidationMessage>();

            DbTsQualifications?.Where(x => x.IsAnyCollectionPropertyContainValue())
                            .ToList()
                            .ForEach(x =>
                            {
                                messages.Add(_messages, x, MessageType.TsEduQulificationIsBeingUsed, x.Qualification);
                            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        //private bool IsEduApprovalInfoExistInDb(List<int> tsPinAndCustomerName,
        //                                        IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsInfos, 
        //                                         ref IList<string> tsStampNotExists,
        //                                         ref IList<ValidationMessage> validationMessages)
        //{
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    if (dbTsInfos == null)
        //        dbTsInfos = new List<DbModel.TechnicalSpecialistEducationalQualification>();

        //    var validMessages = validationMessages;

        //    if (tsPinAndCustomerName?.Count > 0)
        //    {
        //        tsStampNotExists = tsPinAndCustomerName.Where(Id => !dbTsInfos.Any(x1 => x1.Id==Id))
        //                             .Select(id => id)
        //                             .ToList();

        //        tsStampNotExists?.ToList().ForEach(x =>
        //        {
        //            validMessages.Add(_messages, x, MessageType.TsPayScheduleIdDoesNotExist, x);
        //        });
        //    }

        //    if (validMessages.Count > 0)
        //        validationMessages = validMessages;

        //    return validationMessages.Count <= 0;
        //}


        private void GetTsEduApprovalDbInfo(IList<TechnicalSpecialistEducationalQualificationInfo> filteredEduQulifications,
                                        ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbEduQulifications)
        {
            IList<int> tsPayScheduleIds = filteredEduQulifications?.Select(x => x.Id).Distinct().ToList();
            if (dbEduQulifications == null || dbEduQulifications.Count <= 0)
                dbEduQulifications = GetEduQulificationInfoById(tsPayScheduleIds);
        }

        private IList<DbModel.TechnicalSpecialistEducationalQualification> GetEduQulificationInfoById(IList<int> tsEduQulificationIds)
        {

            IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsPayScheduleInfos = null;
            if (tsEduQulificationIds?.Count > 0)
                dbTsPayScheduleInfos = _repository.FindBy(x => tsEduQulificationIds.Contains(x.Id)).ToList();

            return dbTsPayScheduleInfos;

        }
         
        #endregion

        #region Private Methods

        private IList<DbModel.TechnicalSpecialistEducationalQualification> GettsEduQulificationInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbTstsEduQulificationInfos = null;
            if (pins?.Count > 0)
            {
                dbTstsEduQulificationInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTstsEduQulificationInfos;
        }

        private IList<DbModel.TechnicalSpecialistEducationalQualification> GettsEduQulificationInfoById(IList<int> tstsEduQulificationIds)
        {
            IList<DbModel.TechnicalSpecialistEducationalQualification> dbTstsEduQulificationInfos = null;
            if (tstsEduQulificationIds?.Count > 0)
                dbTstsEduQulificationInfos = _repository.FindBy(x => tstsEduQulificationIds.Contains((int)x.Id)).ToList();

            return dbTstsEduQulificationInfos;
        }

        //private IList<DbModel.TechnicalSpecialistEducationalQualification> GettsEduQulificationInfoByScheduleNames(IList<string> tstsEduQulificationNames)
        //{
        //    IList<DbModel.TechnicalSpecialistEducationalQualification> dbTstsEduQulificationInfos = null;
        //    if (tstsEduQulificationNames?.Count > 0)
        //       // dbTstsEduQulificationInfos = _repository.FindBy(x => tstsEduQulificationNames.Contains(x.TechnicalSpecialist)).ToList();

        //    return dbTstsEduQulificationInfos;
        //}

        private Response AddTechSpecialisttsEduQulification(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Country> Country,
                                            bool commitChange = true,
                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
           
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistEducationalQualificationInfo> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                eventId = tsEduQulification?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                {
                    valdResponse = CheckRecordValidForProcess(tsEduQulification, ValidationType.Add, ref recordToBeAdd, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country);
                }

                if (!isDbValidationRequire && tsEduQulification?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsEduQulification, ValidationType.Add);
                }

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && recordToBeAdd?.Count > 0))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;

                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistEducationalQualification>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                    });
                    _repository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _repository.ForceSave();
                        dbtsEduQulification = mappedRecords;
                        if (savedCnt > 0)
                        {

                           ProcessTsEduQulificationDocuments(recordToBeAdd, mappedRecords, ref validationMessages); 
                        }
                    }
                    if (mappedRecords?.Count > 0)
                    {
                        mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsEduQulification?.FirstOrDefault()?.ActionByUser,
                                                                                            null,
                                                                                            ValidationType.Add.ToAuditActionType(),
                                                                                             SqlAuditModuleType.TechnicalSpecialistEducationalQualification,
                                                                                             null,
                                                                                              _mapper.Map<TechnicalSpecialistEducationalQualificationInfo>(x1)
                                                                                             ));
                    }
                 }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulification);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialisttsEduQulification(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                                       ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                       ref IList<DbModel.Country> Country,
                                       bool commitChange = true,
                                       bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistEducationalQualificationInfo> recordToBeModify = null;
            long? eventId = 0;
         
            bool valdResult = false;
            try
            {
                eventId = tsEduQulification?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsEduQulification, ValidationType.Update, ref recordToBeModify, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (!isDbValidationRequired && tsEduQulification?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsEduQulification, ValidationType.Update);
                }

                if (recordToBeModify?.Count > 0)
                {
                    if (dbtsEduQulification == null || (dbtsEduQulification?.Count <= 0 && valdResult == false))
                    {
                        dbtsEduQulification = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && valdResult == false))
                    {
                        //valdResult = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    //if ((Country == null || (Country?.Count <= 0 && valdResult == false))
                    //{
                    //    valdResult = _countryService.IsValidCountryName(recordToBeModify.Select(x => x.CountryName).ToList(), ref Country, ref validationMessages);
                    //}

                    if (!isDbValidationRequired || (valdResult && dbtsEduQulification?.Count > 0))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                         IList<TechnicalSpecialistEducationalQualificationInfo> domExsistanceTechSplEducationalQualification = new List<TechnicalSpecialistEducationalQualificationInfo>();
                        
                        dbtsEduQulification.ToList().ForEach(tsEduQulificationInfo =>
                        {
                            domExsistanceTechSplEducationalQualification.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistEducationalQualificationInfo>(tsEduQulificationInfo)));
                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsEduQulificationInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsEduQulificationInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                });
                                tsEduQulificationInfo.LastModification = DateTime.UtcNow;
                                tsEduQulificationInfo.UpdateCount = tsEduQulificationInfo.UpdateCount.CalculateUpdateCount();
                                tsEduQulificationInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                            
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbtsEduQulification);
                        if (commitChange)
                        {
                            var savedCnt = _repository.ForceSave();
                            if (savedCnt > 0)
                            {
                               ProcessTsEduQulificationDocuments(recordToBeModify, dbtsEduQulification, ref validationMessages);

                                if (recordToBeModify?.Count > 0)
                                {
                                    recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                     null,
                                                                                                     ValidationType.Update.ToAuditActionType(),
                                                                                                     SqlAuditModuleType.TechnicalSpecialistEducationalQualification,
                                                                                                     domExsistanceTechSplEducationalQualification?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                                                     x1
                                                                                                     ));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulification);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialisttsEduQulification(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                                        bool commitChange = true,
                                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<TechnicalSpecialistEducationalQualificationInfo> recordToBeDeleted = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
           
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsEduQulification?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsEduQulification, ValidationType.Delete, ref dbtsEduQulification);
                 
                if (tsEduQulification?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsEduQulification, ValidationType.Delete);
                }
                 
                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbtsEduQulification?.Count > 0)
                {
                    var dbTsEduQulificationToBeDeleted = dbtsEduQulification?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();

                    _repository.AutoSave = false;
                    _repository.Delete(dbTsEduQulificationToBeDeleted);
                    if (commitChange)
                    {

                        _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0)
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                   null,
                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.TechnicalSpecialistEducationalQualification,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulification);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistEducationalQualificationInfo> filteredtsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Country> Country,
                                            bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsEduQulification, ref filteredtsEduQulification, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsEduQulification, ref filteredtsEduQulification, ref dbtsEduQulification, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsEduQulification, ref filteredtsEduQulification, ref dbtsEduQulification, ref dbTechnicalSpecialists, ref Country, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulification);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsRecordValidForAdd(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                                       ref IList<TechnicalSpecialistEducationalQualificationInfo> filteredtsEduQulification,
                                       ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                       ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                       ref IList<DbModel.Country> Countries,
                                       ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsEduQulification != null && tsEduQulification.Count > 0)
            {
                ValidationType validationType = ValidationType.Add; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredtsEduQulification == null || filteredtsEduQulification.Count <= 0)
                    filteredtsEduQulification = FilterRecord(tsEduQulification, validationType);

                if (filteredtsEduQulification?.Count > 0 && IsValidPayload(filteredtsEduQulification, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredtsEduQulification.Select(x => x.Epin.ToString()).ToList();
                    IList<string> currencies = filteredtsEduQulification.Select(x => x.CountryName).ToList();
                    //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                                            ref IList<TechnicalSpecialistEducationalQualificationInfo> filteredtsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Country> Country,
                                            ref IList<ValidationMessage> validationMessages,
                                             bool isDraft = false)
        {
            bool result = false;
            if (tsEduQulification != null && tsEduQulification.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                    validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredtsEduQulification == null || filteredtsEduQulification.Count <= 0)
                    filteredtsEduQulification = FilterRecord(tsEduQulification, validationType);

                if (filteredtsEduQulification?.Count > 0 && IsValidPayload(filteredtsEduQulification, validationType, ref messages))
                {
                    GetTstsEduQulificationDbInfo(filteredtsEduQulification, ref dbtsEduQulification);
                    IList<int> tstsEduQulificationIds = filteredtsEduQulification.Select(x => x.Id).ToList();
                    IList<int> tsDBEduQulificationIds = dbtsEduQulification.Select(x => x.Id).ToList();
                    if (tstsEduQulificationIds.Any(x=> !tsDBEduQulificationIds.Contains(x))) //Invalid TechSpecialist tsEduQulification Id found.
                    {
                        var dbTsInfosByIds = dbtsEduQulification;
                        var idNotExists = tstsEduQulificationIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialisttsEduQulificationsList = filteredtsEduQulification;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tstsEduQulification = techSpecialisttsEduQulificationsList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tstsEduQulification, MessageType.TstsEduQulificationUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredtsEduQulification, dbtsEduQulification, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredtsEduQulification.Select(x => x.Epin.ToString()).ToList();
                            IList<string> currencies = filteredtsEduQulification.Select(x => x.CountryName).ToList();
                            //result = Convert.ToBoolean(_tsInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            if (!result && currencies?.Count > 0)
                                result = _countryService.IsValidCountryName(currencies, ref Country, ref validationMessages);
                            if (!result)
                                result = IsTStsEduQulificationUnique(filteredtsEduQulification, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }



        private bool IsTStsEduQulificationExistInDb(IList<int> tstsEduQulificationIds,
                                                IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                                ref IList<int> tstsEduQulificationIdNotExists,
                                                ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages = validationMessages ??  new List<ValidationMessage>(); 
                dbtsEduQulification = dbtsEduQulification ?? new List<DbModel.TechnicalSpecialistEducationalQualification>(); 

            if (tstsEduQulificationIds?.Count > 0)
            {
                tstsEduQulificationIdNotExists = tstsEduQulificationIds.Where(id => !dbtsEduQulification.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tstsEduQulificationIdNotExists?.ToList().ForEach(x =>
                {
                    // validMessages.Add(_messages, x, MessageType.TstsEduQulificationIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialisttsEduQulificationCanBeRemove(IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbtsEduQulification?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     //messages.Add(_messages, x, MessageType.TstsEduQulificationIsBeingUsed, x.tsEduQulificationName, x.PayCurrency);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private void GetTstsEduQulificationDbInfo(IList<TechnicalSpecialistEducationalQualificationInfo> filteredtsEduQulification,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification)
        {
            dbtsEduQulification = dbtsEduQulification ?? new List<DbModel.TechnicalSpecialistEducationalQualification>();
            IList<int> tstsEduQulificationIds = filteredtsEduQulification?.Select(x => x.Id).Distinct().ToList();
            if (tstsEduQulificationIds?.Count > 0 &&  ( dbtsEduQulification.Count <= 0 || dbtsEduQulification.Any(x=>!tstsEduQulificationIds.Contains(x.Id))))
            {
                var tsEduQulifs = GettsEduQulificationInfoById(tstsEduQulificationIds);
                if (tsEduQulifs != null && tsEduQulifs.Any())
                {
                    dbtsEduQulification.AddRange(tsEduQulifs);
                }
            }  
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                                                IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var notMatchedRecords = tsEduQulification.Where(x => !dbtsEduQulification.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                // messages.Add(_messages, x, MessageType.TstsEduQulificationUpdatedByOther, x.tsEduQulificationName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTStsEduQulificationUnique(IList<TechnicalSpecialistEducationalQualificationInfo> filteredtsEduQulification,
                                            ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var tsEduQulification = filteredtsEduQulification.Select(x => new { x.Epin });
            var dbtsEduQulification = _repository.FindBy(x => tsEduQulification.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin)).ToList();
            if (dbtsEduQulification?.Count > 0)
            {
                var tstsEduQulificationAlreadyExist = filteredtsEduQulification.Where(x => dbtsEduQulification.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin));
                tstsEduQulificationAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.EducationalQulificationAlreadyExist, x.Epin, null, null);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private IList<TechnicalSpecialistEducationalQualificationInfo> FilterRecord(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, ValidationType filterType)
        {
            IList<TechnicalSpecialistEducationalQualificationInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsEduQulification?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsEduQulification?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsEduQulification?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                              ValidationType validationType,
                              ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsEduQulification), validationType);
            //if(tsEduQulification.Any(x => x.RecordStatus != "D")) //D1261 ITK
            //{
            //    if (tsEduQulification != null && tsEduQulification.Any(x => x.Documents == null || x.Documents?.Count==0))
            //        messages.Add(_messages, "Document", MessageType.QualificationDocument);
            //    else if (tsEduQulification.SelectMany(x => x.Documents).Any(x => string.IsNullOrEmpty(x.DocumentName)))
            //        messages.Add(_messages, "Document", MessageType.QualificationDocument);
            //} //Commented For IGO QC D889 #3 Issue (Validation Check Added for DetailsService.cs)
         

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);
            
            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<TechnicalSpecialistEducationalQualificationInfo> PopulateTsEducationalDocument(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulifications)
        {
            try
            {
                if (tsEduQulifications?.Count > 0)
                {
                    var epins = tsEduQulifications.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var eduQulificationIds = tsEduQulifications.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsEduQulificationDocs = _documentService.Get(ModuleCodeType.TS, epins, eduQulificationIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsEduQulificationDocs?.Count > 0)
                    {
                        return tsEduQulifications.GroupJoin(tsEduQulificationDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsCert = tsc, doc }).Select(x =>
                            {
                                x.tsCert.Documents = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_EducationQualification.ToString()).ToList(); ;
                                return x.tsCert;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulifications);
            }

            return tsEduQulifications;
        }
        //private Response ProcessTsEduQulificationDocuments(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulifications, IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulifications, ref IList<ValidationMessage> validationMessages)
        //{
        //    Exception exception = null;
        //    try
        //    {
        //        if (tsEduQulifications?.Count > 0)
        //        {

        //            tsEduQulifications.Join(dbTsEduQulifications,
        //                tsc => tsc.Epin,
        //                dbtsc => dbtsc.Id,
        //                (tsc, dbtsc) => new { tsCert = tsc, dbtsc }).Select(x =>
        //                {
        //                    x.tsCert.Documents.ToList().ForEach(x1 =>
        //                    {
        //                        x1.RecordStatus = RecordStatus.Modify.FirstChar();
        //                        x1.SubModuleRefCode = x?.dbtsc?.Id.ToString();
        //                        x1.DocumentType = DocumentType.TS_EducationQualification.ToString();
        //                    });
        //                    return x.tsCert;
        //                }).ToList();

        //            var docToModify = tsEduQulifications.SelectMany(x => x.Documents).ToList();
        //            return _documentService.Modify(docToModify);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulifications);
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        //}
        private Response ProcessTsEduQulificationDocuments(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulifications, IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulifications, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tsEduQulifications?.Count>0)
                { //Sanity defect 148 fix
                    tsEduQulifications = tsEduQulifications.Join(dbTsEduQulifications,
                        tsE => new { tsE.Qualification, tsE.Institution,tsE.ToDate },
                        dbtsc => new { dbtsc.Qualification, dbtsc.Institution, ToDate=dbtsc.DateTo },
                        (tsc, dbtsc) => new { tsEdu = tsc, dbtsc }).Select(x =>
                        {
                            x.tsEdu?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = x.tsEdu?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = x.dbtsc.Id.ToString();
                                x1.ModuleRefCode = x?.tsEdu?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_EducationQualification.ToString();
                                x1.Status = x1.Status.Trim();
                            }); 
                            return x.tsEdu;
                        }).ToList();

                    var tsDocToBeProcess = tsEduQulifications?.Where(x => x.Documents != null &&
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEduQulifications);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
   
        #endregion


        #endregion

    }
}
