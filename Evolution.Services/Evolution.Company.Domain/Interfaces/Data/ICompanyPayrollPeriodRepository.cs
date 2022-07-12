using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Domain.Interfaces.Data
{   
    public interface ICompanyPayrollPeriodRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyPayrollPeriod>
    {
        IList<DomainModel.Companies.CompanyPayrollPeriod> Search(DomainModel.Companies.CompanyPayrollPeriod model);
    }
}
