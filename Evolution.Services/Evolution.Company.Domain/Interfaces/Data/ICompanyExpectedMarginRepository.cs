using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyExpectedMarginRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyExpectedMargin>
    {
        IList<DomainModel.CompanyExpectedMargin> Search(DomainModel.CompanyExpectedMargin searchModel);
    }
}
