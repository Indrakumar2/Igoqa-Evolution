using Evolution.GenericDbRepository.Interfaces;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using System;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyDivisionRepository : IGenericRepository<DbModel.CompanyDivision>
    {
        IList<DomainModel.CompanyDivision> Search(DomainModel.CompanyDivision searchModel);        
    }
}
