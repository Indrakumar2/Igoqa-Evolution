using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Core.Services
{
    public class TimesheetNoteService : ITimesheetNoteService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetNoteService> _logger = null;
        private readonly ITimesheetNoteRepository _repository = null;
        private readonly ITimesheetNoteValidationService _validationService = null;
        private readonly ITimesheetService _timesheetService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly JObject _messages = null;

        public TimesheetNoteService(IAppLogger<TimesheetNoteService> logger,
                                    ITimesheetNoteRepository timesheetNoteRepository,
                                    ITimesheetNoteValidationService validationService,
                                    ITimesheetService timesheetService,
                                    IMapper mapper,
                                    JObject messages,
                                     IAuditSearchService auditSearchService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = timesheetNoteRepository;
            this._validationService = validationService;
            this._timesheetService = timesheetService;
            this._messages = messages;
            this._auditSearchService = auditSearchService;
        }

        #region Public Methods
        public Response Get(DomainModel.TimesheetNote searchModel)
        {
            IList<DomainModel.TimesheetNote> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                          new TransactionOptions
                                          {
                                              IsolationLevel = IsolationLevel.ReadUncommitted
                                          }))
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

        public Response Add(IList<DomainModel.TimesheetNote> timesheetNotes,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            long? timesheetId = null)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            if (timesheetId.HasValue)
                timesheetNotes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            IList<DbModel.TimesheetNote> dbTimesheetNotes = null;
            IList<DbModel.Timesheet> dbTimesheets = null;
            return AddTimesheeetNote(timesheetNotes,
                                     ref dbTimesheetNotes,
                                     ref dbTimesheets,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.TimesheetNote> timesheetNotes,
                            ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                            ref IList<DbModel.Timesheet> dbTimesheets,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetNotes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTimesheeetNote(timesheetNotes,
                                     ref dbTimesheetNotes,
                                     ref dbTimesheets,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequire);
        }
        //D661 issue 8 Start
        public Response Update(IList<DomainModel.TimesheetNote> timesheetNotes,
                            ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                            ref IList<DbModel.Timesheet> dbTimesheets,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetNotes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return ModifyTimesheetNote(timesheetNotes,
                                     ref dbTimesheetNotes,
                                     ref dbTimesheets,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequire);
        }
        //D661 issue 8 End
        public Response Delete(IList<DomainModel.TimesheetNote> timesheetNotes,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? timesheetId = null)
        {
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.TimesheetNote> dbTimesheetNotes = null;
            if (timesheetId.HasValue)
                timesheetNotes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return this.RemoveTimesheetNotes(timesheetNotes,
                                                  ref dbTimesheetNotes,
                                                  ref dbTimesheet,
                                                   commitChange,
                                                  isDbValidationRequired);
        }



        public Response Delete(IList<DomainModel.TimesheetNote> timesheetNotes,
                               ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                               ref IList<DbModel.Timesheet> dbTimesheets,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetNotes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return this.RemoveTimesheetNotes(timesheetNotes,
                                             ref dbTimesheetNotes,
                                             ref dbTimesheets,
                                             commitChange,
                                             isDbValidationRequired);
        }


        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetNote> timesheetNotes,
                                                ValidationType validationType)
        {
            IList<DbModel.TimesheetNote> dbTimesheetNotes = null;
            IList<DbModel.Timesheet> dbTimesheet = null;

            return IsRecordValidForProcess(timesheetNotes,
                                            validationType,
                                            ref dbTimesheetNotes,
                                            ref dbTimesheet
                                           );
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetNote> timesheetNotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                                ref IList<DbModel.Timesheet> dbTimesheets)
        {
            IList<DomainModel.TimesheetNote> filteredTimesheetNotes = null;
            return IsRecordValidForProcess(timesheetNotes,
                                            validationType,
                                            ref filteredTimesheetNotes,
                                            ref dbTimesheetNotes,
                                            ref dbTimesheets
                                           );
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetNote> timesheetNotes,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                                ref IList<DbModel.Timesheet> dbTimesheets)
        {
            return IsRecordValidForProcess(timesheetNotes,
                                            validationType,
                                            ref dbTimesheetNotes,
                                            ref dbTimesheets
                                           );
        }

        #endregion

        #region Private Method
        private IList<DomainModel.TimesheetNote> FilterRecord(IList<DomainModel.TimesheetNote> timesheetNotes,
                                                                 ValidationType filterType)
        {
            IList<DomainModel.TimesheetNote> filteredTimesheetNotes = null;

            if (filterType == ValidationType.Add)
                filteredTimesheetNotes = timesheetNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTimesheetNotes = timesheetNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTimesheetNotes = timesheetNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTimesheetNotes;
        }

        private bool IsValidPayload(IList<DomainModel.TimesheetNote> timesheetNotes,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(timesheetNotes), validationType);
            if (validationResults?.Count > 0)
                validationMessages.Add(_messages, ModuleType.Timesheet, validationResults);

            return validationMessages?.Count <= 0;
        }


        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetNote> timesheetNotes,
                                        ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                        ref IList<DbModel.Timesheet> dbTimesheets,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            bool result = true;

            var assignmentIds = timesheetNotes.Select(x => (long)x.TimesheetId).Distinct().ToList();

            if (!_timesheetService.IsValidTimesheet(assignmentIds, ref dbTimesheets, ref validationMessages, null))
                result = false;
            return result;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetNote> timesheetNotes,
                                       ValidationType validationType,
                                       ref IList<DomainModel.TimesheetNote> filteredTimesheetNotes,
                                       ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                       ref IList<DbModel.Timesheet> dbTimesheets)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (timesheetNotes != null && timesheetNotes.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredTimesheetNotes == null || filteredTimesheetNotes.Count <= 0)
                        filteredTimesheetNotes = FilterRecord(timesheetNotes, validationType);

                    if (dbTimesheetNotes == null || dbTimesheetNotes.Count <= 0)//D661 issue 8
                        dbTimesheetNotes = GetTimesheetNotes(filteredTimesheetNotes, validationType);

                    if (filteredTimesheetNotes?.Count > 0 && IsValidPayload(filteredTimesheetNotes, validationType, ref validationMessages))
                    {
                        if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredTimesheetNotes,
                                                         ref dbTimesheetNotes,
                                                         ref dbTimesheets,
                                                         ref validationMessages);
                        if (validationType == ValidationType.Update)       //D661 issue 8 
                            result = IsRecordValidForAdd(filteredTimesheetNotes,
                                                         ref dbTimesheetNotes,
                                                         ref dbTimesheets,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response AddTimesheeetNote(IList<DomainModel.TimesheetNote> timesheetNotes,
                                           ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                           ref IList<DbModel.Timesheet> dbTimesheets,
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
                var recordToBeAdd = FilterRecord(timesheetNotes, ValidationType.Add);
                eventId = timesheetNotes?.FirstOrDefault()?.EventId;

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(timesheetNotes,
                                                           ValidationType.Add,
                                                           ref recordToBeAdd,
                                                           ref dbTimesheetNotes,
                                                           ref dbTimesheets);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    var dbTimesheetNote = _mapper.Map<IList<DbModel.TimesheetNote>>(recordToBeAdd);
                    _repository.Add(dbTimesheetNote);

                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbTimesheetNote?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, timesheetNotes?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                SqlAuditModuleType.TimesheetNote,
                                                                                                null,
                                                                                                _mapper.Map<DomainModel.TimesheetNote>(x1),
                                                                                                dbModule
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        //D661 issue 8 Start
        private Response ModifyTimesheetNote(IList<DomainModel.TimesheetNote> timesheetNotes,
                                          ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                          ref IList<DbModel.Timesheet> dbTimesheets,
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
                var recordToBeModify = FilterRecord(timesheetNotes, ValidationType.Update);
                eventId = timesheetNotes?.FirstOrDefault()?.EventId;

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(timesheetNotes,
                                                           ValidationType.Update,
                                                           ref recordToBeModify,
                                                           ref dbTimesheetNotes,
                                                           ref dbTimesheets);
                else if (dbTimesheetNotes?.Count <= 0)
                    dbTimesheetNotes = GetTimesheetNotes(recordToBeModify, ValidationType.Update);

                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeModify?.Count > 0)
                {
                    if (dbTimesheetNotes?.Count > 0)
                    {
                        if (this.IsRecordUpdateCountMatching(recordToBeModify, dbTimesheetNotes, ref errorMessages))
                        {
                            dbTimesheetNotes.ToList().ForEach(dbTimesheetNoteValue =>
                            {
                                var dbTimesheetNoteToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetNoteId == dbTimesheetNoteValue.Id);
                                if (dbTimesheetNoteToBeModify != null)
                                {
                                    dbTimesheetNoteValue.TimesheetId = (long)dbTimesheetNoteToBeModify.TimesheetId;
                                    dbTimesheetNoteValue.Id = (int)dbTimesheetNoteToBeModify.TimesheetNoteId;
                                    dbTimesheetNoteValue.CreatedBy = dbTimesheetNoteToBeModify.CreatedBy;
                                    dbTimesheetNoteValue.Note = dbTimesheetNoteToBeModify.Note;
                                    dbTimesheetNoteValue.CreatedDate = DateTime.UtcNow;
                                    dbTimesheetNoteValue.LastModification = DateTime.UtcNow;
                                    dbTimesheetNoteValue.UpdateCount = dbTimesheetNoteToBeModify.UpdateCount.CalculateUpdateCount();
                                    dbTimesheetNoteValue.ModifiedBy = dbTimesheetNoteToBeModify.ModifiedBy;
                                    dbTimesheetNoteValue.IsCustomerVisible = dbTimesheetNoteToBeModify.IsCustomerVisible;
                                    dbTimesheetNoteValue.IsSpecialistVisible = dbTimesheetNoteToBeModify.IsSpecialistVisible;
                                }
                            });
                            _repository.AutoSave = false;
                            _repository.Update(dbTimesheetNotes);
                            if (commitChange && recordToBeModify?.Count > 0)
                                _repository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }
        //D661 issue 8 End
        private Response RemoveTimesheetNotes(IList<DomainModel.TimesheetNote> timesheetNotes,
                                         ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                                         ref IList<DbModel.Timesheet> dbTimesheet,
                                         bool commitChange,
                                         bool isDbValidationRequired)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(timesheetNotes, ValidationType.Delete);

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(timesheetNotes,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbTimesheetNotes,
                                                       ref dbTimesheet
                                                       );

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequired || (Convert.ToBoolean(response.Result)) && dbTimesheetNotes?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbTimesheetNotes);
                        if (commitChange)
                            _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetNotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        //D661 issue 8 Start
        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetNote> timeSheetNotes, IList<DbModel.TimesheetNote> dbTimesheetNote, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = timeSheetNotes.Where(x => !dbTimesheetNote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.TimesheetNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Visit, errorCode, string.Format(_messages[errorCode].ToString(), x.TimesheetNoteId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private IList<DbModel.TimesheetNote> GetTimesheetNotes(IList<DomainModel.TimesheetNote> timesheetNotes,
                                                                ValidationType validationType)
        {
            IList<DbModel.TimesheetNote> dbTimesheetNotes = null;
            if (validationType != ValidationType.Add)
            {
                if (timesheetNotes?.Count > 0)
                {
                    var timesheetNoteId = timesheetNotes.Select(x => x.TimesheetNoteId).Distinct().ToList();
                    dbTimesheetNotes = _repository.FindBy(x => timesheetNoteId.Contains(x.Id)).ToList();
                }
            }
            return dbTimesheetNotes;
        }
        //D661 issue 8 End
        #endregion

    }
}
