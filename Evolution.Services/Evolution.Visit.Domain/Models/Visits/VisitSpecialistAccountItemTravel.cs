using System;
using Evolution.Common.Models.Base;

namespace Evolution.Visit.Domain.Models.Visits
{
    /// <summary>
    /// This will contain the information of "Travel" Account Item belonging to TechSpec
    /// </summary>
    public class VisitSpecialistAccountItemTravel : BaseModel
    {
        [AuditNameAttribute("Visit Technical Specialist Account Travel Id")]
        public long? VisitTechnicalSpecialistAccountTravelId { get; set; }
        [AuditNameAttribute("Visit Id")]
        public long? VisitId { get; set; }
        [AuditNameAttribute("ePin")]
        public string Pin { get; set; }
        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }
        [AuditNameAttribute("Visit Technical Specialist Id")]
        public long? VisitTechnicalSpecialistId { get; set; }
        [AuditNameAttribute("Expense Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime ExpenseDate { get; set; }
        [AuditNameAttribute("Expense Description")]
        public string ExpenseDescription { get; set; }
        [AuditNameAttribute("Invoice Print Expense Description")]
        public bool? IsInvoicePrintExpenseDescription { get; set; }
        [AuditNameAttribute("Charge Expense Type")]
        public string ChargeExpenseType { get; set; }
        [AuditNameAttribute("Charge Total Unit")]
        public decimal ChargeTotalUnit { get; set; }
        [AuditNameAttribute("Charge Rate")]
        public string ChargeRate { get; set; }
        [AuditNameAttribute("Charge Rate Currency")]
        public string ChargeRateCurrency { get; set; }
        [AuditNameAttribute("Pay Expense Type")]
        public string PayExpenseType { get; set; }
        [AuditNameAttribute("Pay Unit")]
        public decimal PayUnit { get; set; }
        [AuditNameAttribute("Pay Rate")]
        public decimal PayRate { get; set; }
        [AuditNameAttribute("Pay Rate Currency")]
        public string PayRateCurrency { get; set; }
        public string ContractNumber { get; set; }       
        public int ProjectNumber { get; set; }
        [AuditNameAttribute("Invoice Id")]
        public int? InvoiceId { get; set; }
        [AuditNameAttribute("Invoicing Status")]
        public string InvoicingStatus { get; set; }
        [AuditNameAttribute("Cost of Sales Batch Id")]
        public int? CostofSalesBatchId { get; set; }
        [AuditNameAttribute("Cost of Sales Status")]
        public string CostofSalesStatus { get; set; }
        [AuditNameAttribute("Unpaid Status")]
        public string UnPaidStatus { get; set; }
        [AuditNameAttribute("Unpaid Status Reason")]
        public string UnPaidStatusReason { get; set; }
        [AuditNameAttribute("Mode Of Creation")]
        public string ModeOfCreation { get; set; }

        [AuditNameAttribute("Evo Id")]
        public long? Evoid { get; set; } // These needs to be removed once DB sync done
        public int? ChargeRateId { get; set; }
        public int? PayRateId { get; set; }

        [AuditNameAttribute("Sales Tax Id")]
        public int? SalesTaxId { get; set; }

        [AuditNameAttribute("Withholding Tax Id")]
        public int? WithholdingTaxId { get; set; }
    }
}
