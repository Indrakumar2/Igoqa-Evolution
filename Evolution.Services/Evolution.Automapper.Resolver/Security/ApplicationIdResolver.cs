using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Security
{
    public class ApplicationIdResolver : IMemberValueResolver<object, object, string, int>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public ApplicationIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            IList<DbModel.Application> dbApplications = null;

            if (context.Options.Items.ContainsKey("dbApplication"))
                dbApplications = ((List<DbModel.Application>)context.Options.Items["dbApplication"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbApplications == null)
                    dbApplications = this._dbContext.Application.Where(x => x.Name == sourceMember).ToList();

                var dbModule = dbApplications.Where(x => x.Name == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.Id;
            }
            return 0;
        }
    }
}
