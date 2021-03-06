using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.DbRepository.Services.Master
{
    public   class CustomerCommodityRepository : GenericRepository<CustomerCommodity>, ICustomerCommodityRepository
    {

        public CustomerCommodityRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
   
    }
}
