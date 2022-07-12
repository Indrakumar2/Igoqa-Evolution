using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyTaxRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyTax>
    {
        IList<DomainModel.Companies.CompanyTax> Search(DomainModel.Companies.CompanyTax model);
    }
}
