using Evolution.Common.Models.Base;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{ 
    public class TechnicalSpecialistInternalTrainingAndCompetency: BaseTechnicalSpecialistModel
    { 
         [AuditNameAttribute("Technical Specialist Competency And Internal Training Id")]    
        public int Id { get; set; }
         [AuditNameAttribute("ePin")]
        public int Epin { get; set; } 
         [AuditNameAttribute("Expiry", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? Expiry { get; set; }
         [AuditNameAttribute("Score")]
        public string Score { get; set; }
         [AuditNameAttribute("Competency")]
        public string Competency { get; set; }
         [AuditNameAttribute("Record Type")]
        public string RecordType { get; set; } 
         //[AuditNameAttribute("Display Order")]
        public int? DisplayOrder { get; set; }
        // [AuditNameAttribute("Notes")]
        public string Notes { get; set; }
        [AuditNameAttribute("IsILearn")] // added for ILearn
        public bool IsILearn { get; set; }  // added for ILearn

        [AuditNameAttribute("Document Name")]
        public string DocumentName { get; set; }

        public IList<ModuleDocument> Documents { get; set; }

    }
}
