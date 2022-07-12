using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Draft.Domain.Interfaces.Draft;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistDetailService : ITechnicalSpecialistDetailService
    {
        #region Module Level Services Object
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IDocumentService _documentService = null;
        private readonly IAppLogger<TechnicalSpecialistDetailService> _logger = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly ITechnicalSpecialistCertificationService _tsCertificationService = null;
        private readonly ITechnicalSpecialistTrainingService _tsTrainingService = null;
        private readonly ITechnicalSpecialistCodeAndStandardService _tsCodeAndStandardService = null;
        private readonly ITechnicalSpecialCommodityEquipmentKnowledgeService _tsCommodityEquipmentKnowledgeService = null;
        private readonly ITechnicalSpecialistComputerElectronicKnowledgeService _tsComputerElectronicKnowledgeService = null;
        private readonly ITechnicalSpecialistCustomerApprovalService _tsCustomerApprovalService = null;
        private readonly ITechnicalSpecialistLanguageCapabilityService _tsLanguageCapabilityService = null;
        private readonly ITechnicalSpecialistNoteService _tsNoteService = null;
        private readonly ITechnicalSpecialistPayScheduleService _tsPayScheduleService = null;
        private readonly ITechnicalSpecialistPayRateService _tsPayRateService = null;
        private readonly ITechnicalSpecialistStampInfoService _tsStampInfoService = null;
        private readonly ITechnicalSpecialistTaxonomyService _tsTaxonomyService = null;
        private readonly ITechnicalSpecialistWorkHistoryService _tsWorkHistoryService = null;

        private readonly ITechnicalSpecialistCompetencyService _tsCompetencyService = null;
        private readonly ITechnicalSpecialistInternalTrainingService _tsInternalTrainingService = null;
        private readonly ITechnicalSpecialistContactService _tsContactService = null;
        private readonly ITechnicalSpecialistEducationalQualificationService _tsEducationalQualificationService = null;

        private readonly ICertificationsService _certificationsService = null;
        private readonly IUserService _userService = null;
        private readonly IUserTypeService _userTypeService = null;
        private readonly IUserDetailService _userDetailService = null;
        private readonly ITrainingsService _trainingService = null;
        private readonly INativeCurrencyService _currencyService = null;
        private readonly ICodeStandardService _codeStandardService = null;
        private readonly ICommodityService _commodityService = null;
        private readonly IEquipmentService _equipmentService = null;
        private readonly IComputerKnowledgeService _computerKnowledgeService = null;
        private readonly ITechnicalSpecialistCustomerService _customerService = null;
        private readonly ILanguageService _langService = null;
        private readonly ICountryService _countryService = null;
        private readonly ITaxonomyCategoryService _taxonomyCatService = null;
        private readonly IDraftService _draftService = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly ITechnicalSpecialistStampCountryCodeService _tsStampCountryCodeService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #endregion

        #region Constructor

        public TechnicalSpecialistDetailService(IMapper mapper,
                                                JObject messages,
                                                IDocumentService documentService,
                                                IAppLogger<TechnicalSpecialistDetailService> logger,
                                                ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                ITechnicalSpecialistService technicalSpecialistService,
                                                ITechnicalSpecialistCertificationService tsCertificationService,
                                                ITechnicalSpecialistTrainingService tsTrainingService,
                                                ITechnicalSpecialistCodeAndStandardService tsCodeAndStandardService,
                                                ITechnicalSpecialCommodityEquipmentKnowledgeService tsCommodityEquipmentKnowledgeService,
                                                ITechnicalSpecialistComputerElectronicKnowledgeService tsComputerElectronicKnowledgeService,
                                                ITechnicalSpecialistCustomerApprovalService tsCustomerApprovalService,
                                                ITechnicalSpecialistLanguageCapabilityService tsLanguageCapabilityService,
                                                ITechnicalSpecialistNoteService tsNoteService,
                                                ITechnicalSpecialistPayScheduleService tsPayScheduleService,
                                                ITechnicalSpecialistPayRateService tsPayRateService,
                                                ITechnicalSpecialistStampInfoService tsStampInfoService,
                                                ITechnicalSpecialistTaxonomyService tsTaxonomyService,
                                                ITechnicalSpecialistWorkHistoryService tsWorkHistoryService,
                                                ITechnicalSpecialistContactService tsContactService,
                                                ITechnicalSpecialistInternalTrainingService tsInternalTrainingService,
                                                ITechnicalSpecialistCompetencyService tsCompetencyService,
                                                ITechnicalSpecialistEducationalQualificationService tsEducationalQualificationService,
                                                ICertificationsService certificationsService,
                                                IUserService userService,
                                                IUserDetailService userDetailService,
                                                ITrainingsService trainingService,
                                                INativeCurrencyService currencyService,
                                                ICodeStandardService codeStandardService,
                                                IEquipmentService equipmentService,
                                                ICommodityService commodityService,
                                                IComputerKnowledgeService computerKnowledgeService,
                                                ITechnicalSpecialistCustomerService customerService,
                                                ILanguageService langService,
                                                ICountryService countryService,
                                                ITaxonomyCategoryService taxonomyCatService,
                                                IOptions<AppEnvVariableBaseModel> environment,
                                                IDraftService draftService,
                                                IMyTaskService myTaskService,
                                                IEmailQueueService emailService,
                                                IUserTypeService userTypeService,
                                                ITechnicalSpecialistStampCountryCodeService tsStampCountryCodeService,
                                                IAuditSearchService auditSearchService

            )
        {

            _mapper = mapper;
            _messages = messages;
            _logger = logger;
            _documentService = documentService;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _technicalSpecialistService = technicalSpecialistService;
            _tsCertificationService = tsCertificationService;
            _tsTrainingService = tsTrainingService;
            _tsCodeAndStandardService = tsCodeAndStandardService;
            _tsCommodityEquipmentKnowledgeService = tsCommodityEquipmentKnowledgeService;
            _tsComputerElectronicKnowledgeService = tsComputerElectronicKnowledgeService;
            _tsCustomerApprovalService = tsCustomerApprovalService;
            _tsLanguageCapabilityService = tsLanguageCapabilityService;
            _tsNoteService = tsNoteService;
            _tsPayScheduleService = tsPayScheduleService;
            _tsPayRateService = tsPayRateService;
            _tsStampInfoService = tsStampInfoService;
            _tsStampInfoService = tsStampInfoService;
            _tsTaxonomyService = tsTaxonomyService;
            _tsWorkHistoryService = tsWorkHistoryService;
            _tsContactService = tsContactService;
            _tsInternalTrainingService = tsInternalTrainingService;
            _tsCompetencyService = tsCompetencyService;
            _tsEducationalQualificationService = tsEducationalQualificationService;

            _certificationsService = certificationsService;
            _userService = userService;
            _userDetailService = userDetailService;
            _trainingService = trainingService;
            _currencyService = currencyService;
            _codeStandardService = codeStandardService;
            _equipmentService = equipmentService;
            _commodityService = commodityService;
            _computerKnowledgeService = computerKnowledgeService;
            _customerService = customerService;
            _langService = langService;
            _countryService = countryService;
            _taxonomyCatService = taxonomyCatService;
            _environment = environment.Value;
            _draftService = draftService;
            _myTaskService = myTaskService;
            _emailService = emailService;
            _userTypeService = userTypeService;
            _tsStampCountryCodeService = tsStampCountryCodeService;
            _auditSearchService = auditSearchService;

        }

        #endregion

        #region Public Methods

        public Response Add(TechnicalSpecialistDetail techSpecialistDetail, bool commitChange = true, bool isPayloadValidationRequired = false)
        {
            Exception exception = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = null;
            TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection = null;
            long? eventId = null;
            try
            {
                var isValidModel = ValidateMandatoryFieldData(techSpecialistDetail,isPayloadValidationRequired, ref validationMessages);
                if(isValidModel)
                {
                    tsModuleRefDataCollection = new TechnicalSpecialistModuleRefDataCollection();
                    var result = ValidateLinkTableData(techSpecialistDetail, ref validationMessages, tsModuleRefDataCollection);
                    if (result)
                    {
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            _technicalSpecialistRepository.AutoSave = false;
                            response = ProcessTsLogInCredentials(techSpecialistDetail, tsModuleRefDataCollection); //User Table Insertion

                            if(response.Code == MessageType.Success.ToId())
                                response = IsValidTSLoginCredential(techSpecialistDetail.TechnicalSpecialistInfo, tsModuleRefDataCollection); //Get User Table Values For TS

                            if (response.Code == MessageType.Success.ToId())
                                response = ProcessTechnicalSpecialistInfo(new List<TechnicalSpecialistInfo>
                                                                        {
                                                                            techSpecialistDetail.TechnicalSpecialistInfo
                                                                        },
                                                                                ValidationType.Add, ref eventId, commitChange, tsModuleRefDataCollection);
                            if (response.Code == MessageType.Success.ToId())
                            {
                                AppendEvent(techSpecialistDetail, eventId);

                                var newTs = tsModuleRefDataCollection?.DbTechnicalSpecialists?.FirstOrDefault();

                                SetTechSpecEpin(ref techSpecialistDetail, newTs?.Pin);

                                response = ValidateInputModel(techSpecialistDetail, ValidationType.Add, tsModuleRefDataCollection);

                                if (response.Code == MessageType.Success.ToId())
                                    response = ProcessTechnicalSpecialistDetail(techSpecialistDetail, ValidationType.Add, ref tsModuleRefDataCollection.DbTechnicalSpecialists, tsModuleRefDataCollection, ref eventId, commitChange);

                                if (response.Code == MessageType.Success.ToId())
                                {
                                    _technicalSpecialistRepository.AutoSave = false;
                                    var savedCnt = _technicalSpecialistRepository.ForceSave();
                                    if (savedCnt >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.DraftId))
                                        {
                                            _draftService.DeleteDraft(techSpecialistDetail?.TechnicalSpecialistInfo?.DraftId, DraftType.CreateProfile, true);
                                        }
                                       // ProcessTsLogInCredentials(techSpecialistDetail, tsModuleRefDataCollection); Order Changed
                                        if (!string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.UserCompanyCode) && !string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.ActionByUser))
                                        {
                                            var userTypes = FetchCompanyUserType(techSpecialistDetail?.TechnicalSpecialistInfo?.UserCompanyCode, techSpecialistDetail?.TechnicalSpecialistInfo.ActionByUser);
                                            ProcessTechSpecProfileChangeHistory(techSpecialistDetail, newTs?.Pin, userTypes);
                                            ProcessTsTask(techSpecialistDetail?.TechnicalSpecialistInfo, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault(), userTypes);
                                        }
                                        switch (techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction)
                                        {
                                            case TechnicalSpecialistConstants.Profile_Action_Send_To_TM:
                                                ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailTmProfileValidation, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                break;

                                            case TechnicalSpecialistConstants.Profile_Action_Send_To_TS:
                                                if (techSpecialistDetail.TechnicalSpecialistInfo.IsTsCredSent == null || techSpecialistDetail.TechnicalSpecialistInfo.IsTsCredSent == false)
                                                {
                                                    ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailTsProfileLoginCreate, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                }
                                                break;
                                        }

                                    }
                                    tranScope.Complete();
                                    response = new Response().ToPopulate(ResponseType.Success, null, null, null, _mapper.Map<TechnicalSpecialistInfo>(newTs), null);
                                }
                            }
                            return response;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), techSpecialistDetail);
            }
            finally
            {
                _technicalSpecialistRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }

        public Response Delete(TechnicalSpecialistDetail techSpecialistDetail, bool commitChange = true, bool isPayloadValidationRequired = false)
        {
            Exception exception = null;
            Response response = null;
            TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection = null;
            long? eventId = null;
            try
            {
                tsModuleRefDataCollection = new TechnicalSpecialistModuleRefDataCollection();
                response = ValidateInputModel(techSpecialistDetail, ValidationType.Delete, tsModuleRefDataCollection);
                if (response != null && response?.Code == MessageType.Success.ToId() && Convert.ToBoolean(response?.Result))
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        _technicalSpecialistRepository.AutoSave = false;

                        response = ProcessTechnicalSpecialistInfo(new List<TechnicalSpecialistInfo>
                                                                        {
                                                                            techSpecialistDetail.TechnicalSpecialistInfo
                                                                        },
                                                                            ValidationType.Delete, ref eventId, commitChange, tsModuleRefDataCollection, false);

                        AppendEvent(techSpecialistDetail, eventId);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            if (response.Code == MessageType.Success.ToId())
                                response = ProcessTechnicalSpecialistDetail(techSpecialistDetail, ValidationType.Delete, ref tsModuleRefDataCollection.DbTechnicalSpecialists, tsModuleRefDataCollection, ref eventId,commitChange);

                            if (response.Code == MessageType.Success.ToId())
                            {
                                _technicalSpecialistRepository.ForceSave();
                                tranScope.Complete();
                            }

                        }

                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), techSpecialistDetail);
            }
            finally
            {
                _technicalSpecialistRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response Get(TechnicalSpecialistDetail techSpecialistDetail)
        {
            throw new NotImplementedException();
        }

        public Response Modify(TechnicalSpecialistDetail techSpecialistDetail, bool commitChange = true, bool isPayloadValidationRequired = false)
        {
            Exception exception = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = null;
            TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection = null;
            long? eventId = null;
            try
            {
                var isValidModel = ValidateMandatoryFieldData(techSpecialistDetail, isPayloadValidationRequired, ref validationMessages);
                if (isValidModel)
                {
                    tsModuleRefDataCollection = new TechnicalSpecialistModuleRefDataCollection();

                    var userTypes = FetchCompanyUserType(techSpecialistDetail?.TechnicalSpecialistInfo?.UserCompanyCode, techSpecialistDetail?.TechnicalSpecialistInfo.ActionByUser);

                    CheckDraftProfileChangeHistoryExists(techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedByUser, techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedToUser, techSpecialistDetail?.TechnicalSpecialistInfo?.ActionByUser, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin, techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes);//D661 issue1 myTask CR

                    ProcessRejectedTechSpecProfileChanges(ref techSpecialistDetail, userTypes);

                    var result = ValidateLinkTableData(techSpecialistDetail, ref validationMessages, tsModuleRefDataCollection);
                    if (result)
                    {
                        response = ValidateInputModel(techSpecialistDetail, ValidationType.Update, tsModuleRefDataCollection);

                        if (response != null && response?.Code == MessageType.Success.ToId() && Convert.ToBoolean(response?.Result))
                        {
                            //To-Do: Will create helper method get TransactionScope instance based on requirement
                            using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                            //using (var tranScope = new TransactionScope())
                            {
                                string prevProfileAction = tsModuleRefDataCollection?.DbTechnicalSpecialists?.FirstOrDefault(x => x.Id == techSpecialistDetail.TechnicalSpecialistInfo.Id).ProfileAction?.Name;
                                _technicalSpecialistRepository.AutoSave = false;
                                response = ProcessTechnicalSpecialistInfo(new List<TechnicalSpecialistInfo>
                                                                        {
                                                                            techSpecialistDetail.TechnicalSpecialistInfo
                                                                        },
                                                                                    ValidationType.Update, ref eventId, commitChange, tsModuleRefDataCollection, false);

                                AppendEvent(techSpecialistDetail, eventId);
                                if (response.Code == MessageType.Success.ToId())
                                    response = ProcessTechnicalSpecialistDetail(techSpecialistDetail, ValidationType.Update, ref tsModuleRefDataCollection.DbTechnicalSpecialists, tsModuleRefDataCollection, ref eventId, commitChange);

                                if (response.Code == MessageType.Success.ToId())
                                {
                                    _technicalSpecialistRepository.AutoSave = false;
                                    var savedCnt = _technicalSpecialistRepository.ForceSave();
                                    if (savedCnt >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.DraftId) && !string.IsNullOrEmpty(techSpecialistDetail.TechnicalSpecialistInfo.DraftType))
                                        {
                                            _draftService.DeleteDraft(techSpecialistDetail?.TechnicalSpecialistInfo?.DraftId, techSpecialistDetail.TechnicalSpecialistInfo.DraftType.ToEnum<DraftType>(), true);
                                        }
                                        ProcessTsLogInCredentials(techSpecialistDetail, tsModuleRefDataCollection, userTypes); 
                                        switch (techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction)
                                        {
                                            case TechnicalSpecialistConstants.Profile_Action_Send_To_TM:
                                                ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailTmProfileValidation, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailCustomerApprovalByRC, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                break;

                                            case TechnicalSpecialistConstants.Profile_Action_Send_To_TS:  // def 978 Fix
                                                if (techSpecialistDetail?.TechnicalSpecialistInfo.ApprovalStatus == TechnicalSpecialistConstants.TS_Change_Approval_Status_Reject)
                                                {
                                                    ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailProfileChangeRejected, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                }
                                                if (techSpecialistDetail.TechnicalSpecialistInfo.IsTsCredSent == null || techSpecialistDetail.TechnicalSpecialistInfo.IsTsCredSent == false)
                                                {
                                                    ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailTsProfileLoginCreate, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                }
                                                if (!techSpecialistDetail.TechnicalSpecialistInfo.IsDraft)//def 978 
                                                {
                                                    ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailCustomerApprovalByRC, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                }
                                                break;

                                            case TechnicalSpecialistConstants.Profile_Action_Send_To_RC_RM:
                                                if (string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedByUser, techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailProfileChangeUpdate, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault(), prevProfileAction);
                                                }
                                                ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailCustomerApprovalByResource, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                break;

                                            case TechnicalSpecialistConstants.Profile_Action_Create_Update_Profile:
                                                ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailCustomerApprovalByRC, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault());
                                                break;

                                        }
                                        if (userTypes != null && userTypes.Count > 0)
                                        {
                                            ProcessTechSpecProfileChangeHistory(techSpecialistDetail, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin, userTypes);
                                            ProcessTsTask(techSpecialistDetail?.TechnicalSpecialistInfo, tsModuleRefDataCollection.DbTechnicalSpecialists.FirstOrDefault(), userTypes);
                                        }
                                    }
                                    tranScope.Complete();
                                }
                            }
                        }
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), techSpecialistDetail);
            }
            finally
            {
                _technicalSpecialistRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Private Methods

        private Response ProcessTechnicalSpecialistDetail(TechnicalSpecialistDetail techSpecialistDetail,
                                                         ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechSpecialists,
                                                TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection,
                                                ref long? eventId,
                                                bool commitChange = true)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                tsModuleRefDataCollection = tsModuleRefDataCollection ?? new TechnicalSpecialistModuleRefDataCollection();

                if (techSpecialistDetail != null)
                {

                    if (techSpecialistDetail?.TechnicalSpecialistCertification?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecCertificationInfo(techSpecialistDetail.TechnicalSpecialistCertification, validationType, commitChange, tsModuleRefDataCollection);
                    }
                    if (techSpecialistDetail?.TechnicalSpecialistTraining?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecTrainingInfo(techSpecialistDetail.TechnicalSpecialistTraining, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistCodeAndStandard?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecCodeAndStandardInfo(techSpecialistDetail.TechnicalSpecialistCodeAndStandard, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecCommodityEquipmentKnowledgeInfo(techSpecialistDetail.TechnicalSpecialistCommodityAndEquipment, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistComputerElectronicKnowledge?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecComputerElectronicKnowledgeInfo(techSpecialistDetail.TechnicalSpecialistComputerElectronicKnowledge, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistCustomerApproval?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecCustomerApprovalInfo(techSpecialistDetail.TechnicalSpecialistCustomerApproval, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecLanguageCapabilityInfo(techSpecialistDetail.TechnicalSpecialistLanguageCapabilities, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistStamp?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecStampInfo(techSpecialistDetail.TechnicalSpecialistStamp, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistTaxonomy?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecTaxonomyInfo(techSpecialistDetail.TechnicalSpecialistTaxonomy, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistWorkHistory?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecWorkHistoryInfo(techSpecialistDetail.TechnicalSpecialistWorkHistory, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistCompetancy?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecCompetencyInfo(techSpecialistDetail.TechnicalSpecialistCompetancy, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistInternalTraining?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecInternalTrainingInfo(techSpecialistDetail.TechnicalSpecialistInternalTraining, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistContact?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecContactInfo(techSpecialistDetail.TechnicalSpecialistContact, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistEducation?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecEducationalQualificationInfo(techSpecialistDetail.TechnicalSpecialistEducation, validationType, commitChange, tsModuleRefDataCollection);
                    }
                    /** Deleting Payrates before deleting Payschedules*/
                    if (techSpecialistDetail?.TechnicalSpecialistPayRate?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).Count() > 0
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        IList<TechnicalSpecialistPayRateInfo> tsPayRateInfos = techSpecialistDetail?.TechnicalSpecialistPayRate?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                        response = _tsPayRateService.Delete(tsPayRateInfos, ref tsModuleRefDataCollection.DbTsPayRates, commitChange, false);
                        if (response != null || response?.Code == MessageType.Success.ToId())
                        {
                            techSpecialistDetail?.TechnicalSpecialistPayRate.Select(x=> {
                                x.RecordStatus = x.RecordStatus.IsRecordStatusDeleted() ?  null : x.RecordStatus;
                                return x;
                            }); 
                        }
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistPaySchedule?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecPayScheduleInfo(techSpecialistDetail.TechnicalSpecialistPaySchedule, validationType, commitChange, tsModuleRefDataCollection);

                    }

                    if (techSpecialistDetail?.TechnicalSpecialistPayRate?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecPayRateInfo(techSpecialistDetail.TechnicalSpecialistPayRate, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistDocuments?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecDocument(techSpecialistDetail, validationType, commitChange, validationType.ToAuditActionType(),ref eventId);
                    }
                    if (techSpecialistDetail?.TechnicalSpecialistSensitiveDocuments?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecDocument(techSpecialistDetail, validationType, commitChange, validationType.ToAuditActionType(),ref eventId);
                    }

                    if (techSpecialistDetail?.TechnicalSpecialistNotes?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Count() > 0 && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTechSpecNoteInfo(techSpecialistDetail.TechnicalSpecialistNotes, validationType, commitChange, tsModuleRefDataCollection);
                    }

                    return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), techSpecialistDetail);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }

        private void SetTechSpecEpin(ref TechnicalSpecialistDetail techSpecialistDetail, int? ePin)
        {
            if (ePin > 0)
            {
                techSpecialistDetail.TechnicalSpecialistInfo.Epin = ePin.Value;
                techSpecialistDetail?.TechnicalSpecialistCertification?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistCodeAndStandard?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistCompetancy?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistComputerElectronicKnowledge?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistContact?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistCustomerApproval?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistEducation?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistInternalTraining?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistNotes?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistPayRate?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistPaySchedule?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistStamp?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistTaxonomy?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistTraining?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistWorkHistory?.ToList().ForEach(x => x.Epin = ePin.Value);
                techSpecialistDetail?.TechnicalSpecialistDocuments?.ToList().ForEach(x => x.ModuleRefCode = ePin.Value.ToString());
            }

        }

        #region Process Model Infos

        private Response ProcessTechnicalSpecialistInfo(IList<TechnicalSpecialistInfo> technicalSpecialistInfos,
                                                        ValidationType validationType,
                                                        ref long? eventId,
                                                        bool commitChanges,
                                                        TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            Response valResponse = null;

            try
            {

                if (technicalSpecialistInfos != null)
                {
                    if (isDbValidationRequire)
                    {
                        valResponse = _technicalSpecialistService.IsRecordValidForProcess(technicalSpecialistInfos,
                                                              validationType,
                                                              ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                              ref tsModuleRefDataCollection.DbCompanies,
                                                              ref tsModuleRefDataCollection.DbCompPayrolls,
                                                              ref tsModuleRefDataCollection.DbSubDivisions,
                                                              ref tsModuleRefDataCollection.DbStatuses,
                                                              ref tsModuleRefDataCollection.DbActions,
                                                              ref tsModuleRefDataCollection.DbEmploymentTypes,
                                                              ref tsModuleRefDataCollection.DbCountries);
                    }

                    if (valResponse == null || valResponse.Code == MessageType.Success.ToId())
                    {
                        if (validationType == ValidationType.Delete)
                            return _technicalSpecialistService.Delete(technicalSpecialistInfos, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref eventId, commitChanges, false);
                        else if (validationType == ValidationType.Add)
                        {


                            return _technicalSpecialistService.Add(technicalSpecialistInfos,
                                                                    ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                                    tsModuleRefDataCollection.DbCompanies,
                                                                    tsModuleRefDataCollection.DbCompPayrolls,
                                                                    tsModuleRefDataCollection.DbSubDivisions,
                                                                    tsModuleRefDataCollection.DbStatuses,
                                                                    tsModuleRefDataCollection.DbActions,
                                                                    tsModuleRefDataCollection.DbEmploymentTypes,
                                                                    tsModuleRefDataCollection.DbCountries,
                                                                    tsModuleRefDataCollection.DbUsers,
                                                                    ref eventId,
                                                                    commitChanges, false);

                        }
                        else if (validationType == ValidationType.Update)
                        {
                            return _technicalSpecialistService.Modify(technicalSpecialistInfos,
                                                                        ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                                        tsModuleRefDataCollection.DbCompanies,// This we can pass from here as ref to avoid company fetch in transaction scope
                                                                        tsModuleRefDataCollection.DbCompPayrolls,
                                                                        tsModuleRefDataCollection.DbSubDivisions,
                                                                        tsModuleRefDataCollection.DbStatuses,
                                                                        tsModuleRefDataCollection.DbActions,
                                                                        tsModuleRefDataCollection.DbEmploymentTypes,
                                                                        tsModuleRefDataCollection.DbCountries,
                                                                        tsModuleRefDataCollection.DbUsers,
                                                                        ref eventId,
                                                                        commitChanges, false);
                        }

                    }
                    return valResponse;

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecCertificationInfo(IList<TechnicalSpecialistCertification> tsCertifications,
                                                        ValidationType validationType,
                                                        bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsCertifications != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                        response = _tsCertificationService.Modify(tsCertifications,
                                                                ref tsModuleRefDataCollection.DbTsCertifications,
                                                                ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                                ref tsModuleRefDataCollection.DbCertificationTypes,
                                                                ref tsModuleRefDataCollection.DbVarifiedByUsers,
                                                                commitChanges, false);
                    if (ValidationType.Add != validationType)
                        response = _tsCertificationService.Delete(tsCertifications, ref tsModuleRefDataCollection.DbTsCertifications, commitChanges, false);//D556
                    if (ValidationType.Delete != validationType)
                        response = _tsCertificationService.Add(tsCertifications,
                                ref tsModuleRefDataCollection.DbTsCertifications,
                                ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                ref tsModuleRefDataCollection.DbCertificationTypes,
                                ref tsModuleRefDataCollection.DbVarifiedByUsers,
                                commitChanges, false);



                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecTrainingInfo(IList<TechnicalSpecialistTraining> tsTrainings,
                                                        ValidationType validationType,
                                                        bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsTrainings != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsTrainingService.Modify(tsTrainings,
                                                                ref tsModuleRefDataCollection.DbTsTrainings,
                                                                ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                                ref tsModuleRefDataCollection.DbTrainingTypes,
                                                                ref tsModuleRefDataCollection.DbVarifiedByUsers,
                                                                commitChanges, false);
                    }//D556
                    if (ValidationType.Add != validationType)
                        response = _tsTrainingService.Delete(tsTrainings, ref tsModuleRefDataCollection.DbTsTrainings, commitChanges, false);

                    if (ValidationType.Delete != validationType)
                        response = _tsTrainingService.Add(tsTrainings,
                            ref tsModuleRefDataCollection.DbTsCertifications,
                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                            ref tsModuleRefDataCollection.DbTrainingTypes,
                            ref tsModuleRefDataCollection.DbVarifiedByUsers,
                            commitChanges, false);

                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecCodeAndStandardInfo(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandards,
                                                               ValidationType validationType,
                                                               bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsCodeAndStandards != null)
                {
                    if (ValidationType.Add != validationType)
                        response = _tsCodeAndStandardService.Delete(tsCodeAndStandards, ref tsModuleRefDataCollection.DbTsCodeAndStandardInfos, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsCodeAndStandardService.Add(tsCodeAndStandards,
                                                            ref tsModuleRefDataCollection.DbTsCodeAndStandardInfos,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCodeAndStandards,
                                                            commitChanges, false);
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsCodeAndStandardService.Modify(tsCodeAndStandards,
                                                                ref tsModuleRefDataCollection.DbTsCodeAndStandardInfos,
                                                                ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                                ref tsModuleRefDataCollection.DbCodeAndStandards,
                                                                commitChanges, false);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCodeAndStandards);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecCommodityEquipmentKnowledgeInfo(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsCommodityEquipmentKnowledgeInfos,
                                                                         ValidationType validationType,
                                                                         bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {

                if (tsCommodityEquipmentKnowledgeInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsCommodityEquipmentKnowledgeService.Modify(tsCommodityEquipmentKnowledgeInfos,
                                                            ref tsModuleRefDataCollection.DbTsComdEqipKnowledges,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCommodities,
                                                            ref tsModuleRefDataCollection.DbEquipments,
                                                                commitChanges, false);
                    }//D556
                    if (ValidationType.Add != validationType)
                        response = _tsCommodityEquipmentKnowledgeService.Delete(tsCommodityEquipmentKnowledgeInfos, ref tsModuleRefDataCollection.DbTsComdEqipKnowledges, commitChanges, false);

                    if (ValidationType.Delete != validationType)
                        response = _tsCommodityEquipmentKnowledgeService.Add(tsCommodityEquipmentKnowledgeInfos,
                                                            ref tsModuleRefDataCollection.DbTsComdEqipKnowledges,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCommodities,
                                                            ref tsModuleRefDataCollection.DbEquipments,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCommodityEquipmentKnowledgeInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecComputerElectronicKnowledgeInfo(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                                                         ValidationType validationType,
                                                                         bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {

                if (tsComputerElectronicKnowledgeInfos != null)
                {
                    if (ValidationType.Add != validationType)
                        response = _tsComputerElectronicKnowledgeService.Delete(tsComputerElectronicKnowledgeInfos, ref tsModuleRefDataCollection.DbTsCompElecKnowledges, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsComputerElectronicKnowledgeService.Add(tsComputerElectronicKnowledgeInfos,
                                                            ref tsModuleRefDataCollection.DbTsCompElecKnowledges,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbComputerElectronicsKnowledges,
                                                            commitChanges, false);
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsComputerElectronicKnowledgeService.Modify(tsComputerElectronicKnowledgeInfos,
                                                            ref tsModuleRefDataCollection.DbTsCompElecKnowledges,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbComputerElectronicsKnowledges,
                                                            commitChanges, false);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComputerElectronicKnowledgeInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecCustomerApprovalInfo(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                             ValidationType validationType,
                                             bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsCustomerApprovalInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsCustomerApprovalService.Modify(tsCustomerApprovalInfos,
                                                            ref tsModuleRefDataCollection.DbTsCustomerApprovalInfos,
                                                             ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                              ref tsModuleRefDataCollection.DbTechSpecCustomers,
                                                            commitChanges, false);
                    }//D556
                    if (ValidationType.Add != validationType)
                        response = _tsCustomerApprovalService.Delete(tsCustomerApprovalInfos, ref tsModuleRefDataCollection.DbTsCustomerApprovalInfos, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsCustomerApprovalService.Add(tsCustomerApprovalInfos,
                                                            ref tsModuleRefDataCollection.DbTsCustomerApprovalInfos,
                                                             ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                              ref tsModuleRefDataCollection.DbTechSpecCustomers,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCustomerApprovalInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecLanguageCapabilityInfo(IList<TechnicalSpecialistLanguageCapabilityInfo> tsLanguageCapabilityInfos,
                                            ValidationType validationType,
                                            bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsLanguageCapabilityInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsLanguageCapabilityService.Modify(tsLanguageCapabilityInfos,
                                                            ref tsModuleRefDataCollection.DbTsLanguageCapabilities,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbLanguages,
                                                            commitChanges, false);
                    }
                    if (ValidationType.Add != validationType)//D556
                        response = _tsLanguageCapabilityService.Delete(tsLanguageCapabilityInfos, ref tsModuleRefDataCollection.DbTsLanguageCapabilities, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsLanguageCapabilityService.Add(tsLanguageCapabilityInfos,
                                                            ref tsModuleRefDataCollection.DbTsLanguageCapabilities,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbLanguages,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsLanguageCapabilityInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        private Response ProcessTechSpecPayScheduleInfo(IList<TechnicalSpecialistPayScheduleInfo> tsPayScheduleInfos,
                                          ValidationType validationType,
                                          bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsPayScheduleInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsPayScheduleService.Modify(tsPayScheduleInfos,
                                                            ref tsModuleRefDataCollection.DbTsPaySchedules,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCurrencies,
                                                            commitChanges, false);
                    }
                    if (ValidationType.Add != validationType)
                        response = _tsPayScheduleService.Delete(tsPayScheduleInfos, ref tsModuleRefDataCollection.DbTsPaySchedules, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsPayScheduleService.Add(tsPayScheduleInfos,
                                                            ref tsModuleRefDataCollection.DbTsPaySchedules,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCurrencies,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayScheduleInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        private Response ProcessTechSpecPayRateInfo(IList<TechnicalSpecialistPayRateInfo> tsPayRateInfos,
                                          ValidationType validationType,
                                          bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {


                if (tsPayRateInfos != null)
                {
                    //Order Change for D702(#17 issue)
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsPayRateService.Modify(tsPayRateInfos,
                                                            ref tsModuleRefDataCollection.DbTsPayRates,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbTsPaySchedules,
                                                            ref tsModuleRefDataCollection.DbExpenseTypes,
                                                            commitChanges, false);
                    }
                    //if (ValidationType.Add != validationType) /** PayRates deleting done before deleting PayShedules */
                    //    response = _tsPayRateService.Delete(tsPayRateInfos, ref tsModuleRefDataCollection.DbTsPayRates, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                    {
                        // Populate PaySchedule Id 
                        if (tsPayRateInfos?.Where(x => x.RecordStatus.IsRecordStatusNew())?.Count() > 0 && tsModuleRefDataCollection?.DbTsPaySchedules?.Count > 0)
                        {
                            tsPayRateInfos = tsPayRateInfos.Select(x =>
                            {
                                x.PayScheduleId = tsModuleRefDataCollection?.DbTsPaySchedules?.FirstOrDefault(x1 => x1.PayScheduleName == x.PayScheduleName)?.Id;
                                return x;
                            }).ToList();

                        }

                        response = _tsPayRateService.Add(tsPayRateInfos,
                                                           ref tsModuleRefDataCollection.DbTsPayRates,
                                                           ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                           ref tsModuleRefDataCollection.DbTsPaySchedules,
                                                           ref tsModuleRefDataCollection.DbExpenseTypes,
                                                           commitChanges, false);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPayRateInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecStampInfo(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                                  ValidationType validationType,
                                                  bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsStampInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsStampInfoService.Modify(tsStampInfos,
                                                            ref tsModuleRefDataCollection.DbTsStampInfos,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbTsStampCountries,
                                                            commitChanges, false);
                    }
                    if (ValidationType.Add != validationType)
                        response = _tsStampInfoService.Delete(tsStampInfos, ref tsModuleRefDataCollection.DbTsStampInfos, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsStampInfoService.Add(tsStampInfos,
                                                            ref tsModuleRefDataCollection.DbTsStampInfos,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbTsStampCountries,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsStampInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        private Response ProcessTechSpecTaxonomyInfo(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomyInfos,
                                                      ValidationType validationType,
                                                      bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsTaxonomyInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsTaxonomyService.Modify(tsTaxonomyInfos,
                                                    ref tsModuleRefDataCollection.DbTsTaxonomies,
                                                    ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                    ref tsModuleRefDataCollection.DbCategories,
                                                    ref tsModuleRefDataCollection.DbSubCategories,
                                                    ref tsModuleRefDataCollection.DbTaxonomyService,
                                                    commitChanges, false);
                    }
                    if (ValidationType.Add != validationType)
                        response = _tsTaxonomyService.Delete(tsTaxonomyInfos, ref tsModuleRefDataCollection.DbTsTaxonomies, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsTaxonomyService.Add(tsTaxonomyInfos,
                                                    ref tsModuleRefDataCollection.DbTsTaxonomies,
                                                    ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                    ref tsModuleRefDataCollection.DbCategories,
                                                    ref tsModuleRefDataCollection.DbSubCategories,
                                                    ref tsModuleRefDataCollection.DbTaxonomyService,
                                                    commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTaxonomyInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecWorkHistoryInfo(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                                                          ValidationType validationType,
                                                          bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {

                if (tsWorkHistoryInfos != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsWorkHistoryService.Modify(tsWorkHistoryInfos,
                                                            ref tsModuleRefDataCollection.DbTsWorkHistoryInfos,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    }//D556
                    if (ValidationType.Add != validationType)
                        response = _tsWorkHistoryService.Delete(tsWorkHistoryInfos, ref tsModuleRefDataCollection.DbTsWorkHistoryInfos, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsWorkHistoryService.Add(tsWorkHistoryInfos,
                                                            ref tsModuleRefDataCollection.DbTsWorkHistoryInfos,
                                                              ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsWorkHistoryInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecNoteInfo(IList<TechnicalSpecialistNoteInfo> tsNoteInfos,
                                                ValidationType validationType,
                                                bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());       //D661 issue 8 
            try
            {
                if (tsNoteInfos != null)
                {
                    var addNotes = tsNoteInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();        //D661 issue 8 Start
                    var updateNotes = tsNoteInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    if (addNotes?.Count > 0)
                        response = _tsNoteService.Add(tsNoteInfos,
                                                            ref tsModuleRefDataCollection.DbTsNotes,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    if (updateNotes?.Count > 0 && response.Code == MessageType.Success.ToId())        //D661 issue 8 Start
                        response = _tsNoteService.Update(tsNoteInfos,
                                                           ref tsModuleRefDataCollection.DbTsNotes,
                                                           ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                           commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsNoteInfos);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecCompetencyInfo(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                                  ValidationType validationType,
                                                  bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsCompetencies != null)
                {
                    if (ValidationType.Add != validationType)
                        response = _tsCompetencyService.Delete(tsCompetencies, ref tsModuleRefDataCollection.DbTsCompetencies, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsCompetencyService.Add(tsCompetencies,
                                                            ref tsModuleRefDataCollection.DbTsCompetencies,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsCompetencyService.Modify(tsCompetencies,
                                                            ref tsModuleRefDataCollection.DbTsCompetencies,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecInternalTrainingInfo(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                          ValidationType validationType,
                                          bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsInternalTrainings != null)
                {
                    if (ValidationType.Add != validationType)
                        response = _tsInternalTrainingService.Delete(tsInternalTrainings, ref tsModuleRefDataCollection.DbTsInternalTrainings, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsInternalTrainingService.Add(tsInternalTrainings,
                                                            ref tsModuleRefDataCollection.DbTsInternalTrainings,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsInternalTrainingService.Modify(tsInternalTrainings,
                                                            ref tsModuleRefDataCollection.DbTsInternalTrainings,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            commitChanges, false);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecContactInfo(IList<TechnicalSpecialistContactInfo> tsContacts,
                                     ValidationType validationType,
                                     bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsContacts != null)
                {
                    if (ValidationType.Add != validationType)
                        response = _tsContactService.Delete(tsContacts, ref tsModuleRefDataCollection.DbTsContacts, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsContactService.Add(tsContacts,
                                                    ref tsModuleRefDataCollection.DbTsContacts,
                                                    ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                    ref tsModuleRefDataCollection.DbCountries,
                                                    ref tsModuleRefDataCollection.DbCounties,
                                                    ref tsModuleRefDataCollection.DbCities,
                                                    commitChanges, false);
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsContactService.Modify(tsContacts,
                                                    ref tsModuleRefDataCollection.DbTsContacts,
                                                    ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                    ref tsModuleRefDataCollection.DbCountries,
                                                    ref tsModuleRefDataCollection.DbCounties,
                                                    ref tsModuleRefDataCollection.DbCities,
                                                    commitChanges, false);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContacts);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTechSpecEducationalQualificationInfo(IList<TechnicalSpecialistEducationalQualificationInfo> tsEducationalQualifications,
                                  ValidationType validationType,
                                  bool commitChanges, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (tsEducationalQualifications != null)
                {
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _tsEducationalQualificationService.Modify(tsEducationalQualifications,
                                                            ref tsModuleRefDataCollection.DbTsEducationQulifications,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCountries,
                                                            commitChanges, false);
                    } //D556
                    if (ValidationType.Add != validationType)
                        response = _tsEducationalQualificationService.Delete(tsEducationalQualifications, ref tsModuleRefDataCollection.DbTsEducationQulifications, commitChanges, false);
                    if (ValidationType.Delete != validationType)
                        response = _tsEducationalQualificationService.Add(tsEducationalQualifications,
                                                            ref tsModuleRefDataCollection.DbTsEducationQulifications,
                                                            ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                            ref tsModuleRefDataCollection.DbCountries,
                                                            commitChanges, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsEducationalQualifications);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        private Response ProcessTechSpecDocument(TechnicalSpecialistDetail tsDetail, ValidationType validationType, bool commitChanges,
                                                 SqlAuditActionType sqlAuditActionType,
                                                 ref long? eventId)
        {
            Response response = null;
            Exception exception = null;
            IList<ModuleDocument> tsDocuments = new List<ModuleDocument>();
            List<DbModel.Document> dbDocuments = null;
            try
            {
                if (tsDetail.TechnicalSpecialistSensitiveDocuments != null)
                {
                    tsDetail.TechnicalSpecialistSensitiveDocuments = tsDetail.TechnicalSpecialistSensitiveDocuments?.Select(x => { x.ModuleRefCode = tsDetail?.TechnicalSpecialistInfo?.Epin.ToString(); x.SubModuleRefCode = "0"; return x; }).ToList();    //Related issue for ITK D-512 Fixes 
                    tsDocuments.AddRange(tsDetail.TechnicalSpecialistSensitiveDocuments);
                }
                if (tsDetail.TechnicalSpecialistDocuments != null)
                {
                    tsDetail.TechnicalSpecialistDocuments = tsDetail.TechnicalSpecialistDocuments?.Select(x => { x.ModuleRefCode = tsDetail?.TechnicalSpecialistInfo?.Epin.ToString(); x.SubModuleRefCode = "-1"; return x; }).ToList();  //Related issue for ITK D-512 Fixes 
                    tsDocuments.AddRange(tsDetail.TechnicalSpecialistDocuments);
                }
                if (tsDocuments.Any())
                {
                    var auditVisitDetails = ObjectExtension.Clone(tsDetail);
                    var auditDocuments = ObjectExtension.Clone(tsDocuments);

                    if (ValidationType.Add != validationType)
                    {
                        response = _documentService.Delete(tsDocuments, commitChanges);
                    }
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                    {
                        response = _documentService.Modify(tsDocuments, ref dbDocuments, commitChanges);
                    }
                    if (ValidationType.Delete != validationType || ValidationType.Update != validationType) //def 855 doc audit changes
                    {
                        response = _documentService.Save(tsDocuments, ref dbDocuments, commitChanges);
                        auditDocuments = tsDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    }
                    if (response.Code == MessageType.Success.ToId())
                    {
                        DocumentAudit(auditDocuments, sqlAuditActionType, auditVisitDetails, ref eventId, ref dbDocuments);
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsDetail.TechnicalSpecialistDocuments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void DocumentAudit(IList<ModuleDocument> tstDocuments, SqlAuditActionType sqlAuditActionType, TechnicalSpecialistDetail tsDetail, ref long? eventId, ref List<DbModel.Document> dbDocuments)
        {
            //For Document Audit
            if (tstDocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = tstDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = tstDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = tstDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument;
                    _auditSearchService.AuditLog(tsDetail, ref eventId, tsDetail?.TechnicalSpecialistInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.TechnicalSpecialistDocument, null, newData);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(tsDetail, ref eventId, tsDetail?.TechnicalSpecialistInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.TechnicalSpecialistDocument, oldData, newData);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(tsDetail, ref eventId, tsDetail?.TechnicalSpecialistInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.TechnicalSpecialistDocument, oldData, null);
                }
            }
        }


        private Response ProcessTechSpecProfileChangeHistory(TechnicalSpecialistDetail techSpecialistDetail, int? ePin, IList<string> userTypes)
        {
            Exception exception = null;
            try
            {
                if (techSpecialistDetail != null && (userTypes != null && userTypes.Any()) && !string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo.ApprovalStatus, TechnicalSpecialistConstants.TS_Change_Approval_Status_Reject))
                {
                    if ((string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Create_Update_Profile) || string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TS) || string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TM))
                        && (userTypes.Contains(TechnicalSpecialistConstants.User_Type_RM) || userTypes.Contains(TechnicalSpecialistConstants.User_Type_RC)))
                    {
                        var prevChangeResp = _draftService.GetDraft(new Draft.Domain.Models.Draft { DraftId = ePin.ToString(), Description = DraftType.ProfileChangeHistory.ToString() });

                        if (prevChangeResp != null && prevChangeResp.Code == ResponseType.Success.ToId() && prevChangeResp.Result != null)
                        {
                            _draftService.DeleteDraft(prevChangeResp.Result.Populate<IList<Draft.Domain.Models.Draft>>()?.Select(x => x.DraftId).ToList());
                        }
                    }
                    if (userTypes.Contains(TechnicalSpecialistConstants.User_Type_RM) || userTypes.Contains(TechnicalSpecialistConstants.User_Type_RC) || userTypes.Contains(TechnicalSpecialistConstants.User_Type_TM))
                    {
                        return SaveDraftProfileChangeHistory(techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedByUser, techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedToUser, techSpecialistDetail?.TechnicalSpecialistInfo?.ActionByUser, ePin, techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode);//D661 issue1 myTask CR
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "TS_ChangeHistory-", techSpecialistDetail);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }

        private Response SaveDraftProfileChangeHistory(string assignedByUser, string assignedToUser, string actionByUser, int? ePin, string companyCode) //D661 issue1 myTask CR
        {
            return _draftService.SaveDraft(new Draft.Domain.Models.Draft
            {
                Moduletype = ModuleCodeType.TS.ToString(),
                DraftId = ePin.ToString(),
                SerilizableObject = GetTsDetails(ePin.Value).Serialize(SerializationType.JSON),
                SerilizationType = SerializationType.JSON.ToString(),
                AssignedBy = assignedByUser,
                AssignedTo = assignedToUser,
                CreatedBy = actionByUser,// Login user name
                DraftType = DraftType.ProfileChangeHistory.ToString(),
                CompanyCode = companyCode//D661 issue1 myTask CR
            });
        }

        private void CheckDraftProfileChangeHistoryExists(string assignedByUser, string assignedToUser, string actionByUser, int? ePin, string companyCode, IList<string> userTypes)//D661 issue1 myTask CR
        {
            if (userTypes != null && userTypes.Contains(TechnicalSpecialistConstants.User_Type_TS))
            {
                var prevChangeResp = _draftService.GetDraft(new Draft.Domain.Models.Draft { DraftId = ePin.ToString(), Description = DraftType.ProfileChangeHistory.ToString() });

                if (prevChangeResp != null && prevChangeResp.Code == ResponseType.Success.ToId() && prevChangeResp.RecordCount == 0)
                {
                    SaveDraftProfileChangeHistory(assignedByUser, assignedToUser, actionByUser, ePin, companyCode); //D661 issue1 myTask CR
                }
            }
        }

        private Response ProcessTsTask(TechnicalSpecialistInfo tsInfo, DbModel.TechnicalSpecialist dbTsInfo, IList<string> userTypes)
        {
            Exception exception = null;
            IList<Evolution.Home.Domain.Models.Homes.MyTask> myTaskToBeCreated = null;
            IList<Evolution.Home.Domain.Models.Homes.MyTask> myTaskToBeDeleted = null;
            try
            {
                if (tsInfo != null && dbTsInfo != null)
                {
                    myTaskToBeCreated = new List<Evolution.Home.Domain.Models.Homes.MyTask>();
                    myTaskToBeDeleted = new List<Evolution.Home.Domain.Models.Homes.MyTask>();
                    if (!string.IsNullOrEmpty(tsInfo?.ProfileAction) && userTypes != null && userTypes.Any())
                    {
                        var taskType = string.Empty;
                        var taskDescription = string.Empty;
                        switch (tsInfo.ProfileAction)
                        {
                            case TechnicalSpecialistConstants.Profile_Action_Send_To_TM:
                                taskType = TechnicalSpecialistConstants.Task_Type_Taxonomy_Approval_Request;
                                taskDescription = string.Format(TechnicalSpecialistConstants.Task_Description_TM_Verify_And_Validate, tsInfo.LastName, tsInfo.FirstName, tsInfo.Epin.ToString());
                                break;

                            case TechnicalSpecialistConstants.Profile_Action_Send_To_RC_RM:
                                if ((bool)userTypes?.Contains(TechnicalSpecialistConstants.User_Type_TS))
                                {
                                    taskType = TechnicalSpecialistConstants.Task_Type_TS_Updated_Profile;
                                    taskDescription = string.Format(TechnicalSpecialistConstants.Task_Description_TS_Updated_Profile, tsInfo.LastName, tsInfo.FirstName, tsInfo.Epin.ToString());

                                }
                                else if ((bool)userTypes?.Contains(TechnicalSpecialistConstants.User_Type_TM))
                                {
                                    taskType = TechnicalSpecialistConstants.Task_Type_Taxonomy_Updated;
                                    taskDescription = string.Format(TechnicalSpecialistConstants.Task_Description_TM_Validated, tsInfo.LastName, tsInfo.FirstName, tsInfo.Epin.ToString());
                                }
                                break;
                            case TechnicalSpecialistConstants.Profile_Action_Send_To_TS://Added for D761 CR
                                taskType = TechnicalSpecialistConstants.Task_Type_Resource_To_Update_Profile;
                                taskDescription = string.Format(TechnicalSpecialistConstants.Task_Description_Resource_To_Update_Profile, tsInfo.LastName, tsInfo.FirstName, tsInfo.Epin.ToString());
                                break;
                        }

                        if (tsInfo?.ProfileAction == TechnicalSpecialistConstants.Profile_Action_Send_To_TM ||
                            tsInfo?.ProfileAction == TechnicalSpecialistConstants.Profile_Action_Send_To_RC_RM)
                        {
                            myTaskToBeCreated.Add(new Evolution.Home.Domain.Models.Homes.MyTask()
                            {
                                Moduletype = ModuleCodeType.TS.ToString(),
                                TaskType = taskType,
                                Description = taskDescription,
                                TaskRefCode = dbTsInfo?.Pin.ToString(),
                                AssignedBy = tsInfo?.AssignedByUser,
                                AssignedTo = tsInfo?.AssignedToUser,
                                CreatedOn = DateTime.UtcNow,
                                RecordStatus = RecordStatus.New.FirstChar(),
                                CompanyCode = tsInfo?.CompanyCode, //D661 issue1 myTask CR
                            });
                        }

                        //Added for D761 CR - Starts
                        if (tsInfo?.ProfileAction == TechnicalSpecialistConstants.Profile_Action_Send_To_TS)
                        {
                            myTaskToBeCreated.Add(new Evolution.Home.Domain.Models.Homes.MyTask()
                            {
                                Moduletype = ModuleCodeType.TS.ToString(),
                                TaskType = taskType,
                                Description = taskDescription,
                                TaskRefCode = dbTsInfo?.Pin.ToString(),
                                AssignedBy = tsInfo?.AssignedByUser,
                                AssignedTo = tsInfo?.AssignedByUser,//Assigning Task from RC to RC
                                CreatedOn = DateTime.UtcNow,
                                RecordStatus = RecordStatus.New.FirstChar(),
                                CompanyCode = tsInfo?.CompanyCode,
                            });
                        }
                        //Added for D761 CR - Ends

                        if (tsInfo?.MyTaskId > 0)
                        {
                            myTaskToBeDeleted.Add(new Evolution.Home.Domain.Models.Homes.MyTask()
                            {
                                MyTaskId = tsInfo?.MyTaskId,
                                RecordStatus = RecordStatus.Delete.FirstChar()
                            });
                        }
                        else
                        {
                            myTaskToBeDeleted = _myTaskService.Get(new Home.Domain.Models.Homes.MyTask { TaskRefCode = dbTsInfo?.Pin.ToString(), Moduletype = ModuleCodeType.TS.ToString() })
                                                                .Result
                                                                .Populate<IList<Evolution.Home.Domain.Models.Homes.MyTask>>()
                                                                ?.Select(x => { x.RecordStatus = RecordStatus.Delete.FirstChar(); return x; })
                                                                .ToList();
                        }
                    }

                    if (myTaskToBeDeleted.Any())
                    {
                        _myTaskService.Delete(myTaskToBeDeleted);
                    }

                    if (myTaskToBeCreated.Any())
                    {
                        _myTaskService.Add(myTaskToBeCreated, true);
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInfo);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }

        private Response ProcessTsLogInCredentials(TechnicalSpecialistDetail techSpecialistDetail, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection,IList<string> userTypes=null)
        {
            Exception exception = null;
            Response response = null;
            long? eventId = null; // Manage Security Audit changes
            try
            {
                if (techSpecialistDetail != null && techSpecialistDetail.TechnicalSpecialistInfo != null)
                {
                    if (techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() && tsModuleRefDataCollection.DbUsers == null)
                    {
                        var userInfo = new UserInfo
                        {
                            LogonName = techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName,
                            ApplicationName = _environment.SecurityAppName,
                            UserName = string.Format("{0} {1}", techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName).Trim(),
                            Email = techSpecialistDetail?.TechnicalSpecialistContact?.FirstOrDefault(x => x.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress,
                            Password = techSpecialistDetail?.TechnicalSpecialistInfo?.Password,
                            CompanyCode = techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode,
                            SecurityQuestion1 = techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityQuestion,
                            SecurityQuestion1Answer = techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityAnswer,
                            AuthenticationMode = LogonMode.UP.ToString(),
                            IsActive = true,
                            RecordStatus = RecordStatus.New.FirstChar(),
                            IsPasswordNeedToBeChange = true
                        };

                        var userTypeInfo = new CompanyUserType
                        {
                            CompanyCode = techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode,
                            UserTypes = new List<UserTypeInfo>
                            {
                                new UserTypeInfo {
                                  UserType=UserType.TechnicalSpecialist.ToString(),
                                  RecordStatus = RecordStatus.New.FirstChar(),
                                  UserLogonName=techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName,
                                  CompanyCode = techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode,
                                  IsActive=true,
                                }
                             },
                        };

                        response = _userDetailService.Add(new List<UserDetail>()
                        {
                            new UserDetail()
                            {
                                User = userInfo,
                                CompanyUserTypes = new List<CompanyUserType> { userTypeInfo }
                            }
                        });
                    }
                    else if (techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified())
                    {
                        bool isSendTsEmail = false;
                        var dbTSInfo = tsModuleRefDataCollection?.DbTechnicalSpecialists?.FirstOrDefault(x => x.Pin == techSpecialistDetail?.TechnicalSpecialistInfo.Epin);
                        //var dbUserInfo = tsModuleRefDataCollection?.DbUsers?.FirstOrDefault(x => x.SamaccountName == dbTSInfo.LogInName || x.Email == dbTSInfo?.TechnicalSpecialistContact?.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress);//def 978 ,1256 
                        var dbUserInfo = tsModuleRefDataCollection?.DbUsers?.FirstOrDefault(x => x.Id == dbTSInfo.Userid); //User Id Check for D1256 Req
                        if (dbUserInfo != null)
                        {
                            var userInfo = _mapper.Map<UserInfo>(dbUserInfo);

                            if (!string.Equals(userInfo.LogonName, techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName))
                            {
                                userInfo.LogonName = techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName;
                                isSendTsEmail = true;
                            }
                            if (!string.Equals(userInfo.Password, techSpecialistDetail?.TechnicalSpecialistInfo?.Password))
                            {
                                userInfo.Password = techSpecialistDetail?.TechnicalSpecialistInfo?.Password;
                                isSendTsEmail = true;
                            }
                            if (!string.Equals(userInfo.UserName, string.Format("{0} {1}", techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName).Trim()))
                            {
                                userInfo.UserName = string.Format("{0} {1}", techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName).Trim();
                            }
                            if (techSpecialistDetail?.TechnicalSpecialistContact?.Count > 0 && techSpecialistDetail.TechnicalSpecialistContact.Any(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString()) && !string.Equals(userInfo.Email, techSpecialistDetail?.TechnicalSpecialistContact?.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress))//sanity def 147
                            {
                                userInfo.Email = techSpecialistDetail?.TechnicalSpecialistContact?.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress;
                            }
                            if (userInfo.IsActive != techSpecialistDetail?.TechnicalSpecialistInfo?.IsEnableLogin)
                            {
                                userInfo.IsActive = (bool)techSpecialistDetail?.TechnicalSpecialistInfo.IsEnableLogin;
                            }
                            if (userInfo.IsAccountLocked != techSpecialistDetail?.TechnicalSpecialistInfo?.IsLockOut)
                            {
                                userInfo.IsAccountLocked = (bool)techSpecialistDetail?.TechnicalSpecialistInfo.IsLockOut;
                                if (userInfo.IsAccountLocked)//cyber security issue fix 
                                {
                                    userInfo.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100);
                                }
                                else
                                    userInfo.LockoutEndDateUtc = null;

                            }
                            if (!string.Equals(userInfo.SecurityQuestion1, techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityQuestion))
                            {
                                userInfo.SecurityQuestion1 = techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityQuestion;
                            }
                            if (!string.Equals(userInfo.SecurityQuestion1Answer, techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityAnswer))
                            {
                                userInfo.SecurityQuestion1Answer = techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityAnswer;
                            }
                            userInfo.RecordStatus = RecordStatus.Modify.FirstChar();
                            _userService.Modify(new List<UserInfo> { userInfo }, ref eventId); // Manage Security Audit changes

                            if (isSendTsEmail && dbTSInfo.IsTsCredSent == true && (userTypes !=null && !userTypes.Any(x=> string.Equals(x ,TechnicalSpecialistConstants.User_Type_TS))))//def 1278 &1279
                            {
                                ProcessEmailNotifications(techSpecialistDetail, EmailTemplate.EmailTsProfileLoginInfoUpdated, dbTSInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), techSpecialistDetail.TechnicalSpecialistInfo);
            }

            return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }

        private Response ProcessEmailNotifications(TechnicalSpecialistDetail techSpecialistDetail, EmailTemplate emailTemplateType, DbModel.TechnicalSpecialist dbTechSpecialists,string prevProfileAction=null)
        {
            string emailSubject = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = null;
            List<EmailAddress> fromAddresses = null;
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            IList<UserInfo> userInfos = null;
            List<string> userTypes = null;
            string tsEmailAddress = string.Empty;
            try
            {
                if (techSpecialistDetail != null && techSpecialistDetail?.TechnicalSpecialistInfo != null)
                {
                    if (techSpecialistDetail?.TechnicalSpecialistContact!=null && techSpecialistDetail.TechnicalSpecialistContact.Count >=0 )
                    {
                        tsEmailAddress = dbTechSpecialists?.TechnicalSpecialistContact?.FirstOrDefault(x => x.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress;
                    }

                    userInfos = _userService.Get(_environment.SecurityAppName, new List<string> {
                                                                techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedToUser,
                                                                techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedByUser
                                                            }).Result.Populate<IList<UserInfo>>();

                    switch (emailTemplateType)
                    {
                        case EmailTemplate.EmailTmProfileValidation: 

                            if (userInfos != null && userInfos.Any())
                            {
                                fromAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.UserName } };
                                emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_TmProfileValidation_Subject, techSpecialistDetail?.TechnicalSpecialistInfo.Epin);
                                toAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedToUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedToUser)?.UserName } };
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString()),
                                };
                                emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.PTMR, ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses,fromAddresses: fromAddresses);
                            }
                            break;

                        case EmailTemplate.EmailTsProfileLoginCreate:
                            fromAddresses= new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.UserName } };
                            toAddresses = new List<EmailAddress> { new EmailAddress { Address = tsEmailAddress } };
                            emailSubject = TechnicalSpecialistConstants.Email_Notification_TsProfileLogin_Create_Subject;
                            emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Application_URL, _environment?.ResourceExtranetURL),//DEf 1388 #2 URL fix
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_User_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Password,System.Web.HttpUtility.HtmlEncode(techSpecialistDetail?.TechnicalSpecialistInfo?.Password)),
                                };
                            emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.PLDC, ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses,fromAddresses: fromAddresses);
                            break;
                            
                        case EmailTemplate.EmailTsProfileLoginInfoUpdated:
                            if (!string.Equals(userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.Email,tsEmailAddress))
                            {
                                fromAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.UserName } };
                            } 
                            toAddresses = new List<EmailAddress> { new EmailAddress { Address = tsEmailAddress } };
                            emailSubject = TechnicalSpecialistConstants.Email_Notification_TsProfileLogin_Info_Update_Subject;
                            emailContentPlaceholders = new List<KeyValuePair<string, string>> { 
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_User_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Password, System.Web.HttpUtility.HtmlEncode(techSpecialistDetail?.TechnicalSpecialistInfo?.Password)),
                                };
                            emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.PLDU, ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);
                            break;

                        case EmailTemplate.EmailProfileChangeUpdate:
                            EmailType emailType = EmailType.Notification;
                            if (!string.Equals(prevProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TS))
                            {
                                emailType = EmailType.UPIR;
                                userTypes = new List<string> { TechnicalSpecialistConstants.User_Type_RC };

                                var userUserTypeData = _userService.GetUsersByTypeAndCompany(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes);
                                var toUsers = userUserTypeData?.Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();

                                // RCRMuserInfos = _userService.GetByUserType(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes, true).Result.Populate<IList<UserInfo>>();
                                toAddresses = toUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                            }
                            else
                            {
                                //def 1409 fix
                                if (techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileStatus == ResourceProfileStatus.Active.DisplayName())
                                {
                                    emailType = EmailType.UPIA;
                                }
                                else {
                                    emailType = EmailType.UPIN;
                                }

                                userInfos = _userService.Get(_environment.SecurityAppName, new List<string> {
                                                                techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedToUser
                                                            }).Result.Populate<IList<UserInfo>>();
                                if (userInfos != null && userInfos.Any())
                                {
                                    toAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedToUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedToUser)?.UserName } };
                                }
                            } 
                            emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_ProfileChangeUpdate_Subject, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString());
                            emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString()),
                                };
                            emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, emailType , ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, IsMailSendAsGroup: true);//def1163 fix
                            break;

                        case EmailTemplate.EmailProfileChangeRejected:
                            fromAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedByUser)?.UserName } };
                            toAddresses = new List<EmailAddress> { new EmailAddress { Address = techSpecialistDetail?.TechnicalSpecialistContact.FirstOrDefault(x => x.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress } };
                            emailSubject = TechnicalSpecialistConstants.Email_Notification_TsProfileChangeRejected_Subject;
                            emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.PURR, ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses,fromAddresses: fromAddresses);
                            break;

                        case EmailTemplate.EmailCustomerApprovalByResource:
                            if (techSpecialistDetail?.TechnicalSpecialistCustomerApproval != null && techSpecialistDetail.TechnicalSpecialistCustomerApproval.Any(x => (x.RecordStatus == RecordStatus.New.FirstChar() || x.RecordStatus == RecordStatus.Modify.FirstChar())))// && x.Documents != null && x.Documents.Any(x1=>x1.RecordStatus == RecordStatus.New.FirstChar() || x1.RecordStatus == RecordStatus.Modify.FirstChar()))) //def 1163 fix
                            {
                                userTypes = new List<string> { TechnicalSpecialistConstants.User_Type_RC };

                                var userUserTypeData = _userService.GetUsersByTypeAndCompany(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes);
                                var toUsers = userUserTypeData?.Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();

                                // RCRMuserInfos = _userService.GetByUserType(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes, true).Result.Populate<IList<UserInfo>>();
                                toAddresses = toUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                                emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_CustomerApprovalAdd_Subject, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString());
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString()),
                                };
                                emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.NCAR, ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, IsMailSendAsGroup: true);
                            } 
                            break;

                        case EmailTemplate.EmailCustomerApprovalByRC:
                            bool IsCustApprovalIsApprovedByRC = false; // def 978 SLNO : 19 
                            if (techSpecialistDetail?.TechnicalSpecialistInfo.ApprovalStatus == TechnicalSpecialistConstants.TS_Change_Approval_Status_Approved)
                            {
                                IsCustApprovalIsApprovedByRC = IsCustomerApprovalIsApprovedByRC(dbTechSpecialists.Pin, dbTechSpecialists?.TechnicalSpecialistCustomerApproval.ToList());
                            } 
                            if (techSpecialistDetail?.TechnicalSpecialistCustomerApproval != null && (techSpecialistDetail.TechnicalSpecialistCustomerApproval.Any(x => (x.RecordStatus == RecordStatus.New.FirstChar() || x.RecordStatus == RecordStatus.Modify.FirstChar())) || IsCustApprovalIsApprovedByRC))// && x.Documents != null && x.Documents.Any(x1 => x1.RecordStatus == RecordStatus.New.FirstChar() || x1.RecordStatus == RecordStatus.Modify.FirstChar()))) //def 1163 fix
                            {
                                userTypes = new List<string> { TechnicalSpecialistConstants.User_Type_OC };

                                var userUserTypeData = _userService.GetUsersByTypeAndCompany(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes);
                                var toUsers = userUserTypeData?.Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();

                                // var opCorUserInfos = _userService.GetByUserType(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode, userTypes, true).Result.Populate<IList<UserInfo>>();
                                userInfos = _userService.Get(_environment.SecurityAppName, new List<string> {
                                                                techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedByUser
                                                            }).Result.Populate<IList<UserInfo>>(); 
                                if (userInfos != null && userInfos.Any())
                                {
                                    fromAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedToUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x.LogonName == techSpecialistDetail?.TechnicalSpecialistInfo.AssignedToUser)?.UserName } };
                                }
                                toAddresses = toUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                                emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_CustomerApprovalAdd_Subject, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString());
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, techSpecialistDetail?.TechnicalSpecialistInfo?.LastName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString()),
                                };
                                emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.NCA, ModuleCodeType.TS, techSpecialistDetail?.TechnicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses, IsMailSendAsGroup: true);
                            }
                            break;

                    }

                    return _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private bool IsCustomerApprovalIsApprovedByRC(int epin, List<DbModel.TechnicalSpecialistCustomerApproval> TsCustApprovals)
        {
            var prevChangeResp = _draftService.GetDraft(new Draft.Domain.Models.Draft { DraftId = epin.ToString(), Description = DraftType.ProfileChangeHistory.ToString() });
            if (prevChangeResp != null && prevChangeResp.Code == ResponseType.Success.ToId() && prevChangeResp.Result != null)
            {
                var previousChanges = prevChangeResp.Result.Populate<IList<Draft.Domain.Models.Draft>>()?.FirstOrDefault();
                if (previousChanges != null)
                {
                    TechnicalSpecialistDetail techSpecDetailHistory = previousChanges.SerilizableObject.DeSerialize<TechnicalSpecialistDetail>(SerializationType.JSON);
                    if (techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Count >= TsCustApprovals?.Count)
                    {
                        return TsCustApprovals?.Select(x => x.Id)?.Distinct()?.Except(techSpecDetailHistory.TechnicalSpecialistCustomerApproval.Select(x1 => x1.Id)?.Distinct()).ToList()?.Count > 0;
                    }
                    else if (techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Count < TsCustApprovals?.Count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ProcessRejectedTechSpecProfileChanges(ref TechnicalSpecialistDetail techSpecialistDetail, IList<string> userTypes)
        {
            string actionByUser = techSpecialistDetail?.TechnicalSpecialistInfo?.ActionByUser;
            string assignedToUser = techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedToUser;
            string assignedByUser = techSpecialistDetail?.TechnicalSpecialistInfo?.AssignedByUser;
            int epin = (int)techSpecialistDetail?.TechnicalSpecialistInfo?.Epin;
            if (techSpecialistDetail?.TechnicalSpecialistInfo != null && !string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo.ApprovalStatus) && techSpecialistDetail?.TechnicalSpecialistInfo?.ApprovalStatus == TechnicalSpecialistConstants.TS_Change_Approval_Status_Reject &&
                (userTypes.Contains(TechnicalSpecialistConstants.User_Type_RM) || userTypes.Contains(TechnicalSpecialistConstants.User_Type_RC)))
            {
                TechnicalSpecialistDetail dBTechSpecDetail = GetTsDetails(epin);// Ts update change reject issue fix
                if (dBTechSpecDetail != null)
                {
                    TechnicalSpecialistInfo techInfo = techSpecialistDetail?.TechnicalSpecialistInfo;
                    IList<ModuleDocument> proffAfiliationDocs= dBTechSpecDetail?.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments?.ToList();//Sanity defect 148 fix
                    PopulateDatabaseValues(ref techSpecialistDetail, dBTechSpecDetail);// sanity defect fix // def 1306 
                    techSpecialistDetail.TechnicalSpecialistInfo = techInfo;
                    techSpecialistDetail.TechnicalSpecialistInfo.ProfessionalAfiliationDocuments = proffAfiliationDocs;//Sanity defect 148 fix
                }
                var prevChangeResp = _draftService.GetDraft(new Draft.Domain.Models.Draft { DraftId = epin.ToString(), Description = DraftType.ProfileChangeHistory.ToString() });
                if (prevChangeResp != null && prevChangeResp.Code == ResponseType.Success.ToId() && prevChangeResp.Result != null)
                {
                    var previousChanges = prevChangeResp.Result.Populate<IList<Draft.Domain.Models.Draft>>()?.FirstOrDefault();
                    if (previousChanges != null)
                    {
                        var techSpecDetailHistory = previousChanges.SerilizableObject.DeSerialize<TechnicalSpecialistDetail>(SerializationType.JSON);

                        IList<KeyValuePair<string, object>> propertyNameValueUpdate = new List<KeyValuePair<string, object>> {
                            new KeyValuePair<string, object> ("ActionByUser", actionByUser),
                            new KeyValuePair<string, object> ("RecordStatus", RecordStatus.Modify.FirstChar()),
                        };

                        IList<KeyValuePair<string, object>> propertyNameValueAdd = new List<KeyValuePair<string, object>> {
                            new KeyValuePair<string, object> ("ActionByUser", actionByUser),
                            new KeyValuePair<string, object> ("UpdatedCount", null),
                            new KeyValuePair<string, object> ("RecordStatus", RecordStatus.New.FirstChar()),
                            new KeyValuePair<string, object>("IsDeleted", false) //def 1306 Issue 1 :  document delete issue fix
                        };

                        IList<KeyValuePair<string, object>> propertyNameValueDelete = new List<KeyValuePair<string, object>> {
                            new KeyValuePair<string, object> ("ActionByUser", actionByUser),
                            new KeyValuePair<string, object> ("RecordStatus", RecordStatus.Delete.FirstChar()),
                        };

                        techSpecDetailHistory.TechnicalSpecialistInfo.SetPropertyValue(new List<KeyValuePair<string, object>> {
                            new KeyValuePair<string, object> ("ApprovalStatus", TechnicalSpecialistConstants.TS_Change_Approval_Status_Reject),
                            new KeyValuePair<string, object> ("BusinessInformationComment", techSpecialistDetail.TechnicalSpecialistInfo.BusinessInformationComment),
                             new KeyValuePair<string, object> ("ProfileAction", techSpecialistDetail.TechnicalSpecialistInfo.ProfileAction),
                            new KeyValuePair<string, object> ("ActionByUser", actionByUser),
                            new KeyValuePair<string, object> ("PendingWithUser", techSpecialistDetail.TechnicalSpecialistInfo.PendingWithUser),//Added for D946 CR
                            new KeyValuePair<string, object> ("IsDraft", true),
                            new KeyValuePair<string, object> ("AssignedToUser", assignedToUser),
                            new KeyValuePair<string, object> ("AssignedByUser", assignedToUser),
                            new KeyValuePair<string, object> ("RecordStatus", RecordStatus.Modify.FirstChar()),
                        });

                        RejectDeletion(techSpecialistDetail, techSpecDetailHistory, propertyNameValueAdd);//Add priviously deleted datas

                        RejectPrevoiusModification(techSpecialistDetail, techSpecDetailHistory, propertyNameValueUpdate);//Revert modified records

                        var UniqCodeStandardNames = techSpecialistDetail.TechnicalSpecialistCodeAndStandard?.Select(x => x.CodeStandardName).ToList();
                        var UniqComputerKnowledges = techSpecialistDetail.TechnicalSpecialistComputerElectronicKnowledge?.Select(x => x.ComputerKnowledge).ToList();
                        var tsCommodityAndEquipment = techSpecialistDetail.TechnicalSpecialistCommodityAndEquipment;

                        RegectNewAddition(techSpecialistDetail, techSpecDetailHistory, propertyNameValueDelete);// Delete newly added records

                        techSpecialistDetail.TechnicalSpecialistContact?.AddRange(techSpecDetailHistory.TechnicalSpecialistContact?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistInternalTraining?.AddRange(techSpecDetailHistory.TechnicalSpecialistInternalTraining?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistCompetancy?.AddRange(techSpecDetailHistory.TechnicalSpecialistCompetancy?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistCustomerApproval?.AddRange(techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistWorkHistory?.AddRange(techSpecDetailHistory.TechnicalSpecialistWorkHistory?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistEducation?.AddRange(techSpecDetailHistory.TechnicalSpecialistEducation?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistLanguageCapabilities?.AddRange(techSpecDetailHistory.TechnicalSpecialistLanguageCapabilities?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistCertification?.AddRange(techSpecDetailHistory.TechnicalSpecialistCertification?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistTraining?.AddRange(techSpecDetailHistory.TechnicalSpecialistTraining?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        techSpecialistDetail.TechnicalSpecialistNotes?.AddRange(techSpecDetailHistory.TechnicalSpecialistNotes?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());
                        
                        techSpecialistDetail.TechnicalSpecialistDocuments?.AddRange(techSpecDetailHistory.TechnicalSpecialistDocuments?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).ToList());//def 1306 Issue 1 :  document delete issue fix

                        techSpecialistDetail.TechnicalSpecialistComputerElectronicKnowledge?.AddRange(techSpecDetailHistory.TechnicalSpecialistComputerElectronicKnowledge?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !UniqComputerKnowledges.Contains(x.ComputerKnowledge)).ToList());
                        techSpecialistDetail.TechnicalSpecialistCodeAndStandard?.AddRange(techSpecDetailHistory.TechnicalSpecialistCodeAndStandard?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !UniqCodeStandardNames.Contains(x.CodeStandardName)).ToList());
                        techSpecialistDetail.TechnicalSpecialistCommodityAndEquipment?.AddRange(techSpecDetailHistory.TechnicalSpecialistCommodityAndEquipment?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !tsCommodityAndEquipment.Any(x1 => x1.Commodity == x.Commodity && x1.EquipmentKnowledge == x.EquipmentKnowledge && x1.EquipmentKnowledgeLevel == x.EquipmentKnowledgeLevel)).ToList());

                        RevertRejectedDocumentValues(techSpecialistDetail, techSpecDetailHistory, propertyNameValueUpdate, propertyNameValueDelete);//Sanity defect 148 fix
                    }
                }
            }
        }

        private void PopulateDatabaseValues(ref TechnicalSpecialistDetail destination, TechnicalSpecialistDetail source)// Sanity defect reject issue fixes
        {
            destination = destination ?? new TechnicalSpecialistDetail();
            if (source != null) {

                if(source?.TechnicalSpecialistStamp?.Count > 0)
                destination.TechnicalSpecialistStamp.AddRange(source.TechnicalSpecialistStamp);
                if (source?.TechnicalSpecialistContact?.Count > 0)
                    destination.TechnicalSpecialistContact.AddRange(source.TechnicalSpecialistContact);
                if (source?.TechnicalSpecialistPaySchedule?.Count > 0)
                    destination.TechnicalSpecialistPaySchedule.AddRange(source.TechnicalSpecialistPaySchedule);
                if (source?.TechnicalSpecialistPayRate?.Count > 0)
                    destination.TechnicalSpecialistPayRate.AddRange(source.TechnicalSpecialistPayRate);
                if (source?.TechnicalSpecialistTaxonomy?.Count > 0)
                    destination.TechnicalSpecialistTaxonomy.AddRange(source.TechnicalSpecialistTaxonomy);
                if (source?.TechnicalSpecialistInternalTraining?.Count > 0)
                    destination.TechnicalSpecialistInternalTraining.AddRange(source.TechnicalSpecialistInternalTraining);
                if (source?.TechnicalSpecialistCompetancy?.Count > 0)
                    destination.TechnicalSpecialistCompetancy.AddRange(source.TechnicalSpecialistCompetancy);
                if (source?.TechnicalSpecialistCustomerApproval?.Count > 0)
                    destination.TechnicalSpecialistCustomerApproval.AddRange(source.TechnicalSpecialistCustomerApproval);
                if (source?.TechnicalSpecialistWorkHistory?.Count > 0)
                    destination.TechnicalSpecialistWorkHistory.AddRange(source.TechnicalSpecialistWorkHistory);
                if (source?.TechnicalSpecialistEducation?.Count > 0)
                    destination.TechnicalSpecialistEducation.AddRange(source.TechnicalSpecialistEducation);
                if (source?.TechnicalSpecialistCodeAndStandard?.Count > 0)
                    destination.TechnicalSpecialistCodeAndStandard.AddRange(source.TechnicalSpecialistCodeAndStandard);
                if (source?.TechnicalSpecialistTraining?.Count > 0)
                    destination.TechnicalSpecialistTraining.AddRange(source.TechnicalSpecialistTraining);
                if (source?.TechnicalSpecialistCertification?.Count > 0)
                    destination.TechnicalSpecialistCertification.AddRange(source.TechnicalSpecialistCertification);
                if (source?.TechnicalSpecialistCommodityAndEquipment?.Count > 0)
                    destination.TechnicalSpecialistCommodityAndEquipment.AddRange(source.TechnicalSpecialistCommodityAndEquipment);
                if (source?.TechnicalSpecialistComputerElectronicKnowledge?.Count > 0)
                    destination.TechnicalSpecialistComputerElectronicKnowledge.AddRange(source.TechnicalSpecialistComputerElectronicKnowledge);
                if (source?.TechnicalSpecialistLanguageCapabilities?.Count > 0)
                    destination.TechnicalSpecialistLanguageCapabilities.AddRange(source.TechnicalSpecialistLanguageCapabilities);
                if (source?.TechnicalSpecialistDocuments?.Count > 0)
                    destination.TechnicalSpecialistDocuments.AddRange(source.TechnicalSpecialistDocuments);
                if (source?.TechnicalSpecialistNotes?.Count > 0)
                    destination.TechnicalSpecialistNotes.AddRange(source.TechnicalSpecialistNotes);
                if (source?.TechnicalSpecialistSensitiveDocuments?.Count > 0)
                    destination.TechnicalSpecialistSensitiveDocuments.AddRange(source.TechnicalSpecialistSensitiveDocuments);
            }
        }
        private void RevertRejectedDocumentValues(TechnicalSpecialistDetail techSpecialistDetail, TechnicalSpecialistDetail techSpecDetailHistory, IList<KeyValuePair<string, object>> propertyNameValueUpdate, IList<KeyValuePair<string, object>>  propertyNameValueDelete)
        { 
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments?.Count > 0)
            { 
                ProcessRejectDocuments(techSpecialistDetail.TechnicalSpecialistInfo.ProfessionalAfiliationDocuments, techSpecDetailHistory.TechnicalSpecialistInfo.ProfessionalAfiliationDocuments, propertyNameValueUpdate, propertyNameValueDelete,techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus);
            } 
            techSpecialistDetail?.TechnicalSpecialistEducation?.ToList()?.ForEach(x =>
            {
                if (x.Documents?.Count > 0)
                { 
                    ProcessRejectDocuments(x.Documents, techSpecDetailHistory.TechnicalSpecialistEducation?.FirstOrDefault(x1=>x1.Id==x.Id)?.Documents?.ToList(), propertyNameValueUpdate, propertyNameValueDelete, x.RecordStatus);
                } 
            }); 

            techSpecialistDetail?.TechnicalSpecialistCustomerApproval?.ToList()?.ForEach(x =>
            { 
                if (x.Documents?.Count > 0)
                {
                    ProcessRejectDocuments(x.Documents, techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.FirstOrDefault(x1 => x1.Id == x.Id)?.Documents?.ToList(), propertyNameValueUpdate, propertyNameValueDelete, x.RecordStatus);
                } 
            });

            techSpecialistDetail?.TechnicalSpecialistCertification?.ToList()?.ForEach(x =>
            {
                if (x.Documents?.Count > 0)
                {
                    ProcessRejectDocuments(x.Documents, techSpecDetailHistory.TechnicalSpecialistCertification?.FirstOrDefault(x1 => x1.Id == x.Id)?.Documents?.ToList(), propertyNameValueUpdate, propertyNameValueDelete, x.RecordStatus);
                }
                if (x.VerificationDocuments?.Count > 0)
                {
                    ProcessRejectDocuments(x.VerificationDocuments, techSpecDetailHistory.TechnicalSpecialistCertification?.FirstOrDefault(x1 => x1.Id == x.Id)?.VerificationDocuments?.ToList(), propertyNameValueUpdate, propertyNameValueDelete, x.RecordStatus);
                }
            });

            techSpecialistDetail?.TechnicalSpecialistTraining?.ToList()?.ForEach(x =>
            {
                if (x.Documents?.Count > 0)
                {
                    ProcessRejectDocuments(x.Documents, techSpecDetailHistory.TechnicalSpecialistTraining?.FirstOrDefault(x1 => x1.Id == x.Id)?.Documents?.ToList(), propertyNameValueUpdate, propertyNameValueDelete, x.RecordStatus);
                }
                if (x.VerificationDocuments?.Count > 0)
                {
                    ProcessRejectDocuments(x.VerificationDocuments, techSpecDetailHistory.TechnicalSpecialistTraining?.FirstOrDefault(x1 => x1.Id == x.Id)?.VerificationDocuments?.ToList(), propertyNameValueUpdate, propertyNameValueDelete, x.RecordStatus);
                }
            });
        }
        //Sanity defect 148 fix
        private void ProcessRejectDocuments(IList<ModuleDocument> moduleDocuments, IList<ModuleDocument> moduleHistoryDocuments, IList<KeyValuePair<string, object>> propertyNameValueUpdate, IList<KeyValuePair<string, object>> propertyNameValueDelete,string recordStatus)
        { 
            if (propertyNameValueUpdate != null && propertyNameValueUpdate.Any())
                propertyNameValueUpdate.Add(new KeyValuePair<string, object>("IsDeleted", false));
            if (propertyNameValueDelete != null && propertyNameValueDelete.Any())
                propertyNameValueDelete.Add(new KeyValuePair<string, object>("IsDeleted", true));

            if (moduleDocuments != null && moduleDocuments.Any())
            { 
                moduleDocuments.Select(x =>
                {
                    if (!x.RecordStatus.IsRecordStatusDeleted() && (recordStatus.IsRecordStatusNew() || recordStatus.IsRecordStatusModified()))
                    {
                        x.SetPropertyValue(propertyNameValueUpdate);
                    }
                    if (recordStatus.IsRecordStatusDeleted() || x.Id > 0 && (moduleHistoryDocuments!=null &&  !moduleHistoryDocuments.Any(x1 => x1.Id == x.Id)))
                    {
                        x.SetPropertyValue(propertyNameValueDelete);
                    }
                    return x;
                }).ToList(); 
            }
        }

        private void RegectNewAddition(TechnicalSpecialistDetail techSpecialistDetail, TechnicalSpecialistDetail techSpecDetailHistory, IList<KeyValuePair<string, object>> propertyNameValueDelete)
        {
            string draftId=techSpecialistDetail?.TechnicalSpecialistInfo?.DraftId;
            string draftType = techSpecialistDetail?.TechnicalSpecialistInfo?.DraftType;
            var profAfiliationDocuments=techSpecialistDetail.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments?.Where(x => x.Id > 0 && (techSpecDetailHistory.TechnicalSpecialistInfo.ProfessionalAfiliationDocuments !=null && !techSpecDetailHistory.TechnicalSpecialistInfo.ProfessionalAfiliationDocuments.Any(x1 => x1.Id == x.Id)))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; }).ToList();//Sanity defect 148 fix
            techSpecialistDetail.TechnicalSpecialistInfo = techSpecDetailHistory.TechnicalSpecialistInfo;
            techSpecialistDetail.TechnicalSpecialistInfo.DraftId = draftId; //sanity def 156
            techSpecialistDetail.TechnicalSpecialistInfo.DraftType = draftType;//sanity def 156

            if (techSpecialistDetail?.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments?.Count >0) //Sanity defect 148 fix
                techSpecialistDetail.TechnicalSpecialistInfo.ProfessionalAfiliationDocuments.AddRange(profAfiliationDocuments);

            techSpecialistDetail.TechnicalSpecialistContact?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistContact.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistInternalTraining?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistInternalTraining.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistCompetancy?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistCompetancy.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistCustomerApproval?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistCustomerApproval.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistWorkHistory?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistWorkHistory.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistEducation?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistEducation.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistCodeAndStandard?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistCodeAndStandard.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistComputerElectronicKnowledge?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistComputerElectronicKnowledge.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistLanguageCapabilities?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistLanguageCapabilities.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistCertification?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistCertification.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistCommodityAndEquipment?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistCommodityAndEquipment.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistTraining?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistTraining.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistNotes?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistNotes.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();
            techSpecialistDetail.TechnicalSpecialistDocuments?.Where(x => x.Id > 0 && !techSpecDetailHistory.TechnicalSpecialistDocuments.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueDelete); return x; })?.ToList();

        }

        private void RejectDeletion(TechnicalSpecialistDetail dBTechSpecDetail, TechnicalSpecialistDetail techSpecDetailHistory, IList<KeyValuePair<string, object>> propertyNameValueAdd)
        {
            // Add deleted datas
            techSpecDetailHistory.TechnicalSpecialistDocuments?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistDocuments.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistContact?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistContact.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistInternalTraining?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistInternalTraining.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCompetancy?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistCompetancy.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistCustomerApproval.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistWorkHistory?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistWorkHistory.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistEducation?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistEducation.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCodeAndStandard?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistCodeAndStandard.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistComputerElectronicKnowledge?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistComputerElectronicKnowledge.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistLanguageCapabilities?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistLanguageCapabilities.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCertification?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistCertification.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCommodityAndEquipment?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistCommodityAndEquipment.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistTraining?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistTraining.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Where(x => x.Id > 0 && !dBTechSpecDetail.TechnicalSpecialistCustomerApproval.Any(x1 => x1.Id == x.Id))?.Select(x => { x.Id = -1; x.SetPropertyValue(propertyNameValueAdd); return x; })?.ToList();
           
        }

        private void RejectPrevoiusModification(TechnicalSpecialistDetail dBTechSpecDetail, TechnicalSpecialistDetail techSpecDetailHistory, IList<KeyValuePair<string, object>> propertyNameValueUpdate)
        {
            //Revert if any modified records
            techSpecDetailHistory.TechnicalSpecialistDocuments?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistDocuments.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistContact?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistContact.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistInternalTraining?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistInternalTraining.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCompetancy?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistCompetancy.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistCustomerApproval.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistWorkHistory?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistWorkHistory.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistEducation?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistEducation.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCodeAndStandard?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistCodeAndStandard.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistComputerElectronicKnowledge?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistComputerElectronicKnowledge.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistLanguageCapabilities?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistLanguageCapabilities.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCertification?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistCertification.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCommodityAndEquipment?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistCommodityAndEquipment.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistTraining?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistTraining.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            techSpecDetailHistory.TechnicalSpecialistCustomerApproval?.Where(x => x.Id > 0 && dBTechSpecDetail.TechnicalSpecialistCustomerApproval.Any(x1 => x1.Id == x.Id))?.Select(x => { x.SetPropertyValue(propertyNameValueUpdate); return x; })?.ToList();
            
        }

        private TechnicalSpecialistDetail GetTsDetails(int ePin)
        {
            var tsDocs= _documentService.Get(ModuleCodeType.TS, new List<string> { ePin.ToString() }, new List<string> { "-1", "0" }).Result.Populate<IList<ModuleDocument>>();//def 1306 Issue 1 :  document delete issue fix

            return new TechnicalSpecialistDetail
            {
                TechnicalSpecialistInfo = _technicalSpecialistService.Get(new TechnicalSpecialistInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistInfo>>()?.FirstOrDefault(),
                TechnicalSpecialistStamp = _tsStampInfoService.Get(new TechnicalSpecialistStampInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistStampInfo>>(),
                TechnicalSpecialistContact = _tsContactService.Get(new TechnicalSpecialistContactInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistContactInfo>>(),
                TechnicalSpecialistPaySchedule = _tsPayScheduleService.Get(new TechnicalSpecialistPayScheduleInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistPayScheduleInfo>>(),
                TechnicalSpecialistPayRate = _tsPayRateService.Get(new TechnicalSpecialistPayRateInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistPayRateInfo>>(),
                TechnicalSpecialistTaxonomy = _tsTaxonomyService.Get(new TechnicalSpecialistTaxonomyInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistTaxonomyInfo>>(),
                TechnicalSpecialistInternalTraining = _tsInternalTrainingService.Get(new TechnicalSpecialistInternalTraining { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistInternalTraining>>(),
                TechnicalSpecialistCompetancy = _tsCompetencyService.Get(new TechnicalSpecialistCompetency { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistCompetency>>(),
                TechnicalSpecialistCustomerApproval = _tsCustomerApprovalService.Get(new TechnicalSpecialistCustomerApprovalInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistCustomerApprovalInfo>>(),
                TechnicalSpecialistWorkHistory = _tsWorkHistoryService.Get(new TechnicalSpecialistWorkHistoryInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistWorkHistoryInfo>>(),
                TechnicalSpecialistEducation = _tsEducationalQualificationService.Get(new TechnicalSpecialistEducationalQualificationInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistEducationalQualificationInfo>>(),
                TechnicalSpecialistCodeAndStandard = _tsCodeAndStandardService.Get(new TechnicalSpecialistCodeAndStandardinfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistCodeAndStandardinfo>>(),
                TechnicalSpecialistTraining = _tsTrainingService.Get(new TechnicalSpecialistTraining { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistTraining>>(),
                TechnicalSpecialistCertification = _tsCertificationService.Get(new TechnicalSpecialistCertification { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistCertification>>(),
                TechnicalSpecialistCommodityAndEquipment = _tsCommodityEquipmentKnowledgeService.Get(new TechnicalSpecialistCommodityEquipmentKnowledgeInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>>(),
                TechnicalSpecialistComputerElectronicKnowledge = _tsComputerElectronicKnowledgeService.Get(new TechnicalSpecialistComputerElectronicKnowledgeInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistComputerElectronicKnowledgeInfo>>(),
                TechnicalSpecialistLanguageCapabilities = _tsLanguageCapabilityService.Get(new TechnicalSpecialistLanguageCapabilityInfo { Epin = ePin }).Result.Populate<IList<TechnicalSpecialistLanguageCapabilityInfo>>(),
                TechnicalSpecialistDocuments = tsDocs?.Where(x=>x.SubModuleRefCode == "-1")?.ToList(), //def 1306 Issue 1 :  document delete issue fix
                TechnicalSpecialistSensitiveDocuments = tsDocs?.Where(x => x.SubModuleRefCode == "0")?.ToList(), //def 1306 Issue 1 :  document delete issue fix
                TechnicalSpecialistNotes = _tsNoteService.Get(new List<int>() { ePin }, new List<string>() { "TSComment", "General" }).Result.Populate<IList<TechnicalSpecialistNoteInfo>>(),
            };

        }


        #endregion

        #region Modal Validations

        private Response ValidateInputModel(TechnicalSpecialistDetail techSpecialistDetail, ValidationType validationType, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Response response = null;

            tsModuleRefDataCollection = tsModuleRefDataCollection ?? new TechnicalSpecialistModuleRefDataCollection();

            if (ValidationType.Update == validationType || ValidationType.Delete == validationType)
            {
                response = _technicalSpecialistService.IsRecordValidForProcess(new List<TechnicalSpecialistInfo> { techSpecialistDetail.TechnicalSpecialistInfo },
                                                                  validationType,
                                                                  ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                                                                  ref tsModuleRefDataCollection.DbCompanies,
                                                                  ref tsModuleRefDataCollection.DbCompPayrolls,
                                                                  ref tsModuleRefDataCollection.DbSubDivisions,
                                                                  ref tsModuleRefDataCollection.DbStatuses,
                                                                  ref tsModuleRefDataCollection.DbActions,
                                                                  ref tsModuleRefDataCollection.DbEmploymentTypes,
                                                                  ref tsModuleRefDataCollection.DbCountries,
                                                                  techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);

                if (techSpecialistDetail?.TechnicalSpecialistInfo != null && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
                {
                    response = IsValidTSLoginCredential(techSpecialistDetail.TechnicalSpecialistInfo, tsModuleRefDataCollection);
                }
            }

            //if (techSpecialistDetail?.TechnicalSpecialistInfo != null && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            //{
            //    response = IsValidTSLoginCredential(techSpecialistDetail.TechnicalSpecialistInfo, tsModuleRefDataCollection);
            //} For TS Insert, Adding this function inside the ADD Method

            if (techSpecialistDetail?.TechnicalSpecialistCertification?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTsCertificationInfo(techSpecialistDetail.TechnicalSpecialistCertification, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistTraining?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSTrainingInfo(techSpecialistDetail.TechnicalSpecialistTraining, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }

            if (techSpecialistDetail?.TechnicalSpecialistCompetancy?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSCompetencyInfo(techSpecialistDetail.TechnicalSpecialistCompetancy, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistInternalTraining?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSInternalTrainingInfo(techSpecialistDetail.TechnicalSpecialistInternalTraining, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistCodeAndStandard?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSCodeAndStandardInfo(techSpecialistDetail.TechnicalSpecialistCodeAndStandard, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSCommodityEquipmentKnowledgeInfo(techSpecialistDetail.TechnicalSpecialistCommodityAndEquipment, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistComputerElectronicKnowledge?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSComputerElectronicKnowledgeInfo(techSpecialistDetail.TechnicalSpecialistComputerElectronicKnowledge, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistCustomerApproval?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSCustomerApprovalInfo(techSpecialistDetail.TechnicalSpecialistCustomerApproval, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSLanguageCapabilityInfo(techSpecialistDetail.TechnicalSpecialistLanguageCapabilities, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistStamp?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSStampInfo(techSpecialistDetail.TechnicalSpecialistStamp, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistWorkHistory?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSWorkHistoryInfo(techSpecialistDetail.TechnicalSpecialistWorkHistory, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistTaxonomy?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSTaxonomyInfo(techSpecialistDetail.TechnicalSpecialistTaxonomy, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistEducation?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSEducationalQualificationInfo(techSpecialistDetail.TechnicalSpecialistEducation, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistContact?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSContactInfo(techSpecialistDetail.TechnicalSpecialistContact, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }

            if (techSpecialistDetail?.TechnicalSpecialistPaySchedule?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSPayScheduleInfo(techSpecialistDetail.TechnicalSpecialistPaySchedule, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistPayRate?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSPayRateInfo(techSpecialistDetail.TechnicalSpecialistPayRate, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft);
            }
            if (techSpecialistDetail?.TechnicalSpecialistNotes?.Count(x => !string.IsNullOrEmpty(x.RecordStatus)) > 0 && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))))
            {
                response = IsValidTSNoteInfo(techSpecialistDetail.TechnicalSpecialistNotes, tsModuleRefDataCollection, techSpecialistDetail.TechnicalSpecialistInfo.IsDraft); //D661 issue 8 Start
            }

            return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, true, null);
        }

        private bool ValidateLinkTableData(TechnicalSpecialistDetail techSpecialistDetail, ref IList<ValidationMessage> validationMessages, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.Id > 0)
            {
                _technicalSpecialistService.IsRecordExistInDb(new List<string> { techSpecialistDetail?.TechnicalSpecialistInfo.Epin.ToString() },
                    ref tsModuleRefDataCollection.DbTechnicalSpecialists,
                    ref validationMessages,
                    tsc => tsc.TechnicalSpecialistContact,
                    tss => tss.TechnicalSpecialistPaySchedule);

                tsModuleRefDataCollection.DbTsContacts = tsModuleRefDataCollection?.DbTechnicalSpecialists?.SelectMany(x => x.TechnicalSpecialistContact).ToList();
                tsModuleRefDataCollection.DbTsPaySchedules = tsModuleRefDataCollection?.DbTechnicalSpecialists?.SelectMany(x => x.TechnicalSpecialistPaySchedule).ToList();
            }

            IList<string> certificationNames = techSpecialistDetail?.TechnicalSpecialistCertification?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.CertificationName).Distinct().ToList();
            if (certificationNames != null && certificationNames.Any())
            {
                _certificationsService.IsValidCertification(certificationNames, ref tsModuleRefDataCollection.DbCertificationTypes, ref validationMessages);
            }

            IList<string> trainingNames = techSpecialistDetail?.TechnicalSpecialistTraining?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.TrainingName).Distinct().ToList();
            if (trainingNames != null && trainingNames.Any())
            {
                _trainingService.IsValidTraining(trainingNames, ref tsModuleRefDataCollection.DbTrainingTypes, ref validationMessages);
            }

            IList<string> payCurrencies = techSpecialistDetail?.TechnicalSpecialistPaySchedule?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.PayCurrency).Distinct().ToList();
            if (payCurrencies != null && payCurrencies.Any())
            {
                _currencyService.IsValidCurrency(payCurrencies, ref tsModuleRefDataCollection.DbCurrencies, ref validationMessages);
            }

            IList<string> codeStandardNames = techSpecialistDetail?.TechnicalSpecialistCodeAndStandard?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.CodeStandardName).ToList();
            if (codeStandardNames != null && codeStandardNames.Any())
            {
                _codeStandardService.IsValidCodeAndStandardName(codeStandardNames, ref tsModuleRefDataCollection.DbCodeAndStandards, ref validationMessages);
            }

            IList<string> commodities = techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.Commodity).ToList();
            if (commodities != null && commodities.Any())
            {
                _commodityService.IsValidCommodityName(commodities, ref tsModuleRefDataCollection.DbCommodities, ref validationMessages);
            }
            IList<string> equipments = techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.EquipmentKnowledge).ToList();
            if (equipments != null && equipments.Any())
            {
                _equipmentService.IsValidEquipmentName(equipments, ref tsModuleRefDataCollection.DbEquipments, ref validationMessages);
            }

            IList<string> computerknowledges = techSpecialistDetail?.TechnicalSpecialistComputerElectronicKnowledge?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.ComputerKnowledge).ToList();
            if (computerknowledges != null && computerknowledges.Any())
            {
                _computerKnowledgeService.IsValidComputerKnowledgeName(computerknowledges, ref tsModuleRefDataCollection.DbComputerElectronicsKnowledges, ref validationMessages);
            }

            IList<string> customerCodes = techSpecialistDetail?.TechnicalSpecialistCustomerApproval?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.CustomerCode).ToList();
            if (customerCodes != null && customerCodes.Any())
            {
                _customerService.IsValidCustomer(customerCodes, ref tsModuleRefDataCollection.DbTechSpecCustomers, ref validationMessages);
            }

            IList<string> languages = techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.Language).ToList();
            if (languages != null && languages.Any())
            {
                _langService.IsValidLanguage(languages, ref tsModuleRefDataCollection.DbLanguages, ref validationMessages);
            }

            var stmpCountry = techSpecialistDetail?.TechnicalSpecialistStamp?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !string.IsNullOrEmpty(x.CountryName))?.Select(x => x.CountryName).ToList();
            if (stmpCountry != null && stmpCountry.Any())
            {
                _tsStampCountryCodeService.IsValidTechnicalSpecialistStampCountryCode(stmpCountry, ref tsModuleRefDataCollection.DbTsStampCountries, ref validationMessages);
            }

            IList<string> taxonomyCategories = techSpecialistDetail?.TechnicalSpecialistTaxonomy?.Where(x => !string.IsNullOrEmpty(x.RecordStatus)).Select(x => x.TaxonomyCategoryName).ToList();
            if (taxonomyCategories != null && taxonomyCategories.Any())
            {
                _taxonomyCatService.IsValidCategoryName(taxonomyCategories,
                    ref tsModuleRefDataCollection.DbCategories,
                    ref validationMessages);

                tsModuleRefDataCollection.DbSubCategories = tsModuleRefDataCollection?.DbCategories?.SelectMany(x => x.TaxonomySubCategory).ToList();
                tsModuleRefDataCollection.DbTaxonomyService = tsModuleRefDataCollection?.DbSubCategories?.SelectMany(x => x.TaxonomyService).ToList();
            }



            List<string> countries = new List<string>();
            var contCountry = techSpecialistDetail?.TechnicalSpecialistContact?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !string.IsNullOrEmpty(x.Country))?.Select(x => x.Country).ToList();
            var eduCountry = techSpecialistDetail?.TechnicalSpecialistEducation?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !string.IsNullOrEmpty(x.CountryName))?.Select(x => x.CountryName).ToList();

            if (contCountry != null && contCountry.Any())
            {
                countries.AddRange(contCountry);
            }
            if (eduCountry != null && eduCountry.Any())
            {
                countries.AddRange(eduCountry);
            }

            if (!string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.PassportCountryName))
            {
                countries.AddRange(new List<string> { techSpecialistDetail?.TechnicalSpecialistInfo?.PassportCountryName });
            }

            if (countries != null && countries.Any())
            {
                string[] includes = new string[] { "County", "County.City" };
                _countryService.IsValidCountryName(countries, ref tsModuleRefDataCollection.DbCountries, ref validationMessages, includes);

                tsModuleRefDataCollection.DbCounties = tsModuleRefDataCollection.DbCountries.SelectMany(x => x.County).ToList();
                tsModuleRefDataCollection.DbCities = tsModuleRefDataCollection.DbCounties.SelectMany(x => x.City).ToList();
            }

            IList<string> userNotExists = null;
            List<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            var certVerifiedByUsers = techSpecialistDetail?.TechnicalSpecialistCertification?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !string.IsNullOrEmpty(x.VerifiedBy))
                                                        .Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).Distinct().ToList();
            var trainVerifiedByUsers = techSpecialistDetail?.TechnicalSpecialistTraining?.Where(x => !string.IsNullOrEmpty(x.RecordStatus) && !string.IsNullOrEmpty(x.VerifiedBy)).Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList();
            if (certVerifiedByUsers != null && certVerifiedByUsers.Any())
            {
                users.AddRange(certVerifiedByUsers);
            }
            if (trainVerifiedByUsers != null && trainVerifiedByUsers.Any())
            {
                users.AddRange(trainVerifiedByUsers);
            }
            var valResponse = _userService.IsRecordExistInDb(users, ref tsModuleRefDataCollection.DbVarifiedByUsers, ref userNotExists);
            if (!Convert.ToBoolean(valResponse.Result))
            {
                validationMessages.AddRange(valResponse.ValidationMessages);
            }

            return !(validationMessages?.Count > 0);
        }

        private bool ValidateMandatoryFieldData(TechnicalSpecialistDetail techSpecialistDetail,bool isPayloadValidationRequired, ref IList<ValidationMessage> mandatoryValidationMessages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            mandatoryValidationMessages= mandatoryValidationMessages ?? new List<ValidationMessage>();
            
            if (techSpecialistDetail == null || techSpecialistDetail?.TechnicalSpecialistInfo == null)
            {
                validationMessages.Add(new ValidationMessage(techSpecialistDetail, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.RecordStatus))
            {
                validationMessages.Add(new ValidationMessage(techSpecialistDetail?.TechnicalSpecialistInfo, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.TechSpec_InvalidRecordStatus.ToId(), _messages[MessageType.TechSpec_InvalidRecordStatus.ToId()].ToString()) }));
            }
            if (isPayloadValidationRequired)
            {
                if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction))
                {
                    validationMessages.Add(new ValidationMessage(techSpecialistDetail, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_ProfileAction.ToId(), _messages[MessageType.Madatory_Param_ProfileAction.ToId()].ToString()) }));
                }
                else if (string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Create_Update_Profile) || string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TS) || string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TM) || string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_RC_RM))
                {
                    IsValidResourceProfile(techSpecialistDetail, validationMessages);
                }
                if (string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TM) || string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_RC_RM))
                {
                    if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified())
                        && (techSpecialistDetail?.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments == null
                        ||  techSpecialistDetail?.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments?.Count(x=> !x.RecordStatus.IsRecordStatusDeleted())==0))
                    {
                        validationMessages.Add(new ValidationMessage(new { techSpecialistDetail?.TechnicalSpecialistInfo?.ProfessionalAfiliationDocuments }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Professional_Educational_uploadCV.ToId(), _messages[MessageType.Madatory_Param_Professional_Educational_uploadCV.ToId()].ToString()) }));
                    }
                    if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified())
                       && (techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment == null
                       || techSpecialistDetail?.TechnicalSpecialistCommodityAndEquipment?.Count(x => !x.RecordStatus.IsRecordStatusDeleted()) == 0))
                    {
                        validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistCommodityAndEquipment }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Commodity_Equipment_Knowledge.ToId(), _messages[MessageType.Madatory_Param_Commodity_Equipment_Knowledge.ToId()].ToString()) }));
                    }
                }

                if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified()) &&
                     techSpecialistDetail?.TechnicalSpecialistPaySchedule?.Count(x => !x.RecordStatus.IsRecordStatusDeleted()) > 0)
                {
                    if (techSpecialistDetail?.TechnicalSpecialistPayRate?.Count(x => !x.RecordStatus.IsRecordStatusDeleted()) == 0)
                    {
                        validationMessages.Add(new ValidationMessage(new { techSpecialistDetail?.TechnicalSpecialistPaySchedule, techSpecialistDetail?.TechnicalSpecialistPayRate }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_PayScheduleWithOneRate.ToId(), _messages[MessageType.Madatory_Param_PayScheduleWithOneRate.ToId()].ToString()) }));
                    }
                    else if (techSpecialistDetail.TechnicalSpecialistPaySchedule.Any(x => !techSpecialistDetail.TechnicalSpecialistPayRate.Any(x1 => x1.PayScheduleName == x.PayScheduleName && x1.PayScheduleCurrency == x.PayCurrency && !x.RecordStatus.IsRecordStatusDeleted())))
                    {
                        validationMessages.Add(new ValidationMessage(new { techSpecialistDetail?.TechnicalSpecialistPaySchedule, techSpecialistDetail?.TechnicalSpecialistPayRate }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_PayScheduleWithOneRate.ToId(), _messages[MessageType.Madatory_Param_PayScheduleWithOneRate.ToId()].ToString()) }));
                    }
                }

                if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified())
                    && techSpecialistDetail?.TechnicalSpecialistPayRate?.Count(x => !x.RecordStatus.IsRecordStatusDeleted()) > 0 && techSpecialistDetail?.TechnicalSpecialistPaySchedule?.Count(x => !x.RecordStatus.IsRecordStatusDeleted()) == 0)
                {
                    validationMessages.Add(new ValidationMessage(new { techSpecialistDetail?.TechnicalSpecialistPaySchedule, techSpecialistDetail?.TechnicalSpecialistPayRate }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_PayRateWithOneSchedule.ToId(), _messages[MessageType.Madatory_Param_PayRateWithOneSchedule.ToId()].ToString()) }));
                }
                if (string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_RC_RM))
                {
                    IsValidInfoByResource(techSpecialistDetail, validationMessages);
                }

                if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified()) &&
                     techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileStatus == ResourceProfileStatus.Active.DisplayName())
                {
                    IsValidActivetedProfile(techSpecialistDetail, validationMessages);
                }

                //Added For IGOQC D889 #3 Issue 
                if (techSpecialistDetail?.TechnicalSpecialistEducation?.Count(x => x.RecordStatus.IsRecordStatusNew()) > 0 || techSpecialistDetail?.TechnicalSpecialistEducation?.Count(x => x.RecordStatus.IsRecordStatusModified()) > 0)
                {
                    var tsEduQulification = techSpecialistDetail?.TechnicalSpecialistEducation;
                        if (tsEduQulification != null && tsEduQulification.Any(x => x.Documents == null || x.Documents?.Count == 0))
                             validationMessages.Add(new ValidationMessage(techSpecialistDetail?.TechnicalSpecialistEducation, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.QualificationDocument.ToId(), _messages[MessageType.QualificationDocument.ToId()].ToString()) }));
                        else if (tsEduQulification.SelectMany(x => x.Documents).Any(x => string.IsNullOrEmpty(x.DocumentName)))
                            validationMessages.Add(new ValidationMessage(techSpecialistDetail?.TechnicalSpecialistEducation, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.QualificationDocument.ToId(), _messages[MessageType.QualificationDocument.ToId()].ToString()) }));
                } //Added For IGOQC D889 #3 Issue 

                CheckForDuplicateRecords(techSpecialistDetail, validationMessages);

            }
                if (validationMessages?.Count > 0)
                {
                    mandatoryValidationMessages.AddRange(validationMessages);
                }
            
            return validationMessages?.Count == 0 ;
        }
        private void CheckForDuplicateRecords(TechnicalSpecialistDetail techSpecialistDetail, List<ValidationMessage> validationMessages)
        {
            if ( techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities?.Count >0
                   && techSpecialistDetail.TechnicalSpecialistLanguageCapabilities.Where(x=>!x.RecordStatus.IsRecordStatusDeleted()).GroupBy(x=>x.Language).Any(x=> x.Count()>1))
            {
                techSpecialistDetail.TechnicalSpecialistLanguageCapabilities.Where(x => !x.RecordStatus.IsRecordStatusDeleted()).GroupBy(x => x.Language).Where(x => x.Count() > 1).ToList().ForEach(x => {
                    validationMessages.Add(new ValidationMessage(new { x.Key }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Duplicate_Record_LanguageCapability.ToId(), _messages[MessageType.Duplicate_Record_LanguageCapability.ToId()].ToString()) }));
                });

            }
            if (techSpecialistDetail?.TechnicalSpecialistStamp?.Count > 0
                   && techSpecialistDetail.TechnicalSpecialistStamp.Where(x => !x.RecordStatus.IsRecordStatusDeleted() &&  x.ReturnedDate==null).GroupBy(x => x.IsSoftStamp).Any(x => x.Count() > 1))
            {
                techSpecialistDetail.TechnicalSpecialistStamp.Where(x => !x.RecordStatus.IsRecordStatusDeleted() && x.ReturnedDate == null).GroupBy(x => x.IsSoftStamp).Where(x => x.Count() > 1).ToList().ForEach(x => {
                    validationMessages.Add(new ValidationMessage(new { x.Key }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Duplicate_Record_HARD_SOFT_STAMP.ToId(), _messages[MessageType.Duplicate_Record_HARD_SOFT_STAMP.ToId()].ToString()) }));
                });

            }
        }

        private void IsValidResourceProfile(TechnicalSpecialistDetail techSpecialistDetail, List<ValidationMessage> validationMessages)
        {
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyCode))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.CompanyCode }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_CompanyCode.ToId(), _messages[MessageType.Madatory_Param_CompanyCode.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.SubDivisionName))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.SubDivisionName }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_SubDivisionName.ToId(), _messages[MessageType.Madatory_Param_SubDivisionName.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.ProfileStatus))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.ProfileStatus }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_ProfileStatus.ToId(), _messages[MessageType.Madatory_Param_ProfileStatus.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.FirstName))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.FirstName }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_FirstName.ToId(), _messages[MessageType.Madatory_Param_FirstName.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.LastName))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.LastName }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_LastName.ToId(), _messages[MessageType.Madatory_Param_LastName.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.LogonName))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.LogonName }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_UserName.ToId(), _messages[MessageType.Madatory_Param_UserName.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.Password))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.Password }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Password.ToId(), _messages[MessageType.Madatory_Param_Password.ToId()].ToString()) }));
            }
            if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified()) && techSpecialistDetail?.TechnicalSpecialistContact != null && techSpecialistDetail.TechnicalSpecialistContact.Count > 0)
            {
                if (!techSpecialistDetail.TechnicalSpecialistContact.Any(x => x.ContactType == ContactType.PrimaryMobile.ToString()))
                {
                    validationMessages.Add(new ValidationMessage(new { MobileNumber = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Primary_MobileNumber.ToId(), _messages[MessageType.Madatory_Param_Primary_MobileNumber.ToId()].ToString()) }));
                }
                if (!techSpecialistDetail.TechnicalSpecialistContact.Any(x => x.ContactType == ContactType.PrimaryEmail.ToString()))
                {
                    validationMessages.Add(new ValidationMessage(new { EmailAddress = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Primary_Email.ToId(), _messages[MessageType.Madatory_Param_Primary_Email.ToId()].ToString()) }));
                }
                if (!techSpecialistDetail.TechnicalSpecialistContact.Any(x => x.ContactType == ContactType.PrimaryAddress.ToString()))
                {
                    validationMessages.Add(new ValidationMessage(new { Country = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Country.ToId(), _messages[MessageType.Madatory_Param_Country.ToId()].ToString()) }));
                    validationMessages.Add(new ValidationMessage(new { County = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_State.ToId(), _messages[MessageType.Madatory_Param_State.ToId()].ToString()) }));
                    validationMessages.Add(new ValidationMessage(new { City = "", PostalCode = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_CityOrPostalCode.ToId(), _messages[MessageType.Madatory_Param_CityOrPostalCode.ToId()].ToString()) }));
                    validationMessages.Add(new ValidationMessage(new { Address = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Address.ToId(), _messages[MessageType.Madatory_Param_Address.ToId()].ToString()) }));
                }
            }
            if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified()) && techSpecialistDetail?.TechnicalSpecialistContact != null && techSpecialistDetail.TechnicalSpecialistContact.Count > 0)
            {
                techSpecialistDetail.TechnicalSpecialistContact.ToList()
                .ForEach(x =>
                {
                    if (x.ContactType == ContactType.PrimaryMobile.ToString() && string.IsNullOrEmpty(x.MobileNumber))
                        validationMessages.Add(new ValidationMessage(new { x.MobileNumber }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Primary_MobileNumber.ToId(), _messages[MessageType.Madatory_Param_Primary_MobileNumber.ToId()].ToString()) }));
                    if (x.ContactType == ContactType.PrimaryEmail.ToString() && string.IsNullOrEmpty(x.EmailAddress))
                        validationMessages.Add(new ValidationMessage(new { x.EmailAddress }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Primary_Email.ToId(), _messages[MessageType.Madatory_Param_Primary_Email.ToId()].ToString()) }));
                    if (string.IsNullOrEmpty(x.Country) && x.ContactType == ContactType.PrimaryAddress.ToString())
                        validationMessages.Add(new ValidationMessage(new { x.Country }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Country.ToId(), _messages[MessageType.Madatory_Param_Country.ToId()].ToString()) }));
                    if (string.IsNullOrEmpty(x.County) && x.ContactType == ContactType.PrimaryAddress.ToString())
                        validationMessages.Add(new ValidationMessage(new { x.County }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_State.ToId(), _messages[MessageType.Madatory_Param_State.ToId()].ToString()) }));
                    if ((string.IsNullOrEmpty(x.City) && string.IsNullOrEmpty(x.PostalCode)) && x.ContactType == ContactType.PrimaryAddress.ToString())
                        validationMessages.Add(new ValidationMessage(new { x.City, x.PostalCode }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_CityOrPostalCode.ToId(), _messages[MessageType.Madatory_Param_CityOrPostalCode.ToId()].ToString()) }));
                    if ((string.IsNullOrEmpty(x.Address)) && x.ContactType == ContactType.PrimaryAddress.ToString())
                        validationMessages.Add(new ValidationMessage(new { x.Address }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Address.ToId(), _messages[MessageType.Madatory_Param_Address.ToId()].ToString()) }));
                });
            } 
            else if ((techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusNew() || techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified()) && (techSpecialistDetail?.TechnicalSpecialistContact == null || techSpecialistDetail?.TechnicalSpecialistContact?.Count == 0))
            {
                validationMessages.Add(new ValidationMessage(new { MobileNumber = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Primary_MobileNumber.ToId(), _messages[MessageType.Madatory_Param_Primary_MobileNumber.ToId()].ToString()) }));

                validationMessages.Add(new ValidationMessage(new { EmailAddress = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Primary_Email.ToId(), _messages[MessageType.Madatory_Param_Primary_Email.ToId()].ToString()) }));

                validationMessages.Add(new ValidationMessage(new { Country = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Country.ToId(), _messages[MessageType.Madatory_Param_Country.ToId()].ToString()) }));

                validationMessages.Add(new ValidationMessage(new { County = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_State.ToId(), _messages[MessageType.Madatory_Param_State.ToId()].ToString()) }));

                validationMessages.Add(new ValidationMessage(new { City = "", PostalCode = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_CityOrPostalCode.ToId(), _messages[MessageType.Madatory_Param_CityOrPostalCode.ToId()].ToString()) }));

                validationMessages.Add(new ValidationMessage(new { Address = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Address.ToId(), _messages[MessageType.Madatory_Param_Address.ToId()].ToString()) }));
            }
        }

        private void IsValidInfoByResource(TechnicalSpecialistDetail techSpecialistDetail, List<ValidationMessage> validationMessages)
        {
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.Salutation))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.Salutation }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Salutation.ToId(), _messages[MessageType.Madatory_Param_Salutation.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityQuestion))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.SecurityQuestion }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_SecurityQuestion.ToId(), _messages[MessageType.Madatory_Param_SecurityQuestion.ToId()].ToString()) }));
            }
            if (string.IsNullOrEmpty(techSpecialistDetail?.TechnicalSpecialistInfo?.SecurityAnswer))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.SecurityAnswer }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_SecurityAnswer.ToId(), _messages[MessageType.Madatory_Param_SecurityAnswer.ToId()].ToString()) }));
            }
            if (techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified() && string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.PrevProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TS)
                && (techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities == null
                || (techSpecialistDetail?.TechnicalSpecialistLanguageCapabilities != null
                    && !techSpecialistDetail.TechnicalSpecialistLanguageCapabilities.Any(x => x.RecordStatus.IsRecordStatusNew() || x.RecordStatus.IsRecordStatusModified()))))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistLanguageCapabilities }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_LanguageCapabilities.ToId(), _messages[MessageType.Madatory_Param_LanguageCapabilities.ToId()].ToString()) }));
            } 
            if (techSpecialistDetail.TechnicalSpecialistInfo.RecordStatus.IsRecordStatusModified() && string.Equals(techSpecialistDetail?.TechnicalSpecialistInfo?.PrevProfileAction, TechnicalSpecialistConstants.Profile_Action_Send_To_TS)
               && (techSpecialistDetail?.TechnicalSpecialistWorkHistory == null
               || (techSpecialistDetail?.TechnicalSpecialistWorkHistory != null
                   && !techSpecialistDetail.TechnicalSpecialistWorkHistory.Any(x => x.RecordStatus.IsRecordStatusNew() || x.RecordStatus.IsRecordStatusModified()))))
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistWorkHistory }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_Workhistory.ToId(), _messages[MessageType.Madatory_Param_Workhistory.ToId()].ToString()) }));
            }
        }

        private void IsValidActivetedProfile(TechnicalSpecialistDetail techSpecialistDetail, List<ValidationMessage> validationMessages)
        {
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.EmploymentType == null)
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.EmploymentType }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContact_EmploymentType.ToId(), _messages[MessageType.Madatory_Param_EmergencyContact_EmploymentType.ToId()].ToString()) }));
            }
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.StartDate == null)
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.StartDate }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_TechSpechStartDate.ToId(), _messages[MessageType.Madatory_Param_TechSpechStartDate.ToId()].ToString()) }));
            }
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.TaxReference == null)
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.TaxReference }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContact_TaxReference.ToId(), _messages[MessageType.Madatory_Param_EmergencyContact_TaxReference.ToId()].ToString()) }));
            }
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.PayrollReference == null)
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.PayrollReference }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContact_PayrollReference.ToId(), _messages[MessageType.Madatory_Param_EmergencyContact_PayrollReference.ToId()].ToString()) }));
            }
            if (techSpecialistDetail?.TechnicalSpecialistInfo?.CompanyPayrollName == null)
            {
                validationMessages.Add(new ValidationMessage(new { techSpecialistDetail.TechnicalSpecialistInfo.CompanyPayrollName }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContact_CompanyPayrollName.ToId(), _messages[MessageType.Madatory_Param_EmergencyContact_CompanyPayrollName.ToId()].ToString()) }));
            }

            if (techSpecialistDetail.TechnicalSpecialistContact?.Count > 0 && techSpecialistDetail.TechnicalSpecialistContact.Any(x => x.ContactType == ContactType.Emergency.ToString()))
            {
                techSpecialistDetail.TechnicalSpecialistContact?.ToList()
                                   .ForEach(x =>
                                   {
                                       if (x.ContactType == ContactType.Emergency.ToString() && string.IsNullOrEmpty(x.EmergencyContactName))
                                           validationMessages.Add(new ValidationMessage(new { x.EmergencyContactName }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContactName.ToId(), _messages[MessageType.Madatory_Param_EmergencyContactName.ToId()].ToString()) }));
                                       if (x.ContactType == ContactType.Emergency.ToString() && string.IsNullOrEmpty(x.TelephoneNumber))
                                           validationMessages.Add(new ValidationMessage(new { x.TelephoneNumber }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContact_MobileNumber.ToId(), _messages[MessageType.Madatory_Param_EmergencyContact_MobileNumber.ToId()].ToString()) }));
                                   });
            }
            else 
            {
                validationMessages.Add(new ValidationMessage(new { EmergencyContactName="" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContactName.ToId(), _messages[MessageType.Madatory_Param_EmergencyContactName.ToId()].ToString()) }));
                validationMessages.Add(new ValidationMessage(new {  TelephoneNumber = "" }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.Madatory_Param_EmergencyContact_MobileNumber.ToId(), _messages[MessageType.Madatory_Param_EmergencyContact_MobileNumber.ToId()].ToString()) }));
            }
        }

        private Response IsValidTsCertificationInfo(IList<TechnicalSpecialistCertification> tsCertifications, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsCertifications != null && tsCertifications.Any())
            {
                var delCert = tsCertifications?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modCert = tsCertifications?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addCert = tsCertifications?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delCert != null && delCert.Any())
                    response = _tsCertificationService.IsRecordValidForProcess(delCert, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsCertifications, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCertificationTypes, ref tsModuleRefDataCollection.DbVarifiedByUsers);
                if (modCert != null && modCert.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCertificationService.IsRecordValidForProcess(modCert, ValidationType.Update, ref tsModuleRefDataCollection.DbTsCertifications, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCertificationTypes, ref tsModuleRefDataCollection.DbVarifiedByUsers, isDraft);
                if (addCert != null && addCert.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCertificationService.IsRecordValidForProcess(addCert, ValidationType.Add, ref tsModuleRefDataCollection.DbTsCertifications, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCertificationTypes, ref tsModuleRefDataCollection.DbVarifiedByUsers);
            }

            return response;
        }

        private Response IsValidTSTrainingInfo(IList<TechnicalSpecialistTraining> tsTrainings, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsTrainings != null && tsTrainings.Any())
            {
                var delTrain = tsTrainings?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modTrain = tsTrainings?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addTrain = tsTrainings?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delTrain != null && delTrain.Any())
                    _tsTrainingService.IsRecordValidForProcess(delTrain, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsTrainings, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbTrainingTypes, ref tsModuleRefDataCollection.DbVarifiedByUsers);
                if (modTrain.Any())
                    response = _tsTrainingService.IsRecordValidForProcess(modTrain, ValidationType.Update, ref tsModuleRefDataCollection.DbTsTrainings, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbTrainingTypes, ref tsModuleRefDataCollection.DbVarifiedByUsers, isDraft);
                if (addTrain != null && addTrain.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsTrainingService.IsRecordValidForProcess(addTrain, ValidationType.Add, ref tsModuleRefDataCollection.DbTsTrainings, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbTrainingTypes, ref tsModuleRefDataCollection.DbVarifiedByUsers);
            }

            return response;
        }

        private Response IsValidTSPayScheduleInfo(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsPaySchedules != null && tsPaySchedules.Any())
            {
                var delPaySch = tsPaySchedules?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modPaySch = tsPaySchedules?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addPaySch = tsPaySchedules?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delPaySch != null && delPaySch.Any())
                    response = _tsPayScheduleService.IsRecordValidForProcess(delPaySch, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsPaySchedules, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCurrencies);
                if (modPaySch != null && modPaySch.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsPayScheduleService.IsRecordValidForProcess(modPaySch, ValidationType.Update, ref tsModuleRefDataCollection.DbTsPaySchedules, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCurrencies, isDraft);
                if (addPaySch != null && addPaySch.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsPayScheduleService.IsRecordValidForProcess(addPaySch, ValidationType.Add, ref tsModuleRefDataCollection.DbTsPaySchedules, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCurrencies);
            }

            return response;
        }

        private Response IsValidTSPayRateInfo(IList<TechnicalSpecialistPayRateInfo> tsPayRates, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsPayRates != null && tsPayRates.Any())
            {
                var delPayRate = tsPayRates?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modPayRate = tsPayRates?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addPayRate = tsPayRates?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delPayRate != null && delPayRate.Any())
                    response = _tsPayRateService.IsRecordValidForProcess(delPayRate, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsPayRates, ref tsModuleRefDataCollection.DbTsPaySchedules, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbExpenseTypes);
                if (modPayRate != null && modPayRate.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsPayRateService.IsRecordValidForProcess(modPayRate, ValidationType.Update, ref tsModuleRefDataCollection.DbTsPayRates, ref tsModuleRefDataCollection.DbTsPaySchedules, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbExpenseTypes, isDraft);
                if (addPayRate != null && addPayRate.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsPayRateService.IsRecordValidForProcess(addPayRate, ValidationType.Add, ref tsModuleRefDataCollection.DbTsPayRates, ref tsModuleRefDataCollection.DbTsPaySchedules, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbExpenseTypes);
            }

            return response;
        }

        private Response IsValidTSInternalTrainingInfo(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsInternalTrainings != null && tsInternalTrainings.Any())
            {
                var delIntTra = tsInternalTrainings?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modIntTra = tsInternalTrainings?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addIntTra = tsInternalTrainings?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delIntTra != null && delIntTra.Any())
                    response = _tsInternalTrainingService.IsRecordValidForProcess(delIntTra, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsInternalTrainings, ref tsModuleRefDataCollection.DbTechnicalSpecialists);
                if (modIntTra != null && modIntTra.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsInternalTrainingService.IsRecordValidForProcess(modIntTra, ValidationType.Update, ref tsModuleRefDataCollection.DbTsInternalTrainings, ref tsModuleRefDataCollection.DbTechnicalSpecialists, isDraft);
                if (addIntTra != null && addIntTra.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsInternalTrainingService.IsRecordValidForProcess(addIntTra, ValidationType.Add, ref tsModuleRefDataCollection.DbTsInternalTrainings, ref tsModuleRefDataCollection.DbTechnicalSpecialists);

            }

            return response;
        }
        //D661 issue 8 Start
        private Response IsValidTSNoteInfo(IList<TechnicalSpecialistNoteInfo> tsNotes, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsNotes != null && tsNotes.Any())
            {
                var delNotes = tsNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modNotes = tsNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addNots = tsNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (addNots != null && addNots.Any())
                    response = _tsNoteService.IsRecordValidForProcess(addNots, ValidationType.Add, ref tsModuleRefDataCollection.DbTsNotes);
                if (modNotes != null && modNotes.Any())
                    response = _tsNoteService.IsRecordValidForProcess(modNotes, ValidationType.Update, ref tsModuleRefDataCollection.DbTsNotes);

            }

            return response;
        }
        //D661 issue 8 End
        private Response IsValidTSCompetencyInfo(IList<TechnicalSpecialistCompetency> tsCompetencies, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsCompetencies != null && tsCompetencies.Any())
            {
                var delCompetency = tsCompetencies?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modCompetency = tsCompetencies?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addCompetency = tsCompetencies?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delCompetency != null && delCompetency.Any())
                    response = _tsCompetencyService.IsRecordValidForProcess(delCompetency, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsCompetencies, ref tsModuleRefDataCollection.DbTechnicalSpecialists);
                if (modCompetency != null && modCompetency.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCompetencyService.IsRecordValidForProcess(modCompetency, ValidationType.Update, ref tsModuleRefDataCollection.DbTsCompetencies, ref tsModuleRefDataCollection.DbTechnicalSpecialists, isDraft);
                if (addCompetency != null && addCompetency.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCompetencyService.IsRecordValidForProcess(addCompetency, ValidationType.Add, ref tsModuleRefDataCollection.DbTsCompetencies, ref tsModuleRefDataCollection.DbTechnicalSpecialists);

            }

            return response;
        }

        private Response IsValidTSCodeAndStandardInfo(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardinfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsCodeAndStandardinfos != null && tsCodeAndStandardinfos.Any())
            {
                var delCodeAndStan = tsCodeAndStandardinfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modCodeAndStan = tsCodeAndStandardinfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addCodeAndStan = tsCodeAndStandardinfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delCodeAndStan != null && delCodeAndStan.Any())
                    response = _tsCodeAndStandardService.IsRecordValidForProcess(delCodeAndStan, ValidationType.Delete, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCodeAndStandards, ref tsModuleRefDataCollection.DbTsCodeAndStandardInfos);
                if (modCodeAndStan != null && modCodeAndStan.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCodeAndStandardService.IsRecordValidForProcess(modCodeAndStan, ValidationType.Update, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCodeAndStandards, ref tsModuleRefDataCollection.DbTsCodeAndStandardInfos, isDraft);
                if (addCodeAndStan != null && addCodeAndStan.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCodeAndStandardService.IsRecordValidForProcess(addCodeAndStan, ValidationType.Add, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCodeAndStandards, ref tsModuleRefDataCollection.DbTsCodeAndStandardInfos);

            }

            return response;
        }

        private Response IsValidTSCommodityEquipmentKnowledgeInfo(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsCommodityEquipmentKnowledgeInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsCommodityEquipmentKnowledgeInfos != null && tsCommodityEquipmentKnowledgeInfos.Any())
            {
                var delComEquiKld = tsCommodityEquipmentKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modComEquiKld = tsCommodityEquipmentKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addComEquiKld = tsCommodityEquipmentKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delComEquiKld != null && delComEquiKld.Any())
                    response = _tsCommodityEquipmentKnowledgeService.IsRecordValidForProcess(delComEquiKld, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsComdEqipKnowledges, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCommodities, ref tsModuleRefDataCollection.DbEquipments);
                if (modComEquiKld != null && modComEquiKld.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCommodityEquipmentKnowledgeService.IsRecordValidForProcess(modComEquiKld, ValidationType.Update, ref tsModuleRefDataCollection.DbTsComdEqipKnowledges, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCommodities, ref tsModuleRefDataCollection.DbEquipments, isDraft);
                if (addComEquiKld != null && addComEquiKld.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCommodityEquipmentKnowledgeService.IsRecordValidForProcess(addComEquiKld, ValidationType.Add, ref tsModuleRefDataCollection.DbTsComdEqipKnowledges, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCommodities, ref tsModuleRefDataCollection.DbEquipments);

            }

            return response;
        }

        private Response IsValidTSComputerElectronicKnowledgeInfo(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsComputerElectronicKnowledgeInfos != null && tsComputerElectronicKnowledgeInfos.Any())
            {
                var delCompElecKld = tsComputerElectronicKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modCompElecKld = tsComputerElectronicKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addCompElecKld = tsComputerElectronicKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delCompElecKld != null && delCompElecKld.Any())
                    response = _tsComputerElectronicKnowledgeService.IsRecordValidForProcess(delCompElecKld, ValidationType.Delete, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbComputerElectronicsKnowledges, ref tsModuleRefDataCollection.DbTsCompElecKnowledges);
                if (modCompElecKld != null && modCompElecKld.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsComputerElectronicKnowledgeService.IsRecordValidForProcess(modCompElecKld, ValidationType.Update, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbComputerElectronicsKnowledges, ref tsModuleRefDataCollection.DbTsCompElecKnowledges, isDraft);
                if (addCompElecKld != null && addCompElecKld.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsComputerElectronicKnowledgeService.IsRecordValidForProcess(addCompElecKld, ValidationType.Add, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbComputerElectronicsKnowledges, ref tsModuleRefDataCollection.DbTsCompElecKnowledges);

            }

            return response;
        }

        private Response IsValidTSCustomerApprovalInfo(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsCustomerApprovalInfos != null && tsCustomerApprovalInfos.Any())
            {
                var delCustAppr = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modCustAppr = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addCustAppr = tsCustomerApprovalInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delCustAppr != null && delCustAppr.Any())
                    response = _tsCustomerApprovalService.IsRecordValidForProcess(delCustAppr, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsCustomerApprovalInfos, ref tsModuleRefDataCollection.DbTechSpecCustomers, ref tsModuleRefDataCollection.DbTechnicalSpecialists);
                if (modCustAppr != null && modCustAppr.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCustomerApprovalService.IsRecordValidForProcess(modCustAppr, ValidationType.Update, ref tsModuleRefDataCollection.DbTsCustomerApprovalInfos, ref tsModuleRefDataCollection.DbTechSpecCustomers, ref tsModuleRefDataCollection.DbTechnicalSpecialists, isDraft);
                if (addCustAppr != null && addCustAppr.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsCustomerApprovalService.IsRecordValidForProcess(addCustAppr, ValidationType.Add, ref tsModuleRefDataCollection.DbTsCustomerApprovalInfos, ref tsModuleRefDataCollection.DbTechSpecCustomers, ref tsModuleRefDataCollection.DbTechnicalSpecialists);

            }

            return response;
        }

        private Response IsValidTSLanguageCapabilityInfo(IList<TechnicalSpecialistLanguageCapabilityInfo> tsLanguageCapabilityInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsLanguageCapabilityInfos != null && tsLanguageCapabilityInfos.Any())
            {
                var delLanCap = tsLanguageCapabilityInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modLanCap = tsLanguageCapabilityInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addLanCap = tsLanguageCapabilityInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delLanCap != null && delLanCap.Any())
                    response = _tsLanguageCapabilityService.IsRecordValidForProcess(delLanCap, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsLanguageCapabilities, ref tsModuleRefDataCollection.DbLanguages, ref tsModuleRefDataCollection.DbTechnicalSpecialists);
                if (modLanCap != null && modLanCap.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsLanguageCapabilityService.IsRecordValidForProcess(modLanCap, ValidationType.Update, ref tsModuleRefDataCollection.DbTsLanguageCapabilities, ref tsModuleRefDataCollection.DbLanguages, ref tsModuleRefDataCollection.DbTechnicalSpecialists, isDraft);
                if (addLanCap != null && addLanCap.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsLanguageCapabilityService.IsRecordValidForProcess(addLanCap, ValidationType.Add, ref tsModuleRefDataCollection.DbTsLanguageCapabilities, ref tsModuleRefDataCollection.DbLanguages, ref tsModuleRefDataCollection.DbTechnicalSpecialists);

            }

            return response;
        }

        private Response IsValidTSStampInfo(IList<TechnicalSpecialistStampInfo> tsStampInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsStampInfos != null && tsStampInfos.Any())
            {
                var delStamp = tsStampInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modStamp = tsStampInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addStamp = tsStampInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delStamp != null && delStamp.Any())
                    response = _tsStampInfoService.IsRecordValidForProcess(delStamp, ValidationType.Delete, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbTsStampCountries, ref tsModuleRefDataCollection.DbTsStampInfos);
                if (modStamp != null && modStamp.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsStampInfoService.IsRecordValidForProcess(modStamp, ValidationType.Update, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbTsStampCountries, ref tsModuleRefDataCollection.DbTsStampInfos, isDraft);
                if (addStamp != null && addStamp.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsStampInfoService.IsRecordValidForProcess(addStamp, ValidationType.Add, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbTsStampCountries, ref tsModuleRefDataCollection.DbTsStampInfos);

            }

            return response;
        }

        private Response IsValidTSWorkHistoryInfo(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsWorkHistoryInfos != null && tsWorkHistoryInfos.Any())
            {
                var delsWorkHistory = tsWorkHistoryInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modsWorkHistory = tsWorkHistoryInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addsWorkHistory = tsWorkHistoryInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delsWorkHistory != null && delsWorkHistory.Any())
                    response = _tsWorkHistoryService.IsRecordValidForProcess(delsWorkHistory, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsWorkHistoryInfos, ref tsModuleRefDataCollection.DbTechnicalSpecialists);
                if (modsWorkHistory != null && modsWorkHistory.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsWorkHistoryService.IsRecordValidForProcess(modsWorkHistory, ValidationType.Update, ref tsModuleRefDataCollection.DbTsWorkHistoryInfos, ref tsModuleRefDataCollection.DbTechnicalSpecialists, isDraft);
                if (addsWorkHistory != null && addsWorkHistory.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsWorkHistoryService.IsRecordValidForProcess(addsWorkHistory, ValidationType.Add, ref tsModuleRefDataCollection.DbTsWorkHistoryInfos, ref tsModuleRefDataCollection.DbTechnicalSpecialists);

            }

            return response;
        }

        private Response IsValidTSTaxonomyInfo(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomyInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsTaxonomyInfos != null && tsTaxonomyInfos?.Count > 0)
            {
                var delTaxono = tsTaxonomyInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modTaxono = tsTaxonomyInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addTaxono = tsTaxonomyInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delTaxono != null && delTaxono.Any())
                    response = _tsTaxonomyService.IsRecordValidForProcess(delTaxono, ValidationType.Delete, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCategories, ref tsModuleRefDataCollection.DbSubCategories, ref tsModuleRefDataCollection.DbTaxonomyService, ref tsModuleRefDataCollection.DbTsTaxonomies);
                if (modTaxono != null && modTaxono.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsTaxonomyService.IsRecordValidForProcess(modTaxono, ValidationType.Update, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCategories, ref tsModuleRefDataCollection.DbSubCategories, ref tsModuleRefDataCollection.DbTaxonomyService, ref tsModuleRefDataCollection.DbTsTaxonomies, isDraft);
                if (addTaxono != null && addTaxono.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsTaxonomyService.IsRecordValidForProcess(addTaxono, ValidationType.Add, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCategories, ref tsModuleRefDataCollection.DbSubCategories, ref tsModuleRefDataCollection.DbTaxonomyService, ref tsModuleRefDataCollection.DbTsTaxonomies);

            }

            return response;
        }

        private Response IsValidTSEducationalQualificationInfo(IList<TechnicalSpecialistEducationalQualificationInfo> tsEducationalQualificationInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsEducationalQualificationInfos != null && tsEducationalQualificationInfos.Any())
            {
                var delEduQual = tsEducationalQualificationInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modEduQual = tsEducationalQualificationInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addEduQual = tsEducationalQualificationInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delEduQual != null && delEduQual.Any())
                    response = _tsEducationalQualificationService.IsRecordValidForProcess(delEduQual, ValidationType.Delete, ref tsModuleRefDataCollection.DbTsEducationQulifications, ref tsModuleRefDataCollection.DbCountries, ref tsModuleRefDataCollection.DbTechnicalSpecialists);
                if (modEduQual != null && modEduQual.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsEducationalQualificationService.IsRecordValidForProcess(modEduQual, ValidationType.Update, ref tsModuleRefDataCollection.DbTsEducationQulifications, ref tsModuleRefDataCollection.DbCountries, ref tsModuleRefDataCollection.DbTechnicalSpecialists, isDraft);
                if (addEduQual != null && addEduQual.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsEducationalQualificationService.IsRecordValidForProcess(addEduQual, ValidationType.Add, ref tsModuleRefDataCollection.DbTsEducationQulifications, ref tsModuleRefDataCollection.DbCountries, ref tsModuleRefDataCollection.DbTechnicalSpecialists);

            }

            return response;
        }

        private Response IsValidTSContactInfo(IList<TechnicalSpecialistContactInfo> tsContactInfos, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection, bool isDraft = false)
        {
            Response response = null;
            if (tsContactInfos != null && tsContactInfos.Any())
            {
                var delCont = tsContactInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var modCont = tsContactInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var addCont = tsContactInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (delCont != null && delCont?.Count > 0)
                    response = _tsContactService.IsRecordValidForProcess(delCont, ValidationType.Delete, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCountries, ref tsModuleRefDataCollection.DbCounties, ref tsModuleRefDataCollection.DbCities, ref tsModuleRefDataCollection.DbTsContacts);
                if (modCont != null && modCont.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsContactService.IsRecordValidForProcess(modCont, ValidationType.Update, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCountries, ref tsModuleRefDataCollection.DbCounties, ref tsModuleRefDataCollection.DbCities, ref tsModuleRefDataCollection.DbTsContacts, isDraft);
                if (addCont != null && addCont.Any() && (response == null || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response?.Result))))
                    response = _tsContactService.IsRecordValidForProcess(addCont, ValidationType.Add, ref tsModuleRefDataCollection.DbTechnicalSpecialists, ref tsModuleRefDataCollection.DbCountries, ref tsModuleRefDataCollection.DbCounties, ref tsModuleRefDataCollection.DbCities, ref tsModuleRefDataCollection.DbTsContacts);

            }

            return response;
        }

        private Response IsValidTSLoginCredential(TechnicalSpecialistInfo tsInfo, TechnicalSpecialistModuleRefDataCollection tsModuleRefDataCollection)
        {
            Response response = null;
            bool result = true;
            Exception exception = null;
            IList<DbModel.User> dbUser = null;
            IList<string> userNotExist = null;
            List<ValidationMessage> validationMessages = null;
            string logInName = string.Empty;
            try
            {
                if (tsInfo != null)
                {
                    validationMessages = new List<ValidationMessage>();

                    if (tsInfo.RecordStatus.IsRecordStatusNew())
                    {
                        logInName = tsInfo.LogonName;
                    }
                    else if (tsInfo.RecordStatus.IsRecordStatusModified())
                    {
                        logInName = tsModuleRefDataCollection.DbTechnicalSpecialists?.FirstOrDefault(x => x.Pin == tsInfo.Epin)?.LogInName; //Take old value
                    }

                    response = _userService.IsRecordExistInDb(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(_environment.SecurityAppName, logInName) }, ref dbUser, ref userNotExist);

                    if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                    {
                        if (tsInfo.RecordStatus.IsRecordStatusNew())
                        {
                            if (dbUser == null)
                            {
                                result = false;
                                validationMessages.Add(_messages, tsInfo, MessageType.TsLoginCredentialLogInNameNotExists, tsInfo.LogonName);
                            }
                            else
                            {
                                result = true;
                                tsModuleRefDataCollection.DbUsers = dbUser;
                                tsInfo.UserId = (int)dbUser?.FirstOrDefault(x => x.SamaccountName == tsInfo.LogonName)?.Id;
                            }
                            //if (dbUser != null)
                            //{
                            //    result = false;
                            //    validationMessages.Add(_messages, tsInfo, MessageType.TsLoginCredentialLogInNameAlreadyExists, tsInfo.LogonName);
                            //} //For New Profile Already Exisits Validation Checked for ProcessTsLogInCredentials function itself..
                        }
                        else if (tsInfo.RecordStatus.IsRecordStatusModified())
                        {
                            if (dbUser == null)
                            {
                                result = false;
                                validationMessages.Add(_messages, tsInfo, MessageType.TsLoginCredentialLogInNameNotExists, tsInfo.LogonName);
                            }
                            else
                            {   //UserName Duplicate Check Condition For Update -- D1281
                                if(!dbUser.Any(x => x.SamaccountName.ToLower() == tsInfo.LogonName.ToLower()))
                                {
                                    tsModuleRefDataCollection.DbUsers = dbUser;
                                    dbUser = null;
                                    response = _userService.IsRecordExistInDb(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(_environment.SecurityAppName, tsInfo.LogonName) }, ref dbUser, ref userNotExist);
                                    if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                                    {
                                        result = false;
                                        validationMessages.Add(_messages, tsInfo, MessageType.TsLoginCredentialLogInNameAlreadyExists, tsInfo.LogonName);
                                    } else if(response.Code == ResponseType.Validation.ToId() && response.ValidationMessages.Count == 1 && response.ValidationMessages.SelectMany(x => x.Messages).Any(y => y.Code == MessageType.UserNotExists.ToId()))
                                    {
                                        return new Response().ToPopulate(ResponseType.Success, null, null, null, result, null);
                                    } //While User Name update time, the Unique UserName won't be available in the DBUser, So Updating UserName we should allow the Validation ResponseCode.
                                    else
                                     return response; // Added to return the response when code is not success
                                }
                                //UserName Duplicate Check Condition For Update -- D1281
                                else
                                {
                                    result = true;
                                    tsModuleRefDataCollection.DbUsers = dbUser;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInfo);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, result, exception);
        }

        private IList<string> FetchCompanyUserType(string companyCode, string loginUserName)
        {
            try
            {
                if (!string.IsNullOrEmpty(companyCode) && !string.IsNullOrEmpty(loginUserName))
                {
                    var userTypes = _userTypeService.Get(companyCode, loginUserName).Result.Populate<IList<DbModel.UserType>>();

                    if (userTypes?.Count > 0)
                    {
                        return userTypes.Select(x => x.UserTypeName).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return null;
        }


        #endregion


        private void AppendEvent(TechnicalSpecialistDetail TSDetail,
                       long? eventId)
        {
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistCertification, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistCodeAndStandard, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistCommodityAndEquipment, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistCompetancy, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistComputerElectronicKnowledge, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistContact, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistCustomerApproval, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistDocuments, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistEducation, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistInternalTraining, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistLanguageCapabilities, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistNotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistPayRate, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistPaySchedule, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistStamp, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistTaxonomy, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistTraining, "EventId", eventId);
            ObjectExtension.SetPropertyValue(TSDetail.TechnicalSpecialistWorkHistory, "EventId", eventId);

        }

        #endregion
    }
}
