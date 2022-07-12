using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistDetail
    { 
        public TechnicalSpecialistInfo TechnicalSpecialistInfo { get; set; }
        
        public IList<TechnicalSpecialistStampInfo> TechnicalSpecialistStamp { get; set; }

        public IList<TechnicalSpecialistContactInfo> TechnicalSpecialistContact { get; set; }

        public IList<TechnicalSpecialistPayScheduleInfo> TechnicalSpecialistPaySchedule { get; set; } 

        public IList<TechnicalSpecialistPayRateInfo> TechnicalSpecialistPayRate { get; set; }

        public IList<TechnicalSpecialistTaxonomyInfo> TechnicalSpecialistTaxonomy { get; set; }

        public IList<TechnicalSpecialistInternalTraining> TechnicalSpecialistInternalTraining { get; set; }

        public IList<TechnicalSpecialistCompetency> TechnicalSpecialistCompetancy { get; set; } 

        public IList<TechnicalSpecialistCustomerApprovalInfo> TechnicalSpecialistCustomerApproval { get; set; }

        public IList<TechnicalSpecialistWorkHistoryInfo> TechnicalSpecialistWorkHistory { get; set; }

        public IList<TechnicalSpecialistEducationalQualificationInfo> TechnicalSpecialistEducation { get; set; }

        public IList<TechnicalSpecialistCodeAndStandardinfo> TechnicalSpecialistCodeAndStandard { get; set; }

        public IList<TechnicalSpecialistTraining> TechnicalSpecialistTraining { get; set; }

        public IList<TechnicalSpecialistCertification> TechnicalSpecialistCertification { get; set; }

        public IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> TechnicalSpecialistCommodityAndEquipment { get; set; }

        public IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> TechnicalSpecialistComputerElectronicKnowledge { get; set; }

        public IList<TechnicalSpecialistLanguageCapabilityInfo> TechnicalSpecialistLanguageCapabilities { get; set; }

        public IList<ModuleDocument> TechnicalSpecialistDocuments { get; set; }

        public IList<TechnicalSpecialistNoteInfo> TechnicalSpecialistNotes { get; set; }

        public IList<ModuleDocument> TechnicalSpecialistSensitiveDocuments { get; set; }

    }
}
