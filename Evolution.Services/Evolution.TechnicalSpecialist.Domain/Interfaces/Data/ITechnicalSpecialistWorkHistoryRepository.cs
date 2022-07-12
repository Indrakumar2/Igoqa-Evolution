using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistWorkHistoryRepository : IGenericRepository<DbModel.TechnicalSpecialistWorkHistory>
    {
      

        IList<DbModel.TechnicalSpecialistWorkHistory> Search(TechnicalSpecialistWorkHistoryInfo model);

        IList<DbModel.TechnicalSpecialistWorkHistory> Get(IList<int> WorkHistoryIds);

        IList<DbModel.TechnicalSpecialistWorkHistory> GetByPinId(IList<string> pinIds);
    }
}