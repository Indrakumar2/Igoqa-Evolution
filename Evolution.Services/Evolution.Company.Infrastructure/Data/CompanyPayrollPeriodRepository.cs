using Evolution.Company.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Evolution.GenericDbRepository.Services;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using Evolution.Common.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyPayrollPeriodRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyPayrollPeriod>, ICompanyPayrollPeriodRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyPayrollPeriodRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.CompanyPayrollPeriod> Search( DomainModel.CompanyPayrollPeriod searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyPayrollPeriod>(searchModel);
            IQueryable<DbModel.CompanyPayrollPeriod> whereClause = null;

            var compDocs = _dbContext.CompanyPayrollPeriod;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compDocs.WhereLike(x => x.CompanyPayroll.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compDocs.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.CompanyPayroll.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Payroll Type
            if (searchModel.PayrollType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CompanyPayroll.Name, searchModel.CompanyCode, '*');//Defect 45 Changes
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.PayrollType) || x.CompanyPayroll.Name == searchModel.PayrollType);//Defect 45 Changes
            
           //whereClause = whereClause.Where(x => x.IsActive==null || x.IsActive==true); //DEF 499
            whereClause = whereClause.Where(x => x.IsActive != null);      //ITK D-727

            var expression = dbSearchModel.ToExpression(new List<string> { "IsActive" });
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyPayrollPeriod>().OrderByDescending(x=>x.StartDate).ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyPayrollPeriod>().OrderByDescending(x => x.StartDate).ToList();
        }
    }
}
