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
    public class CompanyOfficeRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyOffice>, ICompanyAddressRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyOfficeRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.CompanyAddress> Search(DomainModel.CompanyAddress searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyOffice>(searchModel);
            IQueryable<DbModel.CompanyOffice> whereClause = null;

            var compOffice = _dbContext.CompanyOffice;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compOffice.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compOffice.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for City
            if (searchModel.City.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.City.Name, searchModel.City, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.City) || x.City.Name == searchModel.City);

            //Wildcard Search for State
            if (searchModel.State.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.City.County.Name, searchModel.State, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.State) || x.City.County.Name == searchModel.State);

            //Wildcard Search for Country
            if (searchModel.Country.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.City.County.Country.Name, searchModel.Country, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Country) || x.City.County.Country.Name == searchModel.Country);


            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyAddress>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyAddress>().ToList();
        }
    }
}
