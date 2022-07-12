using Evolution.Common.Models.Base;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetReferenceType : BaseModel
    {
        [AuditNameAttribute("Timesheet Reference Id")]
        public long? TimesheetReferenceId { get; set; }
        [AuditNameAttribute("Timesheet Id")]
        public long? TimesheetId { get; set; }
        [AuditNameAttribute("Reference Type")]
        public string ReferenceType { get; set; }
        [AuditNameAttribute("Reference Value")]
        public string ReferenceValue { get; set; }

        [AuditNameAttribute("Evo Id")]
        public long? Evoid { get; set; } // These needs to be removed once DB sync done

        public int AssignmentReferenceTypeId { get; set; }

    }
}
