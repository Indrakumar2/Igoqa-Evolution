using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentTechnicalSpecialist : BaseModel
    {
        [AuditNameAttribute("Assignment Technical Specialist Id")]
        public int? AssignmentTechnicalSplId { get; set; }

        [AuditNameAttribute("Assignment Id ")]
        public int? AssignmentId { get; set; }
        
        [AuditNameAttribute("Epin")]
        public int? Epin { get; set; }

        [AuditNameAttribute("Resource Name ")]
        public string TechnicalSpecialistName { get; set; }

        [AuditNameAttribute("Active")]
        public bool? IsActive { get; set; }

        [AuditNameAttribute("Supervisor")]
        public bool? IsSupervisor { get; set; }

        [AuditNameAttribute("Profile Status")]
        public string ProfileStatus { get; set; }
        
        [AuditNameAttribute("Created On","dd-MMM-yyyy",AuditNameformatDataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        public List<AssignmentTechnicalSpecialistSchedule> AssignmentTechnicalSpecialistSchedules { get; set; }
    }

    public class AssignmentTechnicalSpecialistSchedule : BaseModel
    {
        [AuditNameAttribute("Assignment Resource Schedule Id ")]
        public int? AssignmentTechnicalSpecialistScheduleId { get; set; }

        [AuditNameAttribute("Contract Schedule Id ")]
        public int? ContractScheduleId { get; set; }

        [AuditNameAttribute("Contract Schedule Name")]
        public string ContractScheduleName { get; set; }

        [AuditNameAttribute("Resource Id ")]
        public int? AssignmentTechnicalSpecilaistId { get; set; }

        [AuditNameAttribute("Resource Pay Schedule Id ")]
        public int? TechnicalSpecialistPayScheduleId { get; set; }

        [AuditNameAttribute("Resource Pay Schedule Name ")]
        public string TechnicalSpecialistPayScheduleName { get; set; }

        [AuditNameAttribute("Schedule Note To Print On Invoice")]
        public string ScheduleNoteToPrintOnInvoice { get; set; }

    }
}
