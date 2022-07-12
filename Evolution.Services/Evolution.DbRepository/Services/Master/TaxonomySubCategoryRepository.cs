using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.DbRepository.Services.Master
{
    public class TaxonomySubCategoryRepository : GenericRepository<TaxonomySubCategory>, ITaxonomySubCategoryRepository
    {

        public TaxonomySubCategoryRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
