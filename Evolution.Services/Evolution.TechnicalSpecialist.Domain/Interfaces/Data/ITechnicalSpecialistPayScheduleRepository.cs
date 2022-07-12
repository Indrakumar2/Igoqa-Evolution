using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
   public interface ITechnicalSpecialistPayScheduleRepository: IGenericRepository<DbModel.TechnicalSpecialistPaySchedule>
    {
        IList<DbModel.TechnicalSpecialistPaySchedule> Search(DomainModel.TechnicalSpecialistPayScheduleInfo model);

        IList<DbModel.TechnicalSpecialistPaySchedule> Get(IList<int> payscheduleIds);

        IList<DbModel.TechnicalSpecialistPaySchedule> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistPaySchedule> Get(IList<string> payScheduleName);

        IList<DbModel.TechnicalSpecialistPaySchedule> Get(IList<KeyValuePair<string, string>> ePinAndPayScheduleNames);
    }
}
