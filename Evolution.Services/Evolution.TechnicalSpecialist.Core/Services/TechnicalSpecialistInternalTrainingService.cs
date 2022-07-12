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
    public class TechnicalSpecialistInternalTrainingService : ITechnicalSpecialistInternalTrainingService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistInternalTrainingService> _logger = null;
        private readonly ITechnicalSpecialistTrainingAndCompetencyRepository _tsTrainingAndCompetencyRepository = null;
        private readonly ITechnicalSpecialistInternalTrainingValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IDocumentService _documentService = null;
        private readonly ITechnicalSpecialistNoteService _tsNoteService = null;
        private readonly ITechnicalSpecialistTrainingAndCompetancyTypeService _tsTrainingAndCompetancyTypeService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor

        public TechnicalSpecialistInternalTrainingService(IMapper mapper,
                                                            JObject messages,
                                                            IAppLogger<TechnicalSpecialistInternalTrainingService> logger,
                                                            ITechnicalSpecialistTrainingAndCompetencyRepository tsTrainingAndCompetencyRepository,
                                                            ITechnicalSpecialistInternalTrainingValidationService validationService,
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

        public Response Add(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return AddTechSpecialistInternalTraining(tsInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialistInternalTraining(tsInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete 

        public Response Delete(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTraining = null;
            return RemoveTechSpecialistInternalTraining(tsInternalTrainings, ref dbTsInternalTraining, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistInternalTraining(tsInternalTrainings, ref dbTsInternalTrainings, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Get

        public Response Get(TechnicalSpecialistInternalTraining searchModel)
        {
            IList<TechnicalSpecialistInternalTraining> result = null;
            Exception exception = null;
            try
            {
                var tsInternalTrainingResult = _mapper.Map<IList<TechnicalSpecialistInternalTraining>>(_tsTrainingAndCompetencyRepository.Search(searchModel));
                var tsInternalTrainingDocResult = PopulateTsInternalTrainingDocuments(tsInternalTrainingResult);
                result = PopulateTsInternalTrainingNotes(tsInternalTrainingDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> tsIntTrainingIds)
        {
            IList<TechnicalSpecialistInternalTraining> result = null;
            Exception exception = null;
            try
            {
                var tsIntTrainingResult = _mapper.Map<IList<TechnicalSpecialistInternalTraining>>(GetTsInternalTrainingById(tsIntTrainingIds));
                var tsInternalTrainingDocResult = PopulateTsInternalTrainingDocuments(tsIntTrainingResult);
                result = PopulateTsInternalTrainingNotes(tsInternalTrainingDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIntTrainingIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> intTrainingNames)
        {
            IList<TechnicalSpecialistInternalTraining> result = null;
            Exception exception = null;
            try
            {
                var tsIntTrainingResult = _mapper.Map<IList<TechnicalSpecialistInternalTraining>>(GetTsInternalTrainingByNames(intTrainingNames));
                var tsInternalTrainingDocResult = PopulateTsInternalTrainingDocuments(tsIntTrainingResult);
                result = PopulateTsInternalTrainingNotes(tsInternalTrainingDocResult);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), intTrainingNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<TechnicalSpecialistInternalTraining> result = null;
            Exception exception = null;
            try
            {
                var tsIntTrainingResult = _mapper.Map<IList<TechnicalSpecialistInternalTraining>>(GetTsInternalTrainingByPin(tsPins));
                var tsInternalTrainingDocResult = PopulateTsInternalTrainingDocuments(tsIntTrainingResult);
                result = PopulateTsInternalTrainingNotes(tsInternalTrainingDocResult);
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

        public Response IsRecordExistInDb(IList<int> tsInternalTrainingIds, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsInternalTrainingIdNotExists = null;
            return IsRecordExistInDb(tsInternalTrainingIds, ref dbTsInternalTrainings, ref tsInternalTrainingIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsInternalTrainingIds, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<int> tsInternalTrainingIdNotExists, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsInternalTrainings == null && tsInternalTrainingIds?.Count > 0)
                    dbTsInternalTrainings = GetTsInternalTrainingById(tsInternalTrainingIds);

                result = IsTsInternalTrainingExistInDb(tsInternalTrainingIds, dbTsInternalTrainings, ref tsInternalTrainingIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainingIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            IList<TechnicalSpecialistInternalTraining> filteredTSInternalTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return IsRecordValidForProcess(tsInternalTrainings, validationType, ref filteredTSInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ValidationType validationType, ref IList<TechnicalSpecialistInternalTraining> filteredTSInternalTrainings, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool isDraft = false)
        {
            return CheckRecordValidForProcess(tsInternalTrainings, validationType, ref filteredTSInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool isDraft = false)
        {
            IList<TechnicalSpecialistInternalTraining> filteredTSInternalTrainings = null;

            return IsRecordValidForProcess(tsInternalTrainings, validationType, ref filteredTSInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return IsRecordValidForProcess(tsInternalTrainings, validationType, ref dbTsInternalTrainings, ref dbTechnicalSpecialists);
        }

        #endregion

        #region Modify 

        public Response Modify(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return UpdateTechSpecialistInternalTraining(tsInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistInternalTraining(tsInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        #endregion

        #endregion

        #region Private Methods 

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTsInternalTrainingByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            if (pins?.Count > 0)
            {
                dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsInternalTrainings;
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTsInternalTrainingById(IList<int> tsInternalTrainingIds)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            if (tsInternalTrainingIds?.Count > 0)
                dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.FindBy(x => tsInternalTrainingIds.Contains(x.Id)).ToList();

            return dbTsInternalTrainings;
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTsInternalTrainingByNames(IList<string> tsInternalTrainingNames)
        {
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings = null;
            if (tsInternalTrainingNames?.Count > 0)
                dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.FindBy(x => x.TechnicalSpecialistTrainingAndCompetencyType.Any(x1 => tsInternalTrainingNames.Contains(x1.TrainingOrCompetencyData.Name))).ToList();

            return dbTsInternalTrainings;
        }


        private Response AddTechSpecialistInternalTraining(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                   ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                   bool commitChange = true,
                                   bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistInternalTraining> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;

                eventId = tsInternalTrainings?.FirstOrDefault()?.EventId;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsInternalTrainings, ValidationType.Add, ref recordToBeAdd, ref dbTsInternalTrainings, ref dbTechnicalSpecialists);
                }

                if (!isDbValidationRequired && tsInternalTrainings?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsInternalTrainings, ValidationType.Add);
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
                    mappedRecords?.Select(y => { y.IsIlearn = false; return y; })?.ToList();//ILearn
                    _tsTrainingAndCompetencyRepository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _tsTrainingAndCompetencyRepository.ForceSave();
                        dbTsInternalTrainings = mappedRecords;
                        if (recordToBeAdd?.Count > 0)
                        {
                            var internalTrainingTypeRes = ProcessTsInternalTrainingTypes(recordToBeAdd, dbTsInternalTrainings, ref validationMessages);
                            var noteRes = ProcessTsInternalTrainingNotes(recordToBeAdd, dbTsInternalTrainings, ValidationType.Add, ref validationMessages);
                            var docRes = ProcessTsInternalTrainingDocuments(recordToBeAdd, mappedRecords, ref validationMessages);

                            int i = 0;
                            recordToBeAdd?.ToList().ForEach(x1 =>
                            {
                                var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                if (newDocuments != null && newDocuments.Count > 0)
                                    x1.DocumentName = string.Join(",", newDocuments);

                                x1.Id = mappedRecords[i++].Id;// def1035 test 2
                                _auditSearchService.AuditLog(x1, ref eventId, tsInternalTrainings?.FirstOrDefault().ActionByUser,
                                                           null,
                                                            ValidationType.Add.ToAuditActionType(),
                                                            SqlAuditModuleType.TechnicalSpecialistInternalTraining,
                                                              null,
                                                              x1);
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }
            finally
            {
                _tsTrainingAndCompetencyRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistInternalTraining(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                   ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                   bool commitChange = true,
                                   bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;

            IList<TechnicalSpecialistInternalTraining> recordToBeModify = null;
            long? eventId = 0;

            Response valdResponse = null;

            try
            {
                validationMessages = validationMessages ?? new List<ValidationMessage>();
                eventId = tsInternalTrainings?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsInternalTrainings, ValidationType.Update, ref recordToBeModify, ref dbTsInternalTrainings, ref dbTechnicalSpecialists);
                }

                if (!isDbValidationRequired && tsInternalTrainings?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsInternalTrainings, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTsInternalTrainings?.Count <= 0)
                    {
                        dbTsInternalTrainings = _tsTrainingAndCompetencyRepository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists?.Count <= 0)
                    {
                        //valdResponse = _technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                        valdResponse = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if ((valdResponse == null || Convert.ToBoolean(valdResponse.Result)) && dbTsInternalTrainings?.Count > 0)
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        IList<TechnicalSpecialistInternalTraining> domExsistanceInternalTraining = new List<TechnicalSpecialistInternalTraining>();
                        TechnicalSpecialistInternalTraining technicalSpecialistInternalTraining = new TechnicalSpecialistInternalTraining();

                        dbTsInternalTrainings.ToList().ForEach(tsCertInfo =>
                        {

                            technicalSpecialistInternalTraining = _mapper.Map<TechnicalSpecialistInternalTraining>(tsCertInfo);
                            var oldDocuments = recordToBeModify?.FirstOrDefault(x => x.Id == tsCertInfo.Id)?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.DocumentName)?.ToList();
                            if (oldDocuments != null && oldDocuments.Count > 0)
                                technicalSpecialistInternalTraining.DocumentName = string.Join(",", oldDocuments);

                            domExsistanceInternalTraining.Add(ObjectExtension.Clone(technicalSpecialistInternalTraining));
                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsCertInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsCertInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                });
                                tsCertInfo.LastModification = DateTime.UtcNow;
                                tsCertInfo.UpdateCount = tsCertInfo.UpdateCount.CalculateUpdateCount();
                                tsCertInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                        });
                        _tsTrainingAndCompetencyRepository.AutoSave = false;
                        _tsTrainingAndCompetencyRepository.Update(dbTsInternalTrainings);
                        if (commitChange)
                        {
                            var savedCnt = _tsTrainingAndCompetencyRepository.ForceSave();
                            if (recordToBeModify?.Count > 0 && savedCnt > 0)
                            {
                                var internalTrainingTypeRes = ProcessTsInternalTrainingTypes(recordToBeModify, dbTsInternalTrainings, ref validationMessages);
                                var noteRes = ProcessTsInternalTrainingNotes(recordToBeModify, dbTsInternalTrainings, ValidationType.Update, ref validationMessages);
                                var docRes = ProcessTsInternalTrainingDocuments(recordToBeModify, dbTsInternalTrainings, ref validationMessages);

                                recordToBeModify?.ToList().ForEach(x1 => {
                                    var newDocuments = x1?.Documents?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.Select(doc => doc.DocumentName)?.ToList();
                                    if (newDocuments != null && newDocuments.Count > 0)
                                        x1.DocumentName = string.Join(",", newDocuments);

                                    _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                              null,
                                                              ValidationType.Update.ToAuditActionType(),
                                                             SqlAuditModuleType.TechnicalSpecialistInternalTraining,
                                                             domExsistanceInternalTraining?.FirstOrDefault(x2 => x2.Id == x1.Id),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }
            finally
            {
                _tsTrainingAndCompetencyRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveTechSpecialistInternalTraining(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
              ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                                bool commitChange = true,
                                                bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<TechnicalSpecialistInternalTraining> recordToBeDeleted = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                eventId = tsInternalTrainings?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(tsInternalTrainings, ValidationType.Delete, ref dbTsInternalTrainings);

                if (tsInternalTrainings?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsInternalTrainings, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsInternalTrainings?.Count > 0)
                {
                    var dbTsIntTrainingToBeDeleted = dbTsInternalTrainings?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _tsTrainingAndCompetencyRepository.AutoSave = false;
                    _tsTrainingAndCompetencyRepository.Delete(dbTsIntTrainingToBeDeleted);
                    if (commitChange)
                        _tsTrainingAndCompetencyRepository.ForceSave();
                    if (recordToBeDeleted.Count > 0)
                    {
                        recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                               null,
                                                                                              ValidationType.Delete.ToAuditActionType(),
                                                                                              SqlAuditModuleType.TechnicalSpecialistInternalTraining,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }
            finally
            {
                _tsTrainingAndCompetencyRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistInternalTraining> filteredTsInternalTrainings,
                                            ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsInternalTrainings, ref filteredTsInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsInternalTrainings, ref filteredTsInternalTrainings, ref dbTsInternalTrainings, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsInternalTrainings, ref filteredTsInternalTrainings, ref dbTsInternalTrainings, ref dbTechnicalSpecialists, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsRecordValidForAdd(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                             ref IList<TechnicalSpecialistInternalTraining> filteredTsInternalTrainings,
                             ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                             ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsInternalTrainings != null && tsInternalTrainings.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInternalTrainings == null || filteredTsInternalTrainings.Count <= 0)
                    filteredTsInternalTrainings = FilterRecord(tsInternalTrainings, validationType);

                if (filteredTsInternalTrainings?.Count > 0 && IsValidPayload(filteredTsInternalTrainings, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsInternalTrainings.Select(x => x.Epin.ToString()).ToList();
                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                    ref IList<TechnicalSpecialistInternalTraining> filteredTsInternalTrainings,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                    ref IList<ValidationMessage> validationMessages,
                                    bool isDraft = false)
        {
            bool result = false;
            if (tsInternalTrainings != null && tsInternalTrainings.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInternalTrainings == null || filteredTsInternalTrainings.Count <= 0)
                    filteredTsInternalTrainings = FilterRecord(tsInternalTrainings, validationType);

                if (filteredTsInternalTrainings?.Count > 0 && IsValidPayload(filteredTsInternalTrainings, validationType, ref messages))
                {
                    GetTsInternalTrainingDbInfo(filteredTsInternalTrainings, ref dbTsInternalTrainings);
                    IList<int> tsInternalTrainingIds = filteredTsInternalTrainings.Select(x => x.Id).ToList();
                    IList<int> tsDBInternalTrainingIds = dbTsInternalTrainings.Select(x => x.Id).ToList();
                    if (tsInternalTrainingIds.Any(x => !tsDBInternalTrainingIds.Contains(x))) //Invalid TechSpecialist InternalTraining Id found.
                    {
                        var dbTsInternalTrainingInfosByIds = dbTsInternalTrainings;
                        var idNotExists = tsInternalTrainingIds.Where(id => !dbTsInternalTrainingInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistCertificateList = filteredTsInternalTrainings;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsCertificate = techSpecialistCertificateList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsCertificate, MessageType.TsInternalTrainingUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsInternalTrainings, dbTsInternalTrainings, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsInternalTrainings.Select(x => x.Epin.ToString()).ToList();
                            //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = Convert.ToBoolean(IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                    ref IList<TechnicalSpecialistInternalTraining> filteredTsInternalTrainings,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                    ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsInternalTrainings != null && tsInternalTrainings.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInternalTrainings == null || filteredTsInternalTrainings.Count <= 0)
                    filteredTsInternalTrainings = FilterRecord(tsInternalTrainings, validationType);

                if (filteredTsInternalTrainings?.Count > 0 && IsValidPayload(filteredTsInternalTrainings, validationType, ref validationMessages))
                {
                    GetTsInternalTrainingDbInfo(filteredTsInternalTrainings, ref dbTsInternalTrainings);
                    IList<int> tsInternalTrainingIdNotExists = null;
                    var tsTrainingIds = filteredTsInternalTrainings.Select(x => x.Id).Distinct().ToList();
                    result = IsTsInternalTrainingExistInDb(tsTrainingIds, dbTsInternalTrainings, ref tsInternalTrainingIdNotExists, ref validationMessages);
                    var internalTrainingTypeRes = ProcessTsInternalTrainingTypes(filteredTsInternalTrainings, dbTsInternalTrainings, ref validationMessages);
                    //if (result)
                    //{
                    //    RemoveTechSpecialistInternalTraining()
                    //}
                    if (result)
                        result = IsTechSpecialistInternalTrainingCanBeRemove(dbTsInternalTrainings, ref validationMessages);
                }
            }
            return result;
        }

        private IList<TechnicalSpecialistInternalTraining> FilterRecord(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ValidationType filterType)
        {
            IList<TechnicalSpecialistInternalTraining> filterTsInternalTrainings = null;

            if (filterType == ValidationType.Add)
                filterTsInternalTrainings = tsInternalTrainings?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInternalTrainings = tsInternalTrainings?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInternalTrainings = tsInternalTrainings?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInternalTrainings;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                      ValidationType validationType,
                      ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsInternalTrainings), validationType);

            if (tsInternalTrainings != null && tsInternalTrainings.Any(x => x.TechnicalSpecialistInternalTrainingTypes == null))
                messages.Add(_messages, "Internal Training", MessageType.TsInternalTrainingIdDoesNotExist);
            else if (tsInternalTrainings.SelectMany(x => x.TechnicalSpecialistInternalTrainingTypes).Any(x => string.IsNullOrEmpty(x.TypeName)))
                messages.Add(_messages, "Iternal Training", MessageType.TsInternalTrainingIdDoesNotExist);

            foreach (var item in tsInternalTrainings)
            {
                if (item.TrainingDate != null && item.Expiry != null)
                    if (item.TrainingDate > item.Expiry)
                        messages.Add(_messages, item.TrainingDate, MessageType.TsInternalTrainingExpiryDate, item.Expiry);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void GetTsInternalTrainingDbInfo(IList<TechnicalSpecialistInternalTraining> filteredTsInternalTrainings,
                                    ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings)
        {
            dbTsInternalTrainings = dbTsInternalTrainings ?? new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();
            IList<int> tsInternalTrainingIds = filteredTsInternalTrainings?.Select(x => x.Id).Distinct().ToList();
            if (tsInternalTrainingIds?.Count > 0 && (dbTsInternalTrainings.Count <= 0 || dbTsInternalTrainings.Any(x => !tsInternalTrainingIds.Contains(x.Id))))
            {
                var tsInternalTraings = GetTsInternalTrainingById(tsInternalTrainingIds);
                if (tsInternalTraings != null && tsInternalTraings.Any())
                {
                    dbTsInternalTrainings.AddRange(tsInternalTraings);
                }
            }

        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsInternalTrainings.Where(x => !dbTsInternalTrainings.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TsInternalTrainingUpdatedByOther);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTsInternalTrainingExistInDb(IList<int> tsInternalTrainingIds,
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                ref IList<int> tsInternalTrainingIdNotExists,
                                ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (dbTsInternalTrainings == null)
                dbTsInternalTrainings = new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();

            var validMessages = validationMessages;

            if (tsInternalTrainingIds?.Count > 0)
            {
                tsInternalTrainingIdNotExists = tsInternalTrainingIds.Where(id => !dbTsInternalTrainings.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsInternalTrainingIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsInternalTrainingIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialistInternalTrainingCanBeRemove(IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsInternalTrainings?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x, MessageType.TsInternalTrainingIsBeingUsed);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }


        private IList<TechnicalSpecialistInternalTraining> PopulateTsInternalTrainingDocuments(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings)
        {
            try
            {
                if (tsInternalTrainings?.Count > 0)
                {
                    var epins = tsInternalTrainings.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var internalTrainingIds = tsInternalTrainings.Select(x => x.Id.ToString()).Distinct().ToList();

                    var tsInternalTrainingDocs = _documentService.Get(ModuleCodeType.TS, epins, internalTrainingIds).Result?.Populate<IList<ModuleDocument>>();

                    if (tsInternalTrainingDocs?.Count > 0)
                    {
                        return tsInternalTrainings.GroupJoin(tsInternalTrainingDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsCert = tsc, doc }).Select(x =>
                            {
                                x.tsCert.Documents = x?.doc.Where(x1 => x1.DocumentType == DocumentType.TS_InternalTraining.ToString()).ToList();
                                return x.tsCert;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }

            return tsInternalTrainings;
        }

        private Response ProcessTsInternalTrainingDocuments(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tsInternalTrainings?.Count == dbTsInternalTrainings?.Count)
                {
                    //TODO : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                    for (int comCnt = 0; comCnt < tsInternalTrainings?.Count; comCnt++)
                    {
                        if (dbTsInternalTrainings[comCnt] != null && tsInternalTrainings[comCnt].Documents != null)
                        {
                            tsInternalTrainings[comCnt]?.Documents?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                {
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                    x1.ModifiedBy = tsInternalTrainings[comCnt]?.ModifiedBy;
                                }
                                x1.SubModuleRefCode = dbTsInternalTrainings[comCnt].Id.ToString();
                                x1.ModuleRefCode = tsInternalTrainings[comCnt]?.Epin.ToString();
                                x1.DocumentType = DocumentType.TS_InternalTraining.ToString();
                                x1.Status = x1.Status.Trim();
                            });
                        }
                    }

                    var tsDocToBeProcess = tsInternalTrainings?.Where(x => x.Documents != null &&
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistInternalTraining> PopulateTsInternalTrainingNotes(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings)
        {
            try
            {
                if (tsInternalTrainings?.Count > 0)
                {
                    var epins = tsInternalTrainings.Select(x => x.Epin.ToString()).Distinct().ToList();
                    var internalTrainingIds = tsInternalTrainings.Select(x => x.Id).Distinct().ToList();
                    var tsInternalTrainingNotes = _tsNoteService.Get(NoteType.ITD, true, epins, internalTrainingIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                    if (tsInternalTrainingNotes?.Count > 0)
                    {
                        return tsInternalTrainings.GroupJoin(tsInternalTrainingNotes,
                            tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                            note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                            (tsc, note) => new { tsCert = tsc, note }).Select(x =>
                            {
                                x.tsCert.Notes = x?.note?.FirstOrDefault()?.Note;//IGO def 922 
                                return x.tsCert;
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }

            return tsInternalTrainings;
        }

        private Response ProcessTsInternalTrainingNotes(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistNoteInfo> tsNewComptNotes = new List<TechnicalSpecialistNoteInfo>();
            try
            {
                if (tsInternalTrainings?.Count == dbTsInternalTrainings?.Count)
                {
                    if (validationType == ValidationType.Add)
                    {
                        //TODO : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                        for (int comCnt = 0; comCnt < tsInternalTrainings?.Count; comCnt++)
                        {
                            if (tsInternalTrainings[comCnt] != null && dbTsInternalTrainings[comCnt] != null)
                            {
                                tsNewComptNotes.Add(
                                new TechnicalSpecialistNoteInfo
                                {
                                    Epin = tsInternalTrainings[comCnt].Epin,
                                    RecordType = NoteType.ITD.ToString(),
                                    RecordRefId = dbTsInternalTrainings[comCnt].Id,
                                    Note = tsInternalTrainings[comCnt].Notes,
                                    RecordStatus = RecordStatus.New.FirstChar(),
                                    CreatedBy = tsInternalTrainings[comCnt].ActionByUser,
                                    CreatedDate = DateTime.UtcNow,
                                    EventId = tsInternalTrainings[comCnt].EventId,
                                    ActionByUser = tsInternalTrainings[comCnt].ActionByUser

                                });
                            }
                        }

                    }
                    else if (validationType == ValidationType.Update)
                    {
                        var epins = tsInternalTrainings.Select(x => x.Epin.ToString()).Distinct().ToList();
                        var competencyIds = tsInternalTrainings.Select(x => x.Id).Distinct().ToList();
                        var tsInternalTrainingNotes = _tsNoteService.Get(NoteType.ITD, true, epins, competencyIds).Result?.Populate<IList<TechnicalSpecialistNoteInfo>>();

                        tsNewComptNotes = tsInternalTrainingNotes.Join(tsInternalTrainings,
                             note => new { epin = note.Epin.ToString(), RefId = note.RecordRefId.ToString() },
                             tsc => new { epin = tsc.Epin.ToString(), RefId = tsc.Id.ToString() },
                             (note, tsc) => new { note, tsCert = tsc }).Where(x => !string.Equals(x.note.Note, x.tsCert.Notes)).Select(x =>
                             {
                                 x.note.Note = x.tsCert.Notes;
                                 x.note.RecordStatus = RecordStatus.New.FirstChar();
                                 x.note.CreatedBy = x.tsCert.ActionByUser;
                                 x.note.CreatedDate = DateTime.UtcNow;
                                 x.note.ActionByUser = x.tsCert.ActionByUser;
                                 x.note.EventId = x.tsCert.EventId;
                                 return x.note;
                             }).ToList();

                    }
                    return _tsNoteService.Add(tsNewComptNotes);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInternalTrainings);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessTsInternalTrainingTypes(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTSTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes = null;
            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys = null;
            IList<DbModel.Data> dbMasterIntTrainingAndCompetencys = null;
            try
            {

                if (tsInternalTrainings?.Count == dbTsInternalTrainings?.Count)
                {
                    //TODO : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                    for (int comCnt = 0; comCnt < tsInternalTrainings?.Count; comCnt++)
                    {
                        if (tsInternalTrainings[comCnt] != null && dbTsInternalTrainings[comCnt] != null)
                        {
                            tsInternalTrainings[comCnt].TechnicalSpecialistInternalTrainingTypes.ToList().ForEach(x => { x.TechnicalSpecialistInternalTrainingAndCompetencyId = dbTsInternalTrainings[comCnt].Id; x.ActionByUser = tsInternalTrainings[comCnt].ActionByUser; x.EventId = tsInternalTrainings[comCnt].EventId; });
                        }
                    }

                    IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsCompetencyTypes = tsInternalTrainings.SelectMany(x => x.TechnicalSpecialistInternalTrainingTypes).ToList();

                    //Check for any valid delete records are present and delete
                    var valRes = _tsTrainingAndCompetancyTypeService.IsRecordValidForProcess(tsCompetencyTypes, ValidationType.Delete, CompCertTrainingType.IT, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                    if (Convert.ToBoolean(valRes.Result))
                    {
                        valRes = _tsTrainingAndCompetancyTypeService.Delete(filteredTSTypes, CompCertTrainingType.Co, true, false);
                    }

                    //Check for any valid Updated records are present and update
                    valRes = _tsTrainingAndCompetancyTypeService.IsRecordValidForProcess(tsCompetencyTypes, ValidationType.Update, CompCertTrainingType.IT, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                    if (Convert.ToBoolean(valRes.Result))
                    {
                        valRes = _tsTrainingAndCompetancyTypeService.Modify(filteredTSTypes, CompCertTrainingType.Co, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, true, false);
                    }

                    //Check for any valid New  records are present and Add
                    valRes = _tsTrainingAndCompetancyTypeService.IsRecordValidForProcess(tsCompetencyTypes, ValidationType.Add, CompCertTrainingType.IT, ref filteredTSTypes, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys);
                    if (Convert.ToBoolean(valRes.Result))
                    {
                        valRes = _tsTrainingAndCompetancyTypeService.Add(filteredTSTypes, CompCertTrainingType.Co, ref dbTsIntTrainingAndCompetencyTypes, ref dbTsIntTrainingAndCompetencys, ref dbMasterIntTrainingAndCompetencys, true, false);
                    }


                    if (!Convert.ToBoolean(valRes.Result))
                    {
                        validationMessages = validationMessages ?? new List<ValidationMessage>();
                        validationMessages.AddRange(valRes.ValidationMessages);
                    }

                    return valRes;
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
