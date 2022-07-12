using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class CompanyIdResolver : IMemberValueResolver<object, object, string, int>
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public CompanyIdResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            IList<DbModel.Company> dbCompanies = null;

            if (context.Options.Items.ContainsKey("dbCompany"))
                dbCompanies = ((List<DbModel.Company>)context.Options.Items["dbCompany"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbCompanies == null)
                    dbCompanies = this._dbContext.Company.Where(x => x.Name == sourceMember || x.Code == sourceMember).ToList();

                var dbModule = dbCompanies.FirstOrDefault(x => x.Name == sourceMember || x.Code == sourceMember);
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int);
        }
    }
}
