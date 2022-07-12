using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Company
    {
        public Company()
        {
            AssignmentAdditionalExpense = new HashSet<AssignmentAdditionalExpense>();
            AssignmentContractCompany = new HashSet<Assignment>();
            AssignmentHostCompany = new HashSet<Assignment>();
            AssignmentInterCompanyDiscount = new HashSet<AssignmentInterCompanyDiscount>();
            AssignmentOperatingCompany = new HashSet<Assignment>();
            CompanyChargeSchedule = new HashSet<CompanyChargeSchedule>();
            CompanyDivision = new HashSet<CompanyDivision>();
            CompanyExpectedMargin = new HashSet<CompanyExpectedMargin>();
            CompanyMessage = new HashSet<CompanyMessage>();
            CompanyNote = new HashSet<CompanyNote>();
            CompanyOffice = new HashSet<CompanyOffice>();
            CompanyPayroll = new HashSet<CompanyPayroll>();
            CompanyTax = new HashSet<CompanyTax>();
            ContractContractHolderCompany = new HashSet<Contract>();
            ContractInvoicingCompany = new HashSet<Contract>();
            CustomerCompanyAccountReference = new HashSet<CustomerCompanyAccountReference>();
            Draft = new HashSet<Draft>();
            InterCompanyInvoiceBatch = new HashSet<InterCompanyInvoiceBatch>();
            InterCompanyInvoiceRaisedAgainstCompany = new HashSet<InterCompanyInvoice>();
            InterCompanyInvoiceRaisedByCompany = new HashSet<InterCompanyInvoice>();
            InterCompanyTransferFromCompany = new HashSet<InterCompanyTransfer>();
            InterCompanyTransferToCompany = new HashSet<InterCompanyTransfer>();
            Invoice = new HashSet<Invoice>();
            InvoiceBatch = new HashSet<InvoiceBatch>();
            InvoiceNumberRange = new HashSet<InvoiceNumberRange>();
            ResourceSearch = new HashSet<ResourceSearch>();
            Task = new HashSet<Task>();
            TechnicalSpecialist = new HashSet<TechnicalSpecialist>();
            TimesheetInterCompanyDiscount = new HashSet<TimesheetInterCompanyDiscount>();
            User = new HashSet<User>();
            UserRole = new HashSet<UserRole>();
            UserType = new HashSet<UserType>();
            VisitInterCompanyDiscount = new HashSet<VisitInterCompanyDiscount>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string InvoiceCompanyName { get; set; }
        public bool? IsActive { get; set; }
        public string NativeCurrency { get; set; }
        public string SalesTaxDescription { get; set; }
        public string WithholdingTaxDescription { get; set; }
        public string InterCompanyExpenseAccRef { get; set; }
        public string InterCompanyRateAccRef { get; set; }
        public string InterCompanyRoyaltyAccRef { get; set; }
        public int? CompanyMiiwaid { get; set; }
        public int? OperatingCountry { get; set; }
        public int? CompanyMiiwaref { get; set; }
        public bool? IsUseIctms { get; set; }
        public bool? IsFullUse { get; set; }
        public string GfsCoa { get; set; }
        public string GfsBu { get; set; }
        public string Region { get; set; }
        public bool? IsCostOfSalesEmailOverrideAllow { get; set; }
        public decimal? AverageTshourlycost { get; set; }
        public string VatTaxRegistrationNo { get; set; }
        public string Euvatprefix { get; set; }
        public string Iaregion { get; set; }
        public string CognosNumber { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? LogoId { get; set; }
        public int ResourceOutsideDistance { get; set; }
        public string VatregTextWithinEc { get; set; }
        public string VatregTextOutsideEc { get; set; }

        public virtual Data Logo { get; set; }
        public virtual Country OperatingCountryNavigation { get; set; }
        public virtual ICollection<AssignmentAdditionalExpense> AssignmentAdditionalExpense { get; set; }
        public virtual ICollection<Assignment> AssignmentContractCompany { get; set; }
        public virtual ICollection<Assignment> AssignmentHostCompany { get; set; }
        public virtual ICollection<AssignmentInterCompanyDiscount> AssignmentInterCompanyDiscount { get; set; }
        public virtual ICollection<Assignment> AssignmentOperatingCompany { get; set; }
        public virtual ICollection<CompanyChargeSchedule> CompanyChargeSchedule { get; set; }
        public virtual ICollection<CompanyDivision> CompanyDivision { get; set; }
        public virtual ICollection<CompanyExpectedMargin> CompanyExpectedMargin { get; set; }
        public virtual ICollection<CompanyMessage> CompanyMessage { get; set; }
        public virtual ICollection<CompanyNote> CompanyNote { get; set; }
        public virtual ICollection<CompanyOffice> CompanyOffice { get; set; }
        public virtual ICollection<CompanyPayroll> CompanyPayroll { get; set; }
        public virtual ICollection<CompanyTax> CompanyTax { get; set; }
        public virtual ICollection<Contract> ContractContractHolderCompany { get; set; }
        public virtual ICollection<Contract> ContractInvoicingCompany { get; set; }
        public virtual ICollection<CustomerCompanyAccountReference> CustomerCompanyAccountReference { get; set; }
        public virtual ICollection<Draft> Draft { get; set; }
        public virtual ICollection<InterCompanyInvoiceBatch> InterCompanyInvoiceBatch { get; set; }
        public virtual ICollection<InterCompanyInvoice> InterCompanyInvoiceRaisedAgainstCompany { get; set; }
        public virtual ICollection<InterCompanyInvoice> InterCompanyInvoiceRaisedByCompany { get; set; }
        public virtual ICollection<InterCompanyTransfer> InterCompanyTransferFromCompany { get; set; }
        public virtual ICollection<InterCompanyTransfer> InterCompanyTransferToCompany { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<InvoiceBatch> InvoiceBatch { get; set; }
        public virtual ICollection<InvoiceNumberRange> InvoiceNumberRange { get; set; }
        public virtual ICollection<ResourceSearch> ResourceSearch { get; set; }
        public virtual ICollection<Task> Task { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialist { get; set; }
        public virtual ICollection<TimesheetInterCompanyDiscount> TimesheetInterCompanyDiscount { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<UserType> UserType { get; set; }
        public virtual ICollection<VisitInterCompanyDiscount> VisitInterCompanyDiscount { get; set; }
    }
}
