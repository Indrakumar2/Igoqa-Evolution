using Evolution.Common.Enums;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel=Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractService
    {
        Response SaveContract(IList<Models.Contracts.Contract> model, ref long? eventId,bool commitChange = true);

        Response SaveContract(IList<Models.Contracts.Contract> contracts, ref IList<DbModel.Contract> dbContracts,ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange = true);

        Response ModifyContract(IList<Models.Contracts.Contract> model, ref long? eventId, bool commitChange = true);

        Response ModifyContract(IList<Models.Contracts.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange = true);

        Task<Response> GetContract(Models.Contracts.ContractSearch searchModel, AdditionalFilter filter = null);

        Response GetContract(string companyCode, string userName, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true);

        Response GetContract(IList<string> contractNumbers);

        Response FetchCompanyContracts(Models.Contracts.BaseContract searchModel);

        Task<Response> GetBaseContract(Models.Contracts.ContractSearch searchModel);

        Response DeleteContract(IList<Models.Contracts.Contract> contracts, ref long? eventId, bool commitChange = true);

        Response DeleteContract(IList<Models.Contracts.Contract> model,  IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChange = true);

        Response GetContractInvoiceInfo(string companyCode = null, List<int> ContractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true);

        Response GetContractInvoiceInfo(List<string> contractNumber);

        Response GetContractBudgetDetails(string companyCode = null, string userName = null,ContractStatus contractStatus = ContractStatus.All,bool isAssignmentOnly = true);

        Response GetContractBasedOnCustomers(Models.Contracts.ContractSearch searchModel);

        Response GetContractBudgetDetails(IList<BudgetAccountItem> budgetAccountItems, IList<ContractExchangeRate> contractExchangeRates = null, IList<Models.Contracts.ContractWithId> contractWithIds = null, IList<ExchangeRate> currencyExchangeRates = null);

        Response GetContractBudgetAccountItems(IList<int> contractIds, string userName, bool isAssignmentOnly = true);

        Response ContractValidForDeletion(IList<Models.Contracts.Contract> contracts, ref IList<DbModel.Contract> dbContracts);

        Response GetApprovedVisitContracts(string customerCode, int ContractHolderCompanyId, bool isVisit, bool isNDT);

        Response GetUnApprovedVisitContracts(string customerCode, int? CoordinatorId, int ContractHolderCompanyId, bool isVisit, bool isOperating);

        Response GetvisitTimesheetKPIContracts(string customerCode, int ContractHolderCompanyId, bool isOperating, bool isVisit);
    }
}
