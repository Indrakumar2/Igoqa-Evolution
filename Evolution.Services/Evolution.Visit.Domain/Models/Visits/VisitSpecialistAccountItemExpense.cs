using System;
using Evolution.Common.Models.Base;

namespace Evolution.Visit.Domain.Models.Visits
{
    /// <summary>
    /// This will contain visit expense.
    /// </summary>
    public class VisitSpecialistAccountItemExpense : BaseModel
    {
        [AuditNameAttribute("Charge Rate Currency")]
        public string ChargeRateCurrency { get; set; }
        [AuditNameAttribute("Visit Technical Specialist Account ExpenseId")]
       
        public long? VisitTechnicalSpecialistAccountExpenseId { get; set; }
        [AuditNameAttribute("Visit Id")]
        public long? VisitId { get; set; }
        [AuditNameAttribute("Visit Technical Specialist Id")]
        public long? VisitTechnicalSpecialistId { get; set; }
        [AuditNameAttribute("PIN")]
        public string Pin { get; set; }
        [AuditNameAttribute("Visit Expense Assignment Id")]
        public int AssignmentId { get; set; }
        [AuditNameAttribute("Visit Expense Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime ExpenseDate { get; set; }
        [AuditNameAttribute("Charge Expense Type")]
        public string ChargeExpenseType { get; set; }
        [AuditNameAttribute("Visit Expense Expense Description")]
        public string ExpenseDescription { get; set; }
        [AuditNameAttribute("Visit Expense Currency")]
        public string Currency { get; set; }
        [AuditNameAttribute("Visit Expense Contract Holder Expense")]
        public bool? IsContractHolderExpense { get; set; }
        [AuditNameAttribute("Visit Expense Charge Unit")]
        public decimal ChargeUnit { get; set; }
        [AuditNameAttribute("Visit Expense Charge Rate")]
        public string ChargeRate { get; set; }
        [AuditNameAttribute("Visit Expense Charge Rate Exchange")]
        public decimal ChargeRateExchange { get; set; }
        [AuditNameAttribute("Visit Expense Pay Unit")]
       public decimal PayUnit { get; set; }
        [AuditNameAttribute("Visit Expense Pay Rate")]
        public decimal PayRate { get; set; }
        [AuditNameAttribute("Visit Expense Pay Rate Tax")]
        public decimal PayRateTax { get; set; }
        [AuditNameAttribute("Visit Expense Pay Rate Exchange")]
        public decimal PayRateExchange { get; set; }
        [AuditNameAttribute("Visit Expense Pay Rate Currency")]
        public string PayRateCurrency { get; set; }
        [AuditNameAttribute("Visit Expense Contract Number")]
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
