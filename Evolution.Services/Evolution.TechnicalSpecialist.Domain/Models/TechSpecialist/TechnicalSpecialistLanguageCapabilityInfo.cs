using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistLanguageCapabilityInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Language Capability Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("Language")]
        public string Language { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Speaking Capability Level")]
        public string SpeakingCapabilityLevel { get; set; }
        [AuditNameAttribute("Writing Capability Level")]
        public string WritingCapabilityLevel { get; set; }
        [AuditNameAttribute("Comprehension Capability Level")]
        public string ComprehensionCapabilityLevel { get; set; }
        //[AuditNameAttribute("DispalyOrder")]
        public int? DispalyOrder { get; set; }

    }
}
