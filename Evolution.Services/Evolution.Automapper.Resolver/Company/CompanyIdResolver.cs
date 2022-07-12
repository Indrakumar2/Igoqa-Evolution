using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Company
{
    public class CompanyIdResolver : IMemberValueResolver<object, object, string, int?>, IMemberValueResolver<object, object, string, int>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public CompanyIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.Company> dbCompanies = null;

            if (context.Options.Items.ContainsKey("dbCompany"))
                dbCompanies = ((List<DbModel.Company>)context.Options.Items["dbCompany"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbCompanies == null)
                    dbCompanies = this._dbContext.Company.Where(x => x.Name == sourceMember || x.Code == sourceMember).ToList();

                var dbModule = dbCompanies.Where(x => x.Name == sourceMember || x.Code == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.Id;
            }
            return null;
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

                var dbModule = dbCompanies.Where(x => x.Name == sourceMember || x.Code == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int);
        }
    }
}
