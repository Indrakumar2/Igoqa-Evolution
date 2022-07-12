using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.DbRepository.Services.Master
{
    public class CountyRepository : GenericRepository<County>, ICountyRepository
    {
        public CountyRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
