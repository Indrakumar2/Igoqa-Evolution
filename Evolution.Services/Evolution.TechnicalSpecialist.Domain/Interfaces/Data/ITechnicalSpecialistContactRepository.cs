using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistContactRepository : IGenericRepository<DbModel.TechnicalSpecialistContact>
    {
        IList<DbModel.TechnicalSpecialistContact> Search(DomainModel.TechnicalSpecialistContactInfo model);

        IList<DbModel.TechnicalSpecialistContact> Search(DomainModel.TechnicalSpecialistContactInfo model, int takeCount);

        IList<DbModel.TechnicalSpecialistContact> Get(IList<int> contactIds);

        IList<DbModel.TechnicalSpecialistContact> GetByPinId(IList<string> pinIds);

        bool UpdateContactSyncStatus(IList<int> tsContactIds);
    }
   
}
