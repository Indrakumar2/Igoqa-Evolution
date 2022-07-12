using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistInternalTrainingAndCompetencyType : BaseTechnicalSpecialistModel
    {
         [AuditNameAttribute("Technical Specialist Id ")]
        public int Id { get; set; }
         [AuditNameAttribute("Resource Internal Training And Competency Id")]
        public int TechnicalSpecialistInternalTrainingAndCompetencyId { get; set; }
         [AuditNameAttribute("Type Name")]
        public string TypeName { get; set; }

        public int? TypeId { get; set; }
    }
     
}
