using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitSearch : BaseModel
    {
        [AuditNameAttribute("Customer Name")]
        public string CustomerName { get; set; }
        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }
        [AuditNameAttribute("Assignment Number")]
        public int? AssignmentNubmer { get; set; }
        [AuditNameAttribute("Contract Holding Company Code")]
        public string ContractHoldingCompanyCode { get; set; }
        [AuditNameAttribute("Contract Holding Company Name")]
        public string ContractHoldingCompanyName { get; set; }
        [AuditNameAttribute("Customer Number")]
        public string CustomerNumber { get; set; }
        [AuditNameAttribute("Report Number")]
        public string ReportNumber { get; set; }
        [AuditNameAttribute("Contract Holding Coordinator Code")]
        public string CHCoordinatorCode { get; set; }
        [AuditNameAttribute("Contract Holding Coordinator Name")]
        public string CHCoordinatorName { get; set; }
        [AuditNameAttribute("Customer Contract Number")]
        public string CustomerContractNumber { get; set; }
        [AuditNameAttribute("Contract Number")]
        public string ContractNumber { get; set; }
        [AuditNameAttribute("From Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FromDate { get; set; }
        [AuditNameAttribute("Operating Company Code")]
        public string OperatingCompanyCode { get; set; }
        [AuditNameAttribute("Operating Company Name")]
        public string OperatingCompanyName { get; set; }

        public int? ProjectNumber { get; set; }
        [AuditNameAttribute("To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ToDate { get; set; }
        [AuditNameAttribute("Operating Company Coordinator Code")]
        public string OCCoordinatorCode { get; set; }
        [AuditNameAttribute("Operating Company Coordinator Name")]
        public string OCCoordinatorName { get; set; }

        public string CustomerProjectName { get; set; }
        [AuditNameAttribute("Supplier PO Number")]
        public string SupplierPONumber { get; set; }
        [AuditNameAttribute("Technical Specialist")]
        //public string SearchDocuments { get; set; }

        public string TechnicalSpecialist { get; set; }
        [AuditNameAttribute("Supplier Sub Supplier")]
        public string SupplierSubSupplier { get; set; }

        //public string SearchText { get; set; }

        public string NotificationReference { get; set; }

        //public string SearchDB { get; set; }
        [AuditNameAttribute("Workflow Type")]
        public string WorkflowType { get; set; }
        [AuditNameAttribute("Project Invoice Instruction Notes")]
        public string ProjectInvoiceInstructionNotes { get; set; }

        public string SearchDocumentType { get; set; }

        public string DocumentSearchText { get; set; }

        public IList<Int64> VisitIds { get; set; }

        [AuditNameAttribute("Visit Id")]
        public long? VisitId { get; set; }

        [AuditNameAttribute("Evo Id")]
        public long? Evoid { get; set; } // These needs to be removed once DB sync done

        public bool IsOnlyViewVisit { get; set; }

        public string LoggedInCompanyCode { get; set; }

        public int LoggedInCompanyId { get; set; }
        public int CustomerId { get; set; }
        [AuditNameAttribute("Supplier Id")]
        public int SupplierId { get; set; }
        public int ContractCompanyId { get; set; }
        public int OperatingCompanyId { get; set; }
        public int ContractCoordinatorId { get; set; }
        public int OperatingCoordinatorId { get; set; }
        public string MaterialDescription { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ServiceId { get; set; }
    }
}
