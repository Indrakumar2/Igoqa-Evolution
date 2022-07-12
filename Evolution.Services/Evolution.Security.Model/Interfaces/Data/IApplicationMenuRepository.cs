using Evolution.GenericDbRepository.Interfaces;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IApplicationMenuRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.ApplicationMenu>
    {
        IList<ApplicationMenuInfo> Search(string applicationName);
    }
}
