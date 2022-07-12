using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyEmailTemplateRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyMessage>, ICompanyEmailTemplateRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyEmailTemplateRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public DomainModel.CompanyEmailTemplate Search(string companyCode)
        {
            IQueryable<DbModel.Company> whereClause = null;
            var companyData = _dbContext.Company;

            //Wildcard Search for Company Code
            if (companyCode.HasEvoWildCardChar())
                whereClause = companyData.WhereLike(x => x.Code, companyCode, '*');
            else
                whereClause = companyData.Where(x => string.IsNullOrEmpty(companyCode) || x.Code == companyCode);

            var company = whereClause.FirstOrDefault();
            var companyMessages = company?.CompanyMessage.ToList();
            var companyEmailDetail = _mapper.Map<DomainModel.CompanyEmailTemplate>(companyMessages);
            companyEmailDetail.CompanyCode = company?.Code;
            return companyEmailDetail; 
        }
    }
}

