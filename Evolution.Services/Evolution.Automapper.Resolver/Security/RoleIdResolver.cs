using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Security
{
    public class RoleIdResolver : IMemberValueResolver<object, object, string, int>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public RoleIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            IList<DbModel.Role> dbRole = null;

            if (context.Options.Items.ContainsKey("dbRole"))
                dbRole = ((List<DbModel.Role>)context.Options.Items["dbRole"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbRole == null)
                    dbRole = this._dbContext.Role.Where(x => x.Name == sourceMember).ToList();

                var role = dbRole.Where(x => x.Name == sourceMember).FirstOrDefault();
                if (role != null)
                    return role.Id;
            }
            return 0;
        }
    }
}
