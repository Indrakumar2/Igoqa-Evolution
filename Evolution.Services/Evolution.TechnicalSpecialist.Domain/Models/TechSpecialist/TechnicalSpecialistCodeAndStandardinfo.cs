using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
   public class TechnicalSpecialistCodeAndStandardinfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Code And Standard info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Code Standard Name")]
        public string CodeStandardName { get; set; }
    }
}
