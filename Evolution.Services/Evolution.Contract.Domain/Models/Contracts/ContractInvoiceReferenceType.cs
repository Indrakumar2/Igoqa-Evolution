using Evolution.Common.Models.Base;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractInvoiceReferenceType : BaseModel
    {
        [AuditNameAttribute("Contract Invoice Reference Id")]
        public int? ContractInvoiceReferenceTypeId { get; set; }

        [AuditNameAttribute("Contract Number")]
        public string ContractNumber { get; set; }
        
        [AuditNameAttribute("Reference Type")]
        public string ReferenceType { get; set; }

        [AuditNameAttribute("Display Order")]
        public int DisplayOrder { get; set; }

        [AuditNameAttribute("Is Visible To Assignment ")]
        public bool? IsVisibleToAssignment { get; set; }

        [AuditNameAttribute("Is Visible To Visit ")]
        public bool? IsVisibleToVisit { get; set; }

        [AuditNameAttribute("Is Visible To Timesheet ")]
        public bool? IsVisibleToTimesheet { get; set; }

    }
}
