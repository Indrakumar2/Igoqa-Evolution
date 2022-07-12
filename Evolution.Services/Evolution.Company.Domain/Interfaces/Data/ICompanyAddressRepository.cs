using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to Company Document DB Model
    /// </summary>
    public interface ICompanyAddressRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyOffice>
    {
        IList<DomainModel.CompanyAddress> Search(DomainModel.CompanyAddress model);
    }
}
