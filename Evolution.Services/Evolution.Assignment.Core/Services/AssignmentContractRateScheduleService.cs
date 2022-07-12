using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
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
    public class AssignmentContractRateScheduleService : IAssignmentContractRateScheduleService
    {
        private readonly IAppLogger<AssignmentContractRateScheduleService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IAssignmentContractRateScheduleRepository _assignmentContractRateScheduleRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentContractRateScheduleValidationService _assignmentContractRateScheduleValidationService = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly IContractScheduleService _contractScheduleService = null;

        #region Constructor

        public AssignmentContractRateScheduleService(IMapper mapper,
                                IAssignmentContractRateScheduleRepository assignmentContractRateScheduleRepository,
                                IAppLogger<AssignmentContractRateScheduleService> logger,
                                IAssignmentContractRateScheduleValidationService assignmentContractRateScheduleValidationService,
                                IAssignmentService assignmentService,
                                IContractScheduleService contractScheduleService,
                                JObject messages)
        {
            _assignmentContractRateScheduleRepository = assignmentContractRateScheduleRepository;
            _logger = logger;
            _mapper = mapper;
            _assignmentContractRateScheduleValidationService = assignmentContractRateScheduleValidationService;
            _assignmentService = assignmentService;
            _contractScheduleService = contractScheduleService;
            _messageDescriptions = messages;
        }

        #endregion

        #region Public Methods

        #region Add

        public Response Add(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            if (assignmentId.HasValue)
                assignmentContractRateSchedules.ToList().ForEach(x => x.AssignmentId = assignmentId);
            return AddAssignmentContractRateSchedules(assignmentContractRateSchedules,
                                                      ref dbAssignment,
                                                      ref dbAssignmentContractRateSchedules,
                                                      ref dbContractSchedules,
                                                      commitChange,
                                                      isDbValidationRequired);
        }

        public Response Add(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                            ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.ContractSchedule> dbContractSchedules,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmentContractRateSchedules.ToList().ForEach(x => x.AssignmentId = assignmentId);
            return AddAssignmentContractRateSchedules(assignmentContractRateSchedules,
                                                     ref dbAssignment,
                                                     ref dbAssignmentContractSchedules,
                                                     ref dbContractSchedules,
                                                     commitChange,
                                                     isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentId = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            if (assignmentId.HasValue)
                assignmentContractRateSchedules.ToList().ForEach(x => x.AssignmentId = assignmentId);
            return RemoveAssignmentContractRateSchedules(assignmentContractRateSchedules,
                                                        ref dbAssignmentContractRateSchedules,
                                                        ref dbAssignment,
                                                        ref dbContractSchedules,
                                                        commitChange,
                                                        isDbValidationRequired);
        }

        public Response Delete(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                               ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                               ref IList<DbModel.Assignment> dbAssignment,
                               ref IList<DbModel.ContractSchedule> dbContractSchedules,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmentContractRateSchedules.ToList().ForEach(x => x.AssignmentId = assignmentId);
            return RemoveAssignmentContractRateSchedules(assignmentContractRateSchedules,
                                                        ref dbAssignmentContractSchedules,
                                                        ref dbAssignment,
                                                        ref dbContractSchedules,
                                                        commitChange,
                                                        isDbValidationRequired);
        }

        #endregion

        #region Get

        public Response Get(AssignmentContractRateSchedule searchModel)
        {
            IList<DomainModel.AssignmentContractRateSchedule> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = _assignmentContractRateScheduleRepository.Search(searchModel);
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

        #region Modify

        public Response Modify(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                int? assignmentId = null)
        {
            IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.Assignment> dbAssignment = null;

            if (assignmentId.HasValue)
                assignmentContractRateSchedules.ToList().ForEach(x => x.AssignmentId = assignmentId);

            return UpdateAssignmentContractRateSchedules(assignmentContractRateSchedules,
                                                         ref dbAssignmentContractSchedules,
                                                         ref dbAssignment,
                                                         ref dbContractSchedules,
                                                         commitChange,
                                                         isDbValidationRequired);
        }

        public Response Modify(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmentContractRateSchedules.ToList().ForEach(x => x.AssignmentId = assignmentId);

            return UpdateAssignmentContractRateSchedules(assignmentContractRateSchedules,
                                                         ref dbAssignmentContractSchedules,
                                                         ref dbAssignment,
                                                         ref dbContractSchedules,
                                                         commitChange,
                                                         isDbValidationRequired);
        }

        #endregion

        #region Validation Check

        public Response IsRecordValidForProcess(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.Assignment> dbAssignment = null;
            return IsRecordValidForProcess(assignmentContractRateSchedules,
                                           validationType,
                                           ref dbAssignment,
                                           ref dbContractSchedules,
                                           ref dbAssignmentContractSchedules);
        }

        public Response IsRecordValidForProcess(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                ValidationType validationType,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                                ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules)
        {
            IList<AssignmentContractRateSchedule> filteredAssignmentContractRateSchedules = null;
            return IsRecordValidForProcess(assignmentContractRateSchedules,
                                        validationType,
                                        ref filteredAssignmentContractRateSchedules,
                                        ref dbAssignmentContractRateSchedules,
                                        ref dbContractSchedules,
                                        ref dbAssignment);
        }

        public Response IsRecordValidForProcess(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                ValidationType validationType,
                                                IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                IList<DbModel.ContractSchedule> dbContractSchedules,
                                                ref IList<DbModel.Assignment> dbAssignment)
        {
            return IsRecordValidForProcess(assignmentContractRateSchedules,
                                            validationType,
                                            ref dbAssignment,
                                            ref dbContractSchedules,
                                            ref dbAssignmentContractRateSchedules);
        }

        #endregion

        #endregion

        #region Private Methods

        private Response AddAssignmentContractRateSchedules(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                            ref IList<DbModel.Assignment> dbAssignment,
                                                            ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                                                            ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                                            bool commitChange = true,
                                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentContractRateSchedules, ValidationType.Add);

                //if (dbAssignmentContractSchedules == null)
                //    dbAssignmentContractSchedules = GetAssignmentContractSchedules(recordToBeAdd);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentContractRateSchedules,
                                                        ValidationType.Add,
                                                        ref recordToBeAdd,
                                                        ref dbAssignmentContractSchedules,
                                                        ref dbContractSchedules,
                                                        ref dbAssignment);

                if (recordToBeAdd?.Count > 0)
                {
                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                    {
                        _assignmentContractRateScheduleRepository.AutoSave = false;
                        dbAssignmentContractSchedules = _mapper.Map<IList<DbModel.AssignmentContractSchedule>>(recordToBeAdd, opt =>
                         {
                             opt.Items["isAssignId"] = false;
                         });
                        _assignmentContractRateScheduleRepository.Add(dbAssignmentContractSchedules);
                        if (commitChange)
                            _assignmentContractRateScheduleRepository.ForceSave();
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentContractRateSchedules(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                                ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                                                                ref IList<DbModel.Assignment> dbAssignment,
                                                                ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                                                bool commitChange = true,
                                                                bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<AssignmentContractRateSchedule> result = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentContractRateSchedules, ValidationType.Update);

                if (dbAssignmentContractSchedules == null)
                    dbAssignmentContractSchedules = GetAssignmentContractSchedules(recordToBeModify);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentContractRateSchedules,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbAssignmentContractSchedules,
                                                            ref dbContractSchedules,
                                                            ref dbAssignment);

                if (recordToBeModify?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result)) && dbAssignmentContractSchedules?.Count > 0)
                    {
                        dbAssignmentContractSchedules.ToList().ForEach(dbAssignmentContractSchedule =>
                        {
                            var activityToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentContractRateScheduleId == dbAssignmentContractSchedule.Id);
                            if (activityToBeModify != null)
                            {
                                dbAssignmentContractSchedule.AssignmentId = activityToBeModify.AssignmentId.Value;
                                dbAssignmentContractSchedule.ContractScheduleId = activityToBeModify.ContractScheduleId.Value;
                                dbAssignmentContractSchedule.LastModification = DateTime.UtcNow;
                                dbAssignmentContractSchedule.UpdateCount = activityToBeModify.UpdateCount.CalculateUpdateCount();
                                dbAssignmentContractSchedule.ModifiedBy = activityToBeModify.ModifiedBy;
                            }
                        });
                        _assignmentContractRateScheduleRepository.AutoSave = false;
                        _assignmentContractRateScheduleRepository.Update(dbAssignmentContractSchedules);
                        if (commitChange)
                            _assignmentContractRateScheduleRepository.ForceSave();
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }
            finally
            {
                _assignmentContractRateScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentContractRateSchedules(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                               ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                               ref IList<DbModel.Assignment> dbAssignment,
                                                               ref IList<DbModel.ContractSchedule> dbContractSchedules,
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
                var recordToBeDelete = FilterRecord(assignmentContractRateSchedules, ValidationType.Delete);

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentContractRateSchedules,
                                                       ValidationType.Update,
                                                       ref recordToBeDelete,
                                                       ref dbAssignmentContractRateSchedules,
                                                       ref dbContractSchedules,
                                                       ref dbAssignment);

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(response.Result)) && dbAssignmentContractRateSchedules?.Count > 0)
                    {
                        _assignmentContractRateScheduleRepository.AutoSave = false;
                        _assignmentContractRateScheduleRepository.Delete(dbAssignmentContractRateSchedules);
                        if (commitChange)
                            _assignmentContractRateScheduleRepository.ForceSave();
                    }
                    else
                        return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }
            finally
            {
                _assignmentContractRateScheduleRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<AssignmentContractRateSchedule> FilterRecord(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                                    ValidationType filterType)
        {
            IList<AssignmentContractRateSchedule> filteredActivitys = null;

            if (filterType == ValidationType.Add)
                filteredActivitys = assignmentContractRateSchedules?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredActivitys = assignmentContractRateSchedules?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredActivitys = assignmentContractRateSchedules?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredActivitys;
        }

        private IList<DbModel.AssignmentContractSchedule> GetAssignmentContractSchedules(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules)
        {
            IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules = null;
            if (assignmentContractRateSchedules?.Count > 0)
            {
                var assignmentContractRateScheduleIds = assignmentContractRateSchedules.Select(x => x.AssignmentContractRateScheduleId).Distinct().ToList();
                dbAssignmentContractSchedules = _assignmentContractRateScheduleRepository.FindBy(x => assignmentContractRateScheduleIds.Contains(x.Id)).ToList();
            }

            return dbAssignmentContractSchedules;
        }

        private Response IsRecordValidForProcess(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                ValidationType validationType,
                                                ref IList<AssignmentContractRateSchedule> filteredAssignmentContractRateSchedules,
                                                ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                                ref IList<DbModel.Assignment> dbAssignment)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentContractRateSchedules != null && assignmentContractRateSchedules.Count > 0)
                {
                    validationMessages = new List<ValidationMessage>();
                    if (filteredAssignmentContractRateSchedules == null || filteredAssignmentContractRateSchedules.Count <= 0)
                        filteredAssignmentContractRateSchedules = FilterRecord(assignmentContractRateSchedules, validationType);

                    if (filteredAssignmentContractRateSchedules != null && filteredAssignmentContractRateSchedules?.Count > 0)
                    {
                        result = IsValidPayload(filteredAssignmentContractRateSchedules, validationType, ref validationMessages);
                        if (filteredAssignmentContractRateSchedules?.Count > 0 && result)
                        {
                            IList<int> moduleNotExists = null;

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                var assignmentContractRateScheduleIds = filteredAssignmentContractRateSchedules.Where(x => x.AssignmentContractRateScheduleId.HasValue).Select(x => x.AssignmentContractRateScheduleId.Value).ToList();

                                if (dbAssignmentContractRateSchedules == null || dbAssignmentContractRateSchedules?.Count == 0)
                                    dbAssignmentContractRateSchedules = GetAssignmentContractSchedules(filteredAssignmentContractRateSchedules);

                                result = IsAssignmentContractRateScheduleExistInDb(assignmentContractRateScheduleIds,
                                                                                   dbAssignmentContractRateSchedules,
                                                                                   ref moduleNotExists,
                                                                                   ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidaForUpdate(filteredAssignmentContractRateSchedules,
                                                                     dbAssignmentContractRateSchedules,
                                                                     ref dbAssignment,
                                                                     ref dbContractSchedules,
                                                                     ref validationMessages,
                                                                     validationType);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidaForAdd(filteredAssignmentContractRateSchedules,
                                                              dbAssignmentContractRateSchedules,
                                                              ref dbAssignment,
                                                              ref dbContractSchedules,
                                                              ref validationMessages,
                                                              validationType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _assignmentContractRateScheduleValidationService.Validate(JsonConvert.SerializeObject(assignmentContractRateSchedules), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsAssignmentContractRateScheduleExistInDb(IList<int> assignmentContractRateScheduleIds,
                                                               IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                               ref IList<int> assignmentContractRateScheduleIdsNotExists,
                                                               ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentContractRateSchedules == null)
                dbAssignmentContractRateSchedules = new List<DbModel.AssignmentContractSchedule>();

            var validMessages = validationMessages;

            if (assignmentContractRateScheduleIds?.Count > 0)
            {
                assignmentContractRateScheduleIdsNotExists = assignmentContractRateScheduleIds.Where(x => !dbAssignmentContractRateSchedules.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentContractRateScheduleIdsNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentContractRateScheduleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForAdd(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                         IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                         ref IList<DbModel.Assignment> dbAssignment,
                                         ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                         ref IList<ValidationMessage> validationMessages,
                                         ValidationType validationType)
        {

            //IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Assignment> dbAssignments = null;
            if (dbAssignment != null)
                dbAssignments = dbAssignment;

            var assignmentContractRateScheduleIds = assignmentContractRateSchedules.Where(x => x.AssignmentContractRateScheduleId.HasValue).Select(x => x.AssignmentContractRateScheduleId.Value).Distinct().ToList();

            var assignmentIds = assignmentContractRateSchedules.Where(x => x.AssignmentId > 0).Select(x => x.AssignmentId.Value).Distinct().ToList();
            var result = _assignmentService.IsValidAssignment(assignmentIds,
                                                         ref dbAssignments,
                                                         ref messages
                                                         );
            if (result)
            {
                var contractScheduleIds = assignmentContractRateSchedules.Where(x => x.ContractScheduleId.HasValue).Select(x => x.ContractScheduleId.Value).Distinct().ToList();
                if (dbContractSchedules.Count == 0)
                    result = _contractScheduleService.IsValidContractSchedule(contractScheduleIds,
                                                                              ref dbContractSchedules,
                                                                              ref messages);
                if (result)
                {
                    result = IsUniqueAssignmentContractSchedule(assignmentContractRateSchedules,
                                                                dbAssignmentContractRateSchedules,
                                                                validationType,
                                                                ref messages);
                }
            }


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return result;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                             IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                             ref IList<DbModel.Assignment> dbAssignment,
                                             ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                             ref IList<ValidationMessage> validationMessages,
                                             ValidationType validationType)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Assignment> dbAssignments = null;
            if (dbAssignment != null)
                dbAssignments = dbAssignment;

            //assignmentContractRateSchedules.Select(x => x.AssignmentContractRateScheduleId).ToList()
            //       .ForEach(x1 =>
            //       {
            //           var isExist = dbAssignmentContractRateSchedules.Any(x2 => x2.Id == x1);
            //           if (!isExist)
            //               messages.Add(_messageDescriptions, x1, MessageType.AssignmentContractRateScheduleNotExists, x1);
            //       });

            //if (messages?.Count <= 0)
            //{
                if (IsRecordUpdateCountMatching(assignmentContractRateSchedules,
                                                dbAssignmentContractRateSchedules,
                                                ref messages))
                {
                    var assignmentIds = assignmentContractRateSchedules.Where(x => x.AssignmentId.HasValue).Select(x => x.AssignmentId.Value).Distinct().ToList();
                    if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages))
                    {
                        var contractScheduleIds = assignmentContractRateSchedules.Where(x => x.ContractScheduleId.HasValue).Select(x => x.ContractScheduleId.Value).Distinct().ToList();
                        if (_contractScheduleService.IsValidContractSchedule(contractScheduleIds,
                                                                             ref dbContractSchedules,
                                                                             ref messages))
                        {
                            IsUniqueAssignmentContractSchedule(assignmentContractRateSchedules,
                                                               dbAssignmentContractRateSchedules,
                                                               validationType,
                                                               ref messages);
                        }
                    }
                }
            //}

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                 IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentContractRateSchedules.Where(x => !dbAssignmentContractRateSchedules.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentContractRateScheduleId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentContractRateScheduleUpdateCountMisMatch, x.AssignmentContractRateScheduleId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsUniqueAssignmentContractSchedule(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                        IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                        ValidationType validationType,
                                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentContractSchedule> contractScheduleExists = null;
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            contractScheduleExists = _assignmentContractRateScheduleRepository.IsUniqueAssignmentContractSchedules(assignmentContractRateSchedules,
                                                                                                                   dbAssignmentContractRateSchedules,
                                                                                                                   validationType);

            if (contractScheduleExists != null)
            {
                string existingScheduleNames = string.Join(',', contractScheduleExists?.Select(x => x.ContractSchedule.Name).ToArray());
                if (contractScheduleExists?.Count > 0 && !string.IsNullOrEmpty(existingScheduleNames))
                    messages.Add(_messageDescriptions, existingScheduleNames, MessageType.AssignmentContractRateScheduleDuplicateRecord, existingScheduleNames);
            }
            //contractScheduleExists?.ToList().ForEach(x =>
            //{
            //    messages.Add(_messageDescriptions, x.ContractSchedule.Name, MessageType.AssignmentContractRateScheduleDuplicateRecord, x.ContractSchedule.Name);

            //});

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        #endregion
    }
}
