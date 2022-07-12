using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository : IGenericRepository<DbModel.TechnicalSpecialistComputerElectronicKnowledge>
    {
        IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Search(TechnicalSpecialistComputerElectronicKnowledgeInfo model);

        IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IList<int> Ids);

        IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IList<string> ComputerKnowledge);

        IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IList<KeyValuePair<string, string>> ePinAndComputerKnowledge);
    }
}
