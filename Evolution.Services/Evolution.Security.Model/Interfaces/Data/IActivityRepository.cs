using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Security.Domain.Models;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IActivityRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Activity>
    {
        IList<Activity> Search(DomainModel.Security.ActivityInfo model);

        IList<Activity> Get(IList<int> ids);

        IList<Activity> Get(IList<string> names);
    }
}
