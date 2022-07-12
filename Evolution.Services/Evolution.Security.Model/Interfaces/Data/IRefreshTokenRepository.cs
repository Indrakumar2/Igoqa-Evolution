using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    { 

    }
}
