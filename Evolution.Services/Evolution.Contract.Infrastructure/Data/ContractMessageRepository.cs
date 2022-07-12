using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Infrastructure.Data
{
    public class ContractMessageRepository: GenericRepository<DbModel.ContractMessage>, IContractMessageRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;

        public ContractMessageRepository(DbModel.EvolutionSqlDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
