using Evolution.Common.Models.Base;
using Evolution.Common.Models.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistSensitiveDocument : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Sensitive Document Id")]
        public int ID { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Expiry Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime ExpiryDate { get; set; }
        [AuditNameAttribute("Comments")]
        public string Comments { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DisplayOrder { get; set; }

        // public IList<ModuleDocument> Documents { get; set; }
    }
}
