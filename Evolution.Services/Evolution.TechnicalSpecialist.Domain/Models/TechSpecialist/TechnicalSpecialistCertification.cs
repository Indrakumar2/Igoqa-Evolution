
using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistCertification : TechnicalSpecialistCertificationAndTraining
    {
        [AuditNameAttribute("Resource Certification")]
        public string CertificationName { get; set; }
        [AuditNameAttribute("Certificate Ref Id")]
        public string CertificateRefId { get; set; }

        public IList<ModuleDocument> VerificationDocuments { get; set; }

        [AuditNameAttribute("Titile ")]
        public string Title { get; set; } // ILearn

        public int ILearnID { get; set; } // ILearn

        public int TypeId { get; set; } //ILearn


    }
}
