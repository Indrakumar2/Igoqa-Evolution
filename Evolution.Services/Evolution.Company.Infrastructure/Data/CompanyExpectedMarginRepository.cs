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
    public class CompanyExpectedMarginRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyExpectedMargin>, ICompanyExpectedMarginRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyExpectedMarginRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.CompanyExpectedMargin> Search(DomainModel.CompanyExpectedMargin searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyExpectedMargin>(searchModel);
            IQueryable<DbModel.CompanyExpectedMargin> whereClause = null;

            var compExpectedMargin = _dbContext.CompanyExpectedMargin;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compExpectedMargin.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compExpectedMargin.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);
                      
            //Wildcard Search for Margin Type
            if (searchModel.MarginType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.MarginType.Name, searchModel.MarginType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.MarginType) || x.MarginType.Name == searchModel.MarginType);
            
            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyExpectedMargin>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyExpectedMargin>().ToList();
        }
    }
}
