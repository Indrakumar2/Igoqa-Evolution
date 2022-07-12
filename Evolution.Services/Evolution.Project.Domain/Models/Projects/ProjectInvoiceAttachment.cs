using Evolution.Common.Models.Base;

namespace Evolution.Project.Domain.Models.Projects
{
    public class ProjectInvoiceAttachment : BaseModel
    {
        [AuditNameAttribute("Project Invoice Attachment Id")]
        public int? ProjectInvoiceAttachmentId { get; set; } 

        [AuditNameAttribute("Project Number")]
        public int? ProjectNumber { get; set; }
        
        [AuditNameAttribute("Document Type")]
        public string DocumentType { get; set; }

    }
}
