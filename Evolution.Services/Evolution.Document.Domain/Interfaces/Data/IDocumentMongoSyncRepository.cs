using Evolution.GenericDbRepository.Interfaces;

namespace Evolution.Document.Domain.Interfaces.Data
{
    public interface IDocumentMongoSyncRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.DocumentMongoSync>
    {
        //IList<ModuleDocument> Get(ModuleDocument model);
    }
}
