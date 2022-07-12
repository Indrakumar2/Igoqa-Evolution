using Evolution.Common.Models.Base;
using System;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentNote : BaseModel
    {
        [AuditNameAttribute("Note")]
        public string Note { get; set; }

        [AuditNameAttribute("Created By")]
        public string CreatedBy { get; set; }

        [AuditNameAttribute("Created On","dd-MMM-yyyy",AuditNameformatDataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }
        
        [AuditNameAttribute("Assignmnet Note Id")]
        public int? AssignmnetNoteId { get; set; }

        public string UserDisplayName { get; set; }
    }
}