using System.Collections.Generic;

namespace Evolution.Document.Domain.Models.Document
{
    public class EmailDocument
    {
        public bool IsDocumentUpload { get; set; }         
        public long VisitId { get; set; }
        public long TimesheetId { get; set; }
        public List<EmailDocumentUpload> EmailDocumentUpload { get; set; }

        public long? EventId { get; set; } //To capture event ID for Email Documents in Audit
    }

    public class EmailDocumentUpload
    {
        public bool IsDocumentUpload { get; set; }
        public DocumentUniqueNameDetail DocumentUniqueName { get; set; } 
        public string DocumentMessage { get; set; }
        public bool IsVisibleToCustomer { get; set; }
        public bool IsVisibleToTS { get; set; }
    }
}