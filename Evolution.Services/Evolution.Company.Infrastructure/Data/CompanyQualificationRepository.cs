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
    public class CompanyQualificationRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyQualificationType>, ICompanyQualificationRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyQualificationRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.CompanyQualification> Search(DomainModel.CompanyQualification searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyQualificationType>(searchModel);
            IQueryable<DbModel.CompanyQualificationType> whereClause = null;

            //var compQualification = _dbContext.CompanyQualificationType;

            ////Wildcard Search for Company Code
            //if (searchModel.CompanyCode.HasEvoWildCardChar())
            //    whereClause = compQualification.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            //else
            //    whereClause = compQualification.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Qualification
            if (searchModel.Qualification.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.QualificationType.Name, searchModel.Qualification, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Qualification) || x.QualificationType.Name == searchModel.Qualification);

           
            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CompanyQualification>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CompanyQualification>().ToList();
        }
    }
}
