using Evolution.Common.Models.Messages;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Data
{
    public interface IProjectRepository : IGenericRepository<DBModel.Project>
    {
        IList<DomainModel.Project> Search(DomainModel.ProjectSearch searchModel);

        IList<DomainModel.ProjectSearch> SearchProject(DomainModel.ProjectSearch searchModel);

        int GetCount(DomainModel.ProjectSearch searchModel);

        int DeleteProject(int projectId);

        int DeleteProject(IList<DBModel.Project> projects);

        int DeleteProject(List<int> projectIds);

        List<int> GetContractProjectIds(string contarctNumber);

        List<int> GetCustomerProjectIds(int? customerId);

        IList<DomainModel.Project> GetProjects(int projectNumber);

        List<DBModel.SystemSetting> MailTemplate();

        bool GetTsVisible();

        string ValidateContractBudget(DomainModel.Project projects, DBModel.Contract dbContracts); //Added for D-1304
        
        string ValidateAssignmentBudget(DomainModel.Project projects); //Added for D-1304 

        List<DomainModel.Project> GetProjectBasedOnStatus(string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isVisit, bool isOperating, bool isNDT, int? CoordinatorId);

        IList<DomainModel.Project> GetProjectKPI(string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating);
    }
}
