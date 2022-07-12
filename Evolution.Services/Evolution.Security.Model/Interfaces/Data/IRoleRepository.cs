using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Security.Domain.Models;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IRoleRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Role>
    {
        IList<Role> Search(DomainModel.Security.RoleInfo model);

        IList<Role> Get(IList<int> ids);

        IList<Role> Get(IList<string> names);
    }
}