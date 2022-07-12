using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyNoteRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyNote>
    {
        IList<DomainModel.CompanyNote> Search(DomainModel.CompanyNote searchModel);
    }
}
