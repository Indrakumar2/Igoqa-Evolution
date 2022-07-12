using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistTraining : TechnicalSpecialistCertificationAndTraining
    {
        [AuditNameAttribute("Training Name")]
        public string TrainingName { get; set; }
        [AuditNameAttribute("Training Ref Id")]
        public string TrainingRefId { get; set; }
        [AuditNameAttribute("Duration")]
        public string Duration { get; set; }

        public IList<ModuleDocument> VerificationDocuments { get; set; }


        [AuditNameAttribute("Titile ")]
        public string Title { get; set; } // ILearn

        public int ILearnID { get; set; } // ILearn

        public int TypeId { get; set; } //ILearn
    }
}
