using Evolution.AuthorizationService.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.AuthorizationService.Infrastructures.Data
{
    public class AuthClientRepository : GenericRepository<Client>, IAuthClientRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public AuthClientRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
