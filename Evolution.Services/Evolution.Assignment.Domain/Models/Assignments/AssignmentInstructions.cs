using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentInstructions : BaseModel
    {
        
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Inter Company Instructions")]
        public string InterCompanyInstructions { get; set; }

        [AuditNameAttribute("Resource Instructions ")]
        public string TechnicalSpecialistInstructions { get; set; }

        //public string ClientReportingRequirements { get; set; }

        //public bool? IsActive { get; set; }

    }
}
