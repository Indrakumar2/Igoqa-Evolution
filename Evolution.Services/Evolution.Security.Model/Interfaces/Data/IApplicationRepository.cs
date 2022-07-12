using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IApplicationRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Application>
    {
        IList<Application> Get(IList<int> ids);
        IList<Application> Get(IList<string> names);        
    }
}
