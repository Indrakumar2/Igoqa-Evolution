using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
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
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentContributionRevenueCostService : IAssignmentContributionRevenueCostService
    {
        private readonly IAppLogger<AssignmentContributionRevenueCostService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IAssignmentContributionRevenueCostRepository _assignmentContributionRevenueCostRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentContributionRevenueCostValidationService _assignmentContributionRevenueCostValidationService = null;
        private readonly IAssignmentContributionCalculationService _assignmentContributionCalculationService = null;

        #region Constructor

        public AssignmentContributionRevenueCostService(IMapper mapper,
                                IAssignmentContributionRevenueCostRepository assignmentContributionRevenueCostRepository,
                                IAssignmentContributionCalculationService assignmentContributionCalculationService,
                                IAppLogger<AssignmentContributionRevenueCostService> logger,
                                IAssignmentContributionRevenueCostValidationService assignmentContributionRevenueCostValidationService,
                                JObject messages)
        {
            _assignmentContributionRevenueCostRepository = assignmentContributionRevenueCostRepository;
            _assignmentContributionCalculationService = assignmentContributionCalculationService;
            _logger = logger;
            _mapper = mapper;
            _assignmentContributionRevenueCostValidationService = assignmentContributionRevenueCostValidationService;
            _messageDescriptions = messages;
        }

        #endregion

        #region Public Methods

        #region Add

        public Response Add(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts = null;
            return AddAssignmentContributionRevenueCosts(contributionRevenueCosts, ref dbAssignmentContributionRevenueCosts, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ref IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddAssignmentContributionRevenueCosts(contributionRevenueCosts, ref dbAssignmentContributionRevenueCosts, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveAssignmentContractRateSchedules(contributionRevenueCosts, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Get

        public Response Get(DomainModel.AssignmentContributionRevenueCost searchModel)
        {
            IList<DomainModel.AssignmentContributionRevenueCost> result = null;
            Exception exception = null;
            try
            {
                result = _assignmentContributionRevenueCostRepository.Search(searchModel);
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

        public Response Modify(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts = null;
            return UpdateAssignmentContributionRevenueCosts(contributionRevenueCosts, ref dbAssignmentContributionRevenueCosts, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ref IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateAssignmentContributionRevenueCosts(contributionRevenueCosts, ref dbAssignmentContributionRevenueCosts, commitChange, isDbValidationRequired);
        }
        #endregion

        #region Validation Check

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ValidationType validationType)
        {
            IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts = null;
            return IsRecordValidForProcess(contributionRevenueCosts, validationType, ref dbAssignmentContributionRevenueCosts);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ValidationType validationType, ref IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts)
        {
            IList<DomainModel.AssignmentContributionRevenueCost> filteredAssignmentContributionRevenueCosts = null;
            return IsRecordValidForProcess(contributionRevenueCosts, validationType, ref filteredAssignmentContributionRevenueCosts, ref dbAssignmentContributionRevenueCosts);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ValidationType validationType, IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts)
        {
            return IsRecordValidForProcess(contributionRevenueCosts, validationType, ref dbAssignmentContributionRevenueCosts);
        }
        #endregion

        #endregion

        #region Private Methods

        private Response AddAssignmentContributionRevenueCosts(IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts, ref IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCostss, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentContributionRevenueCosts, ValidationType.Add);

                if (dbAssignmentContributionRevenueCostss == null)
                    dbAssignmentContributionRevenueCostss = GetAssignmentContributionRevenueCosts(recordToBeAdd);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentContributionRevenueCosts, ValidationType.Add, ref recordToBeAdd, ref dbAssignmentContributionRevenueCostss);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _assignmentContributionRevenueCostRepository.AutoSave = false;
                    _assignmentContributionRevenueCostRepository.Add(_mapper.Map<IList<DbModel.AssignmentContributionRevenueCost>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                    }));
                    if (commitChange)
                        _assignmentContributionRevenueCostRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionRevenueCosts);
            }
            finally {
                _assignmentContributionRevenueCostRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentContributionRevenueCosts(IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts, ref IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentContributionRevenueCost> result = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentContributionRevenueCosts, ValidationType.Update);

                if (dbAssignmentContributionRevenueCosts == null)
                    dbAssignmentContributionRevenueCosts = GetAssignmentContributionRevenueCosts(recordToBeModify);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentContributionRevenueCosts, ValidationType.Update, ref recordToBeModify, ref dbAssignmentContributionRevenueCosts);

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbAssignmentContributionRevenueCosts?.Count > 0))
                {
                    dbAssignmentContributionRevenueCosts.ToList().ForEach(dbAssignmentContractSchedule =>
                    {
                        var contributionRevenueCostToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentContributionRevenueCostId == dbAssignmentContractSchedule.Id);
                        dbAssignmentContractSchedule.AssignmentContributionCalculationId = contributionRevenueCostToBeModify.AssignmentContributionCalculationId.Value;
                        dbAssignmentContractSchedule.SectionType = contributionRevenueCostToBeModify.Type;
                        dbAssignmentContractSchedule.Value = contributionRevenueCostToBeModify.Value;
                        dbAssignmentContractSchedule.Description = contributionRevenueCostToBeModify.Description;
                        dbAssignmentContractSchedule.LastModification = DateTime.UtcNow;
                        dbAssignmentContractSchedule.UpdateCount = contributionRevenueCostToBeModify.UpdateCount.CalculateUpdateCount();
                        dbAssignmentContractSchedule.ModifiedBy = contributionRevenueCostToBeModify.ModifiedBy;
                    });
                    _assignmentContributionRevenueCostRepository.AutoSave = false;
                    _assignmentContributionRevenueCostRepository.Update(dbAssignmentContributionRevenueCosts);
                    if (commitChange)
                        _assignmentContributionRevenueCostRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionRevenueCosts);
            }
            finally
            {
                _assignmentContributionRevenueCostRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentContractRateSchedules(IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentContributionRevenueCosts, ValidationType.Delete, ref dbAssignmentContributionRevenueCosts);

                if (!isDbValidationRequire || (Convert.ToBoolean(response.Result) && dbAssignmentContributionRevenueCosts?.Count > 0))
                {
                    _assignmentContributionRevenueCostRepository.AutoSave = false;
                    _assignmentContributionRevenueCostRepository.Delete(dbAssignmentContributionRevenueCosts);
                    if (commitChange)
                        _assignmentContributionRevenueCostRepository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionRevenueCosts);
            }
            finally
            {
                _assignmentContributionRevenueCostRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
         
        private IList<DomainModel.AssignmentContributionRevenueCost> FilterRecord(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts, ValidationType filterType)
        {
            IList<DomainModel.AssignmentContributionRevenueCost> filteredActivitys = null;

            if (filterType == ValidationType.Add)
                filteredActivitys = assignmentContributionRevenueCosts?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredActivitys = assignmentContributionRevenueCosts?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredActivitys = assignmentContributionRevenueCosts?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredActivitys;
        }

        private IList<DbModel.AssignmentContributionRevenueCost> GetAssignmentContributionRevenueCosts(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts)
        {
            IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts = null;
            if (assignmentContributionRevenueCosts?.Count > 0)
            {
                var assignmentContributionRevenueCostIds = assignmentContributionRevenueCosts.Select(x => x.AssignmentContributionRevenueCostId).Distinct().ToList();
                dbAssignmentContributionRevenueCosts = _assignmentContributionRevenueCostRepository.FindBy(x => assignmentContributionRevenueCostIds.Contains(x.Id)).ToList();
            }

            return dbAssignmentContributionRevenueCosts;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts,
                                      ValidationType validationType,
                                      ref IList<DomainModel.AssignmentContributionRevenueCost> filteredAssignmentContributionRevenueCosts,
                                      ref IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentContributionRevenueCosts != null && assignmentContributionRevenueCosts.Count > 0)
                {
                    validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentContributionRevenueCosts == null || filteredAssignmentContributionRevenueCosts.Count <= 0)
                        filteredAssignmentContributionRevenueCosts = FilterRecord(assignmentContributionRevenueCosts, validationType);

                    result = IsValidPayload(filteredAssignmentContributionRevenueCosts, validationType, ref validationMessages);
                    if (filteredAssignmentContributionRevenueCosts?.Count > 0 && result)
                    {
                        IList<int> moduleNotExists = null;

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            var assignmentContractRateScheduleIds = filteredAssignmentContributionRevenueCosts.Where(x => x.AssignmentContributionRevenueCostId.HasValue).Select(x => x.AssignmentContributionRevenueCostId.Value).Distinct().ToList();

                            if (dbAssignmentContributionRevenueCosts == null || dbAssignmentContributionRevenueCosts?.Count == 0)
                            {
                                dbAssignmentContributionRevenueCosts = GetAssignmentContributionRevenueCosts(filteredAssignmentContributionRevenueCosts);
                            }

                            result = IsAssignmentContributionRevenueCostExistInDb(assignmentContractRateScheduleIds, dbAssignmentContributionRevenueCosts, ref moduleNotExists, ref validationMessages);

                            if (result && validationType == ValidationType.Update)
                                result = IsRecordValidaForUpdate(filteredAssignmentContributionRevenueCosts, dbAssignmentContributionRevenueCosts, ref validationMessages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidaForAdd(filteredAssignmentContributionRevenueCosts, dbAssignmentContributionRevenueCosts, ref validationMessages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionRevenueCosts);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts,
                       ValidationType validationType,
                       ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _assignmentContributionRevenueCostValidationService.Validate(JsonConvert.SerializeObject(assignmentContributionRevenueCosts), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsAssignmentContributionRevenueCostExistInDb(IList<int> assignmentContributionRevenueCostIds,
                        IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts,
                        ref IList<int> assignmentContributionRevenueCostsNotExists,
                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentContributionRevenueCosts == null)
                dbAssignmentContributionRevenueCosts = new List<DbModel.AssignmentContributionRevenueCost>();

            var validMessages = validationMessages;

            if (assignmentContributionRevenueCostIds?.Count > 0)
            {
                assignmentContributionRevenueCostsNotExists = assignmentContributionRevenueCostIds.Where(x => !dbAssignmentContributionRevenueCosts.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentContributionRevenueCostsNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentContributionRevenueCostNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForAdd(IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts, IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, ref IList<ValidationMessage> validationMessages)
        {
            IList<int> assignmentContributionRevenueCostNotExists = null;
            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assignmentContributionRevenueCostIds = assignmentContributionRevenueCosts.Where(x => x.AssignmentContributionRevenueCostId.HasValue).Select(x => x.AssignmentContributionRevenueCostId.Value).Distinct().ToList();

            var result = (!IsAssignmentContributionRevenueCostExistInDb(assignmentContributionRevenueCostIds, dbAssignmentContributionRevenueCosts, ref assignmentContributionRevenueCostNotExists, ref messages) &&
                          messages.Count == assignmentContributionRevenueCostIds.Count);
            if (!result)
            {
                var assignmentContributionRevenueCostAlreadyExists = assignmentContributionRevenueCostIds.Where(x => !assignmentContributionRevenueCostNotExists.Contains(x)).ToList();
                assignmentContributionRevenueCostAlreadyExists?.ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentContributionRevenueCostAlreadyExist, x);
                });
            }

            if (messages?.Count == 0)
            {
                var assignmentContributionCalculationIds = assignmentContributionRevenueCosts.Where(x => x.AssignmentContributionCalculationId.HasValue).Select(x => x.AssignmentContributionCalculationId.Value).Distinct().ToList();
                result = _assignmentContributionCalculationService.IsValidAssignmentContributionCalculation(assignmentContributionCalculationIds, ref dbAssignmentContributionCalculations, ref messages);
                if (result)
                { 
                        result = IsUniqueAssignmentContributionRevenueCost(assignmentContributionRevenueCosts, dbAssignmentContributionRevenueCosts, ref messages);
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return result;
        }
          
        private bool IsUniqueAssignmentContributionRevenueCost(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts, IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assingmentContributionRevenueCosts = assignmentContributionRevenueCosts.Select(x => new { x.AssignmentContributionRevenueCostId, x.AssignmentContributionCalculationId, x.Type }).ToList();
            var duplicateRecords = _assignmentContributionRevenueCostRepository.FindBy(x => assingmentContributionRevenueCosts.Any(x1 => x1.AssignmentContributionRevenueCostId != x.Id && x1.AssignmentContributionCalculationId == x.AssignmentContributionCalculationId && x1.Type == x.SectionType)).ToList();

            assingmentContributionRevenueCosts.Where(x => duplicateRecords.Any(x1 => x1.AssignmentContributionCalculationId == x.AssignmentContributionCalculationId && x.Type == x1.SectionType))?.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentContributionRevenueCostDuplicateRecord, x.Type);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts, IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            assignmentContributionRevenueCosts.Select(x => x.AssignmentContributionRevenueCostId).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbAssignmentContributionRevenueCosts.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.AssignmentContributionRevenueCostNotExists, x1);
                   });

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentContributionRevenueCosts, dbAssignmentContributionRevenueCosts, ref messages))
                {
                    var assignmentContributionCalculationIds = assignmentContributionRevenueCosts.Where(x => x.AssignmentContributionCalculationId.HasValue).Select(x => x.AssignmentContributionCalculationId.Value).Distinct().ToList();
                    if (_assignmentContributionCalculationService.IsValidAssignmentContributionCalculation(assignmentContributionCalculationIds, ref dbAssignmentContributionCalculations, ref messages))
                    {
                       IsUniqueAssignmentContributionRevenueCost(assignmentContributionRevenueCosts, dbAssignmentContributionRevenueCosts, ref messages);
                    }
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentContributionRevenueCost>  assignmentContributionRevenueCosts, IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentContributionRevenueCosts.Where(x => !dbAssignmentContributionRevenueCosts.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentContributionRevenueCostId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentContractRateScheduleUpdateCountMisMatch, x.AssignmentContributionRevenueCostId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        #endregion


    }
}
