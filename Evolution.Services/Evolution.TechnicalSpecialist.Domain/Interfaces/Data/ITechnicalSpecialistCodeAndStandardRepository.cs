using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
  public interface ITechnicalSpecialistCodeAndStandardRepository : IGenericRepository<DbModel.TechnicalSpecialistCodeAndStandard>
    {
        IList<DbModel.TechnicalSpecialistCodeAndStandard> Search(TechnicalSpecialistCodeAndStandardinfo model);

        IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IList<int> CodeAndStandardIds);

        IList<DbModel.TechnicalSpecialistCodeAndStandard> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IList<string> CodeStandardName);

        IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IList<KeyValuePair<string, string>> ePinAndCodeStandardName);
    }
}
