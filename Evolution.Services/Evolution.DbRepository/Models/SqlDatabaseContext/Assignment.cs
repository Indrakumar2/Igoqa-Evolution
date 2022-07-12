using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Assignment
    {
        public Assignment()
        {
            AssignmentAdditionalExpense = new HashSet<AssignmentAdditionalExpense>();
            AssignmentContractSchedule = new HashSet<AssignmentContractSchedule>();
            AssignmentContributionCalculation = new HashSet<AssignmentContributionCalculation>();
            AssignmentHistory = new HashSet<AssignmentHistory>();
            AssignmentInterCompanyDiscount = new HashSet<AssignmentInterCompanyDiscount>();
            AssignmentMessage = new HashSet<AssignmentMessage>();
            AssignmentNote = new HashSet<AssignmentNote>();
            AssignmentReferenceNavigation = new HashSet<AssignmentReference>();
            AssignmentSubSupplier = new HashSet<AssignmentSubSupplier>();
            AssignmentTaxonomy = new HashSet<AssignmentTaxonomy>();
            AssignmentTechnicalSpecialist = new HashSet<AssignmentTechnicalSpecialist>();
            InterCompanyInvoiceItem = new HashSet<InterCompanyInvoiceItem>();
            InvoiceItem = new HashSet<InvoiceItem>();
            Timesheet = new HashSet<Timesheet>();
            TimesheetTechnicalSpecialistAccountItemConsumable = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpense = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTime = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravel = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            Visit = new HashSet<Visit>();
            VisitTechnicalSpecialistAccountItemConsumable = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpense = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTime = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravel = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string AssignmentReference { get; set; }
        public int AssignmentNumber { get; set; }
        public bool IsAssignmentComplete { get; set; }
        public string AssignmentStatus { get; set; }
        public string AssignmentType { get; set; }
        public int? SupplierPurchaseOrderId { get; set; }
        public int ContractCompanyId { get; set; }
        public int? ContractCompanyCoordinatorId { get; set; }
        public int OperatingCompanyId { get; set; }
        public int? OperatingCompanyCoordinatorId { get; set; }
        public decimal? BudgetValue { get; set; }
        public int? BudgetWarning { get; set; }
        public decimal BudgetHours { get; set; }
        public int? BudgetHoursWarning { get; set; }
        public int? AssignmentLifecycleId { get; set; }
        public bool? IsCustomerFormatReportRequired { get; set; }
        public int? CustomerAssignmentContactId { get; set; }
        public int? AssignmentCompanyAddressId { get; set; }
        public int? ReviewAndModerationProcessId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string DocumentStatus { get; set; }
        public int? HostCompanyId { get; set; }
        public int? OperationCompanyCountryId { get; set; }
        public int? OperatingCompanyCountyId { get; set; }
        public int? OperatingCompanyLocationId { get; set; }
        public bool? IsFirstVisit { get; set; }
        public bool? IsInternalAssignment { get; set; }
        public string OperatingCompanyPinCode { get; set; }
        public int? ResourceSearchId { get; set; }
        public DateTime? FirstVisitTimesheetStartDate { get; set; }
        public DateTime? FirstVisitTimesheetEndDate { get; set; }
        public string FirstVisitTimesheetStatus { get; set; }
        public bool? IsEformReportRequired { get; set; }
        public bool? IsOverrideOrPlo { get; set; }

        public virtual CustomerAddress AssignmentCompanyAddress { get; set; }
        public virtual Data AssignmentLifecycle { get; set; }
        public virtual Company ContractCompany { get; set; }
        public virtual User ContractCompanyCoordinator { get; set; }
        public virtual CustomerContact CustomerAssignmentContact { get; set; }
        public virtual Company HostCompany { get; set; }
        public virtual Company OperatingCompany { get; set; }
        public virtual User OperatingCompanyCoordinator { get; set; }
        public virtual County OperatingCompanyCounty { get; set; }
        public virtual City OperatingCompanyLocation { get; set; }
        public virtual Country OperationCompanyCountry { get; set; }
        public virtual Project Project { get; set; }
        public virtual Data ReviewAndModerationProcess { get; set; }
        public virtual SupplierPurchaseOrder SupplierPurchaseOrder { get; set; }
        public virtual ICollection<AssignmentAdditionalExpense> AssignmentAdditionalExpense { get; set; }
        public virtual ICollection<AssignmentContractSchedule> AssignmentContractSchedule { get; set; }
        public virtual ICollection<AssignmentContributionCalculation> AssignmentContributionCalculation { get; set; }
        public virtual ICollection<AssignmentHistory> AssignmentHistory { get; set; }
        public virtual ICollection<AssignmentInterCompanyDiscount> AssignmentInterCompanyDiscount { get; set; }
        public virtual ICollection<AssignmentMessage> AssignmentMessage { get; set; }
        public virtual ICollection<AssignmentNote> AssignmentNote { get; set; }
        public virtual ICollection<AssignmentReference> AssignmentReferenceNavigation { get; set; }
        public virtual ICollection<AssignmentSubSupplier> AssignmentSubSupplier { get; set; }
        public virtual ICollection<AssignmentTaxonomy> AssignmentTaxonomy { get; set; }
        public virtual ICollection<AssignmentTechnicalSpecialist> AssignmentTechnicalSpecialist { get; set; }
        public virtual ICollection<InterCompanyInvoiceItem> InterCompanyInvoiceItem { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItem { get; set; }
        public virtual ICollection<Timesheet> Timesheet { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravel { get; set; }
        public virtual ICollection<Visit> Visit { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravel { get; set; }
    }
}
