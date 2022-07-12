using Evolution.Common.Enums;
using Evolution.Common.Models.Budget;
using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractRepository: IGenericRepository<DbModel.Contract>
    {
        IList<DomainModel.Contract> Search(DomainModel.ContractSearch searchModel);

        IList<DomainModel.BaseContract> SearchBaseContract(DomainModel.ContractSearch searchModel);

        int GetCount(DomainModel.ContractSearch model);

        IList<BudgetAccountItem> GetBudgetAccountItemDetails(string companyCode = null, List<int> contractIds=null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true);

        int DeleteContract(int contractId, string contractNum);

        int DeleteContract(IList<DbModel.Contract> contracts);

        int DeleteContract(List<int> contractIds);

        List<string> GetCustomerContractNumbers(int? customerId);

        bool CheckUseExchange(DomainModel.Contract contract,DbModel.Contract dbContract, ValidationType validationType);

        IList<DomainModel.BaseContract> FetchCompanyContract(DomainModel.BaseContract searchModel);

        string ValidateProjectBudget(DomainModel.Contract contract); //Added for D-1304 

        //Added for reports
        IList<DomainModel.ContractWithId> GetApprovedContractByCustomer(string customerCode, int ContractHolderCompanyId, bool isVisit, bool isNDT);

        IList<DomainModel.ContractWithId> GetUnApprovedContractByCustomer(string customerCode, int? CoordinatorId, int ContractHolderCompanyId, bool isVisit, bool isOperating);

        IList<DomainModel.ContractWithId> GetvisitTimesheetKPIContracts(string customerCode, int ContractHolderCompanyId, bool isOperating, bool isVisit);

    }
}