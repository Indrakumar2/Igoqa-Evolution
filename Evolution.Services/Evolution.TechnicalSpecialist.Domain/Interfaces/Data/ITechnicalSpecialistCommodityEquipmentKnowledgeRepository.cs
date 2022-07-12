using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;


namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistCommodityEquipmentKnowledgeRepository : IGenericRepository<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>
    {
        IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Search(TechnicalSpecialistCommodityEquipmentKnowledgeInfo model);

        IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IList<int> Ids);

        IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IList<string> Commodity);

        IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetByEquipmentKnowladge(IList<string> EquipmentKnowledge);

        IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IList<KeyValuePair<string, string>> ePinAndCommodity);
    }
}
