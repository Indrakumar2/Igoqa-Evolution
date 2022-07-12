using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistPayScheduleInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Technical Specialist Pay Schedule Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("Pay Schedule Name")]
        public string PayScheduleName { get; set; }
        [AuditNameAttribute("Pay Currency")]
        public string PayCurrency { get; set; }
        [AuditNameAttribute("Pay Schedule Note")]
        public string PayScheduleNote { get; set; }
        [AuditNameAttribute("Is Active")]
        public bool IsActive { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
