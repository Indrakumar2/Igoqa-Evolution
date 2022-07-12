using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Security
{
    public class UserIdResolver : IMemberValueResolver<object, object, string, int>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public UserIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            IList<DbModel.User> dbApplications = null;

            if (context.Options.Items.ContainsKey("dbUser"))
                dbApplications = ((List<DbModel.User>)context.Options.Items["dbUser"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbApplications == null)
                    dbApplications = this._dbContext.User.Where(x => x.SamaccountName == sourceMember).ToList();

                var dbUser = dbApplications.Where(x => x.SamaccountName == sourceMember).FirstOrDefault();
                if (dbUser != null)
                    return dbUser.Id;
            }
            return 0;
        }
    }
}
