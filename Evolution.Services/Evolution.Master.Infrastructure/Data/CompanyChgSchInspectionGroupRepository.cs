using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class CompanyChgSchInspectionGroupRepository : GenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyChgSchInspGroup>, ICompanyChgSchInspectionGroupRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public CompanyChgSchInspectionGroupRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<Domain.Models.CompanyChgSchInspGroup> Search(Domain.Models.CompanyChgSchInspGroup search)
        {
            return this.FindBy(x=>(string.IsNullOrEmpty(search.CompanyCode)|| x.CompanyChargeSchedule.Company.Code==search.CompanyCode)
                               &&(string.IsNullOrEmpty(search.Name)||x.StandardInspectionGroup.Name==search.Name)
                               &&(string.IsNullOrEmpty(search.CompanyChargeScheduleName)||x.CompanyChargeSchedule.StandardChargeSchedule.Name==search.CompanyChargeScheduleName)
                               && (search.IsActive == null || x.StandardInspectionGroup.IsActive == search.IsActive))  //Changes for Defect 112
                                .Select(x => new Domain.Models.CompanyChgSchInspGroup()
                                {
                                    Name = x.StandardInspectionGroup.Name,
                                    CompanyCode = x.CompanyChargeSchedule.Company.Code,
                                    CompanyChargeScheduleName = x.CompanyChargeSchedule.StandardChargeSchedule.Name,
                                    IsActive=x.StandardInspectionGroup.IsActive, //Changes for Defect 112
                                    Id = x.Id
                                }).ToList();


        }
    }
}
