using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
   public interface ITechnicalSpecialistStampInfoRepository : IGenericRepository<DbModel.TechnicalSpecialistStamp>
    {
        IList<DbModel.TechnicalSpecialistStamp> Search(DomainModel.TechnicalSpecialistStampInfo model);

        IList<DbModel.TechnicalSpecialistStamp> Get(IList<int> stampIds);

        IList<DbModel.TechnicalSpecialistStamp> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistStamp> Get(IList<string> stampNumbers);

        IList<DbModel.TechnicalSpecialistStamp> Get(IList<KeyValuePair<string,string>> ePinAndStampNumbers);
    }
}