using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Enums;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Validations;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Core.Services
{
    public class ContractService : IContractService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractService> _logger = null;
        private readonly IContractRepository _contractRepository = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IMasterService _masterService = null;
        private readonly IContractValidationService _validationService = null;
        private readonly IContractMessageRepository _contractMessageRepository = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public ContractService(IMapper mapper,
                                IAppLogger<ContractService> logger,
                                IContractRepository repository,
                                ICustomerRepository customerRepository,
                                ICompanyRepository companyRepository,
                                IAssignmentRepository assignmentRepository,
                                IDataRepository dataRepository,
                                IContractExchangeRateService contractExchangeRateService,
                                ICurrencyExchangeRateService currencyExchangeRateService,
                                IMasterService masterService,
                                IContractValidationService validationService,
                                IMongoDocumentService mongoDocumentService,
                                JObject messages,
                                IContractMessageRepository contractMessageRepository,
                                IAuditSearchService auditSearchService)

        {
            _mongoDocumentService = mongoDocumentService;
            _mapper = mapper;
            _logger = logger;
            _contractRepository = repository;
            _dataRepository = dataRepository;
            _companyRepository = companyRepository;
            _assignmentRepository = assignmentRepository;
            _customerRepository = customerRepository;
            _contractExchangeRateService = contractExchangeRateService;
            _masterService = masterService;
            _validationService = validationService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _messageDescriptions = messages;
            _contractMessageRepository = contractMessageRepository;
            _auditSearchService = auditSearchService;


        }

        #region Public

        public async Task<Response> GetContract(DomainModel.ContractSearch searchModel, AdditionalFilter filter = null)
        {
            IList<DomainModel.Contract> result = null;
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
                            searchModel.ContractNumbers = mongoSearch;
                            result = FetchContracts(searchModel);
                            if (result != null)
                                result = result.Where(x => mongoSearch.Contains(x.ContractNumber.Trim())).ToList();
                        }
                        else
                            result = new List<DomainModel.Contract>();
                    }
                    else
                        result = FetchContracts(searchModel);

                    if (result?.Count > 0 && filter?.IsInvoiceDetailRequired == true)
                    {
                        var contractNumbers = result?.Select(x => x.ContractNumber)?.Distinct()?.ToList();
                        var invoiceInfos = GetContractInvoiceInfo(contractNumbers)?.Result?.Populate<IList<InvoicedInfo>>();

                        result.ToList().ForEach(x =>
                        {
                            var invoiceInfo = invoiceInfos?.Where(x1 => x1.ContractNumber == x.ContractNumber || x1.ParentContractId == x.Id);
                            if (invoiceInfo != null)
                            {
                                x.ContractInvoicedToDate = invoiceInfo.Sum(x2 => x2.InvoicedToDate);
                                x.ContractUninvoicedToDate = invoiceInfo.Sum(x2 => x2.UninvoicedToDate);
                                x.ContractHoursInvoicedToDate = invoiceInfo.Sum(x2 => x2.HoursInvoicedToDate);
                                x.ContractHoursUninvoicedToDate = invoiceInfo.Sum(x2 => x2.HoursUninvoicedToDate);
                            }
                        });
                    }
                }
                else
                    count = _contractRepository.GetCount(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, count == null ? result?.Count : count);
        }

        public Response GetContractBasedOnCustomers(DomainModel.ContractSearch searchModel)
        {

            IList<DomainModel.ContractWithId> result = null;
            Exception exception = null;
            try
            {
                result = _contractRepository.FindBy(x => (string.IsNullOrEmpty(searchModel.ContractCustomerCode) || x.Customer.Code == searchModel.ContractCustomerCode))
                               .Select(x =>
                               new DomainModel.ContractWithId()
                               {
                                   ContractId = x.Id,
                                   ContractCustomerCode = x.Customer.Code,
                                   ContractCustomerName = x.Customer.Name,
                                   CustomerContractNumber = x.CustomerContractNumber,
                                   ContractNumber = x.ContractNumber,
                               }
                               ).Distinct().ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public async Task<Response> GetBaseContract(DomainModel.ContractSearch searchModel)
        {
            IList<DomainModel.BaseContract> result = null;
            Exception exception = null;
            Int32? count = null;
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
                        searchModel.ContractNumbers = mongoSearch;
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                        {
                            result = _contractRepository.SearchBaseContract(searchModel);
                            tranScope.Complete();
                        }
                        if (result != null)
                            result = result.Where(x => mongoSearch.Contains(x.ContractNumber.Trim())).ToList();
                    }
                    else
                        result = new List<DomainModel.BaseContract>();
                }
                else
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                 new TransactionOptions
                                                 {
                                                     IsolationLevel = IsolationLevel.ReadUncommitted
                                                 }))
                    {
                        result = _contractRepository.SearchBaseContract(searchModel);
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, count == null ? result?.Count : count);
        }

        private IList<DomainModel.Contract> FetchContracts(DomainModel.ContractSearch searchModel)
        {
            IList<DomainModel.Contract> result = null;
            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
            {
                result = _contractRepository.Search(searchModel);
                tranScope.Complete();
            }
            return result;
        }

        //Added on 19th Oct 2020 for Performance (As per Thiru's email)
        public Response FetchCompanyContracts(DomainModel.BaseContract searchModel)
        {
            Exception exception = null;
            IList<DomainModel.BaseContract> result = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    result = _contractRepository.FetchCompanyContract(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception,null);
        }

        public Response GetContract(string companyCode, string userName, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true)
        {
            IList<DomainModel.ContractWithId> result = null;
            Exception exception = null;
            try
            {
                result = _assignmentRepository.FindBy(x =>
                                (string.IsNullOrEmpty(companyCode) || x.ContractCompany.Code == companyCode) &&
                                (contractStatus == ContractStatus.All ||
                                    x.Project.Contract.Status == contractStatus.FirstChar()
                                ) &&
                                (x.BudgetValue > 0 || x.Project.Budget > 0 || x.Project.Contract.Budget > 0) &&
                                (//(isAssignmentOnly == false && (string.IsNullOrEmpty(userName) || x.Project.Coordinator.SamaccountName == userName)) ||
                                  (isAssignmentOnly == true && (string.IsNullOrEmpty(userName) || x.OperatingCompanyCoordinator.SamaccountName == userName) ||
                                   (string.IsNullOrEmpty(userName) || x.ContractCompanyCoordinator.SamaccountName == userName))
                                )
                                )
                               .Select(x =>
                               new DomainModel.ContractWithId()
                               {
                                   //TODO : Return All property of Contract
                                   ContractId = x.Project.Contract.Id,
                                   ContractCustomerCode = x.Project.Contract.Customer.Code,
                                   ContractCustomerName = x.Project.Contract.Customer.Name,
                                   ContractBudgetMonetaryValue = x.Project.Contract.Budget,
                                   CustomerContractNumber = x.Project.Contract.CustomerContractNumber,
                                   ContractBudgetMonetaryCurrency = x.Project.Contract.BudgetCurrency,
                                   ContractBudgetMonetaryWarning = x.Project.Contract.BudgetWarning,
                                   ContractBudgetHours = x.Project.Contract.BudgetHours,
                                   ContractBudgetHoursWarning = x.Project.Contract.BudgetHoursWarning,
                                   ContractHoldingCompanyCode = x.ContractCompany.Code //D-1351 Fix
                               }
                               ).Distinct().ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetContract(IList<string> contractNumbers)
        {
            IList<DomainModel.ContractWithId> result = null;
            Exception exception = null;
            try
            {
                result = _contractRepository.FindBy(x => contractNumbers.Contains(x.ContractNumber))
                               .Select(x =>
                               //TODO : Return Full COntract detail
                                       new DomainModel.ContractWithId()
                                       {
                                           ContractId = x.Id,
                                           ContractCustomerCode = x.Customer.Code,
                                           ContractCustomerName = x.Customer.Name,
                                           ContractBudgetMonetaryValue = x.Budget,
                                           CustomerContractNumber = x.CustomerContractNumber,
                                           ContractBudgetMonetaryCurrency = x.BudgetCurrency,
                                           ContractBudgetMonetaryWarning = x.BudgetWarning,
                                           ContractBudgetHours = x.BudgetHours,
                                           ContractBudgetHoursWarning = x.BudgetHoursWarning,
                                       }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetContractInvoiceInfo(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true)
        {
            Exception exception = null;
            try
            {
                var contractBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(companyCode, contractIds, userName, contractStatus, showMyAssignmentsOnly);
                return PopulateContractInvoiceInfo(contractBudgetAccountItems, null);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetContractInvoiceInfo(List<string> contractNumber)
        {
            Exception exception = null;
            try
            {
                var contractIds = _contractRepository.FindBy(x => contractNumber.Contains(x.ContractNumber))?.Select(x => x.Id)?.ToList();
                var contractBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(contractIds: contractIds);
                return PopulateContractInvoiceInfo(contractBudgetAccountItems, null);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNumber);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response SaveContract(IList<DomainModel.Contract> contracts, ref long? eventId, bool commitChange = true)
        {
            IList<DbModel.Data> dbContractRef = null;
            IList<DbModel.Data> dbContractExpense = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Data> dbCurrency = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return AddContracts(contracts, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChange);
        }

        public Response SaveContract(IList<DomainModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange = true)
        {
            return AddContracts(contracts, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChange);
        }

        public Response ModifyContract(IList<DomainModel.Contract> contracts, ref long? eventId, bool commitChange = true)
        {
            IList<DbModel.Data> dbContractRef = null;
            IList<DbModel.Data> dbContractExpense = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Data> dbCurrency = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateContracts(contracts, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChange);
        }

        public Response ModifyContract(IList<DomainModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange = true)
        {
            return UpdateContracts(contracts, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChange);
        }

        public Response DeleteContract(IList<DomainModel.Contract> contracts, IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange = true)
        {
            return DeleteContracts(contracts, dbModule, ref eventId, commitChange);
        }

        public Response DeleteContract(IList<DomainModel.Contract> contracts, ref long? eventId, bool commitChange = true)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            return DeleteContracts(contracts, dbModule, ref eventId, commitChange);
        }

        public Response GetApprovedVisitContracts(string customerCode, int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {

            IList<DomainModel.ContractWithId> result = null;
            Exception exception = null;
            try
            {
                result = _contractRepository.GetApprovedContractByCustomer(customerCode, ContractHolderCompanyId, isVisit, isNDT);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetUnApprovedVisitContracts(string customerCode, int? Coordinator, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {

            IList<DomainModel.ContractWithId> result = null;
            Exception exception = null;
            try
            {
                result = _contractRepository.GetUnApprovedContractByCustomer(customerCode, Coordinator, ContractHolderCompanyId, isVisit, isOperating);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetvisitTimesheetKPIContracts(string customerCode, int ContractHolderCompanyId, bool isOperating, bool isVisit)
        {
            IList<DomainModel.ContractWithId> result = null;
            Exception exception = null;
            try
            {
                result = _contractRepository.GetvisitTimesheetKPIContracts(customerCode, ContractHolderCompanyId, isOperating, isVisit);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetContractBudgetAccountItems(IList<int> contractIds, string userName, bool isAssignmentOnly = true)
        {
            IList<BudgetAccountItem> result = null;
            Exception exception = null;
            try
            {
                result = _contractRepository.GetBudgetAccountItemDetails(null, contractIds?.ToList(), userName, ContractStatus.All, isAssignmentOnly);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetContractBudgetDetails(string companyCode,
                                                    string userName,
                                                    ContractStatus contractStatus = ContractStatus.All,
                                                    bool isAssignmentOnly = true)
        {
            IList<Budget> result = null;
            Exception exception = null;
            try
            {

                var contracts = GetContract(companyCode, userName, contractStatus, isAssignmentOnly).Result.Populate<IList<DomainModel.ContractWithId>>();

                var contractIds = contracts?.Select(x => x.ContractId).Distinct().ToList();

                var contractBudgetAccountItems = _contractRepository
                                                .GetBudgetAccountItemDetails(companyCode,
                                                                                contractIds,
                                                                                userName,
                                                                                contractStatus,
                                                                                isAssignmentOnly);

                return GetContractBudgetDetails(contractBudgetAccountItems, contractWithIds: contracts);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetContractBudgetDetails(IList<BudgetAccountItem> budgetAccountItems,
                                                    IList<ContractExchangeRate> contractExchangeRates = null,
                                                    IList<DomainModel.ContractWithId> contractWithIds = null,
                                                    IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<Budget> result = null;
            Exception exception = null;
            try
            {
                if (budgetAccountItems?.Count > 0)
                {
                    if (contractWithIds?.Count < 0)
                    {
                        var contractNumber = budgetAccountItems.Select(x => x.ContractNumber).Distinct().ToList();
                        contractWithIds = this.GetContract(contractNumber).Result.Populate<IList<DomainModel.ContractWithId>>();
                    }

                    var contractInvoiceInfo = PopulateContractInvoiceInfo(budgetAccountItems, contractExchangeRates, currencyExchangeRates)
                                              .Result.Populate<List<InvoicedInfo>>();

                    result = _mapper.Map<List<InvoicedInfo>, List<Budget>>(contractInvoiceInfo);

                    result.ToList().ForEach(x =>
                    {
                        var contract = contractWithIds?.FirstOrDefault(x1 => x1.ContractId == x.ContractId);
                        x.ContractCustomerCode = contract.ContractCustomerCode;
                        x.ContractCustomerName = contract.ContractCustomerName;
                        x.BudgetValue = contract.ContractBudgetMonetaryValue;
                        x.CustomerContractNumber = contract.CustomerContractNumber;
                        x.BudgetCurrency = contract.ContractBudgetMonetaryCurrency;
                        x.BudgetWarning = contract.ContractBudgetMonetaryWarning;
                        x.BudgetHours = contract.ContractBudgetHours;
                        x.BudgetHoursWarning = contract.ContractBudgetHoursWarning;
                        x.ContractHoldingCompanyCode = contract.ContractHoldingCompanyCode; //D-1351 Fix
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

        public Response ContractValidForDeletion(IList<DomainModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts)
        {
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Contract> recordToBeDeleted = null;
            Exception exception = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (IsRecordsValidToProcess(contracts, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                {
                    if (IsValidContract(recordToBeDeleted, ref dbContracts, ref errorMessages))
                    {
                        if (IsRecordCanBeDeleted(recordToBeDeleted, dbContracts, ref errorMessages))
                        {
                            if (IsContractAssociated(recordToBeDeleted, dbContracts, ref errorMessages))
                                return new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contracts);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        #endregion

        #region Private 
        private Response AddContracts(IList<DomainModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Contract> result = null;
            long? eventID = 0;
            try
            {
                _contractRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                if (contracts?.Count > 0)
                {
                    IList<DbModel.Customer> dbCustomers = null;
                    IList<DbModel.Company> dbCompanies = null;
                    IList<DbModel.Company> dbParentCompanies = null;
                    IList<DbModel.Company> dbFrameworkParentCompanies = null;
                    IList<DbModel.Data> dbPaymentTerms = null;
                    IList<DbModel.Data> dbMasterData = null;
                    IList<DomainModel.Contract> recordToBeInserted = null;
                    bool useFixedExchangeValue = false;
                    bool IsValid = Validate(contracts, ref recordToBeInserted, ValidationType.Add, ref dbCompanies, ref dbParentCompanies, ref dbMasterData,
                                         ref dbPaymentTerms, ref dbContracts, ref dbFrameworkParentCompanies, ref dbCustomers, ref errorMessages, ref validationMessages, ref useFixedExchangeValue, ref dbModule);
                    dbContractRef = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType))?.ToList();
                    dbContractExpense = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ExpenseType))?.ToList();
                    dbCurrency = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Currency))?.ToList();
                    if (IsValid)
                    {
                        var dbContractToBeInserted = AssignContractValues(recordToBeInserted, dbCustomers, dbCompanies, dbContracts, dbPaymentTerms, dbParentCompanies, useFixedExchangeValue);
                        if (dbContractToBeInserted != null)
                            _contractRepository.Add(dbContractToBeInserted);

                        if (commitChange && !_contractRepository.AutoSave && dbContractToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                        {
                            int value = _contractRepository.ForceSave();
                            if (value > 0)
                            {
                                // if (dbCurrency?.Any() == false)
                                //if(string.IsNullOrEmpty(contracts.FirstOrDefault()?.ContractSaveOrigin)) //Commented for 30-Sep  PT run to identity transaction related errors
                                //  UpdateContractNumber(dbContractToBeInserted, dbCustomers);

                                dbContracts = dbContractToBeInserted;
                                result = _mapper.Map<IList<DomainModel.Contract>>(dbContractToBeInserted);
                                if (dbContractToBeInserted?.Count > 0 && recordToBeInserted?.Count > 0)
                                {
                                    var dbModules = dbModule;
                                    dbContractToBeInserted?.ToList()?.ForEach(x =>
                                    {
                                        x.ContractNumber = string.Format("{0}/{1}", contracts.FirstOrDefault().ContractCustomerCode.Trim(), Regex.Match(x.Id.ToString(), @"(.{4})\s*$"));
                                        recordToBeInserted?.ToList().ForEach(x1 =>
                                        {
                                            x1.ContractNumber = x.ContractNumber;
                                            x1.CreatedDate = x.CreatedDate;
                                            _auditSearchService.AuditLog(x1, ref eventID, x1?.ActionByUser,
                                                                                                                "{" + AuditSelectType.Id + ":" + x.Id + "}${" + AuditSelectType.ContractNumber + ":" + x.ContractNumber?.Trim()
                                                                                                                + "}${" + AuditSelectType.CustomerContractNumber + ":" + x.CustomerContractNumber?.Trim() + "}",
                                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                                SqlAuditModuleType.Contract,
                                                                                                                null,
                                                                                                                //_mapper.Map<DomainModel.Contract>(x),
                                                                                                                x1,
                                                                                                                dbModules);
                                        });
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
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contracts);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private void UpdateContractNumber(List<DbModel.Contract> dbContractNumberToBeUpdated, IList<DbModel.Customer> dbCustomers)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            _contractRepository.AutoSave = true;
            try
            {
                dbContractNumberToBeUpdated?.ToList().ForEach(x =>
                {
                    var customerCode = dbCustomers.ToList().FirstOrDefault(x1 => x1.Id == x.CustomerId).Code;
                    x.ContractNumber = string.Format("{0}/{1}", customerCode.Trim(), Regex.Match(x.Id.ToString(), @"(.{4})\s*$"));
                    _contractRepository.Update(x, c => c.ContractNumber);
                });
                //_contractRepository.Update(dbContractNumberToBeUpdated);
            }
            catch (Exception ex)
            {
                string errorCode = MessageType.ContractNumberNotGenerated.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), ex.Message)));
            }
        }

        private Response UpdateContracts(IList<DomainModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Contract> result = null;
            long? eventID = 0;
            try
            {
                _contractRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                if (contracts?.Count > 0)
                {
                    IList<DbModel.Customer> dbCustomers = null;
                    IList<DbModel.Company> dbCompanies = null;
                    IList<DbModel.Company> dbParentCompanies = null;
                    IList<DbModel.Company> dbFrameworkParentCompanies = null;
                    IList<DbModel.Contract> dbParentContracts = null;
                    IList<DbModel.Data> dbPaymentTerms = null;
                    IList<DomainModel.Contract> recordToBeModify = null;
                    IList<DbModel.Data> dbMasterData = null;
                    bool useFixedExchangeValue = false;
                    bool IsValid = Validate(contracts, ref recordToBeModify, ValidationType.Update, ref dbCompanies, ref dbParentCompanies, ref dbMasterData,
                                            ref dbPaymentTerms, ref dbContracts, ref dbFrameworkParentCompanies, ref dbCustomers, ref errorMessages, ref validationMessages, ref useFixedExchangeValue, ref dbModule);
                    dbContractRef = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType))?.ToList();
                    dbContractExpense = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ExpenseType))?.ToList();
                    dbCurrency = dbMasterData?.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Currency))?.ToList();

                    if (IsValid)
                    {
                        IList<DomainModel.Contract> domExistingContracts = new List<DomainModel.Contract>();
                        dbContracts?.ToList().ForEach(x =>
                        {
                            domExistingContracts.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.Contract>(x)));
                        });
                        var dbContractToBeUpdated = AssignContractValues(recordToBeModify, dbCustomers, dbCompanies, dbContracts, dbParentContracts, dbPaymentTerms, dbParentCompanies, useFixedExchangeValue);
                        _contractRepository.Update(dbContractToBeUpdated);

                        if (commitChange && recordToBeModify?.Count > 0)
                        {
                            int value = _contractRepository.ForceSave(); // D - 789
                            _contractMessageRepository.ForceSave();
                            if (value > 0)
                                result = _mapper.Map<IList<DomainModel.Contract>>(dbContractToBeUpdated);
                            else
                                result = recordToBeModify;

                            if (value > 0 && dbContractToBeUpdated?.Count > 0 && recordToBeModify?.Count > 0)
                            {
                                var dbModules = dbModule;
                                dbContractToBeUpdated?.ToList().ForEach(x =>
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                      "{" + AuditSelectType.Id + ":" + x.Id + "}${" + AuditSelectType.ContractNumber + ":" + x.ContractNumber?.Trim()
                                                                                                            + "}${" + AuditSelectType.CustomerContractNumber + ":" + x.CustomerContractNumber?.Trim() + "}",
                                                                                                      ValidationType.Update.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.Contract,
                                                                                                      _mapper.Map<DomainModel.Contract>(domExistingContracts?.FirstOrDefault(x2 => x2.ContractNumber == x1.ContractNumber)),
                                                                                                      x1,
                                                                                                      dbModules)));
                                eventId = eventID;

                            }
                        }
                        else
                            result = recordToBeModify;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contracts);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private Response DeleteContracts(IList<DomainModel.Contract> contracts, IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.Contract> result = null;
            long? eventID = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                if (contracts?.Count > 0)
                {
                    _contractRepository.AutoSave = false;
                    errorMessages = new List<MessageDetail>();
                    IList<DomainModel.Contract> recordToBeDeleted = null;
                    if (IsRecordsValidToProcess(contracts, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                    {
                        IList<DbModel.Contract> dbContracts = null;
                        if (IsValidContract(recordToBeDeleted, ref dbContracts, ref errorMessages))
                        {
                            if (IsRecordCanBeDeleted(recordToBeDeleted, dbContracts, ref errorMessages))
                            {
                                if (IsContractAssociated(recordToBeDeleted, dbContracts, ref errorMessages))
                                {
                                    int value = _contractRepository.DeleteContract(dbContracts);
                                    if (value > 0 && dbContracts?.Count > 0)
                                    {
                                        dbContracts?.ToList().ForEach(x =>
                                        contracts?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, contracts?.FirstOrDefault()?.ActionByUser,
                                                                                                       "{" + AuditSelectType.Id + ":" + x.Id + "}${" + AuditSelectType.ContractNumber + ":" + x.ContractNumber?.Trim()
                                                                                                            + "}${" + AuditSelectType.CustomerContractNumber + ":" + x.CustomerContractNumber?.Trim() + "}",
                                                                                                       ValidationType.Delete.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.Contract,
                                                                                                        x1,
                                                                                                       null,
                                                                                                       dbModule)));
                                        eventId = eventID;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contracts);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private List<DbModel.Contract> AssignContractValues(IList<DomainModel.Contract> recordToBeInserted, IList<DbModel.Customer> dbCustomers,
           IList<DbModel.Company> dbCompanies, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbPaymentTerms, IList<DbModel.Company> dbParentCompanies, bool useFixedExchangeValue)
        {
            Exception exception = null;
            List<DbModel.Contract> contracts = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    contracts = recordToBeInserted?.Select(x => new DbModel.Contract()
                    {
                        CustomerId = (int)dbCustomers?.FirstOrDefault(x1 => x1.Code == x.ContractCustomerCode)?.Id,
                        CustomerContractNumber = x.CustomerContractNumber,
                        ContractHolderCompanyId = (int)dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.Id,
                        Budget = x.ContractBudgetMonetaryValue,
                        BudgetCurrency = x.ContractBudgetMonetaryCurrency,
                        BudgetWarning = x.ContractBudgetMonetaryWarning,
                        StartDate = (DateTime)x.ContractStartDate,
                        EndDate = x.ContractEndDate,
                        IsUseFixedExchangeRates = useFixedExchangeValue,
                        Status = x.ContractStatus,
                        DefaultCustomerContractAddressId = dbCustomers?.FirstOrDefault(x1 => x1.Code == x.ContractCustomerCode)?
                                                .CustomerAddress?.FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                 == String.Join("", x.ContractCustomerContactAddress.Where(c => !char.IsWhiteSpace(c))))?.Id,

                        DefaultCustomerContractContactId = (int)dbCustomers?.FirstOrDefault(x1 => x1.Code == x.ContractCustomerCode)?.CustomerAddress?
                                                .SelectMany(x2 => x2.CustomerContact)
                                                ?.FirstOrDefault(x3 => String.Join("", x3.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x.ContractCustomerContact.Where(c => !char.IsWhiteSpace(c))))?.Id,


                        DefaultCustomerInvoiceAddressId = (int)dbCustomers?.FirstOrDefault(x1 => x1.Code == x.ContractCustomerCode)?
                                                .CustomerAddress?.FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                 == String.Join("", x.ContractCustomerInvoiceAddress.Where(c => !char.IsWhiteSpace(c))))?.Id,

                        DefaultCustomerInvoiceContactId = (int)dbCustomers?.FirstOrDefault(x1 => x1.Code == x.ContractCustomerCode)?.CustomerAddress?
                                                .SelectMany(x2 => x2.CustomerContact)
                                                ?.FirstOrDefault(x3 => String.Join("", x3.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x.ContractCustomerInvoiceContact.Where(c => !char.IsWhiteSpace(c))))?.Id,

                        InvoicingCompanyId = (x.ContractType == ContractType.CHD.ToString() ? (int)dbParentCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractInvoicingCompanyCode)?.Id :
                                                        (int)dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractInvoicingCompanyCode)?.Id),  //Changes For Sanity Defect on P/C Scenario
                        DefaultSalesTaxId = (int)dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.CompanyTax?.FirstOrDefault(x2 => x2.Name == x.ContractSalesTax)?.Id,
                        DefaultWithholdingTaxId = dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.CompanyTax?.FirstOrDefault(x2 => x2.Name == x.ContractWithHoldingTax)?.Id,
                        DefaultInvoiceCurrency = x.ContractInvoicingCurrency,
                        DefaultInvoiceGrouping = x.ContractInvoiceGrouping,
                        DefaultRemittanceTextId = dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.CompanyMessage?.FirstOrDefault(x2 => x2.Identifier == x.ContractInvoiceRemittanceIdentifier && x2.Company.Code == x.ContractHoldingCompanyCode && x2.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractRemittanceText))?.Id,
                        DefaultFooterTextId = dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.CompanyMessage?.FirstOrDefault(x2 => x2.Identifier == x.ContractInvoiceFooterIdentifier && x2.Company.Code == x.ContractHoldingCompanyCode && x2.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractFooterText))?.Id, //Changes for D1444
                        ParentContractId = dbContracts?.FirstOrDefault(x1 => x1.ContractNumber == x.ParentContractNumber)?.Id,
                        ParentContractDiscountPercentage = x.ParentContractDiscount,
                        CompanyOfficeId = dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.CompanyOffice?.FirstOrDefault(x2 => x2.OfficeName == x.ParentCompanyOffice)?.Id,
                        CreatedDate = DateTime.UtcNow,
                        IsUseInvoiceDetailsFromParentContract = x.IsParentContractInvoiceUsed,
                        InvoicePaymentTermsId = dbPaymentTerms?.FirstOrDefault(x2 => x2.Name == x.ContractInvoicePaymentTerms)?.Id,
                        IsCrmstatus = x.IsCRM,
                        Crmreference = x.ContractCRMReference,
                        Crmreason = x.ContractCRMReason,
                        FrameworkContractId = dbContracts?.FirstOrDefault(x1 => x1.ContractNumber == x.FrameworkContractNumber)?.Id,
                        FrameworkCompanyOfficeId = dbCompanies?.FirstOrDefault(x1 => x1.Code == x.ContractHoldingCompanyCode)?.CompanyOffice?.FirstOrDefault(x2 => x2.OfficeName == x.FrameworkCompanyOfficeName)?.Id,
                        BudgetHours = x.ContractBudgetHours,
                        BudgetHoursWarning = x.ContractBudgetHoursWarning,
                        IsRemittanceText = true,
                        ModifiedBy = x.ModifiedBy,
                        ContractType = x.ContractType,
                        ContractMessage = AssignContractMessages(new List<DbModel.ContractMessage>(), x)
                    }).ToList();

                    tranScope.Complete();
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contracts);
            }

            return contracts;
        }

        private List<DbModel.Contract> AssignContractValues(IList<DomainModel.Contract> recordToBeModify, IList<DbModel.Customer> dbCustomers,
          IList<DbModel.Company> dbCompanies, IList<DbModel.Contract> dbContracts, IList<DbModel.Contract> dbParentContracts, IList<DbModel.Data> dbPaymentTerms, IList<DbModel.Company> dbParentCompanies, bool useFixedExchangeValue)
        {
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    dbContracts?.ToList()?.ForEach(x =>
                        {
                            var recordToBeModified = recordToBeModify.FirstOrDefault(x1 => x1.ContractNumber == x.ContractNumber);
                            var customerContacts = dbCustomers?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractCustomerCode)?.CustomerAddress?.SelectMany(x2 => x2.CustomerContact);
                            x.CustomerContractNumber = recordToBeModified.CustomerContractNumber;
                            x.Budget = recordToBeModified.ContractBudgetMonetaryValue;
                            x.BudgetCurrency = recordToBeModified.ContractBudgetMonetaryCurrency;
                            x.BudgetWarning = recordToBeModified.ContractBudgetMonetaryWarning;
                            x.StartDate = (DateTime)recordToBeModified.ContractStartDate;
                            x.EndDate = recordToBeModified.ContractEndDate;
                            x.IsUseFixedExchangeRates = recordToBeModified.IsFixedExchangeRateUsed;
                            x.Status = recordToBeModified.ContractStatus;
                            x.DefaultCustomerContractAddressId = dbCustomers?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractCustomerCode)?
                                                            .CustomerAddress?.FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                             == String.Join("", recordToBeModified.ContractCustomerContactAddress.Where(c => !char.IsWhiteSpace(c))))?.Id;

                            x.DefaultCustomerContractContactId = (int)customerContacts?.FirstOrDefault(x1 => String.Join("", x1.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", recordToBeModified.ContractCustomerContact.Where(c => !char.IsWhiteSpace(c))))?.Id;
                            x.DefaultCustomerInvoiceAddressId = (int)dbCustomers?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractCustomerCode)?
                                                                .CustomerAddress?.FirstOrDefault(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                             == String.Join("", recordToBeModified.ContractCustomerInvoiceAddress.Where(c => !char.IsWhiteSpace(c))))?.Id;

                            x.DefaultCustomerInvoiceContactId = (int)customerContacts?.FirstOrDefault(x1 => String.Join("", x1.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", recordToBeModified.ContractCustomerInvoiceContact.Where(c => !char.IsWhiteSpace(c))))?.Id;
                            x.InvoicingCompanyId = (recordToBeModified.ContractType == ContractType.CHD.ToString() ? (int)dbParentCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractInvoicingCompanyCode)?.Id : (int)dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractInvoicingCompanyCode)?.Id); //Changes For Sanity Defect on P/C Scenario
                            x.DefaultSalesTaxId = (int)dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractHoldingCompanyCode)?.CompanyTax?.FirstOrDefault(x2 => x2.Name == recordToBeModified.ContractSalesTax)?.Id;
                            x.DefaultWithholdingTaxId = dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractHoldingCompanyCode)?.CompanyTax?.FirstOrDefault(x2 => x2.Name == recordToBeModified.ContractWithHoldingTax)?.Id;
                            x.DefaultInvoiceCurrency = recordToBeModified.ContractInvoicingCurrency;
                            x.DefaultInvoiceGrouping = recordToBeModified.ContractInvoiceGrouping;
                            x.DefaultRemittanceTextId = dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractHoldingCompanyCode)?
                                                                                                .CompanyMessage?.FirstOrDefault(x2 => x2.Identifier == recordToBeModified.ContractInvoiceRemittanceIdentifier
                                                                                                 && x2.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractRemittanceText)
                                                                                                 && x2.Company.Code == recordToBeModified.ContractHoldingCompanyCode)?.Id;//D-90                   
                            x.DefaultFooterTextId = dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractHoldingCompanyCode)?
                                                                        .CompanyMessage?.FirstOrDefault(x2 => x2.Identifier == recordToBeModified.ContractInvoiceFooterIdentifier
                                                                        && x2.Company.Code == recordToBeModified.ContractHoldingCompanyCode
                                                                        && x2.MessageTypeId == Convert.ToInt32(DefaultRemittanceFooterText.ContractFooterText))?.Id;
                            //x.ParentContractId = dbParentContracts?.FirstOrDefault(x1 => x1.ContractNumber == recordToBeModified.ParentContractNumber)?.Id;//Commented for ITK D-1145
                            x.ParentContractDiscountPercentage = recordToBeModified.ParentContractDiscount;
                            //x.CompanyOfficeId = !string.IsNullOrEmpty(recordToBeModified.ParentCompanyOffice) ? dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractHoldingCompanyCode)?.CompanyOffice?.FirstOrDefault(x2 => x2.OfficeName == recordToBeModified.ParentCompanyOffice)?.Id : null;//Commented for ITK D-1145
                            //x.CreatedDate = DateTime.UtcNow;
                            x.IsUseInvoiceDetailsFromParentContract = recordToBeModified.IsParentContractInvoiceUsed;
                            x.InvoicePaymentTermsId = dbPaymentTerms?.FirstOrDefault(x2 => x2.Name == recordToBeModified.ContractInvoicePaymentTerms)?.Id;
                            x.IsCrmstatus = recordToBeModified.IsCRM;
                            x.Crmreference = recordToBeModified.ContractCRMReference;
                            x.Crmreason = recordToBeModified.ContractCRMReason;
                            //x.FrameworkContractId = dbParentContracts?.FirstOrDefault(x1 => x1.ContractNumber == recordToBeModified.FrameworkContractNumber)?.Id;//Commented for ITK D-1145
                            //x.FrameworkCompanyOfficeId = !string.IsNullOrEmpty(recordToBeModified.FrameworkCompanyOfficeName) ? dbCompanies?.FirstOrDefault(x1 => x1.Code == recordToBeModified.ContractHoldingCompanyCode)?.CompanyOffice?.FirstOrDefault(x2 => x2.OfficeName == recordToBeModified.FrameworkCompanyOfficeName)?.Id : null;//Commented for ITK D-1145
                            x.BudgetHours = recordToBeModified.ContractBudgetHours;
                            x.BudgetHoursWarning = recordToBeModified.ContractBudgetHoursWarning;
                            x.IsRemittanceText = true;
                            x.ModifiedBy = recordToBeModified.ModifiedBy;
                            x.UpdateCount = dbContracts?.FirstOrDefault(x1 => x1.ContractNumber == x.ContractNumber)?.UpdateCount.CalculateUpdateCount();
                            x.LastModification = DateTime.UtcNow;
                            x.ContractType = recordToBeModified.ContractType;
                            x.IsUseFixedExchangeRates = useFixedExchangeValue;
                            ProcessContractMessages(dbContracts?.FirstOrDefault(x1 => x1.ContractNumber == recordToBeModified.ContractNumber)?.ContractMessage.ToList(), recordToBeModified, dbContracts);
                        });
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                var value = Transaction.Current;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return dbContracts.ToList();
        }

        private List<DbModel.ContractMessage> AssignContractMessages(List<DbModel.ContractMessage> dbMessages, DomainModel.Contract contracts)
        {
            DbModel.ContractMessage contractMessage = null;

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.OperationalNotes && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractOperationalNote))
                dbMessages.Add(AddDbContractMessage(ContractMessageType.OperationalNotes, contracts?.ContractOperationalNote));
            else if (contractMessage != null && !string.IsNullOrEmpty(contracts?.ContractOperationalNote) && !contractMessage.Message.Equals(contracts?.ContractOperationalNote))
                dbMessages.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractOperationalNote, contracts?.ModifiedBy));
            else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractOperationalNote))
                dbMessages.Remove(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.ConflictofInterest && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractConflictOfInterest))
                dbMessages.Add(AddDbContractMessage(ContractMessageType.ConflictofInterest, contracts?.ContractConflictOfInterest));
            else if (contractMessage != null && !string.IsNullOrEmpty(contracts?.ContractConflictOfInterest) && !contractMessage.Message.Equals(contracts?.ContractConflictOfInterest))
                dbMessages.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractConflictOfInterest, contracts?.ModifiedBy));
            else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractConflictOfInterest))
                dbMessages.Remove(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.DefaultInvoiceNotes && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractInvoiceInstructionNotes))
                dbMessages.Add(AddDbContractMessage(ContractMessageType.DefaultInvoiceNotes, contracts?.ContractInvoiceInstructionNotes));
            else if (contractMessage != null && !string.IsNullOrEmpty(contracts?.ContractInvoiceInstructionNotes) && !contractMessage.Message.Equals(contracts?.ContractInvoiceInstructionNotes))
                dbMessages.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractInvoiceInstructionNotes, contracts?.ModifiedBy));
            else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractInvoiceInstructionNotes))
                dbMessages.Remove(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.InvoiceFreeText && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractInvoiceFreeText))
                dbMessages.Add(AddDbContractMessage(ContractMessageType.InvoiceFreeText, contracts?.ContractInvoiceFreeText));
            else if (contractMessage != null && !string.IsNullOrEmpty(contracts?.ContractInvoiceFreeText) && !contractMessage.Message.Equals(contracts?.ContractInvoiceFreeText))
                dbMessages.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractInvoiceFreeText, contracts?.ModifiedBy));
            else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractInvoiceFreeText))
                dbMessages.Remove(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.ReportingRequirements && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractClientReportingRequirement))
                dbMessages.Add(AddDbContractMessage(ContractMessageType.ReportingRequirements, contracts?.ContractClientReportingRequirement));
            else if (contractMessage != null && !string.IsNullOrEmpty(contracts?.ContractClientReportingRequirement) && !contractMessage.Message.Equals(contracts?.ContractClientReportingRequirement))
                dbMessages.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractClientReportingRequirement, contracts?.ModifiedBy));
            else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractClientReportingRequirement))
                dbMessages.Remove(contractMessage);

            return dbMessages;
        }

        private void ProcessContractMessages(List<DbModel.ContractMessage> dbMessages, DomainModel.Contract contracts, IList<DbModel.Contract> dbContract)
        {

            _contractMessageRepository.AutoSave = false;
            DbModel.ContractMessage contractMessage = null;
            List<DbModel.ContractMessage> dbMessagesToAdd = new List<DbModel.ContractMessage>();
            List<DbModel.ContractMessage> dbMessagesToUpdate = new List<DbModel.ContractMessage>();
            List<DbModel.ContractMessage> dbMessagesToDelete = new List<DbModel.ContractMessage>();

            int contractId = dbContract.FirstOrDefault(x => x.ContractNumber == contracts.ContractNumber).Id;


            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.OperationalNotes && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractOperationalNote))
                dbMessagesToAdd.Add(AddDbContractMessage(ContractMessageType.OperationalNotes, contracts?.ContractOperationalNote, contractId));
            else if (contractMessage != null && (string.IsNullOrEmpty(contractMessage.Message) || !contractMessage.Message.Equals(contracts?.ContractOperationalNote))) //Hot Fix 26
                dbMessagesToUpdate.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractOperationalNote, contracts?.ModifiedBy));
            //else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractOperationalNote))
            //    dbMessagesToDelete.Add(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.ConflictofInterest && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractConflictOfInterest))
                dbMessagesToAdd.Add(AddDbContractMessage(ContractMessageType.ConflictofInterest, contracts?.ContractConflictOfInterest, contractId));
            else if (contractMessage != null && (string.IsNullOrEmpty(contractMessage.Message) || !contractMessage.Message.Equals(contracts?.ContractConflictOfInterest))) //Hot Fix 26
                dbMessagesToUpdate.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractConflictOfInterest, contracts?.ModifiedBy));
            //else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractConflictOfInterest))
            //    dbMessagesToDelete.Add(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.DefaultInvoiceNotes && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractInvoiceInstructionNotes))
                dbMessagesToAdd.Add(AddDbContractMessage(ContractMessageType.DefaultInvoiceNotes, contracts?.ContractInvoiceInstructionNotes, contractId));
            else if (contractMessage != null && (string.IsNullOrEmpty(contractMessage.Message) || !contractMessage.Message.Equals(contracts?.ContractInvoiceInstructionNotes))) //Hot Fix 26
                dbMessagesToUpdate.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractInvoiceInstructionNotes, contracts?.ModifiedBy));
            //else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractInvoiceInstructionNotes))
            //    dbMessagesToDelete.Add(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.InvoiceFreeText && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractInvoiceFreeText))
                dbMessagesToAdd.Add(AddDbContractMessage(ContractMessageType.InvoiceFreeText, contracts?.ContractInvoiceFreeText, contractId));
            else if (contractMessage != null && (string.IsNullOrEmpty(contractMessage.Message) || !contractMessage.Message.Equals(contracts?.ContractInvoiceFreeText))) //Hot Fix 26
                dbMessagesToUpdate.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractInvoiceFreeText, contracts?.ModifiedBy));
            //else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractInvoiceFreeText))
            //    dbMessagesToDelete.Add(contractMessage);

            contractMessage = dbMessages?.FirstOrDefault(x => x.MessageTypeId == (int)ContractMessageType.ReportingRequirements && x.Contract.ContractHolderCompany.Code == contracts.ContractHoldingCompanyCode && x.Contract.Customer.Code == contracts.ContractCustomerCode);
            if (contractMessage == null && !string.IsNullOrEmpty(contracts?.ContractClientReportingRequirement))
                dbMessagesToAdd.Add(AddDbContractMessage(ContractMessageType.ReportingRequirements, contracts?.ContractClientReportingRequirement, contractId));
            else if (contractMessage != null && (string.IsNullOrEmpty(contractMessage.Message) || !contractMessage.Message.Equals(contracts?.ContractClientReportingRequirement))) //Hot Fix 26
                dbMessagesToUpdate.Add(UpdateDbContractMessage(contractMessage, contracts?.ContractClientReportingRequirement, contracts?.ModifiedBy));
            //else if (contractMessage != null && string.IsNullOrEmpty(contracts?.ContractClientReportingRequirement))
            //    dbMessagesToDelete.Add(contractMessage);

            if (dbMessagesToAdd.Count > 0)
                _contractMessageRepository.Add(dbMessagesToAdd);

            if (dbMessagesToUpdate.Count > 0)
                _contractMessageRepository.Update(dbMessagesToUpdate);

            if (dbMessagesToDelete.Count > 0)
                _contractMessageRepository.Delete(dbMessagesToDelete);

        }

        private DbModel.ContractMessage AddDbContractMessage(ContractMessageType contractMessageType, string msgText)
        {
            var dbMessage = new DbModel.ContractMessage()
            {
                Message = msgText,
                MessageTypeId = (int)contractMessageType
            };

            return dbMessage;
        }


        private DbModel.ContractMessage AddDbContractMessage(ContractMessageType contractMessageType, string msgText, int contractId)
        {
            var dbMessage = new DbModel.ContractMessage()
            {
                ContractId = contractId,
                Message = msgText,
                MessageTypeId = (int)contractMessageType
            };

            return dbMessage;
        }

        private DbModel.ContractMessage UpdateDbContractMessage(DbModel.ContractMessage dbMessage, string msgText,
                                        string modifyBy = null)
        {
            dbMessage.Message = msgText.IsEmptyReturnNull();
            dbMessage.LastModification = DateTime.UtcNow;
            dbMessage.ModifiedBy = modifyBy;
            return dbMessage;
        }
        private bool ContractProjectBudgetValidation(IList<DomainModel.Contract> contracts, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            string budgetCode = _contractRepository.ValidateProjectBudget(contracts.FirstOrDefault());
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
        private bool Validate(IList<DomainModel.Contract> contracts, ref IList<DomainModel.Contract> recordTobeProcessed, ValidationType validationType,
                            ref IList<DbModel.Company> dbCompanies, ref IList<DbModel.Company> dbParentCompanies, ref IList<DbModel.Data> dbMasterData,
                            ref IList<DbModel.Data> dbPaymentTerms, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Company> dbFrameworkParentCompanies,
                            ref IList<DbModel.Customer> dbCustomers, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages,
                            ref bool useFixedExchangeValue, ref IList<DbModel.SqlauditModule> dbModule)
        {
            using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                IsValidMasterData(contracts, ref dbMasterData);
                if (validationType == ValidationType.Add)
                    useFixedExchangeValue = _contractRepository.CheckUseExchange(contracts?.FirstOrDefault(), null, validationType);
                if (IsRecordsValidToProcess(contracts, validationType, ref recordTobeProcessed, ref errorMessages, ref validationMessages))
                {
                    if (IsValidCompany(recordTobeProcessed, ref dbCompanies, ref dbParentCompanies, ref dbFrameworkParentCompanies, ref errorMessages))
                    {
                        if (IsValidCompanyOffice(recordTobeProcessed, dbParentCompanies, dbFrameworkParentCompanies, ref errorMessages))
                        {
                            if (IsValidCustomer(recordTobeProcessed, ref dbCustomers, ref errorMessages))
                            {
                                if (IsValidCustomerAddress(recordTobeProcessed, ref dbCustomers, ref errorMessages))
                                {
                                    if (IsValidCustomerContact(recordTobeProcessed, ref dbCustomers, ref errorMessages))
                                    {
                                        if (IsValidCustomerContractNumber(recordTobeProcessed, validationType, ref errorMessages))
                                        {
                                            if (IsValidContract(recordTobeProcessed, dbParentCompanies, dbFrameworkParentCompanies, ref dbCompanies, ref dbContracts, ref dbCustomers, ref errorMessages))
                                            {
                                                if (IsValidInvoicePaymentTerm(recordTobeProcessed, ref dbPaymentTerms, dbMasterData, ref errorMessages))
                                                {
                                                    if (IsValidTax(recordTobeProcessed, ref dbCompanies, ref errorMessages))
                                                    {
                                                        if (IsValidIdentifier(recordTobeProcessed, ref dbCompanies, ref errorMessages))
                                                        {
                                                            if (IsValidCurrency(recordTobeProcessed, dbMasterData, ref errorMessages))
                                                            {
                                                                dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                                                                               SqlAuditModuleType.Contract.ToString(),
                                                                                                               SqlAuditModuleType.ContractReferences.ToString(),
                                                                                                               SqlAuditModuleType.ContractInvoiceAttachment.ToString(),
                                                                                                               SqlAuditModuleType.ContractExchange.ToString(),
                                                                                                               SqlAuditModuleType.ContractSchedule.ToString(),
                                                                                                               SqlAuditModuleType.ContractRate.ToString(),
                                                                                                               SqlAuditModuleType.ContractNote.ToString(),
                                                                                                               SqlAuditModuleType.ContractDocument.ToString(),
                                                                                                               });
                                                                if (validationType == ValidationType.Update)
                                                                {
                                                                    if (IsValidContract(recordTobeProcessed, ref dbContracts, ref errorMessages))
                                                                    {
                                                                        if (ContractProjectBudgetValidation(contracts, validationType, ref validationMessages))
                                                                        {
                                                                            useFixedExchangeValue = _contractRepository.CheckUseExchange(contracts?.FirstOrDefault(), dbContracts?.FirstOrDefault(), validationType);
                                                                            if (IsRecordUpdateCountMatching(recordTobeProcessed, dbContracts, ref errorMessages))
                                                                                if (IsContractStartDateIsGreaterThanProjectStartDate(recordTobeProcessed, dbContracts, ref errorMessages))
                                                                                    return true;
                                                                            return false;
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
                    tranScope.Complete();
                }

            }
            return false;
        }

        private void IsValidMasterData(IList<DomainModel.Contract> contracts, ref IList<DbModel.Data> dbMasterData)
        {
            var contractExpenses = contracts?.Select(x => x.ContractExpense)?.Where(x => x?.Count > 0).SelectMany(x => x).Distinct().ToList();
            var contractRef = contracts?.Select(x => x.ContractRef)?.Where(x => x?.Count > 0).SelectMany(x => x).Distinct().ToList();
            var currencyCodes = contracts?.Select(x => x.ContractCurrency)?.Where(x => x?.Count > 0).SelectMany(x => x).Distinct().ToList();

            var masterData = new List<string>[] {contracts?.Select(x => x.ContractInvoicePaymentTerms)?.Distinct()?.ToList(),
                                                  contractExpenses, contractRef}?
                                                  .Where(x => x?.Count > 0).SelectMany(x => x).ToList();


            var masterCodes = new List<string>[] {contracts?.Select(x => x.ContractInvoicingCurrency)?.Distinct()?.ToList(),
                                                  currencyCodes}?
                                                  .Where(x => x?.Count > 0).SelectMany(x => x).ToList();

            var masterTypeId = new List<int>() { (int)MasterType.ExpenseType, (int)MasterType.Currency, (int)MasterType.AssignmentReferenceType, (int)MasterType.InvoicePaymentTerms };


            dbMasterData = _dataRepository.FindBy(x => masterTypeId.Contains(x.MasterDataTypeId) && (masterData.Contains(x.Name) || masterCodes.Contains(x.Code)))?.AsNoTracking()
                                           .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type, IsActive = x.IsActive }).AsNoTracking().ToList();
        }
        private bool IsRecordCanBeDeleted(IList<DomainModel.Contract> contracts, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbContracts?.Where(x => x.Status.IsContractClosed()).ToList().
            ForEach(x =>
            {
                string errorCode = MessageType.ContractStatusClosed.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));

            });

            dbContracts.Where(x => x.Project?.Count > 0
                                  || x.VisitTechnicalSpecialistAccountItemConsumable.Count > 0
                                  || x.VisitTechnicalSpecialistAccountItemExpense.Count > 0
                                  || x.VisitTechnicalSpecialistAccountItemTime.Count > 0
                                  || x.VisitTechnicalSpecialistAccountItemTravel.Count > 0
                                  || x.TimesheetTechnicalSpecialistAccountItemConsumable.Count > 0
                                  || x.TimesheetTechnicalSpecialistAccountItemExpense.Count > 0
                                  || x.TimesheetTechnicalSpecialistAccountItemTime.Count > 0
                                  || x.TimesheetTechnicalSpecialistAccountItemTravel.Count > 0

                                  ).ToList()
            .ForEach(x =>
            {
                string errorCode = MessageType.ContractCannotBeDeleted.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractAssociated(IList<DomainModel.Contract> contracts, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contractNumber = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractNumber))?.Select(x => x.ContractNumber)?.ToList();
            var dbChildContract = _contractRepository?.FindBy(x => contractNumber.Contains(x.ParentContract.ContractNumber))?.ToList();
            var dbRelatedFrameworkContract = _contractRepository?.FindBy(x => contractNumber.Contains(x.FrameworkContract.ContractNumber))?.ToList();

            if(dbChildContract?.Count >0)
            {
                string errorCode = MessageType.ChildContractExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), dbChildContract[0].ContractNumber)));
            }
            //Commented for IGO QC D903
            //dbChildContract?.ForEach(x =>
            //{
            //    string errorCode = MessageType.ChildContractExists.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));
            //});
            if (dbRelatedFrameworkContract?.Count > 0)
            {
                string errorCode = MessageType.RelatedContractExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), dbRelatedFrameworkContract[0].ContractNumber)));
            }
            //Commented for IGO QC D903
            //dbRelatedFrameworkContract?.ForEach(x =>
            //{
            //    string errorCode = MessageType.RelatedContractExists.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));
            //});

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Contract> contracts, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contracts.Where(x => !dbContracts.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.ContractNumber == x.ContractNumber)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractUpdatedByOtherUser.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractStartDateIsGreaterThanProjectStartDate(IList<DomainModel.Contract> contracts, IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var ctrStartDates = contracts.Join(dbContracts,
                dcnt => dcnt.ContractNumber,
                dbcnt => dbcnt.ContractNumber,
                (dcnt, dbcnt) => new { dcnt, dbcnt }
                ).Select(x => new KeyValuePair<int, DateTime>(x.dbcnt.Id, x.dcnt.ContractStartDate.Value)).ToList();
            var prjMatchedRecords = dbContracts.SelectMany(x => x.Project).Where(x => ctrStartDates.Any(x1 => x1.Key == x.ContractId && x.StartDate.Date < x1.Value.Date)).ToList();

            prjMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractStartDateIsGreaterThanProjecrStartDate.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectNumber)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCurrency(IList<DomainModel.Contract> contracts, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            IList<DbModel.Data> dbCurrencies = null;
            var currencies = contracts?.Select(x => x.ContractInvoicingCurrency)?.ToList();

            if (dbMasterData?.Count > 0)
                dbCurrencies = dbMasterData?.Where(x => currencies.Contains(x.Code) && x.MasterDataTypeId == Convert.ToInt16(MasterType.Currency.ToId()))?.ToList();
            else
                dbCurrencies = _dataRepository?.FindBy(x => currencies.Contains(x.Code) && x.MasterDataTypeId == Convert.ToInt16(MasterType.Currency.ToId()))?
                                               .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type, IsActive = x.IsActive })?.ToList();

            var currencyNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractInvoicingCurrency))?.Where(x1 => !dbCurrencies.Any(x2 => x2.Code == x1.ContractInvoicingCurrency))?.ToList();
            currencyNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractInvoicingCurrency.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractInvoicingCurrency)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidTax(IList<DomainModel.Contract> contracts, ref IList<DbModel.Company> dbCompanies, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Company> dbCompany = dbCompanies;

            var companyTax = dbCompany?.SelectMany(x => x.CompanyTax)?.ToList();
            var salesTaxNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractSalesTax))?.Where(x1 => !companyTax.Any(x2 => x2.Name == x1.ContractSalesTax && x2.TaxType.IsSalesTax()
                                                        && x2.Company.Code == x1.ContractHoldingCompanyCode))?.ToList();

            var withHoldingTaxNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractWithHoldingTax))?.Where(x1 => !companyTax.Any(x2 => x2.Name == x1.ContractWithHoldingTax && x2.TaxType.IsWithholdingTax()
                                                        && x2.Company.Code == x1.ContractHoldingCompanyCode))?.ToList();

            withHoldingTaxNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.TaxWithholdingInvalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractWithHoldingTax)));
            });

            salesTaxNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.TaxSalesInvalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractSalesTax)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidIdentifier(IList<DomainModel.Contract> contracts, ref IList<DbModel.Company> dbCompanies, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Company> dbCompany = dbCompanies;
            var companyMessages = dbCompany?.SelectMany(x => x.CompanyMessage)?.ToList();
            var remittanceNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractInvoiceRemittanceIdentifier))?.Where(x1 => !companyMessages.Any(x2 => x2.Identifier == x1.ContractInvoiceRemittanceIdentifier
                                                          && x2.Company.Code == x1.ContractHoldingCompanyCode))?.ToList();

            var footerNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractInvoiceFooterIdentifier))?.Where(x1 => !companyMessages.Any(x2 => x2.Identifier == x1.ContractInvoiceFooterIdentifier
                                                        && x2.Company.Code == x1.ContractHoldingCompanyCode))?.ToList();

            remittanceNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractRemittance.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractInvoiceRemittanceIdentifier)));
            });

            footerNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractFooter.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractInvoiceFooterIdentifier)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidInvoicePaymentTerm(IList<DomainModel.Contract> contracts, ref IList<DbModel.Data> dbPaymentTerms, IList<DbModel.Data> dbMasterData, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            IList<DbModel.Data> dbInvoicePaymentTerms = null;
            var invoicePaymentTerms = contracts?.Select(x => x.ContractInvoicePaymentTerms)?.ToList();
            if (dbMasterData?.Count > 0)
                dbInvoicePaymentTerms = dbMasterData?.Where(x => invoicePaymentTerms.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt16(MasterType.InvoicePaymentTerms.ToId()) && x.IsActive == true)?.ToList();
            else
                dbInvoicePaymentTerms = _dataRepository?.FindBy(x => invoicePaymentTerms.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt16(MasterType.InvoicePaymentTerms.ToId()) && x.IsActive == true)?
                                                        .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type, IsActive = x.IsActive })?.ToList();
            var invoicePaymentTermsNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractInvoicePaymentTerms))?.Where(x1 => !dbInvoicePaymentTerms.Any(x2 => x2.Name == x1.ContractInvoicePaymentTerms))?.ToList();
            invoicePaymentTermsNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractInvoicePaymentTerm.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractInvoicePaymentTerms)));
            });

            dbPaymentTerms = dbInvoicePaymentTerms;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCustomerContractNumber(IList<DomainModel.Contract> contracts, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();


            var customerContractNumber = contracts?.Where(x => !string.IsNullOrEmpty(x.CustomerContractNumber)
                                         && (x.ContractType == ContractType.PAR.ToString() || x.ContractType == ContractType.STD.ToString()))
                                        .Select(x1 => new
                                        {
                                            x1.CustomerContractNumber,
                                            x1.ContractCustomerCode,
                                            x1.ContractNumber,
                                            x1.ContractType
                                        })?.ToList();

            var customerContractNumberAlreadyExists = customerContractNumber;

            if (customerContractNumber != null && customerContractNumber.Count > 0)
            {
                var filterExpressions = new List<Expression<Func<DbModel.Contract, bool>>>();
                Expression<Func<DbModel.Contract, bool>> predicate = null;
                Expression<Func<DbModel.Contract, bool>> containsExpression = null;

                foreach (var ccn in customerContractNumber)
                {
                    containsExpression = a => a.CustomerContractNumber == ccn.CustomerContractNumber
                                          && a.Customer.Code == ccn.ContractCustomerCode;
                    filterExpressions.Add(containsExpression);
                }
                predicate = filterExpressions.CombinePredicates<DbModel.Contract>(Expression.OrElse);
                if (predicate != null)
                {
                    containsExpression = a => (a.ContractType == ContractType.PAR.ToString() || a.ContractType == ContractType.STD.ToString());
                    predicate = predicate.CombineWithAndAlso(containsExpression);
                }

                var dbContracts = _contractRepository.FindBy(predicate).ToList();

                if (validationType == ValidationType.Add)
                    customerContractNumberAlreadyExists = customerContractNumber.Where(x => dbContracts.Any(x1 => x1.CustomerContractNumber == x.CustomerContractNumber)).ToList();


                if (validationType == ValidationType.Update)
                    customerContractNumberAlreadyExists = customerContractNumber.Where(x => dbContracts.Any(x1 => x1.CustomerContractNumber == x.CustomerContractNumber
                                                                                                              && x.ContractNumber != x1.ContractNumber)).ToList();
                customerContractNumberAlreadyExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCustomerContractNumber.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CustomerContractNumber)));
                });
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidContract(IList<DomainModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contractNumber = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractNumber))?.Select(x => x.ContractNumber)?.ToList();
            if (contractNumber?.Any() == true)
            {
                var dbContract = _contractRepository?.FindBy(x => contractNumber.Contains(x.ContractNumber))?.ToList();
                var contractNumberNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractNumber))?.Where(x1 => !dbContract.Any(x2 => x2.ContractNumber == x1.ContractNumber))?.ToList();
                contractNumberNotExists.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidContractNumber.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNumber)));
                });

                dbContracts = dbContract;

                if (messages.Count > 0)
                    errorMessages.AddRange(messages);
            }

            return errorMessages?.Count <= 0;
        }

        private bool IsValidContract(IList<DomainModel.Contract> contracts,
                                        IList<DbModel.Company> dbParentCompanies,
                                        IList<DbModel.Company> dbFrameworkParentCompanies,
                                        ref IList<DbModel.Company> dbCompanies,
                                        ref IList<DbModel.Contract> dbContracts,
                                        ref IList<DbModel.Customer> dbCustomers,
                                        ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contractNumber = contracts.ToList()?.Select(x => new { x.ParentContractNumber, x.FrameworkContractNumber })?.ToList();
            var contractType = contracts.ToList()?.Select(x => new { x.ContractType })?.FirstOrDefault()?.ContractType;
            var companyId = contractType == ContractType.CHD.ToString() ? dbCompanies?.Select(x => x.Id)?.ToList()?.Union(dbParentCompanies?.Select(x => x.Id)?.ToList()) :
                            contractType == ContractType.IRF.ToString() ? dbCompanies?.Select(x => x.Id)?.ToList()?.Union(dbFrameworkParentCompanies?.Select(x => x.Id)?.ToList()) :
                            dbCompanies?.Select(x => x.Id)?.ToList();
            var customerId = dbCustomers?.Select(x => x.Id)?.ToList();

            var filterExpressions = new List<Expression<Func<DbModel.Contract, bool>>>();
            Expression<Func<DbModel.Contract, bool>> predicate = null;
            Expression<Func<DbModel.Contract, bool>> containsExpression = null;

            foreach (var contractNum in contractNumber)
            {
                containsExpression = a => (a.ContractNumber == contractNum.FrameworkContractNumber || a.ContractNumber == contractNum.ParentContractNumber);
                filterExpressions.Add(containsExpression);
            }
            predicate = filterExpressions.CombinePredicates<DbModel.Contract>(Expression.OrElse);

            if (predicate != null)
            {
                containsExpression = a => companyId.Contains(a.ContractHolderCompanyId) && customerId.Contains(a.CustomerId);
                predicate = predicate.CombineWithAndAlso(containsExpression);
            }

            var dbContract = _contractRepository.FindBy(predicate).ToList();

            var parentContractNumberNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ParentContractNumber))?.Where(x1 => !dbContract.Any(x2 => x2.ContractNumber == x1.ParentContractNumber))?.ToList();

            var frameworkContractNumberNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.FrameworkContractNumber))?.Where(x1 => !dbContract.Any(x2 => x2.ContractNumber == x1.FrameworkContractNumber))?.ToList();

            parentContractNumberNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidParentContractNumber.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ParentContractNumber)));
            });

            frameworkContractNumberNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidFrameworkContractNumber.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.FrameworkContractNumber)));
            });

            dbContracts = dbContract;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyOffice(IList<DomainModel.Contract> contracts, IList<DbModel.Company> dbParentCompanies,
                                            IList<DbModel.Company> dbFrameworkCompanies, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (dbParentCompanies?.Count > 0 || dbFrameworkCompanies?.Count > 0)
            {

                var companyParentOffices = dbParentCompanies?.SelectMany(x => x.CompanyOffice)?.ToList();
                var companyFrameworkOffices = dbFrameworkCompanies?.SelectMany(x => x.CompanyOffice)?.ToList();
                var companyParentOfficesNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ParentCompanyOffice))?.Where(x1 => !companyParentOffices.Any(x2 => x2.OfficeName == x1.ParentCompanyOffice))?.ToList();
                var companyFrameworkOfficesNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.FrameworkCompanyOfficeName))?.Where(x1 => !companyFrameworkOffices.Any(x2 => x2.OfficeName == x1.FrameworkCompanyOfficeName))?.ToList();

                companyParentOfficesNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.ParentOfficeName_Not_Exists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ParentCompanyOffice)));
                });

                companyFrameworkOfficesNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.FrameworkOfficeName_Not_Exists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.FrameworkCompanyOfficeName)));
                });

                if (messages.Count > 0)
                    errorMessages.AddRange(messages);
            }

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCustomerContact(IList<DomainModel.Contract> contracts, ref IList<DbModel.Customer> dbCustomers, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Customer> dbCustomer = dbCustomers;

            var customerContacts = dbCustomer?.ToList().SelectMany(x => x.CustomerAddress).SelectMany(x1 => x1.CustomerContact).ToList();
            var customerContactsNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractCustomerContact))?.Where(x1 => !customerContacts.Any(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x1.ContractCustomerContact.Where(c => !char.IsWhiteSpace(c)))))?.ToList();
            var customerInvoiceContactsNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractCustomerInvoiceContact))?.Where(x1 => !customerContacts.Any(x2 => String.Join("", x2.ContactName.Where(c => !char.IsWhiteSpace(c))) == String.Join("", x1.ContractCustomerInvoiceContact.Where(c => !char.IsWhiteSpace(c)))))?.ToList();

            customerContactsNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.Customer_Contact_Invalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractCustomerContact)));
            });

            customerInvoiceContactsNotExists.ForEach(x =>
            {
                string errorCode = MessageType.Customer_InvoiceContact_Invalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractCustomerInvoiceContact)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCustomerAddress(IList<DomainModel.Contract> contracts, ref IList<DbModel.Customer> dbCustomers, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<DbModel.Customer> dbCustomer = dbCustomers;

            var customerAddresses = dbCustomer?.ToList().SelectMany(x => x.CustomerAddress)?.ToList();

            var customerAddressesNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractCustomerContactAddress))?
                                                       .Where(x1 => !customerAddresses.Any(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                        == String.Join("", x1.ContractCustomerContactAddress.Where(c => !char.IsWhiteSpace(c)))))?.ToList();

            var customerInvoiceAddressesNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractCustomerInvoiceAddress))?
                                                    .Where(x1 => !customerAddresses.Any(x2 => String.Join("", x2.Address.Where(c => !char.IsWhiteSpace(c)))
                                                     == String.Join("", x1.ContractCustomerInvoiceAddress.Where(c => !char.IsWhiteSpace(c)))))?.ToList();

            customerAddressesNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.CustAddr_Address.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractCustomerContactAddress)));
            });

            customerInvoiceAddressesNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.CustAddr_Invoice_Address.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractCustomerInvoiceAddress)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCustomer(IList<DomainModel.Contract> contracts, ref IList<DbModel.Customer> dbCustomers, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var customerCodes = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractCustomerCode))?.Select(x1 => x1.ContractCustomerCode)?.ToList();
            //var dbCustomerData = _customerRepository?.FindBy(x => customerCodes.Contains(x.Code),
            //                                        new string[] {
            //                                            "CustomerAddress",
            //                                            "CustomerAddress.CustomerContact",
            //                                        })?.ToList();

            var dbCustomerData = _customerRepository?.FindBy(x => customerCodes.Contains(x.Code), new string[] { "CustomerAddress", "CustomerAddress.CustomerContact", })
                                                   ?.Select(x => new DbModel.Customer
                                                   {
                                                       Id = x.Id,
                                                       Code = x.Code,
                                                       Name = x.Name,
                                                       CustomerAddress = x.CustomerAddress != null ? x.CustomerAddress.Select(x1 => new DbModel.CustomerAddress
                                                       {
                                                           Id = x1.Id,
                                                           Address = x1.Address,
                                                           CustomerContact = x1.CustomerContact != null ? x1.CustomerContact.Select(x2 => new DbModel.CustomerContact
                                                           {
                                                               Id = x2.Id,
                                                               ContactName = x2.ContactName
                                                           }).ToList() : null
                                                       }).ToList() : null,
                                                   })?.ToList();

            var customerCodeNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractCustomerCode))?.Where(x1 => !dbCustomerData.Any(x2 => x2.Code == x1.ContractCustomerCode))?.ToList();
            customerCodeNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.CustAddr_Code_Invalid.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractCustomerCode)));
            });

            dbCustomers = dbCustomerData;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompany(IList<DomainModel.Contract> contracts,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<DbModel.Company> dbParentCompanies,
                                    ref IList<DbModel.Company> dbFrameworkParentCompanies,
                                    ref List<MessageDetail> errorMessages)

        {
            List<MessageDetail> messages = new List<MessageDetail>();
            IList<DbModel.Company> dbParentCompanyData = null;
            IList<DbModel.Company> dbFrameworkCompanyData = null;
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contractCompanyCodes = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractHoldingCompanyCode))?.Select(x1 => x1.ContractHoldingCompanyCode)?.ToList();
            var parentCompanyCodes = contracts?.Where(x => !string.IsNullOrEmpty(x.ParentCompanyCode))?.Select(x1 => x1.ParentCompanyCode)?.ToList();
            var frameworkCompanyCodes = contracts?.Where(x => !string.IsNullOrEmpty(x.FrameworkCompanyCode))?.Select(x1 => x1.FrameworkCompanyCode)?.ToList();

            if (parentCompanyCodes?.Any() == true && contractCompanyCodes?.Any() == true)
                contractCompanyCodes.AddRange(parentCompanyCodes);

            if (frameworkCompanyCodes?.Any() == true && contractCompanyCodes?.Any() == true)
                contractCompanyCodes.AddRange(frameworkCompanyCodes);


            var dbCompanyData = _companyRepository?.FindBy(x => contractCompanyCodes.Contains(x.Code))?.Select(x => new DbModel.Company
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CompanyOffice = x.CompanyOffice != null ? x.CompanyOffice.Select(x1 => new DbModel.CompanyOffice
                {
                    Id = x1.Id,
                    OfficeName = x1.OfficeName
                }).ToList() : null,
                CompanyTax = x.CompanyTax != null ? x.CompanyTax.Select(x1 => new DbModel.CompanyTax
                {
                    Id = x1.Id,
                    Name = x1.Name,
                    TaxType = x1.TaxType,
                    Company = new DbModel.Company { Id = x.Id, Code = x.Code }
                }).ToList() : null,
                CompanyMessage = x.CompanyMessage != null ? x.CompanyMessage.Select(x1 => new DbModel.CompanyMessage
                {
                    Id = x1.Id,
                    Identifier = x1.Identifier,
                    MessageTypeId = x1.MessageTypeId,
                    Company = new DbModel.Company { Id = x.Id, Code = x.Code }
                }).ToList() : null,
            })?.ToList();
            var companyCodeNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ContractHoldingCompanyCode))?.Where(x1 => !dbCompanyData.Any(x2 => x2.Code == x1.ContractHoldingCompanyCode))?.ToList();

            companyCodeNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidCompanyCode.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractHoldingCompanyCode)));
            });


            if (parentCompanyCodes?.Count > 0)
            {
                dbParentCompanyData = dbCompanyData?.ToList()?.Where(x => parentCompanyCodes.Contains(x.Code))?.ToList();
                if (dbParentCompanyData?.Any() == false)
                    dbParentCompanyData = _companyRepository?.FindBy(x => parentCompanyCodes.Contains(x.Code))?.ToList();
                var parentCompanyCodeNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.ParentCompanyCode))?.Where(x1 => !dbParentCompanyData.Any(x2 => x2.Code == x1.ParentCompanyCode))?.ToList();
                parentCompanyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCompanyCode.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ParentCompanyCode)));
                });
            }

            if (frameworkCompanyCodes?.Count > 0)
            {
                dbFrameworkCompanyData = dbCompanyData?.ToList()?.Where(x => frameworkCompanyCodes.Contains(x.Code))?.ToList();
                if (dbFrameworkCompanyData?.Any() == false)
                    dbFrameworkCompanyData = _companyRepository?.FindBy(x => frameworkCompanyCodes.Contains(x.Code))?.ToList();
                var frameworkCompanyCodeNotExists = contracts?.Where(x => !string.IsNullOrEmpty(x.FrameworkCompanyCode))?.Where(x1 => !dbFrameworkCompanyData.Any(x2 => x2.Code == x1.FrameworkCompanyCode))?.ToList();
                frameworkCompanyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCompanyCode.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.FrameworkCompanyCode)));
                });
            }

            dbCompanies = dbCompanyData;
            dbParentCompanies = dbParentCompanyData;
            dbFrameworkParentCompanies = dbFrameworkCompanyData;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordsValidToProcess(IList<DomainModel.Contract> contracts, ValidationType validationType, ref IList<DomainModel.Contract> filteredContracts, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {

            if (validationType == ValidationType.Add)
                filteredContracts = contracts.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            if (validationType == ValidationType.Update)
                filteredContracts = contracts.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            if (validationType == ValidationType.Delete)
                filteredContracts = contracts.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            if (filteredContracts.Count <= 0)
                return false;
            return IsContractHasValidSchema(filteredContracts, validationType, ref errorMessages, ref validationMessages);

        }

        private bool IsContractHasValidSchema(IList<DomainModel.Contract> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(models), validationType);
            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Contract, x.Code, x.Message) }));
            });

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;

        }

        private Response PopulateContractInvoiceInfo(IList<BudgetAccountItem> budgetAccountItems, IList<ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
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

                        //contractWithoutExchangeRate = contractWithoutExchangeRate.Where(x => x.ExpenseType == "E" && x.AssignmentId == 21897)?.ToList();

                        var currencyWithoutExchangeRate = contractWithoutExchangeRate?//.Where(x => x.ExpenseType == "R" && x.AssignmentId == 410677)
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

                    #region NotToBeRemoved
                    // List<Tuple<int, decimal>> value = null;
                    //// budgetAccountItems = budgetAccountItems?.Where(x => x.ExpenseType == "E" && x.AssignmentId == 283713)?.ToList();
                    // result = budgetAccountItems?.Where(x => x.ExpenseType == "E")
                    //         .GroupBy(x => x.AssignmentId)
                    //         .Select(x =>
                    //         new InvoicedInfo
                    //         {
                    //             AssignmentId = x.FirstOrDefault().AssignmentId,
                    //             ContractId = x.Key,
                    //             InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                    //             UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                    //             HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                    //             HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                    //             ContractNumber = x?.FirstOrDefault().ContractNumber,
                    //         }).ToList();

                    // value = new List<Tuple<int, decimal>>();
                    // foreach (var budget in result.Where(x => x.InvoicedToDate > 0))
                    // {
                    //     value.Add(Tuple.Create(budget.AssignmentId, budget.InvoicedToDate));
                    // }

                    //var count1 = budgetAccountItems?.Where(x => x.ExpenseType == "R")?.Count();
                    //budgetAccountItems = budgetAccountItems.OrderBy(x => x.AssignmentId)?.ToList();
                    ////budgetAccountItems = budgetAccountItems.Where(x => x.AssignmentId ==  410677 )?.ToList();
                    //List<Tuple<int, decimal>> value = null;
                    //result = budgetAccountItems?.Where(x => x.ExpenseType == "R")
                    //           .GroupBy(x => x.ContractId)
                    //           .Select(x =>
                    //           new InvoicedInfo
                    //           {
                    //               AssignmentId = x.FirstOrDefault().AssignmentId,
                    //               ContractId = x.FirstOrDefault().ContractId,
                    //               InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                    //               UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                    //               HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                    //               HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                    //               ContractNumber = x?.FirstOrDefault().ContractNumber,
                    //           }).ToList();
                    //budgetAccountItems = budgetAccountItems?.Where(x => x.ExpenseType == "R" && x.AssignmentId == 309538)?.ToList();
                    //result = budgetAccountItems?.Where(x => x.ExpenseType == "R")?
                    //            .GroupBy(x => x.AssignmentId)
                    //            .Select(x =>
                    //            new InvoicedInfo
                    //            {
                    //                AssignmentId = x.FirstOrDefault().AssignmentId,
                    //                ContractId = x.FirstOrDefault().ContractId,
                    //                InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                    //                UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                    //                HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name?.Trim() == x1.ExpenseName?.Trim())?.Hour ?? 0)) : 0)),
                    //                HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name?.Trim() == x1.ExpenseName?.Trim())?.Hour ?? 0)))),
                    //                ContractNumber = x?.FirstOrDefault().ContractNumber,
                    //            }).ToList();

                    //value = new List<Tuple<int, decimal>>();
                    //foreach (var budget in result.Where(x => x.InvoicedToDate > 0))
                    //{
                    //    value.Add(Tuple.Create(budget.AssignmentId, budget.InvoicedToDate));
                    //}

                    //var count = budgetAccountItems?.Where(x => x.ExpenseType == "T")?.Count();

                    //result = budgetAccountItems?.Where(x => x.ExpenseType == "T")
                    //        .GroupBy(x => x.AssignmentId)
                    //        .Select(x =>
                    //        new InvoicedInfo
                    //        {
                    //            AssignmentId = x.FirstOrDefault().AssignmentId,
                    //            ContractId = x.Key,
                    //            InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                    //            UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                    //            HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                    //            HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                    //            ContractNumber = x?.FirstOrDefault().ContractNumber,
                    //        }).ToList();
                    #endregion

                    result = budgetAccountItems?
                             .GroupBy(x => x.ContractId)
                             .Select(x =>
                             new InvoicedInfo
                             {
                                 ContractId = x.Key,
                                 ParentContractId = x?.FirstOrDefault()?.ParentContractId,
                                 InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                                 UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                                 HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                                 HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour?.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                                 ContractNumber = x?.FirstOrDefault().ContractNumber,
                             }).ToList();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }



        #endregion
    }
}
