using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Enums;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Evolution.Project.Domain.Enums;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Validations;
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
using DomainModel = Evolution.Project.Domain.Models.Projects;
namespace Evolution.Project.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IAppLogger<ProjectService> _logger = null;
        private readonly IProjectRepository _projectRepository = null;
        private readonly IContractRepository _contractRepository = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly IProjectValidationService _validationService = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IMasterService _masterService = null;
        private readonly IProjectMessageRepository _projectMessageRepository = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public ProjectService(IAppLogger<ProjectService> logger,
                                IProjectRepository repository,
                                IProjectValidationService validationService,
                                IMapper mapper,
                                IContractExchangeRateService contractExchangeRateService,
                                ICurrencyExchangeRateService currencyExchangeRateService,
                                IMasterService masterService, IDataRepository dataRepository,
                                IContractRepository contractRepository,
                                IAssignmentRepository assignmentRepository,
                                ICompanyRepository companyRepository,
                                ICustomerRepository customerRepository,
                                JObject messages,
                                IMongoDocumentService mongoDocumentService,
                                IProjectMessageRepository projectMessageRepository,
                                IAuditSearchService auditSearchService,
                                IOptions<AppEnvVariableBaseModel> environment)
        {
            _mongoDocumentService = mongoDocumentService;
            _logger = logger;
            _projectRepository = repository;
            _validationService = validationService;
            _contractExchangeRateService = contractExchangeRateService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _masterService = masterService;
            _contractRepository = contractRepository;
            _dataRepository = dataRepository;
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _messageDescriptions = messages;
            _projectMessageRepository = projectMessageRepository;
            _auditSearchService = auditSearchService;
            _environment = environment.Value;
        }

        #region Public Exposed Method

        public Response DeleteProjects(IList<DomainModel.Project> projects, ref long? eventId, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true)
        {
            return RemoveProjects(projects, ref eventId, dbModule, commitChange);
        }

        public Response DeleteProjects(IList<DomainModel.Project> projects, ref long? eventId, bool commitChange = true)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            return RemoveProjects(projects, ref eventId,  dbModule, commitChange);
        }

        public async Task<Response> GetProjects(DomainModel.ProjectSearch searchModel, AdditionalFilter filter = null)
        {
            IList<DomainModel.Project> result = null;
            Exception exception = null;
            Int32? count = null;
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
                            var projNumbers = mongoSearch.Select(x => Convert.ToInt32(x)).Distinct().ToList();
                            searchModel.ProjectNumbers = projNumbers;
                            result = GetProjects(searchModel);
                            if (result != null)
                                result = result.Where(x => projNumbers.Contains(Convert.ToInt32(x.ProjectNumber))).ToList();
                        }
                        else
                            result = new List<DomainModel.Project>();
                    }
                    else
                    {
                        result = GetProjects(searchModel);
                    }

                    if (result?.Count > 0 && filter?.IsInvoiceDetailRequired == true)
                    {
                        var contractNumbers = result.Select(x => x.ContractNumber).Distinct().ToList();
                        var projectNumbers = result.Select(x => (int)x.ProjectNumber).Distinct().ToList();
                        var invoiceInfos = GetProjectInvoiceInfo(contractNumbers, projectNumbers, false).Result.Populate<IList<InvoicedInfo>>();

                        result.ToList().ForEach(x =>
                        {
                            var invoiceInfo = invoiceInfos?.FirstOrDefault(x1 => x1.ContractNumber == x.ContractNumber);
                           
                            x.ProjectInvoicedToDate = invoiceInfo?.InvoicedToDate ?? 0;
                            x.ProjectUninvoicedToDate = invoiceInfo?.UninvoicedToDate ?? 0;
                            x.ProjectHoursInvoicedToDate = invoiceInfo?.HoursInvoicedToDate ?? 0;
                            x.ProjectHoursUninvoicedToDate = invoiceInfo?.HoursUninvoicedToDate ?? 0;
                        });
                    }
                }
                else
                    count = _projectRepository.GetCount(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, count == null ? result?.Count : count);

        }

        private IList<DomainModel.Project> GetProjects(DomainModel.ProjectSearch searchModel)
        {
            IList<DomainModel.Project> result = null;
            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
            {
                result = _projectRepository.Search(searchModel);
                tranScope.Complete();
            }
            return result;
        }

        //Can be removed if it is not used -Need to be checked(chunk size)
        //public async Task<Response> SearchProjects(DomainModel.ProjectSearch searchModel)
        //{
        //    IList<DomainModel.ProjectSearch> result = null;
        //    Exception exception = null;
        //    IList<Document.Domain.Models.Document.EvolutionMongoDocument> mongoSearch = null;
        //    try
        //    {
        //        //Mongo Doc Search
        //        if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
        //        {
        //            var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
        //            mongoSearch = await this._mongoDocumentService.SearchAsync(evoMongoDocSearch);
        //            if (mongoSearch != null && mongoSearch.Count > 0)
        //            {
        //                var projNumbers = mongoSearch.Select(x => Convert.ToInt32(x.ReferenceCode)).Distinct().ToList();
        //                searchModel.ProjectNumbers = projNumbers;
        //                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
        //                {
        //                    result = _projectRepository.SearchProject(searchModel,false);
        //                    tranScope.Complete();
        //                }
        //                if (result != null)
        //                    result = result.Where(x => projNumbers.Contains(Convert.ToInt32(x.ProjectNumber))).ToList();
        //            }
        //            else
        //                result = new List<DomainModel.ProjectSearch>();
        //        }
        //        else
        //        {
        //            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
        //            {
        //                result = _projectRepository.SearchProject(searchModel, false);
        //                tranScope.Complete();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        //}

        //Added to perform Chunk
        public async Task<Response> SearchProjects(DomainModel.ProjectSearch searchModel)
        {
            IList<DomainModel.ProjectSearch> result = null;
            Exception exception = null;
            IList<string> mongoSearch = null;
            try
            {
                //Mongo Doc Search
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    {
                        var projNumbers = mongoSearch.Select(x => Convert.ToInt32(x)).Distinct().ToList();
                        searchModel.ProjectNumbers = projNumbers;
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                        {
                            searchModel.FetchCount = _environment.ProjectRecordSize;
                            result = _projectRepository.SearchProject(searchModel);
                            tranScope.Complete();
                        }
                        if (result != null)
                            result = result.Where(x => projNumbers.Contains(Convert.ToInt32(x.ProjectNumber)))?.ToList();
                    }
                    else
                        result = new List<DomainModel.ProjectSearch>();
                }
                else
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        searchModel.FetchCount = _environment.ProjectRecordSize;
                        result = _projectRepository.SearchProject(searchModel);
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

        public Response GetProjects(int projectNumber)
        {
            IList<DomainModel.Project> result = null;
            Exception exception = null;
            try
            {
                if (projectNumber > 0)
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                    {
                        result = _projectRepository.GetProjects(projectNumber);
                        tranScope.Complete();
                    }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNumber);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        public Response GetProjectInvoiceInfo(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true)
        {
            IList<InvoicedInfo> result = null;
            Exception exception = null;
            try
            {
                var projectBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(companyCode, contractIds, userName, contractStatus, showMyAssignmentsOnly);
                return PopulateProjectInvoiceInfo(projectBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetProjectInvoiceInfo(List<string> contractNumber = null, List<int> projectNumber = null, bool IsProjectFetchRequired = true)
        {
            List<int> contractIds = null;
            Exception exception = null;
            try
            {
                contractIds = new List<int>();
                if (contractNumber != null)
                {
                    contractIds.AddRange(_contractRepository.FindBy(x => contractNumber.Contains(x.ContractNumber)).Select(x => x.Id).ToList());
                }
                if (projectNumber != null && IsProjectFetchRequired)
                {
                    contractIds.AddRange(_projectRepository.FindBy(x => x.ProjectNumber != null && projectNumber.Contains(x.ProjectNumber.Value)).Select(x => x.ContractId).Distinct().ToList());
                }

                contractIds = contractIds.Distinct().ToList();

                var projectBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(contractIds: contractIds);
                projectBudgetAccountItems = projectBudgetAccountItems?.Where(x => projectNumber.Contains(x.ProjectNumber))?.ToList();

                return PopulateProjectInvoiceInfo(projectBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNumber);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetProjectBudgetDetails(string companyCode = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true)
        {
            Exception exception = null;
            try
            {

                var contracts = _assignmentRepository.FindBy(x =>
                                          (string.IsNullOrEmpty(companyCode) || x.ContractCompany.Code == companyCode) &&
                                          (contractStatus == ContractStatus.All ||
                                              x.Project.Contract.Status == contractStatus.FirstChar()
                                          ) &&
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
                                             ProjectId = x.Project.Id,
                                             ContractCustomerCode = x.Project.Contract.Customer.Code,
                                             ContractCustomerName = x.Project.Contract.Customer.Name,
                                             BudgetValue = x.Project.Budget,
                                             x.Project.Contract.CustomerContractNumber,
                                             x.Project.Contract.BudgetCurrency,
                                             x.Project.BudgetWarning,
                                             x.Project.BudgetHours,
                                             x.Project.BudgetHoursWarning,
                                         }).Distinct().ToList();

                var contractIds = contracts?.Select(x => x.ContractId).Distinct().ToList();

                var projectInvoiceInfo = _contractRepository
                                                 .GetBudgetAccountItemDetails(companyCode,
                                                                                 contractIds,
                                                                                 userName,
                                                                                 contractStatus,
                                                                                 isAssignmentOnly);

                return GetProjectBudgetDetails(projectInvoiceInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetProjectBudgetDetails(IList<BudgetAccountItem> budgetAccountItems, IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<Budget> result = null;
            Exception exception = null;
            try
            {
                if (budgetAccountItems?.Count > 0)
                {
                    var contractIds = budgetAccountItems.Select(x => x.ContractId).Distinct().ToList();
                    var projects = _projectRepository
                                    .FindBy(x => contractIds.Contains(x.ContractId))
                                    .Select(x => new
                                    {
                                        ContractCustomerCode = x.Contract.Customer.Code,
                                        ContractCustomerName = x.Contract.Customer.Name,
                                        BudgetValue = x.Budget,
                                        ProjectId = x.Id,
                                        x.ContractId,
                                        x.Contract.CustomerContractNumber,
                                        x.Contract.BudgetCurrency,
                                        x.BudgetWarning,
                                        x.BudgetHours,
                                        x.BudgetHoursWarning,
                                    }).ToList();

                    var contractInvoiceInfo = PopulateProjectInvoiceInfo(budgetAccountItems, contractExchangeRates, currencyExchangeRates).Result
                                                                                            .Populate<List<InvoicedInfo>>();

                    result = _mapper.Map<List<InvoicedInfo>, List<Budget>>(contractInvoiceInfo);

                    result.ToList().ForEach(x =>
                    {
                        var contract = projects?.FirstOrDefault(x1 => x1.ProjectId == x.ProjectId);
                        x.ContractCustomerCode = contract.ContractCustomerCode;
                        x.ContractCustomerName = contract.ContractCustomerName;
                        x.BudgetValue = contract.BudgetValue;
                        x.CustomerContractNumber = contract.CustomerContractNumber;
                        x.BudgetCurrency = contract.BudgetCurrency;
                        x.BudgetWarning = contract.BudgetWarning;
                        x.BudgetHours = contract.BudgetHours;
                        x.BudgetHoursWarning = contract.BudgetHoursWarning;
                    });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response ModifyProjects(IList<DomainModel.Project> projects, ref long? eventId, bool commitChange = true)
        {
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Data> dbProjectRef = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateProjects(projects, ref dbProjects, ref dbProjectRef, ref eventId,ref dbModule, commitChange);
        }

        public Response ModifyProjects(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProjects, ref IList<DbModel.Data> dbProjectRef, ref long? eventId,ref IList<DbModel.SqlauditModule> dbModule, bool commitChange = true)
        {
            return UpdateProjects(projects, ref dbProjects, ref dbProjectRef, ref eventId, ref dbModule, commitChange);
        }

        public Response SaveProjects(IList<DomainModel.Project> projects, ref long? eventId, bool commitChange = true)
        {
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Data> dbProjectReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return AddProjects(projects, ref dbProject, ref dbProjectReference, ref eventId, ref dbModule, commitChange);
        }

        public Response SaveProjects(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProject, ref IList<DbModel.Data> dbProjectReference, ref long? eventId,ref IList<DbModel.SqlauditModule> dbModule,bool commitChange = true)
        {
            return AddProjects(projects, ref dbProject, ref dbProjectReference, ref eventId, ref dbModule,commitChange);
        }

        public bool IsValidProjectNumber(IList<int> projectNumber, ref IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> messages, params Expression<Func<DbModel.Project, object>>[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            var dbProjectNumber = _projectRepository?.FindBy(x => projectNumber.Contains((int)x.ProjectNumber), includes)?.ToList();

            var projectNotExists = projectNumber?.Where(x => !dbProjectNumber.Any(x2 => x2.ProjectNumber == x))?.ToList();
            projectNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectNumber.ToId();
                message.Add(_messageDescriptions, x, MessageType.InvalidProjectNumber, x);
            });
            dbProjects = dbProjectNumber;
            messages = message;

            return messages?.Count <= 0;
        }

        public bool IsValidProjectNumber(IList<int> projectNumber, ref IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> messages, string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            var dbProjectNumber = _projectRepository?.FindBy(x => projectNumber.Contains((int)x.ProjectNumber), includes)?.ToList();

            var projectNotExists = projectNumber?.Where(x => !dbProjectNumber.Any(x2 => x2.ProjectNumber == x))?.ToList();
            projectNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectNumber.ToId();
                message.Add(_messageDescriptions, x, MessageType.InvalidProjectNumber, x);
            });
            dbProjects = dbProjectNumber;
            messages = message;

            return messages?.Count <= 0;
        }

        public Response ProjectValidForDeletion(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProjects)
        {
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Project> recordToBeDeleted = null;
            Exception exception = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (IsRecordValidForProcess(projects, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                {
                    if (IsValidProject(recordToBeDeleted, ref dbProjects, ref errorMessages))
                    {
                        if (IsRecordCanBeDeleted(recordToBeDeleted, dbProjects, ref errorMessages))
                            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projects);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        public Response GetProjectBasedOnStatus(string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isVisit, bool isOperating, bool isNDT, int? CoordinatorId)
        {
            IList<DomainModel.Project> result = null;
            Exception exception = null;
            try
            {
                result = _projectRepository.GetProjectBasedOnStatus(contractNumber, ContractHolderCompanyId, isApproved, isVisit, isOperating, isNDT, CoordinatorId);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetProjectKPI(string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            IList<DomainModel.Project> result = null;
            Exception exception = null;
            try
            {
                result = _projectRepository.GetProjectKPI(contractNumber, ContractHolderCompanyId, isVisit, isOperating);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Private Exposed Methods
        private Response RemoveProjects(IList<DomainModel.Project> projects, ref long? eventId,  IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Project> result = null;
            long? eventID = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (projects?.Count > 0)
                {
                    _projectRepository.AutoSave = false;
                    errorMessages = new List<MessageDetail>();
                    IList<DomainModel.Project> recordToBeDeleted = null;
                    if (IsRecordValidForProcess(projects, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                    {
                        IList<DbModel.Project> dbProjects = null;
                        if (IsValidProjectNumber(recordToBeDeleted, ref dbProjects, ref errorMessages))
                        {
                            if (IsRecordCanBeDeleted(recordToBeDeleted, dbProjects, ref errorMessages))
                            {
                                _projectRepository.DeleteProject(dbProjects);
                                if (dbProjects?.Count > 0 && projects?.Count > 0)
                                {
                                   dbProjects?.ToList().ForEach(x =>
                                   projects?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                      "{" + AuditSelectType.Id + ":" + x.ProjectNumber + "}${" + AuditSelectType.ProjectNumber + ":" + x.ProjectNumber
                                                                                                     + "}${" + AuditSelectType.CustomerProjectName + ":" + x.CustomerProjectName.Trim()
                                                                                                     + "}${" + AuditSelectType.CustomerProjectNumber + ":" + x.CustomerProjectNumber.Trim()
                                                                                                     + "}${" + AuditSelectType.ContractNumber + ":" + x.Contract.ContractNumber + "}",
                                                                       ValidationType.Delete.ToAuditActionType(),
                                                                       SqlAuditModuleType.Project,
                                                                       x1,
                                                                      null,
                                                                      dbModule
                                                                       )));


                                    eventId = eventID;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projects);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private Response AddProjects(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProjects, ref IList<DbModel.Data> dbProjectRef, ref long? eventId,ref IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Project> result = null;
            long? eventID = 0;
            try
            {
                _projectRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                if (projects?.Count > 0)
                {
                    IList<DbModel.CompanyOffice> dbCompanyOffices = null;
                    IList<DbModel.Contract> dbContracts = null;
                    IList<DbModel.Data> dbProjectTypes = null;
                    IList<DbModel.Data> dbPaymentTerms = null;
                    IList<DbModel.Data> dbLogo = null;
                    IList<DbModel.CompanyDivision> dbCompanyDivisions = null;
                    IList<DbModel.CompanyDivisionCostCenter> dbCompanyDivisionCostCenters = null;
                    IList<DbModel.User> dbUsers = null;
                    IList<DomainModel.Project> recordToBeInserted = null;
                    IList<DbModel.Data> dbMasterData = null;

                    bool IsDataValid = Validate(projects, ref recordToBeInserted, ValidationType.Add, ref dbCompanyOffices, ref dbContracts,
                              ref dbProjectTypes, ref dbPaymentTerms, ref dbMasterData, ref dbLogo, ref dbCompanyDivisions,
                              ref dbCompanyDivisionCostCenters, ref dbUsers, ref dbProjects, ref errorMessages, ref validationMessages,ref dbModule);

                    dbProjectRef = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType))?.ToList();

                    if (IsDataValid)
                    {
                        var dbProjectToBeInserted = AssignProjectValues(recordToBeInserted, dbContracts, dbCompanyDivisions,
                                            dbCompanyDivisionCostCenters, dbPaymentTerms, dbProjectTypes, dbUsers, dbLogo);

                        if (dbProjectToBeInserted != null)
                            _projectRepository.Add(dbProjectToBeInserted);

                        if (commitChange && !_projectRepository.AutoSave && dbProjectToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                        {
                            int value = _projectRepository.ForceSave();
                            if (value > 0)
                            {
                                UpdateProjectNumber(dbProjectToBeInserted);
                                result = _mapper.Map<IList<DomainModel.Project>>(dbProjectToBeInserted);
                                dbProjects = dbProjectToBeInserted;
                                if (dbProjectToBeInserted?.Count > 0 && recordToBeInserted?.Count > 0)
                                {
                                    var dbModules = dbModule;
                                    dbProjectToBeInserted?.ToList().ForEach(x =>
                                    {
                                        var domProjectAudit = _mapper.Map<DomainModel.Project>(x);
                                        domProjectAudit.ManagedServiceTypeName = dbMasterData?.FirstOrDefault(x1 => x1.Id == x.ManagedServicesType)?.Name;

                                        domProjectAudit.ProjectType = dbProjectTypes?.FirstOrDefault(x1 => x1.Id == x.ProjectTypeId)?.Name;
                                        domProjectAudit.ProjectInvoicePaymentTerms = dbPaymentTerms?.FirstOrDefault(x2 => x2.Id == x.InvoicePaymentTermsId)?.Name;
                                        recordToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                                   "{" + AuditSelectType.Id + ":" + x.ProjectNumber + "}${" + AuditSelectType.ProjectNumber + ":" + x.ProjectNumber
                                                                                                        + "}${" + AuditSelectType.CustomerProjectName + ":" + x.CustomerProjectName.Trim()
                                                                                                     + "}${" + AuditSelectType.CustomerProjectNumber + ":" + x.CustomerProjectNumber.Trim()
                                                                                                     + "}${" + AuditSelectType.ContractNumber + ":" + x.Contract.ContractNumber + "}",
                                                                                                                   ValidationType.Add.ToAuditActionType(),
                                                                                                                   SqlAuditModuleType.Project,
                                                                                                                   null,
                                                                                                                  domProjectAudit,
                                                                                                                  dbModules));
                                    });
                                    eventId = eventID;

                                }
                            }
                            else
                                result = recordToBeInserted;
                        }
                        else
                            result = recordToBeInserted;
                    }
                    else
                        return new Response().ToPopulate(ResponseType.Validation, null, errorMessages, validationMessages, result, exception);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projects);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }
        //Added for D-1304-Start
        private bool BudgetValidation(IList<DomainModel.Project> projects,IList<DbModel.Contract> dbContracts, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            string budgetCode = _projectRepository.ValidateContractBudget(projects.FirstOrDefault(), dbContracts.FirstOrDefault());
            if (!string.IsNullOrEmpty(budgetCode))
            {
                List<ValidationMessage> messages = new List<ValidationMessage>();

                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                messages.Add(new ValidationMessage(ResponseType.Warning.ToId(), new List<MessageDetail> { new MessageDetail(ModuleType.Project, budgetCode, string.Format(_messageDescriptions[budgetCode].ToString())) }));
                if (messages.Count > 0)
                {
                    validationMessages.AddRange(messages);
                }
            }
            return validationMessages?.Count <= 0;
        }
        //Added for D-1304-End

        private bool ProjectAssignmentBudgetValidation(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            string budgetCode = _projectRepository.ValidateAssignmentBudget(projects.FirstOrDefault());
            if (!string.IsNullOrEmpty(budgetCode))
            {
                List<ValidationMessage> messages = new List<ValidationMessage>();

                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                messages.Add(new ValidationMessage(ResponseType.Warning.ToId(), new List<MessageDetail> { new MessageDetail(ModuleType.Project, budgetCode, string.Format(_messageDescriptions[budgetCode].ToString())) }));
                if (messages.Count > 0)
                {
                    validationMessages.AddRange(messages);
                }
            }
            return validationMessages?.Count <= 0;
        }
        private bool Validate(IList<DomainModel.Project> projects, ref IList<DomainModel.Project> recordTobeProcessed, ValidationType validationType,
                              ref IList<DbModel.CompanyOffice> dbCompanyOffices, ref IList<DbModel.Contract> dbContracts,
                              ref IList<DbModel.Data> dbProjectTypes, ref IList<DbModel.Data> dbPaymentTerms, ref IList<DbModel.Data> dbMasterData,
                              ref IList<DbModel.Data> dbLogo, ref IList<DbModel.CompanyDivision> dbCompanyDivisions,
                              ref IList<DbModel.CompanyDivisionCostCenter> dbCompanyDivisionCostCenters, ref IList<DbModel.User> dbUsers,
                              ref IList<DbModel.Project> dbProjects, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages,
                              ref IList<DbModel.SqlauditModule> dbModule)
        {
            using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                IsValidMasterData(projects, ref dbMasterData);
                if (IsRecordValidForProcess(projects, validationType, ref recordTobeProcessed, ref errorMessages, ref validationMessages))
                {
                    if (IsValidContract(recordTobeProcessed, ref dbContracts, ref errorMessages))
                    {
                        if (IsValidCompanyDivision(recordTobeProcessed, dbContracts, ref dbCompanyDivisions, ref errorMessages))
                        {
                            if (IsValidCompanyDivisionCostCenters(recordTobeProcessed, dbCompanyDivisions, ref dbCompanyDivisionCostCenters, ref errorMessages))
                            {
                                if (IsValidCompanyOffice(recordTobeProcessed, dbContracts, ref dbCompanyOffices, ref errorMessages))
                                {
                                    if (IsValidProjectType(recordTobeProcessed, ref dbProjectTypes, dbMasterData, ref errorMessages))
                                    {
                                        if (IsValidIndustrySector(recordTobeProcessed, dbMasterData, ref errorMessages))
                                        {
                                            if (IsValidUser(recordTobeProcessed, ref dbUsers, dbContracts, ref errorMessages))
                                            {
                                                if (IsValidCustomerAddress(recordTobeProcessed, dbContracts, ref errorMessages))
                                                {
                                                    if (IsValidCustomerContact(recordTobeProcessed, dbContracts, ref errorMessages))
                                                    {
                                                        if (IsValidInvoicePaymentTerm(recordTobeProcessed, ref dbPaymentTerms, dbMasterData, ref errorMessages))
                                                        {
                                                            if (IsValidIdentifier(recordTobeProcessed, dbContracts, ref errorMessages))
                                                            {
                                                                if (IsValidTax(recordTobeProcessed, dbContracts, ref errorMessages))
                                                                {
                                                                    if (IsValidCurrency(recordTobeProcessed, dbMasterData, ref errorMessages))
                                                                    {
                                                                        if (IsValidLogo(recordTobeProcessed, ref dbLogo, dbMasterData, ref errorMessages))
                                                                        {
                                                                            dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                                                        SqlAuditModuleType.Project.ToString(),
                                                                                        SqlAuditModuleType.ProjectAttachment.ToString(),
                                                                                        SqlAuditModuleType.ProjectReference.ToString(),
                                                                                        SqlAuditModuleType.ProjectNote.ToString(),
                                                                                        SqlAuditModuleType.ProjectNotification.ToString(),
                                                                                        SqlAuditModuleType.ProjectDocument.ToString()
                                                                                        });
                                                                            //Added for D-1304
                                                                            if (BudgetValidation(projects,dbContracts, validationType, ref validationMessages))
                                                                            {
                                                                                if (validationType == ValidationType.Update)
                                                                                {
                                                                                    if (IsValidProjectNumber(recordTobeProcessed, ref dbProjects, ref errorMessages))
                                                                                    {
                                                                                        if (ProjectAssignmentBudgetValidation(projects, dbContracts, validationType, ref validationMessages))
                                                                                        {
                                                                                            if (IsRecordUpdateCountMatching(recordTobeProcessed, dbProjects, ref errorMessages))
                                                                                                return true;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        return false;
                                                                                }
                                                                                else
                                                                                    return true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    tranScope.Complete();
                }
            }
            return false;
        }

        private Response UpdateProjects(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProjects, ref IList<DbModel.Data> dbProjectRef, ref long? eventId, ref IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Project> result = null;
            long? eventID = 0;
            try
            {
                _projectRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                if (projects?.Count > 0)
                {
                    IList<DbModel.CompanyOffice> dbCompanyOffices = null;
                    IList<DbModel.Contract> dbContracts = null;
                    //IList<DbModel.Project> dbProjects = null;
                    IList<DbModel.Data> dbProjectTypes = null;
                    IList<DbModel.Data> dbPaymentTerms = null;
                    IList<DbModel.Data> dbLogo = null;
                    IList<DbModel.CompanyDivision> dbCompanyDivisions = null;
                    IList<DbModel.CompanyDivisionCostCenter> dbCompanyDivisionCostCenters = null;
                    IList<DbModel.User> dbUsers = null;
                    IList<DomainModel.Project> recordToBeModify = null;
                    IList<DbModel.Data> dbMasterData = null;

                    bool IsDataValid = Validate(projects, ref recordToBeModify, ValidationType.Update, ref dbCompanyOffices, ref dbContracts,
                              ref dbProjectTypes, ref dbPaymentTerms, ref dbMasterData, ref dbLogo, ref dbCompanyDivisions,
                              ref dbCompanyDivisionCostCenters, ref dbUsers, ref dbProjects, ref errorMessages, ref validationMessages,ref dbModule);

                    dbProjectRef = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType))?.ToList();
                    if (IsDataValid)
                    {
                        IList<DomainModel.Project> domExistingProjects = new List<DomainModel.Project>();
                        dbProjects?.ToList().ForEach(x =>
                        {
                            domExistingProjects.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.Project>(x)));
                        });
                        var dbProjectToBeUpdated = AssignProjectValues(recordToBeModify, dbContracts, dbCompanyDivisions, dbCompanyDivisionCostCenters, dbPaymentTerms,
                                                                        dbProjectTypes, dbUsers, dbLogo, dbProjects);
                        _projectRepository.Update(dbProjectToBeUpdated);
                        if (commitChange && recordToBeModify?.Count > 0)
                        {
                            _projectMessageRepository.ForceSave();

                            int value = _projectRepository.ForceSave();
                            if (value > 0)
                            {
                                result = _mapper.Map<IList<DomainModel.Project>>(dbProjectToBeUpdated);
                            }
                            else
                                result = recordToBeModify;
                            if (dbProjectToBeUpdated?.Count > 0 && recordToBeModify?.Count > 0)
                            {
                                var dbModules = dbModule;
                                dbProjectToBeUpdated?.ToList().ForEach(x =>
                                              recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                    "{" + AuditSelectType.Id + ":" + x.ProjectNumber + "}${" + AuditSelectType.ProjectNumber + ":" + x.ProjectNumber
                                                                                                 + "}${" + AuditSelectType.CustomerProjectName + ":" + x.CustomerProjectName.Trim()
                                                                                                 + "}${" + AuditSelectType.CustomerProjectNumber + ":" + x.CustomerProjectNumber.Trim()
                                                                                                 + "}${" + AuditSelectType.ContractNumber + ":" + x.Contract.ContractNumber + "}",
                                                                                                 ValidationType.Update.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.Project,
                                                                                                 _mapper.Map<DomainModel.Project>(domExistingProjects?.FirstOrDefault(x2 => x2.ProjectNumber == x1.ProjectNumber)),
                                                                                                  x1, dbModules
                                                                                                  )));


                                eventId = eventID;
                            }
                        }
                        else
                            result = recordToBeModify;
                    }
                    else
                        return new Response().ToPopulate(ResponseType.Validation, null, errorMessages, validationMessages, result, exception);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projects);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private List<DbModel.Project> AssignProjectValues(IList<DomainModel.Project> recordToBeInserted, IList<DbModel.Contract> dbContracts,
           IList<DbModel.CompanyDivision> dbCompanyDivisions, IList<DbModel.CompanyDivisionCostCenter> dbDivisionCostCenters, IList<DbModel.Data> dbPaymentTerms,
           IList<DbModel.Data> dbProjectType, IList<DbModel.User> dbUsers, IList<DbModel.Data> dbLogo)
        {
            Exception exception = null;
            List<DbModel.Project> projects = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    projects = recordToBeInserted?.ToList()?.Select(x => new DbModel.Project()
                    {
                        ContractId = (int)dbContracts?.ToList().FirstOrDefault(x1 => x1.ContractNumber == x.ContractNumber)?.Id,
                        StartDate = (DateTime)x.ProjectStartDate,
                        EndDate = x.ProjectEndDate,
                        Status = x.ProjectStatus,
                        Budget = x.ProjectBudgetValue,
                        BudgetWarning = x.ProjectBudgetWarning,
                        BudgetHours = x.ProjectBudgetHoursUnit,
                        BudgetHoursWarning = x.ProjectBudgetHoursWarning,
                        WorkFlowType = x.WorkFlowType,
                        //CoordinatorId = (int)dbUsers?.ToList().FirstOrDefault(x1 => !string.IsNullOrEmpty(x.ProjectCoordinatorName) && x1.Name == x.ProjectCoordinatorName)?.Id,
                        CoordinatorId =(int)dbUsers?.ToList().FirstOrDefault(x1=> !string.IsNullOrEmpty(x.ProjectCoordinatorCode) && x1.SamaccountName == x.ProjectCoordinatorCode)?.Id, //IGO QC D-900 Issue 1
                        ProjectTypeId = (int)dbProjectType?.ToList().FirstOrDefault(x1 => !string.IsNullOrEmpty(x.ProjectType) && x1.Name == x.ProjectType)?.Id,
                        IndustrySector = x.IndustrySector,
                        IsNewFacility = x.IsProjectForNewFacility,
                        CreationDate = DateTime.UtcNow,
                        IsManagedServices = x.IsManagedServices,
                        IsVisitOnPopUp = x.IsVisitOnPopUp,
                        ManagedServicesType = x.ManagedServiceType,
                        ManagedServicesCoordinatorId = !string.IsNullOrEmpty(x.ManagedServiceCoordinatorName) ? dbUsers?.ToList().FirstOrDefault(x2 => x2.SamaccountName == x.ManagedServiceCoordinatorCode)?.Id : null, //IGO QC D-900 Issue 1
                        IsExtranetSummaryVisibleToClient = x.IsExtranetSummaryVisibleToClient,
                        IsEreportProjectMapped = x.IsEReportProjectMapped,
                        CompanyDivisionId = (int)dbCompanyDivisions?.ToList().FirstOrDefault(x1 => !string.IsNullOrEmpty(x.CompanyDivision) && x1.Division?.Name == x.CompanyDivision && !string.IsNullOrEmpty(x.ContractHoldingCompanyCode) &&
                                            x1.Company.Code == x.ContractHoldingCompanyCode)?.Id,
                        CompanyanyDivCostCentreId = (int)dbDivisionCostCenters?.ToList().FirstOrDefault(x2 => x2.Code == x.CompanyCostCenterCode
                                                                && x2.Name == x.CompanyCostCenterName    //Added for D-996
                                                                && x2.CompanyDivision?.Division?.Name == x.CompanyDivision
                                                                && x2.CompanyDivision?.Company?.Code == x.ContractHoldingCompanyCode)?.Id,

                        CompanyOfficeId = (int)dbContracts?.SelectMany(x1 => x1.ContractHolderCompany?.CompanyOffice?.Where(x2 => String.Join("", x2.OfficeName.Where(c => !char.IsWhiteSpace(c)))
                                                == String.Join("", x.CompanyOffice.Where(c => !char.IsWhiteSpace(c))))).FirstOrDefault().Id,

                        CustomerProjectNumber = x.CustomerProjectNumber,
                        CustomerProjectName = x.CustomerProjectName,
                        CustomerContactId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress).SelectMany(x1 => x1.CustomerContact)?.ToList()
                                                        ?.FirstOrDefault(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x.ProjectCustomerInvoiceContact.Where(c => !char.IsWhiteSpace(c))))?.Id,

                        CustomerInvoiceAddressId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress)?
                                                        .FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                         == String.Join("", x.ProjectCustomerInvoiceAddress.Where(c => !char.IsWhiteSpace(c))))?.Id,


                        CustomerProjectContactId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress).SelectMany(x1 => x1.CustomerContact)?.ToList()
                                                        ?.FirstOrDefault(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x.ProjectCustomerContact.Where(c => !char.IsWhiteSpace(c))))?.Id,


                        CustomerProjectAddressId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress)?
                                                        .FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                         == String.Join("", x.ProjectCustomerContactAddress.Where(c => !char.IsWhiteSpace(c))))?.Id,

                        InvoiceSalesTaxId = dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?   ///Added for D-1164
                                            (int)dbContracts?.ToList().SelectMany(x1=> x1.ParentContract?.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == x.ProjectSalesTax)?.Id:
                                            (int)dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == x.ProjectSalesTax)?.Id,
                        InvoiceWithholdingTaxId = !string.IsNullOrEmpty(x.ProjectWithHoldingTax) ? 
                                        (dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?     //Added for D-1164
                                            dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == x.ProjectWithHoldingTax)?.Id :
                                            dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == x.ProjectWithHoldingTax)?.Id ): null,
                        InvoiceCurrency = x.ProjectInvoicingCurrency,
                        InvoiceGrouping = x.ProjectInvoiceGrouping,
                        InvoiceRemittanceTextId = dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?    //Added for D-1164
                                                (int)dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x2 => x2?.Identifier == x.ProjectInvoiceRemittanceIdentifier && x2?.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractRemittanceText))?.Id : //Changes for D1444
                                                (int)dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x1 => x1.Company?.Code == x.ContractHoldingCompanyCode && x1.Identifier == x.ProjectInvoiceRemittanceIdentifier && x1.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractRemittanceText))?.Id,
                        InvoiceFooterTextId = dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?    //Added for D-1164
                                            (int)dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x2 => x2?.Identifier == x.ProjectInvoiceFooterIdentifier && x2?.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractFooterText))?.Id : //Changes for D1444
                                            (int)dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x1 => x1.Company?.Code == x.ContractHoldingCompanyCode && x1.Identifier == x.ProjectInvoiceFooterIdentifier && x1.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractFooterText))?.Id,
                        InvoicePaymentTermsId = dbPaymentTerms?.FirstOrDefault(x2 => x2.Name == x.ProjectInvoicePaymentTerms)?.Id,
                        IsRemittanceText = true,
                        ModifiedBy = x.ModifiedBy,
                        LogoId = !string.IsNullOrEmpty(x.LogoText) ? dbLogo?.ToList().FirstOrDefault(x1 => x1.Name == x.LogoText)?.Id : null,
                        CustomerDirectReportingEmailAddress = x.CustomerDirectReportingEmailAddress,
                        ProjectMessage = AssignProjectMessages(new List<DbModel.ProjectMessage>(), x)
                    }).ToList();

                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projects);
            }

            return projects;
        }

        private List<DbModel.Project> AssignProjectValues(IList<DomainModel.Project> recordToBeModify, IList<DbModel.Contract> dbContracts,
           IList<DbModel.CompanyDivision> dbCompanyDivisions, IList<DbModel.CompanyDivisionCostCenter> dbDivisionCostCenters, IList<DbModel.Data> dbPaymentTerms,
           IList<DbModel.Data> dbProjectType, IList<DbModel.User> dbUsers, IList<DbModel.Data> dbLogo, IList<DbModel.Project> dbProjects)
        {
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    dbProjects.ToList()?.ForEach(x =>
                    {
                        var recordToBeModified = recordToBeModify.FirstOrDefault(x1 => x1.ProjectNumber == x.ProjectNumber);
                        x.ContractId = (int)dbContracts.ToList().FirstOrDefault(x1 => x1.ContractNumber == recordToBeModified.ContractNumber)?.Id;
                        x.StartDate = (DateTime)recordToBeModified.ProjectStartDate;
                        x.EndDate = recordToBeModified.ProjectEndDate;
                        x.Status = recordToBeModified.ProjectStatus;
                        x.Budget = recordToBeModified.ProjectBudgetValue;
                        x.BudgetWarning = recordToBeModified.ProjectBudgetWarning;
                        x.BudgetHours = recordToBeModified.ProjectBudgetHoursUnit;
                        x.BudgetHoursWarning = recordToBeModified.ProjectBudgetHoursWarning;
                        x.WorkFlowType = recordToBeModified.WorkFlowType;
                        //x.CoordinatorId = (int)dbUsers?.ToList().FirstOrDefault(x1 => x1.Name == recordToBeModified.ProjectCoordinatorName)?.Id;
                        x.CoordinatorId = (int)dbUsers?.ToList().FirstOrDefault(x1 => x1.SamaccountName == recordToBeModified.ProjectCoordinatorCode)?.Id; //IGO QC D-900 Issue 1
                        x.ProjectTypeId = (int)dbProjectType?.ToList().FirstOrDefault(x1 => x1.Name == recordToBeModified.ProjectType)?.Id;
                        x.IndustrySector = recordToBeModified.IndustrySector;
                        x.IsNewFacility = recordToBeModified.IsProjectForNewFacility;
                        // x.CreationDate = recordToBeModified.CreationDate;
                        x.IsManagedServices = recordToBeModified.IsManagedServices;
                        x.IsVisitOnPopUp = recordToBeModified.IsVisitOnPopUp;
                        x.ManagedServicesType = recordToBeModified.ManagedServiceType;
                        x.ManagedServicesCoordinatorId = !string.IsNullOrEmpty(recordToBeModified.ManagedServiceCoordinatorName) ? dbUsers?.ToList().FirstOrDefault(x2 => x2.SamaccountName == recordToBeModified.ManagedServiceCoordinatorCode)?.Id : null; //IGO QC D-900 Issue 1
                        x.IsExtranetSummaryVisibleToClient = recordToBeModified.IsExtranetSummaryVisibleToClient;
                        x.IsEreportProjectMapped = recordToBeModified.IsEReportProjectMapped;
                        x.CompanyDivisionId = (int)dbCompanyDivisions?.ToList().FirstOrDefault(x1 => x1.Division?.Name == recordToBeModified.CompanyDivision && x1.Company?.Code == recordToBeModified.ContractHoldingCompanyCode)?.Id;
                        x.CompanyanyDivCostCentreId = (int)dbDivisionCostCenters?.ToList().FirstOrDefault(x1 => x1.Code == recordToBeModified.CompanyCostCenterCode
                                                                && x1.Name == recordToBeModified.CompanyCostCenterName    //Added for D-996
                                                                && x1.CompanyDivision.Division?.Name == recordToBeModified.CompanyDivision
                                                                && x1.CompanyDivision.Company?.Code == recordToBeModified.ContractHoldingCompanyCode)?.Id;

                        x.CompanyOfficeId = (int)dbContracts?.SelectMany(x1 => x1.ContractHolderCompany?.CompanyOffice?.Where(x2 => String.Join("", x2.OfficeName.Where(c => !char.IsWhiteSpace(c)))
                                                == String.Join("", recordToBeModified.CompanyOffice.Where(c => !char.IsWhiteSpace(c))))).FirstOrDefault().Id;
                        x.CustomerProjectNumber = recordToBeModified.CustomerProjectNumber;
                        x.CustomerProjectName = recordToBeModified.CustomerProjectName;

                        x.CustomerContactId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress).SelectMany(x1 => x1.CustomerContact).ToList()
                                                            ?.FirstOrDefault(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", recordToBeModified.ProjectCustomerInvoiceContact.Where(c => !char.IsWhiteSpace(c))))?.Id;

                        x.CustomerInvoiceAddressId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress)?
                                                            .FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                             == String.Join("", recordToBeModified.ProjectCustomerInvoiceAddress.Where(c => !char.IsWhiteSpace(c))))?.Id;


                        x.CustomerProjectContactId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress).SelectMany(x1 => x1.CustomerContact).ToList()
                                                            ?.FirstOrDefault(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", recordToBeModified.ProjectCustomerContact.Where(c => !char.IsWhiteSpace(c))))?.Id;


                        x.CustomerProjectAddressId = (int)dbContracts?.ToList().SelectMany(x1 => x1.Customer?.CustomerAddress)?
                                                            .FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                             == String.Join("", recordToBeModified.ProjectCustomerContactAddress.Where(c => !char.IsWhiteSpace(c))))?.Id;

                        x.InvoiceSalesTaxId = dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?    //Added for D-1164
                                            (int)dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == recordToBeModified.ProjectSalesTax)?.Id:
                                            (int)dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == recordToBeModified.ProjectSalesTax)?.Id;
                        x.InvoiceWithholdingTaxId = !string.IsNullOrEmpty(recordToBeModified.ProjectWithHoldingTax) ?
                                                    (dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ? //Added for D-1164
                                                    dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == recordToBeModified.ProjectWithHoldingTax)?.Id : 
                                                    dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyTax)?.FirstOrDefault(x2 => x2?.Name == recordToBeModified.ProjectWithHoldingTax)?.Id) : null;
                        x.InvoiceCurrency = recordToBeModified.ProjectInvoicingCurrency;
                        x.InvoiceGrouping = recordToBeModified.ProjectInvoiceGrouping;
                        x.InvoiceRemittanceTextId = dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?  //Added for D-1164
                                                    (int)dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x1 => x1?.Identifier == recordToBeModified.ProjectInvoiceRemittanceIdentifier && x1?.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractRemittanceText))?.Id:
                                                    (int)dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x1 => x1.Company?.Code == recordToBeModified.ContractHoldingCompanyCode && x1.Identifier == recordToBeModified.ProjectInvoiceRemittanceIdentifier && x1.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractRemittanceText))?.Id;
                        x.InvoiceFooterTextId = dbContracts?.Select(x3 => x3.ContractType).FirstOrDefault() == "CHD" ?  //Added for D-1164
                                                (int)dbContracts?.ToList().SelectMany(x1 => x1.ParentContract?.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x1 => x1?.Identifier == recordToBeModified.ProjectInvoiceFooterIdentifier && x1?.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractFooterText))?.Id:
                                                (int)dbContracts?.ToList().SelectMany(x1 => x1.ContractHolderCompany?.CompanyMessage)?.FirstOrDefault(x1 => x1.Company?.Code == recordToBeModified.ContractHoldingCompanyCode && x1.Identifier == recordToBeModified.ProjectInvoiceFooterIdentifier && x1.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractFooterText))?.Id; //Changes for D1444
                        x.InvoicePaymentTermsId = dbPaymentTerms?.FirstOrDefault(x2 => x2.Name == recordToBeModified.ProjectInvoicePaymentTerms)?.Id;
                        x.IsRemittanceText = true;
                        x.ModifiedBy = x.ModifiedBy;
                        x.UpdateCount = dbProjects?.FirstOrDefault(x1 => x1.ProjectNumber == x.ProjectNumber)?.UpdateCount.CalculateUpdateCount();
                        x.LastModification = DateTime.UtcNow;
                        x.LogoId = !string.IsNullOrEmpty(recordToBeModified.LogoText) ? dbLogo?.ToList().FirstOrDefault(x1 => x1.Name == recordToBeModified.LogoText)?.Id : null;
                        x.CustomerDirectReportingEmailAddress = recordToBeModified.CustomerDirectReportingEmailAddress;
                        ProcessProjectMessages(dbProjects?.FirstOrDefault(x1 => x1.ProjectNumber == recordToBeModified.ProjectNumber)?.ProjectMessage.ToList(), recordToBeModified, dbProjects);
                    });

                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return dbProjects.ToList();
        }

        private List<DbModel.ProjectMessage> AssignProjectMessages(List<DbModel.ProjectMessage> dbMessages, DomainModel.Project projects)
        {
            DbModel.ProjectMessage projectMessage = null;

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.OperationalNotes && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectAssignmentOperationNotes))
                dbMessages.Add(AddDbProjectMessage(ProjectMessageType.OperationalNotes, projects?.ProjectAssignmentOperationNotes));
            else if (projectMessage != null && !string.IsNullOrEmpty(projects?.ProjectAssignmentOperationNotes) && !projectMessage.Message.Equals(projects?.ProjectAssignmentOperationNotes))
                dbMessages.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectAssignmentOperationNotes, projects?.ModifiedBy));
            else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectAssignmentOperationNotes))
                dbMessages.Remove(projectMessage);

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.InvoiceNotes && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectInvoiceInstructionNotes))
                dbMessages.Add(AddDbProjectMessage(ProjectMessageType.InvoiceNotes, projects?.ProjectInvoiceInstructionNotes));
            else if (projectMessage != null && !string.IsNullOrEmpty(projects?.ProjectInvoiceInstructionNotes) && !projectMessage.Message.Equals(projects?.ProjectInvoiceInstructionNotes))
                dbMessages.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectInvoiceInstructionNotes, projects?.ModifiedBy));
            else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectInvoiceInstructionNotes))
                dbMessages.Remove(projectMessage);

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.InvoiceFreeText && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectInvoiceFreeText))
                dbMessages.Add(AddDbProjectMessage(ProjectMessageType.InvoiceFreeText, projects?.ProjectInvoiceFreeText));
            else if (projectMessage != null && !string.IsNullOrEmpty(projects?.ProjectInvoiceFreeText) && !projectMessage.Message.Equals(projects?.ProjectInvoiceFreeText))
                dbMessages.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectInvoiceFreeText, projects?.ModifiedBy));
            else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectInvoiceFreeText))
                dbMessages.Remove(projectMessage);

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.ReportingRequirements && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectClientReportingRequirement))
                dbMessages.Add(AddDbProjectMessage(ProjectMessageType.ReportingRequirements, projects?.ProjectClientReportingRequirement));
            else if (projectMessage != null && !string.IsNullOrEmpty(projects?.ProjectClientReportingRequirement) && !projectMessage.Message.Equals(projects?.ProjectClientReportingRequirement))
                dbMessages.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectClientReportingRequirement, projects?.ModifiedBy));
            else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectClientReportingRequirement))
                dbMessages.Remove(projectMessage);

            return dbMessages;
        }

        private void ProcessProjectMessages(List<DbModel.ProjectMessage> dbMessages, DomainModel.Project projects, IList<DbModel.Project> dbProjects)
        {
            DbModel.ProjectMessage projectMessage = null;

            _projectMessageRepository.AutoSave = false;

            List<DbModel.ProjectMessage> dbMessagesToAdd = new List<DbModel.ProjectMessage>();
            List<DbModel.ProjectMessage> dbMessagesToUpdate = new List<DbModel.ProjectMessage>();
            List<DbModel.ProjectMessage> dbMessagesToDelete = new List<DbModel.ProjectMessage>();

            int projectId = dbProjects.FirstOrDefault(x => x.ProjectNumber == projects.ProjectNumber).Id;

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.OperationalNotes && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectAssignmentOperationNotes))
                dbMessagesToAdd.Add(AddDbProjectMessage(ProjectMessageType.OperationalNotes, projects?.ProjectAssignmentOperationNotes, projectId));
            else if (projectMessage != null && (string.IsNullOrEmpty(projectMessage.Message) || !projectMessage.Message.Equals(projects?.ProjectAssignmentOperationNotes))) //HotFix id 26
                dbMessagesToUpdate.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectAssignmentOperationNotes, projects?.ModifiedBy));
            //else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectAssignmentOperationNotes))
            //    dbMessagesToDelete.Add(projectMessage);

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.InvoiceNotes && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectInvoiceInstructionNotes))
                dbMessagesToAdd.Add(AddDbProjectMessage(ProjectMessageType.InvoiceNotes, projects?.ProjectInvoiceInstructionNotes, projectId));
            else if (projectMessage != null && (string.IsNullOrEmpty(projectMessage.Message) || !projectMessage.Message.Equals(projects?.ProjectInvoiceInstructionNotes))) //HotFix id 26
                dbMessagesToUpdate.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectInvoiceInstructionNotes, projects?.ModifiedBy));
            //else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectInvoiceInstructionNotes))
            //    dbMessagesToDelete.Add(projectMessage);

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.InvoiceFreeText && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectInvoiceFreeText))
                dbMessagesToAdd.Add(AddDbProjectMessage(ProjectMessageType.InvoiceFreeText, projects?.ProjectInvoiceFreeText, projectId));
            else if (projectMessage != null && (string.IsNullOrEmpty(projectMessage.Message) || !projectMessage.Message.Equals(projects?.ProjectInvoiceFreeText))) //HotFix id 26
                dbMessagesToUpdate.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectInvoiceFreeText, projects?.ModifiedBy));
            //else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectInvoiceFreeText))
            //    dbMessagesToDelete.Add(projectMessage);

            projectMessage = dbMessages?.ToList().FirstOrDefault(x => x.MessageTypeId == (int)ProjectMessageType.ReportingRequirements && x.ProjectId == projects.ProjectNumber);
            if (projectMessage == null && !string.IsNullOrEmpty(projects?.ProjectClientReportingRequirement))
                dbMessagesToAdd.Add(AddDbProjectMessage(ProjectMessageType.ReportingRequirements, projects?.ProjectClientReportingRequirement, projectId)); // ProjectId Added for on defect 117(Additional Failure issue)
            else if (projectMessage != null && (string.IsNullOrEmpty(projectMessage.Message) || !projectMessage.Message.Equals(projects?.ProjectClientReportingRequirement))) //HotFix id 26
                dbMessagesToUpdate.Add(UpdateDbProjectMessage(projectMessage, projects?.ProjectClientReportingRequirement, projects?.ModifiedBy));
            //else if (projectMessage != null && string.IsNullOrEmpty(projects?.ProjectClientReportingRequirement))
            //    dbMessagesToDelete.Add(projectMessage);

            if (dbMessagesToAdd.Count > 0)
                _projectMessageRepository.Add(dbMessagesToAdd);

            if (dbMessagesToUpdate.Count > 0)
                _projectMessageRepository.Update(dbMessagesToUpdate);

            if (dbMessagesToDelete.Count > 0)
                _projectMessageRepository.Delete(dbMessagesToDelete);

        }

        private DbModel.ProjectMessage AddDbProjectMessage(ProjectMessageType projectMessageType, string msgText)
        {
            var dbMessage = new DbModel.ProjectMessage()
            {
                Message = msgText,
                MessageTypeId = (int)projectMessageType
            };

            return dbMessage;
        }

        private DbModel.ProjectMessage AddDbProjectMessage(ProjectMessageType projectMessageType, string msgText, int projectId)
        {
            var dbMessage = new DbModel.ProjectMessage()
            {
                ProjectId = projectId,
                Message = msgText,
                MessageTypeId = (int)projectMessageType
            };

            return dbMessage;
        }

        private DbModel.ProjectMessage UpdateDbProjectMessage(DbModel.ProjectMessage dbMessage, string msgText,
                                        string modifyBy = null)
        {

            dbMessage.Message = msgText.IsEmptyReturnNull();
            dbMessage.LastModification = DateTime.UtcNow;
            dbMessage.ModifiedBy = modifyBy;
            return dbMessage;
        }

        private void UpdateProjectNumber(List<DbModel.Project> dbProjecttNumberToBeUpdated)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            _projectRepository.AutoSave = true;
            try
            {
                dbProjecttNumberToBeUpdated?.ToList().ForEach(x =>
                {
                    x.ProjectNumber = x.Id;
                    _projectRepository.Update(x, p => p.ProjectNumber);
                });

                // _projectRepository.Update(dbProjecttNumberToBeUpdated);
            }
            catch (Exception ex)
            {
                string errorCode = MessageType.ProjectNumberNotGenerated.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), ex.Message)));
            }
        }

        private bool IsRecordValidForProcess(IList<DomainModel.Project> projects, ValidationType validationType, ref IList<DomainModel.Project> filteredProjects, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {

            if (validationType == ValidationType.Add)
                filteredProjects = projects.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            if (validationType == ValidationType.Update)
                filteredProjects = projects.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            if (validationType == ValidationType.Delete)
                filteredProjects = projects.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();


            //return filteredProjects.Count <= 0;
            if (filteredProjects.Count <= 0)
                return false;
            return IsProjectHasValidSchema(filteredProjects, validationType, ref errorMessages, ref validationMessages);

        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Project> projects, IList<DbModel.Project> dbProjects, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projects.Where(x => !dbProjects.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.ProjectNumber == x.ProjectNumber)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectUpdatedByOtherUser.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsProjectHasValidSchema(IList<DomainModel.Project> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(models), validationType);
            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Project, x.Code, x.Message) }));
            });

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;

        }

        private bool IsValidContract(IList<DomainModel.Project> projects, ref IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contractNumber = projects.Select(x => x.ContractNumber).ToList();

            var dbContractList = _contractRepository.FindBy(x => contractNumber.Contains(x.ContractNumber),
                    new string[] {
                        "ContractHolderCompany.CompanyDivision.Division",
                        "ContractHolderCompany.UserRole.User",
                        "Customer.CustomerAddress",
                        "Customer.CustomerAddress.CustomerContact", }).ToList();

            var contractNumberNotExists = projects.Where(x => !dbContractList.Any(x1 => x1.ContractNumber == x.ContractNumber)).ToList();
            contractNumberNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectContractNumberNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));
            });

            dbContracts = dbContractList;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCustomerContact(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Contract> dbContract = dbContracts;

            var customerContacts = dbContracts?.ToList().SelectMany(x => x.Customer.CustomerAddress).SelectMany(x1 => x1.CustomerContact).ToList();
            var customerContactsNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectCustomerContact))?.Where(x1 => !customerContacts.Any(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x1.ProjectCustomerContact.Where(c => !char.IsWhiteSpace(c)))))?.ToList();
            var customerInvoiceContactsNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectCustomerInvoiceContact))?.Where(x1 => !customerContacts.Any(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x1.ProjectCustomerInvoiceContact.Where(c => !char.IsWhiteSpace(c)))))?.ToList();

            customerContactsNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.Customer_Contact_Invalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectCustomerContact)));
            });

            customerInvoiceContactsNotExists.ForEach(x =>
            {
                string errorCode = MessageType.Customer_InvoiceContact_Invalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectCustomerInvoiceContact)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCustomerAddress(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var customerAddresses = dbContracts?.ToList().SelectMany(x => x.Customer.CustomerAddress)?.ToList();

            var customerAddressesNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectCustomerContactAddress))?
                                                       .Where(x1 => !customerAddresses.Any(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                        == String.Join("", x1.ProjectCustomerContactAddress.Where(c => !char.IsWhiteSpace(c)))))?.ToList();

            var customerInvoiceAddressesNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectCustomerInvoiceAddress))?
                                                    .Where(x1 => !customerAddresses.Any(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                     == String.Join("", x1.ProjectCustomerInvoiceAddress.Where(c => !char.IsWhiteSpace(c)))))?.ToList();

            customerAddressesNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.CustAddr_Address.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectCustomerContactAddress)));
            });

            customerInvoiceAddressesNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.CustAddr_Invoice_Address.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectCustomerInvoiceAddress)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidInvoicePaymentTerm(IList<DomainModel.Project> projects, ref IList<DbModel.Data> dbPaymentTerms, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Data> dbInvoicePaymentTerms = null;

            var invoicePaymentTerms = projects?.Select(x => x.ProjectInvoicePaymentTerms)?.ToList();
            if (dbMasterData?.Count > 0)
                dbInvoicePaymentTerms = dbMasterData?.Where(x => invoicePaymentTerms.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt16(MasterType.InvoicePaymentTerms.ToId()) && x.IsActive == true)?.ToList();
            else
                dbInvoicePaymentTerms = _dataRepository?.FindBy(x => invoicePaymentTerms.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt16(MasterType.InvoicePaymentTerms.ToId()) && x.IsActive == true)?.ToList();

            var invoicePaymentTermsNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectInvoicePaymentTerms))?.Where(x1 => !dbInvoicePaymentTerms.Any(x2 => x2.Name == x1.ProjectInvoicePaymentTerms))?.ToList();
            invoicePaymentTermsNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractInvoicePaymentTerm.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectInvoicePaymentTerms)));
            });

            dbPaymentTerms = dbInvoicePaymentTerms;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyDivision(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ref IList<DbModel.CompanyDivision> dbCompanyDivisions, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var companyDivision = projects?.Select(x => x.CompanyDivision).ToList();

            var dbContractCompanyDivisions = dbContracts.SelectMany(x => x.ContractHolderCompany.CompanyDivision)?.Where(x => companyDivision.Contains(x.Division.Name)).ToList();

            var companyDivisionNotExists = projects.Where(x => !dbContractCompanyDivisions.Any(x1 => x1.Division.Name == x.CompanyDivision)).ToList();
            companyDivisionNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectCompanyDivisionNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyDivision)));
            });

            dbCompanyDivisions = dbContractCompanyDivisions;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyDivisionCostCenters(IList<DomainModel.Project> projects, IList<DbModel.CompanyDivision> dbCompanyDivisions, ref IList<DbModel.CompanyDivisionCostCenter> dbCompanyDivisionCostCenters, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var companyDivisionCostCenter = projects?.Select(x => new { x.CompanyDivision, x.CompanyCostCenterCode }).ToList();

            var dbContractCompanyDivisionCostCenters = dbCompanyDivisions.Where(x => companyDivisionCostCenter.Any(x1 => x1.CompanyDivision == x.Division.Name
                                                                        && x.CompanyDivisionCostCenter.Any(x2 => x2.Code == x1.CompanyCostCenterCode))).SelectMany(x => x.CompanyDivisionCostCenter).ToList();

            var companyDivisionCostCentersNotExists = projects.Where(x => !dbContractCompanyDivisionCostCenters.Any(x1 => x1.Code == x.CompanyCostCenterCode)).ToList();
            companyDivisionCostCentersNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectCompanyDivisionCostCenterNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyCostCenterName)));
            });

            dbCompanyDivisionCostCenters = dbContractCompanyDivisionCostCenters;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyOffice(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ref IList<DbModel.CompanyOffice> dbCompanyOffices, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            var companyOfficesInfo = projects?.Select(x => new { x.CompanyOffice, x.ContractHoldingCompanyCode });

            var dbCompOffice = dbContracts.SelectMany(x => x.ContractHolderCompany.CompanyOffice).Where(x => companyOfficesInfo.Any(x1 => x1.ContractHoldingCompanyCode == x.Company.Code && x1.CompanyOffice == x.OfficeName)).ToList();

            var projectCompanyOfficeNameNotExists = projects.Where(x => !dbCompOffice.Any(x1 => x1.OfficeName == x.CompanyOffice)).ToList();

            projectCompanyOfficeNameNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectCompanyOfficeNameNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyOffice)));
            });

            dbCompanyOffices = dbCompOffice;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidProjectType(IList<DomainModel.Project> projects, ref IList<DbRepository.Models.SqlDatabaseContext.Data> dbProjectTypes, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            IList<DbModel.Data> dbProjectType = null;
            var projectType = projects.Select(x => x.ProjectType).ToList();
            if (dbMasterData?.Count > 0)
                dbProjectType = dbMasterData?.Where(x => projectType.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt16(MasterType.ProjectType.ToId()))?.ToList();
            else
                dbProjectType = _dataRepository?.FindBy(x => projectType.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt16(MasterType.ProjectType.ToId()))?.ToList();

            var projectTypeNotExists = projects.Where(x => !dbProjectType.Any(x1 => x1.Name == x.ProjectType)).ToList();

            projectTypeNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectTypeNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectType)));
            });

            dbProjectTypes = dbProjectType;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidIndustrySector(IList<DomainModel.Project> projects, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            IList<DbModel.Data> dbIndustrySectors = null;
            var industrySector = projects.Select(x => x.IndustrySector).ToList();
            if (dbMasterData?.Count > 0)
                dbIndustrySectors = dbMasterData.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.IndustrySector) &&
                                                                industrySector.Contains(x.Name)).ToList();
            else
                dbIndustrySectors = _dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.IndustrySector) &&
                                                                    industrySector.Contains(x.Name)).ToList();

            var projectIndustrySectorNotExists = projects.Where(x => !dbIndustrySectors.Any(x1 => x1.Name == x.IndustrySector)).ToList();

            projectIndustrySectorNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectIndustrySectorNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.IndustrySector)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidTax(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var companyTax = dbContracts?.SelectMany(x => x.ContractHolderCompany.CompanyTax)?.ToList();
            if (dbContracts?.Select(x => x.ContractType).FirstOrDefault() == "CHD")//Added for D-1164
            {
                var ParentContractHolderCompany = dbContracts?.Select(x => x.ParentContract.ContractHolderCompany)?.ToList();
                companyTax = ParentContractHolderCompany?.SelectMany(x => x.CompanyTax)?.ToList();
            }
            var salesTaxNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectSalesTax))?.Where(x1 => !companyTax.Any(x2 => x2.Name == x1.ProjectSalesTax && x2.TaxType.IsSalesTax()))?.ToList();

            var withHoldingTaxNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectWithHoldingTax))?.Where(x1 => !companyTax.Any(x2 => x2.Name == x1.ProjectWithHoldingTax && x2.TaxType.IsWithholdingTax()))?.ToList();

            withHoldingTaxNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.TaxWithholdingInvalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectWithHoldingTax)));
            });

            salesTaxNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.TaxSalesInvalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectSalesTax)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidIdentifier(IList<DomainModel.Project> projects, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var companyMessages = dbContracts?.SelectMany(x => x.ContractHolderCompany.CompanyMessage)?.ToList();
            if (dbContracts?.Select(x=>x.ContractType).FirstOrDefault() == "CHD")//Added for D-1164
            {
                var ParentContractHolderCompany = dbContracts?.Select(x => x.ParentContract.ContractHolderCompany)?.ToList();
                companyMessages = ParentContractHolderCompany?.SelectMany(x => x.CompanyMessage)?.ToList();
            }
            var remittanceNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectInvoiceRemittanceIdentifier))?.Where(x1 => !companyMessages.Any(x2 => x2.Identifier == x1.ProjectInvoiceRemittanceIdentifier))?.ToList();

            var footerNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectInvoiceFooterIdentifier))?.Where(x1 => !companyMessages.Any(x2 => x2.Identifier == x1.ProjectInvoiceFooterIdentifier))?.ToList();

            remittanceNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractRemittance.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectInvoiceRemittanceIdentifier)));
            });

            footerNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractFooter.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectInvoiceFooterIdentifier)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidUser(IList<DomainModel.Project> projects, ref IList<DbModel.User> dbCoordinator, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var coordinator = dbContracts?.SelectMany(x => x.ContractHolderCompany.UserRole)?.ToList();
            var managedServiceCoordinatorNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ManagedServiceCoordinatorCode))?.Where(x1 => !coordinator.Any(x2 => x2.User.SamaccountName == x1.ManagedServiceCoordinatorCode //IGO QC D-900 Issue 1
                                                          && x2.Company.Code == x1.ContractHoldingCompanyCode))?.ToList();

            var projectCoordinatorNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectCoordinatorCode))?.Where(x1 => !coordinator.Any(x2 => x2.User.SamaccountName == x1.ProjectCoordinatorCode //IGO QC D-900 Issue 1
                                                        && x2.Company.Code == x1.ContractHoldingCompanyCode))?.ToList();

            managedServiceCoordinatorNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectManagedCoordinator.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ManagedServiceCoordinatorName)));
            });

            projectCoordinatorNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectCoordinator.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectCoordinatorName)));
            });
            dbCoordinator = coordinator?.Select(x => x.User)?.ToList();
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCurrency(IList<DomainModel.Project> projects, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Data> dbCurrencies = null;

            var currencies = projects?.Select(x => x.ProjectInvoicingCurrency)?.ToList();
            if (dbMasterData?.Count > 0)
                dbCurrencies = dbMasterData?.Where(x => currencies.Contains(x.Code) && x.MasterDataTypeId == Convert.ToInt16(MasterType.Currency.ToId()))?.ToList();
            else
                dbCurrencies = _dataRepository?.FindBy(x => currencies.Contains(x.Code) && x.MasterDataTypeId == Convert.ToInt16(MasterType.Currency.ToId()))?.ToList();

            var currencyNotExists = projects?.Where(x => !string.IsNullOrEmpty(x.ProjectInvoicingCurrency))?.Where(x1 => !dbCurrencies.Any(x2 => x2.Code == x1.ProjectInvoicingCurrency))?.ToList();
            currencyNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectInvoicingCurrency.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectInvoicingCurrency)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private void IsValidMasterData(IList<DomainModel.Project> projects, ref IList<DbModel.Data> dbMasterData)
        {
            var projectReferences = projects?.Select(x => x.ProjectReferences).Where(x => x?.Count > 0).SelectMany(x => x).ToList();
            var masterData = new List<string>[] { projects?.Where(x => !string.IsNullOrEmpty(x.LogoText))?.Select(x1 => x1.LogoText)?.ToList(),
                                                  projects.Select(x => x.IndustrySector).ToList(),
                                                  projects.Select(x => x.ProjectType).ToList(),
                                                  projects?.Select(x => x.ProjectInvoicePaymentTerms)?.ToList(),
                                                  projects?.Select(x => x.ManagedServiceTypeName)?.ToList(),
                                                  projectReferences}?
                                                  .Where(x => x?.Count > 0).SelectMany(x => x).ToList();

            var masterCodes = projects?.Select(x => x.ProjectInvoicingCurrency)?.ToList();

            var masterTypeId = new List<int>() { (int)MasterType.ExpenseType, (int)MasterType.Logo, (int)MasterType.Currency,(int)MasterType.AssignmentReferenceType,
                                                       (int)MasterType.IndustrySector, (int)MasterType.ProjectType, (int)MasterType.InvoicePaymentTerms,(int)MasterType.ManagedServicesType };


            dbMasterData = _dataRepository.FindBy(x => masterTypeId.Contains(x.MasterDataTypeId) && (masterData.Contains(x.Name) || masterCodes.Contains(x.Code)))?.AsNoTracking()
                                           .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type, IsActive = x.IsActive }).AsNoTracking().ToList();
        }

        private bool IsValidProjectNumber(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProjects, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var projectNumber = projects?.Select(x => x.ProjectNumber)?.ToList();

            var dbProjectNumber = _projectRepository?.FindBy(x => projectNumber.Contains(x.ProjectNumber))?.ToList();

            var projectNotExists = projects?.Where(x => !dbProjectNumber.Any(x2 => x2.ProjectNumber == x.ProjectNumber))?.ToList();
            projectNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectNumber.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));
            });
            dbProjects = dbProjectNumber;
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidLogo(IList<DomainModel.Project> projects, ref IList<DbModel.Data> dbLogo, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            IList<DbModel.Data> dbLogoText = null;
            var logoText = projects?.Where(x => !string.IsNullOrEmpty(x.LogoText))?.Select(x1 => x1.LogoText)?.ToList();
            if (logoText.Count > 0)
            {
                if (dbMasterData?.Count > 0)
                    dbLogoText = dbMasterData?.Where(x => logoText.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt32(MasterType.Logo.ToId()))?.ToList();
                else
                    dbLogoText = _dataRepository?.FindBy(x => logoText.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt32(MasterType.Logo.ToId()))?.ToList();
                if (dbLogoText.Count <= 0)
                {
                    string errorCode = MessageType.InvalidProjectLogoText.ToId();
                    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString())));
                }

                dbLogo = dbLogoText;
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordCanBeDeleted(IList<DomainModel.Project> projects, IList<DbModel.Project> dbProjects, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbProjects?.Where(x => x.Status.IsProjectClosed()).ToList().
            ForEach(x =>
            {
                string errorCode = MessageType.ProjectStatusClosed.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));

            });

            dbProjects.Where(x => x.Assignment?.Count > 0).ToList()
            .ForEach(x =>
            {
                string errorCode = MessageType.ProjectCannotBeDeleted.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));
            });
            //Added for D-1014 Start
            dbProjects.Where(x => x.SupplierPurchaseOrder?.Count > 0).ToList()
            .ForEach(x =>
            {
                string errorCode = MessageType.ProjectSupplierPO.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));
            });
            //Added for D-1014 End
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private Response PopulateProjectInvoiceInfo(IList<BudgetAccountItem> budgetAccountItems, IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<InvoicedInfo> result = null;
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
                        var contractWithoutExchangeRate = budgetAccountItems?
                                                            .Where(x => !contractExchangeRates
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
                           .GetExchangeRates(currencyWithoutExchangeRate)
                           .Result
                           .Populate<IList<ExchangeRate>>();
                    }


                    var masterExpenceTypeToHour = _masterService.Search(null, MasterType.ExpenseTypeHour).Result.Populate<List<MasterData>>();

                    ExchangeRateClaculations.CalculateExchangeRate(contractExchangeRates, currencyExchangeRates, budgetAccountItems);

                    result = budgetAccountItems?
                         .GroupBy(x => new { x.ContractId, x.ProjectId })
                         .Select(x =>
                         new InvoicedInfo
                         {
                             ContractId = x.Key.ContractId,
                             ProjectId = x.Key.ProjectId,
                             InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                             UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                             HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                             HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                             ContractNumber = x?.FirstOrDefault().ContractNumber,
                             ProjectNumber = x?.FirstOrDefault()?.ProjectNumber ?? 0,
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

        private bool IsValidProject(IList<DomainModel.Project> projects, ref IList<DbModel.Project> dbProjects, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var projectNumber = projects?.Where(x => x.ProjectNumber > 0)?.Select(x => x.ProjectNumber)?.ToList();
            var dbProject = _projectRepository?.FindBy(x => projectNumber.Contains(x.ProjectNumber))?.ToList();

            var projectNumberNotExists = projects?.Where(x => x.ProjectNumber > 0)?.Where(x1 => !dbProject.Any(x2 => x2.ProjectNumber == x1.ProjectNumber))?.ToList();

            projectNumberNotExists.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectNumber.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));
            });

            dbProjects = dbProject;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        #endregion
    }
}
