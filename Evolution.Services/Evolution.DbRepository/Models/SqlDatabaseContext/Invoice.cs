using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Invoice
    {
        public Invoice()
        {
            InterCompanyInvoice = new HashSet<InterCompanyInvoice>();
            InterCompanyTransfer = new HashSet<InterCompanyTransfer>();
            InverseOriginalInvoice = new HashSet<Invoice>();
            InvoiceAssignmentReferenceType = new HashSet<InvoiceAssignmentReferenceType>();
            InvoiceExchangeRate = new HashSet<InvoiceExchangeRate>();
            InvoiceItem = new HashSet<InvoiceItem>();
            InvoiceMessage = new HashSet<InvoiceMessage>();
            TimesheetTechnicalSpecialistAccountItemConsumableCreditNote = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemConsumableInvoice = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpenseCreditNote = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemExpenseInvoice = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTimeCreditNote = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTimeInvoice = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravelCreditNote = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            TimesheetTechnicalSpecialistAccountItemTravelInvoice = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemConsumableCreditNote = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemConsumableInvoice = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpenseCreditNote = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemExpenseInvoice = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTimeCreditNote = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTimeInvoice = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravelCreditNote = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemTravelInvoice = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int DraftInvoiceNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoicingCompanyId { get; set; }
        public int ContractId { get; set; }
        public int? ParentContractId { get; set; }
        public int? OriginalInvoiceId { get; set; }
        public string InvoiceStatus { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Currency { get; set; }
        public decimal Net { get; set; }
        public int? SalesTaxId { get; set; }
        public int? WithholdingTaxId { get; set; }
        public decimal InvoiceTotal { get; set; }
        public int? BatchId { get; set; }
        public string InterCompanyTransferStatus { get; set; }
        public string Ictmsreference { get; set; }
        public string Lang { get; set; }
        public int? CreditNoteReasonId { get; set; }
        public bool IsPrintInvoiceTotalToDate { get; set; }
        public bool? IsExported { get; set; }
        public bool IsCreditNote { get; set; }
        public bool IsPrintTotalsInLocalCurrency { get; set; }
        public bool IsExcludeTaxColumn { get; set; }
        public bool IsPrintExchangeRateToLocalCurrency { get; set; }
        public bool IsSpecifyExchangeRates { get; set; }
        public bool IsPrintCustomerAssignmentContact { get; set; }
        public bool? IsInterCompanyTransferRequired { get; set; }
        public bool? IsIncludeBreakdownByAssignment { get; set; }
        public bool? IsIncludeBreakdownByExpenseType { get; set; }
        public bool? IsIncludeBackup { get; set; }
        public bool IsPrintLineItemBreakdown { get; set; }
        public DateTime? CommittedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string SalesTaxName { get; set; }
        public decimal? SalesTaxValue { get; set; }
        public string SalesTaxCode { get; set; }
        public decimal? SalesTaxTotal { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public decimal? WithholdingTax { get; set; }

        public virtual InvoiceBatch Batch { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Data CreditNoteReason { get; set; }
        public virtual Company InvoicingCompany { get; set; }
        public virtual Invoice OriginalInvoice { get; set; }
        public virtual Contract ParentContract { get; set; }
        public virtual Project Project { get; set; }
        public virtual CompanyTax SalesTax { get; set; }
        public virtual CompanyTax WithholdingTaxNavigation { get; set; }
        public virtual ICollection<InterCompanyInvoice> InterCompanyInvoice { get; set; }
        public virtual ICollection<InterCompanyTransfer> InterCompanyTransfer { get; set; }
        public virtual ICollection<Invoice> InverseOriginalInvoice { get; set; }
        public virtual ICollection<InvoiceAssignmentReferenceType> InvoiceAssignmentReferenceType { get; set; }
        public virtual ICollection<InvoiceExchangeRate> InvoiceExchangeRate { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItem { get; set; }
        public virtual ICollection<InvoiceMessage> InvoiceMessage { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumableCreditNote { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumableInvoice { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpenseCreditNote { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpenseInvoice { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTimeCreditNote { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTimeInvoice { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelCreditNote { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelInvoice { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumableCreditNote { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumableInvoice { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpenseCreditNote { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpenseInvoice { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTimeCreditNote { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTimeInvoice { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelCreditNote { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelInvoice { get; set; }
    }
}
