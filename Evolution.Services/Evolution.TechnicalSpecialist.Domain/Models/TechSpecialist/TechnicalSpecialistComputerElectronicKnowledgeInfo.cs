using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistComputerElectronicKnowledgeInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Computer Electronic Knowledge Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Computer Knowledge")]
        public string ComputerKnowledge { get; set; }
    }
}
