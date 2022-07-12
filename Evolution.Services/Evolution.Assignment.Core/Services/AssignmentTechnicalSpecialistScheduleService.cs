using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
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
    public class AssignmentTechnicalSpecialistScheduleService : IAssignmentTechnicalSpecialistScheduleService
    {
        private readonly IAppLogger<AssignmentTechnicalSpecialistScheduleService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IAssignmentTechnicalSpecialistScheduleRepository _assignmentTechnicalSpecialistScheduleRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentTechnicalSpecialistScheduleValidationService _assignmentTechnicalSpecialistScheduleValidationService = null;
        private readonly IAssignmentTechnicalSpecilaistService _assignmentTechnicalSpecilaistService = null;
        private readonly IContractScheduleService _contractScheduleService = null;
        private readonly ITechnicalSpecialistPayScheduleService _technicalSpecialistPayScheduleService = null;

        #region Constructor

        public AssignmentTechnicalSpecialistScheduleService(IMapper mapper,
                        IAssignmentTechnicalSpecialistScheduleRepository assignmentTechnicalSpecialistScheduleRepository,
                        IAppLogger<AssignmentTechnicalSpecialistScheduleService> logger,
                        IAssignmentTechnicalSpecialistScheduleValidationService assignmentTechnicalSpecialistScheduleValidationService,
                        IAssignmentTechnicalSpecilaistService assignmentTechnicalSpecilaistService,
                        IContractScheduleService contractScheduleService,
                        ITechnicalSpecialistPayScheduleService technicalSpecialistPayScheduleService,
                        JObject messages)
        {
            _assignmentTechnicalSpecialistScheduleRepository = assignmentTechnicalSpecialistScheduleRepository;
            _logger = logger;
            _mapper = mapper;
            _assignmentTechnicalSpecialistScheduleValidationService = assignmentTechnicalSpecialistScheduleValidationService;
            _assignmentTechnicalSpecilaistService = assignmentTechnicalSpecilaistService;
            _contractScheduleService = contractScheduleService;
            _technicalSpecialistPayScheduleService = technicalSpecialistPayScheduleService;
            _messageDescriptions = messages;
        }

        #endregion

        #region Public Methods

        #region Add

        public Response Add(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules = null;
            return AddAssignmentTechnicalSpecialistSchedules(assignmentTechnicalSpecialistSchedules, ref dbAssignmentTechnicalSpecialistSchedules, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                              ref IList<DbModel.ContractSchedule> dbContractSchedule,ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule,
                                                               bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddAssignmentTechnicalSpecialistSchedules(assignmentTechnicalSpecialistSchedules, ref dbAssignmentTechnicalSpecialistSchedules, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveAssignmentTechnicalSpecialistSchedules(assignmentTechnicalSpecialistSchedules, commitChange, isDbValidationRequired);
        }
        public Response Delete(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                              ref IList<DbModel.ContractSchedule> dbContractSchedule, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule,
                                                               bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveAssignmentTechnicalSpecialistSchedules(assignmentTechnicalSpecialistSchedules, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Get

        public Response Get(DomainModel.AssignmentTechnicalSpecialistSchedule searchModel)
        {
            IList<DomainModel.AssignmentTechnicalSpecialistSchedule> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = _assignmentTechnicalSpecialistScheduleRepository.Search(searchModel);
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


        public Response Get(int assignmentId, int epin)
        {
           DomainModel.AssignmentTechSpecSchedules result = null;
            Exception exception = null;
            try
            {
                string[] includes = new string[] {
                                                    "AssignmentTechnicalSpecialist",
                                                    "ContractChargeSchedule.ContractRate",
                                                    "TechnicalSpecialistPaySchedule.TechnicalSpecialistPayRate",
                                                 };
                result = this._assignmentTechnicalSpecialistScheduleRepository.GetAssignmentTechSpecRateSchedules(assignmentId, epin, includes);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId, epin);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response Get(int assignmentId,
                            IList<DbModel.Data> dbExpenseType)
        {
            DomainModel.AssignmentTechSpecSchedules result = null;
            Exception exception = null;
            try
            {
                var includes = new string[] {
                                                    "AssignmentTechnicalSpecialist",
                                                    "ContractChargeSchedule.ContractRate",
                                                    "TechnicalSpecialistPaySchedule.TechnicalSpecialistPayRate"
                                            };
                result = this._assignmentTechnicalSpecialistScheduleRepository.GetAssignmentTechSpecRateSchedules(assignmentId,dbExpenseType,includes);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        #endregion

        #region Modify

        public Response Modify(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules = null;
            return UpdateAssignmentTechnicalSpecialistSchedules(assignmentTechnicalSpecialistSchedules, ref dbAssignmentTechnicalSpecialistSchedules, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                              ref IList<DbModel.ContractSchedule> dbContractSchedule, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule,
                                                               bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateAssignmentTechnicalSpecialistSchedules(assignmentTechnicalSpecialistSchedules, ref dbAssignmentTechnicalSpecialistSchedules, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Validation Check

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType validationType)
        {
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules = null;
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.ContractSchedule> dbContractSchedule = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbechnicalSpecialistPaySchedule = null;
            return IsRecordValidForProcess(assignmentTechnicalSpecialistSchedules, validationType, ref dbAssignmentTechnicalSpecialistSchedules, ref dbAssignmentTechnicalSpecialist,ref  dbContractSchedule,ref  dbechnicalSpecialistPaySchedule);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType validationType,
                                                ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                                                ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                ref IList<DbModel.ContractSchedule> dbcontractSchedules,
                                                ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedule)
        {
            IList<DomainModel.AssignmentTechnicalSpecialistSchedule> filteredAssignmentTechnicalSpecialistSchedules = null;
            return IsRecordValidForProcess(assignmentTechnicalSpecialistSchedules, validationType, ref filteredAssignmentTechnicalSpecialistSchedules, 
                                           ref dbAssignmentTechnicalSpecialistSchedules,ref dbAssignmentTechnicalSpecialist,ref dbcontractSchedules,ref dbTechnicalSpecialistPaySchedule);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType validationType, 
                                                   IList<DbRepository.Models.SqlDatabaseContext.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                                                   IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                   IList<DbModel.ContractSchedule> dbContractSchedule,
                                                   IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedule)
        {
            return IsRecordValidForProcess(assignmentTechnicalSpecialistSchedules, validationType, ref dbAssignmentTechnicalSpecialistSchedules,ref dbAssignmentTechnicalSpecialist,ref dbContractSchedule,ref dbTechnicalSpecialistPaySchedule);
        }

        public bool IsValidAssignmentTechnicalSpecialistSchedules(IList<int> assignmentTechnicalSpecialistScheduleIds, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbAssigTechSpecSchedules = _assignmentTechnicalSpecialistScheduleRepository.FindBy(x => assignmentTechnicalSpecialistScheduleIds.Contains(x.Id)).ToList();
            var invalidAssignmentTechnicalSpecilaistSchedules = assignmentTechnicalSpecialistScheduleIds.Where(x => !dbAssigTechSpecSchedules.Any(x1 => x1.Id == x));

            invalidAssignmentTechnicalSpecilaistSchedules.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleInvalidId, x);
            });

            dbAssignmentTechnicalSpecialistSchedules = dbAssigTechSpecSchedules;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        #endregion

        #endregion

        #region Private Methods

        private Response AddAssignmentTechnicalSpecialistSchedules(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedules = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentTechnicalSpecialistSchedules, ValidationType.Add);

                //if (dbAssignmentTechnicalSpecialistSchedules == null)
                //    dbAssignmentTechnicalSpecialistSchedules = GetAssignmentTechnicalSpecialistSchedules(recordToBeAdd);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentTechnicalSpecialistSchedules, ValidationType.Add, ref recordToBeAdd, ref dbAssignmentTechnicalSpecialistSchedules,
                                                                         ref dbAssignmentTechnicalSpecialist, ref dbContractSchedules, ref dbTechnicalSpecialistPaySchedules);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
                    _assignmentTechnicalSpecialistScheduleRepository.Add(_mapper.Map<IList<DbModel.AssignmentTechnicalSpecialistSchedule>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                    }));
                    if (commitChange)
                        _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }
           // finally { _assignmentTechnicalSpecialistScheduleRepository.Dispose(); }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentTechnicalSpecialistSchedules(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, 
                                                    ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentTechnicalSpecialistSchedule> result = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedules = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentTechnicalSpecialistSchedules, ValidationType.Update);

                if (dbAssignmentTechnicalSpecialistSchedules == null)
                    dbAssignmentTechnicalSpecialistSchedules = GetAssignmentTechnicalSpecialistSchedules(recordToBeModify);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentTechnicalSpecialistSchedules, ValidationType.Update, ref recordToBeModify, ref dbAssignmentTechnicalSpecialistSchedules,ref dbAssignmentTechnicalSpecialist,ref dbContractSchedules,ref dbTechnicalSpecialistPaySchedules);

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbAssignmentTechnicalSpecialistSchedules?.Count > 0))
                {
                    dbAssignmentTechnicalSpecialistSchedules.ToList().ForEach(dbAssignmentTechnicalSpecialistSchedule =>
                    {
                        var activityToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentTechnicalSpecialistScheduleId == dbAssignmentTechnicalSpecialistSchedule.Id);
                        dbAssignmentTechnicalSpecialistSchedule.AssignmentTechnicalSpecialistId = activityToBeModify.AssignmentTechnicalSpecilaistId.Value;
                        dbAssignmentTechnicalSpecialistSchedule.ContractChargeScheduleId = activityToBeModify.ContractScheduleId.Value;
                        dbAssignmentTechnicalSpecialistSchedule.AssignmentTechnicalSpecialistId = activityToBeModify.AssignmentTechnicalSpecilaistId.Value;
                        dbAssignmentTechnicalSpecialistSchedule.TechnicalSpecialistPayScheduleId = activityToBeModify.TechnicalSpecialistPayScheduleId.Value;
                        dbAssignmentTechnicalSpecialistSchedule.LastModification = DateTime.UtcNow;
                        dbAssignmentTechnicalSpecialistSchedule.UpdateCount = activityToBeModify.UpdateCount.CalculateUpdateCount();
                        dbAssignmentTechnicalSpecialistSchedule.ModifiedBy = activityToBeModify.ModifiedBy;
                    });
                    _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
                    _assignmentTechnicalSpecialistScheduleRepository.Update(dbAssignmentTechnicalSpecialistSchedules);
                    if (commitChange)
                        _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }
            finally
            {
                _assignmentTechnicalSpecialistScheduleRepository.AutoSave = true;
                //_assignmentTechnicalSpecialistScheduleRepository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentTechnicalSpecialistSchedules(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules = null;
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedules = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentTechnicalSpecialistSchedules, ValidationType.Delete, ref dbAssignmentTechnicalSpecialistSchedules,
                                                          ref dbAssignmentTechnicalSpecialist,ref dbContractSchedules,ref dbTechnicalSpecialistPaySchedules);

                if (!isDbValidationRequire || (Convert.ToBoolean(response.Result) && dbAssignmentTechnicalSpecialistSchedules?.Count > 0))
                {
                    _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
                    _assignmentTechnicalSpecialistScheduleRepository.Delete(dbAssignmentTechnicalSpecialistSchedules);
                    if (commitChange)
                        _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }
            finally
            {
                _assignmentTechnicalSpecialistScheduleRepository.AutoSave = true;
                //_assignmentTechnicalSpecialistScheduleRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<DomainModel.AssignmentTechnicalSpecialistSchedule> FilterRecord(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType filterType)
        {
            IList<DomainModel.AssignmentTechnicalSpecialistSchedule> filteredActivitys = null;

            if (filterType == ValidationType.Add)
                filteredActivitys = assignmentTechnicalSpecialistSchedules?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredActivitys = assignmentTechnicalSpecialistSchedules?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredActivitys = assignmentTechnicalSpecialistSchedules?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredActivitys;
        }

        private IList<DbModel.AssignmentTechnicalSpecialistSchedule> GetAssignmentTechnicalSpecialistSchedules(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules)
        {
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules = null;
            if (assignmentTechnicalSpecialistSchedules?.Count > 0)
            {
                var assignmentTechnicalSpecialistScheduleIds = assignmentTechnicalSpecialistSchedules.Select(x => x.AssignmentTechnicalSpecialistScheduleId).Distinct().ToList();
                dbAssignmentTechnicalSpecialistSchedules = _assignmentTechnicalSpecialistScheduleRepository.FindBy(x => assignmentTechnicalSpecialistScheduleIds.Contains(x.Id)).ToList();
            }

            return dbAssignmentTechnicalSpecialistSchedules;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules,
                                ValidationType validationType,
                                ref IList<DomainModel.AssignmentTechnicalSpecialistSchedule> filteredAssignmentTechnicalSpecialistSchedules,
                                ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                                ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                ref IList<DbModel.ContractSchedule> dbContractSchedule,
                                ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedule)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentTechnicalSpecialistSchedules != null && assignmentTechnicalSpecialistSchedules.Count > 0)
                {
                    validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentTechnicalSpecialistSchedules == null || filteredAssignmentTechnicalSpecialistSchedules.Count <= 0)
                        filteredAssignmentTechnicalSpecialistSchedules = FilterRecord(assignmentTechnicalSpecialistSchedules, validationType);

                    result = IsValidPayload(filteredAssignmentTechnicalSpecialistSchedules, validationType, ref validationMessages);
                    if (filteredAssignmentTechnicalSpecialistSchedules?.Count > 0 && result)
                    {
                        IList<int> moduleNotExists = null;

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            var assignmentTechnicalSpecialistScheduleIds = filteredAssignmentTechnicalSpecialistSchedules.Where(x => x.AssignmentTechnicalSpecialistScheduleId.HasValue).Select(x => x.AssignmentTechnicalSpecialistScheduleId.Value).Distinct().ToList();

                            if (dbAssignmentTechnicalSpecialistSchedules == null || dbAssignmentTechnicalSpecialistSchedules.Count <= 0)
                            {
                                dbAssignmentTechnicalSpecialistSchedules = GetAssignmentTechnicalSpecialistSchedules(filteredAssignmentTechnicalSpecialistSchedules);
                            }

                            result = IsAssignmentTechnicalSpecialistScheduleExistInDb(assignmentTechnicalSpecialistScheduleIds, dbAssignmentTechnicalSpecialistSchedules, ref moduleNotExists, ref validationMessages);

                            if (result && validationType == ValidationType.Update)
                                result = IsRecordValidaForUpdate(filteredAssignmentTechnicalSpecialistSchedules, dbAssignmentTechnicalSpecialistSchedules,
                                                                ref  dbAssignmentTechnicalSpecialist,ref dbContractSchedule, ref dbTechnicalSpecialistPaySchedule, ref validationMessages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidaForAdd(filteredAssignmentTechnicalSpecialistSchedules, dbAssignmentTechnicalSpecialistSchedules, 
                                                         ref dbAssignmentTechnicalSpecialist, ref dbContractSchedule, ref dbTechnicalSpecialistPaySchedule, ref validationMessages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules,
                    ValidationType validationType,
                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _assignmentTechnicalSpecialistScheduleValidationService.Validate(JsonConvert.SerializeObject(assignmentTechnicalSpecialistSchedules), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsAssignmentTechnicalSpecialistScheduleExistInDb(IList<int> assignmentTechnicalSpecialistScheduleIds,
                        IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                        ref IList<int> assignmentTechnicalSpecialistScheduleIdsNotExists,
                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentTechnicalSpecialistSchedules == null)
                dbAssignmentTechnicalSpecialistSchedules = new List<DbModel.AssignmentTechnicalSpecialistSchedule>();

            var validMessages = validationMessages;

            if (assignmentTechnicalSpecialistScheduleIds?.Count > 0)
            {
                assignmentTechnicalSpecialistScheduleIdsNotExists = assignmentTechnicalSpecialistScheduleIds.Where(x => !dbAssignmentTechnicalSpecialistSchedules.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentTechnicalSpecialistScheduleIdsNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForAdd(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, 
                                          IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, 
                                          ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                          ref IList<DbModel.ContractSchedule> dbContractSchedule,
                                          ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedule,
                                          ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedules = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assignmentTechnicalSpecialistScheduleIds = assignmentTechnicalSpecialistSchedules.Where(x => x.AssignmentTechnicalSpecialistScheduleId.HasValue).Select(x => x.AssignmentTechnicalSpecialistScheduleId.Value).Distinct().ToList();

            //var result = (!IsAssignmentTechnicalSpecialistScheduleExistInDb(assignmentTechnicalSpecialistScheduleIds, dbAssignmentTechnicalSpecialistSchedules, ref assignmentTechnicalSpecialistScheduleNotExists, ref messages) &&
            //              messages.Count == assignmentTechnicalSpecialistScheduleIds.Count);
            //if (!result)
            //{
            //    var assignmentTechnicalSpecialistScheduleAlreadyExists = assignmentTechnicalSpecialistScheduleIds.Where(x => !assignmentTechnicalSpecialistScheduleNotExists.Contains(x)).ToList();
            //    assignmentTechnicalSpecialistScheduleAlreadyExists?.ForEach(x =>
            //    {
            //        messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleAlreadyExist, x);
            //    });
            //}


            var assignmentTechnicalSpecilaistIds = assignmentTechnicalSpecialistSchedules.Where(x => x.AssignmentTechnicalSpecilaistId.HasValue).Select(x => x.AssignmentTechnicalSpecilaistId.Value).Distinct().ToList();
            var result = _assignmentTechnicalSpecilaistService.IsValidAssignmentTechnicalSpecialist(assignmentTechnicalSpecilaistIds, ref dbAssignmentTechnicalSpecialists, ref messages);
            if (result)
            {
                var contractScheduleIds = assignmentTechnicalSpecialistSchedules.Where(x => x.ContractScheduleId.HasValue).Select(x => x.ContractScheduleId.Value).Distinct().ToList();
                result = _contractScheduleService.IsValidContractSchedule(contractScheduleIds, ref dbContractSchedules, ref messages);
                if (result)
                {
                    var technicalSpecialistPayScheduleIds = assignmentTechnicalSpecialistSchedules.Where(x => x.TechnicalSpecialistPayScheduleId.HasValue).Select(x => x.TechnicalSpecialistPayScheduleId.Value).Distinct().ToList();
                    result = _technicalSpecialistPayScheduleService.IsValidPaySchedule(technicalSpecialistPayScheduleIds, ref dbTechnicalSpecialistPaySchedules, ref messages);
                    if (result)
                    {
                        result = IsUniqueTechnicalSpecialistPaySchedule(assignmentTechnicalSpecialistSchedules, ref messages);
                    }

                }


                if (messages.Count > 0)
                    validationMessages.AddRange(messages);

               
            }
            return result;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, 
                                              IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                                              ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                              ref IList<DbModel.ContractSchedule> dbcontractSchedule,
                                              ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedule,
                                                ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedules = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            assignmentTechnicalSpecialistSchedules.Select(x => x.AssignmentTechnicalSpecialistScheduleId).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbAssignmentTechnicalSpecialistSchedules.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.AssignmentTechnicalSpecialistScheduleNotExists, x1);
                   });

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentTechnicalSpecialistSchedules, dbAssignmentTechnicalSpecialistSchedules, ref messages))
                {
                    var assignmentTechnicalSpecilaistIds = assignmentTechnicalSpecialistSchedules.Where(x => x.AssignmentTechnicalSpecilaistId.HasValue).Select(x => x.AssignmentTechnicalSpecilaistId.Value).Distinct().ToList();
                    if (_assignmentTechnicalSpecilaistService.IsValidAssignmentTechnicalSpecialist(assignmentTechnicalSpecilaistIds, ref dbAssignmentTechnicalSpecialist, ref messages))
                    {
                        var contractScheduleIds = assignmentTechnicalSpecialistSchedules.Where(x => x.ContractScheduleId.HasValue).Select(x => x.ContractScheduleId.Value).Distinct().ToList();
                        if (_contractScheduleService.IsValidContractSchedule(contractScheduleIds, ref dbContractSchedules, ref messages))
                        {
                            var technicalSpecialistPayScheduleIds = assignmentTechnicalSpecialistSchedules.Where(x => x.TechnicalSpecialistPayScheduleId.HasValue).Select(x => x.TechnicalSpecialistPayScheduleId.Value).Distinct().ToList();
                            if (_technicalSpecialistPayScheduleService.IsValidPaySchedule(technicalSpecialistPayScheduleIds,ref dbTechnicalSpecialistPaySchedules, ref messages))
                              IsUniqueTechnicalSpecialistPaySchedule(assignmentTechnicalSpecialistSchedules, ref messages);
                        }
                    }
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
 
        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentTechnicalSpecialistSchedules.Where(x => !dbAssignmentTechnicalSpecialistSchedules.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentTechnicalSpecialistScheduleId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleUpdateCountMisMatch, x.AssignmentTechnicalSpecialistScheduleId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsUniqueTechnicalSpecialistPaySchedule(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assingmentTechSpecSchedules = assignmentTechnicalSpecialistSchedules.Select(x => new { x.AssignmentTechnicalSpecialistScheduleId, x.ContractScheduleId, x.AssignmentTechnicalSpecilaistId, x.TechnicalSpecialistPayScheduleId }).ToList();
            var duplicateRecords = _assignmentTechnicalSpecialistScheduleRepository.FindBy(x => assingmentTechSpecSchedules.Any(x1 => x1.ContractScheduleId == x.ContractChargeScheduleId && x1.AssignmentTechnicalSpecilaistId == x.AssignmentTechnicalSpecialistId && x1.TechnicalSpecialistPayScheduleId == x.TechnicalSpecialistPayScheduleId && x1.AssignmentTechnicalSpecialistScheduleId != x.Id)).ToList();

            assignmentTechnicalSpecialistSchedules.Where(x => duplicateRecords.Any(x1 => x.ContractScheduleId == x1.ContractChargeScheduleId && x.AssignmentTechnicalSpecilaistId == x1.AssignmentTechnicalSpecialistId && x.TechnicalSpecialistPayScheduleId == x1.TechnicalSpecialistPayScheduleId))?.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentTechnicalSpecialistScheduleDuplicateRecord, x.AssignmentTechnicalSpecilaistId,x.ContractScheduleId,x.TechnicalSpecialistPayScheduleId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        #endregion
    }
}
