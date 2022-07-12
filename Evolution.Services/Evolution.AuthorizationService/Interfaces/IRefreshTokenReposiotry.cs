using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuthorizationService.Interfaces
{
    public interface IRefreshTokenReposiotry : IGenericRepository<RefreshToken>
    {

    }
}
