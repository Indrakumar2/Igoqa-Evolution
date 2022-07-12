using Evolution.Common.Models.Base;
using System;


namespace Evolution.Common.Models.Notes
{
    public class Note : BaseModel
    {
        [AuditNameAttribute("Notes")]
        public string Notes { get; set; }

        [AuditNameAttribute("Created By")]
        public string CreatedBy { get; set; }

        [AuditNameAttribute("Created On", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        public string UserDisplayName { get; set; }

    }
}
