using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Security.Infrastructure.Data
{
    public class RefreshTokenRepository : GenericRepository<DbModel.RefreshToken>,  IRefreshTokenRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null; 

        public RefreshTokenRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext; 
        }
    }
}
