using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class CompanyChargeScheduleRepository:GenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyChargeSchedule>,ICompanyChargeScheduleRepository
   {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public CompanyChargeScheduleRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<Domain.Models.CompanyChargeSchedule> Search(Domain.Models.CompanyChargeSchedule search)
        {
            return this.FindBy(x => (String.IsNullOrEmpty(search.CompanyCode) || x.Company.Code == search.CompanyCode)
                                          && (string.IsNullOrEmpty(search.Name) || x.StandardChargeSchedule.Name == search.Name)
                                          && (string.IsNullOrEmpty(search.Currency) || x.Currency == search.Currency)
                                          && (search.IsActive == null || x.StandardChargeSchedule.IsActive == search.IsActive))   //Changes for Defect 112
                                        
                                          .Select(x => new Domain.Models.CompanyChargeSchedule() {
                                                        Name = x.StandardChargeSchedule.Name,
                                                       CompanyCode = x.Company.Code,
                                                       IsActive =x.StandardChargeSchedule.IsActive,   //Changes for Defect 112
                                                       Currency=x.Currency,Id=x.Id}).OrderBy(x1=>x1.Name).ToList();
        }
    }
}
