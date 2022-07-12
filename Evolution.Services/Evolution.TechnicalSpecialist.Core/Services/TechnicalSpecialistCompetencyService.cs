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
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistCompetencyService : ITechnicalSpecialistCompetencyService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistCompetencyService> _logger = null;
        private readonly ITechnicalSpecialistTrainingAndCompetencyRepository _tsTrainingAndCompetencyRepository = null;
        private readonly ITechnicalSpecialistCompetencyValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IDocumentService _documentService = null;
        private readonly ITechnicalSpecialistNoteService _tsNoteService = null;
        private readonly ITechnicalSpecialistTrainingAndCompetancyTypeService _tsTrainingAndCompetancyTypeService = null;
        private readonly IAuditSearchService _auditSearchService = null;


        #region Constructor

        public TechnicalSpecialistCompetencyService(IMapper mapper,
                                                    JObject messages,
                                                    IAppLogger<TechnicalSpecialistCompetencyService> logger,
                                                    ITechnicalSpecialistTrainingAndCompetencyRepository tsTrainingAndCompetencyRepository,
                                                    ITechnicalSpecialistCompetencyValidationService validationService,
                                                    //ITechnicalSpecialistService technSpecServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    IDocumentService documentService,
                                                    ITechnicalSpecialistNoteService tsNoteService,
                                                    ITechnicalSpecialistTrainingAndCompetancyTypeService tsTrainingAndCompetancyTypeService,
                                                    IAuditSearchService auditSearchService

            )
        {
            _mapper = mapper;
            _messages = messages;
            _logger = logger;
            _tsTrainingAndCompetencyRepository = tsTrainingAndCompetencyRepository;
            _validationService = validationService;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _documentService = documentService;
            _tsNoteService = tsNoteService;
            _tsTrainingAndCompetancyTypeService = tsTrainingAndCompetancyTypeService;

            _auditSearchService = auditSearchService;
        }

        #endregion

        #region Public Methods

        #region Add

        public Response Add(IList<TechnicalSpecialistCompetency> tsCompetencies, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return AddTechSpecialistCompetency(tsCompetencies, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistCompetency> tsCompetencies, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialistCompetency(tsCompetencies, ref dbTsCompetencies, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistCompetency> tsCompetencies, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies = null;
            return RemoveTechSpecialistCompetency(tsCompetencies, ref dbTsCompetencies, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistCompetency> tsCompetencies, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistCompetency(tsCompetencies, ref dbTsCompetencies, commitChange, isDbValidationRequired);
        }

        public Response Get(TechnicalSpecialistCompetency searchModel)
        {
            IList<TechnicalSpecialistCompetency> result = null;
            Exception exception = null;
            try
            {
                var tsCompetencyResult = _mapper.Map<IList<TechnicalSpecialistCompetency>>(_tsTrainingAndCompetencyRepository.Search(searchModel));
                var tsCompDocResult = PopulateTsCompetencyDocuments(tsCompetencyResult);
                result = PopulateTsCompetencyNotes(tsCompDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> tsCompetencyIds)
        {
            IList<TechnicalSpecialistCompetency> result = null;
            Exception exception = null;
            try
            {
                var tsCompetencyResult = _mapper.Map<IList<TechnicalSpecialistCompetency>>(GetTsCompetencyById(tsCompetencyIds));
                var tsCompDocResult = PopulateTsCompetencyDocuments(tsCompetencyResult);
                result = PopulateTsCompetencyNotes(tsCompDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencyIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> competencyNames)
        {
            IList<TechnicalSpecialistCompetency> result = null;
            Exception exception = null;
            try
            {
                var tsCompetencyResult = _mapper.Map<IList<TechnicalSpecialistCompetency>>(GetTsCompetencyByNames(competencyNames));
                var tsCompDocResult = PopulateTsCompetencyDocuments(tsCompetencyResult);
                result = PopulateTsCompetencyNotes(tsCompDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), competencyNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<TechnicalSpecialistCompetency> result = null;
            Exception exception = null;
            try
            {
                var tsCompetencyResult = _mapper.Map<IList<TechnicalSpecialistCompetency>>(GetTsCompetencyByPin(tsPins));
                var tsCompDocResult = PopulateTsCompetencyDocuments(tsCompetencyResult);
                result = PopulateTsCompetencyNotes(tsCompDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Validations

        public Response IsRecordExistInDb(IList<int> tsCompetencyIds, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsCompetencIdNotExists = null;
            return IsRecordExistInDb(tsCompetencyIds, ref dbTsCompetencies, ref tsCompetencIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsCompetencyIds, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<int> tsCompetencyIdNotExists, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsCompetencies == null && tsCompetencyIds?.Count > 0)
                    dbTsCompetencies = GetTsCompetencyById(tsCompetencyIds);

                result = IsTsCompetencyExistInDb(tsCompetencyIds, dbTsCompetencies, ref tsCompetencyIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencyIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencys = null;
            IList<TechnicalSpecialistCompetency> filteredTSCompetencys = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return IsRecordValidForProcess(tsCompetencies, validationType, ref filteredTSCompetencys, ref dbTsCompetencys, ref dbTechnicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies, ValidationType validationType, ref IList<TechnicalSpecialistCompetency> filteredTSCompetencies, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool isDraft = false)
        {
            return CheckRecordValidForProcess(tsCompetencies, validationType, ref filteredTSCompetencies, ref dbTsCompetencies, ref dbTechnicalSpecialists, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool isDraft = false)
        {
            IList<TechnicalSpecialistCompetency> filteredTSInternalCompetencys = null;

            return IsRecordValidForProcess(tsCompetencies, validationType, ref filteredTSInternalCompetencys, ref dbTsCompetencies, ref dbTechnicalSpecialists, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return IsRecordValidForProcess(tsCompetencies, validationType, ref dbTsCompetencies, ref dbTechnicalSpecialists);
        }

        public Response Modify(IList<TechnicalSpecialistCompetency> tsCompetencies, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return UpdateTechSpecialistCompetency(tsCompetencies, ref dbTsCompetencies, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistCompetency> tsCompetencies, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistCompetency(tsCompetencies, ref dbTsCompetencies, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }
        #endregion

        #endregion

        #region Private Methods 

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTsCompetencyByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            if (pins?.Count > 0)
            {
                dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsInternalTrainings;
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTsCompetencyById(IList<int> tsInternalTrainingIds)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            if (tsInternalTrainingIds?.Count > 0)
                dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.FindBy(x => tsInternalTrainingIds.Contains(x.Id)).ToList();

            return dbTsInternalTrainings;
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTsCompetencyByNames(IList<string> tsInternalTrainingNames)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            if (tsInternalTrainingNames?.Count > 0)
                dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.FindBy(x => x.TechnicalSpecialistTrainingAndCompetencyType.Any(x1 => tsInternalTrainingNames.Contains(x1.TrainingOrCompetencyData.Name))).ToList();

            return dbTsInternalTrainings;
        }

        private Response AddTechSpecialistCompetency(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                   ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                   bool commitChange = true,
                                   bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistCompetency> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                eventId = tsCompetencies?.FirstOrDefault()?.EventId; 

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsCompetencies, ValidationType.Add, ref recordToBeAdd, ref dbTsCompetencies, ref dbTechnicalSpecialists);
                }

                if (!isDbValidationRequired && tsCompetencies?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsCompetencies, ValidationType.Add);
                }

                if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;

                    _tsTrainingAndCompetencyRepository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistTrainingAndCompetency>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                    });
                    mappedRecords?.Select(y => { y.IsIlearn = false; return y; })?.ToList();
                    _tsTrainingAndCompetencyRepository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _tsTrainingAndCompetencyRepository.ForceSave();
                        dbTsCompetencies = mappedRecords;
                        if (savedCnt > 0)
                        {
                            ProcessTsCompetencyTypes(recordToBeAdd, dbTsCompetencies, ref validationMessages);
                            ProcessTsCompetencyNotes(recordToBeAdd, mappedRecords, ValidationType.Add, ref validationMessages);
                            ProcessTsCompetencyDocuments(recordToBeAdd, mappedRecords, ref validationMessages);

                        }
                        if (recordToBeAdd?.Count > 0)
                        {
                            int i = 0;
                            recordToBeAdd?.ToList().ForEach(x1 => {
                                var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                if (newDocuments != null && newDocuments.Count > 0)
                                    x1.DocumentName = string.Join(",", newDocuments);

                                x1.Id = mappedRecords[i++].Id;// def1035 test 2
                                _auditSearchService.AuditLog(x1, ref eventId, tsCompetencies?.FirstOrDefault()?.ActionByUser,
                                                          null,
                                                           ValidationType.Add.ToAuditActionType(),
                                                            SqlAuditModuleType.TechnicalSpecialistTrainingAndCompetency,
                                                            null,
                                                            x1
                                                            );
                            }
                            );
                        }
                        

                    }
                     
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }
            finally
            {
                _tsTrainingAndCompetencyRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistCompetency(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                   ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                   bool commitChange = true,
                                   bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            long? eventId = 0;

            IList<TechnicalSpecialistCompetency> recordToBeModify = null;
            Response valdResponse = null;

            try
            { 
                eventId = tsCompetencies?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsCompetencies, ValidationType.Update, ref recordToBeModify, ref dbTsCompetencies, ref dbTechnicalSpecialists);
                }

                if (!isDbValidationRequired && tsCompetencies?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsCompetencies, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTsCompetencies?.Count <= 0)
                    {
                        dbTsCompetencies = _tsTrainingAndCompetencyRepository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists?.Count <= 0)
                    {
                        //valdResponse = _technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                        valdResponse = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if ((valdResponse == null || Convert.ToBoolean(valdResponse.Result)) && dbTsCompetencies?.Count > 0)
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        IList<TechnicalSpecialistCompetency> domExsistingTechSplCompetency = new List<TechnicalSpecialistCompetency>();
                        TechnicalSpecialistCompetency technicalSpecialistCompetency = new TechnicalSpecialistCompetency();

                        dbTsCompetencies.ToList().ForEach(tsCompInfo =>
                        {
                            technicalSpecialistCompetency = _mapper.Map<TechnicalSpecialistCompetency>(tsCompInfo);
                            var oldDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsCompInfo.Id)?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.DocumentName)?.ToList();
                            if (oldDocuments != null && oldDocuments.Count > 0)
                                technicalSpecialistCompetency.DocumentName = string.Join(",", oldDocuments);

                            domExsistingTechSplCompetency.Add(technicalSpecialistCompetency);

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsCompInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsCompInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                });
                                tsCompInfo.LastModification = DateTime.UtcNow;
                                tsCompInfo.UpdateCount = tsCompInfo.UpdateCount.CalculateUpdateCount();
                                tsCompInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                        });
                        _tsTrainingAndCompetencyRepository.AutoSave = false;
                        _tsTrainingAndCompetencyRepository.Update(dbTsCompetencies);
                        if (commitChange)
                        {
                            var savedCnt = _tsTrainingAndCompetencyRepository.ForceSave();
                            if (savedCnt > 0)
                            {
                                ProcessTsCompetencyTypes(recordToBeModify, dbTsCompetencies, ref validationMessages);
                                ProcessTsCompetencyNotes(recordToBeModify, dbTsCompetencies, ValidationType.Update, ref validationMessages);
                                ProcessTsCompetencyDocuments(recordToBeModify, dbTsCompetencies, ref validationMessages);

                            }
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
                                                         SqlAuditModuleType.TechnicalSpecialistTrainingAndCompetency,
                                                         domExsistingTechSplCompetency?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                          x1
                                                         );
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }
            finally
            {
                _tsTrainingAndCompetencyRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialistCompetency(IList<TechnicalSpecialistCompetency> tsCompetencies,
              ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                                bool commitChange = true,
                                                bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistCompetency> recordToBeDeleted = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsCompetencies?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(tsCompetencies, ValidationType.Delete, ref dbTsCompetencies);

                if (!isDbValidationRequired && tsCompetencies?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsCompetencies, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsCompetencies?.Count > 0)
                {
                    var dbTsCompetenciesToBeDeleted = dbTsCompetencies?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _tsTrainingAndCompetencyRepository.AutoSave = false;
                    _tsTrainingAndCompetencyRepository.Delete(dbTsCompetenciesToBeDeleted);
                    if (commitChange)
                    {
                        _tsTrainingAndCompetencyRepository.ForceSave();
                        if (recordToBeDeleted.Count > 0)
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser, null,
                                                                                                   ValidationType.Delete.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.TechnicalSpecialistTrainingAndCompetency,
                                                                                                   x1,
                                                                                                   null));
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }
            finally
            {
                _tsTrainingAndCompetencyRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistCompetency> filteredTsCompetencies,
                                            ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists
                                            , bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsCompetencies, ref filteredTsCompetencies, ref dbTsCompetencies, ref dbTechnicalSpecialists, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsCompetencies, ref filteredTsCompetencies, ref dbTsCompetencies, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsCompetencies, ref filteredTsCompetencies, ref dbTsCompetencies, ref dbTechnicalSpecialists, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistCompetency> tsCompetencies,
                             ref IList<TechnicalSpecialistCompetency> filteredTsCompetencies,
                             ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                             ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCompetencies != null && tsCompetencies.Count > 0)
            {
                ValidationType validationType = ValidationType.Add; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsCompetencies == null || filteredTsCompetencies.Count <= 0)
                    filteredTsCompetencies = FilterRecord(tsCompetencies, validationType);

                if (filteredTsCompetencies?.Count > 0 && IsValidPayload(filteredTsCompetencies, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsCompetencies.Select(x => x.Epin.ToString()).ToList();
                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                    ref IList<TechnicalSpecialistCompetency> filteredTsCompetencies,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                    ref IList<ValidationMessage> validationMessages,
                                    bool isDraft = false)
        {
            bool result = false;
            if (tsCompetencies != null && tsCompetencies.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                    validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsCompetencies == null || filteredTsCompetencies.Count <= 0)
                    filteredTsCompetencies = FilterRecord(tsCompetencies, validationType);

                if (filteredTsCompetencies?.Count > 0 && IsValidPayload(filteredTsCompetencies, validationType, ref messages))
                {
                    GetTsCompetencyDbInfo(filteredTsCompetencies, ref dbTsCompetencies);
                    IList<int> tsCompetencyIds = filteredTsCompetencies.Select(x => x.Id).ToList();
                    IList<int> tsDBCompetencyIds = dbTsCompetencies.Select(x => x.Id).ToList();
                    if (tsCompetencyIds.Any(x => !tsDBCompetencyIds.Contains(x))) //Invalid TechSpecialist Competency Id found.
                    {
                        var dbTsCompetencyInfosByIds = dbTsCompetencies;
                        var idNotExists = tsCompetencyIds.Where(id => !dbTsCompetencyInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistCertificateList = filteredTsCompetencies;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsCertificate = techSpecialistCertificateList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsCertificate, MessageType.TsCompetencyUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsCompetencies, dbTsCompetencies, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsCompetencies.Select(x => x.Epin.ToString()).ToList();
                            //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);

                if (isDraft) //To handle reject TS changes and duplicate value validation
                {
                    result = true;
                    validationMessages.Clear();
                }
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                    ref IList<TechnicalSpecialistCompetency> filteredTsCompetencies,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                    ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsCompetencies != null && tsCompetencies.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsCompetencies == null || filteredTsCompetencies.Count <= 0)
                    filteredTsCompetencies = FilterRecord(tsCompetencies, validationType);

                if (filteredTsCompetencies?.Count > 0 && IsValidPayload(filteredTsCompetencies, validationType, ref validationMessages))
                {
                    GetTsCompetencyDbInfo(filteredTsCompetencies, ref dbTsCompetencies);
                    IList<int> tsCompetencyIdNotExists = null;
                    var tsCompetencyIds = filteredTsCompetencies.Select(x => x.Id).Distinct().ToList();
                    result = IsTsCompetencyExistInDb(tsCompetencyIds, dbTsCompetencies, ref tsCompetencyIdNotExists, ref validationMessages);
                    ProcessTsCompetencyTypes(filteredTsCompetencies, dbTsCompetencies, ref validationMessages);
                    if (result)
                        result = IsTechSpecialistCompetencyCanBeRemove(dbTsCompetencies, ref validationMessages);
                }
            }
            return result;
        }

        private IList<TechnicalSpecialistCompetency> FilterRecord(IList<TechnicalSpecialistCompetency> tsCompetencies, ValidationType filterType)
        {
            IList<TechnicalSpecialistCompetency> filterTsCompetencies = null;

            if (filterType == ValidationType.Add)
                filterTsCompetencies = tsCompetencies?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsCompetencies = tsCompetencies?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsCompetencies = tsCompetencies?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsCompetencies;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistCompetency> tsCompetencies,
                      ValidationType validationType,
                      ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsCompetencies), validationType);

            if (tsCompetencies != null && tsCompetencies.Any(x => x.TechnicalSpecialistCompetencyTypes?.Count==0))
                messages.Add(_messages, "DVA Charters", MessageType.TsDVACharters);
            else if(tsCompetencies.SelectMany(x => x.TechnicalSpecialistCompetencyTypes).Any(x=>string.IsNullOrEmpty(x.TypeName)))
                messages.Add(_messages, "DVA Charters", MessageType.TsDVACharters);

            foreach (var item in tsCompetencies)
            {
                if (item.EffectiveDate != null && item.Expiry != null)
                    if (item.Expiry < item.EffectiveDate)
                        messages.Add(_messages, item.EffectiveDate, MessageType.TsCompetencyExpiryDate, item.Expiry);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void GetTsCompetencyDbInfo(IList<TechnicalSpecialistCompetency> filteredTsCompetencies,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies)
        {
            dbTsCompetencies = dbTsCompetencies ?? new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();
            IList<int> tsInternalTrainingIds = filteredTsCompetencies?.Select(x => x.Id).Distinct().ToList();
            if (tsInternalTrainingIds?.Count > 0 && (dbTsCompetencies.Count <= 0 || dbTsCompetencies.Any(x => !tsInternalTrainingIds.Contains(x.Id))))
            {
                var tsCompetencies = GetTsCompetencyById(tsInternalTrainingIds);
                if (tsCompetencies != null && tsCompetencies.Any())
                {
                    dbTsCompetencies.AddRange(tsCompetencies);
                }
            }
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var notMatchedRecords = tsCompetencies.Where(x => !dbTsCompetencies.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsCompetencyUpdatedByOther);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTsCompetencyExistInDb(IList<int> tsCompetencyIds,
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                ref IList<int> tsCompetencyIdNotExists,
                                ref IList<ValidationMessage> validationMessages)
        {
            var validMessages=validationMessages = validationMessages ??  new List<ValidationMessage>(); 
                dbTsCompetencies = dbTsCompetencies ?? new List<DbModel.TechnicalSpecialistTrainingAndCompetency>(); 

            if (tsCompetencyIds?.Count > 0)
            {
                tsCompetencyIdNotExists = tsCompetencyIds.Where(id => !dbTsCompetencies.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsCompetencyIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsCompetencyIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialistCompetencyCanBeRemove(IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsCompetencies?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x, MessageType.TsCompetencyIsBeingUsed);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private IList<TechnicalSpecialistCompetency> PopulateTsCompetencyDocuments(IList<TechnicalSpecialistCompetency> tsCompetencies)
        {
            try
            {
                if (tsCompetencies?.Count > 0)
                {
                    var epins = tsCompetencies.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var competencyIds = tsCompetencies.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsCompetencyDocs = _documentService.Get(ModuleCodeType.TS, epins, competencyIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsCompetencyDocs?.Count > 0)
                    {
                        return tsCompetencies.GroupJoin(tsCompetencyDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsCert = tsc, doc }).Select(x =>
                            {
                                x.tsCert.Documents = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_Competency.ToString()).ToList();
                                return x.tsCert;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }

            return tsCompetencies;
        }

        private Response ProcessTsCompetencyDocuments(IList<TechnicalSpecialistCompetency> tsCompetencies, IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tsCompetencies?.Count == dbTsCompetencies?.Count)
                {
                    //NOTE : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                    for (int comCnt = 0; comCnt < tsCompetencies?.Count; comCnt++)
                    {
                        if (dbTsCompetencies[comCnt] != null && tsCompetencies[comCnt].Documents != null)
                        {
                            tsCompetencies[comCnt]?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = tsCompetencies[comCnt]?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = dbTsCompetencies[comCnt].Id.ToString();
                                x1.ModuleRefCode = tsCompetencies[comCnt]?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_Competency.ToString();
                                x1.Status = x1.Status.Trim();
                            });
                        }
                    }

                    var tsDocToBeProcess = tsCompetencies?.Where(x => x.Documents != null &&
                                                                             x.Documents.Any(x1 => x1.RecordStatus != null))?
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistCompetency> PopulateTsCompetencyNotes(IList<TechnicalSpecialistCompetency> tsCompetencies)
        {
            try
            {
                if (tsCompetencies?.Count > 0)
                {
                    var epins = tsCompetencies.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var competencyIds = tsCompetencies.Select(x => x.Id).Distinct().ToList();
                    var tsCompetencyNotes = _tsNoteService.Get(NoteType.CD, true, epins, competencyIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                    if (tsCompetencyNotes?.Count > 0)
                    {
                        return tsCompetencies.GroupJoin(tsCompetencyNotes,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }

            return tsCompetencies;
        }

        private Response ProcessTsCompetencyNotes(IList<TechnicalSpecialistCompetency> tsCompetencies, IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistNoteInfo> tsNewComptNotes = new List<TechnicalSpecialistNoteInfo>();
            try
            {
                if (tsCompetencies?.Count == dbTsCompetencies?.Count)
                {
                    if (validationType == ValidationType.Add)
                    {
                        //Note : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                        for (int comCnt = 0; comCnt < tsCompetencies?.Count; comCnt++)
                        {
                            if (tsCompetencies[comCnt] != null && dbTsCompetencies[comCnt] != null)
                            {
                                tsNewComptNotes.Add(
                                new TechnicalSpecialistNoteInfo
                                {
                                    Epin = tsCompetencies[comCnt].Epin,
                                    RecordType = NoteType.CD.ToString(),
                                    RecordRefId = dbTsCompetencies[comCnt].Id,
                                    Note = tsCompetencies[comCnt].Notes,
                                    RecordStatus = RecordStatus.New.FirstChar(),
                                    CreatedBy = tsCompetencies[comCnt].ActionByUser,
                                    CreatedDate = DateTime.UtcNow,
                                    EventId = tsCompetencies[comCnt].EventId,
                                    ActionByUser = tsCompetencies[comCnt].ActionByUser
                                });
                            }
                        }

                    }
                    else if (validationType == ValidationType.Update)
                    {
                        var epins = tsCompetencies.Select(x => x.Epin.ToString()).Distinct().ToList();
                        var competencyIds = tsCompetencies.Select(x => x.Id).Distinct().ToList();
                        var tsCompetencyNotes = _tsNoteService.Get(NoteType.CD, true, epins, competencyIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                        tsNewComptNotes = tsCompetencyNotes.Join(tsCompetencies,
                             note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                             tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                             (note, tsc) => new { note, tsCert = tsc }).Where(x => !string.Equals(x.note.Note, x.tsCert.Notes)).Select(x =>
                             {
                                 x.note.Note = x.tsCert.Notes;
                                 x.note.RecordStatus = RecordStatus.New.FirstChar();
                                 x.note.CreatedBy = x.tsCert.ActionByUser;
                                 x.note.CreatedDate = DateTime.UtcNow;
                                 x.note.EventId = x.tsCert.EventId;
                                 x.note.ActionByUser = x.tsCert.ActionByUser;
                                 return x.note;
                             }).ToList();

                    }
                    return _tsNoteService.Add(tsNewComptNotes);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsCompetencies);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTsCompetencyTypes(IList<TechnicalSpecialistCompetency> tsCompetencies, IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTSTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys = null;
            IList<DbModel.Data> dbMasterIntTrainingAndCompetencys = null;
            Response response = null;
            try
            {

                if (tsCompetencies?.Count == dbTsCompetencies?.Count)
                {
                    //Note : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                    for (int comCnt = 0; comCnt < tsCompetencies?.Count; comCnt++)
                    {
                        if (tsCompetencies[comCnt] != null && dbTsCompetencies[comCnt] != null)
                        {
                            tsCompetencies[comCnt].TechnicalSpecialistCompetencyTypes.ToList().ForEach(x => { x.TechnicalSpecialistInternalTrainingAndCompetencyId = dbTsCompetencies[comCnt].Id; x.ActionByUser = tsCompetencies[comCnt].ActionByUser; x.EventId = tsCompetencies[comCnt].EventId; });
                        }
                    }

                    IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsCompetencyTypes = tsCompetencies.SelectMany(x => x.TechnicalSpecialistCompetencyTypes).ToList();

                    //Check for any valid delete records are present and delete
                    var valDelRes = _tsTrainingAndCompetancyTypeService.IsRecordValidForProcess(tsCompetencyTypes, ValidationType.Delete, CompCertTrainingType.Co, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                    if (Convert.ToBoolean(valDelRes.Result))
                    {
                        response = _tsTrainingAndCompetancyTypeService.Delete(filteredTSTypes, CompCertTrainingType.Co, true, false);
                    }

                    //Check for any valid Updated records are present and update
                    var valUpdRes = _tsTrainingAndCompetancyTypeService.IsRecordValidForProcess(tsCompetencyTypes, ValidationType.Update, CompCertTrainingType.Co, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                    if (Convert.ToBoolean(valUpdRes.Result) && (response == null || Convert.ToBoolean(response.Result)))
                    {
                        response = _tsTrainingAndCompetancyTypeService.Modify(filteredTSTypes, CompCertTrainingType.Co, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, true, false);
                    }

                    //Check for any valid New  records are present and Add
                    var valAddRes = _tsTrainingAndCompetancyTypeService.IsRecordValidForProcess(tsCompetencyTypes, ValidationType.Add, CompCertTrainingType.Co, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                    if (Convert.ToBoolean(valAddRes.Result) && (response == null || Convert.ToBoolean(response.Result)))
                    {
                        response = _tsTrainingAndCompetancyTypeService.Add(filteredTSTypes, CompCertTrainingType.Co, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, true, false);
                    }

                    if (response!=null && response.Result != null && !Convert.ToBoolean(response.Result))
                    {
                        validationMessages = validationMessages ?? new List<ValidationMessage>();
                        validationMessages.AddRange(response.ValidationMessages);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
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
        #endregion

    }
}
