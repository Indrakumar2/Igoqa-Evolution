using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyRepository : IGenericRepository<DbModel.Company>
    {
        IList<DomainModel.Companies.Company> Search(DomainModel.Companies.CompanySearch model);

        IList<DomainModel.Companies.Company> GetCompanyList(DomainModel.Companies.CompanySearch model);
    }
}
