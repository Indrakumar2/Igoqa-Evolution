using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentTechnicalSpecialistService : IAssignmentTechnicalSpecilaistService
    {
        private readonly IAppLogger<AssignmentTechnicalSpecialistService> _logger = null; 
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IAssignmentTechnicalSpecilaistValidationService _validationService = null;
        private readonly IAssignmentTechnicalSpecilaistRepository _assignmentTechnicalSpecialistRepository = null; 
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly IAssignmentTechnicalSpecialistScheduleRepository _assignmentTechnicalSpecialistScheduleRepository = null;
        private readonly ITechnicalSpecialistContactRepository _technicalSpecialistContactRepository = null; 
        private readonly IEmailQueueService _emailService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMapper _mapper = null; 

        public AssignmentTechnicalSpecialistService(IAppLogger<AssignmentTechnicalSpecialistService> logger, IMapper mapper, 
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    IAssignmentTechnicalSpecilaistValidationService validationService,
                                                    IAssignmentTechnicalSpecilaistRepository assignmentTechnicalSpecilaistRepository, 
                                                    IAssignmentRepository assignmentRepository,
                                                    IAssignmentTechnicalSpecialistScheduleRepository assignmentTechnicalSpecialistScheduleRepository,
                                                    IEmailQueueService emailService,
                                                    ITechnicalSpecialistContactRepository technicalSpecialistContactRepository, 
                                                    JObject messageDescriptions)
        {
            _logger = logger;
            _mapper = mapper; 
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _validationService = validationService;
            _messageDescriptions = messageDescriptions;
            _assignmentTechnicalSpecialistRepository = assignmentTechnicalSpecilaistRepository;
            _assignmentTechnicalSpecialistScheduleRepository = assignmentTechnicalSpecialistScheduleRepository; 
            _assignmentRepository = assignmentRepository; 
            _technicalSpecialistContactRepository = technicalSpecialistContactRepository;
             _emailService = emailService;
        }

        #region PublicMethods

        #region Get
        public Response Get(AssignmentTechnicalSpecialist searchModel)
        {
            IList<DomainModel.AssignmentTechnicalSpecialist> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = this._assignmentTechnicalSpecialistRepository.Search(searchModel,
                        assign => assign.Assignment,
                        techSpecialist => techSpecialist.TechnicalSpecialist
                    );
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

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            IList<DbModel.Assignment> dbAssignments = null;
            return IsRecordValidForProcess(assignmentTechnicalSpecialists,
                                            validationType,
                                            ref dbAssignments,
                                            ref dbAssignmentTechnicalSpecialists,
                                            ref technicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                                ValidationType validationType,
                                                ref IList<DbModel.Assignment> dbAssignments,
                                                ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            IList<DomainModel.AssignmentTechnicalSpecialist> filteredAssignmentTechnicalSpecialist = null;
            return IsRecordValidForProcess(assignmentTechnicalSpecialists,
                                            validationType,
                                            ref dbAssignments,
                                            ref filteredAssignmentTechnicalSpecialist,
                                            ref dbAssignmentTechnicalSpecialists,
                                            ref dbTechnicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                                ValidationType validationType,
                                                IList<DbModel.Assignment> dbAssignments,
                                                IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            return IsRecordValidForProcess(assignmentTechnicalSpecialist,
                                            validationType,
                                            ref dbAssignments,
                                            ref dbAssignmentTechnicalSpecialist,
                                            ref dbTechnicalSpecialists);
        }
        #endregion

        #region Add
        public Response Add(IList<AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                assignmenTechniaclSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Assignment> dbAssignment = null;

            return AddAssignmentTechnicalSpecialist(assignmenTechniaclSpecialist,
                                                    ref dbAssignmentTechnicalSpecialists,
                                                    ref dbTechnicalSpecialist,
                                                    ref dbAssignment,
                                                    commitChange,
                                                    isDbValidationRequired);
        }

        public Response Add(IList<AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                            ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                            ref IList<DbModel.Assignment> dbAssignment,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmenTechniaclSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });
            return AddAssignmentTechnicalSpecialist(assignmenTechniaclSpecialist,
                                                    ref dbAssignmentTechnicalSpecialist,
                                                    ref dbTechnicalSpecialist,
                                                    ref dbAssignment,
                                                    commitChange,
                                                    isDbValidationRequired);
        }
        #endregion

        #region Modify
        public Response Modify(IList<AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                assignmenTechniaclSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Assignment> dbAssignment = null;
            return UpdateAssignmentTechnicalSpecialist(assignmenTechniaclSpecialist,
                                                       ref dbAssignmentTechnicalSpecialist,
                                                       ref dbTechnicalSpecialist,
                                                       ref dbAssignment,
                                                       commitChange,
                                                       isDbValidationRequired);
        }

        public Response Modify(IList<AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                                ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                ref IList<DbModel.Assignment> dbAssignment,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmenTechniaclSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            return UpdateAssignmentTechnicalSpecialist(assignmenTechniaclSpecialist,
                                                        ref dbAssignmentTechnicalSpecialist,
                                                        ref dbTechnicalSpecialist,
                                                        ref dbAssignment,
                                                        commitChange,
                                                        isDbValidationRequired);
        }
        #endregion

        #region Delete
        public Response Delete(IList<AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentId = null)
        {
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Assignment> dbAssignment = null;
            if (assignmentId.HasValue)
                assignmenTechniaclSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            return this.RemoveAssignmentTechnicalSpecialist(assignmenTechniaclSpecialist,
                                                            ref dbAssignmentTechnicalSpecialist,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbAssignment,
                                                            commitChange,
                                                            isDbValidationRequired);
        }

        public Response Delete(IList<AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                               ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                               ref IList<DbModel.Assignment> dbAssignment,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmenTechniaclSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            return this.RemoveAssignmentTechnicalSpecialist(assignmenTechniaclSpecialist,
                                                            ref dbAssignmentTechnicalSpecialist,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbAssignment,
                                                            commitChange,
                                                            isDbValidationRequired);
        }
        #endregion

        public bool IsValidAssignmentTechnicalSpecialist(IList<int> assignmentTechnicalSpecilaistIds,
                                                         ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbAssigTechSpecilaists = _assignmentTechnicalSpecialistRepository.FindBy(x => assignmentTechnicalSpecilaistIds.Contains((int)x.Id)).ToList();
            var invalidAssignmentTechnicalSpecilaistIds = assignmentTechnicalSpecilaistIds.Where(x => !dbAssigTechSpecilaists.Any(x1 => x1.Id == x));
            invalidAssignmentTechnicalSpecilaistIds.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistInvalidId, x);
            });
            dbAssignmentTechnicalSpecialist = dbAssigTechSpecilaists;
            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        #endregion

        #region Private Methods

        private Response AddAssignmentTechnicalSpecialist(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                                          ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                          ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                          ref IList<DbModel.Assignment> dbAssignment,
                                                          bool commitChange = true,
                                                          bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            IList<DbModel.Assignment> dbAssignments = dbAssignment;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentTechnicalSpecialist, ValidationType.Add);

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(assignmentTechnicalSpecialist,
                                                           ValidationType.Add,
                                                           ref dbAssignments,
                                                           ref recordToBeAdd,
                                                           ref dbAssignmentTechnicalSpecialist,
                                                           ref technicalSpecialists);

                if (recordToBeAdd?.Count > 0)
                {
                    if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                    { 
                        _assignmentTechnicalSpecialistRepository.AutoSave = false; 

                        recordToBeAdd?.ToList().ForEach(x =>
                        {
                            x.AssignmentTechnicalSpecialistSchedules = x.AssignmentTechnicalSpecialistSchedules?.Where(x1 => x1.RecordStatus.IsRecordStatusNew()).ToList();
                        });

                        recordToBeAdd = recordToBeAdd?.Select(x => { x.AssignmentTechnicalSplId = null; return x; }).ToList();
                        var dbRecordToAdd = _mapper.Map<IList<DbModel.AssignmentTechnicalSpecialist>>(recordToBeAdd);
                        _assignmentTechnicalSpecialistRepository.Add(dbRecordToAdd);
                        ProcessEmailNotifications(dbRecordToAdd, dbAssignments);
                        if (commitChange)
                            _assignmentTechnicalSpecialistRepository.ForceSave();

                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialist);
            } 

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentTechnicalSpecialist(IList<AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                                             ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                             ref IList<DbModel.Assignment> dbAssignment,
                                                             bool commitChange = true,
                                                             bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentTechnicalSpecialist> result = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentTechnicalSpecialist, ValidationType.Update);
                var technicalSpecialist = assignmentTechnicalSpecialist.Where(x => (x.Epin.HasValue))?.Select(x => x.Epin).ToList();

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentTechnicalSpecialist,
                                                            ValidationType.Update,
                                                            ref dbAssignment,
                                                            ref recordToBeModify,
                                                            ref dbAssignmentTechnicalSpecialist,
                                                            ref dbTechnicalSpecialist);
                else if (dbAssignmentTechnicalSpecialist?.Count <= 0)
                    dbAssignmentTechnicalSpecialist = GetAssignmentTechnicalspecialist(recordToBeModify);

                var dbAssignmentTechnicalSpecialists = dbAssignmentTechnicalSpecialist;
                var dbSpecialistSchedule = dbAssignmentTechnicalSpecialists?.ToList().SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();

                if (recordToBeModify?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbAssignmentTechnicalSpecialist?.Count > 0))
                    {
                        if (dbTechnicalSpecialist?.Count > 0)
                            technicalSpecialists = dbTechnicalSpecialist;

                        dbAssignmentTechnicalSpecialist?.ToList().ForEach(dbActivity =>
                        {
                            var assignmentTechnicalSpecialistToBeModify = recordToBeModify?.FirstOrDefault(x => x.AssignmentTechnicalSplId == dbActivity.Id);
                            if (assignmentTechnicalSpecialistToBeModify != null)
                            {
                                dbActivity.TechnicalSpecialistId = (int)technicalSpecialists?.ToList().FirstOrDefault(x1 => x1.Pin == assignmentTechnicalSpecialistToBeModify.Epin).Id;
                                dbActivity.AssignmentId = (int)assignmentTechnicalSpecialistToBeModify.AssignmentId;
                                dbActivity.IsSupervisor = assignmentTechnicalSpecialistToBeModify.IsSupervisor;
                                dbActivity.IsActive = assignmentTechnicalSpecialistToBeModify.IsActive;
                                dbActivity.LastModification = DateTime.UtcNow;
                                dbActivity.UpdateCount = assignmentTechnicalSpecialistToBeModify.UpdateCount.CalculateUpdateCount();
                                dbActivity.ModifiedBy = assignmentTechnicalSpecialistToBeModify.ModifiedBy;

                                assignmentTechnicalSpecialistToBeModify?.AssignmentTechnicalSpecialistSchedules?.ForEach(x =>
                                {
                                    ProcessTechSpecSchedule((int)assignmentTechnicalSpecialistToBeModify.AssignmentTechnicalSplId,
                                                            (int)assignmentTechnicalSpecialistToBeModify.AssignmentId,
                                                            x,
                                                            dbAssignmentTechnicalSpecialists,
                                                            dbSpecialistSchedule);
                                });
                            }

                        });
                        _assignmentTechnicalSpecialistRepository.AutoSave = false;
                        _assignmentTechnicalSpecialistRepository.Update(dbAssignmentTechnicalSpecialist);
                        if (commitChange)
                        {
                            _assignmentTechnicalSpecialistRepository.ForceSave();
                            _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                        }

                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialist);
            }
            finally
            {
                _assignmentTechnicalSpecialistRepository.AutoSave = true;
              //  _assignmentTechnicalSpecialistRepository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentTechnicalSpecialist(IList<AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                                             ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                             ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                             ref IList<DbModel.Assignment> dbAssignment,
                                                             bool commitChange,
                                                             bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignments = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(assignmentTechnicalSpecialist, ValidationType.Delete);

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentTechnicalSpecialist,
                                                        ValidationType.Delete,
                                                        ref dbAssignments,
                                                        ref recordToBeDelete,
                                                        ref dbAssignmentTechnicalSpecialist,
                                                        ref dbTechnicalSpecialist);

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(response.Result) && dbAssignmentTechnicalSpecialist?.Count > 0))
                    {
                        _assignmentTechnicalSpecialistRepository.AutoSave = false;
                        _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
                        var dbAssignmentTechSpecSchedule = dbAssignmentTechnicalSpecialist?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();
                        var techSpecSchedule = recordToBeDelete?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules.Where(x1 => x1.RecordStatus.IsRecordStatusDeleted())).ToList();

                        var tsScheduleToDelete = dbAssignmentTechSpecSchedule?.Where(x => techSpecSchedule.Any(x1 => x1.AssignmentTechnicalSpecilaistId == x.AssignmentTechnicalSpecialistId)).ToList();
                        _assignmentTechnicalSpecialistScheduleRepository.Delete(tsScheduleToDelete);

                        if (commitChange)
                        {
                            _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                            _assignmentTechnicalSpecialistRepository.Delete(dbAssignmentTechnicalSpecialist);
                            _assignmentTechnicalSpecialistRepository.ForceSave();

                        }
                    }
                    else
                        return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialist);
            }
            finally
            {
                _assignmentTechnicalSpecialistRepository.AutoSave = true;
               // _assignmentTechnicalSpecialistRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        private IList<DomainModel.AssignmentTechnicalSpecialist> FilterRecord(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                                                              ValidationType filterType)
        {
            IList<DomainModel.AssignmentTechnicalSpecialist> filteredAssignmentTechnicalSpecialist = null;

            if (filterType == ValidationType.Add)
                filteredAssignmentTechnicalSpecialist = assignmentTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredAssignmentTechnicalSpecialist = assignmentTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredAssignmentTechnicalSpecialist = assignmentTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredAssignmentTechnicalSpecialist;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                               ValidationType validationType,
                                               ref IList<DbModel.Assignment> dbAssignments,
                                               ref IList<DomainModel.AssignmentTechnicalSpecialist> filteredAssignmentTechnicalSpecialist,
                                               ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                               ref IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            var AssignmnetTechnicalspecialistSchedules = assignmentTechnicalSpecialists.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules).ToList();
            try
            {
                if (assignmentTechnicalSpecialists != null && assignmentTechnicalSpecialists.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentTechnicalSpecialist == null || filteredAssignmentTechnicalSpecialist.Count <= 0)
                        filteredAssignmentTechnicalSpecialist = FilterRecord(assignmentTechnicalSpecialists, validationType);

                    if (filteredAssignmentTechnicalSpecialist?.Count > 0 && IsValidPayload(filteredAssignmentTechnicalSpecialist, validationType, ref validationMessages))
                    {
                        IList<int> moduleNotExists = null;
                        var assignmentTechnicalSpecialistId = filteredAssignmentTechnicalSpecialist.Where(x => x.AssignmentTechnicalSplId.HasValue).Select(x => x.AssignmentTechnicalSplId.Value).Distinct().ToList();

                        if ((dbAssignmentTechnicalSpecialist == null || dbAssignmentTechnicalSpecialist.Count <= 0) && validationType != ValidationType.Add)
                            dbAssignmentTechnicalSpecialist = GetAssignmentTechnicalspecialist(filteredAssignmentTechnicalSpecialist);

                        var assignmentIds = filteredAssignmentTechnicalSpecialist.Where(x => x.AssignmentId != null).Distinct().Select(x => (int)x.AssignmentId).ToList();

                        if (IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages))
                        {
                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsAssignmentTechnicalSpecialistExistInDb(assignmentTechnicalSpecialistId, dbAssignmentTechnicalSpecialist, ref moduleNotExists, ref validationMessages);
                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidaForUpdate(filteredAssignmentTechnicalSpecialist, dbAssignmentTechnicalSpecialist, ref validationMessages, ref technicalSpecialists);
                                if (result && validationType == ValidationType.Delete)
                                {
                                    var ePins = filteredAssignmentTechnicalSpecialist.Where(x => x.Epin != null).Distinct().Select(x => (int)x.Epin).ToList();

                                    result = IsAssignmentTechnicalSpecialistCanBeRemove(ePins, dbAssignments, ref validationMessages);

                                }
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidaForAdd(filteredAssignmentTechnicalSpecialist, dbAssignmentTechnicalSpecialist, ref validationMessages, ref technicalSpecialists);
                        }

                    }
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        private bool IsValidPayload(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenttechnicalSpecialists,
                                  ValidationType validationType,
                                  ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmenttechnicalSpecialists), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;

            //return true;
        }

        private bool IsValidAssignment(IList<int> assignmentId,
                                      ref IList<DbModel.Assignment> dbAssignments,
                                      ref IList<ValidationMessage> messages,
                                      params Expression<Func<DbModel.Assignment, object>>[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbAssignments == null)
            {
                var dbAssignment = _assignmentRepository?.FindBy(x => assignmentId.Contains(x.Id), includes).ToList();
                var assignmentNotExists = assignmentId?.Where(x => !dbAssignment.Any(x2 => x2.Id == x))?.ToList();
                assignmentNotExists?.ForEach(x =>
                { 
                    message.Add(_messageDescriptions, x, MessageType.AssignmentNotExists, x);
                });
                dbAssignments = dbAssignment;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private bool IsValidTechSpec(IList<string> tsPins,
                                          ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                          ref IList<ValidationMessage> validationMessages,
                                          string[] includes)
        {
            IList<DbModel.TechnicalSpecialist> dbTechSpecInfos = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (tsPins?.Count > 0)
            {
                dbTechSpecInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString()), includes).ToList();

                var tsPinNotExists = tsPins.Where(pin => !dbTechSpecInfos.Any(x1 => x1.Pin.ToString() == pin))
                                    .Select(pin => pin)
                                    .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    message.Add(_messageDescriptions, x, MessageType.TsEPinDoesNotExist, x);
                });
                dbTsInfos = dbTechSpecInfos;
                validationMessages.AddRange(message);
            } 
            return message?.Count <= 0;
        }

        private IList<DbModel.AssignmentTechnicalSpecialist> GetAssignmentTechnicalspecialist(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist)
        {
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            if (assignmentTechnicalSpecialist?.Count > 0)
            {
                var assignmentTechnicalSpecialistId = assignmentTechnicalSpecialist.Select(x => x.AssignmentTechnicalSplId).Distinct().ToList();
                dbAssignmentTechnicalSpecialist = _assignmentTechnicalSpecialistRepository.FindBy(x => assignmentTechnicalSpecialistId.Contains(x.Id)).ToList();
            }

            return dbAssignmentTechnicalSpecialist;
        }

        private bool IsRecordValidaForAdd(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                          IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                          ref IList<ValidationMessage> validationMessages,
                                          ref IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
            IList<ValidationMessage> messages = null; 
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var includes = new string[] { "TechnicalSpecialistPaySchedule",
                                          "TechnicalSpecialistPayRate",
                                          "TechnicalSpecialistContact"
                                         };

            var assignmentTechnicalSpecialistSchedule = assignmentTechnicalSpecialist.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules).ToList(); 
            var technicalSpecialistIds = assignmentTechnicalSpecialist.Where(x => x.Epin > 0).Select(u => u.Epin.ToString()).Distinct().ToList();
            // if(dbAssignmentTechnicalSpecialist !=null)
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbTechSpecSchedule = dbAssignmentTechnicalSpecialist?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();
            var assignmentTechnicalSpecialistSchedules = assignmentTechnicalSpecialist.Where(x => x.AssignmentTechnicalSpecialistSchedules != null).SelectMany(x => x.AssignmentTechnicalSpecialistSchedules).ToList();
            if (Convert.ToBoolean(IsValidTechSpec(technicalSpecialistIds, ref technicalSpecialists, ref validationMessages, includes))
                && IsAssignmentTechnicalSpecialistContractScheduleAlreadyExists(assignmentTechnicalSpecialistSchedules, dbTechSpecSchedule, ValidationType.Add, ref validationMessages)
                && IsUniqueAssignmentTechnicalSpecialist(assignmentTechnicalSpecialist, ValidationType.Add, dbAssignmentTechnicalSpecialist, ref validationMessages))
            {
                IsUniqueTechnicalSpecialistSchedule(assignmentTechnicalSpecialistSchedule, dbTechSpecSchedule, ref validationMessages);
            }   
            messages = validationMessages; 
            return messages?.Count <= 0;
        }

        private bool IsAssignmentTechnicalSpecialistExistInDb(IList<int> AssignmentTechnicalSpecialistIds,
                                                               IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                               ref IList<int> assignmentTechnicalSpecialistNotExists,
                                                               ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentTechnicalSpecialist == null)
                dbAssignmentTechnicalSpecialist = new List<DbModel.AssignmentTechnicalSpecialist>();

            var validMessages = validationMessages;

            if (AssignmentTechnicalSpecialistIds?.Count > 0)
            {
                assignmentTechnicalSpecialistNotExists = AssignmentTechnicalSpecialistIds.Where(x => !dbAssignmentTechnicalSpecialist.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentTechnicalSpecialistNotExists?.ToList().ForEach(x =>
                    {
                        validMessages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistNotExists, x);
                    });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                             IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                             ref IList<ValidationMessage> validationMessages,
                                             ref IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbTechSpecSchedule = dbAssignmentTechnicalSpecialist.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();
            var assignmentTechnicalSpecialistSchedule = assignmentTechnicalSpecialist.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules).ToList();

            assignmentTechnicalSpecialist.Select(x => x.AssignmentTechnicalSplId).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbAssignmentTechnicalSpecialist.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.AssignmentTechnicalSpecialistNotExists, x1);
                   });

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentTechnicalSpecialist, dbAssignmentTechnicalSpecialist, ref validationMessages))
                { 
                    var technicalSpecialistEpins = assignmentTechnicalSpecialist.Where(x => x.Epin > 0).Select(u => u.Epin.ToString()).Distinct().ToList();

                    var assignmentTechnicalSpecialistSchedules = assignmentTechnicalSpecialist.Where(x => x.AssignmentTechnicalSpecialistSchedules != null).SelectMany(x => x.AssignmentTechnicalSpecialistSchedules).ToList();
                    if (
                        Convert.ToBoolean(IsValidTechSpec(technicalSpecialistEpins, ref technicalSpecialists, ref validationMessages,null))
                        && IsAssignmentTechnicalSpecialistContractScheduleAlreadyExists(assignmentTechnicalSpecialistSchedules, dbTechSpecSchedule, ValidationType.Update, ref validationMessages)
                        && IsUniqueAssignmentTechnicalSpecialist(assignmentTechnicalSpecialist, ValidationType.Update, dbAssignmentTechnicalSpecialist, ref validationMessages))
                        IsUniqueTechnicalSpecialistSchedule(assignmentTechnicalSpecialistSchedule, dbTechSpecSchedule, ref validationMessages);



                }
            }
            messages = validationMessages;

            //if (messages.Count > 0)
            //    validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                                 IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentTechnicalSpecialist.Where(x => !dbAssignmentTechnicalSpecialist.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentTechnicalSplId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentTechnicalSplId, MessageType.AssignmentTechnicalSpecialistUpdateCountMisMatch, x.AssignmentTechnicalSplId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsUniqueAssignmentTechnicalSpecialist(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                                           ValidationType validationType,
                                                           IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                                           ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentTechnicalSpecialist> tsExists = null;
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            tsExists = _assignmentTechnicalSpecialistRepository.IsUniqueAssignmentTS(assignmentTechnicalSpecialists,
                                                                                     dbAssignmentTechnicalSpecialists,
                                                                                     validationType);

            tsExists?.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.TechnicalSpecialistId, MessageType.AssignmentTechnicalSpecialistAlreadyExist, x.TechnicalSpecialistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private bool IsAssignmentTechnicalSpecialistContractScheduleAlreadyExists(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules,
                                                    IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbTechSpecSchedule,
                                                    ValidationType validationType,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assignmentPartialTechSplSchedules = assignmentTechnicalSpecialistSchedules.Select(x => new { x.ContractScheduleId, x.TechnicalSpecialistPayScheduleId, x.AssignmentTechnicalSpecilaistId, x.AssignmentTechnicalSpecialistScheduleId }).ToList();

            var duplicateRecords = dbTechSpecSchedule?.Where(x => assignmentPartialTechSplSchedules.Any(x1 => x1.ContractScheduleId == x.ContractChargeScheduleId &&
                                                                                                            x1.TechnicalSpecialistPayScheduleId == x.TechnicalSpecialistPayScheduleId &&
                                                                                                            x1.AssignmentTechnicalSpecilaistId == x.AssignmentTechnicalSpecialistId
                                                                                                            && x1.AssignmentTechnicalSpecialistScheduleId != x.Id)).ToList();
            if (duplicateRecords != null)
            {
                assignmentPartialTechSplSchedules.Where(x => duplicateRecords.Any(x1 => x1.ContractChargeScheduleId == x.ContractScheduleId && x1.TechnicalSpecialistPayScheduleId == x.TechnicalSpecialistPayScheduleId && x1.AssignmentTechnicalSpecialistId == x.AssignmentTechnicalSpecilaistId))?.ToList().ForEach(x =>
                    {
                        messages.Add(_messageDescriptions, x.AssignmentTechnicalSpecilaistId, MessageType.AssignmentContractRateScheduleAlreadyExist, x.AssignmentTechnicalSpecilaistId);
                    });
            }


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;



        }

        private bool IsAssignmentTechnicalSpecialistCanBeRemove(IList<int> ePins,
                                                                IList<DbModel.Assignment> dbAssignments,
                                                                ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            var dbVisitTechnicalSpecialists = dbAssignments.SelectMany(x => x.Visit.SelectMany(x1 => x1.VisitTechnicalSpecialist));
            var dbTimesheetTechnicalSpecialists = dbAssignments.SelectMany(x => x.Timesheet.SelectMany(x1 => x1.TimesheetTechnicalSpecialist));

            var visitEpins = ePins.Where(x => dbVisitTechnicalSpecialists.ToList().Any(x1 => x1.TechnicalSpecialist?.Pin == x)).ToList();

            var timesheetEpins = ePins.Where(x => dbTimesheetTechnicalSpecialists.ToList().Any(x1 => x1.TechnicalSpecialist?.Pin == x)).ToList();

            if (visitEpins.Count > 0)
            {
                visitEpins.ToList().ForEach(x =>
            {
                validationMessages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistIsBeingUsed, x);
            });
            }

            if (timesheetEpins.Count > 0)
            {
                timesheetEpins.ToList().ForEach(x =>
                {
                    validationMessages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistIsBeingUsed, x);
                });
            }

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }

        private bool IsChildRecordExistsInDb(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                             IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                             ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var messages = validationMessages;

            var techSpecSchedules = assignmentTechnicalSpecialists.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules).ToList();

            var dbTechSpecSchedules = dbAssignmentTechnicalSpecialists.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();

            var techSpecScheduleId = techSpecSchedules.Where(x => x.AssignmentTechnicalSpecialistScheduleId != null
                                                            && !x.RecordStatus.IsRecordStatusNew()).Distinct()
                                                            .Select(x => x.AssignmentTechnicalSpecialistScheduleId).ToList();

            var recordNotExists = techSpecScheduleId.Where(x => !dbTechSpecSchedules.Any(x1 => x1.Id == x)).ToList();

            recordNotExists?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleNotExists, x);
            });

            validationMessages = messages;

            return validationMessages?.Count <= 0;

        }

        private bool IsChildRecordUpdateCountMatching(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                                      IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechicalSpecialits,
                                                      ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var messages = validationMessages;

            var dbTechSpecSchedules = dbAssignmentTechicalSpecialits.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();

            var techSpecSchedules = assignmentTechnicalSpecialists.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)
                                                                  .Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            var updateCountNotMatchingRecords = techSpecSchedules.Where(x => !dbTechSpecSchedules.Any(x1 => x1.Id == x.AssignmentTechnicalSpecialistScheduleId &&
                                                                      x.UpdateCount == x1.UpdateCount)).ToList();

            updateCountNotMatchingRecords?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleUpdateCountMisMatch, x);
            });

            validationMessages = messages;

            return validationMessages?.Count <= 0;

        }

        private void ProcessTechSpecSchedule(int techSpecId,
                                             int assignmentId,
                                             DomainModel.AssignmentTechnicalSpecialistSchedule technicalSpecialistSchedule,
                                             IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechSpecialist,
                                             IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechSpecSchedule
                                             )
        {
            _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
            if (technicalSpecialistSchedule.RecordStatus.IsRecordStatusNew())
            {
                var recordToAdd = _mapper.Map<DbModel.AssignmentTechnicalSpecialistSchedule>(technicalSpecialistSchedule);
                recordToAdd.AssignmentTechnicalSpecialistId = (int)techSpecId;
                _assignmentTechnicalSpecialistScheduleRepository.Add(recordToAdd);
            }
            if (technicalSpecialistSchedule.RecordStatus.IsRecordStatusModified())
            {
                var recordToUpdate = dbAssignmentTechSpecSchedule?.Where(x => x.Id == technicalSpecialistSchedule.AssignmentTechnicalSpecialistScheduleId)?.ToList();
                recordToUpdate.ForEach(TS =>
                {

                    _mapper.Map(technicalSpecialistSchedule, TS);
                    TS.LastModification = DateTime.UtcNow;
                    TS.UpdateCount = technicalSpecialistSchedule.UpdateCount.CalculateUpdateCount();
                    TS.ModifiedBy = technicalSpecialistSchedule.ModifiedBy;
                });
                _assignmentTechnicalSpecialistScheduleRepository.Update(recordToUpdate);
                _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
            }
            if (technicalSpecialistSchedule.RecordStatus.IsRecordStatusDeleted())
            {
                var recordToDelete = dbAssignmentTechSpecSchedule.Where(x => x.Id == technicalSpecialistSchedule.AssignmentTechnicalSpecialistScheduleId)?.FirstOrDefault();
                _assignmentTechnicalSpecialistScheduleRepository.Delete(recordToDelete);
            }
        }

        private bool IsUniqueTechnicalSpecialistSchedule(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules,
                                                         IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTSSchedule,
                                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<DbModel.AssignmentTechnicalSpecialistSchedule> tsScheduleExists = null;
            tsScheduleExists = _assignmentTechnicalSpecialistScheduleRepository.IsUniqueTSSchedule(assignmentTechnicalSpecialistSchedules,
                                                                                                   dbAssignmentTSSchedule);

            tsScheduleExists?.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentTechnicalSpecialistId, MessageType.AssignmentTechnicalSpecialistScheduleDuplicateRecord, x.AssignmentTechnicalSpecialistId, x.ContractChargeScheduleId, x.TechnicalSpecialistPayScheduleId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private Response ProcessEmailNotifications(IList<DbModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists, IList<DbModel.Assignment> assignment)
        {
            string emailSubject = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = null;
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            List<EmailQueueMessage> emailQueueMessage = new List<EmailQueueMessage>();
            try
            {
                if (assignmentTechnicalSpecialists != null && assignmentTechnicalSpecialists?.Count > 0)
                {
                    var epins = assignmentTechnicalSpecialists.Select(x => x.TechnicalSpecialistId).Distinct().ToList();
                    if (assignment != null)
                    {
                        var tsTstContactInfos = GetByPinAndContactType(epins, new List<string> { ContactType.PrimaryEmail.ToString() });
                        assignmentTechnicalSpecialists.ToList().ForEach(x =>
                        {
                            //Sending ResourceAssignmentNotification Emails
                            ResourceAssignmentEmailNotification(assignment, x, out emailSubject, out emailMessage, out toAddresses, out ccAddresses, tsTstContactInfos);
                            if (emailMessage != null)
                                emailQueueMessage.Add(emailMessage);
                             
                        });
                    }

                    return _emailService.Add(emailQueueMessage);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistContactInfo> GetByPinAndContactType(IList<int> tsIds, IList<string> contactTypes)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                if (tsIds?.Count > 0)
                {
                    result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(_technicalSpecialistContactRepository.FindBy(x => tsIds.Contains(x.TechnicalSpecialist.Id) && contactTypes.Contains(x.ContactType)).ToList());
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIds, contactTypes);
            }

            return result;
        }

        private void ResourceAssignmentEmailNotification(IList<DbModel.Assignment> assignment, DbModel.AssignmentTechnicalSpecialist x, out string emailSubject, out EmailQueueMessage emailMessage, out List<EmailAddress> toAddresses, out List<EmailAddress> ccAddresses, IList<TechnicalSpecialistContactInfo> tsTstContactInfos)
        {
            toAddresses= ccAddresses = new List<EmailAddress>();
            emailSubject = string.Empty;
            emailMessage = new EmailQueueMessage(); 

            var dbAssignment = assignment?.FirstOrDefault(x1 => x1.Id == x.AssignmentId);

            if (dbAssignment != null)
            {
                if (dbAssignment?.ContractCompanyId!= dbAssignment.OperatingCompanyId)
                {
                    ccAddresses = new List<EmailAddress> {
                                        new EmailAddress() { Address = dbAssignment?.ContractCompanyCoordinator?.Email }
                                    };
                }
                
                toAddresses = tsTstContactInfos?.Where(x1 => x1.Epin == x.TechnicalSpecialistId && !string.IsNullOrEmpty(x1.EmailAddress))?.Select(x1 => new EmailAddress() { Address = x1.EmailAddress }).ToList();
                emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_ResourceAssignmentNotification_Subject, dbAssignment?.Project?.ProjectNumber, dbAssignment?.AssignmentNumber);

                var emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                        new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Customer_Name, dbAssignment?.Project?.Contract?.Customer?.Name),
                                        new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Project_Number, dbAssignment?.Project?.ProjectNumber.ToString()),
                                        new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Assignment_Number, dbAssignment?.AssignmentNumber.ToString())
                                    };
                emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, EmailTemplate.EmailResourceAssignmentNotification, EmailType.RAA, ModuleCodeType.ASGMNT, x?.AssignmentId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, IsMailSendAsGroup: true);
            }
        }

        public List<DbModel.AssignmentTechnicalSpecialist> GetAssignment(IList<int> epins)
        {
            List<DbModel.AssignmentTechnicalSpecialist> result = null;
            Exception exception = null;
            try
            {
                result = this._assignmentTechnicalSpecialistRepository.FindBy(x => x.Assignment.AssignmentStatus == "P" && epins.Contains(x.TechnicalSpecialistId), new string[] { "Assignment" }).ToList().OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), epins);
            }

            return result;
        }
        #endregion
    }
}
