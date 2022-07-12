using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyPayrollRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyPayroll>
    {
        IList<DomainModel.Companies.CompanyPayroll> Search(DomainModel.Companies.CompanyPayroll model);
    }
}
