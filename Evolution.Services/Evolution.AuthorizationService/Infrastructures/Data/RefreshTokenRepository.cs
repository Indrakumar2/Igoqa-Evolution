using Evolution.AuthorizationService.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.AuthorizationService.Infrastructures.Data
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenReposiotry
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public RefreshTokenRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
