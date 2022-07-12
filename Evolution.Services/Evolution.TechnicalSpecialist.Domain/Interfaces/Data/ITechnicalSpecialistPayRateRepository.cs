using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
 
    public interface ITechnicalSpecialistPayRateRepository : IGenericRepository<DbModel.TechnicalSpecialistPayRate>
    {
        IList<DbModel.TechnicalSpecialistPayRate> Search(DomainModel.TechnicalSpecialistPayRateInfo model);

        IList<DbModel.TechnicalSpecialistPayRate> Get(IList<int> payRateIds);

        IList<DbModel.TechnicalSpecialistPayRate> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistPayRate> Get(IList<string> payScheduleName);

        IList<DbModel.TechnicalSpecialistPayRate> Get(IList<KeyValuePair<string, string>> ePinAndPayScheduleNames);
    }
}
