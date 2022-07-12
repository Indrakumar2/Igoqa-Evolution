using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public   class AssignmentNoteService: IAssignmentNoteService
    {
        private readonly IAppLogger<AssignmentNoteService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentNoteRepository _assignmentNoterepository = null;
        private readonly IAssignmentNoteValidationService _validationService=null;
        private readonly IAssignmentService _assignmentService = null;

        #region Constructor
        public AssignmentNoteService(IAppLogger<AssignmentNoteService> logger,
                                IMapper mapper,
                                JObject messgaes,
                                IAssignmentNoteRepository assignmentNoterepository,
                                IAssignmentNoteValidationService validationService,
                                IAssignmentService assignmentService)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._messageDescriptions = messgaes;
            this._assignmentNoterepository = assignmentNoterepository;
            this._validationService = validationService;
            this._assignmentService = assignmentService;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(AssignmentNote searchModel)
        {
            IList<AssignmentNote> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = this._assignmentNoterepository.Search(searchModel);
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
        #endregion

        #region Add
        public Response Add(IList<AssignmentNote> assignmentNotes, bool commitChange = true, bool isDbValidationRequire = true,int? assignmentIds=null)
        {
            if (assignmentIds.HasValue)
                assignmentNotes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            IList<DbModel.AssignmentNote> dbAssignmentNotes = null;
            IList<DbModel.Assignment> dbAssignments = null;
            return AddAssignmentNote(assignmentNotes, ref dbAssignmentNotes,ref dbAssignments, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<AssignmentNote> assignmentNotes, ref IList<DbModel.AssignmentNote> dbAssignmentNotes, ref IList<DbModel.Assignment> dbAssignments, bool commitChange = true, bool isDbValidationRequire = true, int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequire)
                assignmentNotes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });

            return AddAssignmentNote(assignmentNotes, ref dbAssignmentNotes,ref dbAssignments, commitChange, isDbValidationRequire);
        }


        #region Delete
        public Response Delete(IList<AssignmentNote> assignmentNotes,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
           IList<DbModel.AssignmentNote> dbAssignmentNotes = null;
            if (assignmentIds.HasValue)
            {
                assignmentNotes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            }
            return this.RemoveAssignmentNotes(assignmentNotes,
                                                  ref dbAssignmentNotes,
                                                  ref dbAssignment,
                                                   commitChange,
                                                  isDbValidationRequired);
        }

        public Response Delete(IList<AssignmentNote> assignmentNotes,
                               ref IList<DbModel.AssignmentNote> dbAssignmentNotes,
                               ref IList<DbModel.Assignment> dbAssignment,
                                bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            if (assignmentIds.HasValue)
            {
                assignmentNotes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            }
            return this.RemoveAssignmentNotes(assignmentNotes,
                                                  ref dbAssignmentNotes,
                                                  ref dbAssignment,
                                                  commitChange,
                                                  isDbValidationRequired);
        }
        #endregion
        #endregion

        #region validationCheck
        public Response IsRecordValidForProcess(IList<AssignmentNote> assignmentNotes,
                                              ValidationType validationType)
        {
            IList<DbModel.AssignmentNote> dbAssignementNotes = null;
            IList<DbModel.Assignment> dbAssignment = null;
          
            return IsRecordValidForProcess(assignmentNotes,
                                            validationType,
                                            ref dbAssignementNotes,
                                            ref dbAssignment
                                           );
        }

        public Response IsRecordValidForProcess(IList<AssignmentNote> assignmentNotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.AssignmentNote> dbAssignementNotes,
                                                ref IList<DbModel.Assignment> dbAssignment
                                                )
        {
            IList<DomainModel.AssignmentNote> filteredAssignemntNotes = null;
            return IsRecordValidForProcess(assignmentNotes,
                                            validationType,
                                            ref filteredAssignemntNotes,
                                            ref dbAssignementNotes,
                                            ref dbAssignment
                                           );
        }

        public Response IsRecordValidForProcess(IList<AssignmentNote> assignmentNotes,
                                                ValidationType validationType,
                                                IList<DbModel.AssignmentNote> dbAssignmentNotes,
                                                ref IList<DbModel.Assignment> dbAssignment
                                               )
        {
            return IsRecordValidForProcess(assignmentNotes,
                                            validationType,
                                            ref dbAssignmentNotes,
                                            ref dbAssignment
                                           );
        }
#endregion

        #endregion

        #region Private Methods
        private Response AddAssignmentNote(IList<AssignmentNote> assignmentNotes, ref IList<DbModel.AssignmentNote> dbAssignmentNotes,
                                          ref IList<DbModel.Assignment> dbAssignment, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentNotes, ValidationType.Add);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentNotes, ValidationType.Add, ref recordToBeAdd, ref dbAssignmentNotes, ref dbAssignment);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _assignmentNoterepository.AutoSave = false;
                    _assignmentNoterepository.Add(_mapper.Map<IList<DbModel.AssignmentNote>>(recordToBeAdd));

                    if (commitChange)
                        _assignmentNoterepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }
            //finally
            //{
            //    _assignmentNoterepository.Dispose();
            //};

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response RemoveAssignmentNotes(IList<DomainModel.AssignmentNote> assignmentNotes,
                                                 ref IList<DbModel.AssignmentNote> dbAssignmentNotes,
                                                 ref IList<DbModel.Assignment> dbAssignment,
                                                 bool commitChange,
                                                 bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
           
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(assignmentNotes, ValidationType.Delete);

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentNotes,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbAssignmentNotes,
                                                       ref dbAssignment
                                                       );

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(response.Result)) && dbAssignmentNotes?.Count > 0)
                    {
                        _assignmentNoterepository.AutoSave = false;
                        _assignmentNoterepository.Delete(dbAssignmentNotes);
                        if (commitChange)
                            _assignmentNoterepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }
            finally
            {
                _assignmentNoterepository.AutoSave = true;
               // _assignmentNoterepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DomainModel.AssignmentNote> FilterRecord(IList<DomainModel.AssignmentNote> assignmentNotes, ValidationType filterType)
        {
            IList<DomainModel.AssignmentNote> filteredAssignmentNotes = null;

            if (filterType == ValidationType.Add)
                filteredAssignmentNotes = assignmentNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredAssignmentNotes = assignmentNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredAssignmentNotes = assignmentNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredAssignmentNotes;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentNote> assignmentNotes,
                                        ValidationType validationType,
                                        ref IList<DomainModel.AssignmentNote> filteredAssignmentNotes,
                                        ref IList<DbModel.AssignmentNote> dbAssignmentNotes,
                                        ref IList<DbModel.Assignment> dbAssignments)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentNotes != null && assignmentNotes.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentNotes == null || filteredAssignmentNotes.Count <= 0)
                        filteredAssignmentNotes = FilterRecord(assignmentNotes, validationType);

                    if (filteredAssignmentNotes?.Count > 0 && IsValidPayload(filteredAssignmentNotes, validationType, ref validationMessages))
                    {
                        if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredAssignmentNotes, ref dbAssignmentNotes,ref dbAssignments, ref validationMessages);
                    }
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentNote> assignmentNotes,
                                 ValidationType validationType,
                                 ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmentNotes), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.AssignmentNote> assignmentNotes, ref IList<DbModel.AssignmentNote> dbAssignmentNotes,ref IList<DbModel.Assignment> dbAssignments, ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            bool result = true;

            var assignmentIds = assignmentNotes.Select(x => (int)x.AssignmentId).Distinct().ToList();
           
            if (!_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages))
            {
                result = false;
            }
            return result;
        }      

        #endregion
    }
}
