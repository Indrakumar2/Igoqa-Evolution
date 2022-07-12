using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCommodityEquipmentKnowledge
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int CommodityId { get; set; }
        public int EquipmentKnowledgeId { get; set; }
        public int EquipmentKnowledgeLevel { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data Commodity { get; set; }
        public virtual Data EquipmentKnowledge { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
