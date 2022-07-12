using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TimesheetTechnicalSpecialistAccountItemConsumable
    {
        public long Id { get; set; }
        public long TimesheetTechnicalSpecialistId { get; set; }
        public DateTime ExpenceDate { get; set; }
        public string ChargeDescription { get; set; }
        public bool? IsInvoicePrintChargeDescription { get; set; }
        public int ChargeExpenseTypeId { get; set; }
        public decimal? ChargeTotalUnit { get; set; }
        public decimal? ChargeRate { get; set; }
        public string ChargeRateCurrency { get; set; }
        public string PayRateDescription { get; set; }
        public int PayExpenseTypeId { get; set; }
        public decimal PayTotalUnit { get; set; }
        public string PayRateCurrency { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public decimal PayRate { get; set; }
        public string InvoicingStatus { get; set; }
        public int? InvoiceId { get; set; }
        public int ContractId { get; set; }
        public int ProjectId { get; set; }
        public int AssignmentId { get; set; }
        public long TimesheetId { get; set; }
        public long? Evoid { get; set; }
        public int? PayrollPeriodId { get; set; }
        public bool? IsUseItemChargeExchangeRate { get; set; }
        public int? SalesTaxId { get; set; }
        public int? WithholdingTaxId { get; set; }
        public bool? IsDescriptionPrintedOnInvoice { get; set; }
        public int? CreditNoteId { get; set; }
        public int? UnpaidStatusId { get; set; }
        public int? UnpaidReasonId { get; set; }
        public string CostofSalesStatus { get; set; }
        public int? CostofSalesBatchId { get; set; }
        public int? ChargeRateId { get; set; }
        public int? PayRateId { get; set; }
        public string ModeOfCreation { get; set; }
        public string SyncFlag { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Data ChargeExpenseType { get; set; }
        public virtual ContractRate ChargeRateNavigation { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Invoice CreditNote { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Data PayExpenseType { get; set; }
        public virtual TechnicalSpecialistPayRate PayRateNavigation { get; set; }
        public virtual CompanyPayrollPeriod PayrollPeriod { get; set; }
        public virtual Project Project { get; set; }
        public virtual CompanyTax SalesTax { get; set; }
        public virtual Timesheet Timesheet { get; set; }
        public virtual TimesheetTechnicalSpecialist TimesheetTechnicalSpecialist { get; set; }
        public virtual UnpaidStatusReason UnpaidReason { get; set; }
        public virtual Data UnpaidStatus { get; set; }
        public virtual CompanyTax WithholdingTax { get; set; }
    }
}
