using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.DbRepository.Services.Master
{
    public class TechnicalSpecialistCustomerRepository : GenericRepository<TechnicalSpecialistCustomers>, ITechnicalSpecialistCustomerRepository
    {
        public TechnicalSpecialistCustomerRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
