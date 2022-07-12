using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Company
{
    public class CompanyOfficeIdResolver : IMemberValueResolver<object, object, string, int?>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public CompanyOfficeIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.CompanyOffice> dbCompanyOffices = null;
            if (context.Options.Items.ContainsKey("dbCompanyOffice"))
                dbCompanyOffices = ((List<DbModel.CompanyOffice>)context.Options.Items["dbCompanyOffice"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbCompanyOffices == null)
                    dbCompanyOffices = this._dbContext.CompanyOffice.Where(x => x.OfficeName.Trim() == sourceMember.Trim()).ToList();

                var dbCompOffice = dbCompanyOffices?.FirstOrDefault(x => x.OfficeName.Trim() == sourceMember.Trim());
                if (dbCompOffice != null)
                    return dbCompOffice.Id;
            }
            return null;
        }
    }
}
