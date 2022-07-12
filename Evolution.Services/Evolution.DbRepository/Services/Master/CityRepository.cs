using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.DbRepository.Services.Master
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
