using Evolution.Common.Models.Base;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistCertificationAndTraining : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Certification And Training Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Certification Type")]
        public bool? IsExternal { get; set; }
        [AuditNameAttribute("Description")]
        public string Description { get; set; }
        [AuditNameAttribute("Effecive Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EffeciveDate { get; set; }
        [AuditNameAttribute("Expiry Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ExpiryDate { get; set; }
        [AuditNameAttribute("Verification Status")]
        public string VerificationStatus { get; set; }
        [AuditNameAttribute("Verification Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VerificationDate { get; set; }
        [AuditNameAttribute("Verified By")]
        public string VerifiedBy { get; set; }
        [AuditNameAttribute("Verification Type")]
        public string VerificationType { get; set; }
        [AuditNameAttribute("Record Type")]
        public string RecordType { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DispalyOrder { get; set; }
      //  [AuditNameAttribute("Notes")]
        public string Notes { get; set; }

        [AuditNameAttribute("Document Name")]
        public string DocumentName { get; set; }

        [AuditNameAttribute("Verification Document Name")]
        public string VerificationDocumentName { get; set; }

        public IList<ModuleDocument> Documents { get; set; }

        [AuditNameAttribute("IsILearn")] // added for ILearn
        public bool IsILearn { get; set; }  // added for ILearn

    }

}
