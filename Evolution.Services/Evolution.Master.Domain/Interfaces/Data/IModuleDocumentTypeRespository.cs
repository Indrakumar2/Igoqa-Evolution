using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Master.Domain.Models;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface IModuleDocumentTypeRepository : IGenericRepository<DbRepository.Models.SqlDatabaseContext.ModuleDocumentType>
    {
        IList<Models.ModuleDocumentType> Search(Models.ModuleDocumentType search);
    }
}
