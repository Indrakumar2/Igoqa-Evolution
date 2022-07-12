using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
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
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Evolution.SupplierPO.Domain.Models.SupplierPO;
using Evolution.Assignment.Domain.Interfaces.Assignments;

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
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;
using Evolution.Assignment.Domain.Interfaces.Assignments;

namespace Evolution.SupplierPO.Core.Services
{
    public class SupplierPOService : ISupplierPOService
    {
        private ISupplierPORepository _repository = null;
        private IMapper _mapper = null;
        private IAppLogger<SupplierPOService> _logger = null;
        private IContractRepository _contractRepository = null;
        private IProjectRepository _projectRepository = null;
        private IProjectService _projectService = null;
        private ISupplierService _supplierService = null;
        private IMasterService _masterService = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly JObject _messageDescriptions = null;
        private ISupplierPoValidationService _validationService = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly IAppLogger<LogEventGeneration> _applogger = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private IAssignmentService _assignmentService = null;
        #region Constructor

        public SupplierPOService(ISupplierPORepository repository,
                                 IMapper mapper, IAppLogger<SupplierPOService> logger,
                                 IProjectService projectService,
                                 IContractRepository contractRepository,
                                 IProjectRepository projectRepository,
                                 IContractExchangeRateService contractExchangeRateService,
                                 ICurrencyExchangeRateService currencyExchangeRateService,
                                 IMasterService masterService,
                                 ISupplierService supplierService,
                                 ISupplierPoValidationService validationService,
                                 IMongoDocumentService mongoDocumentService,
                                 IAppLogger<LogEventGeneration> applogger,
                                 JObject messages,
                                 IAuditSearchService auditSearchService,
                                 IOptions<AppEnvVariableBaseModel> environment,
                                  IAssignmentService assignmentService)
        {
            _mongoDocumentService = mongoDocumentService;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _projectService = projectService;
            _supplierService = supplierService;
            _contractRepository = contractRepository;
            _currencyExchangeRateService = currencyExchangeRateService;
            _projectRepository = projectRepository;
            _masterService = masterService;
            _contractExchangeRateService = contractExchangeRateService;
            _validationService = validationService;
            _auditSearchService = auditSearchService;
            _applogger = applogger;
            _messageDescriptions = messages;
            _environment = environment.Value;
            _assignmentService = assignmentService;
        }
        #endregion

        #region Public Methods
        #region Get
        public async Task<Response> GetAsync(DomainModel.SupplierPOSearch searchModel)
        {
            Exception exception = null;
            Result export = null;
            IList<DomainModel.BaseSupplierPO> result = null;
            ResponseType responseType = ResponseType.Success;
            IList<string> mongoSearch = null;
            try
            {
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    {
                        var supPOIds = mongoSearch.Select(x => Convert.ToInt32(x)).Distinct().ToList();
                        searchModel.SupplierPOIds = supPOIds;
                        if (searchModel.IsExport == true)
                            export = SearchSupplierPOs(searchModel, _environment.ChunkSize);
                        else
                            result = SearchSupplierPOs(searchModel, _environment.SupplierPORecordSize)?.BaseSupplierPO;
                    }
                    else
                        result = new List<DomainModel.BaseSupplierPO>();
                }
                else
                {
                    if (searchModel.IsExport == true)
                        export = SearchSupplierPOs(searchModel, _environment.ChunkSize);
                    else
                        result = SearchSupplierPOs(searchModel, _environment.SupplierPORecordSize)?.BaseSupplierPO;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            if (searchModel.IsExport == true)
                return new Response().ToPopulate(responseType, null, null, null, export, exception, export?.BaseSupplierPO?.FirstOrDefault().TotalCount);
            else
                return new Response().ToPopulate(responseType, null, null, null, result, exception, result?.FirstOrDefault().TotalCount);
        }

        private Result SearchSupplierPOs(DomainModel.SupplierPOSearch searchModel, int fetchCount)
        {
            Result result = null;
            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
            {
                searchModel.FetchCount = fetchCount;
                result = _repository.SearchSupplierPO<DomainModel.BaseSupplierPO>(searchModel);
                tranScope.Complete();
            }
            return result;
        }

        public Response Get(DomainModel.SupplierPOSearch searchModel, AdditionalFilter filter = null)
        {
            IList<DomainModel.SupplierPO> result = null;
            Exception exception = null;
            try
            {
                if (filter != null && !filter.IsRecordCountOnly)
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                    {
                        result = this._repository.Search<DomainModel.SupplierPO>(searchModel);
                        tranScope.Complete();
                    }
                    if (result?.Count > 0 && filter?.IsInvoiceDetailRequired == true)
                    {
                        var contractNumbers = result.Select(x => x.SupplierPOContractNumber).Distinct().ToList();
                        var supplierPoId = result.Select(x => (int)x.SupplierPOId).Distinct().ToList();
                        var invoiceInfos = GetSupplierPOInvoiceInfo(contractNumbers, null, supplierPoId).Result.Populate<IList<InvoicedInfo>>();

                        result.ToList().ForEach(x =>
                        {
                            var invoiceInfo = invoiceInfos?.FirstOrDefault(x1 => x1.ContractNumber == x.SupplierPOContractNumber);

                            x.SupplierPOBudgetInvoicedToDate = invoiceInfo?.InvoicedToDate ?? 0;
                            x.SupplierPOBudgetUninvoicedToDate = invoiceInfo?.UninvoicedToDate ?? 0;
                            x.SupplierPOBudgetHoursInvoicedToDate = invoiceInfo?.HoursInvoicedToDate ?? 0;
                            x.SupplierPOBudgetHoursUnInvoicedToDate = invoiceInfo?.HoursUninvoicedToDate ?? 0;
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetSupplierPOInvoiceInfo(List<string> contractNumber = null, List<int> projectNumber = null, List<int> supplierPoId = null)
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
                if (projectNumber != null)
                {
                    contractIds.AddRange(_projectRepository.FindBy(x => x.ProjectNumber != null && projectNumber.Contains(x.ProjectNumber.Value)).Select(x => x.ContractId).Distinct().ToList());
                }

                contractIds = contractIds.Distinct().ToList();

                var supplierPOBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(contractIds: contractIds);
                supplierPOBudgetAccountItems = supplierPOBudgetAccountItems.Where(x => supplierPoId.Contains(x.SupplierPurchaseOrderId))?.ToList();

                return PopulateSupplierPOInvoiceInfo(supplierPOBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNumber);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response PopulateSupplierPOInvoiceInfo(IList<BudgetAccountItem> budgetAccountItems, IList<ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
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
                                                                            .Populate<IList<ContractExchangeRate>>();

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
                             InvoicedToDate = (decimal)x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate : 0)),
                             UninvoicedToDate = (decimal)x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate)),
                             HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
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

        public Response GetSupplierPO(DomainModel.SupplierPOSearch searchModel)
        {
            IList<DomainModel.SupplierPO> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = this._repository.SearchSupplierPO(searchModel);
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

        public Response Get(IList<int> supplierPOIds)
        {
            IList<DomainModel.SupplierPO> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DomainModel.SupplierPO>>(this.GetSupplierPOById(supplierPOIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<int> supplierPOIds,
                                         ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                         ref IList<ValidationMessage> validationMessages,
                                         params Expression<Func<DbModel.SupplierPurchaseOrder, object>>[] includes)
        {
            IList<int> supplierPOIdNotExists = null;
            return IsRecordExistInDb(supplierPOIds, ref dbSupplierPOs, ref supplierPOIdNotExists, ref validationMessages, includes);
        }

        public Response IsRecordExistInDb(IList<int> supplierPOIds,
                                          ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                          ref IList<int> supplierPOIdNotExists,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.SupplierPurchaseOrder, object>>[] includes)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbSupplierPOs == null && supplierPOIds?.Count > 0)
                    dbSupplierPOs = GetSupplierPOById(supplierPOIds, includes);

                result = IsSupplierPOExistInDb(supplierPOIds, dbSupplierPOs, ref supplierPOIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #region Add
        public Response Add(IList<DomainModel.SupplierPO> supplierPOs,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            long? eventId = null;
            return AddSupplierPO(supplierPOs, null, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChange, isValidationRequire);
        }

        public Response Add(IList<DomainModel.SupplierPO> supplierPOs,
             IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            ref IList<DbModel.Project> dbProjects,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddSupplierPO(supplierPOs, dbModule, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Modify
        public Response Modify(IList<DomainModel.SupplierPO> supplierPOs,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            long? eventId = null;
            return UpdateSupplierPO(supplierPOs, null, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChange, isValidationRequire);
        }

        public Response Modify(IList<DomainModel.SupplierPO> supplierPOs,
             IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref IList<DbModel.Project> dbProjects,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            return UpdateSupplierPO(supplierPOs, dbModule, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Delete
        public Response Delete(IList<DomainModel.SupplierPO> supplierPOs,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            long? eventId = null;
            return RemoveSupplierPOs(supplierPOs, null, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChange, isValidationRequire);
        }

        public Response Delete(IList<DomainModel.SupplierPO> supplierPOs,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref IList<DbModel.Project> dbProjects,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {

            return RemoveSupplierPOs(supplierPOs, dbModule, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChange, isValidationRequire);
        }

        #endregion

        #region Record Valid Check

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                ValidationType validationType)
        {
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            return IsRecordValidForProcess(supplierPOs, validationType, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                                ref IList<DbModel.Supplier> dbSuppliers,
                                                ref IList<DbModel.Project> dbProjects)
        {
            IList<DomainModel.SupplierPO> filteredSupplierPOs = null;
            return CheckRecordValidForProcess(supplierPOs, validationType, ref filteredSupplierPOs, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                                IList<DbModel.Supplier> dbSuppliers,
                                                IList<DbModel.Project> dbProjects)
        {
            return IsRecordValidForProcess(supplierPOs, validationType, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects);
        }
        #endregion
        #endregion

        #region Private Metods

        #region Get
        private IList<DbModel.SupplierPurchaseOrder> GetSupplierPOById(IList<int> supplierPOIds,
                                                                       params Expression<Func<DbModel.SupplierPurchaseOrder, object>>[] includes)
        {
            IList<DbModel.SupplierPurchaseOrder> dbTsInfos = null;
            if (supplierPOIds?.Count > 0)
                dbTsInfos = _repository.FindBy(x => supplierPOIds.Contains(x.Id), includes).ToList();

            return dbTsInfos;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.SupplierPO> supplierPOs,
                                         ref IList<DomainModel.SupplierPO> filteredSupplierPOs,
                                         ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                         ref IList<DbModel.Supplier> dbSuppliers,
                                         ref IList<DbModel.Project> dbProjects,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierPOs != null && supplierPOs.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierPOs == null || filteredSupplierPOs.Count <= 0)
                    filteredSupplierPOs = FilterRecord(supplierPOs, validationType);

                if (filteredSupplierPOs?.Count > 0 && IsValidPayload(filteredSupplierPOs, validationType, ref validationMessages))
                {
                    var projectNumbers = filteredSupplierPOs.Where(x => x.SupplierPOProjectNumber != null).Select(x => (int)x.SupplierPOProjectNumber).Distinct().ToList();
                    var mainSupplierIds = filteredSupplierPOs.Where(x => x.SupplierPOMainSupplierId > 0).Select(x => (int)x.SupplierPOMainSupplierId).Distinct().ToList();
                    IList<int> supplierIdNotExists = null;
                    if (_projectService.IsValidProjectNumber(projectNumbers, ref dbProjects, ref validationMessages))
                        result = Convert.ToBoolean(_supplierService.IsRecordExistInDb(mainSupplierIds,
                                                  ref dbSuppliers,
                                                  ref supplierIdNotExists,
                                                  ref validationMessages).Result);
                }
            }

            return result;
        }

        private Response AddSupplierPO(IList<DomainModel.SupplierPO> supplierPOs,
            IList<DbModel.SqlauditModule> dbModule,
                                       ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                       ref IList<DbModel.Supplier> dbSuppliers,
                                       ref IList<DbModel.Project> dbProjects,
                                       ref long? eventId,
                                       bool commitChange = true,
                                       bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0;
            IList<DbModel.SupplierPurchaseOrder> dbRecordToAdd = new List<DbModel.SupplierPurchaseOrder>();
            try
            {
                Response valdResponse = null;
                IList<DomainModel.SupplierPO> recordToBeAdd = null;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierPOs, ValidationType.Add, ref recordToBeAdd, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;

                    if (recordToBeAdd == null)
                        recordToBeAdd = supplierPOs;

                    var projects = dbProjects;
                    recordToBeAdd.ToList().ForEach(x =>
                    {
                        x.SupplierPOId = null;
                        var dbRecord = _mapper.Map<DbModel.SupplierPurchaseOrder>(x);
                        dbRecord.ProjectId = projects.FirstOrDefault(x1 => x1.ProjectNumber == x.SupplierPOProjectNumber).Id;
                        dbRecordToAdd.Add(dbRecord);
                    });

                    _repository.Add(dbRecordToAdd);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            var dbProject = dbProjects?.FirstOrDefault();
                            dbSupplierPOs = dbRecordToAdd;
                            dbSupplierPOs?.ToList().ForEach(x =>
                          recordToBeAdd?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                             "{" + AuditSelectType.Id + ":" + x.Id.ToString() + "}${" + AuditSelectType.Number + ":" + x.SupplierPonumber.ToString() + "}${" + AuditSelectType.CustomerProjectName + ":" + x.Project.CustomerProjectName + "}${" + AuditSelectType.ProjectNumber + ":" + x.Project.ProjectNumber + "}",
                                                                                             ValidationType.Add.ToAuditActionType(),
                                                                                             SqlAuditModuleType.SupplierPO,
                                                                                              null,
                                                                                             _mapper.Map<DomainModel.SupplierPO>(x),
                                                                                             dbModule
                                                                                            )));


                            eventId = eventID;
                        }
                    }
                }
                else
                    return valdResponse;

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOs);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }

        #endregion

        #region Modify
        private bool IsRecordValidForUpdate(IList<DomainModel.SupplierPO> supplierPOs,
                                            ref IList<DomainModel.SupplierPO> filteredSupplierPOs,
                                            ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                            ref IList<DbModel.Supplier> dbSuppliers,
                                            ref IList<DbModel.Project> dbProjects,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierPOs != null & supplierPOs.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierPOs == null || filteredSupplierPOs.Count <= 0)
                    filteredSupplierPOs = FilterRecord(supplierPOs, validationType);
                if (filteredSupplierPOs?.Count > 0 && IsValidPayload(filteredSupplierPOs, validationType, ref validationMessages))
                {
                    var supplierPOIds = filteredSupplierPOs.Where(x => x.SupplierPOId > 0).Select(x => (int)x.SupplierPOId).Distinct().ToList();
                    var projectNumbers = filteredSupplierPOs.Where(x => x.SupplierPOProjectNumber != null).Select(x => (int)x.SupplierPOProjectNumber).Distinct().ToList();
                    var mainSupplierIds = filteredSupplierPOs.Where(x => x.SupplierPOMainSupplierId > 0).Select(x => (int)x.SupplierPOMainSupplierId).Distinct().ToList();
                    IList<int> supplierIdNotExists = null;

                    if (dbSupplierPOs == null || dbSupplierPOs.Count <= 0)
                    {
                        dbSupplierPOs = GetSupplierPOById(supplierPOIds);
                        IList<int> supplierPOIdNotExists = null;
                        if (_projectService.IsValidProjectNumber(projectNumbers, ref dbProjects, ref validationMessages))
                            if (IsSupplierPOExistInDb(supplierPOIds, dbSupplierPOs, ref supplierPOIdNotExists, ref validationMessages))
                                if (IsRecordUpdateCountMatching(filteredSupplierPOs, dbSupplierPOs, ref validationMessages))
                                    result = Convert.ToBoolean(_supplierService.IsRecordExistInDb(mainSupplierIds,
                                                          ref dbSuppliers,
                                                          ref supplierIdNotExists,
                                                          ref validationMessages).Result);

                                   if (result && dbSupplierPOs[0]?.SupplierId != supplierPOs[0]?.SupplierPOMainSupplierId)
                                   {
                                       result = IsSupplierAlreadyAssociated(dbSupplierPOs, ref validationMessages);
                                   }
                    }
                }
            }
            return result;
        }

        //Hot Fix 668
        private bool IsSupplierAlreadyAssociated(IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs, ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var noUpdateRecords = dbSupplierPOs?.Where(a => a.Assignment.Count > 0 
            && a.Assignment.Any(b => b.AssignmentSubSupplier.Any(c => c.AssignmentSubSupplierTechnicalSpecialist
            .Any(d => d.AssignmentSubSupplierId == c.Id)
              && c.SupplierId == a.SupplierId && c.SupplierType != "S"))).ToList();

            noUpdateRecords?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x.Id, MessageType.SupplierCannotBeUpdated, x.Id);
            });

            validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response UpdateSupplierPO(IList<DomainModel.SupplierPO> supplierPOs,
            IList<DbModel.SqlauditModule> dbModule,
                                       ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                       ref IList<DbModel.Supplier> dbSuppliers,
                                       ref IList<DbModel.Project> dbProjects,
                                       ref long? eventId,
                                       bool commitChange = true,
                                       bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0;

            try
            {
                Response valdResponse = null;
                IList<DomainModel.SupplierPO> dbExistingSupplierPOs = new List<DomainModel.SupplierPO>();
                var recordToUpdate = FilterRecord(supplierPOs, ValidationType.Update);
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierPOs, ValidationType.Update, ref recordToUpdate, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects);
                eventId = supplierPOs.Select(x => x.EventId).FirstOrDefault();
                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    dbSupplierPOs.ToList().ForEach(x =>
                    {
                        dbExistingSupplierPOs.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.SupplierPO>(x)));
                    });
                    #region D995 Main supplier Modify 
                    IList<DbModel.SupplierPurchaseOrder> dbExistingSubsuppliers;
                    dbExistingSubsuppliers = dbSupplierPOs;
                    var RecordstoupdateAssSubSupplier = dbSupplierPOs?.Where(a => a.Assignment.Count > 0
                     && a.Assignment.Any(b => b.AssignmentSubSupplier.Any(c => c.SupplierId == a.SupplierId && c.SupplierType == "M" && c.IsDeleted==false))).ToList();
                    List<int> ExistingSupplierid = dbExistingSubsuppliers.Select(a => a.SupplierId).ToList();
                    int supplierPoId = dbSupplierPOs.Select(a => a.Id).FirstOrDefault();
                    if (RecordstoupdateAssSubSupplier.Count > 0)
                    {
                     if(dbSupplierPOs.Select(a => a.SupplierId ).FirstOrDefault()!=supplierPOs.Select(x => x.SupplierPOMainSupplierId ).FirstOrDefault())
                         _assignmentService.RemoveAsssubsupplier(supplierPoId, ExistingSupplierid,"M");
                    }
                    #endregion
                    dbSupplierPOs.ToList().ForEach(x =>
                    {
                        var supplierPOToModify = recordToUpdate.FirstOrDefault(x1 => x1.SupplierPOId == x.Id);
                        x.MaterialDescription = supplierPOToModify.SupplierPOMaterialDescription;
                        x.SupplierPonumber = supplierPOToModify.SupplierPONumber;
                        x.DeliveryDate = supplierPOToModify.SupplierPODeliveryDate;
                        x.CompletionDate = supplierPOToModify.SupplierPOCompletedDate;
                        x.BudgetHoursUnit = supplierPOToModify.SupplierPOBudgetHours;
                        x.SupplierId = supplierPOToModify.SupplierPOMainSupplierId; // ITK D-744
                        x.BudgetValue = supplierPOToModify.SupplierPOBudget;
                        x.BudgetWarning = supplierPOToModify.SupplierPOBudgetWarning;
                        x.BudgetHoursUnitWarning = supplierPOToModify.SupplierPOBudgetHoursWarning;
                        x.LastModification = DateTime.UtcNow;
                        x.ModifiedBy = supplierPOToModify.ModifiedBy;
                        x.Status = supplierPOToModify.SupplierPOStatus;
                        x.UpdateCount = supplierPOToModify.UpdateCount.CalculateUpdateCount();
                    });
                    _repository.AutoSave = false;
                    _repository.Update(dbSupplierPOs);
                    List<int> supplierIDList = new List<int>();
                    foreach (var supplierId in dbExistingSubsuppliers)
                    {
                        supplierIDList.Add((int)supplierId.SupplierId);
                    }
                    if (dbExistingSubsuppliers.Count > 0)
                    {
                       _assignmentService.addAssigenmentSubSupplier(supplierPoId, supplierIDList, dbSupplierPOs[0].ModifiedBy,"M");
                    }
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            var dbProject = dbProjects?.FirstOrDefault();
                            dbSupplierPOs?.ToList().ForEach(x =>
                              recordToUpdate?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                   "{" + AuditSelectType.Id + ":" + x.Id.ToString() + "}${" + AuditSelectType.Number + ":" + x.SupplierPonumber.ToString() + "}${" + AuditSelectType.CustomerProjectName + ":" + x.Project.CustomerProjectName + "}${" + AuditSelectType.ProjectNumber + ":" + x.Project.ProjectNumber + "}",
                                                                   ValidationType.Update.ToAuditActionType(),
                                                                   SqlAuditModuleType.SupplierPO,
                                                                    _mapper.Map<DomainModel.SupplierPO>(dbExistingSupplierPOs?.FirstOrDefault(x2 => x2.SupplierPOId == x1.SupplierPOId)),
                                                                  x1,
                                                                  dbModule
                                                                   )));
                            eventId = eventID;
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOs);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.SupplierPO> supplierPOs,
                                            ref IList<DomainModel.SupplierPO> filteredSupplierPOs,
                                            ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;

            if (supplierPOs != null && supplierPOs.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierPOs == null || filteredSupplierPOs.Count <= 0)
                    filteredSupplierPOs = FilterRecord(supplierPOs, validationType);

                if (filteredSupplierPOs?.Count > 0 && IsValidPayload(filteredSupplierPOs, validationType, ref validationMessages))
                {
                    var supplierPOIds = filteredSupplierPOs.Where(x => x.SupplierPOId > 0).Select(x => (int)x.SupplierPOId).Distinct().ToList();
                    if (dbSupplierPOs == null || dbSupplierPOs.Count <= 0)
                    {
                        dbSupplierPOs = GetSupplierPOById(supplierPOIds);
                        IList<int> supplierPOIdNotExists = null;
                        if (IsSupplierPOExistInDb(supplierPOIds, dbSupplierPOs, ref supplierPOIdNotExists, ref validationMessages))
                            result = IsSupplierPOCanBeRemoved(dbSupplierPOs, ref validationMessages);
                    }
                }
            }

            return result;
        }

        private Response RemoveSupplierPOs(IList<DomainModel.SupplierPO> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                                           ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                           ref IList<DbModel.Supplier> dbSuppliers,
                                           ref IList<DbModel.Project> dbProjects,
                                           ref long? eventId,
                                           bool commitChange,
                                           bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventID = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(suppliers, ValidationType.Delete, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result) && dbSupplierPOs?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbSupplierPOs);
                    if (commitChange)
                        _repository.ForceSave();

                    if (dbSupplierPOs?.Count > 0 && suppliers?.Count > 0)
                    {
                        var dbProject = dbProjects?.FirstOrDefault();

                        dbSupplierPOs?.ToList().ForEach(x => _auditSearchService.AuditLog(x, ref eventID, suppliers?.FirstOrDefault()?.ActionByUser,
                                                                    "{" + AuditSelectType.Id + ":" + x.Id.ToString() + "}${" + AuditSelectType.Number + ":" + x.SupplierPonumber.ToString() + "}${" + AuditSelectType.CustomerProjectName + ":" + x.Project.CustomerProjectName + "}${" + AuditSelectType.ProjectNumber + ":" + x.Project.ProjectNumber + "}",
                                                                    ValidationType.Delete.ToAuditActionType(),
                                                                    SqlAuditModuleType.SupplierPO,
                                                                    x,
                                                                   null
                                                                    ));
                        eventId = eventID;
                    }
                }
                else
                    return response;

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), suppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        #endregion

        #region Common
        private Response CheckRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.SupplierPO> filteredSuppliers,
                                                    ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                                    ref IList<DbModel.Supplier> dbSuppliers,
                                                    ref IList<DbModel.Project> dbProjects)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(supplierPOs, ref filteredSuppliers, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(supplierPOs, ref filteredSuppliers, ref dbSupplierPOs, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(supplierPOs, ref filteredSuppliers, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOs);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<DomainModel.SupplierPO> FilterRecord(IList<DomainModel.SupplierPO> supplierPOs, ValidationType filterType)
        {
            IList<DomainModel.SupplierPO> filteredSuppierPO = null;
            if (filterType == ValidationType.Add)
                filteredSuppierPO = supplierPOs?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredSuppierPO = supplierPOs?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredSuppierPO = supplierPOs?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            return filteredSuppierPO;
        }

        private bool IsValidPayload(IList<DomainModel.SupplierPO> supplierPOs, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(supplierPOs), validationType);
            if (validationResults?.Count > 0)
            {
                messages.Add(_messageDescriptions, ModuleType.Supplier, validationResults);
                validationMessages.AddRange(messages);
            }

            return validationMessages?.Count <= 0;
        }

        private bool IsSupplierPOExistInDb(IList<int> supplierPOIds,
                                           IList<DbModel.SupplierPurchaseOrder> dbSupplierPurchaseOrders,
                                           ref IList<int> supplierPONotExists,
                                           ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSupplierPurchaseOrders == null)
                dbSupplierPurchaseOrders = new List<DbModel.SupplierPurchaseOrder>();

            var validMessages = validationMessages;

            if (supplierPOIds?.Count > 0)
            {
                supplierPONotExists = supplierPOIds.Where(x => !dbSupplierPurchaseOrders.Select(x1 => x1.Id).Contains(x)).ToList();
                supplierPONotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.SupplierPODoesNotExist, x);
                });
            }
            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private bool IsSupplierPOCanBeRemoved(IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbSupplierPOs?.Where(x => x.Assignment.Count > 0).ToList().ForEach(x =>
           {
               messages.Add(_messageDescriptions, x.SupplierPonumber, MessageType.SupplierPOCantBeDeleted, x.SupplierPonumber);
           });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;

        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.SupplierPO> supplierPOs, IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = supplierPOs.Where(x => !dbSupplierPOs.Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.SupplierPOId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.SupplierPOAlreadyUpdated, x.SupplierPONumber);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        #endregion


        #endregion
    }
}
