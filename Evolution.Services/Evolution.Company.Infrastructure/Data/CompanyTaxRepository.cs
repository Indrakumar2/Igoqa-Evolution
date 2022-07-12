using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyTaxRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyTax>, ICompanyTaxRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyTaxRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.Companies.CompanyTax> Search(DomainModel.Companies.CompanyTax searchModel)
        {  
            var dbSearchModel = this._mapper.Map<DbModel.CompanyTax>(searchModel);            
            IQueryable<DbModel.CompanyTax> whereClause = null;

            var compTaxes = _dbContext.CompanyTax;

            //Wildcard Search for Company Code
            if ( searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compTaxes.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compTaxes.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Tax
            if (searchModel.TaxName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Name, searchModel.TaxName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TaxName) || x.Name == searchModel.TaxName);

            //Wildcard Search for Tax Type
            if (searchModel.TaxType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.TaxType, searchModel.TaxType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TaxType) || x.TaxType == searchModel.TaxType);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Companies.CompanyTax>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Companies.CompanyTax>().ToList();
        }
    }
}
