using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class TechSpecIdResolver : IMemberValueResolver<DomainModel.OverridenPreferredResource, DbModel.OverrideResource, DomainModel.BaseResourceSearchTechSpecInfo, int>
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public TechSpecIdResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int Resolve(DomainModel.OverridenPreferredResource source, DbModel.OverrideResource destination, DomainModel.BaseResourceSearchTechSpecInfo sourceMember, int destMember, ResolutionContext context)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;

            if (context.Options.Items.ContainsKey("dbTechnicalSpecialist"))
                dbTechnicalSpecialist = ((List<DbModel.TechnicalSpecialist>)context.Options.Items["dbTechnicalSpecialist"]);

            if (sourceMember?.Epin > 0)
            {
                if (dbTechnicalSpecialist == null)
                    dbTechnicalSpecialist = this._dbContext.TechnicalSpecialist.Where(x => x.Pin== sourceMember.Epin).ToList();

                var dbModule = dbTechnicalSpecialist.FirstOrDefault(x => x.Pin == sourceMember.Epin);
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int);
        }
    }
}
