using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Validations;
using Evolution.Visit.Domain.Interfaces.Visits;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Core.Services
{
    public class VisitNotesService : IVisitNoteService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitNotesService> _logger = null;
        private readonly IVisitNotesRepository _repository = null;
        private readonly IVisitNoteValidationService _validationService = null;
        private readonly IVisitService _visitService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public VisitNotesService(IAppLogger<VisitNotesService> logger,
                                    IVisitNotesRepository visitNoteRepository,
                                    IVisitNoteValidationService validationService,
                                    IVisitService visitService,
                                    IMapper mapper,
                                    JObject messages,
                                    IAuditLogger auditLogger,
                                   IAuditSearchService auditSearchService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = visitNoteRepository;
            this._validationService = validationService;
            this._visitService = visitService;
            this._messages = messages;
            this._auditSearchService = auditSearchService;
        }


        public Response Get(DomainModel.VisitNote searchModel)
        {
            IList<DomainModel.VisitNote> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope())
                {
                    result = this._repository.Search(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Add(IList<DomainModel.VisitNote> visitNotes,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitNotes?.ToList().ForEach(x => { x.VisitId = visitId; });

            IList<DbModel.VisitNote> dbVisitNotes = null;
            IList<DbModel.Visit> dbVisits = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return AddVisitNote(visitNotes,
                                     ref dbVisitNotes,
                                     ref dbVisits,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.VisitNote> visitNotes,
                            ref IList<DbModel.VisitNote> dbVisitNotes,
                            ref IList<DbModel.Visit> dbVisits,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitNotes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddVisitNote(visitNotes,
                                     ref dbVisitNotes,
                                     ref dbVisits,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequire);
        }

       //D661 issue 8 Start
        public Response Update(IList<DomainModel.VisitNote> visitNotes,
                            ref IList<DbModel.VisitNote> dbVisitNotes,
                            ref IList<DbModel.Visit> dbVisits,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitNotes?.ToList().ForEach(x => { x.VisitId = visitId; });
            return ModifyVisitNote(visitNotes,
                                     ref dbVisitNotes,
                                     ref dbVisits,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequire);
        }
       //D661 issue 8 End


        public Response Delete(IList<DomainModel.VisitNote> visitNotes,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.VisitNote> dbVisitNotes = null;
            if (visitId.HasValue)
                visitNotes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveVisitNotes(visitNotes,
                                                  ref dbVisitNotes,
                                                  ref dbVisit,
                                                   commitChange,
                                                  isDbValidationRequired);
        }



        public Response Delete(IList<DomainModel.VisitNote> visitNotes,
                               ref IList<DbModel.VisitNote> dbVisitNotes,
                               ref IList<DbModel.Visit> dbVisits,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            if (visitId.HasValue)
                visitNotes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveVisitNotes(visitNotes,
                                             ref dbVisitNotes,
                                             ref dbVisits,
                                             commitChange,
                                             isDbValidationRequired);
        }


        public Response IsRecordValidForProcess(IList<DomainModel.VisitNote> visitNotes,
                                                ValidationType validationType)
        {
            IList<DbModel.VisitNote> dbVisitNotes = null;
            IList<DbModel.Visit> dbVisit = null;

            return IsRecordValidForProcess(visitNotes,
                                            validationType,
                                            ref dbVisitNotes,
                                            ref dbVisit
                                           );
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitNote> visitNotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitNote> dbVisitNotes,
                                                ref IList<DbModel.Visit> dbVisits)
        {
            IList<DomainModel.VisitNote> filteredVisitNotes = null;
            return IsRecordValidForProcess(visitNotes,
                                            validationType,
                                            ref filteredVisitNotes,
                                            ref dbVisitNotes,
                                            ref dbVisits
                                           );
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitNote> visitNotes,
                                                ValidationType validationType,
                                                IList<DbModel.VisitNote> dbVisitNotes,
                                                ref IList<DbModel.Visit> dbVisits)
        {
            return IsRecordValidForProcess(visitNotes,
                                            validationType,
                                            ref dbVisitNotes,
                                            ref dbVisits
                                           );
        }
        private IList<DomainModel.VisitNote> FilterRecord(IList<DomainModel.VisitNote> visitNotes,
                                                                 ValidationType filterType)
        {
            IList<DomainModel.VisitNote> filteredVisitNotes = null;

            if (filterType == ValidationType.Add)
                filteredVisitNotes = visitNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredVisitNotes = visitNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredVisitNotes = visitNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredVisitNotes;
        }

        private bool IsValidPayload(IList<DomainModel.VisitNote> visitNotes,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(visitNotes), validationType);
            if (validationResults?.Count > 0)
                validationMessages.Add(_messages, ModuleType.Visit, validationResults);

            return validationMessages?.Count <= 0;
        }


        private bool IsRecordValidForAdd(IList<DomainModel.VisitNote> visitNotes,
                                        ref IList<DbModel.VisitNote> dbVisitNotes,
                                        ref IList<DbModel.Visit> dbVisits,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            bool result = true;

            var assignmentIds = visitNotes.Select(x => (long)x.VisitId).Distinct().ToList();

            if (!_visitService.IsValidVisit(assignmentIds, ref dbVisits, ref validationMessages, null))
                result = false;
            return result;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.VisitNote> visitNotes,
                                       ValidationType validationType,
                                       ref IList<DomainModel.VisitNote> filteredVisitNotes,
                                       ref IList<DbModel.VisitNote> dbVisitNotes,
                                       ref IList<DbModel.Visit> dbVisits)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (visitNotes != null && visitNotes.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredVisitNotes == null || filteredVisitNotes.Count <= 0)
                        filteredVisitNotes = FilterRecord(visitNotes, validationType);
                    if (dbVisitNotes == null || dbVisitNotes.Count <= 0) //D661 issue 8
                        dbVisitNotes = GetVisitNotes(filteredVisitNotes, validationType);

                    if (filteredVisitNotes?.Count > 0 && IsValidPayload(filteredVisitNotes, validationType, ref validationMessages))
                    {
                        if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredVisitNotes,
                                                         ref dbVisitNotes,
                                                         ref dbVisits,
                                                         ref validationMessages);
                        if(validationType == ValidationType.Update)       //D661 issue 8 
                            result = IsRecordValidForAdd(filteredVisitNotes,
                                                        ref dbVisitNotes,
                                                        ref dbVisits,
                                                        ref validationMessages);
                    }
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response AddVisitNote(IList<DomainModel.VisitNote> visitNotes,
                                           ref IList<DbModel.VisitNote> dbVisitNotes,
                                           ref IList<DbModel.Visit> dbVisits,
                                           IList<DbModel.SqlauditModule> dbModule,
                                           bool commitChange,
                                           bool isDbValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(visitNotes, ValidationType.Add);
                eventId = visitNotes?.FirstOrDefault().EventId;
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(visitNotes,
                                                           ValidationType.Add,
                                                           ref recordToBeAdd,
                                                           ref dbVisitNotes,
                                                           ref dbVisits);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    var dbNotes = _mapper.Map<IList<DbModel.VisitNote>>(recordToBeAdd);
                    _repository.Add(dbNotes);
                    if (commitChange)
                    {
                       var value= _repository.ForceSave();
                        if (value > 0)
                        {
                            dbNotes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, visitNotes.FirstOrDefault().ActionByUser, null,
                                                                                        ValidationType.Add.ToAuditActionType(),
                                                                                        SqlAuditModuleType.VisitNote,
                                                                                        null,
                                                                                        _mapper.Map<DomainModel.VisitNote>(x1),
                                                                                        dbModule));
                        }
                    }
                        
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
       //D661 issue 8 Start
        private Response ModifyVisitNote(IList<DomainModel.VisitNote> visitNotes,
                                          ref IList<DbModel.VisitNote> dbVisitNotes,
                                          ref IList<DbModel.Visit> dbVisits,
                                          IList<DbModel.SqlauditModule> dbModule,
                                          bool commitChange,
                                          bool isDbValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                List<MessageDetail> errorMessages = null;
                var recordToBeModify = FilterRecord(visitNotes, ValidationType.Update);
                eventId = visitNotes?.FirstOrDefault().EventId;
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(visitNotes,
                                                           ValidationType.Update,
                                                           ref recordToBeModify,
                                                           ref dbVisitNotes,
                                                           ref dbVisits);
                else if (dbVisitNotes?.Count <= 0)
                    dbVisitNotes = GetVisitNotes(recordToBeModify, ValidationType.Update);

                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeModify?.Count > 0)
                {
                    if(dbVisitNotes?.Count > 0)
                    {
                        if (this.IsRecordUpdateCountMatching(recordToBeModify, dbVisitNotes, ref errorMessages))
                        {
                            dbVisitNotes.ToList().ForEach(dbVisitNotesValue =>
                            {
                                var dbVisitNoteToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitNoteId == dbVisitNotesValue.Id);
                                if(dbVisitNoteToBeModify != null)
                                {
                                    dbVisitNotesValue.VisitId = (long)dbVisitNoteToBeModify.VisitId;
                                    dbVisitNotesValue.Id = (int)dbVisitNoteToBeModify.VisitNoteId;
                                    dbVisitNotesValue.CreatedBy = dbVisitNoteToBeModify.CreatedBy;
                                    dbVisitNotesValue.Note = dbVisitNoteToBeModify.Note;
                                    dbVisitNotesValue.CreatedDate = DateTime.UtcNow;
                                    dbVisitNotesValue.LastModification = DateTime.UtcNow;
                                    dbVisitNotesValue.UpdateCount = dbVisitNoteToBeModify.UpdateCount.CalculateUpdateCount();
                                    dbVisitNotesValue.ModifiedBy = dbVisitNoteToBeModify.ModifiedBy;
                                    dbVisitNotesValue.IsSpecialistVisible = dbVisitNoteToBeModify.VisibleToSpecialist;
                                    dbVisitNotesValue.IsCustomerVisible = dbVisitNoteToBeModify.VisibleToCustomer;
                                }
                            });
                            _repository.AutoSave = false;
                            _repository.Update(dbVisitNotes);
                            if (commitChange && recordToBeModify?.Count > 0)
                                _repository.ForceSave();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }
       //D661 issue 8 end
        private Response RemoveVisitNotes(IList<DomainModel.VisitNote> visitNotes,
                                         ref IList<DbModel.VisitNote> dbVisitNotes,
                                         ref IList<DbModel.Visit> dbVisit,
                                         bool commitChange,
                                         bool isDbValidationRequired)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(visitNotes, ValidationType.Delete);
                eventId = visitNotes?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(visitNotes,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbVisitNotes,
                                                       ref dbVisit
                                                       );

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequired || (Convert.ToBoolean(response.Result)) && dbVisitNotes?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbVisitNotes);
                        if (commitChange)
                        {
                           var delCont= _repository.ForceSave();
                            if (delCont > 0)
                            {
                                 recordToBeDelete?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser, null,
                                                                                                      ValidationType.Delete.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.VisitNote,
                                                                                                        x1,
                                                                                                       null));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitNotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
       //D661 issue 8 Start
        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitNote> visitNotes, IList<DbModel.VisitNote> dbVisitNote, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = visitNotes.Where(x => !dbVisitNote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.VisitNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Visit, errorCode, string.Format(_messages[errorCode].ToString(), x.VisitNoteId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private IList<DbModel.VisitNote> GetVisitNotes(IList<DomainModel.VisitNote> visitNotes,
                                                        ValidationType validationType)
        {
            IList<DbModel.VisitNote> dbVisitNotes = null;
            if (validationType != ValidationType.Add)
            {
                if (visitNotes?.Count > 0)
                {
                    var timesheetNoteId = visitNotes.Select(x => x.VisitNoteId).Distinct().ToList();
                    dbVisitNotes = _repository.FindBy(x => timesheetNoteId.Contains(x.Id)).ToList();
                }
            }
            return dbVisitNotes;
        }
        //D661 issue 8 End
    }
}
