using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Project
    {
        public Project()
        {
            Assignment = new HashSet<Assignment>();
            CustomerUserProjectAccess = new HashSet<CustomerUserProjectAccess>();
            Invoice = new HashSet<Invoice>();
            ProjectClientNotification = new HashSet<ProjectClientNotification>();
            ProjectInvoiceAssignmentReference = new HashSet<ProjectInvoiceAssignmentReference>();
            ProjectInvoiceAttachment = new HashSet<ProjectInvoiceAttachment>();
            ProjectMessage = new HashSet<ProjectMessage>();
            ProjectNote = new HashSet<ProjectNote>();
            SupplierPurchaseOrder = new HashSet<SupplierPurchaseOrder>();
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
        public int ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ProjectNumber { get; set; }
        public decimal Budget { get; set; }
        public int? BudgetWarning { get; set; }
        public decimal BudgetHours { get; set; }
        public int? BudgetHoursWarning { get; set; }
        public string WorkFlowType { get; set; }
        public int CoordinatorId { get; set; }
        public int ProjectTypeId { get; set; }
        public string IndustrySector { get; set; }
        public bool IsNewFacility { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool IsManagedServices { get; set; }
        public int? ManagedServicesType { get; set; }
        public int? ManagedServicesCoordinatorId { get; set; }
        public bool IsExtranetSummaryVisibleToClient { get; set; }
        public bool? IsRemittanceText { get; set; }
        public bool IsEreportProjectMapped { get; set; }
        public int? CompanyDivisionId { get; set; }
        public int? CompanyanyDivCostCentreId { get; set; }
        public int? CompanyOfficeId { get; set; }
        public string CustomerProjectNumber { get; set; }
        public string CustomerProjectName { get; set; }
        public int CustomerContactId { get; set; }
        public int CustomerInvoiceAddressId { get; set; }
        public int? CustomerProjectContactId { get; set; }
        public int? CustomerProjectAddressId { get; set; }
        public int InvoiceSalesTaxId { get; set; }
        public int? InvoiceWithholdingTaxId { get; set; }
        public string InvoiceCurrency { get; set; }
        public string InvoiceGrouping { get; set; }
        public int? InvoiceRemittanceTextId { get; set; }
        public int? InvoiceFooterTextId { get; set; }
        public int? InvoicePaymentTermsId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? LogoId { get; set; }
        public bool? IsVisitOnPopUp { get; set; }
        public string CustomerDirectReportingEmailAddress { get; set; }

        public virtual CompanyDivision CompanyDivision { get; set; }
        public virtual CompanyOffice CompanyOffice { get; set; }
        public virtual CompanyDivisionCostCenter CompanyanyDivCostCentre { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual User Coordinator { get; set; }
        public virtual CustomerContact CustomerContact { get; set; }
        public virtual CustomerAddress CustomerInvoiceAddress { get; set; }
        public virtual CustomerAddress CustomerProjectAddress { get; set; }
        public virtual CustomerContact CustomerProjectContact { get; set; }
        public virtual CompanyMessage InvoiceFooterText { get; set; }
        public virtual Data InvoicePaymentTerms { get; set; }
        public virtual CompanyMessage InvoiceRemittanceText { get; set; }
        public virtual CompanyTax InvoiceSalesTax { get; set; }
        public virtual CompanyTax InvoiceWithholdingTax { get; set; }
        public virtual Data Logo { get; set; }
        public virtual User ManagedServicesCoordinator { get; set; }
        public virtual Data ManagedServicesTypeNavigation { get; set; }
        public virtual Data ProjectType { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<CustomerUserProjectAccess> CustomerUserProjectAccess { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<ProjectClientNotification> ProjectClientNotification { get; set; }
        public virtual ICollection<ProjectInvoiceAssignmentReference> ProjectInvoiceAssignmentReference { get; set; }
        public virtual ICollection<ProjectInvoiceAttachment> ProjectInvoiceAttachment { get; set; }
        public virtual ICollection<ProjectMessage> ProjectMessage { get; set; }
        public virtual ICollection<ProjectNote> ProjectNote { get; set; }
        public virtual ICollection<SupplierPurchaseOrder> SupplierPurchaseOrder { get; set; }
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
