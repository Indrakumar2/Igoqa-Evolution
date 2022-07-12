using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
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
    public class AssignmentAdditionalExpenseService : IAssignmentAdditionalExpenseService
    {
        private readonly IAppLogger<AssignmentAdditionalExpenseService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentAdditionalExpenseRepository _repository = null;
        private readonly IAssignmentAdditionalExpenseValidationService _validationService = null;
        private readonly ICompanyService _companyService = null;
        private readonly IExpenseType _expenseTypeService = null;
        private readonly IAssignmentService _assignmentService = null;

        #region Constructor
        public AssignmentAdditionalExpenseService(IAppLogger<AssignmentAdditionalExpenseService> logger,
                                IMapper mapper,
                                JObject messgaes,
                                IAssignmentAdditionalExpenseRepository repository,
                                IAssignmentAdditionalExpenseValidationService validationService,
                                ICompanyService companyService,
                                IExpenseType expenseTypeService,
                                IAssignmentService assignmentService)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messageDescriptions = messgaes;
            this._companyService = companyService;
            this._expenseTypeService = expenseTypeService;
            this._assignmentService = assignmentService;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(AssignmentAdditionalExpense searchModel)
        {
            IList<AssignmentAdditionalExpense> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
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
        #endregion

        #region Add
        public Response Add(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            int? assignmentIds = null)
        {
            if (assignmentIds.HasValue)
                assignmentAdditionalExpenses?.ToList().ForEach(x => { x.AssignmentId = assignmentIds.Value; });

            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses = null;
            return AddAssignmentAdditionalExpense(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, ref dbAssignment, ref dbCompanies, ref dbExpenseType, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                            ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                            ref IList<DbModel.Company> dbCompanies,
                            ref IList<DbModel.Data> dbExpenseType,
                            ref IList<DbModel.Assignment> dbAssignment,
                            bool commitChange = true,
                            bool isDbValidationRequire = true,
                            int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequire)
            {
                assignmentAdditionalExpenses?.ToList().ForEach(x => { x.AssignmentId = assignmentIds.Value; });
            }
            return AddAssignmentAdditionalExpense(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, ref dbAssignment, ref dbCompanies, ref dbExpenseType, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Modify
        public Response Modify(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                               bool commitChange = true,
                               bool isDbValidationRequire = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Data> dbExpenseTypes = null;
            IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses = null;
            if (assignmentIds.HasValue)
                assignmentAdditionalExpenses?.ToList().ForEach(x => { x.AssignmentId = assignmentIds.Value; });

            return UpdateAssignmentAdditionalExpenses(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, ref dbAssignment, ref dbCompanies, ref dbExpenseTypes, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                 ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                ref IList<DbModel.Company> dbCompanies,
                                 ref IList<DbModel.Data> dbExpenseTypes,
                                ref IList<DbModel.Assignment> dbAssignment,
                                bool commitChange = true,
                                bool isDbValidationRequire = true,
                                int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequire)
                assignmentAdditionalExpenses?.ToList().ForEach(x => { x.AssignmentId = assignmentIds.Value; });
            return UpdateAssignmentAdditionalExpenses(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, ref dbAssignment, ref dbCompanies, ref dbExpenseTypes, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete
        public Response Delete(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                               bool commitChange = true,
                               bool isDbValidationRequire = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Data> dbExpenseTypes = null;
            IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenseType = null;
            if (assignmentIds.HasValue)
                assignmentAdditionalExpenses?.ToList().ForEach(x => { x.AssignmentId = assignmentIds.Value; });
            return this.RemoveAssignmentAdditionalExpense(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenseType, ref dbAssignment, ref dbCompanies, ref dbExpenseTypes, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                              ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentExpenseType,
                              ref IList<DbModel.Company> dbCompanies,
                              ref IList<DbModel.Data> dbExpenseTypes,
                              ref IList<DbModel.Assignment> dbAssignment,
                              bool commitChange = true,
                              bool isDbValidationRequire = true,
                              int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequire)
                assignmentAdditionalExpenses?.ToList().ForEach(x => { x.AssignmentId = assignmentIds.Value; });
            return this.RemoveAssignmentAdditionalExpense(assignmentAdditionalExpenses, ref dbAssignmentExpenseType, ref dbAssignment, ref dbCompanies, ref dbExpenseTypes, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses = null;
            IList<DbModel.Assignment> dbAssignments = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Data> dbExpenseTypes = null;

            return IsRecordValidForProcess(assignmentAdditionalExpenses, validationType, ref dbAssignmentAdditionalExpenses, ref dbCompanies, ref dbExpenseTypes, ref dbAssignments);
        }

        public Response IsRecordValidForProcess(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                ValidationType validationType,
                                                ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                                ref IList<DbModel.Company> dbCompanies,
                                                ref IList<DbModel.Data> dbExpenseTypes,
                                                ref IList<DbModel.Assignment> dbAssignments

                                               )
        {
            IList<DomainModel.AssignmentAdditionalExpense> filteredAssignmentAdditionalExpenses = null;

            return IsRecordValidForProcess(assignmentAdditionalExpenses,
                validationType,
                ref dbAssignments,
                  ref dbCompanies,
                   ref dbExpenseTypes,
                    ref filteredAssignmentAdditionalExpenses,
                 ref dbAssignmentAdditionalExpenses
                 );
        }

        public Response IsRecordValidForProcess(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                ValidationType validationType,
                                                 IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                                 IList<DbModel.Company> dbCompanies,
                                                 IList<DbModel.Data> dbExpenseTypes,
                                                IList<DbModel.Assignment> dbAssignments
                                               )
        {
            return IsRecordValidForProcess(assignmentAdditionalExpenses, validationType, ref dbAssignmentAdditionalExpenses, ref dbCompanies, ref dbExpenseTypes, ref dbAssignments);
        }
        #endregion

        #endregion

        #region Private Metods

        private Response AddAssignmentAdditionalExpense(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                        ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                                        ref IList<DbModel.Assignment> dbAssignment,
                                                        ref IList<DbModel.Company> dbCompanies,
                                                        ref IList<DbModel.Data> dbExpenseTypes,
                                                        bool commitChange,
                                                        bool isDbValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Company> dbCompany = null;
            IList<DbModel.Data> dbExpenseType = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentAdditionalExpenses, ValidationType.Add);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentAdditionalExpenses, ValidationType.Add, ref dbAssignment, ref dbCompanies, ref dbExpenseTypes, ref recordToBeAdd, ref dbAssignmentAdditionalExpenses);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    if (recordToBeAdd?.Count > 0)
                    {
                        if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                        {
                            _repository.AutoSave = false;
                            dbExpenseType = dbExpenseTypes;
                            dbCompany = dbCompanies;
                            dbAssignmentAdditionalExpenses = _mapper.Map<IList<DbModel.AssignmentAdditionalExpense>>(recordToBeAdd, opt =>
                            {
                                opt.Items["isAssignId"] = false;
                                opt.Items["ExpenseTypes"] = dbExpenseType;
                                opt.Items["CompanyCodes"] = dbCompany;

                            });

                            _repository.Add(dbAssignmentAdditionalExpenses);
                            if (commitChange)
                                _repository.ForceSave();
                        }
                        else
                            return valdResponse;
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
           // finally { _repository.Dispose(); }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentAdditionalExpenses(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                            ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                                            ref IList<DbModel.Assignment> dbAssignments,
                                                            ref IList<DbModel.Company> dbCompanies,
                                                            ref IList<DbModel.Data> dbExpenseTypes,
                                                            bool commitChange = true,
                                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentAdditionalExpense> result = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Company> dbCompany = null;
            IList<DbModel.Data> dbExpenseType = null;

            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentAdditionalExpenses, ValidationType.Update);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentAdditionalExpenses, ValidationType.Update, ref dbAssignments, ref dbCompanies, ref dbExpenseTypes, ref recordToBeModify, ref dbAssignmentAdditionalExpenses);
                else if (dbAssignmentAdditionalExpenses?.Count <= 0 && recordToBeModify?.Count > 0)
                    dbAssignmentAdditionalExpenses = GetAssignmentAdditionalExpenses(recordToBeModify);

                if (recordToBeModify?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbAssignmentAdditionalExpenses?.Count > 0))
                    {

                        IList<string> companyCodes = assignmentAdditionalExpenses.Select(x => x.CompanyCode).ToList();
                        IList<string> expenseTypes = assignmentAdditionalExpenses.Select(x => x.ExpenseType).ToList();
                        dbAssignment = dbAssignments;
                        dbCompany = dbCompanies;
                        dbExpenseType = dbExpenseTypes;

                        dbAssignmentAdditionalExpenses.ToList().ForEach(dbExpense =>
                        {
                            var expenseToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentAdditionalExpenseId == dbExpense.Id);
                            if (expenseToBeModify != null)
                            {
                                _mapper.Map(expenseToBeModify, dbExpense, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["ExpenseTypes"] = dbExpenseType;
                                    opt.Items["CompanyCodes"] = dbCompany;

                                });
                                dbExpense.LastModification = DateTime.UtcNow;
                                dbExpense.UpdateCount = expenseToBeModify.UpdateCount.CalculateUpdateCount();
                                dbExpense.ModifiedBy = expenseToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbAssignmentAdditionalExpenses);
                        if (commitChange)
                            _repository.ForceSave();
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
            finally
            {
                _repository.AutoSave = true;
               // _repository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentAdditionalExpense(IList<AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                           ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                                            ref IList<DbModel.Assignment> dbAssignments,
                                                            ref IList<DbModel.Company> dbCompanies,
                                                            ref IList<DbModel.Data> dbExpenseTypes,
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

                var recordToBeDelete = FilterRecord(assignmentAdditionalExpenses, ValidationType.Delete);

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentAdditionalExpenses, ValidationType.Delete, ref dbAssignments, ref dbCompanies, ref dbExpenseTypes, ref recordToBeDelete, ref dbAssignmentAdditionalExpenses);
                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(response.Result) && dbAssignmentAdditionalExpenses?.Count > 0))
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbAssignmentAdditionalExpenses);
                        if (commitChange)
                            _repository.ForceSave();
                    }
                    else
                        return response;
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
            finally
            {
                _repository.AutoSave = true;
               // _repository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                ValidationType validationType,
                                                ref IList<DbModel.Assignment> dbAssignments,
                                                ref IList<DbModel.Company> dbCompanies,
                                                ref IList<DbModel.Data> dbExpenseTypes,
                                                ref IList<DomainModel.AssignmentAdditionalExpense> filteredAssignmentAdditionalExpenses,
                                                ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentAdditionalExpenses != null && assignmentAdditionalExpenses.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentAdditionalExpenses == null || filteredAssignmentAdditionalExpenses.Count <= 0)
                        filteredAssignmentAdditionalExpenses = FilterRecord(assignmentAdditionalExpenses, validationType);

                    IList<int> assignmentIds = filteredAssignmentAdditionalExpenses.Where(x => x.AssignmentId > 0).Distinct().Select(x => (int)x.AssignmentId).ToList();
                    IList<string> companyCodes = filteredAssignmentAdditionalExpenses.Where(x => !string.IsNullOrEmpty(x.CompanyCode)).Distinct().Select(x => x.CompanyCode).ToList();

                    if (filteredAssignmentAdditionalExpenses?.Count > 0
                        && IsValidPayload(filteredAssignmentAdditionalExpenses, validationType, ref validationMessages))

                    {
                        IList<int?> expenseNotExists = null;
                        var assignmentAdditionalExpenseIds = filteredAssignmentAdditionalExpenses.Select(x => x.AssignmentAdditionalExpenseId).Distinct().ToList();
                        if ((dbAssignmentAdditionalExpenses == null || dbAssignmentAdditionalExpenses.Count <= 0) && validationType != ValidationType.Add)
                            dbAssignmentAdditionalExpenses = GetAssignmentAdditionalExpenses(filteredAssignmentAdditionalExpenses);

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsAssignmentAdditionalExpenseExistInDb(assignmentAdditionalExpenseIds, dbAssignmentAdditionalExpenses, ref expenseNotExists, ref validationMessages);
                            if (result && validationType == ValidationType.Update)
                            {
                                if (IsRecordValidaForUpdate(filteredAssignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, ref dbAssignments, ref dbCompanies, ref dbExpenseTypes, validationType, ref validationMessages))
                                    result = true;
                                else
                                    result = false;
                            }

                        }
                        else if (validationType == ValidationType.Add)
                        {
                            if (IsRecordValidForAdd(filteredAssignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, ref dbAssignments, ref dbCompanies, ref dbExpenseTypes, validationType, ref validationMessages))
                                result = true;
                            else
                                result = false;
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsAssignmentAdditionalExpenseExistInDb(IList<int?> assignmentAdditionalExpenseId,
                                        IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                        ref IList<int?> additionalExpenseNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentAdditionalExpenses == null)
                dbAssignmentAdditionalExpenses = new List<DbModel.AssignmentAdditionalExpense>();

            var validMessages = validationMessages;

            if (assignmentAdditionalExpenseId?.Count > 0)
            {
                additionalExpenseNotExists = assignmentAdditionalExpenseId.Where(x => !dbAssignmentAdditionalExpenses.Select(x1 => x1.Id).ToList().Contains((int)x)).ToList();
                additionalExpenseNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentAdditionalExpenseNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                   ValidationType validationType,
                                   ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmentAdditionalExpenses), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<DomainModel.AssignmentAdditionalExpense> FilterRecord(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                                            ValidationType filterType)
        {
            IList<DomainModel.AssignmentAdditionalExpense> filteredAssignmentAdditionalExpenses = null;

            if (filterType == ValidationType.Add)
                filteredAssignmentAdditionalExpenses = assignmentAdditionalExpenses?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredAssignmentAdditionalExpenses = assignmentAdditionalExpenses?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredAssignmentAdditionalExpenses = assignmentAdditionalExpenses?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredAssignmentAdditionalExpenses;

        }

        private IList<DbModel.AssignmentAdditionalExpense> GetAssignmentAdditionalExpenses(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses)
        {
            IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses = null;
            if (assignmentAdditionalExpenses?.Count > 0)
            {
                var existingRecords = assignmentAdditionalExpenses.Select(x => x.AssignmentAdditionalExpenseId).Distinct().ToList();
                dbAssignmentAdditionalExpenses = _repository.FindBy(x => existingRecords.Contains(x.Id)).ToList();
            }

            return dbAssignmentAdditionalExpenses;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                        ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                        ref IList<DbModel.Assignment> dbAssignments,
                                        ref IList<DbModel.Company> dbCompanies,
                                        ref IList<DbModel.Data> dbExpenseTypes,
                                        ValidationType validationType,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<string> companyCodes = assignmentAdditionalExpenses.Select(x => x.CompanyCode).ToList();
            IList<string> expenseTypes = assignmentAdditionalExpenses.Select(x => x.ExpenseType).ToList();
            var assignmentIds = assignmentAdditionalExpenses.Where(x => x.AssignmentId > 0).Select(u => (int)u.AssignmentId).Distinct().ToList();
            if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages)
            && _companyService.IsValidCompany(companyCodes, ref dbCompanies, ref messages)
            && _expenseTypeService.IsValidExpenseType(expenseTypes, ref dbExpenseTypes, ref validationMessages))
                IsUniqueAdditionalExpenseType(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, validationType, ref validationMessages);



            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                            ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                            ref IList<DbModel.Assignment> dbAssignment,
                                            ref IList<DbModel.Company> dbCompanies,
                                            ref IList<DbModel.Data> dbExpenseTypes,
                                            ValidationType validationType,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (messages?.Count <= 0)
                IsRecordUpdateCountMatching(assignmentAdditionalExpenses, dbAssignmentAdditionalExpenses, ref messages);

            if (messages?.Count <= 0)
            {
                var assignmentIds = assignmentAdditionalExpenses.Where(x => x.AssignmentId > 0).Select(u => (int)u.AssignmentId).Distinct().ToList();
                IList<string> companyCodes = assignmentAdditionalExpenses.Select(x => x.CompanyCode).ToList();
                IList<string> expenseTypes = assignmentAdditionalExpenses.Select(x => x.ExpenseType).ToList();
                if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignment, ref validationMessages)
                && _companyService.IsValidCompany(companyCodes, ref dbCompanies, ref validationMessages)
                && _expenseTypeService.IsValidExpenseType(expenseTypes, ref dbExpenseTypes, ref validationMessages))
                    IsUniqueAdditionalExpenseType(assignmentAdditionalExpenses, ref dbAssignmentAdditionalExpenses, validationType, ref validationMessages);

            }


            if (messages?.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses,
                                                IList<DbRepository.Models.SqlDatabaseContext.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentAdditionalExpenses.Where(x => !dbAssignmentAdditionalExpenses.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentAdditionalExpenseId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentAdditionalExpenseUpdateCountMismatch, x.AssignmentAdditionalExpenseId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsUniqueAdditionalExpenseType(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpense,
                                                    ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpense,
                                                    ValidationType validationType,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentAdditionalExpense> assignmentExpenseExists = null;
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            assignmentExpenseExists = _repository.IsUniqueAssignmentExpense(assignmentAdditionalExpense,
                                                                            dbAssignmentAdditionalExpense,
                                                                            validationType);

            assignmentExpenseExists?.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.ExpenseType.Name, MessageType.AssignmentAdditionalExpenseDuplicateRecord, x.ExpenseType.Name);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        public void Dispose()
        {
            
        }
        #endregion
    }
}