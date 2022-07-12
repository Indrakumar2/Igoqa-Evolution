using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistCommodityEquipmentKnowledgeInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Commodity Equipment Knowledge Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Equipment Knowledge")]
        public string EquipmentKnowledge { get; set; }
        [AuditNameAttribute("Equipment Knowledge Level")]
        public int EquipmentKnowledgeLevel { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DispalyOrder { get; set; }
        [AuditNameAttribute("Commodity")]
        public string Commodity { get; set; }

    }
}
