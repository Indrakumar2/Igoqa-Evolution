using Evolution.Common.Enums;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.Master.Infrastructure.Data
{
    public class CompanyChgSchInspGrpInspectionTypeRepository : GenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyChgSchInspGrpInspectionType>, ICompanyChgSchInspGrpInspectionTypeRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        public CompanyChgSchInspGrpInspectionTypeRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<Domain.Models.CompanyChgSchInspGrpInspectionType> Search(Domain.Models.CompanyChgSchInspGrpInspectionType search)
        {
            return this.FindBy(x => (string.IsNullOrEmpty(search.CompanyCode) || x.CompanyChgSchInspGroup.CompanyChargeSchedule.Company.Code == search.CompanyCode)
                              && (string.IsNullOrEmpty(search.Name) || x.StandardInspectionType.Name== search.Name)
                              && (string.IsNullOrEmpty(search.CompanyChgSchInspectionGroupName) || x.CompanyChgSchInspGroup.StandardInspectionGroup.Name==search.CompanyChgSchInspectionGroupName)
                              && (string.IsNullOrEmpty(search.CompanyChgSchName) || x.CompanyChgSchInspGroup.CompanyChargeSchedule.StandardChargeSchedule.Name== search.CompanyChgSchName)
                              && x.StandardInspectionType.MasterDataTypeId == (int)MasterType.StandardInspectionType
                              && (search.IsActive == null || x.StandardInspectionType.IsActive == search.IsActive) //Changes for Defect 112
                              )
                               .Select(x => new Domain.Models.CompanyChgSchInspGrpInspectionType()
                               {
                                   Name = x.StandardInspectionType.Name,
                                   CompanyCode = x.CompanyChgSchInspGroup.CompanyChargeSchedule.Company.Code,
                                   CompanyChgSchInspectionGroupName = x.CompanyChgSchInspGroup.StandardInspectionGroup.Name,
                                   CompanyChgSchName= x.CompanyChgSchInspGroup.CompanyChargeSchedule.StandardChargeSchedule.Name,
                                   IsActive=x.StandardInspectionType.IsActive,//Changes for Defect 112
                                   Id = x.Id
                               }).ToList();
        }
    }
}
