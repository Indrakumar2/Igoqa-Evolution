using Evolution.Common.Models.Base;

namespace Evolution.Project.Domain.Models.Projects
{
    public class ProjectInvoiceReference : BaseModel
    {
        [AuditNameAttribute("Project Invoice Reference ID")]
        public int? ProjectInvoiceReferenceTypeId { get; set; }

        [AuditNameAttribute("Project Number")]
        public int? ProjectNumber { get; set; }

        [AuditNameAttribute("Reference Type")]
        public string ReferenceType { get; set; }

        [AuditNameAttribute("Display Order")]
        public int DisplayOrder { get; set; }
        
        [AuditNameAttribute("Is Visible To Assignment")]
        public bool? IsVisibleToAssignment { get; set; }

        [AuditNameAttribute("Is Visible To Visit")]
        public bool? IsVisibleToVisit { get; set; }
        
        [AuditNameAttribute("Is Visible To Timesheet")]
        public bool? IsVisibleToTimesheet { get; set; }

    }
}
