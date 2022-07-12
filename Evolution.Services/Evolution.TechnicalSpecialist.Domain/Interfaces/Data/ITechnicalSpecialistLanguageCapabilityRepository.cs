using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;


namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistLanguageCapabilityRepository : IGenericRepository<DbModel.TechnicalSpecialistLanguageCapability>
    {
       
        IList<DbModel.TechnicalSpecialistLanguageCapability> Search(TechnicalSpecialistLanguageCapabilityInfo model);

        IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IList<int> LanguageCapabilityIds);

        IList<DbModel.TechnicalSpecialistLanguageCapability> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IList<string> Language);

        IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IList<KeyValuePair<string, string>> ePinAndLanguage);
    }
}
