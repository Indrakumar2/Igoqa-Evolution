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
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentContributionCalculationService : IAssignmentContributionCalculationService
    {
        private readonly IAppLogger<AssignmentContributionCalculationService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IAssignmentContributionCalculationRepository _assignmentContributionCalculationRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentContributionCalculationValidationService _validationService = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly IAssignmentContributionRevenueCostRepository _revenueCostRepository = null;

        #region Constructor
        public AssignmentContributionCalculationService(IAppLogger<AssignmentContributionCalculationService> logger,
                                                        IMapper mapper,
                                                        IAssignmentContributionCalculationRepository assignmentContributionCalculationRepository,
                                                        JObject messageDescriptions,
                                                        IAssignmentService assignmentService,
                                                        IAssignmentContributionRevenueCostRepository assignmentContributionRevenueCostRepository,
                                                        IAssignmentContributionCalculationValidationService validationService)
        {
            _logger = logger;
            _mapper = mapper;
            _assignmentContributionCalculationRepository = assignmentContributionCalculationRepository;
            _messageDescriptions = messageDescriptions;
            _revenueCostRepository = assignmentContributionRevenueCostRepository;
            _assignmentService = assignmentService;
            _validationService = validationService;
        }
        #endregion

        #region Get
        public Response Get(DomainModel.AssignmentContributionCalculation searchModel)
        {
            IList<DomainModel.AssignmentContributionCalculation> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = this._assignmentContributionCalculationRepository.Search(searchModel);
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
        public Response Add(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                            bool commitChange = true,
                            bool isValidationRequired = true,
                            int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation = null;
            return AddAssignmentContributionCalculation(assignmentContributionCalculation,
                                                        ref dbAssignment, 
                                                        ref dbAssignmentContributionCalculation, 
                                                        commitChange, 
                                                        isValidationRequired);
        }

        public Response Add(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                            bool commitChange = true,
                            bool isValidationRequired = true,
                            int? assignmentIds = null)
        {
            return AddAssignmentContributionCalculation(assignmentContributionCalculation, 
                                                        ref dbAssignment,
                                                        ref dbAssignmentContributionCalculation, 
                                                        commitChange, 
                                                        isValidationRequired);
        }

        #endregion

        #region Modify
        public Response Modify(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                               bool commitChange = true,
                               bool isValidationRequired = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation = null;
            return UpdateAssignmentContributionCalculation(assignmentContributionCalculation,
                                                            ref dbAssignment,
                                                            ref dbAssignmentContributionCalculation, 
                                                            commitChange, 
                                                            isValidationRequired);
        }

        public Response Modify(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                               ref IList<DbModel.Assignment> dbAssignment,
                               ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                               bool commitChange = true,
                               bool isValidationRequired = true,
                               int? assignmentIds = null)

        {
            return UpdateAssignmentContributionCalculation(assignmentContributionCalculation,
                                                           ref dbAssignment, 
                                                           ref dbAssignmentContributionCalculation, 
                                                           commitChange, 
                                                           isValidationRequired);
        }

        #endregion

        #region Delete
        public Response Delete(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                               bool commitChange = true,
                               bool isValidationRequired = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation = null;
            return this.RemoveAssignmentContributionCalculation(assignmentContributionCalculation, 
                                                                ref dbAssignment,
                                                                ref dbAssignmentContributionCalculation,
                                                                commitChange, 
                                                                isValidationRequired);
        }

        public Response Delete(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                              ref IList<DbModel.Assignment> dbAssignment,
                              ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                              bool commitChange = true,
                              bool isValidationRequired = true,
                              int? assignmentIds = null)
        {
            return this.RemoveAssignmentContributionCalculation(assignmentContributionCalculation,
                                                                ref dbAssignment, 
                                                                ref dbAssignmentContributionCalculation,
                                                                commitChange, 
                                                                isValidationRequired);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentContributionCalculation> dbassignmentContributionCalculation = null;
            IList<DbModel.Assignment> dbAssignments = null;
            return IsRecordValidForProcess(assignmentContributionCalculation, 
                                            validationType, 
                                            ref dbAssignments, 
                                            ref dbassignmentContributionCalculation);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                ValidationType validationType,
                                                ref IList<DbModel.Assignment> dbAssignments,
                                                ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation)
        {
            IList<DomainModel.AssignmentContributionCalculation> filteredAssignmentInterCompanyDiscounts = null;
            return IsRecordValidForProcess(assignmentContributionCalculation, 
                                            validationType, 
                                            ref dbAssignments, 
                                            ref dbAssignmentContributionCalculation,
                                            ref filteredAssignmentInterCompanyDiscounts);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                ValidationType validationType,
                                                IList<DbModel.Assignment> dbAssignments,
                                                IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation)
        {
            return IsRecordValidForProcess(assignmentContributionCalculation, 
                                            validationType, 
                                            ref dbAssignments, 
                                            ref dbAssignmentContributionCalculation);
        }

        public bool IsValidAssignmentContributionCalculation(IList<int> assignmentContributionRevenueCostIds,
                                                             ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations,
                                                             ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbAssigContributionCalculations = _assignmentContributionCalculationRepository.FindBy(x => assignmentContributionRevenueCostIds.Contains(x.Id)).ToList();
            var invalidContributionCalculationIds = assignmentContributionRevenueCostIds.Where(x => !dbAssigContributionCalculations.Any(x1 => x1.Id == x));
            invalidContributionCalculationIds.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentContributionRevenueCostInvalidId, x);
            });
            dbAssignmentContributionCalculations = dbAssigContributionCalculations;
            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        #endregion     

        #region PrivateMethods
        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                 ValidationType validationType,
                                                 ref IList<DbModel.Assignment> dbAssignments,
                                                 ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                 ref IList<DomainModel.AssignmentContributionCalculation> filteredAssignmentContributionCalculation
                                                 )
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentContributionCalculation != null && assignmentContributionCalculation.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentContributionCalculation == null || filteredAssignmentContributionCalculation.Count <= 0)
                        filteredAssignmentContributionCalculation = FilterRecord(assignmentContributionCalculation, validationType);

                    if (filteredAssignmentContributionCalculation?.Count > 0 && IsValidPayload(filteredAssignmentContributionCalculation, validationType, ref validationMessages))
                    {
                        IList<int> assignmentContributionCalculationNotExists = null;
                        IList<int> assignmentContributionCalculationIds = filteredAssignmentContributionCalculation.Where(x => x.AssignmentContCalculationId > 0).Select(x => (int)x.AssignmentContCalculationId).Distinct().ToList();
                        IList<int> assignmentIds = filteredAssignmentContributionCalculation.Where(x => x.AssignmentId != null).Distinct().Select(x => (int)x.AssignmentId).ToList();

                        if (dbAssignmentContributionCalculation == null || dbAssignmentContributionCalculation.Count <= 0)
                            dbAssignmentContributionCalculation = GetAssignmentContributionCalculation(assignmentContributionCalculationIds);

                        if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages))
                        {
                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                if(validationType == ValidationType.Delete)
                                {
                                    if (IsAssignmentContributionCalculationExistInDb(assignmentContributionCalculationIds, dbAssignmentContributionCalculation, ref assignmentContributionCalculationNotExists, ref validationMessages))
                                        if (IsChildRecordExistsInDb(filteredAssignmentContributionCalculation, dbAssignmentContributionCalculation, ref validationMessages))
                                            result = true;

                                }

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAssignmentContributionCalculation,
                                                                    dbAssignments,
                                                                    dbAssignmentContributionCalculation,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                            {
                                result = true;

                            }
                        }
                        else
                            result = false;
                    }
                    
                }

            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.AssignmentContributionCalculation> filteredAssignmentContributionCalculation,
                                            IList<DbModel.Assignment> dbAssignments,
                                            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            bool result = false;

            IList<int> recordIdNotExists = null;
            var contributionCalculationIds = filteredAssignmentContributionCalculation?.Where(x => x.AssignmentContCalculationId > 0).Distinct().Select(x => (int)x.AssignmentContCalculationId).ToList();

            if (IsAssignmentContributionCalculationExistInDb(contributionCalculationIds,dbAssignmentContributionCalculation,ref recordIdNotExists,ref validationMessages))
                if (IsChildRecordExistsInDb(filteredAssignmentContributionCalculation, dbAssignmentContributionCalculation, ref validationMessages))
                    if(IsRecordUpdateCountMatching(filteredAssignmentContributionCalculation,dbAssignmentContributionCalculation,ref validationMessages))
                        if (IsChildRecordUpdateCountMatching(filteredAssignmentContributionCalculation, dbAssignmentContributionCalculation, ref validationMessages))
                            result = true;
            return result;

        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentContributionCalculation> filteredAssignmentContributionCalculation,
                                                 IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = filteredAssignmentContributionCalculation.Where(x => !dbAssignmentContributionCalculation.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentContCalculationId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentContCalculationId, MessageType.AssignmentContributionCalculationUpdateCountMismatch, x.AssignmentContCalculationId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsAssignmentContributionCalculationExistInDb(IList<int> assignmentContributionCalculationIds,
                                                                  IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                                  ref IList<int> assignmentContributionCalculationNotExists,
                                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentContributionCalculation == null)
                dbAssignmentContributionCalculation = new List<DbModel.AssignmentContributionCalculation>();

            var validMessages = validationMessages;

            if (assignmentContributionCalculationIds?.Count > 0)
            {
                assignmentContributionCalculationNotExists = assignmentContributionCalculationIds.Where(x => !dbAssignmentContributionCalculation.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentContributionCalculationNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentContributionCalculationNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private IList<DbModel.AssignmentContributionCalculation> GetAssignmentContributionCalculation(IList<int> contributionCaluationIds)
        {
            IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation = null;
            if (contributionCaluationIds?.Count > 0)
            {

                dbAssignmentContributionCalculation = _assignmentContributionCalculationRepository.FindBy(x => contributionCaluationIds.Contains(x.Id)).ToList();
            }

            return dbAssignmentContributionCalculation;
        }

        private IList<DomainModel.AssignmentContributionCalculation> FilterRecord(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                                                  ValidationType validationType)
        {
            IList<DomainModel.AssignmentContributionCalculation> filteredAssignmentContributionCalculation = null;

            if (validationType == ValidationType.Add)
                filteredAssignmentContributionCalculation = assignmentContributionCalculation?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Update)
                filteredAssignmentContributionCalculation = assignmentContributionCalculation?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredAssignmentContributionCalculation = assignmentContributionCalculation?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredAssignmentContributionCalculation;
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                  ValidationType validationType,
                                  ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmentContributionCalculation), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private Response AddAssignmentContributionCalculation(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                              ref IList<DbModel.Assignment> dbAssignment,
                                                              ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                              bool commitChange,
                                                              bool isValidationRequired)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignments = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentContributionCalculation, ValidationType.Add);

                if (recordToBeAdd?.Count > 0)
                {
                    if (isValidationRequired)
                        valdResponse = IsRecordValidForProcess(assignmentContributionCalculation,
                                                                ValidationType.Add,
                                                                ref dbAssignments,
                                                                ref dbAssignmentContributionCalculation,
                                                                ref recordToBeAdd);

                    //else if (dbAssignmentContributionCalculation?.Count <= 0)
                    //{
                    //    var contributionCalculationsIds = recordToBeAdd.Where(x => x.AssignmentContCalculationId > 0).Distinct().Select(x => (int)x.AssignmentContCalculationId).ToList();
                    //    dbAssignmentContributionCalculation = GetAssignmentContributionCalculation(contributionCalculationsIds);

                    //}

                    if (!isValidationRequired || Convert.ToBoolean(valdResponse.Result) && recordToBeAdd?.Count > 0)
                    {
                        _assignmentContributionCalculationRepository.AutoSave = false;
                        var dbRecordToAdd = _mapper.Map<IList<DbModel.AssignmentContributionCalculation>>(recordToBeAdd, opt =>
                         {
                             opt.Items["isAssignId"] = false;
                             opt.Items["isContributionRevenueCostId"] = false;
                             opt.Items["isContributionCalculationId"] = false;
                         });

                        _assignmentContributionCalculationRepository.Add(dbRecordToAdd);
                        if (commitChange)
                            _assignmentContributionCalculationRepository.ForceSave();
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
           // finally { _assignmentContributionCalculationRepository.Dispose(); }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentContributionCalculation(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                                 ref IList<DbModel.Assignment> dbAssignment,
                                                                 ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                                 bool commitChange,
                                                                 bool isValidationRequired)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentContributionCalculation> result = null;
            IList<DbModel.Assignment> dbAssignments = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentContributionCalculation, ValidationType.Update);

                if (recordToBeModify?.Count > 0)
                {
                    if (isValidationRequired)
                        valdResponse = IsRecordValidForProcess(assignmentContributionCalculation,
                                                                ValidationType.Update,
                                                                ref dbAssignments,
                                                                ref dbAssignmentContributionCalculation,
                                                                ref recordToBeModify);

                    else if (dbAssignmentContributionCalculation?.Count <= 0)
                    {
                        var contributionCalculationIds = recordToBeModify.Where(x => x.AssignmentContCalculationId > 0).Distinct().Select(x => (int)x.AssignmentContCalculationId).ToList();
                        dbAssignmentContributionCalculation = GetAssignmentContributionCalculation(contributionCalculationIds);
                    }

                    if (!isValidationRequired || (Convert.ToBoolean(valdResponse.Result) && dbAssignmentContributionCalculation?.Count > 0 && recordToBeModify?.Count > 0))
                    {
                        var dbContributionCalculation = dbAssignmentContributionCalculation;

                        dbAssignmentContributionCalculation.ToList().ForEach(dbAssignmentContribution =>
                        {
                            var assignmentContributionCalculationToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentContCalculationId == dbAssignmentContribution.Id);
                            if (assignmentContributionCalculationToBeModify != null)
                            {
                                dbAssignmentContribution.TotalContributionValue = (int)assignmentContributionCalculationToBeModify.TotalContributionValue;
                                dbAssignmentContribution.ContractHolderPercentage = (int)assignmentContributionCalculationToBeModify.ContractHolderPercentage;
                                dbAssignmentContribution.OperatingCompanyPercentage = assignmentContributionCalculationToBeModify.OperatingCompanyPercentage;
                                dbAssignmentContribution.CountryCompanyPercentage = assignmentContributionCalculationToBeModify.CountryCompanyPercentage;
                                dbAssignmentContribution.ContractHolderValue = (int)assignmentContributionCalculationToBeModify.ContractHolderValue;
                                dbAssignmentContribution.OperatingCompanyValue = assignmentContributionCalculationToBeModify.OperatingCompanyValue;
                                dbAssignmentContribution.CountryCompanyValue = assignmentContributionCalculationToBeModify.CountryCompanyValue;
                                dbAssignmentContribution.MarkupPercentage = assignmentContributionCalculationToBeModify.MarkupPercentage;
                                dbAssignmentContribution.LastModification = DateTime.UtcNow;
                                dbAssignmentContribution.UpdateCount = assignmentContributionCalculationToBeModify.UpdateCount.CalculateUpdateCount();
                                dbAssignmentContribution.ModifiedBy = assignmentContributionCalculationToBeModify.ModifiedBy;
                                assignmentContributionCalculationToBeModify.AssignmentContributionRevenueCosts?.ToList().ForEach(x =>
                                {
                                    ProcessContributionRevenueCost((int)assignmentContributionCalculationToBeModify.AssignmentContCalculationId,
                                                                   x,
                                                                   dbContributionCalculation,
                                                                   dbAssignmentContribution.AssignmentContributionRevenueCost?.ToList());
                                });
                            }
                        });
                        _assignmentContributionCalculationRepository.AutoSave = false;
                        _assignmentContributionCalculationRepository.Update(dbAssignmentContributionCalculation);
                        if (commitChange)
                        {
                            _revenueCostRepository.ForceSave();
                            _assignmentContributionCalculationRepository.ForceSave();

                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
            finally
            {
                _assignmentContributionCalculationRepository.AutoSave = true;
               // _assignmentContributionCalculationRepository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentContributionCalculation(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation,
                                                                 ref IList<DbModel.Assignment> dbAssignment,
                                                                 ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                                 bool commitChange,
                                                                 bool isValidationRequired)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(assignmentContributionCalculation, ValidationType.Delete);

                if (recordToBeDelete?.Count > 0)
                {
                    if (isValidationRequired)
                        response = IsRecordValidForProcess(assignmentContributionCalculation,
                                                           ValidationType.Delete,
                                                           ref dbAssignment,
                                                           ref dbAssignmentContributionCalculation,
                                                           ref recordToBeDelete);

                    if (!isValidationRequired || (Convert.ToBoolean(response.Result) && dbAssignmentContributionCalculation?.Count > 0 && recordToBeDelete?.Count > 0))
                    {
                        var dbRevenueCostToDelete = dbAssignmentContributionCalculation.SelectMany(x => x.AssignmentContributionRevenueCost).ToList();

                        _assignmentContributionCalculationRepository.AutoSave = false;
                        _revenueCostRepository.Delete(dbRevenueCostToDelete);

                        if (commitChange)
                        {
                            _revenueCostRepository.ForceSave();
                            _assignmentContributionCalculationRepository.Delete(dbAssignmentContributionCalculation);
                            _assignmentContributionCalculationRepository.ForceSave();

                        }
                    }
                    else
                        return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
            finally
            {
                _assignmentContributionCalculationRepository.AutoSave = true;
               // _assignmentContributionCalculationRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        #endregion

        private void ProcessContributionRevenueCost(int contributionCalculationId,
                                                    DomainModel.AssignmentContributionRevenueCost assignmentContributionRevenueCost,
                                                    IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations,
                                                    IList<DbModel.AssignmentContributionRevenueCost> dbassignmentContributionRevenueCosts)
        {
            _revenueCostRepository.AutoSave = false;
            if (assignmentContributionRevenueCost.RecordStatus.IsRecordStatusNew())
            {
                IList<DbModel.AssignmentContributionRevenueCost> dbRecordToAdd = new List<DbModel.AssignmentContributionRevenueCost>();
                var recordToAdd = _mapper.Map<DbModel.AssignmentContributionRevenueCost>(assignmentContributionRevenueCost);
                recordToAdd.Id = 0;
                recordToAdd.AssignmentContributionCalculationId = contributionCalculationId;
                dbRecordToAdd.Add(recordToAdd);
                _revenueCostRepository.Add(dbRecordToAdd);
            }
            if (assignmentContributionRevenueCost.RecordStatus.IsRecordStatusModified())
            {
                IList<DbModel.AssignmentContributionRevenueCost> dbRecordToUpdate = new List<DbModel.AssignmentContributionRevenueCost>();
                var recordToUpdate = dbassignmentContributionRevenueCosts.Where(x => x.Id == assignmentContributionRevenueCost.AssignmentContributionRevenueCostId)?.ToList();
                recordToUpdate.ForEach(Cost => {

                    _mapper.Map(assignmentContributionRevenueCost, Cost);
                    Cost.LastModification = DateTime.UtcNow;
                    Cost.UpdateCount = assignmentContributionRevenueCost.UpdateCount.CalculateUpdateCount();
                    Cost.ModifiedBy = assignmentContributionRevenueCost.ModifiedBy;
                });

                //recordToUpdate.Value = assignmentContributionRevenueCost.Value;
                //recordToUpdate.Description = assignmentContributionRevenueCost.Description;
                //recordToUpdate.LastModification = DateTime.UtcNow;
                //recordToUpdate.UpdateCount = recordToUpdate.UpdateCount.CalculateUpdateCount();
                //recordToUpdate.ModifiedBy = recordToUpdate.ModifiedBy;
               // dbRecordToUpdate.Add(recordToUpdate);
                _revenueCostRepository.Update(recordToUpdate);
                _revenueCostRepository.ForceSave();
            }
            if (assignmentContributionRevenueCost.RecordStatus.IsRecordStatusDeleted())
            {
                IList<DbModel.AssignmentContributionRevenueCost> dbRecordToDelete = new List<DbModel.AssignmentContributionRevenueCost>();
                var recordToDelete = dbassignmentContributionRevenueCosts.Where(x => x.Id == assignmentContributionRevenueCost.AssignmentContributionRevenueCostId)?.FirstOrDefault();
                dbRecordToDelete.Add(recordToDelete);
                _revenueCostRepository.Delete(dbRecordToDelete);
            }

        }

        private bool IsChildRecordExistsInDb(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations,
                                             IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations,
                                             ref IList<ValidationMessage> validationMessages)
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                var messages = validationMessages;

                var dbAssignmentContributionRevenueCosts = dbAssignmentContributionCalculations?.ToList().SelectMany(x => x.AssignmentContributionRevenueCost).ToList();
                var asssignmentContributionRevenueCosts = assignmentContributionCalculations.SelectMany(x => x.AssignmentContributionRevenueCosts).ToList();
                IList<int> revenueCostIds = asssignmentContributionRevenueCosts.Where(x => !x.RecordStatus.IsRecordStatusNew()
                                                                                  && x.AssignmentContributionRevenueCostId != null)
                                                                                .Select(x => (int)x.AssignmentContributionRevenueCostId).ToList();

                var recordNotExists = revenueCostIds.Where(x => !dbAssignmentContributionRevenueCosts.ToList().Any(x1 => x1.Id == x)).ToList();

                recordNotExists?.ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentContributionRevenueCostNotExists, x);
                });

                validationMessages = messages;

                return validationMessages?.Count <= 0;

            }

       
        public bool IsChildRecordUpdateCountMatching(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations,
                                                     IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations,
                                                     ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var messages = validationMessages;

            var dbAssignmentContributionRevenueCosts = dbAssignmentContributionCalculations.SelectMany(x => x.AssignmentContributionRevenueCost).ToList();
            var asssignmentContributionRevenueCosts = assignmentContributionCalculations.SelectMany(x => x.AssignmentContributionRevenueCosts).ToList();
            var revenueCosts = asssignmentContributionRevenueCosts.Where(x => x.RecordStatus.IsRecordStatusModified()
                                                                              && x.AssignmentContributionRevenueCostId != null).ToList();

            var updateCountNotMatching = revenueCosts.Where(x => !dbAssignmentContributionRevenueCosts.Any(x1 => x1.Id == x.AssignmentContributionRevenueCostId
                                                                                                    && x1.UpdateCount == x.UpdateCount)).ToList();

            updateCountNotMatching?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentContributionRevenueCostId, MessageType.AssignmentContributionRevenueCostUpdateCountMisMatch, x.AssignmentContributionRevenueCostId);
            });

            validationMessages = messages;

            return validationMessages?.Count <= 0;
        }
    }
}
