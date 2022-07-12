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
    public class TechnicalSpecialistNoteService : ITechnicalSpecialistNoteService
    { 
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistNoteService> _logger = null; 
        private readonly ITechnicalSpecialistNoteValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ITechnicalSpecialistNoteRepository  _tsNoteRepository = null;

        #region Constructor

        public TechnicalSpecialistNoteService(IMapper mapper,
                                            JObject messages,
                                            IAppLogger<TechnicalSpecialistNoteService> logger,
                                            ITechnicalSpecialistNoteValidationService validationService,
                                            //ITechnicalSpecialistService technSpecServices,
                                            ITechnicalSpecialistRepository technicalSpecialistRepository,
                                            ITechnicalSpecialistNoteRepository tsNoteRepository,
                                          IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _messages = messages;
            _logger = logger; 
            _validationService = validationService;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _tsNoteRepository = tsNoteRepository;
            _auditSearchService = auditSearchService;
        }

        #endregion
         

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistNoteInfo searchModel)
        {
            IList<TechnicalSpecialistNoteInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistNoteInfo>>(_tsNoteRepository.Search(searchModel));
                
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int>epin, IList<string> recordType)
        {
            IList<TechnicalSpecialistNoteInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistNoteInfo>>(_tsNoteRepository.Search(recordType, epin));

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), result);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<long> tsNoteIds)
        {
            IList<TechnicalSpecialistNoteInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistNoteInfo>>(GetTsNoteById(tsNoteIds));
               
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsNoteIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(NoteType noteType, IList<string> tsPins = null, IList<int> recordRefId = null)
        {
            IList<TechnicalSpecialistNoteInfo> result = null;
            Exception exception = null;

            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistNoteInfo>>(_tsNoteRepository.Get(noteType, tsPins, recordRefId));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            } 

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
         
        public Response Get(NoteType noteType,bool fetchLatest, IList<string> tsPins = null, IList<int> recordRefId = null)
        {
            IList<TechnicalSpecialistNoteInfo> result = null;
            Exception exception = null;

            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistNoteInfo>>(_tsNoteRepository.Get(noteType, fetchLatest, tsPins, recordRefId));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTSPin(IList<string> tsPins)
        {
            IList<TechnicalSpecialistNoteInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistNoteInfo>>(GetTsNoteByPin(tsPins)); 
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

        public Response Add(IList<TechnicalSpecialistNoteInfo> tsNotes, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistNote> dbTsNotes = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null; 
            return AddTechSpecialistNote(tsNotes, ref dbTsNotes,ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistNoteInfo> tsNotes, ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechSpecialistNote(tsNotes, ref dbTsNotes,ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }
       //D661 issue 8 Start
        public Response Update(IList<TechnicalSpecialistNoteInfo> tsNotes, ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return ModifyTechSpecialistNote(tsNotes, ref dbTsNotes, ref dbTechnicalSpecialists, commitChange, isDbValidationRequired);
        }
       //D661 issue 8 End
 

        #endregion

        #region Validations 

        public Response IsRecordExistInDb(IList<long> tsNoteIds,
            ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
            ref IList<ValidationMessage> validationMessages)
        {
            return IsRecordExistInDb(tsNoteIds, ref dbTsNotes, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<long> tsNoteIds,
            ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
            ref IList<long> tsNoteIdNotExists,
            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsNotes == null && tsNoteIds?.Count > 0)
                    dbTsNotes = GetTsNoteById(tsNoteIds);

                result = IsTsCertificationExistInDb(tsNoteIds, dbTsNotes, ref tsNoteIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsNoteIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
  
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType validationType)
        {  
            IList<DbModel.TechnicalSpecialistNote> dbTsNotes = null;
            return IsRecordValidForProcess(tsNotes, validationType, ref dbTsNotes);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType validationType, ref IList<TechnicalSpecialistNoteInfo> filteredTSNotes, ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            return CheckRecordValidForProcess(tsNotes, validationType, ref filteredTSNotes, ref dbTsNotes,ref dbTechnicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType validationType,  ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes)
        {
            IList<TechnicalSpecialistNoteInfo> filteredTSNotes = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            return CheckRecordValidForProcess(tsNotes, validationType, ref filteredTSNotes, ref dbTsNotes, ref dbTechnicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType validationType, IList<DbModel.TechnicalSpecialistNote> dbTsNotes)
        {
            return IsRecordValidForProcess(tsNotes, validationType, ref dbTsNotes);
        }

        #endregion

        #endregion

        #region Private Methods 

        private IList<DbModel.TechnicalSpecialistNote> GetTsNoteByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistNote> dbTsNotes = null;
            if (pins?.Count > 0)
            {
                dbTsNotes = _tsNoteRepository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsNotes;
        }

        private IList<DbModel.TechnicalSpecialistNote> GetTsNoteById(IList<long> tsNoteIds)
        {
            IList<DbModel.TechnicalSpecialistNote> dbTsCertifications = null;
            if (tsNoteIds?.Count > 0)
                dbTsCertifications = _tsNoteRepository.FindBy(x => tsNoteIds.Contains(x.Id)).ToList();

            return dbTsCertifications;
        }

        private Response AddTechSpecialistNote(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                   ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                   bool commitChange = true,
                                   bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            List<MessageDetail> errorMessages = null;
            long? eventId = 0;
        
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistNoteInfo> recordToBeAdd = null;
                eventId = tsNotes?.FirstOrDefault()?.EventId;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsNotes, ValidationType.Add, ref recordToBeAdd, ref dbTsNotes,ref dbTechnicalSpecialists);
                }

                if (!isDbValidationRequired && tsNotes?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsNotes, ValidationType.Add);
                }

                if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    _tsNoteRepository.AutoSave = false;
                    
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistNote>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                    });

                    _tsNoteRepository.Add(mappedRecords);

                    if (commitChange && !_tsNoteRepository.AutoSave && recordToBeAdd?.Count > 0 && errorMessages.Count <= 0)
                    {
                        var savedCnt = _tsNoteRepository.ForceSave();
                        dbTsNotes = mappedRecords;


                        if (mappedRecords?.Count > 0)
                        {
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsNotes.FirstOrDefault().ActionByUser,
                                                                                                null,
                                                                                               ValidationType.Add.ToAuditActionType(),
                                                                                               SqlAuditModuleType.TechnicalSpecialistNote,
                                                                                                 null,
                                                                                                 _mapper.Map<TechnicalSpecialistNoteInfo>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsNotes);
            }
            finally
            {
                _tsNoteRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
       //D661 issue 8 Start
        private Response ModifyTechSpecialistNote(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                 ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                 ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                 bool commitChange = true,
                                 bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            List<MessageDetail> errorMessages = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistNoteInfo> recordToBeModify = null;
                eventId = tsNotes?.FirstOrDefault()?.EventId;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                recordToBeModify = FilterRecord(tsNotes, ValidationType.Update);
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsNotes, ValidationType.Update, ref recordToBeModify, ref dbTsNotes, ref dbTechnicalSpecialists);
                }
                if ((!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result)) && recordToBeModify?.Count > 0)
                {
                    dbTsNotes = GetComments(recordToBeModify, ValidationType.Update);
                    if (dbTsNotes?.Count > 0)
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        if (this.IsRecordUpdateCountMatching(recordToBeModify, dbTsNotes, ref errorMessages))
                        {
                            dbTsNotes.ToList().ForEach(dbTechnicalSpecialistNote =>
                            {
                                var dbTsNoteToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == dbTechnicalSpecialistNote.Id);
                                if (dbTsNoteToBeModify != null)
                                {
                                    dbTechnicalSpecialistNote.TechnicalSpecialistId = dbTsNoteToBeModify.Epin;
                                    dbTechnicalSpecialistNote.Id = dbTsNoteToBeModify.Id;
                                    dbTechnicalSpecialistNote.CreatedBy = dbTsNoteToBeModify.CreatedBy;
                                    dbTechnicalSpecialistNote.Note = dbTsNoteToBeModify.Note;
                                    dbTechnicalSpecialistNote.CreatedDate = DateTime.UtcNow;
                                    dbTechnicalSpecialistNote.LastModification = DateTime.UtcNow;
                                    dbTechnicalSpecialistNote.UpdateCount = dbTsNoteToBeModify.UpdateCount.CalculateUpdateCount();
                                    dbTechnicalSpecialistNote.ModifiedBy = dbTsNoteToBeModify.ModifiedBy;
                                }

                            });
                            _tsNoteRepository.AutoSave = false;
                            _tsNoteRepository.Update(dbTsNotes);
                            if (commitChange && recordToBeModify?.Count > 0)
                                _tsNoteRepository.ForceSave();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }
       //D661 issue 8 End
        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                    ValidationType validationType,
                                    ref IList<TechnicalSpecialistNoteInfo> filteredTsNotes,
                                    ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {        //D661 issue 8 
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsNotes, ref filteredTsNotes, ref dbTsNotes, ref dbTechnicalSpecialists, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsNotes, ref filteredTsNotes, ref dbTsNotes, ref dbTechnicalSpecialists, ref validationMessages);

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistNoteInfo> tsNotes,
                             ref IList<TechnicalSpecialistNoteInfo> filteredTsNotes,
                             ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                             ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsNotes != null && tsNotes.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsNotes == null || filteredTsNotes.Count <= 0)
                    filteredTsNotes = FilterRecord(tsNotes, validationType);

                if (filteredTsNotes?.Count > 0 && IsValidPayload(filteredTsNotes, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsNotes.Select(x => x.Epin.ToString()).ToList(); 

                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                }
            }
            return result;
        }
       //D661 issue 8 Start
        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistNoteInfo> tsNotes,
                              ref IList<TechnicalSpecialistNoteInfo> filteredTsNotes,
                             ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                             ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsNotes != null && tsNotes.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsNotes == null || filteredTsNotes.Count <= 0)
                    filteredTsNotes = FilterRecord(tsNotes, validationType);

                if (filteredTsNotes?.Count > 0 && IsValidPayload(filteredTsNotes, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsNotes.Select(x => x.Epin.ToString()).ToList();

                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                }
            }
            return result;
        }
       //D661 issue 8 End
        private IList<TechnicalSpecialistNoteInfo> FilterRecord(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType filterType)
        {
            IList<TechnicalSpecialistNoteInfo> filterTsCertifications = null;

            if (filterType == ValidationType.Add)
                filterTsCertifications = tsNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsCertifications = tsNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsCertifications = tsNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsCertifications;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsNotes), validationType);

            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.TechnicalSpecialist, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private bool IsTsCertificationExistInDb(IList<long> tsNoteIds,
                                        IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                        ref IList<long> tsNoteIdNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsNotes == null)
                dbTsNotes = new List<DbModel.TechnicalSpecialistNote>();

            var validMessages = validationMessages;

            if (tsNoteIds?.Count > 0)
            {
                tsNoteIdNotExists = tsNoteIds.Where(id => !dbTsNotes.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsNoteIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsNoteIdDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
       //D661 issue 8 Start
        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistNoteInfo> tsNotes, IList<DbModel.TechnicalSpecialistNote> dbTSNote, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = tsNotes.Where(x => !dbTSNote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Visit, errorCode, string.Format(_messages[errorCode].ToString(), x.Id)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private IList<DbModel.TechnicalSpecialistNote> GetComments(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistNote> dbComments = null;
            if (validationType != ValidationType.Add)
            {
                if (tsNotes?.Count > 0)
                {
                    var tsNotesId = tsNotes.Select(x => x.Id).Distinct().ToList();
                    dbComments = _tsNoteRepository.FindBy(x => tsNotesId.Contains(x.Id)).ToList();
                }
            }
            return dbComments;
        }
        //D661 issue 8 End

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
    }
}
