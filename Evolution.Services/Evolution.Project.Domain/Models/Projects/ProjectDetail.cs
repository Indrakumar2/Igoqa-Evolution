using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;

namespace Evolution.Project.Domain.Models.Projects
{
    public class ProjectDetail
    {
        public Project ProjectInfo { get; set; }
        
        public IList<ProjectInvoiceAttachment> ProjectInvoiceAttachments { get; set; }

        public IList<ProjectInvoiceReference> ProjectInvoiceReferences { get; set; }

        public IList<ProjectClientNotification> ProjectNotifications { get; set; } 

        public IList<ProjectNote> ProjectNotes { get; set; }

        public IList<ModuleDocument> ProjectDocuments { get; set; }
    }
}
