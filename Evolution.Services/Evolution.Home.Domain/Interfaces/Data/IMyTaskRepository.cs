using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Home.Domain.Models.Homes;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Home.Domain.Interfaces.Data
{
    public interface IMyTaskRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Task>
    {
        IList<DomainModel.MyTask> Search(DomainModel.MyTask model);

        IList<DomainModel.MyTask> Search(string taskType, IList<string> taskRefCode = null, IList<string> assignedBy = null, IList<string> assignedTo = null);

        IList<DomainModel.MyTask> Search(IList<string> moduleType, IList<string> assignedTo, IList<string> assignedBy = null, IList<string> taskRefCode = null);
        //D946 CR Start
        void UpdateTechSpec(IList<DomainModel.MyTask> model, int? pendingWithId); //UAT 07-08Dec20 Doc Ref: Resource #2 Issue

        IList<DbModel.ResourceSearch> GetResourceSearchByIds(IList<int> resourceSearchIds);

        void UpdateResourceSearchOnReassign(IList<DbModel.ResourceSearch> resourceSearch);
    }
}
