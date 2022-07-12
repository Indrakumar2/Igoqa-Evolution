using System;
using Evolution.Common.Models.Base;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetSpecialistAccountItemConsumable : BaseModel
    {
        [AuditNameAttribute("Timesheet Technical Specialist Account Item Consumable Id")]
        public long? TimesheetTechnicalSpecialistAccountConsumableId { get; set; }

        [AuditNameAttribute("Timesheet Specialist Account Item Consumable Technical Specialist Id")]
        public long? TimesheetTechnicalSpecialistId { get; set; }

        [AuditNameAttribute("ePin")]
        public string Pin { get; set; }

        [AuditNameAttribute("Timesheet Id")]
        public long? TimesheetId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int AssignmentId { get; set; }

        [AuditNameAttribute("Expense Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime ExpenseDate { get; set; }

        [AuditNameAttribute("Charge Description")]
        public string ChargeDescription { get; set; }

        [AuditNameAttribute("Invoice Print Charge Description")]
        public bool? IsInvoicePrintChargeDescription { get; set; }

        [AuditNameAttribute("Charge Expense Type")]
        public string ChargeExpenseType { get; set; }

        [AuditNameAttribute("Charge Unit")]
        public decimal ChargeUnit { get; set; }

        [AuditNameAttribute("Charge Rate")]
        public string ChargeRate { get; set; } //Changes for ITK 1532

        [AuditNameAttribute("Charge Rate Currency")]
       public string ChargeRateCurrency { get; set; }

        [AuditNameAttribute("Pay Rate Description")]
          public string PayRateDescription { get; set; }

        [AuditNameAttribute("Pay Type")]
        public string PayType { get; set; }

        [AuditNameAttribute("Pay Unit")]
        public decimal PayUnit { get; set; }

        [AuditNameAttribute("Pay Rate")]
         public decimal PayRate { get; set; }

        [AuditNameAttribute("Pay Rate Currency")]
        public string PayRateCurrency { get; set; }

        [AuditNameAttribute("Contract Number")]
         public string ContractNumber { get; set; }

        [AuditNameAttribute("Project Number")]
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
        public int? ChargeRateId { get; set; }
        public int? PayRateId { get; set; }

        [AuditNameAttribute("Evo Id")]
        public long? Evoid { get; set; } // These needs to be removed once DB sync done

        [AuditNameAttribute("Sales Tax Id")]
        public int? SalesTaxId { get; set; }

        [AuditNameAttribute("Withholding Tax Id")]
        public int? WithholdingTaxId { get; set; }
    }
}
