using Evolution.Common.Models.Base;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractInvoiceAttachment : BaseModel
    {
        [AuditNameAttribute("Contract Invoice Attachment Id")]
        public int? ContractInvoiceAttachmentId { get; set; }

        [AuditNameAttribute("Contract Number")]
        public string ContractNumber { get; set; }
        
        [AuditNameAttribute("Document Type")]
        public string DocumentType { get; set; }
    }
}
