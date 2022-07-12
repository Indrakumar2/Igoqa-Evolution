using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class SubCategoryIdResolver : IMemberValueResolver<object, object, string, int?>
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public SubCategoryIdResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.TaxonomySubCategory> dbSubCategory = null;

            if (context.Options.Items.ContainsKey("dbSubCategory"))
                dbSubCategory = ((List<DbModel.TaxonomySubCategory>)context.Options.Items["dbSubCategory"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbSubCategory == null)
                    dbSubCategory = this._dbContext.TaxonomySubCategory.Where(x => x.TaxonomySubCategoryName == sourceMember).ToList();

                var dbModule = dbSubCategory.Where(x => x.TaxonomySubCategoryName == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int?);
        }
    }
}
