using AutoMapper;
using Evolution.Assignment.Domain.Enums;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Home.Domain.Models.Homes;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Evolution.NumberSequence.InfraStructure.Interface;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using IUR = Evolution.Admin.Domain.Interfaces.Data;


namespace Evolution.Assignment.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<AssignmentService> _logger = null;
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly IAssignmentSubSupplerRepository _assignmentSubSupplerRepository = null;
        private readonly IMasterRepository _masterRepository = null;
        private readonly IProjectRepository _projectRepository = null;
        private readonly IContractRepository _contractRepository = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IAssignmentValidationService _assignmentValidationService = null;
        private readonly ICountryService _countryService = null;
        private readonly ICompanyService _companyService = null;
        private readonly IProjectService _projectService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly INumberSequenceRepository _numberSequenceRepository = null;
        private readonly IModuleRepository _moduleRepository = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public AssignmentService(IMapper mapper, IAppLogger<AssignmentService> logger, IAssignmentRepository repository, 
                                IContractExchangeRateService contractExchangeRateService, ICurrencyExchangeRateService currencyExchangeRateService,
                                IContractRepository contractRepository, IProjectRepository projectRepository,
                                JObject messages, IAssignmentValidationService assignmentValidationService, IProjectService projectService,
                                ICompanyService companyService, IMasterRepository masterRepository,
                                ICountryService countryService, IMyTaskService myTaskService,
                                IMongoDocumentService mongoDocumentService, INumberSequenceRepository numberSequenceRepository,
                                IModuleRepository moduleRepository, IOptions<AppEnvVariableBaseModel> environment , IAssignmentSubSupplerRepository assignmentSubSupplerRepository)
         {
            _mapper = mapper;
            _logger = logger;
            _assignmentRepository = repository;
            _masterRepository = masterRepository;
            _assignmentValidationService = assignmentValidationService;
            _contractExchangeRateService = contractExchangeRateService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _contractRepository = contractRepository;
            _projectRepository = projectRepository;
            _projectService = projectService;
            _companyService = companyService;
            _countryService = countryService;
            _myTaskService = myTaskService;
            _messageDescriptions = messages;
            _mongoDocumentService = mongoDocumentService;
            _numberSequenceRepository = numberSequenceRepository;
            _moduleRepository = moduleRepository;
            _environment = environment.Value;
            _assignmentSubSupplerRepository = assignmentSubSupplerRepository;
        }


        #region Public Exposed Method

        #region Dashboard Get

        public async Task<Response> GetAssignment(DomainModel.AssignmentSearch searchModel, AdditionalFilter filter = null)
        {
            IList<DomainModel.AssignmentDashboard> result = null;
            Int32? count = null;
            Exception exception = null;
            IList<string> mongoSearch = null;
            try
            {
                if (filter != null && !filter.IsRecordCountOnly)
                {
                    //Mongo Doc Search
                    if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                    {
                        var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                        mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                        if (mongoSearch != null && mongoSearch.Count > 0)
                        { 
                            searchModel.AssignmentIDs = mongoSearch;
                            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                            {
                                if (!string.IsNullOrEmpty(searchModel.AssignmentStatus))
                                    result = _assignmentRepository.Search(searchModel);
                                tranScope.Complete();
                            }
                            if (result != null)
                                result = result.Where(x => mongoSearch.Contains(x.AssignmentId.ToString())).ToList();
                        }
                        else
                            result = new List<DomainModel.AssignmentDashboard>();
                    }
                    else
                    {
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                        {
                            if (!string.IsNullOrEmpty(searchModel.AssignmentStatus))
                                result = _assignmentRepository.Search(searchModel);
                            tranScope.Complete();
                        }
                    }


                    if (result?.Count > 0 && filter?.IsInvoiceDetailRequired == true)
                    {
                        var contractNumbers = result.Select(x => x.AssignmentContractNumber).Distinct().ToList();
                        var invoiceInfos = GetAssignmentInvoiceInfo(contractNumbers).Result
                            .Populate<IList<InvoicedInfo>>();

                        result.ToList().ForEach(x =>
                        {
                            var invoiceInfo = invoiceInfos?.FirstOrDefault(x1 =>
                                                                            x1.ContractNumber == x.AssignmentContractNumber &&
                                                                            x1.ProjectNumber == x.AssignmentProjectNumber &&
                                                                            x1.AssignmentNumber == x.AssignmentNumber);
                            x.AssignmentInvoicedToDate = invoiceInfo?.InvoicedToDate ?? 0;
                            x.AssignmentUninvoicedToDate = invoiceInfo?.UninvoicedToDate ?? 0;
                            x.AssignmentHoursInvoicedToDate = invoiceInfo?.HoursInvoicedToDate ?? 0;
                            x.AssignmentHoursUninvoicedToDate = invoiceInfo?.HoursUninvoicedToDate ?? 0;
                        });
                    }
                }
                else
                    if (!string.IsNullOrEmpty(searchModel.AssignmentStatus))
                    count = _assignmentRepository.GetCount(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, filter?.IsRecordCountOnly == true ? count : result?.Count);
        }
       
        /*This is used for Document Approval Dropdown binding*/
        public Response GetDocumentAssignment(DomainModel.AssignmentDashboard searchModel)
        {
            IList<DomainModel.AssignmentDashboard> result = null;
            Exception exception = null;
            try
            {
                string[] includes = new string[] { };
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _assignmentRepository.GetAssignment(searchModel)?.Select(x => new DomainModel.AssignmentDashboard
                    {
                        AssignmentFormattedNumber = string.Format("{0:D5}", x.AssignmentNumber),
                        AssignmentId = x.Id
                    })?.OrderBy(x => x.AssignmentFormattedNumber).ToList();
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

        #region Assignment Search Get
        public async Task<Response> SearchAssignment(DomainModel.AssignmentEditSearch searchModel)
        {
            IList<DomainModel.AssignmentEditSearch> result = null;
            Exception exception = null;
            try
            {
                //Mongo Doc Search
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    var mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    {
                        var assignmentIds = mongoSearch.Select(x => Convert.ToInt32(x)).Distinct().ToList();
                        searchModel.AssignmentIDs = assignmentIds;
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                        {
                            searchModel.FetchCount = _environment.AssignmentRecordSize;
                            result = _assignmentRepository.AssignmentSearch(searchModel);
                            //result = result?.Where(x => assignmentIds.Contains(x.AssignmentId)).ToList();
                            tranScope.Complete();
                        }
                    }
                    else
                        result = new List<DomainModel.AssignmentEditSearch>();
                }
                else
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        searchModel.FetchCount = _environment.AssignmentRecordSize;
                        result = _assignmentRepository.AssignmentSearch(searchModel);
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.FirstOrDefault()?.TotalCount);
        }
        #endregion

        #region Budget Get

        public Response GetAssignmentBudgetDetails(string companyCode = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true)
        {
            Exception exception = null;
            try
            {
                var contracts = _assignmentRepository.FindBy(x =>
                                         (string.IsNullOrEmpty(companyCode) || x.ContractCompany.Code == companyCode) &&
                                         (contractStatus == ContractStatus.All || x.Project.Contract.Status == contractStatus.FirstChar()) &&
                                         (x.BudgetValue > 0 || x.Project.Budget > 0 || x.Project.Contract.Budget > 0) &&
                                         ((isAssignmentOnly == false && (string.IsNullOrEmpty(userName) || x.Project.Coordinator.SamaccountName == userName)) ||
                                           (isAssignmentOnly == true && (string.IsNullOrEmpty(userName) || x.OperatingCompanyCoordinator.SamaccountName == userName) ||
                                            (string.IsNullOrEmpty(userName) || x.ContractCompanyCoordinator.SamaccountName == userName))
                                         )
                                         )
                                        .Select(x =>
                                        new
                                        {
                                            ContractId = x.Project.Contract.Id,
                                            ContractCustomerCode = x.Project.Contract.Customer.Code,
                                            ContractCustomerName = x.Project.Contract.Customer.Name,
                                            BudgetValue = x.Project.Contract.Budget,
                                            x.Project.Contract.CustomerContractNumber,
                                            x.Project.Contract.BudgetCurrency,
                                            x.BudgetWarning,
                                            x.BudgetHours,
                                            x.BudgetHoursWarning,
                                        }).Distinct().ToList();

                var contractIds = contracts?.Select(x => x.ContractId).Distinct().ToList();

                var projectInvoiceInfo = _contractRepository
                                              .GetBudgetAccountItemDetails(companyCode,
                                                                              contractIds,
                                                                              userName,
                                                                              contractStatus,
                                                                              isAssignmentOnly);

                return GetAssignmentBudgetDetails(projectInvoiceInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetAssignmentBudgetDetails(IList<BudgetAccountItem> budgetAccountItems,
                                                    IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null,
                                                    IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<Budget> result = null;
            Exception exception = null;
            try
            {
                if (budgetAccountItems?.Count > 0)
                {
                    var contractIds = budgetAccountItems.Select(x => x.ContractId).Distinct().ToList();
                    var assignments = _assignmentRepository.FindBy(x => contractIds.Contains(x.Project.ContractId))
                                        .Select(x =>
                                        new
                                        {
                                            ContractId = x.Project.Contract.Id,
                                            AssignmentId = x.Id,
                                            ContractCustomerCode = x.Project.Contract.Customer.Code,
                                            ContractCustomerName = x.Project.Contract.Customer.Name,
                                            BudgetValue = x.BudgetValue ?? 0,
                                            x.Project.Contract.CustomerContractNumber,
                                            x.Project.Contract.BudgetCurrency,
                                            x.BudgetWarning,
                                            x.BudgetHours,
                                            x.BudgetHoursWarning,
                                        }).Distinct().ToList();

                    var assignmentInvoiceInfo = PopulateAssignmentInvoiceInfo(budgetAccountItems, contractExchangeRates, currencyExchangeRates).Result
                                                                                               .Populate<List<InvoicedInfo>>();
                    result = _mapper.Map<List<InvoicedInfo>, List<Budget>>(assignmentInvoiceInfo);

                    result.ToList().ForEach(x =>
                    {
                        var assignment = assignments?.FirstOrDefault(x1 => x1.AssignmentId == x.AssignmentId);
                        x.ContractCustomerCode = assignment.ContractCustomerCode;
                        x.ContractCustomerName = assignment.ContractCustomerName;
                        x.BudgetValue = assignment.BudgetValue;
                        x.CustomerContractNumber = assignment.CustomerContractNumber;
                        x.BudgetCurrency = assignment.BudgetCurrency;
                        x.BudgetWarning = assignment.BudgetWarning;
                        x.BudgetHours = assignment.BudgetHours;
                        x.BudgetHoursWarning = assignment.BudgetHoursWarning;
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response GetAssignmentInvoiceInfo(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true)
        {
            Exception exception = null;
            try
            {
                var assignmetBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(companyCode, contractIds, userName, contractStatus, showMyAssignmentsOnly);
                return PopulateAssignmentInvoiceInfo(assignmetBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetAssignmentInvoiceInfo(List<string> contractNumber = null, List<int> projectNumber = null, List<int> assignmentNumber = null, bool IsAssignmentFetchRequired = true,List<int> autoContractIds=null)
        {
            List<int> contractIds = null;
            Exception exception = null;
            try
            {
                contractIds = new List<int>();
                if (contractNumber != null && autoContractIds == null)
                {
                    contractIds.AddRange(_contractRepository.FindBy(x => contractNumber.Contains(x.ContractNumber)).Select(x => x.Id).ToList());
                }
                if (projectNumber != null)
                {
                    contractIds.AddRange(_projectRepository.FindBy(x => x.ProjectNumber != null && projectNumber.Contains(x.ProjectNumber.Value)).Select(x => x.ContractId).Distinct().ToList());
                }
                if (assignmentNumber != null && IsAssignmentFetchRequired)
                {
                    contractIds.AddRange(_assignmentRepository.FindBy(x => assignmentNumber.Contains((int)x.AssignmentNumber)).Select(x => x.Project.Contract.Id).Distinct().ToList());
                }
                contractIds = autoContractIds == null ? contractIds.Distinct().ToList() : autoContractIds.Distinct().ToList();

                var assignmetBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(contractIds: contractIds);
                assignmetBudgetAccountItems = assignmetBudgetAccountItems.Where(x => assignmentNumber.Contains(x.AssignmentId))?.ToList();

                return PopulateAssignmentInvoiceInfo(assignmetBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNumber);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response GetAssignmentInvoiceInfo(int contractId)
        {
            Exception exception = null;
            try
            {
                var assignmentBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(contractIds: new List<int> { contractId });
                return PopulateAssignmentInvoiceInfo(assignmentBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractId);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        #endregion

        #region Assignment Edit Get

        public Response GetAssignment(DomainModel.AssignmentSearch searchModel)
        {
            var flag = 0;
            IList<DomainModel.Assignment> result = null;
            Exception exception = null;
            try
            {
                string[] includes = new string[] { };

                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    if (searchModel?.AssignmentId > 0)
                        result = _assignmentRepository.AssignmentSearch(flag, searchModel, includes);

                    if (result?.Count > 0 && !searchModel.isVisitTimesheet)  // !isVisitTimesheet is added to prevent call of these codes when it is from Visit & Timesheet
                    {
                        var resSearchIds = result.Where(x => x.ResourceSearchId.HasValue)
                            .Select(x => x.ResourceSearchId.ToString()).ToList();
                        if (resSearchIds?.Count > 0)
                        {
                            var myTasks = _myTaskService
                                .Get(new List<string> { ModuleCodeType.RSEARCH.ToString() }, resSearchIds).Result
                                .Populate<IList<MyTask>>();
                            if (myTasks?.Count > 0)
                            {
                                result = result.GroupJoin(myTasks,
                                        l => new { ResourceSearchId = l.ResourceSearchId.ToString() },
                                        r => new { ResourceSearchId = r.TaskRefCode },
                                        (asmnt, task) => new { asmnt, task })
                                    .Select(x =>
                                    {
                                        x.asmnt.ARSTaskType = x.task?.FirstOrDefault()?.TaskType;
                                        return x.asmnt;
                                    }).ToList();
                            }
                        }
                    }

                    if (result?.Count > 0 && !searchModel.isVisitTimesheet)  // !isVisitTimesheet is added to prevent call of these codes when it is from Visit & Timesheet
                    {
                        var contractNumbers = result.Select(x => x.AssignmentContractNumber).Distinct().ToList();
                        var assignmentIds = result.Select(x => (int)x.AssignmentId).Distinct().ToList();
                        var contractIds = result.Where(x => x.AssignmentContractId > 0)?.Select(x => (int)x.AssignmentContractId).Distinct().ToList();
                        var invoiceInfos = GetAssignmentInvoiceInfo(contractNumbers, null, assignmentIds, false, contractIds)?.Result.Populate<IList<InvoicedInfo>>();
                        result.ToList().ForEach(x =>
                        {
                            var invoiceInfo = invoiceInfos?.FirstOrDefault(x1 => x1.ContractNumber == x.AssignmentContractNumber && x1.ProjectNumber == x.AssignmentProjectNumber && x1.AssignmentNumber == x.AssignmentNumber);
                            x.AssignmentInvoicedToDate = invoiceInfo?.InvoicedToDate ?? 0;
                            x.AssignmentUninvoicedToDate = invoiceInfo?.UninvoicedToDate ?? 0;
                            x.AssignmentHoursInvoicedToDate = invoiceInfo?.HoursInvoicedToDate ?? 0;
                            x.AssignmentHoursUninvoicedToDate = invoiceInfo?.HoursUninvoicedToDate ?? 0;
                        });
                    }
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

        #region Post

        public Response Add(IList<DomainModel.Assignment> assignments,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            DomainModel.AssignmentDatabaseCollection assignmentDBCollection = new DomainModel.AssignmentDatabaseCollection();
            return AddAssignment(assignments,
                                ref dbAssignment,
                                ref assignmentDBCollection,
                                commitChange,
                                isValidationRequire);
        }

        public Response Add(IList<DomainModel.Assignment> assignments,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {
            return AddAssignment(assignments,
                                ref dbAssignment,
                                ref assignmentDBCollection,
                                commitChange,
                                isValidationRequire);
        }

        public Response Add(IList<DomainModel.Assignment> assignments,
                            ref DbModel.Assignment dbAssignment,
                            bool commitChange = true)
        {
            return AddAssignment(assignments,
                                ref dbAssignment,
                                commitChange);
        }

        #endregion

        #region Put

        public Response Modify(IList<DomainModel.Assignment> assignments,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            DomainModel.AssignmentDatabaseCollection assignmentDBCollection = new DomainModel.AssignmentDatabaseCollection();
            return UpdateAssignment(assignments,
                                    ref dbAssignment,
                                    ref assignmentDBCollection,
                                    commitChange,
                                    isValidationRequire);
        }

        public Response Modify(DbModel.Assignment dbAssignment, bool commitChange = true)
        {
            Exception exception = null;
            try
            {
                _assignmentRepository.Update(dbAssignment);
                if (commitChange)
                    _assignmentRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response Modify(IList<DomainModel.Assignment> assignments,
                               ref IList<DbModel.Assignment> dbAssignment,
                               ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            return UpdateAssignment(assignments,
                                    ref dbAssignment,
                                    ref assignmentDBCollection,
                                    commitChange,
                                    isValidationRequire);
        }

        public Response Modify(int assignmentId, List<KeyValuePair<string, object>> updateValueProps, bool commitChange = true, params Expression<Func<DbModel.Assignment, object>>[] updatedProperties)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                _assignmentRepository.Update(assignmentId, updateValueProps, updatedProperties);
                if (commitChange)
                    _assignmentRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        #endregion

        #region Delete

        public Response Delete(IList<DomainModel.Assignment> assignments,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            return this.RemoveModule(assignments,
                                    commitChange,
                                    isValidationRequire);
        }

        #endregion

        #region General

        public Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                                ValidationType validationType)
        {
            IList<DbModel.Assignment> dbAssignments = null;
            DomainModel.AssignmentDatabaseCollection assignmentDBCollection = new DomainModel.AssignmentDatabaseCollection();
            return IsRecordValidForProcess(assignments,
                                           validationType,
                                           ref dbAssignments,
                                           ref assignmentDBCollection);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                                ValidationType validationType,
                                                ref IList<DomainModel.Assignment> filteredAssignment,
                                                ref IList<DbModel.Assignment> dbAssignments)
        {
            DomainModel.AssignmentDatabaseCollection assignmentDBCollection = new DomainModel.AssignmentDatabaseCollection();
            return IsRecordValidForProcess(assignments,
                                            validationType,
                                            ref dbAssignments,
                                            ref assignmentDBCollection);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                                ValidationType validationType,
                                                ref IList<DbModel.Assignment> dbAssignments,
                                                ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                                bool IsAssignmentValidationRequired = true)
        {
            IList<DomainModel.Assignment> filteredAssignment = null;
            return IsRecordValidForProcess(assignments,
                                           validationType,
                                           ref filteredAssignment,
                                           ref dbAssignments,
                                           ref assignmentDBCollection,
                                           IsAssignmentValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments, IList<DbModel.Assignment> dbAssignments, ValidationType validationType)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                validationMessages = new List<ValidationMessage>();
                var filteredData = FilterRecord(assignments, validationType);
                if (filteredData != null && filteredData.Count > 0)
                {
                    IsValidPayload(filteredData, validationType, ref validationMessages);
                    if (validationMessages?.Count == 0 && validationType == ValidationType.Update)
                        IsRecordUpdateCountMatching(filteredData, dbAssignments, ref validationMessages);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                                ValidationType validationType,
                                                IList<DbModel.Assignment> dbAssignments,
                                                ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection)
        {
            return IsRecordValidForProcess(assignments,
                                           validationType,
                                           ref dbAssignments,
                                           ref assignmentDBCollection);
        }


        public bool IsValidAssignment(IList<int> assignmentId,
                                      ref IList<DbModel.Assignment> dbAssignments,
                                      ref IList<ValidationMessage> messages,
                                      params Expression<Func<DbModel.Assignment, object>>[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbAssignments == null)
            {
                var dbAssignment = _assignmentRepository?.FindBy(x => assignmentId.Contains(x.Id), includes).AsNoTracking()?.ToList();
                var assignmentNotExists = assignmentId?.Where(x => !dbAssignment.Any(x2 => x2.Id == x))?.ToList();
                assignmentNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentNotExists.ToId();
                    message.Add(_messageDescriptions, x, MessageType.AssignmentNotExists, x);
                });
                dbAssignments = dbAssignment;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        public bool IsValidAssignment(int assignmentNumber,
                                        int assignmentProjectNumber,
                              ref DbModel.Assignment dbAssignment,
                              ref IList<ValidationMessage> messages,
                             string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (assignmentNumber > 0 && assignmentProjectNumber > 0)
            {
                if (dbAssignment == null)
                {
                    if (includes == null)
                        includes = new string[] { "Project" };
                    else
                        includes = includes.Append("Project").ToArray();

                    var dbAssign = _assignmentRepository?.FindBy(x => x.AssignmentNumber == assignmentNumber && x.Project.ProjectNumber == assignmentProjectNumber, includes)?.FirstOrDefault();
                    if (dbAssign == null)
                        message.Add(_messageDescriptions, null, MessageType.AssignmentWithNumberNotExists, assignmentNumber, assignmentProjectNumber);
                    dbAssignment = dbAssign;
                    messages = message;
                }
            }
            return messages?.Count <= 0;
        }

        public bool IsValidAssignment(IList<int> assignmentId,
                                     ref IList<DbModel.Assignment> dbAssignments,
                                     ref IList<ValidationMessage> messages,
                                     string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbAssignments == null || dbAssignments?.Count == 0)
            {
                var dbAssignment = _assignmentRepository?.FindBy(x => assignmentId.Contains(x.Id), includes)?.ToList();
                var assignmentNotExists = assignmentId?.Where(x => !dbAssignment.Any(x2 => x2.Id == x))?.ToList();
                assignmentNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentNotExists.ToId();
                    message.Add(_messageDescriptions, x, MessageType.AssignmentNotExists, x);
                });
                dbAssignments = dbAssignment;
                messages = message;
            }

            return messages?.Count <= 0;
        }


        public IList<DbModel.AssignmentMessage> AssignAssignmentMessages(List<DbModel.AssignmentMessage> dbMessages,
                                                                 DomainModel.Assignment assignment)
        {
            DbModel.AssignmentMessage assignmentMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)AssignmentMessageType.ReportingRequirements
                                                                         && x.AssignmentId == assignment.AssignmentId);

            if (assignmentMessage == null && !string.IsNullOrEmpty(assignment?.ClientReportingRequirements))
                dbMessages.Add(AddDbAssignmentMessage(AssignmentMessageType.ReportingRequirements,
                                                       assignment?.ClientReportingRequirements,
                                                       true));

            else if (assignmentMessage != null && assignmentMessage.Message != null && !assignmentMessage.Message.Equals(assignment.ClientReportingRequirements))// && !string.IsNullOrEmpty(assignment?.ClientReportingRequirements))
                dbMessages.Add(UpdateDbAssignmentMessage(assignmentMessage,
                                                         assignment?.ClientReportingRequirements,
                                                         true,
                                                         assignment?.ModifiedBy));

            else if (assignmentMessage != null && string.IsNullOrEmpty(assignment?.ClientReportingRequirements))
                dbMessages.Add(UpdateDbAssignmentMessage(assignmentMessage,
                                                         dbMessages?.ToList()?.Where(x => x.MessageTypeId == (int)AssignmentMessageType.ReportingRequirements)?.ToList()?.FirstOrDefault().Message,
                                                         true, //false
                                                         assignment?.ModifiedBy));
            else if (assignmentMessage != null && !string.IsNullOrEmpty(assignment?.ClientReportingRequirements))
            {
                assignmentMessage.Message = assignment?.ClientReportingRequirements;
                dbMessages.Add(UpdateDbAssignmentMessage(assignmentMessage,
                                                        dbMessages?.ToList()?.Where(x => x.MessageTypeId == (int)AssignmentMessageType.ReportingRequirements)?.ToList()?.FirstOrDefault().Message,
                                                        true, //false
                                                        assignment?.ModifiedBy));
            }
            return dbMessages;
        }
        #endregion

        #endregion

        #region Private Exposed Methods

        private Response AddAssignment(IList<DomainModel.Assignment> assignments,
                                       ref DbModel.Assignment dbAssignments,
                                       bool commitChange)
        {
            try
            {
                var assignment = assignments?.FirstOrDefault();
                DbModel.AssignmentMessage dbAssignmentMessages = null;
                if (!string.IsNullOrEmpty(assignment?.ClientReportingRequirements))
                {
                    dbAssignmentMessages = new DbModel.AssignmentMessage
                    {
                        MessageTypeId = (int)AssignmentMessageType.ReportingRequirements,
                        Message = assignment.ClientReportingRequirements,
                        UpdateCount = 0,
                        IsActive = true
                    };
                }

                if (assignment?.AssignmentProjectId == null || assignment.AssignmentContractHoldingCompanyId == null ||
                    assignment.AssignmentOperatingCompanyId == null)
                    return new Response().ToPopulate(ResponseType.Error, null, null, null, null, null);
                var dbAssignment = new DbModel.Assignment
                {
                    ProjectId = (int)assignment.AssignmentProjectId,
                    AssignmentReference = assignment.AssignmentReference,
                    AssignmentNumber = 1,
                    IsAssignmentComplete = assignment.IsAssignmentCompleted != null &&
                                           (bool)assignment.IsAssignmentCompleted,
                    AssignmentStatus = assignment.AssignmentStatus,
                    AssignmentType = assignment.AssignmentType,
                    SupplierPurchaseOrderId = assignment.AssignmentSupplierPurchaseOrderId,
                    ContractCompanyId = (int)assignment.AssignmentContractHoldingCompanyId,
                    ContractCompanyCoordinatorId = assignment.AssignmentContractHoldingCompanyCoordinatorId,
                    OperatingCompanyId = (int)assignment.AssignmentOperatingCompanyId,
                    OperatingCompanyCoordinatorId = assignment.AssignmentOperatingCompanyCoordinatorId,
                    BudgetValue = assignment.AssignmentBudgetValue,
                    BudgetWarning = assignment.AssignmentBudgetWarning,
                    BudgetHours = assignment.AssignmentBudgetHours,
                    BudgetHoursWarning = assignment.AssignmentBudgetHoursWarning,
                    AssignmentLifecycleId = assignment.AssignmentLifecycleId,
                    IsCustomerFormatReportRequired = assignment.IsAssignmentCustomerFormatReportRequired,
                    CustomerAssignmentContactId = assignment.AssignmentCustomerAssigmentContactId,
                    AssignmentCompanyAddressId = assignment.AssignmentCompanyAddressId,
                    ReviewAndModerationProcessId = assignment.AssignmentReviewAndModerationProcessId,
                    CreatedDate = DateTime.UtcNow,
                    AssignmentMessage = new List<DbModel.AssignmentMessage>() { dbAssignmentMessages }
                };

                _assignmentRepository.Add(dbAssignment);
                if (commitChange)
                    _assignmentRepository.ForceSave();
                dbAssignments = dbAssignment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, dbAssignments, null);
        }

        private Response AddAssignment(IList<DomainModel.Assignment> assignments,
                                       ref IList<DbModel.Assignment> dbAssignments,
                                       ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                       bool commitChange, bool isValidationRequire)
        {
            #region Declaration
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Company> dbHostCompanies = null;
            IList<DbModel.User> dbContractCoordinatorUsers = null;
            IList<DbModel.User> dbOperatingUsers = null;
            List<DbModel.Data> dbAssignmentLifeCycle = null;
            List<DbModel.Data> dbReviewAndModerations = null;
            IList<DbModel.CustomerContact> dbCustomerContacts = null;
            IList<DbModel.CustomerAddress> dbCustomerOffices = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.NumberSequence> numberSequenceToInsert = null;
            IList<DbModel.NumberSequence> numberSequenceToUpdate = null;
            IList<DbModel.NumberSequence> numberSequenceToSelect = null;
            #endregion
            try
            {
                Response valdResponse = null;
                IList<DomainModel.Assignment> recordToBeAdd = FilterRecord(assignments, ValidationType.Add);
                if (isValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignments,
                                                           ValidationType.Add,
                                                           ref assignments,
                                                           ref dbAssignments,
                                                           ref assignmentDBCollection);

                if (!isValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    dbProjects = assignmentDBCollection.DBProjects;
                    dbContracts = assignmentDBCollection.DBContracts;
                    dbCompanies = assignmentDBCollection.DBCompanies;
                    dbHostCompanies = assignmentDBCollection.DBCompanies;
                    dbContractCoordinatorUsers = assignmentDBCollection.DBContractCoordinatorUsers;
                    dbOperatingUsers = assignmentDBCollection.DBOperatingUsers;
                    dbAssignmentLifeCycle = assignmentDBCollection.DBAssignmentLifeCycle;
                    dbReviewAndModerations = assignmentDBCollection.DBAssignmentReviewAndModeration;
                    dbCustomerContacts = assignmentDBCollection.DBCustomerContacts;
                    dbCustomerOffices = assignmentDBCollection.DBCustomerOffices;
                    dbCountries = assignmentDBCollection.DBCountry;
                    dbCounties = assignmentDBCollection.DBCounty;
                    dbCities = assignmentDBCollection.DBCity;
                    _assignmentRepository.AutoSave = false;

                    numberSequenceToInsert = new List<DbModel.NumberSequence>();
                    numberSequenceToUpdate = new List<DbModel.NumberSequence>();
                    numberSequenceToSelect = new List<DbModel.NumberSequence>();

                    var modules = _moduleRepository.FindBy(x => x.Name == "Project" || x.Name == "Assignment").ToList();

                    int? projectNumber = dbProjects?.FirstOrDefault()?.Id;
                    List<DbModel.NumberSequence> assignmentNumberSequence = _numberSequenceRepository.FindBy(x => x.ModuleData == projectNumber && x.Module.Name == "Project").ToList();

                    /* Commented for Performance Issue */
                    //var assignmentNumberSequence = _numberSequenceRepository.FindBy(x => dbProjects.Any(x1 => x1.Id == x.ModuleData)
                    //                                                                                   && x.Module.Name == "Project").ToList();

                    dbProjects?.ToList().ForEach(x =>
                    {
                        //var sequenceData = assignmentNumberSequence?.FirstOrDefault(x1 => x1.ModuleData == x.Id);
                        if (assignmentNumberSequence.Count == 0)
                        {
                            DbModel.NumberSequence numberSequenceToInserts = new DbModel.NumberSequence();
                            {
                                numberSequenceToInserts.LastSequenceNumber = 1;
                                numberSequenceToInserts.ModuleId = modules.FirstOrDefault(x1 => x1.Name == "Project").Id;
                                numberSequenceToInserts.ModuleData = x.Id;
                                numberSequenceToInserts.ModuleRefId = modules.FirstOrDefault(x1 => x1.Name == "Assignment").Id;
                                numberSequenceToInsert.Add(numberSequenceToInserts);
                                numberSequenceToSelect = numberSequenceToInsert;
                            }
                        }
                        else
                        {
                            var sequenceData = assignmentNumberSequence?.FirstOrDefault(x1 => x1.ModuleData == x.Id);
                            sequenceData.LastSequenceNumber = sequenceData.LastSequenceNumber + 1;
                            numberSequenceToUpdate.Add(sequenceData);
                            numberSequenceToSelect = numberSequenceToUpdate;
                        }
                    });

                    IList<DbModel.Assignment> dbRecordToBeInserted = _mapper.Map<IList<DbModel.Assignment>>(recordToBeAdd, opt =>
                    {
                        opt.Items["AssignmentCompanyAddress"] = dbCustomerOffices;
                        opt.Items["AssignmentLifeCycle"] = dbAssignmentLifeCycle;
                        opt.Items["AssignmentReview"] = dbReviewAndModerations;
                        opt.Items["AssignmentCustomerContact"] = dbCustomerContacts;
                        opt.Items["AssignmentContractHoldingCompany"] = dbContracts;
                        opt.Items["AssignmentContractHoldingCoordinator"] = dbContractCoordinatorUsers;
                        opt.Items["AssignmentOperatingCompany"] = dbCompanies;
                        opt.Items["AssignmentOperatingCompanyCoordinator"] = dbOperatingUsers;
                        opt.Items["AssignmentHostCompany"] = dbCompanies;
                        opt.Items["AssignmentProjectNumber"] = dbProjects;
                        opt.Items["AssignmentWorkCountry"] = dbCountries;
                        opt.Items["AssignmentWorkCounty"] = dbCounties;
                        opt.Items["AssignmentWorkCity"] = dbCities;
                        opt.Items["isAssignId"] = false;
                        opt.Items["isAssignmentNumber"] = true;
                        opt.Items["AssignmentNumberSequence"] = numberSequenceToSelect?.ToList();
                    });

                    _assignmentRepository.Add(dbRecordToBeInserted);
                    if (commitChange)
                    {
                        _assignmentRepository.ForceSave();
                        _numberSequenceRepository.AutoSave = false;
                        if (numberSequenceToInsert.Count() > 0)
                            _numberSequenceRepository.Add(numberSequenceToInsert);
                        if (numberSequenceToUpdate.Count() > 0)
                            _numberSequenceRepository.Update(numberSequenceToUpdate);
                        _numberSequenceRepository.ForceSave();
                    }
                    dbAssignments = dbRecordToBeInserted;
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                //  _assignmentRepository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), dbAssignments, exception);
        }

        private Response UpdateAssignment(IList<DomainModel.Assignment> assignments,
                                          ref IList<DbModel.Assignment> dbAssignments,
                                          ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                          bool commitChange = true,
                                          bool isValidationRequire = true)
        {
            #region Declaration
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DomainModel.Assignment> result = null;
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Company> dbHostCompanies = null;
            IList<DbModel.User> dbContractCoordinatorUsers = null;
            IList<DbModel.User> dbOperatingUsers = null;
            List<DbModel.Data> dbAssignmentLifeCycle = null;
            List<DbModel.Data> dbReviewAndModerations = null;
            IList<DbModel.CustomerContact> dbCustomerContacts = null;
            IList<DbModel.CustomerAddress> dbCustomerOffices = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCities = null;
            #endregion
            try
            {
                var recordToBeModify = FilterRecord(assignments, ValidationType.Update);
                Response valdResponse = null;

                if (isValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignments,
                                                           ValidationType.Update,
                                                           ref recordToBeModify,
                                                           ref dbAssignments,
                                                           ref assignmentDBCollection);

                if (!isValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbAssignments?.Count > 0))
                {
                    dbProjects = assignmentDBCollection.DBProjects;
                    dbContracts = assignmentDBCollection.DBContracts;
                    dbCompanies = assignmentDBCollection.DBCompanies;
                    dbHostCompanies = assignmentDBCollection.DBCompanies;
                    dbContractCoordinatorUsers = assignmentDBCollection.DBContractCoordinatorUsers;
                    dbOperatingUsers = assignmentDBCollection.DBOperatingUsers;
                    dbAssignmentLifeCycle = assignmentDBCollection.DBAssignmentLifeCycle;
                    dbReviewAndModerations = assignmentDBCollection.DBAssignmentReviewAndModeration;
                    dbCustomerContacts = assignmentDBCollection.DBCustomerContacts;
                    dbCustomerOffices = assignmentDBCollection.DBCustomerOffices;
                    dbCountries = assignmentDBCollection.DBCountry;
                    dbCounties = assignmentDBCollection.DBCounty;
                    dbCities = assignmentDBCollection.DBCity;
                    dbAssignments.ToList().ForEach(assignment =>
                    {
                        var assignmentToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentId == assignment.Id);
                        _mapper.Map(assignmentToBeModify, assignment, opt =>
                        {
                            opt.Items["isAssignId"] = true;
                            opt.Items["isAssignmentNumber"] = false;
                            opt.Items["AssignmentCompanyAddress"] = dbCustomerOffices;
                            opt.Items["AssignmentLifeCycle"] = dbAssignmentLifeCycle;
                            opt.Items["AssignmentReview"] = dbReviewAndModerations;
                            opt.Items["AssignmentCustomerContact"] = dbCustomerContacts;
                            opt.Items["AssignmentContractHoldingCompany"] = dbContracts;
                            opt.Items["AssignmentContractHoldingCoordinator"] = dbContractCoordinatorUsers;
                            opt.Items["AssignmentOperatingCompany"] = dbCompanies;
                            opt.Items["AssignmentOperatingCompanyCoordinator"] = dbOperatingUsers;
                            opt.Items["AssignmentHostCompany"] = dbCompanies;
                            opt.Items["AssignmentProjectNumber"] = dbProjects;
                            opt.Items["AssignmentWorkCountry"] = dbCountries;
                            opt.Items["AssignmentWorkCounty"] = dbCounties;
                            opt.Items["AssignmentWorkCity"] = dbCities;
                        });
                        assignment.LastModification = DateTime.UtcNow;
                        assignment.UpdateCount = assignmentToBeModify.UpdateCount.CalculateUpdateCount();
                        assignment.ModifiedBy = assignmentToBeModify.ModifiedBy;
                    });

                    _assignmentRepository.AutoSave = false;
                    _assignmentRepository.Update(dbAssignments);
                    if (commitChange)
                        _assignmentRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                //_assignmentRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveModule(IList<DomainModel.Assignment> assignments, bool commitChange, bool isValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignments = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                var recordToDelete = FilterRecord(assignments, ValidationType.Delete);
                Response response = null;
                if (isValidationRequire)
                    response = IsRecordValidForProcess(assignments, ValidationType.Delete, ref recordToDelete, ref dbAssignments);

                if (!isValidationRequire || (Convert.ToBoolean(response.Code == ResponseType.Success.ToId()) && dbAssignments?.Count > 0))
                {
                    _assignmentRepository.AutoSave = false;
                    _assignmentRepository.Delete(dbAssignments);
                    if (commitChange)
                        _assignmentRepository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                // _assignmentRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private DbModel.AssignmentMessage AddDbAssignmentMessage(AssignmentMessageType assignmentMessageType,
                                                                  string msgText,
                                                                  bool IsActive)
        {
            var dbMessage = new DbModel.AssignmentMessage()
            {
                Message = msgText,
                MessageTypeId = (int)assignmentMessageType,
                IsActive = IsActive
            };

            return dbMessage;
        }

        private DbModel.AssignmentMessage UpdateDbAssignmentMessage(DbModel.AssignmentMessage dbMessage,
                                                                    string msgText,
                                                                    bool IsActive,
                                                                    string modifyBy = null
                                                                    )
        {
            dbMessage.Message = msgText.IsEmptyReturnNull();
            dbMessage.LastModification = DateTime.UtcNow;
            dbMessage.ModifiedBy = modifyBy;
            dbMessage.IsActive = IsActive;
            return dbMessage;
        }


        private IList<DomainModel.Assignment> FilterRecord(IList<DomainModel.Assignment> assignments,
                                                           ValidationType filterType)
        {
            IList<DomainModel.Assignment> filteredModules = null;

            if (filterType == ValidationType.Add)
                filteredModules = assignments?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredModules = assignments?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredModules = assignments?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredModules;
        }

        private IList<DbModel.Assignment> GetAssignments(IList<DomainModel.Assignment> assignments,
                                                        params Expression<Func<DbModel.Assignment, object>>[] includes)
        {
            IList<DbModel.Assignment> dbAssignments = null;
            if (assignments?.Count > 0)
            {
                var assignmentId = assignments.Where(x => x.AssignmentId > 0).Select(x => x.AssignmentId).ToList();
                if (assignmentId?.Count > 0)
                    dbAssignments = _assignmentRepository.FindBy(x => assignmentId.Contains(x.Id), includes).ToList();
            }

            return dbAssignments;
        }

        private bool IsValidPayload(IList<DomainModel.Assignment> assignments,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var validationResults = _assignmentValidationService.Validate(JsonConvert.SerializeObject(assignments), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            return messages?.Count <= 0;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                                 ValidationType validationType,
                                                 ref IList<DomainModel.Assignment> filteredData,
                                                 ref IList<DbModel.Assignment> dbAssignment,
                                                 ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                                 bool IsAssignmentValidationRequired = true)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> messages = null;
            try
            {
                if (filteredData == null || filteredData.Count <= 0)
                    filteredData = FilterRecord(assignments, validationType);

                if (filteredData != null && filteredData.Count > 0)
                {
                    if (messages == null)
                        messages = new List<ValidationMessage>();

                    result = IsValidPayload(filteredData, validationType, ref messages);

                    if (assignmentDBCollection == null)
                        assignmentDBCollection = new DomainModel.AssignmentDatabaseCollection();
                    if (result)
                    {
                        List<int?> assignmentNotExists = null;
                        var assignmentIds = filteredData.Where(x => x.AssignmentId != null).Select(x => x.AssignmentId).ToList();
                        if ((dbAssignment == null || dbAssignment.Count <= 0) && validationType != ValidationType.Add)
                            dbAssignment = GetAssignments(filteredData,
                                                                prj => prj.Project,
                                                                cnt => cnt.Project.Contract,
                                                                contCompanyCoordinator => contCompanyCoordinator.Project.Coordinator,
                                                                spo => spo.Project.SupplierPurchaseOrder,
                                                                cntCustomer => cntCustomer.Project.Contract.Customer,
                                                                cntCustomerAddress => cntCustomerAddress.Project.Contract.Customer.CustomerAddress);

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsAssignmentExistInDb(assignmentIds,
                                                           dbAssignment,
                                                           ref assignmentNotExists,
                                                           ref messages,
                                                           IsAssignmentValidationRequired);

                            if (result && validationType == ValidationType.Delete)
                                result = IsAssignmentCanBeRemove(dbAssignment, ref messages);

                            else if (result && validationType == ValidationType.Update)
                                result = IsRecordValidForUpdate(filteredData,
                                                                dbAssignment,
                                                                ref assignmentDBCollection,
                                                                ref messages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredData,
                                                         ref assignmentDBCollection,
                                                         ref messages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), filteredData);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<DomainModel.Assignment> filteredData,
                                         ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                         ref IList<ValidationMessage> messages)
        {
            bool result = false;
            if (messages == null)
                messages = new List<ValidationMessage>();

            var projectNumbers = filteredData.Where(x => x.AssignmentProjectNumber != null).Select(x1 => (int)x1.AssignmentProjectNumber).Distinct().ToList();
            var countries = filteredData.Where(x => x.WorkLocationCountry != null).Select(x1 => x1.WorkLocationCountry).Distinct().ToList();
            var companyCodes = filteredData.Where(x => x.AssignmentOperatingCompanyCode != null).Select(x1 => x1.AssignmentOperatingCompanyCode)
                                                    .Union(filteredData.Where(x => x.AssignmentHostCompanyCode != null).Select(x1 => x1.AssignmentHostCompanyCode)).ToList();

            companyCodes = companyCodes.Union(filteredData.FirstOrDefault(x => x.InterCompCodes != null)?.InterCompCodes).ToList();

            var countryIncludes = new string[] { "County",
                                         "County.City"};

            var includes = new string[] {
                                         "Contract.ContractSchedule.ContractRate",
                                         "Contract.Customer.CustomerAddress.CustomerContact",
                                         "SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier.Supplier.SupplierContact"
                                          };

            if (_projectService.IsValidProjectNumber(projectNumbers, ref assignmentDBCollection.DBProjects, ref messages, includes)
            && IsValidContractNumber(assignmentDBCollection.DBProjects, ref assignmentDBCollection, filteredData, ref messages)
            && IsValidCustomer(assignmentDBCollection.DBProjects, ref assignmentDBCollection, filteredData, ref messages)
            && _companyService.IsValidCompany(companyCodes, ref assignmentDBCollection.DBCompanies, ref messages) //usr => usr.User
            && IsValidCompanyAddress(filteredData, ref assignmentDBCollection, ref messages)
            && IsValidCustomerContact(filteredData, ref assignmentDBCollection, ref messages)
            && IsValidSupplierPONumber(filteredData, ref assignmentDBCollection, ref messages)
            && IsValidUser(filteredData, ref assignmentDBCollection, ref messages)
            && IsValidMasterData(filteredData, ref assignmentDBCollection, ref messages)
            && _countryService.IsValidCountryName(countries, ref assignmentDBCollection.DBCountry, ref messages, countryIncludes)
            && IsValidCounty(filteredData, ref assignmentDBCollection, ref messages)
            && IsValidCity(filteredData, ref assignmentDBCollection, ref messages)
            )
            {
                return !result;
            }
            return result;
        }

        private bool IsValidMasterData(IList<DomainModel.Assignment> filteredData,
                                       ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                       ref IList<ValidationMessage> messages)
        {
            Exception exception = null;
            IList<DbModel.Data> dbMasterData = new List<DbModel.Data>();
            try
            {
                IList<ValidationMessage> message = new List<ValidationMessage>();
                if (assignmentDBCollection.DBAssignmentLifeCycle == null || assignmentDBCollection.DBAssignmentStatus == null || assignmentDBCollection.DBAssignmentType == null || assignmentDBCollection.DBAssignmentReviewAndModeration == null)
                {
                    dbMasterData = assignmentDBCollection.DBMasterData;
                    if (dbMasterData == null)
                    {
                        assignmentDBCollection.DBMasterData = this._masterRepository.FindBy(x => x.MasterDataTypeId == (int)MasterType.AssignmentType ||
                                                                              x.MasterDataTypeId == (int)MasterType.AssignmentStatus ||
                                                                              x.MasterDataTypeId == (int)MasterType.AssignmentLifeCycle ||
                                                                              x.MasterDataTypeId == (int)MasterType.ReviewAndModerationProcess).AsNoTracking()
                                                                       .ToList();
                        dbMasterData = assignmentDBCollection.DBMasterData;
                    }

                    var assignmentStatusNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentStatus) && !dbMasterData.Any(x1 => x.AssignmentStatus == x1.Description && x1.MasterDataTypeId == (int)MasterType.AssignmentStatus))?.ToList();
                    var assignmentTypeNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentType) && !dbMasterData.Any(x1 => x.AssignmentType == x1.Description && x1.MasterDataTypeId == (int)MasterType.AssignmentType))?.ToList();
                    var assignmentLifeCycleNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentLifecycle) && !dbMasterData.Any(x1 => x.AssignmentLifecycle == x1.Name && x1.MasterDataTypeId == (int)MasterType.AssignmentLifeCycle))?.ToList();
                    var reviewAndModerationNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentReviewAndModerationProcess) && !dbMasterData.Any(x1 => x.AssignmentReviewAndModerationProcess == x1.Name && x1.MasterDataTypeId == (int)MasterType.ReviewAndModerationProcess))?.ToList();

                    assignmentStatusNotExists?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentStatus.ToId();
                        message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentStatus, x.AssignmentStatus);
                    });

                    assignmentTypeNotExists?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentType.ToId();
                        message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentType, x.AssignmentType);
                    });

                    assignmentLifeCycleNotExists?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentLifeCycle.ToId();
                        message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentLifeCycle, x.AssignmentLifecycle);
                    });
                    reviewAndModerationNotExists?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentLifeCycle.ToId();
                        message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentReviewAndModeration, x.AssignmentReviewAndModerationProcess);
                    });

                    messages = message;
                    assignmentDBCollection.DBAssignmentStatus = dbMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentStatus).ToList();
                    assignmentDBCollection.DBAssignmentType = dbMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentType).ToList(); ;
                    assignmentDBCollection.DBAssignmentLifeCycle = dbMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentLifeCycle).ToList();
                    assignmentDBCollection.DBAssignmentReviewAndModeration = dbMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ReviewAndModerationProcess).ToList();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), filteredData);
            }
            finally { dbMasterData.Clear(); }

            return messages?.Count <= 0;
        }


        private bool IsValidCounty(IList<DomainModel.Assignment> filteredData,
                                   ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                   ref IList<ValidationMessage> messages)
        {
            IList<DbModel.County> dbCounty = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCounty = assignmentDBCollection.DBCountry?.ToList().SelectMany(x => x.County).ToList();

            var countyNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.WorkLocationCounty)
                                                                    && !dbCounty.Any(x1 => x1.Name == x.WorkLocationCounty))?.ToList();
            if (countyNotExists?.Count > 0)
            {
                countyNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.MasterInvalidCounty.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.MasterInvalidCounty, x.WorkLocationCounty);
                });
            }
            assignmentDBCollection.DBCounty = dbCounty;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCity(IList<DomainModel.Assignment> filteredData,
                                   ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                   ref IList<ValidationMessage> messages)
        {
            IList<DbModel.City> dbCity = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCity = assignmentDBCollection.DBCounty?.ToList().SelectMany(x => x.City).ToList();

            var cityNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.WorkLocationCity)
                                                                    && !dbCity.Any(x1 => x1.Name == x.WorkLocationCity))?.ToList();
            if (cityNotExists?.Count > 0)
            {
                cityNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.MasterInvalidCity.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.MasterInvalidCity, x.WorkLocationCity);
                });
            }
            assignmentDBCollection.DBCity = dbCity;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidUser(IList<DomainModel.Assignment> filteredData,
                             ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                             ref IList<ValidationMessage> messages)
        {
            var coordinators = new List<string>() { filteredData.FirstOrDefault(x => !string.IsNullOrEmpty(x.AssignmentContractHoldingCompanyCoordinator))?.AssignmentContractHoldingCompanyCoordinator,
                                                      filteredData.FirstOrDefault(x => !string.IsNullOrEmpty(x.AssignmentOperatingCompanyCoordinator))?.AssignmentOperatingCompanyCoordinator}
                                                 .Select(x => x).ToList();

            var dbCoordinators = _assignmentRepository.GetUser(coordinators);

            assignmentDBCollection.DBContractCoordinatorUsers = dbCoordinators;
            assignmentDBCollection.DBOperatingUsers = dbCoordinators;

            return messages?.Count <= 0;
        }

        private bool IsValidSupplierPONumber(IList<DomainModel.Assignment> filteredData,
                                             ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                             ref IList<ValidationMessage> messages)
        {
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPO = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbSupplierPO = assignmentDBCollection.DBProjects?.SelectMany(x => x.SupplierPurchaseOrder).ToList();

            var supplierPONotExists = filteredData.Where(x => x.AssignmentSupplierPurchaseOrderId > 0
                                                              && !dbSupplierPO.Any(x1 => x1.Id == x.AssignmentSupplierPurchaseOrderId))?.ToList();
            supplierPONotExists?.ForEach(x =>
            {
                string errorCode = MessageType.AssignmentSupplierPONumber.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentSupplierPONumber, x.AssignmentSupplierPurchaseOrderNumber);
            });
            assignmentDBCollection.DBSupplierPO = dbSupplierPO;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCustomerContact(IList<DomainModel.Assignment> filteredData,
                                            ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                            ref IList<ValidationMessage> messages)
        {
            IList<DbModel.CustomerContact> dbCustomerContact = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCustomerContact = assignmentDBCollection.DBCustomerOffices.SelectMany(x => x.CustomerContact).ToList();

            var customerContactNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentCustomerAssigmentContact)
                                                                    && !dbCustomerContact.Any(x1 => x.AssignmentCustomerAssigmentContact == x1.ContactName))?.ToList();
            if (customerContactNotExists.Count > 0)
            {
                customerContactNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentCustomerContact.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentCustomerContact, x.AssignmentCustomerAssigmentContact);
                });
            }
            assignmentDBCollection.DBCustomerContacts = dbCustomerContact;

            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCompanyAddress(IList<DomainModel.Assignment> filteredData,
                                            ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                            ref IList<ValidationMessage> messages)
        {
            IList<DbModel.CustomerAddress> dbCustomerOffice = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCustomerOffice = assignmentDBCollection.DBCustomers?.SelectMany(x => x.CustomerAddress).ToList();

            var customerAddressesNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentCompanyAddress)
                                                                    && !dbCustomerOffice.Any(x1 => String.Join("", x1.Address.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x.AssignmentCompanyAddress.Where(c => !char.IsWhiteSpace(c)))))?.ToList();
            if (customerAddressesNotExists?.Count > 0)
            {
                customerAddressesNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentCompanyAddress.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentCompanyAddress, x.AssignmentCompanyAddress);
                });
            }
            assignmentDBCollection.DBCustomerOffices = dbCustomerOffice;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCustomer(IList<DbModel.Project> dbProjects,
                                    ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                    IList<DomainModel.Assignment> filteredData,
                                    ref IList<ValidationMessage> messages)
        {
            IList<DbModel.Customer> dbCustomer = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCustomer = dbProjects?.ToList().Select(x => x.Contract.Customer).ToList();

            var customertNotExists = filteredData.Where(x => !dbCustomer.Any(x1 => x.AssignmentCustomerCode == x1.Code))?.ToList();
            customertNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.Customer_InvalidCustomerCode.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.Customer_InvalidCustomerCode, x.AssignmentCustomerCode);
            });

            assignmentDBCollection.DBCustomers = dbCustomer;
            messages.AddRange(message);

            return messages?.Count <= 0;
        }


        private bool IsValidContractNumber(IList<DbModel.Project> dbProjects,
                                           ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                           IList<DomainModel.Assignment> filteredData,
                                           ref IList<ValidationMessage> messages)
        {
            IList<DbModel.Contract> dbContract = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbContract = dbProjects?.ToList().Select(x => x.Contract).ToList();

            var contractNotExists = filteredData.Where(x => !dbContract.Any(x1 => x.AssignmentContractNumber == x1.ContractNumber))?.ToList();
            var contractHoldingCompanyNotExists = filteredData.Where(x => !dbContract.Any(x1 => x.AssignmentContractNumber == x1.ContractNumber))?.ToList();

            contractNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractNumber.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.InvalidContractNumber, x.AssignmentContractNumber);
            });

            contractHoldingCompanyNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.AssignmentContractHoldingCompany.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentContractHoldingCompany, x.AssignmentContractHoldingCompanyCode);
            });

            assignmentDBCollection.DBContracts = dbContract;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.Assignment> filteredData,
                                            IList<DbModel.Assignment> dbAssignment,
                                            ref DomainModel.AssignmentDatabaseCollection assignmentDBCollection,
                                            ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (messages?.Count <= 0)
                if (IsRecordUpdateCountMatching(filteredData, dbAssignment, ref messages))
                {
                    assignmentDBCollection.DBProjects = dbAssignment?.Select(x => x.Project)?.ToList();
                    assignmentDBCollection.DBContracts = assignmentDBCollection.DBProjects?.Select(x2 => x2.Contract)?.ToList();


                    var countries = filteredData.Where(x => x.WorkLocationCountry != null).Select(x1 => x1.WorkLocationCountry).Distinct().ToList();
                    var conties = filteredData.Where(x => x.WorkLocationCounty != null).Select(x1 => x1.WorkLocationCounty).Distinct().ToList();
                    var cities = filteredData.Where(x => x.WorkLocationCity != null).Select(x1 => x1.WorkLocationCity).Distinct().ToList();
                    var companyCodes = filteredData.Where(x => x.AssignmentOperatingCompanyCode != null).Select(x1 => x1.AssignmentOperatingCompanyCode)
                                                   .Union(filteredData.Where(x => x.AssignmentHostCompanyCode != null).Select(x1 => x1.AssignmentHostCompanyCode)).ToList();

                    var includes = new string[] { "County",
                                                 "County.City"};

                    if (IsValidCustomer(assignmentDBCollection.DBProjects, ref assignmentDBCollection, filteredData, ref messages)
                          && _companyService.IsValidCompany(companyCodes, ref assignmentDBCollection.DBCompanies, ref messages) //, ref messages, usr => usr.User
                          && IsValidCompanyAddress(filteredData, ref assignmentDBCollection, ref messages)
                          && IsValidCustomerContact(filteredData, ref assignmentDBCollection, ref messages)
                          && IsValidSupplierPONumber(filteredData, ref assignmentDBCollection, ref messages)
                          && IsValidUser(filteredData, ref assignmentDBCollection, ref messages)
                          && _countryService.IsValidCountryName(countries, ref assignmentDBCollection.DBCountry, ref messages, includes)
                          && IsValidCounty(filteredData, ref assignmentDBCollection, ref messages)
                          && IsValidCity(filteredData, ref assignmentDBCollection, ref messages)

                          )
                    {
                        IsValidMasterData(filteredData, ref assignmentDBCollection, ref messages);
                    }
                }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsAssignmentExistInDb(List<int?> assignmentIds,
                                           IList<DbModel.Assignment> dbAssignment,
                                           ref List<int?> assignmentNotExists,
                                           ref IList<ValidationMessage> messages,
                                           bool IsAssignmentValidationRequired = true)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (dbAssignment == null)
                dbAssignment = new List<DbModel.Assignment>();

            var validMessages = messages;

            if (assignmentIds?.Count > 0 && IsAssignmentValidationRequired)
            {
                assignmentNotExists = assignmentIds.Where(x => !dbAssignment.Select(x1 => x1.Id).ToList().Contains((int)x)).ToList();
                assignmentNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                messages = validMessages;

            return messages.Count <= 0;
        }

        private bool IsAssignmentCanBeRemove(IList<DbModel.Assignment> dbAssignment,
                                             ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            dbAssignment.ToList().ForEach(x =>
            {
                bool result = x.IsAnyCollectionPropertyContainValue();
                if (result)
                    validationMessages.Add(_messageDescriptions, x.Id, MessageType.AssignmentIsBeingUsed, x.Id);
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Assignment> filteredData,
                                                 IList<DbModel.Assignment> dbAssignment,
                                                 ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            var notMatchedRecords = filteredData.Where(x => !dbAssignment.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                validationMessages.Add(_messageDescriptions, x, MessageType.ModuleUpdateCountMismatch, x.AssignmentId);
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages?.Count <= 0;
        }


        private Response PopulateAssignmentInvoiceInfo(IList<BudgetAccountItem> budgetAccountItems, IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<InvoicedInfo> result = null;
            IList<DbModel.Data> dbData = null;
            IList<DbModel.Data> masterExpenceTypeToHour = null; 
            Exception exception = null;
            try
            {
                if (budgetAccountItems?.Count > 0)
                {
                    var distinctContractIds = budgetAccountItems.Select(x => x.ContractId).Distinct().ToList();

                    if (contractExchangeRates == null)
                        contractExchangeRates = _contractExchangeRateService.GetContractExchangeRates(distinctContractIds)
                                                                            .Result
                                                                            .Populate<IList<Common.Models.ExchangeRate.ContractExchangeRate>>();
                    if (currencyExchangeRates == null)
                    {
                        dbData= _assignmentRepository.GetMasterData(null, null, new List<int> { (int)MasterType.ExpenseTypeHour,(int)MasterType.Currency }, null);
                        var dbCurrency = dbData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();

                        var contractWithoutExchangeRate = budgetAccountItems?
                                                            .Where(x =>  !(bool)contractExchangeRates?
                                                                        .Any(x1 => x.ContractId == x1.ContractId &&
                                                                             x.ChargeRateCurrency == x1.CurrencyFrom &&
                                                                             x.BudgetCurrency == x1.CurrencyTo)).ToList();

                        var currencyWithoutExchangeRate = contractWithoutExchangeRate?
                                                        .Select(x => new ExchangeRate()
                                                        {
                                                            CurrencyFrom = x.ChargeRateCurrency,
                                                            CurrencyTo = x.BudgetCurrency,
                                                            EffectiveDate = x.VisitDate
                                                        }).ToList();

                        currencyExchangeRates = _currencyExchangeRateService
                                                   .GetExchangeRates(currencyWithoutExchangeRate, dbCurrency)
                                                   .Result
                                                   .Populate<IList<ExchangeRate>>();
                    }

                    //var masterExpenceTypeToHour = _masterService.Search(null, MasterType.ExpenseTypeHour).Result.Populate<List<MasterData>>();
                    if (dbData?.Count > 0)
                        masterExpenceTypeToHour= dbData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseTypeHour)?.ToList();
                    else
                        masterExpenceTypeToHour = _assignmentRepository.GetMasterData(null, null, new List<int> { (int)MasterType.ExpenseTypeHour}, null);

                    ExchangeRateClaculations.CalculateExchangeRate(contractExchangeRates, currencyExchangeRates, budgetAccountItems);

                    result = budgetAccountItems?
                         .GroupBy(x => new { x.ContractId, x.ProjectId, x.AssignmentId })
                         .Select(x =>
                         new InvoicedInfo
                         {
                             ContractId = x.Key.ContractId,
                             ProjectId = x.Key.ProjectId,
                             AssignmentId = x.Key.AssignmentId,
                             InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                             UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                             HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                             HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                             ContractNumber = x?.FirstOrDefault()?.ContractNumber,
                             ProjectNumber = x?.FirstOrDefault()?.ProjectNumber ?? 0,
                             AssignmentNumber = x?.FirstOrDefault()?.AssignmentNumber ?? 0,
                         }).ToList();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), budgetAccountItems);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        public List<DbModel.Assignment> GetAssignmentData(int supplierPOId , int? supplierId , String modifiedby)
        {
            List<int> assignmentIds = new List<int>();
            List<DomainModel.AssignmentSubSupplier> listAssignementSubSupplier = new List<DomainModel.AssignmentSubSupplier>();
            List<DbModel.Assignment> listassignmentdata = _assignmentRepository.GetAssignmentData(supplierPOId);
            foreach (var assignmentdata  in listassignmentdata)
            {
                assignmentIds.Add(assignmentdata.Id);
                /*List<AssignmentSubSupplier> listAssignementSubSupplier = _assignmentSubSupplerRepository.GetAssignmentSubSuppliers(assignmentdata.AssignmentNumber);

                foreach (var assignmentsubsupplierdata in listAssignementSubSupplier)
                {
                    assignmentsubsupplierdata.IsDeleted = true;
                    _assignmentSubSupplerRepository.RemoveAssignementSubSupplier(assignmentsubsupplierdata);
                }*/
            }
            //_assignmentSubSupplerRepository.RemoveAssignementSubSupplier(assignmentIds);
            foreach (var assignmentdata in listassignmentdata)
            {
                AssignmentSubSupplier assignmentSubSupplier = new AssignmentSubSupplier
                {
                AssignmentId = assignmentdata.Id,
                SupplierType = "M",
                SubSupplierId = supplierId,
                ModifiedBy = modifiedby,
            };
                listAssignementSubSupplier.Add(assignmentSubSupplier);
            }
            return listassignmentdata;
        }
        public void RemoveAsssubsupplier(int supplierPOId, List<int> supplierIdlist,string SupplierType)
        {
            List<int> assignmentIds = new List<int>();
            List<DbModel.Assignment> listassignmentdata = _assignmentRepository.GetAssignmentData(supplierPOId);
            foreach (var assignmentdata in listassignmentdata)
            {
                assignmentIds.Add(assignmentdata.Id);
            }
            _assignmentSubSupplerRepository.RemoveAssignementSubSupplier(assignmentIds, supplierIdlist,SupplierType);
        }
        public List<DbModel.Assignment> addAssigenmentSubSupplier(int supplierPOId, List<int> supplierIdlist, String modifiedby,string SupplierType)
        {
            List<int> assignmentIds = new List<int>();
            List<DbModel.Assignment> listassignmentdata = _assignmentRepository.GetAssignmentData(supplierPOId);
            foreach (var assignmentdata in listassignmentdata)
            {
                assignmentIds.Add(assignmentdata.Id);
            }
            foreach (var suppllierId in supplierIdlist)
            {
                List<DomainModel.AssignmentSubSupplier> listAssignementSubSupplier = new List<DomainModel.AssignmentSubSupplier>();
                foreach (var assignmentdata in listassignmentdata)
                {
                    AssignmentSubSupplier assignmentSubSupplier = new AssignmentSubSupplier
                    {
                        AssignmentId = assignmentdata.Id,
                        SupplierType = SupplierType,
                        SubSupplierId = suppllierId,
                        ModifiedBy = modifiedby,
                        IsDeleted = false,
                    };
                    listAssignementSubSupplier.Add(assignmentSubSupplier);
                }
               _assignmentSubSupplerRepository.AddAssignmentsubsupplier(listAssignementSubSupplier);
            }
            return listassignmentdata;
        }
        #endregion
    }
}
