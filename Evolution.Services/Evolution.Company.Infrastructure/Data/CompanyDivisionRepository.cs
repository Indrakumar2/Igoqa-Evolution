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
    public class CompanyDivisionRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyDivision>, ICompanyDivisionRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyDivisionRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.CompanyDivision> Search(Domain.Models.Companies.CompanyDivision searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyDivision>(searchModel);
            IQueryable<DbModel.CompanyDivision> whereClause = null;

            var compNotes = _dbContext.CompanyDivision;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compNotes.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compNotes.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Note
            if (searchModel.DivisionName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Division.Name, searchModel.DivisionName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.DivisionName) || x.Division.Name == searchModel.DivisionName);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyDivision>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyDivision>().ToList();
        }
    }
}