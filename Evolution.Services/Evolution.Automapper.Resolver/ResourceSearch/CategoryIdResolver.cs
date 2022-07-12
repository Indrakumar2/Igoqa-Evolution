using AutoMapper;
using Evolution.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class CategoryIdResolver : IMemberValueResolver<object, object, string, int?>
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public CategoryIdResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.Data> dbCategory = null;

            if (context.Options.Items.ContainsKey("dbCategory"))
                dbCategory = ((List<DbModel.Data>)context.Options.Items["dbCategory"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbCategory == null)
                    dbCategory = this._dbContext.Data.Where(x => x.Name == sourceMember && x.MasterDataTypeId == (int)MasterType.TaxonomyCategory).ToList();

                var dbModule = dbCategory.FirstOrDefault(x => x.Name == sourceMember);
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int?);
        }
    }
}
