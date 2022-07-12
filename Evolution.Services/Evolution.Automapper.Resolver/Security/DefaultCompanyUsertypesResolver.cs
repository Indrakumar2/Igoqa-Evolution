using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Security
{
   public class DefaultCompanyUsertypesResolver : IMemberValueResolver<DbModel.User, object, ICollection<DbModel.UserType>, IList<string>>
    { 
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public DefaultCompanyUsertypesResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<string> Resolve(DbModel.User source, object destination, ICollection<DbModel.UserType> sourceMember, IList<string> destMember, ResolutionContext context)
        {
            IList<DbModel.UserType> dbUserTypes = null;
            IList<string> userTypes = null;

            if (context.Options.Items.ContainsKey("dbCompanyUserType"))
                dbUserTypes = ((List<DbModel.UserType>)context.Options.Items["dbCompanyUserType"]);
             
            if (dbUserTypes == null)
                dbUserTypes = this._dbContext.UserType.Where(x => x.CompanyId == source.CompanyId && x.User.SamaccountName==source.SamaccountName).ToList();

            if (dbUserTypes?.Count > 0)
            {
                userTypes = dbUserTypes.Where(x => x.CompanyId == source.CompanyId && x.User.SamaccountName == source.SamaccountName).Select(x => x.UserTypeName).ToList();

            }
            return userTypes;
        }
    }
}
