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
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistCertificationService : ITechnicalSpecialistCertificationService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistCertificationService> _logger = null;
        private readonly ITechnicalSpecialistCertificationAndTrainingRepository _tsCertificationAndTrainingRepository = null;
        private readonly ITechnicalSpecialistCertificationValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ICertificationsService _certificationsService = null;
        private readonly IUserService _userService = null;
        private readonly IDocumentService _documentService = null;
        private readonly ITechnicalSpecialistNoteService _tsNoteService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IUserTypeService _userTypeService = null;
        private readonly IEmailQueueService _emailService = null;

        #region Constructor

        public TechnicalSpecialistCertificationService(IMapper mapper,
                                                        JObject messages,
                                                        IAppLogger<TechnicalSpecialistCertificationService> logger,
                                                        ITechnicalSpecialistCertificationAndTrainingRepository tsCertificationAndTrainingRepository,
                                                        ITechnicalSpecialistCertificationValidationService validationService,
                                                        //ITechnicalSpecialistService technSpecServices,
                                                        ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                        ICertificationsService certificationsService,
                                                        IUserService userService,
                                                        IAuditSearchService auditSearchService,
                                                        IDocumentService documentService,
                                                        ITechnicalSpecialistNoteService tsNoteService,
                                                        IUserTypeService userTypeService,
                                                        IEmailQueueService emailService,
                                                        IOptions<AppEnvVariableBaseModel> environment)
        {
            _mapper = mapper;
            _messages = messages;
            _logger = logger;
            _tsCertificationAndTrainingRepository = tsCertificationAndTrainingRepository;
            _validationService = validationService;
            // _technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _certificationsService = certificationsService;
            _userService = userService;
            _documentService = documentService;
            _auditSearchService = auditSearchService;
            _tsNoteService = tsNoteService;
            _environment = environment.Value;
            _userTypeService = userTypeService;
            _emailService = emailService;

        }

        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistCertification searchModel)
        {
            IList<TechnicalSpecialistCertification> result = null;
            Exception exception = null;
            try
            {
                var tsCertResult = _mapper.Map<IList<TechnicalSpecialistCertification>>(_tsCertificationAndTrainingRepository.Search(searchModel));
                var tsCertDocResult = PopulateTsCertificateDocuments(tsCertResult);
                result = PopulateTsCertificateNotes(tsCertDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> tsCertificationIds)
        {
            IList<TechnicalSpecialistCertification> result = null;
            Exception exception = null;
            try
            {
                var tsCertResult = _mapper.Map<IList<TechnicalSpecialistCertification>>(GetTsCertificationById(tsCertificationIds));
                var tsCertDocResult = PopulateTsCertificateDocuments(tsCertResult);
                result = PopulateTsCertificateNotes(tsCertDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertificationIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> CertificationName)
        {
            IList<TechnicalSpecialistCertification> result = null;
            Exception exception = null;
            try
            {
                var tsCertResult = _mapper.Map<IList<TechnicalSpecialistCertification>>(GetTsCertificationByCertificateNames(CertificationName));
                var tsCertDocResult = PopulateTsCertificateDocuments(tsCertResult);
                result = PopulateTsCertificateNotes(tsCertDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), CertificationName);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<TechnicalSpecialistCertification> result = null;
            Exception exception = null;
            try
            {
                var tsCertResult = _mapper.Map<IList<TechnicalSpecialistCertification>>(GetTsCertificationByPin(tsPins));
                var tsCertDocResult = PopulateTsCertificateDocuments(tsCertResult);
                result = PopulateTsCertificateNotes(tsCertDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Add 

        public Response Add(IList<TechnicalSpecialistCertification> tsCertifications, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCertificationTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            return AddTechSpecialistCertification(tsCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistCertification> tsCertifications, ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Data> dbCertificationTypes, ref IList<DbModel.User> dbVarifiedByUsers, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialistCertification(tsCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete 

        public Response Delete(IList<TechnicalSpecialistCertification> tsCertifications, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications = null;
            return RemoveTechSpecialistCertification(tsCertifications, ref dbTsCertifications, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistCertification> tsCertifications, ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistCertification(tsCertifications, ref dbTsCertifications, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Modify 

        public Response Modify(IList<TechnicalSpecialistCertification> tsCertifications, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCertificationTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            return UpdateTechSpecialistCertification(tsCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistCertification> tsCertifications, ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Data> dbCertificationTypes, ref IList<DbModel.User> dbVarifiedByUsers, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistCertification(tsCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Validation 

        public Response IsRecordExistInDb(IList<int> tsCertificationIds,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsCertificationIdNotExists = null;
            return IsRecordExistInDb(tsCertificationIds, ref dbTsCertifications, ref tsCertificationIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsCertificationIds,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                            ref IList<int> tsCertificationIdNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsCertifications == null && tsCertificationIds != null && tsCertificationIds.Any())
                    dbTsCertifications = GetTsCertificationById(tsCertificationIds);

                result = IsTsCertificationExistInDb(tsCertificationIds, dbTsCertifications, ref tsCertificationIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertificationIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications = null;
            IList<TechnicalSpecialistCertification> filteredTSCertifications = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCertificationTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;

            return IsRecordValidForProcess(tsCertifications, validationType, ref filteredTSCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                                ValidationType validationType,
                                                ref IList<TechnicalSpecialistCertification> filteredTSCertifications,
                                                ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCertificationTypes,
                                                ref IList<DbModel.User> dbVarifiedByUsers)
        {
            return CheckRecordValidForProcess(tsCertifications, validationType, ref filteredTSCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                        ValidationType validationType,
                                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Data> dbCertificationTypes,
                                        ref IList<DbModel.User> dbVarifiedByUsers, bool isDraft = false)
        {
            IList<TechnicalSpecialistCertification> filteredTSCertifications = null;
            return CheckRecordValidForProcess(tsCertifications, validationType, ref filteredTSCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications)
        {
            IList<TechnicalSpecialistCertification> filteredTSCertifications = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCertificationTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            return IsRecordValidForProcess(tsCertifications, validationType, ref filteredTSCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers);
        }

        #endregion

        #endregion

        #region Private Methods 

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetTsCertificationByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertificates = null;
            if (pins?.Count > 0)
            {
                dbTsCertificates = _tsCertificationAndTrainingRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsCertificates;
        }

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetTsCertificationById(IList<int> tsCertificationIds)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications = null;
            if (tsCertificationIds?.Count > 0)
                dbTsCertifications = _tsCertificationAndTrainingRepository.FindBy(x => tsCertificationIds.Contains(x.Id)).ToList();

            return dbTsCertifications;
        }

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetTsCertificationByCertificateNames(IList<string> tsCertificateNames)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications = null;
            if (tsCertificateNames?.Count > 0)
                dbTsCertifications = _tsCertificationAndTrainingRepository.FindBy(x => tsCertificateNames.Contains(x.CertificationAndTraining.Name)).ToList();

            return dbTsCertifications;
        }

        private Response AddTechSpecialistCertification(IList<TechnicalSpecialistCertification> tsCertifications,
                                           ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.Data> dbCertificationTypes,
                                           ref IList<DbModel.User> dbVarifiedByUsers,
                                           bool commitChange = true,
                                           bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistCertification> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                IList<DbModel.Data> dbMasterCertificationTypes = null;
                IList<DbModel.User> dbCertVarifiedByUser = null;
                eventId = tsCertifications.Select(x => x.EventId).FirstOrDefault();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsCertifications, ValidationType.Add, ref recordToBeAdd, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers);
                }

                if (!isDbValidationRequired && tsCertifications?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsCertifications, ValidationType.Add);
                }

                if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbMasterCertificationTypes = dbCertificationTypes;
                    dbCertVarifiedByUser = dbVarifiedByUsers;

                    _tsCertificationAndTrainingRepository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistCertificationAndTraining>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                        opt.Items["DbCertificationTypes"] = dbMasterCertificationTypes;
                        opt.Items["DbVarifiedByUser"] = dbCertVarifiedByUser;
                    });
                    _tsCertificationAndTrainingRepository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _tsCertificationAndTrainingRepository.ForceSave();
                        dbTsCertifications = mappedRecords;
                        if (savedCnt > 0)
                        {
                            ProcessTsCertificateNotes(recordToBeAdd, mappedRecords, ValidationType.Add, ref validationMessages);
                            ProcessTsCertificateDocuments(recordToBeAdd, mappedRecords, ref validationMessages);

                            //mappedRecords.Select(x => x.TechnicalSpecialistId).Distinct().ToList().ForEach(x =>
                            //{
                            //    ProcessEmailNotifications(dbTechSpecialists.FirstOrDefault(x1 => x1.Id == x), EmailTemplate.ClientApprovalCertification);
                            //});
                        }
                        if (mappedRecords?.Count > 0)
                        {
                            int i = 0;
                            recordToBeAdd?.ToList().ForEach(x1 =>
                            { 
                                var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                var newVerificationDocuments = x1?.VerificationDocuments?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                if (newDocuments != null && newDocuments.Count > 0)
                                    x1.DocumentName = string.Join(",", newDocuments);

                                if (newVerificationDocuments != null && newVerificationDocuments?.Count > 0)
                                    x1.VerificationDocumentName = string.Join(",", newVerificationDocuments);

                                x1.Id = mappedRecords[i++].Id;// def1035 test 2
                                _auditSearchService.AuditLog(x1, ref eventId, tsCertifications?.FirstOrDefault()?.ActionByUser,
                                                          null,
                                                           ValidationType.Add.ToAuditActionType(),
                                                           SqlAuditModuleType.TechnicalSpecialistCertificationAndTraining,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }
            finally
            {
                _tsCertificationAndTrainingRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistCertification(IList<TechnicalSpecialistCertification> tsCertifications,
                                           ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.Data> dbCertificationTypes,
                                           ref IList<DbModel.User> dbVarifiedByUsers,
                                           bool commitChange = true,
                                           bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbMasterCertificationTypes = null;
            IList<DbModel.User> dbCertVarifiedByUsers = null;
            long? eventId = 0;
            IList<TechnicalSpecialistCertification> recordToBeModify = null;
            IList<TechnicalSpecialistCertification> domExistingTechCertification = null;
            Response valdResponse = null;
            bool valdResult = true;

            try
            {
                validationMessages = validationMessages ?? new List<ValidationMessage>();
                eventId = tsCertifications?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsCertifications, ValidationType.Update, ref recordToBeModify, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers);
                }

                if (!isDbValidationRequired && tsCertifications?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsCertifications, ValidationType.Update);
                }

                if (recordToBeModify != null && recordToBeModify.Any())
                {
                    if (dbTsCertifications == null || (dbTsCertifications?.Count <= 0 && !valdResult))
                    {
                        dbTsCertifications = _tsCertificationAndTrainingRepository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult))
                    {
                        //valdResponse = _technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                        valdResponse = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if (dbVarifiedByUsers == null || (dbVarifiedByUsers?.Count <= 0 && valdResponse == null))
                    {
                        IList<string> userNotExists = null;
                        valdResponse = _userService.IsRecordExistInDb(recordToBeModify.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList(), ref dbVarifiedByUsers, ref userNotExists);
                        if (!Convert.ToBoolean(valdResponse.Result))
                        {
                            validationMessages.AddRange(valdResponse.ValidationMessages);
                        }
                    }
                    if (dbCertificationTypes == null || (dbCertificationTypes?.Count <= 0 && valdResponse == null))
                    {
                        valdResult = _certificationsService.IsValidCertification(recordToBeModify.Select(x => x.CertificationName).ToList(), ref dbCertificationTypes, ref validationMessages);
                    }
                }

                if ((valdResponse == null || Convert.ToBoolean(valdResponse?.Result)) && valdResult && dbTsCertifications?.Count > 0)
                {
                    domExistingTechCertification = new List<TechnicalSpecialistCertification>();
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbMasterCertificationTypes = dbCertificationTypes;
                    dbCertVarifiedByUsers = dbVarifiedByUsers;

                    TechnicalSpecialistCertification technicalSpecialistCertification = new TechnicalSpecialistCertification();
                    dbTsCertifications.ToList().ForEach(tsCertInfo =>
                    {
                        technicalSpecialistCertification = _mapper.Map<TechnicalSpecialistCertification>(tsCertInfo);
                        var oldDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsCertInfo.Id)?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted()).Select(doc => doc.DocumentName).ToList();
                        var oldVerificationDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsCertInfo.Id)?.VerificationDocuments?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted()).Select(doc => doc.DocumentName).ToList();
                        if (oldDocuments != null && oldDocuments.Count > 0)
                            technicalSpecialistCertification.DocumentName = string.Join(",", oldDocuments);
                        if (oldVerificationDocuments != null && oldVerificationDocuments.Count > 0)
                            technicalSpecialistCertification.VerificationDocumentName = string.Join(",", oldVerificationDocuments);

                        domExistingTechCertification.Add(ObjectExtension.Clone(technicalSpecialistCertification));

                        var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsCertInfo.Id);
                        if (tsToBeModify != null)
                        {
                            _mapper.Map(tsToBeModify, tsCertInfo, opt =>
                            {
                                opt.Items["isAssignId"] = true;
                                opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                opt.Items["DbCertificationTypes"] = dbMasterCertificationTypes;
                                opt.Items["DbVarifiedByUser"] = dbCertVarifiedByUsers;
                            });
                            tsCertInfo.LastModification = DateTime.UtcNow;
                            tsCertInfo.UpdateCount = tsCertInfo.UpdateCount.CalculateUpdateCount();
                            tsCertInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                        }
                    });
                    _tsCertificationAndTrainingRepository.AutoSave = false;
                    _tsCertificationAndTrainingRepository.Update(dbTsCertifications);
                    if (commitChange)
                    {
                        var savedCnt = _tsCertificationAndTrainingRepository.ForceSave();
                        if (savedCnt > 0)
                        {
                            ProcessTsCertificateNotes(recordToBeModify, dbTsCertifications, ValidationType.Update, ref validationMessages);
                            ProcessTsCertificateDocuments(recordToBeModify, dbTsCertifications, ref validationMessages);
                            if (recordToBeModify?.Count > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 =>
                                {
                                    var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                    var newVerificationDocuments = x1?.VerificationDocuments?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                    if (newDocuments != null && newDocuments.Count > 0)
                                        x1.DocumentName = string.Join(",", newDocuments);
                                    if (newVerificationDocuments != null && newVerificationDocuments?.Count > 0)
                                        x1.VerificationDocumentName = string.Join(",", newVerificationDocuments);
                                    _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                          null,
                                                          ValidationType.Update.ToAuditActionType(),
                                                          SqlAuditModuleType.TechnicalSpecialistCertificationAndTraining,
                                                           domExistingTechCertification?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }
            finally
            {
                _tsCertificationAndTrainingRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialistCertification(IList<TechnicalSpecialistCertification> tsCertifications,
            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                                        bool commitChange = true,
                                                        bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecilist = null;
            IList<DbModel.Data> dbCertificationTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            long? eventId = 0;

            IList<TechnicalSpecialistCertification> recordToBeDeleted = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsCertifications?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(tsCertifications, ValidationType.Delete, ref dbTsCertifications, ref dbTechnicalSpecilist, ref dbCertificationTypes, ref dbVarifiedByUsers);

                if (tsCertifications?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsCertifications, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsCertifications?.Count > 0)
                {
                    var dbTsCertificToBeDeleted = dbTsCertifications?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _tsCertificationAndTrainingRepository.AutoSave = false;
                    _tsCertificationAndTrainingRepository.Delete(dbTsCertificToBeDeleted);
                    if (commitChange)
                    {
                        _tsCertificationAndTrainingRepository.ForceSave();

                        if (recordToBeDeleted.Count > 0)
                            tsCertifications?.ToList().ForEach(x1 =>
                            {
                                _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                           null,
                                                           ValidationType.Delete.ToAuditActionType(),
                                                           SqlAuditModuleType.TechnicalSpecialistCertificationAndTraining,
                                                           x1,
                                                           null
                                                           );
                            });
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }
            finally
            {
                _tsCertificationAndTrainingRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistCertification> filteredTsCertifications,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCertificationTypes,
                                            ref IList<DbModel.User> dbVarifiedByUsers,
                                            bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsCertifications, ref filteredTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsCertifications, ref filteredTsCertifications, ref dbTsCertifications, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsCertifications, ref filteredTsCertifications, ref dbTsCertifications, ref dbTechnicalSpecialists, ref dbCertificationTypes, ref dbVarifiedByUsers, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsTsCertificationExistInDb(IList<int> tsCertificationIds,
                                        IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                        ref IList<int> tsCertificationIdNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {

            validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsCertifications = dbTsCertifications ?? new List<DbModel.TechnicalSpecialistCertificationAndTraining>();

            var validMessages = validationMessages;

            if (tsCertificationIds != null && tsCertificationIds.Any())
            {
                tsCertificationIdNotExists = tsCertificationIds.Where(id => !dbTsCertifications.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsCertificationIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCertificationIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private IList<TechnicalSpecialistCertification> FilterRecord(IList<TechnicalSpecialistCertification> tsCertifications, ValidationType filterType)
        {
            IList<TechnicalSpecialistCertification> filterTsCertifications = null;

            if (filterType == ValidationType.Add)
                filterTsCertifications = tsCertifications?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsCertifications = tsCertifications?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsCertifications = tsCertifications?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsCertifications;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistCertification> tsCertifications,
                              ValidationType validationType,
                              ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsCertifications), validationType);
            foreach (var item in tsCertifications)
            {
                if (item.EffeciveDate != null && item.ExpiryDate != null)
                    if (item.ExpiryDate < item.EffeciveDate)
                        messages.Add(_messages, item.EffeciveDate, MessageType.TsInternalTrainingExpiryDate, item.ExpiryDate);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistCertification> tsCertifications,
                                     ref IList<TechnicalSpecialistCertification> filteredTsCertifications,
                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                     ref IList<DbModel.Data> dbCertificationTypes,
                                     ref IList<DbModel.User> dbVarifiedByUsers,
                                     ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCertifications != null && tsCertifications.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsCertifications == null || filteredTsCertifications.Count <= 0)
                    filteredTsCertifications = FilterRecord(tsCertifications, validationType);

                if (filteredTsCertifications?.Count > 0 && IsValidPayload(filteredTsCertifications, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsCertifications.Select(x => x.Epin.ToString()).ToList();
                    IList<string> certificationNames = filteredTsCertifications.Select(x => x.CertificationName).ToList();

                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);

                    if (result && certificationNames?.Count > 0)
                        result = _certificationsService.IsValidCertification(certificationNames, ref dbCertificationTypes, ref validationMessages);
                    if (result)
                    {
                        IList<string> userNotExists = null;
                        var valResponse = _userService.IsRecordExistInDb(filteredTsCertifications.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList(), ref dbVarifiedByUsers, ref userNotExists);
                        result = Convert.ToBoolean(valResponse.Result);
                        if (!result)
                        {
                            validationMessages.AddRange(valResponse.ValidationMessages);
                        }
                    }

                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistCertification> tsCertifications,
                                            ref IList<TechnicalSpecialistCertification> filteredTsCertifications,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCertificationTypes,
                                            ref IList<DbModel.User> dbVarifiedByUsers,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {
            bool result = false;
            if (tsCertifications != null && tsCertifications.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsCertifications == null || filteredTsCertifications.Count <= 0)
                    filteredTsCertifications = FilterRecord(tsCertifications, validationType);

                if (filteredTsCertifications?.Count > 0 && IsValidPayload(filteredTsCertifications, validationType, ref messages))
                {
                    GetTsCertificationDbInfo(filteredTsCertifications, ref dbTsCertifications);
                    IList<int> tsCertificationIds = filteredTsCertifications.Select(x => x.Id).ToList();
                    IList<int> tsDBCertificationIds = dbTsCertifications.Select(x => x.Id).ToList();
                    if (tsCertificationIds.Any(x => !tsDBCertificationIds.Contains(x))) //Invalid TechSpecialist Certification Id found.
                    {
                        var dbTsCertificateInfosByIds = dbTsCertifications;
                        var idNotExists = tsCertificationIds.Where(id => !dbTsCertificateInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistCertificateList = filteredTsCertifications;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsCertificate = techSpecialistCertificateList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsCertificate, MessageType.TsCertificationUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsCertifications, dbTsCertifications, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsCertifications.Select(x => x.Epin.ToString()).ToList();
                            IList<string> certificationNames = filteredTsCertifications.Select(x => x.CertificationName).ToList();

                            //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            if (result && certificationNames?.Count > 0)
                                result = _certificationsService.IsValidCertification(certificationNames, ref dbCertificationTypes, ref validationMessages);
                            if (result)
                            {
                                IList<string> userNotExists = null;
                                var valResponse = _userService.IsRecordExistInDb(filteredTsCertifications.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList(), ref dbVarifiedByUsers, ref userNotExists);
                                result = Convert.ToBoolean(valResponse.Result);
                                if (!result)
                                {
                                    validationMessages.AddRange(valResponse.ValidationMessages);
                                }
                            }
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }


        private bool IsRecordValidForRemove(IList<TechnicalSpecialistCertification> tsCertifications,
                                            ref IList<TechnicalSpecialistCertification> filteredTsCertifications,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCertifications != null && tsCertifications.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsCertifications == null || filteredTsCertifications.Count <= 0)
                    filteredTsCertifications = FilterRecord(tsCertifications, validationType);

                if (filteredTsCertifications?.Count > 0 && IsValidPayload(filteredTsCertifications, validationType, ref validationMessages))
                {
                    GetTsCertificationDbInfo(filteredTsCertifications, ref dbTsCertifications);
                    IList<int> tsCertificationIdNotExists = null;
                    var tsCertificationIds = filteredTsCertifications.Select(x => x.Id).Distinct().ToList();
                    result = IsTsCertificationExistInDb(tsCertificationIds, dbTsCertifications, ref tsCertificationIdNotExists, ref validationMessages);
                    if (result)
                        result = IsTechSpecialistCertificationCanBeRemove(dbTsCertifications, ref validationMessages);



                }
            }
            return result;
        }

        private void GetTsCertificationDbInfo(IList<TechnicalSpecialistCertification> filteredTsCertifications,
                                    ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications)
        {
            dbTsCertifications = dbTsCertifications ?? new List<DbModel.TechnicalSpecialistCertificationAndTraining>();
            IList<int> tsCertificationIds = filteredTsCertifications?.Select(x => x.Id).Distinct().ToList();
            if (tsCertificationIds?.Count > 0 && (dbTsCertifications.Count <= 0 || dbTsCertifications.Any(x => !tsCertificationIds.Contains(x.Id))))
            {
                var tsCertifs = GetTsCertificationById(tsCertificationIds);
                if (tsCertifs != null && tsCertifs.Any())
                {
                    dbTsCertifications.AddRange(tsCertifs.ToList());
                }
            }

        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistCertification> tsCertifications,
                                        IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                        ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = tsCertifications.Where(x => !dbTsCertifications.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsCertificationUpdatedByOther, x.CertificationName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTechSpecialistCertificationCanBeRemove(IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbTsCertifications?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x, MessageType.TsCertificationIsBeingUsed, x.CertificationAndTraining.Name);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private IList<TechnicalSpecialistCertification> PopulateTsCertificateDocuments(IList<TechnicalSpecialistCertification> tsCertifications)
        {
            try
            {
                if (tsCertifications?.Count > 0)
                {
                    var epins = tsCertifications.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var certificationIds = tsCertifications.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsCertificationDocs = _documentService.Get(ModuleCodeType.TS, epins, certificationIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsCertificationDocs?.Count > 0)
                    {
                        return tsCertifications.GroupJoin(tsCertificationDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsCert = tsc, doc }).Select(x =>
                            {
                                x.tsCert.Documents = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_Certificate.ToString()).ToList();
                                x.tsCert.VerificationDocuments = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_CertVerification.ToString()).ToList();
                                return x.tsCert;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }

            return tsCertifications;
        }

        private Response ProcessTsCertificateDocuments(IList<TechnicalSpecialistCertification> tsCertifications, IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            List<ModuleDocument> tsDocToBeProcess = new List<ModuleDocument>();
            try
            {
                if (tsCertifications?.Count > 0)
                {

                    tsCertifications = tsCertifications.Join(dbTsCertifications,
                        tsc => tsc.CertificationName,
                        dbtsc => dbtsc.CertificationAndTraining.Name,
                        (tsc, dbtsc) => new { tsCert = tsc, dbtsc }).Select(x =>
                        {
                            x.tsCert?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = x.tsCert?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = x.dbtsc.Id.ToString();
                                x1.ModuleRefCode = x?.tsCert?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_Certificate.ToString();
                            });

                            x.tsCert?.VerificationDocuments?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = x.tsCert?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = x.dbtsc.Id.ToString();
                                x1.ModuleRefCode = x?.tsCert?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_CertVerification.ToString();
                            });
                            return x.tsCert;
                        }).ToList();

                    tsDocToBeProcess = tsCertifications?.Where(x => x.Documents != null &&
                                                                         x.Documents.Any(x1 => x1.RecordStatus != null))
                                                             .SelectMany(x => x.Documents)
                                                             .ToList();

                    var tsCerVarifDocs = tsCertifications?.Where(x => x.VerificationDocuments != null &&
                                                                          x.VerificationDocuments.Any(x1 => x1.RecordStatus != null))
                                                              .SelectMany(x => x.VerificationDocuments)
                                                              .ToList();
                    if (tsCerVarifDocs != null && tsCerVarifDocs.Count > 0)
                    {
                        tsDocToBeProcess.AddRange(tsCerVarifDocs);
                    }

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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistCertification> PopulateTsCertificateNotes(IList<TechnicalSpecialistCertification> tsCertifications)
        {
            try
            {
                if (tsCertifications?.Count > 0)
                {
                    var epins = tsCertifications.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var certificationIds = tsCertifications.Select(x => x.Id).Distinct().ToList();
                    var tsCertificationNotes = _tsNoteService.Get(NoteType.CED, true, epins, certificationIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                    if (tsCertificationNotes?.Count > 0)
                    {
                        return tsCertifications.GroupJoin(tsCertificationNotes,
                            tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                            note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                            (tsc, note) => new { tsCert = tsc, note }).Select(x =>
                            {
                                x.tsCert.Notes = x?.note?.FirstOrDefault()?.Note;
                                return x.tsCert;
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }

            return tsCertifications;
        }

        private Response ProcessTsCertificateNotes(IList<TechnicalSpecialistCertification> tsCertifications, IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistNoteInfo> tsNewCertNotes = null;
            try
            {
                if (tsCertifications?.Count > 0)
                {
                    if (validationType == ValidationType.Add)
                    {
                        tsNewCertNotes = tsCertifications.Join(dbTsCertifications,
                             tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.CertificationName },
                             dbtsc => new { epin = dbtsc.TechnicalSpecialistId.ToString(), RefId = dbtsc.CertificationAndTraining.Name },
                             (tsc, dbtsc) => new { tsc, dbtsc }).Select(x =>
                             new TechnicalSpecialistNoteInfo
                             {
                                 Epin = x.tsc.Epin,
                                 RecordType = NoteType.CED.ToString(),
                                 RecordRefId = x.dbtsc.Id,
                                 Note = x.tsc.Notes,
                                 RecordStatus = RecordStatus.New.FirstChar(),
                                 CreatedBy = x.tsc.ActionByUser,
                                 CreatedDate = DateTime.UtcNow,
                                 ActionByUser = x.tsc.ActionByUser,
                                 EventId = x.tsc.EventId
                             }).ToList();

                    }
                    else if (validationType == ValidationType.Update)
                    {
                        var epins = tsCertifications.Select(x => x.Epin.ToString()).Distinct().ToList();
                        var certificationIds = tsCertifications.Select(x => x.Id).Distinct().ToList();
                        var tsCertificationNotes = _tsNoteService.Get(NoteType.CED, true, epins, certificationIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                        tsNewCertNotes = tsCertifications.GroupJoin(tsCertificationNotes,
                                tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                             note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                             (tsc, note) => new { note, tsCert = tsc }).Where(x => !string.Equals(x.note?.FirstOrDefault()?.Note, x.tsCert.Notes)).Select(x =>
                              new TechnicalSpecialistNoteInfo
                              {
                                  Note = x.tsCert.Notes,
                                  RecordStatus = RecordStatus.New.FirstChar(),
                                  CreatedBy = x.tsCert.ActionByUser,
                                  CreatedDate = DateTime.UtcNow,
                                  Epin = x.tsCert.Epin,
                                  RecordType = NoteType.CED.ToString(),
                                  RecordRefId = x.tsCert.Id,
                                  EventId = x.tsCert.EventId,
                                  ActionByUser = x.tsCert.ActionByUser,
                              }).ToList();

                    }
                    return _tsNoteService.Add(tsNewCertNotes);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCertifications);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        //private Response ProcessEmailNotifications(DbModel.TechnicalSpecialist technicalSpecialistInfo, EmailTemplate emailTemplateType, List<KeyValuePair<string, string>> emailContentPlaceholders = null)
        //{
        //    string emailSubject = string.Empty;
        //    Exception exception = null;
        //    EmailQueueMessage emailMessage = null;
        //    List<EmailAddress> toAddresses = null;
        //    List<EmailAddress> ccAddresses = null;
        //    try
        //    {
        //        emailContentPlaceholders = emailContentPlaceholders ?? new List<KeyValuePair<string, string>>();
        //        if (technicalSpecialistInfo != null)
        //        {
        //            switch (emailTemplateType)
        //            {
        //                case EmailTemplate.ClientApprovalCertification:
        //                    List<string> userTypes = new List<string> { TechnicalSpecialistConstants.User_Type_RC, TechnicalSpecialistConstants.User_Type_RM, TechnicalSpecialistConstants.User_Type_TM };
        //                    var userInfos = _userTypeService.Get(technicalSpecialistInfo.CompanyId, userTypes, new string[] { "User" }).Result.Populate<IList<DbModel.UserType>>();
        //                    var RcUsers = userInfos.Where(x => x.UserTypeName == ResourceSearchConstants.User_Type_RC).Select(x => x.User);
        //                    var RMTMUsers = userInfos.Where(x => x.UserTypeName == ResourceSearchConstants.User_Type_RM || x.UserTypeName == TechnicalSpecialistConstants.User_Type_TM).Select(x => x.User);
        //                    emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_ClientApprovalCertification_Subject, technicalSpecialistInfo?.FirstName, technicalSpecialistInfo?.LastName, technicalSpecialistInfo?.Pin.ToString());
        //                    toAddresses = RcUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
        //                    ccAddresses = RMTMUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
        //                    emailContentPlaceholders.AddRange(new List<KeyValuePair<string, string>> {
        //                            new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, technicalSpecialistInfo?.FirstName),
        //                            new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, technicalSpecialistInfo?.LastName),
        //                            new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, technicalSpecialistInfo?.Pin.ToString())
        //                        });
        //                    emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.Notification, ModuleCodeType.TS, technicalSpecialistInfo?.Pin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses);
        //                    break;

        //            }

        //            return _emailService.Add(new List<EmailQueueMessage> { emailMessage });

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        //}


        private Response IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
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

                result = validationMessages.Count <= 0;
            }

            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPins);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

    }
}
