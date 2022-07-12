using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyPayrollRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyPayroll>, ICompanyPayrollRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyPayrollRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        
        public IList<DomainModel.CompanyPayroll> Search(DomainModel.CompanyPayroll searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyPayroll>(searchModel);
            IQueryable<DbModel.CompanyPayroll> whereClause = null;

            var compDocs = _dbContext.CompanyPayroll;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compDocs.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compDocs.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Payroll Type
            if (searchModel.PayrollType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Name, searchModel.PayrollType, '*');//Defect 45 Changes
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.PayrollType) || x.Name == searchModel.PayrollType);//Defect 45 Changes

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyPayroll>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyPayroll>().ToList();
        }
    }
}
