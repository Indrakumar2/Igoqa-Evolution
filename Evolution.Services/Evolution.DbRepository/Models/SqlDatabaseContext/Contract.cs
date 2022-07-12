using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Contract
    {
        public Contract()
        {
            ContractExchangeRate = new HashSet<ContractExchangeRate>();
            ContractInvoiceAttachment = new HashSet<ContractInvoiceAttachment>();
            ContractInvoiceReference = new HashSet<ContractInvoiceReference>();
            ContractMessage = new HashSet<ContractMessage>();
            ContractNote = new HashSet<ContractNote>();
            ContractSchedule = new HashSet<ContractSchedule>();
            InverseFrameworkContract = new HashSet<Contract>();
            InverseParentContract = new HashSet<Contract>();
            InvoiceContract = new HashSet<Invoice>();
            InvoiceParentContract = new HashSet<Invoice>();
            Project = new HashSet<Project>();
            TimesheetTechnicalSpecialistAccountItemConsumable = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpense = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTime = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravel = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemConsumable = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpense = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTime = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravel = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ContractNumber { get; set; }
        public string CustomerContractNumber { get; set; }
        public int ContractHolderCompanyId { get; set; }
        public decimal Budget { get; set; }
        public string BudgetCurrency { get; set; }
        public int? BudgetWarning { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public bool? IsUseFixedExchangeRates { get; set; }
        public int DefaultCustomerContractContactId { get; set; }
        public int DefaultCustomerInvoiceContactId { get; set; }
        public int DefaultCustomerInvoiceAddressId { get; set; }
        public int DefaultSalesTaxId { get; set; }
        public int? DefaultWithholdingTaxId { get; set; }
        public string DefaultInvoiceCurrency { get; set; }
        public string DefaultInvoiceGrouping { get; set; }
        public int? DefaultCustomerContractAddressId { get; set; }
        public int? DefaultRemittanceTextId { get; set; }
        public int? DefaultFooterTextId { get; set; }
        public int? ParentContractId { get; set; }
        public int InvoicingCompanyId { get; set; }
        public decimal? ParentContractDiscountPercentage { get; set; }
        public int? CompanyOfficeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsUseInvoiceDetailsFromParentContract { get; set; }
        public int? InvoicePaymentTermsId { get; set; }
        public bool? IsCrmstatus { get; set; }
        public decimal? Crmreference { get; set; }
        public string Crmreason { get; set; }
        public int? FrameworkContractId { get; set; }
        public int? FrameworkCompanyOfficeId { get; set; }
        public decimal BudgetHours { get; set; }
        public int? BudgetHoursWarning { get; set; }
        public bool? IsRemittanceText { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string ContractType { get; set; }

        public virtual CompanyOffice CompanyOffice { get; set; }
        public virtual Company ContractHolderCompany { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustomerAddress DefaultCustomerContractAddress { get; set; }
        public virtual CustomerContact DefaultCustomerContractContact { get; set; }
        public virtual CustomerAddress DefaultCustomerInvoiceAddress { get; set; }
        public virtual CustomerContact DefaultCustomerInvoiceContact { get; set; }
        public virtual CompanyMessage DefaultFooterText { get; set; }
        public virtual CompanyMessage DefaultRemittanceText { get; set; }
        public virtual CompanyTax DefaultSalesTax { get; set; }
        public virtual CompanyTax DefaultWithholdingTax { get; set; }
        public virtual CompanyOffice FrameworkCompanyOffice { get; set; }
        public virtual Contract FrameworkContract { get; set; }
        public virtual Data InvoicePaymentTerms { get; set; }
        public virtual Company InvoicingCompany { get; set; }
        public virtual Contract ParentContract { get; set; }
        public virtual ICollection<ContractExchangeRate> ContractExchangeRate { get; set; }
        public virtual ICollection<ContractInvoiceAttachment> ContractInvoiceAttachment { get; set; }
        public virtual ICollection<ContractInvoiceReference> ContractInvoiceReference { get; set; }
        public virtual ICollection<ContractMessage> ContractMessage { get; set; }
        public virtual ICollection<ContractNote> ContractNote { get; set; }
        public virtual ICollection<ContractSchedule> ContractSchedule { get; set; }
        public virtual ICollection<Contract> InverseFrameworkContract { get; set; }
        public virtual ICollection<Contract> InverseParentContract { get; set; }
        public virtual ICollection<Invoice> InvoiceContract { get; set; }
        public virtual ICollection<Invoice> InvoiceParentContract { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravel { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravel { get; set; }
    }
}
