using System;
using Evolution.Common.Models.Base;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    /// <summary>
    /// Contains the information of timesheet Note
    /// </summary>
    public class TimesheetNote : BaseModel
    {
        [AuditNameAttribute("Timesheet Id")]
        public long? TimesheetId { get; set; }
        [AuditNameAttribute("Note Id")]
        public int? TimesheetNoteId { get; set; }
        [AuditNameAttribute("Created On","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
        [AuditNameAttribute("Created By")]
        public string CreatedBy { get; set; }
        [AuditNameAttribute("Note")]
        public string Note { get; set; }
        [AuditNameAttribute("Customer Visible")]
        public bool? IsCustomerVisible { get; set; }
        [AuditNameAttribute("Specialist Visible")]
        public bool? IsSpecialistVisible { get; set; }

        public string UserDisplayName { get; set; }
    }
}
