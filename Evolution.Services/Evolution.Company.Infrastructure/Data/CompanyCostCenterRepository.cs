using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyDivisionCOCRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyDivisionCostCenter>, ICompanyCostCenterRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyDivisionCOCRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<Domain.Models.Companies.CompanyDivisionCostCenter> Search(DomainModel.CompanyDivisionCostCenter searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyDivisionCostCenter>(searchModel);
            IQueryable<DbModel.CompanyDivisionCostCenter> whereClause = null;

            var divisionCOC = _dbContext.CompanyDivisionCostCenter;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = divisionCOC.WhereLike(x => x.CompanyDivision.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = divisionCOC.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.CompanyDivision.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Note
            if (searchModel.Division.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CompanyDivision.Division.Name, searchModel.Division, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Division) || x.CompanyDivision.Division.Name == searchModel.Division);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyDivisionCostCenter>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyDivisionCostCenter>().ToList();
        }

        public IList<DomainModel.CompanyDivisionCostCenter> Create(string companyCode, string divsionName, IList<DomainModel.CompanyDivisionCostCenter> companyDivisionCostCenters)
        {
            List<DbModel.CompanyDivisionCostCenter> divisionCostCenters = new List<DbModel.CompanyDivisionCostCenter>();

            int divisionId = GetDivisionId(companyCode, divsionName);
            if (divisionId != 0)
            {
                var dbCostCenters = companyDivisionCostCenters.AsQueryable().ProjectTo<DbModel.CompanyDivisionCostCenter>().ToList();

                dbCostCenters.ForEach(x =>
                {
                    x.CompanyDivisionId = divisionId;
                    _dbContext.CompanyDivisionCostCenter.Add(x);
                    _dbContext.SaveChanges();
                    divisionCostCenters.Add(x);
                });
                return divisionCostCenters.AsQueryable().ProjectTo<DomainModel.CompanyDivisionCostCenter>().ToList();
            }
            return null;


        }

        public IList<DomainModel.CompanyDivisionCostCenter> Modify(string companyCode, string divisionName, IList<DomainModel.CompanyDivisionCostCenter> companyDivisionCostCenters)
        {
            int divisionId = GetDivisionId(companyCode, divisionName);
            IList<DomainModel.CompanyDivisionCostCenter> result = new List<DomainModel.CompanyDivisionCostCenter>();
            foreach (DomainModel.CompanyDivisionCostCenter costCenter in companyDivisionCostCenters)
            {
                var dbCostCenter = _dbContext.CompanyDivisionCostCenter.Where(x => x.Id == costCenter.CompanyDivisionCostCenterId).FirstOrDefault();
                if(dbCostCenter!= null)
                {
                    dbCostCenter.Name = costCenter.CostCenterName;
                    dbCostCenter.Code = costCenter.CostCenterCode;
                    dbCostCenter.LastModification = DateTime.UtcNow;
                    dbCostCenter.UpdateCount = Convert.ToByte(dbCostCenter.UpdateCount == null ? 0 : dbCostCenter.UpdateCount + 1);
                    _dbContext.CompanyDivisionCostCenter.Update(dbCostCenter);
                    _dbContext.SaveChanges();
                    result.Add(_mapper.Map<DomainModel.CompanyDivisionCostCenter>(dbCostCenter));
                    return result;
                }
               
            }
            return null;
        }

        private int GetDivisionId(string companyCode, string divisionName)
        {
            var division = _dbContext.CompanyDivision.Where(x => x.Division.Name == divisionName && x.Company.Code == companyCode).FirstOrDefault();
            return division != null ? division.Id : 0;
        }
    }
}
