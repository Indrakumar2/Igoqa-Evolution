using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Document.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Document.Infrastructure.Data
{
    public class DocumentMongoSyncRepository : GenericRepository<DbModel.DocumentMongoSync>, IDocumentMongoSyncRepository
    {
        private EvolutionSqlDbContext _dbContext = null;

        public DocumentMongoSyncRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
