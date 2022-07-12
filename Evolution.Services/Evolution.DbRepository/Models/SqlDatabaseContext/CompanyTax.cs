using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyTax
    {
        public CompanyTax()
        {
            ContractDefaultSalesTax = new HashSet<Contract>();
            ContractDefaultWithholdingTax = new HashSet<Contract>();
            InterCompanyInvoice = new HashSet<InterCompanyInvoice>();
            InterCompanyInvoiceItem = new HashSet<InterCompanyInvoiceItem>();
            InterCompanyInvoiceItemBackup = new HashSet<InterCompanyInvoiceItemBackup>();
            InvoiceItem = new HashSet<InvoiceItem>();
            InvoiceItemBackup = new HashSet<InvoiceItemBackup>();
            InvoiceSalesTax = new HashSet<Invoice>();
            InvoiceWithholdingTaxNavigation = new HashSet<Invoice>();
            ProjectInvoiceSalesTax = new HashSet<Project>();
            ProjectInvoiceWithholdingTax = new HashSet<Project>();
            TimesheetTechnicalSpecialistAccountItemConsumableSalesTax = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemConsumableWithholdingTax = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpenseSalesTax = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemExpenseWithholdingTax = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTimeSalesTax = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTimeWithholdingTax = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravelSalesTax = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            TimesheetTechnicalSpecialistAccountItemTravelWithholdingTax = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemConsumableSalesTax = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemConsumableWithholdingTax = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpenseSalesTax = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemExpenseWithholdingTax = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTimeSalesTax = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTimeWithholdingTax = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravelSalesTax = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemTravelWithholdingTax = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
        public string TaxType { get; set; }
        public bool? IsIcinv { get; set; }
        public bool? IsActive { get; set; }
        public int? EvolutionId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Contract> ContractDefaultSalesTax { get; set; }
        public virtual ICollection<Contract> ContractDefaultWithholdingTax { get; set; }
        public virtual ICollection<InterCompanyInvoice> InterCompanyInvoice { get; set; }
        public virtual ICollection<InterCompanyInvoiceItem> InterCompanyInvoiceItem { get; set; }
        public virtual ICollection<InterCompanyInvoiceItemBackup> InterCompanyInvoiceItemBackup { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItem { get; set; }
        public virtual ICollection<InvoiceItemBackup> InvoiceItemBackup { get; set; }
        public virtual ICollection<Invoice> InvoiceSalesTax { get; set; }
        public virtual ICollection<Invoice> InvoiceWithholdingTaxNavigation { get; set; }
        public virtual ICollection<Project> ProjectInvoiceSalesTax { get; set; }
        public virtual ICollection<Project> ProjectInvoiceWithholdingTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumableSalesTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumableWithholdingTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpenseSalesTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpenseWithholdingTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTimeSalesTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTimeWithholdingTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelSalesTax { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelWithholdingTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumableSalesTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumableWithholdingTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpenseSalesTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpenseWithholdingTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTimeSalesTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTimeWithholdingTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelSalesTax { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelWithholdingTax { get; set; }
    }
}
