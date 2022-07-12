using Evolution.Common.Models.Base;
using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistStampInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Stamp Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Is Soft Stamp")]
        public bool? IsSoftStamp { get; set; }
        [AuditNameAttribute("Country Code")]
        public string CountryCode { get; set; }
        [AuditNameAttribute("Country Name")]
        public string CountryName { get; set; }
        [AuditNameAttribute("Issued Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? IssuedDate { get; set; }
        [AuditNameAttribute("Returned Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ReturnedDate { get; set; }
        [AuditNameAttribute("Stamp Number")]
        public string StampNumber { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int? DisplayOrder { get; set; }
        [AuditNameAttribute("Document Name")]
        public string DocumentName { get; set; }

        public IList<ModuleDocument> Documents { get; set; }
    }
}
