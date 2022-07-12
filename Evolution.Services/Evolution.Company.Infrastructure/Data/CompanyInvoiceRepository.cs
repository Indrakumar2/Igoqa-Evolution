using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyInvoiceRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyMessage>, ICompanyInvoiceRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyInvoiceRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public DomainModel.CompanyInvoice Search(string companyCode)
        {
            var companyMessages = _dbContext.CompanyMessage.Include(x=>x.MessageType)
            .Where(x => x.Company.Code == companyCode).ToList();
            if (companyMessages?.Count > 0)
            {
                var companyInvoiceDetail = _mapper.Map<DomainModel.CompanyInvoice>(companyMessages);
                companyInvoiceDetail.CompanyCode = companyCode;
                //companyInvoiceDetail.InvoiceCompanyName = companyMessages.FirstOrDefault()?.Company.InvoiceCompanyName;
                return companyInvoiceDetail;
            }
            else
                return null;
        }
    }
}

