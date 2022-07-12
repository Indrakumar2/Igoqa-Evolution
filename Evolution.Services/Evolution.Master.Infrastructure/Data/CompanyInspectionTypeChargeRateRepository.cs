using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class CompanyInspectionTypeChargeRateRepository : GenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyInspectionTypeChargeRate>, ICompanyInspectionTypeChargeRateRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public CompanyInspectionTypeChargeRateRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper =mapper;
        }

        public IList<Domain.Models.CompanyInspectionTypeChargeRate> Search(Domain.Models.CompanyInspectionTypeChargeRate search)
        {
            return this.FindBy(x => (string.IsNullOrEmpty(search.CompanyCode) || x.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.CompanyChargeSchedule.Company.Code == search.CompanyCode)
                               && (string.IsNullOrEmpty(search.Name) || x.ItemDescription == search.Name)
                               && (string.IsNullOrEmpty(search.CompanyChargeScheduleName) || x.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.CompanyChargeSchedule.StandardChargeSchedule.Name == search.CompanyChargeScheduleName)
                               && (string.IsNullOrEmpty(search.CompanyChgSchInspGroupName) || x.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.StandardInspectionGroup.Name == search.CompanyChgSchInspGroupName)
                               && (string.IsNullOrEmpty(search.CompanyChgSchInspGrpInspectionTypeName) ||x.CompanyChgSchInspGrpInspectionType.StandardInspectionType.Name==search.CompanyChgSchInspGrpInspectionTypeName )
                               && (string.IsNullOrEmpty(search.ItemSize) || x.ItemSize.Name == search.ItemSize)
                               && (string.IsNullOrEmpty(search.ItemThickness) || x.ItemThickness.Name == search.ItemThickness)
                               && (string.IsNullOrEmpty(search.FilmSize) || x.FilmSize.Name == search.FilmSize)
                               &&(string.IsNullOrEmpty(search.FilmType) || x.FilmType.Name == search.FilmType)
                               && (string.IsNullOrEmpty(search.ExpenseType) || x.ExpenseType.Name == search.ExpenseType))
                                .Select(x =>
                                new Domain.Models.CompanyInspectionTypeChargeRate()
                                {
                                    Name = x.ItemDescription,
                                    CompanyCode = x.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.CompanyChargeSchedule.Company.Code,
                                    CompanyChgSchInspGrpInspectionTypeName = x.CompanyChgSchInspGrpInspectionType.StandardInspectionType.Name,
                                    ItemSize = x.ItemSize.Name,
                                    ItemThickness = x.ItemThickness.Name,
                                    FilmSize = x.FilmSize.Name,
                                    FilmType = x.FilmType.Name,
                                    ExpenseType=x.ExpenseType.Name,
                                    RateOffShoreOil=x.RateOffShoreOil.ToString(),
                                    RateOnShoreOil=x.RateOnShoreOil.ToString(),
                                    RateOnShoreNonOil=x.RateOnShoreNonOil.ToString(),
                                    IsRateOffShoreOil=x.IsRateOffShoreOil.ToString(),
                                    IsRateOnShoreNonOil=x.IsRateOnShoreNonOil.ToString(),
                                    IsRateOnShoreOil=x.IsRateOnShoreOil.ToString(),
                                    Id = x.Id,
                                    LastModification=x.LastModification,
                                    UpdateCount=x.UpdateCount,
                                    ModifiedBy=x.ModifiedBy,
                                }

                                ).ToList();


        }
    }
   
    
}
