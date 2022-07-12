using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
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
    public class TechnicalSpecialistTrainingService : ITechnicalSpecialistTrainingService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistTrainingService> _logger = null;
        private readonly ITechnicalSpecialistCertificationAndTrainingRepository _tsCertificationAndTrainingRepository = null;
        private readonly ITechnicalSpecialistTrainingValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ITrainingsService _trainingService = null;
        private readonly IUserService _userService = null;
        private readonly IDocumentService _documentService = null;
        private readonly ITechnicalSpecialistNoteService _tsNoteService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor

        public TechnicalSpecialistTrainingService(IMapper mapper,
                                                        JObject messages,
                                                        IAppLogger<TechnicalSpecialistTrainingService> logger,
                                                        ITechnicalSpecialistCertificationAndTrainingRepository tsCertificationAndTrainingRepository,
                                                        ITechnicalSpecialistTrainingValidationService validationService,
                                                        //ITechnicalSpecialistService technSpecServices,
                                                        ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                        ITrainingsService trainingService,
                                                        IUserService userService,
                                                        IDocumentService documentService,
                                                        ITechnicalSpecialistNoteService tsNoteService,
                                                        IOptions<AppEnvVariableBaseModel> environment,
                                                        IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _messages = messages;
            _logger = logger;
            _tsCertificationAndTrainingRepository = tsCertificationAndTrainingRepository;
            _validationService = validationService;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _trainingService = trainingService;
            _userService = userService;
            _documentService = documentService;
            _tsNoteService = tsNoteService;
            _environment = environment.Value;
            _auditSearchService = auditSearchService;
        }

        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistTraining searchModel)
        {
            IList<TechnicalSpecialistTraining> result = null;
            Exception exception = null;
            try
            {
                var tsTrainingResult = _mapper.Map<IList<TechnicalSpecialistTraining>>(_tsCertificationAndTrainingRepository.Search(searchModel));
                var tsTrainingDocResult = PopulateTsTrainingDocuments(tsTrainingResult);
                result = PopulateTsTrainingNotes(tsTrainingDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> tsTrainingIds)
        {
            IList<TechnicalSpecialistTraining> result = null;
            Exception exception = null;
            try
            {
                var tsTrainingResult = _mapper.Map<IList<TechnicalSpecialistTraining>>(GetTsTrainingById(tsTrainingIds));
                var tsTrainingDocResult = PopulateTsTrainingDocuments(tsTrainingResult);
                result = PopulateTsTrainingNotes(tsTrainingDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainingIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> trainingName)
        {
            IList<TechnicalSpecialistTraining> result = null;
            Exception exception = null;
            try
            {
                var tsTrainingResult = _mapper.Map<IList<TechnicalSpecialistTraining>>(GetTsTrainingByTrainingNames(trainingName));
                var tsTrainingDocResult = PopulateTsTrainingDocuments(tsTrainingResult);
                result = PopulateTsTrainingNotes(tsTrainingDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), trainingName);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<TechnicalSpecialistTraining> result = null;
            Exception exception = null;
            try
            {
                var tsTrainingResult = _mapper.Map<IList<TechnicalSpecialistTraining>>(GetTsTrainingByPin(tsPins));
                var tsTrainingDocResult = PopulateTsTrainingDocuments(tsTrainingResult);
                result = PopulateTsTrainingNotes(tsTrainingDocResult);
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

        public Response Add(IList<TechnicalSpecialistTraining> tsTrainings, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbTrainingTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            return AddTechSpecialistTraining(tsTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistTraining> tsTrainings,
                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                            ref IList<DbModel.Data> dbTrainingTypes,
                            ref IList<DbModel.User> dbVarifiedByUsers,
                            bool commitChange = true,
                            bool isDbValidationRequired = true)
        {
            return AddTechSpecialistTraining(tsTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete 


        public Response Delete(IList<TechnicalSpecialistTraining> tsTrainings,
                               bool commitChange = true,
                               bool isDbValidationRequired = true)
        {

            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings = null;
            return RemoveTechSpecialistTraining(tsTrainings, ref dbTsTrainings, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistTraining> tsTrainings,
                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                bool commitChange = true,
                                bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistTraining(tsTrainings, ref dbTsTrainings, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Modify 

        public Response Modify(IList<TechnicalSpecialistTraining> tsTrainings,
                                bool commitChange = true,
                                bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbTrainingTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            return UpdateTechSpecialistTraining(tsTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistTraining> tsTrainings,
                                ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                ref IList<DbModel.Data> dbTrainingTypes,
                                ref IList<DbModel.User> dbVarifiedByUsers,
                                bool commitChange = true,
                                bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistTraining(tsTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Validation 

        public Response IsRecordExistInDb(IList<int> tsTrainingIds,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsTrainingIdNotExists = null;
            return IsRecordExistInDb(tsTrainingIds, ref dbTsTrainings, ref tsTrainingIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsTrainingIds,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                            ref IList<int> tsTrainingIdNotExists,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsTrainings == null && tsTrainingIds?.Count > 0)
                    dbTsTrainings = GetTsTrainingById(tsTrainingIds);

                result = IsTsTrainingExistInDb(tsTrainingIds, dbTsTrainings, ref tsTrainingIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainingIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings = null;
            IList<TechnicalSpecialistTraining> filteredTSTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbTrainingTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;

            return IsRecordValidForProcess(tsTrainings, validationType, ref filteredTSTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers);

        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                                ValidationType validationType,
                                                ref IList<TechnicalSpecialistTraining> filteredTSTrainings,
                                                ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbTrainingTypes,
                                                ref IList<DbModel.User> dbVarifiedByUsers,
                                                bool isDraft = false)
        {
            return CheckRecordValidForProcess(tsTrainings, validationType, ref filteredTSTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                               ref IList<DbModel.Data> dbTrainingTypes,
                                               ref IList<DbModel.User> dbVarifiedByUsers,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistTraining> filteredTSTrainings = null;
            return CheckRecordValidForProcess(tsTrainings, validationType, ref filteredTSTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings)
        {
            IList<TechnicalSpecialistTraining> filteredTSTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbTrainingTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            return IsRecordValidForProcess(tsTrainings, validationType, ref filteredTSTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers);
        }

        #endregion

        #endregion

        #region Private Methods 

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetTsTrainingByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertificates = null;
            if (pins?.Count > 0)
            {
                dbTsCertificates = _tsCertificationAndTrainingRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsCertificates;
        }

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetTsTrainingById(IList<int> tsTrainingIds)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings = null;
            if (tsTrainingIds?.Count > 0)
                dbTsTrainings = _tsCertificationAndTrainingRepository.FindBy(x => tsTrainingIds.Contains(x.Id)).ToList();

            return dbTsTrainings;
        }

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetTsTrainingByTrainingNames(IList<string> tsTrainingNames)
        {
            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings = null;
            if (tsTrainingNames?.Count > 0)
                dbTsTrainings = _tsCertificationAndTrainingRepository.FindBy(x => tsTrainingNames.Contains(x.CertificationAndTraining.Name)).ToList();

            return dbTsTrainings;
        }

        private Response AddTechSpecialistTraining(IList<TechnicalSpecialistTraining> tsTrainings,
                                           ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.Data> dbTrainingTypes,
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
                IList<TechnicalSpecialistTraining> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                IList<DbModel.Data> dbMasterTrainingTypes = null;
                IList<DbModel.User> dbTrainingVarifiedByUser = null;

                eventId = tsTrainings?.FirstOrDefault().EventId;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsTrainings, ValidationType.Add, ref recordToBeAdd, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers);
                }

                if (!isDbValidationRequired && tsTrainings?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsTrainings, ValidationType.Add);
                }

                if (!isDbValidationRequired || (Convert.ToBoolean(valdResponse.Result) && recordToBeAdd?.Count > 0))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbMasterTrainingTypes = dbTrainingTypes;
                    dbTrainingVarifiedByUser = dbVarifiedByUsers;

                    _tsCertificationAndTrainingRepository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistCertificationAndTraining>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                        opt.Items["DbTrainingTypes"] = dbMasterTrainingTypes;
                        opt.Items["DbVarifiedByUser"] = dbTrainingVarifiedByUser;
                    });
                    _tsCertificationAndTrainingRepository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _tsCertificationAndTrainingRepository.ForceSave();
                        dbTsTrainings = mappedRecords;
                        if (savedCnt > 0)
                        {
                            ProcessTsTrainingNotes(recordToBeAdd, mappedRecords, ValidationType.Add, ref validationMessages);
                            ProcessTsTrainingDocuments(recordToBeAdd, mappedRecords, ref validationMessages);

                        }

                        if (mappedRecords?.Count > 0 && savedCnt > 0)
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
                                _auditSearchService.AuditLog(x1, ref eventId, tsTrainings.FirstOrDefault().ActionByUser,
                                                            null,
                                                            ValidationType.Add.ToAuditActionType(),
                                                            SqlAuditModuleType.TechnicalSpecialistCertificationAndTraining,
                                                             null,
                                                             x1); 
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }
            finally
            {
                _tsCertificationAndTrainingRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistTraining(IList<TechnicalSpecialistTraining> tsTrainings,
                                           ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                           ref IList<DbModel.Data> dbTrainingTypes,
                                           ref IList<DbModel.User> dbVarifiedByUsers,
                                           bool commitChange = true,
                                           bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbMasterTrainingTypes = null;
            IList<DbModel.User> dbTrainingVarifiedByUsers = null;
            IList<TechnicalSpecialistTraining> recordToBeModify = null;
            Response valdResponse = null;
            bool valdResult = true;
            long? eventId = 0;

            try
            {
                eventId = tsTrainings?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsTrainings, ValidationType.Update, ref recordToBeModify, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers);
                }

                if (!isDbValidationRequired && tsTrainings?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsTrainings, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTsTrainings?.Count <= 0)
                    {
                        dbTsTrainings = _tsCertificationAndTrainingRepository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists?.Count <= 0)
                    {
                        //valdResponse = _technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                        valdResponse = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if (dbVarifiedByUsers?.Count <= 0)
                    {
                        IList<string> userNotExists = null;
                        valdResponse = _userService.IsRecordExistInDb(recordToBeModify.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList(), ref dbVarifiedByUsers, ref userNotExists);
                        if (!Convert.ToBoolean(valdResponse.Result))
                        {
                            validationMessages.AddRange(valdResponse.ValidationMessages);
                        }
                    }

                    if (dbTrainingTypes?.Count <= 0)
                    {
                        valdResult = _trainingService.IsValidTraining(recordToBeModify.Select(x => x.TrainingName).ToList(), ref dbTrainingTypes, ref validationMessages);
                    }

                    if ((valdResponse == null || Convert.ToBoolean(valdResponse.Result)) && valdResult && dbTsTrainings?.Count > 0)
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbMasterTrainingTypes = dbTrainingTypes;
                        dbTrainingVarifiedByUsers = dbVarifiedByUsers;
                        IList<TechnicalSpecialistTraining> domExsistanceTrainings = new List<TechnicalSpecialistTraining>();
                        TechnicalSpecialistTraining technicalSpecialistTraining = new TechnicalSpecialistTraining();
                        dbTsTrainings.ToList().ForEach(tsTrainingInfo =>
                        {
                            technicalSpecialistTraining = _mapper.Map<TechnicalSpecialistTraining>(tsTrainingInfo);

                            var oldDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsTrainingInfo.Id)?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.DocumentName)?.ToList();
                            var oldVerificationDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsTrainingInfo.Id)?.VerificationDocuments?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.DocumentName)?.ToList();
                            if (oldDocuments != null && oldDocuments.Count > 0)
                                technicalSpecialistTraining.DocumentName = string.Join(",", oldDocuments);
                            if (oldVerificationDocuments != null && oldVerificationDocuments.Count > 0)
                                technicalSpecialistTraining.VerificationDocumentName = string.Join(",", oldVerificationDocuments);

                            domExsistanceTrainings.Add(ObjectExtension.Clone(technicalSpecialistTraining));
                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsTrainingInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsTrainingInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                    opt.Items["DbTrainingTypes"] = dbMasterTrainingTypes;
                                    opt.Items["DbVarifiedByUser"] = dbTrainingVarifiedByUsers;
                                });
                                tsTrainingInfo.LastModification = DateTime.UtcNow;
                                tsTrainingInfo.UpdateCount = tsTrainingInfo.UpdateCount.CalculateUpdateCount();
                                tsTrainingInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }

                        });
                        _tsCertificationAndTrainingRepository.AutoSave = false;
                        _tsCertificationAndTrainingRepository.Update(dbTsTrainings);
                        if (commitChange)
                        {
                            var savedCnt = _tsCertificationAndTrainingRepository.ForceSave();
                            if (savedCnt > 0)
                            {
                                ProcessTsTrainingNotes(recordToBeModify, dbTsTrainings, ValidationType.Update, ref validationMessages);
                                ProcessTsTrainingDocuments(recordToBeModify, dbTsTrainings, ref validationMessages);
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
                                                                 domExsistanceTrainings?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }
            finally
            {
                _tsCertificationAndTrainingRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialistTraining(IList<TechnicalSpecialistTraining> tsTrainings,
            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                                        bool commitChange = true,
                                                        bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecilist = null;
            IList<DbModel.Data> dbCertificationTypes = null;
            IList<DbModel.User> dbVarifiedByUsers = null;
            IList<TechnicalSpecialistTraining> recordToBeDeleted = null;
            long? eventId = 0;
            try
            {
                eventId = tsTrainings?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(tsTrainings, ValidationType.Delete, ref dbTsTrainings, ref dbTechnicalSpecilist, ref dbCertificationTypes, ref dbVarifiedByUsers);

                if (tsTrainings?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsTrainings, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsTrainings?.Count > 0)
                {
                    var dbTsTrainingToBeDeleted = dbTsTrainings?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _tsCertificationAndTrainingRepository.AutoSave = false;
                    _tsCertificationAndTrainingRepository.Delete(dbTsTrainingToBeDeleted);
                    if (commitChange)
                    {
                        int value = _tsCertificationAndTrainingRepository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value > 0)
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                  null,
                                                                                                 ValidationType.Delete.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.TechnicalSpecialistCertificationAndTraining,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }
            finally
            {
                _tsCertificationAndTrainingRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistTraining> filteredTsTrainings,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbTrainingTypes,
                                            ref IList<DbModel.User> dbVarifiedByUsers,
                                            bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsTrainings, ref filteredTsTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsTrainings, ref filteredTsTrainings, ref dbTsTrainings, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsTrainings, ref filteredTsTrainings, ref dbTsTrainings, ref dbTechnicalSpecialists, ref dbTrainingTypes, ref dbVarifiedByUsers, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsTsTrainingExistInDb(IList<int> tsTrainingIds,
                                        IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                        ref IList<int> tsTrainingIdNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {

            validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsTrainings = dbTsTrainings ?? new List<DbModel.TechnicalSpecialistCertificationAndTraining>();

            var validMessages = validationMessages;

            if (tsTrainingIds?.Count > 0)
            {
                tsTrainingIdNotExists = tsTrainingIds.Where(id => !dbTsTrainings.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsTrainingIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsTrainingIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private IList<TechnicalSpecialistTraining> FilterRecord(IList<TechnicalSpecialistTraining> tsTrainings, ValidationType filterType)
        {
            IList<TechnicalSpecialistTraining> filterTsTrainings = null;

            if (filterType == ValidationType.Add)
                filterTsTrainings = tsTrainings?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsTrainings = tsTrainings?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsTrainings = tsTrainings?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsTrainings;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistTraining> tsTrainings,
                              ValidationType validationType,
                              ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsTrainings), validationType);
            foreach (var item in tsTrainings)
            {
                if (item.EffeciveDate != null && item.ExpiryDate != null)
                    if (item.ExpiryDate < item.EffeciveDate)
                        messages.Add(_messages, item.EffeciveDate, MessageType.TsInternalTrainingExpiryDate, item.ExpiryDate);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistTraining> tsTrainings,
                                     ref IList<TechnicalSpecialistTraining> filteredTsTrainings,
                                     ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                     ref IList<DbModel.Data> dbTrainingTypes,
                                     ref IList<DbModel.User> dbVarifiedByUsers,
                                     ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsTrainings != null && tsTrainings.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsTrainings == null || filteredTsTrainings.Count <= 0)
                    filteredTsTrainings = FilterRecord(tsTrainings, validationType);

                if (filteredTsTrainings?.Count > 0 && IsValidPayload(filteredTsTrainings, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsTrainings.Select(x => x.Epin.ToString()).ToList();
                    IList<string> TrainingNames = filteredTsTrainings.Select(x => x.TrainingName).ToList();

                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    if (result && TrainingNames?.Count > 0)
                        result = _trainingService.IsValidTraining(TrainingNames, ref dbTrainingTypes, ref validationMessages);
                    if (result)
                    {
                        IList<string> userNotExists = null;
                        var valResponse = _userService.IsRecordExistInDb(filteredTsTrainings.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList(), ref dbVarifiedByUsers, ref userNotExists);
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

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistTraining> tsTrainings,
                                            ref IList<TechnicalSpecialistTraining> filteredTsTrainings,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbTrainingTypes,
                                            ref IList<DbModel.User> dbVarifiedByUsers,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {
            bool result = false;
            if (tsTrainings != null && tsTrainings.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsTrainings == null || filteredTsTrainings.Count <= 0)
                    filteredTsTrainings = FilterRecord(tsTrainings, validationType);

                if (filteredTsTrainings?.Count > 0 && IsValidPayload(filteredTsTrainings, validationType, ref messages))
                {
                    GetTsTrainingDbInfo(filteredTsTrainings, ref dbTsTrainings);
                    IList<int> tsTrainingIds = filteredTsTrainings.Select(x => x.Id).ToList();
                    IList<int> tsDBTrainingIds = dbTsTrainings.Select(x => x.Id).ToList();
                    if (tsTrainingIds.Any(x => !tsDBTrainingIds.Contains(x))) //Invalid TechSpecialist Training Id found.
                    {
                        var dbTsTrainingInfosByIds = dbTsTrainings;
                        var idNotExists = tsTrainingIds.Where(id => !dbTsTrainingInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistTrainingList = filteredTsTrainings;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsTraining = techSpecialistTrainingList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsTraining, MessageType.TsTrainingUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsTrainings, dbTsTrainings, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsTrainings.Select(x => x.Epin.ToString()).ToList();
                            IList<string> TrainingNames = filteredTsTrainings.Select(x => x.TrainingName).ToList();

                            //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            if (result && TrainingNames?.Count > 0)
                                result = _trainingService.IsValidTraining(TrainingNames, ref dbTrainingTypes, ref validationMessages);
                            if (result)
                            {
                                IList<string> userNotExists = null;
                                var valResponse = _userService.IsRecordExistInDb(filteredTsTrainings.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.VerifiedBy)).ToList(), ref dbVarifiedByUsers, ref userNotExists);
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


        private bool IsRecordValidForRemove(IList<TechnicalSpecialistTraining> tsTrainings,
                                            ref IList<TechnicalSpecialistTraining> filteredTsTrainings,
                                            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsTrainings != null && tsTrainings.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsTrainings == null || filteredTsTrainings.Count <= 0)
                    filteredTsTrainings = FilterRecord(tsTrainings, validationType);

                if (filteredTsTrainings?.Count > 0 && IsValidPayload(filteredTsTrainings, validationType, ref validationMessages))
                {
                    GetTsTrainingDbInfo(filteredTsTrainings, ref dbTsTrainings);
                    IList<int> tsTrainingIdNotExists = null;
                    var tsTrainingIds = filteredTsTrainings.Select(x => x.Id).Distinct().ToList();
                    result = IsTsTrainingExistInDb(tsTrainingIds, dbTsTrainings, ref tsTrainingIdNotExists, ref validationMessages);
                    if (result)
                        result = IsTechSpecialistTrainingCanBeRemove(dbTsTrainings, ref validationMessages);
                }
            }
            return result;
        }

        private void GetTsTrainingDbInfo(IList<TechnicalSpecialistTraining> filteredTsTrainings,
                                    ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings)
        {
            dbTsTrainings = dbTsTrainings ?? new List<DbModel.TechnicalSpecialistCertificationAndTraining>();
            IList<int> tsPayScheduleIds = filteredTsTrainings?.Select(x => x.Id).Distinct().ToList();
            if (tsPayScheduleIds?.Count > 0 && (dbTsTrainings.Count <= 0 || dbTsTrainings.Any(x => !tsPayScheduleIds.Contains(x.Id))))
            {
                var tsTrainings = GetTsTrainingById(tsPayScheduleIds);
                if (tsTrainings != null && tsTrainings.Any())
                {
                    dbTsTrainings.AddRange(tsTrainings);
                }
            }

        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistTraining> tsTrainings,
                                        IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                        ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsTrainings.Where(x => !dbTsTrainings.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsTrainingUpdatedByOther, x.TrainingName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTechSpecialistTrainingCanBeRemove(IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsTrainings?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x, MessageType.TsTrainingIsBeingUsed, x.CertificationAndTraining.Name);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private IList<TechnicalSpecialistTraining> PopulateTsTrainingDocuments(IList<TechnicalSpecialistTraining> tsTrinings)
        {
            try
            {
                if (tsTrinings?.Count > 0)
                {
                    var epins = tsTrinings.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var trainingIds = tsTrinings.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsTrainingDocs = _documentService.Get(ModuleCodeType.TS, epins, trainingIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsTrainingDocs?.Count > 0)
                    {
                        return tsTrinings.GroupJoin(tsTrainingDocs,
                             tst => new { moduleRefCode = tst.Epin.ToString(), subModuleRefCode = tst.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsTrain = tsc, doc }).Select(x =>
                            {
                                x.tsTrain.Documents = x?.doc?.Where(x1 => x1.DocumentType == DocumentType.TS_Training.ToString())?.ToList();
                                x.tsTrain.VerificationDocuments = x?.doc?.Where(x1 => x1.DocumentType == DocumentType.TS_TrainingVerification.ToString())?.ToList();
                                return x.tsTrain;
                            })?.ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrinings);
            }

            return tsTrinings;
        }

        private Response ProcessTsTrainingDocuments(IList<TechnicalSpecialistTraining> tsTrainings, IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tsTrainings?.Count > 0)
                {
                    tsTrainings = tsTrainings.Join(dbTsTrainings,
                        tsc => tsc.TrainingName,
                        dbtsc => dbtsc.CertificationAndTraining.Name,
                        (tsc, dbtsc) => new { tsTrain = tsc, dbtsc }).Select(x =>
                        {
                            x.tsTrain?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = x.tsTrain?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = x?.dbtsc?.Id.ToString();
                                x1.ModuleRefCode = x?.tsTrain?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_Training.ToString();
                            });
                            x.tsTrain?.VerificationDocuments?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = x.tsTrain?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = x.dbtsc.Id.ToString();
                                x1.ModuleRefCode = x?.tsTrain?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_TrainingVerification.ToString();
                            });
                            return x.tsTrain;
                        }).ToList();

                    var tsDocToBeProcess = tsTrainings?.Where(x => x.Documents != null &&
                                                                                          x.Documents.Any(x1 => x1.RecordStatus != null))
                                                                              .SelectMany(x => x.Documents)
                                                                              .ToList();

                    var tsTrainingVarifDocs = tsTrainings?.Where(x => x.VerificationDocuments != null &&
                                                                          x.VerificationDocuments.Any(x1 => x1.RecordStatus != null))
                                                              .SelectMany(x => x.VerificationDocuments)
                                                              .ToList();

                    if (tsTrainingVarifDocs != null && tsTrainingVarifDocs.Count > 0)
                    {
                        tsDocToBeProcess.AddRange(tsTrainingVarifDocs);
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistTraining> PopulateTsTrainingNotes(IList<TechnicalSpecialistTraining> tsTrainings)
        {
            try
            {
                if (tsTrainings?.Count > 0)
                {
                    var epins = tsTrainings.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var trainingIds = tsTrainings.Select(x => x.Id).Distinct().ToList();
                    var tsTrainingNotes = _tsNoteService.Get(NoteType.TD, true, epins, trainingIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                    if (tsTrainingNotes?.Count > 0)
                    {
                        return tsTrainings.GroupJoin(tsTrainingNotes,
                            tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                            note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                            (tsc, note) => new { tsTraining = tsc, note }).Select(x =>
                            {
                                x.tsTraining.Notes = x?.note?.FirstOrDefault()?.Note;
                                return x.tsTraining;
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }

            return tsTrainings;
        }

        private Response ProcessTsTrainingNotes(IList<TechnicalSpecialistTraining> tsTrainings, IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistNoteInfo> tsNewTrainNotes = null;
            try
            {
                if (tsTrainings?.Count > 0)
                {
                    if (validationType == ValidationType.Add)
                    {
                        tsNewTrainNotes = tsTrainings.Join(dbTsTrainings,
                             tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.TrainingName },
                             dbtsc => new { epin = dbtsc.TechnicalSpecialistId.ToString(), RefId = dbtsc.CertificationAndTraining.Name },
                             (tsc, dbtsc) => new { tsc, dbtsc }).Select(x =>
                             new TechnicalSpecialistNoteInfo
                             {
                                 Epin = x.tsc.Epin,
                                 RecordType = NoteType.TD.ToString(),
                                 RecordRefId = x?.dbtsc?.Id,
                                 Note = x?.tsc?.Notes,
                                 RecordStatus = RecordStatus.New.FirstChar(),
                                 CreatedBy = x?.tsc.ActionByUser,
                                 CreatedDate = DateTime.UtcNow,
                                 EventId = x?.tsc.EventId,
                                 ActionByUser = x?.tsc.ActionByUser,
                             }).ToList();

                    }
                    else if (validationType == ValidationType.Update)
                    {
                        var epins = tsTrainings.Select(x => x.Epin.ToString()).Distinct().ToList();
                        var trainingIds = tsTrainings.Select(x => x.Id).Distinct().ToList();
                        var tsTrainingNotes = _tsNoteService.Get(NoteType.TD, true, epins, trainingIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();



                        tsNewTrainNotes = tsTrainings.GroupJoin(tsTrainingNotes,
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
                                 RecordType = NoteType.TD.ToString(),
                                 RecordRefId = x.tsCert.Id,
                                 EventId = x.tsCert.EventId,
                                 ActionByUser = x.tsCert.ActionByUser
                             }).ToList();


                    }
                    return _tsNoteService.Add(tsNewTrainNotes);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTrainings);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        #endregion
    }
}
