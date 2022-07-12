using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Company
{
    public class TsCompanyIdResolver : IMemberValueResolver<object, object, int, int?>, IMemberValueResolver<object, object, int, int>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public TsCompanyIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, int sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;

            if (context.Options.Items.ContainsKey("dbTechnicalSpecialists"))
                dbTechnicalSpecialists = ((List<DbModel.TechnicalSpecialist>)context.Options.Items["dbTechnicalSpecialists"]);

            if (sourceMember>0)
            {
                if (dbTechnicalSpecialists == null)
                    dbTechnicalSpecialists = this._dbContext.TechnicalSpecialist.Where(x => x.Id == sourceMember).ToList();

                var dbModule = dbTechnicalSpecialists.Where(x => x.Id == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.CompanyId;
            }
            return null;
        }

        public int Resolve(object source, object destination, int sourceMember, int destMember, ResolutionContext context)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;

            if (context.Options.Items.ContainsKey("dbTechnicalSpecialists"))
                dbTechnicalSpecialists = ((List<DbModel.TechnicalSpecialist>)context.Options.Items["dbTechnicalSpecialists"]);

            if (sourceMember > 0)
            {
                if (dbTechnicalSpecialists == null)
                    dbTechnicalSpecialists = this._dbContext.TechnicalSpecialist.Where(x => x.Id == sourceMember).ToList();

                var dbModule = dbTechnicalSpecialists.Where(x => x.Id == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.CompanyId;
            }
            return default(int);
        }
    }
}
