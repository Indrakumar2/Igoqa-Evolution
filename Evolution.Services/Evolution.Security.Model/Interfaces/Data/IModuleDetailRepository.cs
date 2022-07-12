using Evolution.GenericDbRepository.Interfaces;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IModuleDetailRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.ModuleActivity>
    {
        IList<ModuleDetail> Search(ModuleInfo model);
    }
}
