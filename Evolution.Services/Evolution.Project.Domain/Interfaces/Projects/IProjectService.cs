using Evolution.Common.Enums;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Domain.Interfaces.Data
{
    public interface IProjectService
    {
        Response SaveProjects(IList<Models.Projects.Project> projects, ref long? eventId, bool commitChange = true);

        Response SaveProjects(IList<Models.Projects.Project> projects, ref IList<DbModel.Project> dbProject, ref IList<DbModel.Data> dbProjectReference, ref long? eventId, ref IList<DbModel.SqlauditModule> dbModule, bool commitChange = true);

        Task<Response> GetProjects(Models.Projects.ProjectSearch searchModel, AdditionalFilter filter = null);

        Response GetProjectBasedOnStatus(string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isVisit, bool isOperating, bool isNDT, int? CoordinatorId);

        Response GetProjectKPI(string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating);

        Task<Response> SearchProjects(Models.Projects.ProjectSearch searchModel);

        Response GetProjects(int projectNumber);

        Response GetProjectBudgetDetails(string companyCode, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true);

        Response GetProjectBudgetDetails(IList<BudgetAccountItem> budgetAccountItems,IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null);

        Response GetProjectInvoiceInfo(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true);

        Response GetProjectInvoiceInfo(List<string> contractNumber = null, List<int> projectNumber = null,bool IsProjectFetchRequired = true);

        Response ModifyProjects(IList<Models.Projects.Project> projects, ref long? eventId, bool commitChange = true);

        Response ModifyProjects(IList<Models.Projects.Project> projects, ref IList<DbModel.Project> dbProjects,ref IList<DbModel.Data> dbProjectRef, ref long? eventId, ref IList<DbModel.SqlauditModule> dbModule, bool commitChange = true);

        Response DeleteProjects(IList<Models.Projects.Project> projects, ref long? eventId, bool commitChange = true);

        Response DeleteProjects(IList<Models.Projects.Project> projects, ref long? eventId,  IList<DbModel.SqlauditModule> dbModule, bool commitChange = true);

        Response ProjectValidForDeletion(IList<Models.Projects.Project> projects, ref IList<DbModel.Project> dbProjects);

        bool IsValidProjectNumber(IList<int> projectNumber, ref IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> messages, params Expression<Func<DbModel.Project, object>>[] includes);

        bool IsValidProjectNumber(IList<int> projectNumber, ref IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> messages,string[] includes);
    }
}
