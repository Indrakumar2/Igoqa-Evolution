using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class ServiceIdResolver :  IMemberValueResolver<object, object, string, int?>
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public ServiceIdResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.TaxonomyService> dbService = null;

            if (context.Options.Items.ContainsKey("dbService"))
                dbService = ((List<DbModel.TaxonomyService>)context.Options.Items["dbService"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbService == null)
                    dbService = this._dbContext.TaxonomyService.Where(x => x.TaxonomyServiceName == sourceMember).ToList();

                var dbModule = dbService.FirstOrDefault(x => x.TaxonomyServiceName == sourceMember);
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int?);
        }
    }
}
