using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Models.Contracts;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Project.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using DomainModel = Evolution.Draft.Domain.Models;
using ResourceSearchDomainModel = Evolution.ResourceSearch.Domain.Models;
using System.Collections;
using AutoMapper;
using Evolution.Draft.Domain.Interfaces.Draft;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Evolution.Security.Domain.Models.Security;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Common.Constants;

namespace Evolution.Home.Core.Services
{
    public class HomeService : IHomeService
    {
        private readonly IContractService _contractService = null;
        private readonly IProjectService _projectService = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly IAppLogger<HomeService> _logger = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IDraftService _draftService = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IMapper _mapper = null;
        private readonly IResourceSearchService _resourceSearchService = null;
        private readonly IUserService _userService = null;

        public HomeService(IAppLogger<HomeService> logger,
                            IContractService contractService,
                            IProjectService projectService,
                            IAssignmentService assignmentService,
                            IContractExchangeRateService contractExchangeRateService,
                            ICurrencyExchangeRateService currencyExchangeRateService,
                            IDraftService draftService,
                            IMyTaskService myTaskService,
                            IResourceSearchService resourceSearchService,
                            IUserService userService,
                            IMapper mapper)
        {
            _contractService = contractService;
            _projectService = projectService;
            _assignmentService = assignmentService;
            _contractExchangeRateService = contractExchangeRateService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _logger = logger;
            _draftService = draftService;
            _myTaskService = myTaskService;
            _resourceSearchService = resourceSearchService;
            _userService = userService;
            this._mapper = mapper;
        }

        public Response GetBudget(string companyCode = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool isMyAssignmentOnly = true, bool? IsOverBudgetValue = null, bool? IsOverBudgetHour = null)
        {
            List<Budget> result = new List<Budget>();
            Exception exception = null;
            try
            {
                var contracts = _contractService.GetContract(companyCode, userName, contractStatus, isMyAssignmentOnly)
                                            .Result
                                            .Populate<IList<ContractWithId>>();

                var contractIds = contracts?.Select(x => x.ContractId).Distinct().ToList();

                var contractBudgetAccountItems = _contractService.GetContractBudgetAccountItems(contractIds, userName, isMyAssignmentOnly)
                                                                 .Result
                                                                 .Populate<IList<BudgetAccountItem>>();

                var contractExchRates = _contractExchangeRateService.GetContractExchangeRates(contractIds)
                                                                    .Result
                                                                    .Populate<IList<Common.Models.ExchangeRate.ContractExchangeRate>>();


                var contractWithoutExchangeRate = contractBudgetAccountItems?
                                                       .Where(x => !contractExchRates
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

                var currencyExchangeRates = _currencyExchangeRateService
                                            .GetExchangeRates(currencyWithoutExchangeRate)
                                            .Result
                                            .Populate<IList<ExchangeRate>>();

                var allCoordinatorContractBudgetAccountItems = _contractService.GetContractBudgetAccountItems(contractIds, null, isMyAssignmentOnly)
                                                                 .Result
                                                                 .Populate<IList<BudgetAccountItem>>();

                var allCoordinatorContractWithoutExchangeRate = allCoordinatorContractBudgetAccountItems?
                                                       .Where(x => !contractExchRates
                                                                   .Any(x1 => x.ContractId == x1.ContractId &&
                                                                        x.ChargeRateCurrency == x1.CurrencyFrom &&
                                                                        x.BudgetCurrency == x1.CurrencyTo)).ToList();

                var allCoordinatorCurrencyWithoutExchangeRate = allCoordinatorContractWithoutExchangeRate?
                                               .Select(x => new ExchangeRate()
                                               {
                                                   CurrencyFrom = x.ChargeRateCurrency,
                                                   CurrencyTo = x.BudgetCurrency,
                                                   EffectiveDate = x.VisitDate
                                               }).ToList();

                var allCoordinatorCurrencyExchangeRates = _currencyExchangeRateService
                                           .GetExchangeRates(allCoordinatorCurrencyWithoutExchangeRate)
                                           .Result
                                           .Populate<IList<ExchangeRate>>();

                //var contractBudgets = _contractService.GetContractBudgetDetails(contractBudgetAccountItems, contractExchRates, contracts, currencyExchangeRates)
                //                         .Result.Populate<IList<Budget>>();

                var contractBudgets = _contractService.GetContractBudgetDetails(allCoordinatorContractBudgetAccountItems, contractExchRates, contracts, allCoordinatorCurrencyExchangeRates)
                                      .Result.Populate<IList<Budget>>(); //Changes for ITK - D1478(To show all Coordinator's Contract Budgets)

                if (contractBudgets?.Count > 0)
                    result.AddRange(contractBudgets);

                var projectBudgets = _projectService.GetProjectBudgetDetails(contractBudgetAccountItems, contractExchRates, currencyExchangeRates)
                                        .Result.Populate<IList<Budget>>();

                if (projectBudgets?.Count > 0)
                    result.AddRange(projectBudgets);

                var assignmentBudgets = _assignmentService.GetAssignmentBudgetDetails(contractBudgetAccountItems, contractExchRates, currencyExchangeRates)
                                        .Result.Populate<IList<Budget>>();

                if (assignmentBudgets?.Count > 0)
                    result.AddRange(assignmentBudgets);

                if (IsOverBudgetValue != null || IsOverBudgetHour != null)
                {
                    var overBudgetContractIds = GetOverBudgetedContracts(IsOverBudgetValue, IsOverBudgetHour, contractBudgets, projectBudgets, assignmentBudgets);

                    if (overBudgetContractIds?.Count > 0)
                    {
                        result = result?.Where(x => overBudgetContractIds.Contains(x.ContractId)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetMyTask(string companyCode, string userName, ModuleCodeType moduleCodeType)
        {
            DomainModel.Draft searchModel = new DomainModel.Draft
            {
                Moduletype = moduleCodeType.ToString(),
                AssignedTo = userName
            };
            // searchModel.Description = DraftType.Task.ToString();

            Domain.Models.Homes.MyTask myTasksearchModel = new Domain.Models.Homes.MyTask
            {
                Moduletype = moduleCodeType.ToString(),
                AssignedTo = userName
            };

            IList<Domain.Models.Homes.MyTask> result = new List<Domain.Models.Homes.MyTask>();

            Exception exception = null;

            try
            {
                var Drafts = _draftService.GetDraft(searchModel).Result.Populate<IList<DomainModel.Draft>>();
                var Tasks = _myTaskService.Get(myTasksearchModel).Result.Populate<IList<Domain.Models.Homes.MyTask>>();


                if (Drafts?.Count > 0)
                    result.AddRange(_mapper.Map<IList<Domain.Models.Homes.MyTask>>(Drafts));
                if (Tasks?.Count > 0)
                    result.AddRange(Tasks);
                for (int i = 1; i <= result.Count; i++)
                {
                    result[i - 1].MyTaskId = i;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetMyTask(string companyCode, string userName)
        {
            IList<Domain.Models.Homes.MyTask> result = null;
            Exception exception = null;
            try
            {
                List<string> companyUsers = new List<string> { userName };
                if (string.IsNullOrEmpty(userName))
                {
                    Response UserResponse = this.GetMyTaskAssignUsers(companyCode);
                    if (UserResponse != null && UserResponse.Code != null && UserResponse.Code == "1" && UserResponse.Result != null)
                    {
                        companyUsers = UserResponse.Result.Populate<IList<UserInfo>>().Select(m => m.LogonName).ToList();
                    }
                }

                DomainModel.Draft searchModel = new DomainModel.Draft
                {
                    Moduletype = ModuleCodeType.TS.ToString(),
                    AssignedToUsers = companyUsers,
                    ///  CompanyCode = companyCode, //D661 issue1 myTask CR //Commented for D363 CR Change
                    //Description = DraftType.Task.ToString()
                };
                //D661 issue1 myTask CR
                Domain.Models.Homes.MyTask myTasksearchModel = new Domain.Models.Homes.MyTask
                {
                    AssignedTo = userName
                };
                if (string.IsNullOrEmpty(userName)) { myTasksearchModel.CompanyCode = companyCode;
                    searchModel.CompanyCode = companyCode;  } //D702 #18issue (Ref ALM Doc 11-06-2020)
                    ///  myTasksearchModel.CompanyCode = companyCode; //Commented for D363 CR Change
                    //D661 issue 1 myTask CR
                    result = new List<Domain.Models.Homes.MyTask>();
                var moduletype = new List<string> { ModuleCodeType.TS.ToString(), ModuleCodeType.RSEARCH.ToString() };
                var Drafts = _draftService.GetDraftMyTask(searchModel).Result.Populate<IList<DomainModel.Draft>>()?.Where(task => !(task.DraftType.Equals(TechnicalSpecialistConstants.ProfileChangeHistory))).ToList();
                //  var Tasks = _myTaskService.Get(moduletype, companyUsers, null).Result.Populate<IList<Domain.Models.Homes.MyTask>>();
                var Tasks = _myTaskService.Get(myTasksearchModel).Result.Populate<IList<Domain.Models.Homes.MyTask>>();//D661 issue1 myTask CR

                if (Drafts?.Count > 0)
                {
                    result.AddRange(_mapper.Map<IList<Domain.Models.Homes.MyTask>>(Drafts));
                }
                if (Tasks?.Count > 0)
                    result.AddRange(Tasks);
                for (int i = 1; i <= result.Count; i++)
                {
                    result[i - 1].MyTaskId = i;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetMyResourceSearch(string companyCode, string assignedToUser, bool isAllCoordinator)
        {
            Exception exception = null;
            IList<ResourceSearchDomainModel.ResourceSearch.ResourceSearchResult> result = null;
            try
            {
                var mySearchTypes = new List<KeyValuePair<string, IList<string>>> {
                    new KeyValuePair<string, IList<string>>(ResourceSearchType.Quick.ToString(), new List<string> { ResourceSearchAction.SS.ToString() }),
                    new KeyValuePair<string, IList<string>>(ResourceSearchType.PreAssignment.ToString(), new List<string> { ResourceSearchAction.CUP.ToString(), ResourceSearchAction.W.ToString() }),
                };

                result = _resourceSearchService.Get(mySearchTypes, assignedToUser, companyCode, isAllCoordinator).Result.Populate<IList<ResourceSearchDomainModel.ResourceSearch.ResourceSearchResult>>()?.OrderBy(x => x.Id)?.ToList();

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        private IList<int> GetOverBudgetedContracts(bool? IsOverBudgetValue, bool? IsOverBudgetHour, IList<Budget> contractBudgets, IList<Budget> projectBudgets, IList<Budget> assignmentBudgets)
        {
            List<int> overBudgetContractIds = new List<int>();
            if (IsOverBudgetValue != null)
            {
                var overBudgetContracts = contractBudgets?.Where(x => x.IsOverBudgetValue == IsOverBudgetValue);
                if (overBudgetContracts?.Count() > 0)
                {
                    overBudgetContractIds.AddRange(overBudgetContracts?.Select(x => x.ContractId));
                }

                var overBudgetProjects = projectBudgets?.Where(x => x.IsOverBudgetValue == IsOverBudgetValue);
                if (overBudgetProjects?.Count() > 0)
                {
                    overBudgetContractIds.AddRange(overBudgetProjects?.Select(x => x.ContractId));
                }

                var overBudgetAssignments = assignmentBudgets?.Where(x => x.IsOverBudgetValue == IsOverBudgetValue);
                if (overBudgetAssignments?.Count() > 0)
                {
                    overBudgetContractIds.AddRange(overBudgetAssignments?.Select(x => x.ContractId));
                }
            }
            if (IsOverBudgetHour != null)
            {
                var overBudgetHourContracts = contractBudgets?.Where(x => x.IsOverBudgetHour == IsOverBudgetHour);
                if (overBudgetHourContracts?.Count() > 0)
                {
                    overBudgetContractIds.AddRange(overBudgetHourContracts?.Select(x => x.ContractId));
                }

                var overBudgetHourProjects = projectBudgets?.Where(x => x.IsOverBudgetHour == IsOverBudgetHour);
                if (overBudgetHourProjects?.Count() > 0)
                {
                    overBudgetContractIds.AddRange(overBudgetHourProjects?.Select(x => x.ContractId));
                }

                var overBudgetHourAssignments = assignmentBudgets?.Where(x => x.IsOverBudgetHour == IsOverBudgetHour);
                if (overBudgetHourAssignments?.Count() > 0)
                {
                    overBudgetContractIds.AddRange(overBudgetHourAssignments?.Select(x => x.ContractId));
                }
            }

            return overBudgetContractIds?.Distinct().ToList();
        }

        public Response GetMyTaskAssignUsers(string companyCode)
        {
            IList<UserInfo> result = null;
            Exception exception = null;
            try
            {
                List<string> userTypes = new List<string> { MyTaskAssignUserConstants.UserType_ResourceManager,
                MyTaskAssignUserConstants.UserType_ResourceCoordinator,
                MyTaskAssignUserConstants.UserType_TechnicalManager,
                MyTaskAssignUserConstants.UserType_OperationManager};
                result = _userService.GetByUserType(companyCode, userTypes, true)
                                        .Result
                                        .Populate<IList<UserInfo>>();
                result = result?.GroupBy(p => p.LogonName)?.Select(g => g.FirstOrDefault()).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        //Added for ITK Defect 908(Ref ALM Document 14-05-2020)
        public Response GetMyTaskReAssignUsers(string companyCode)
        {
            IList<UserTypeInfo> result = null;
            Exception exception = null;
            try
            {
                List<string> userTypes = new List<string> { MyTaskAssignUserConstants.UserType_ResourceManager,
                MyTaskAssignUserConstants.UserType_ResourceCoordinator,
                MyTaskAssignUserConstants.UserType_TechnicalManager,
                MyTaskAssignUserConstants.UserType_OperationManager,
                MyTaskAssignUserConstants.UserType_MICoordinator }; //D908(Ref ALM 11-06-2020)
                result = _userService.GetUserType(companyCode, userTypes)
                                        .Result
                                        .Populate<IList<UserTypeInfo>>();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        //Added for ITK Defect 908(Ref ALM Document 14-05-2020)
    }
}
