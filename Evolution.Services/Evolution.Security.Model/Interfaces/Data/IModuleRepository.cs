using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Security.Domain.Models;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IModuleRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Module>
    {
        IList<Module> Search(DomainModel.Security.ModuleInfo model);

        IList<Module> Get(IList<int> ids);

        IList<Module> Get(IList<string> names);
    }
}
