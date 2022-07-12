using System;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class Assignment : AssignmentSearch
    {
        private decimal _assignmentRemainingBudgetValue = 0;
        private decimal _assignmentRemainingBudgetHours = 0;
        private decimal _assignmentBudgetValueWarningPercentage = 0;
        private decimal _assignmentBudgetHoursWarningPercentage = 0;

        [AuditNameAttribute("Created Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? AssignmentCreatedDate { get; set; }

        [AuditNameAttribute("Budget Value")]
        public decimal? AssignmentBudgetValue { get; set; }

        [AuditNameAttribute("Budget Currency")]
        public string AssignmentBudgetCurrency { get; set; }

        [AuditNameAttribute("Budget Hours")]
        public decimal AssignmentBudgetHours { get; set; }

        [AuditNameAttribute("Budget Monetary Warning")]
        public int? AssignmentBudgetWarning { get; set; }

        [AuditNameAttribute("Budget Hours Warning ")]
        public int? AssignmentBudgetHoursWarning { get; set; }

       
        public decimal AssignmentInvoicedToDate { get; set; }

        
        public decimal AssignmentUninvoicedToDate { get; set; }

        
        public decimal AssignmentHoursInvoicedToDate { get; set; }

        
        public decimal AssignmentHoursUninvoicedToDate { get; set; }

        [AuditNameAttribute("Project Name ")]
        public string AssignmentProjectName { get; set; }

        [AuditNameAttribute("Project Work Flow ")]
        public string AssignmentProjectWorkFlow { get; set; }

        [AuditNameAttribute("Project Business Unit ")]
        public string AssignmentProjectBusinessUnit { get; set; }

        [AuditNameAttribute("Review And Moderation ProcessId")]
        public int? AssignmentReviewAndModerationProcessId { get; set; }

        [AuditNameAttribute("Review And Moderation Process ")]
        public string AssignmentReviewAndModerationProcess { get; set; }

        public string AssignmentSupplierPoMaterial { get; set; }

        [AuditNameAttribute("Is Customer Format Report Required ")]
        public bool? IsAssignmentCustomerFormatReportRequired { get; set; }

        
        public decimal AssignmentRemainingBudgetValue
        {
            get
            {
                return _assignmentRemainingBudgetValue = ((AssignmentBudgetValue ?? 0) - Math.Round(AssignmentInvoicedToDate,2) - Math.Round(AssignmentUninvoicedToDate,2));
            }
        }

       
        public decimal AssignmentRemainingBudgetHours
        {
            get
            {
                return _assignmentRemainingBudgetHours = (AssignmentBudgetHours - Math.Round(AssignmentHoursInvoicedToDate,2) - Math.Round(AssignmentHoursUninvoicedToDate,2));
            }
        }
        
        public decimal AssignmentBudgetValueWarningPercentage
        {
            get
            {
                var value = (AssignmentBudgetValue ?? 0) > 0 ? ((Math.Round(AssignmentInvoicedToDate, 2) + Math.Round(AssignmentUninvoicedToDate, 2)) / AssignmentBudgetValue ?? 0) * 100 : 0;
                return _assignmentBudgetValueWarningPercentage = value > 0 && value >= (AssignmentBudgetWarning ?? 0) ? Math.Round(value, 2) : 0;
            }
        }

        
        public decimal AssignmentBudgetHourWarningPercentage
        {
            get
            {
                var value = AssignmentBudgetHours > 0 ? ((Math.Round(AssignmentHoursInvoicedToDate, 2) + Math.Round(AssignmentHoursUninvoicedToDate, 2)) / AssignmentBudgetHours) * 100 : 0;
                return _assignmentBudgetHoursWarningPercentage = value > 0 && value >= (AssignmentBudgetHoursWarning ?? 0) ? Math.Round(value, 2) : 0;
            }
        }

        [AuditNameAttribute("Client Reporting Requirements")]
        public string ClientReportingRequirements { get; set; }

        [AuditNameAttribute("First Visit Status")]
        public string VisitStatus { get; set; }

        [AuditNameAttribute("First Timesheet Status ")]
        public string TimesheetStatus { get; set; }

        [AuditNameAttribute("Visit From Date ", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitFromDate { get; set; }

        [AuditNameAttribute("Visit To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitToDate { get; set; }

        [AuditNameAttribute("Timesheet From Date ", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetFromDate { get; set; }

        [AuditNameAttribute("Timesheet To Date ", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetToDate { get; set; }

        [AuditNameAttribute("First Visit Timesheet Start Date ", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FirstVisitTimesheetStartDate { get; set; }

        [AuditNameAttribute("First Visit Timesheet End Date ", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FirstVisitTimesheetEndDate { get; set; }

        [AuditNameAttribute("Work Location CountryId ")]
        public int? WorkLocationCountryId { get; set; }

        [AuditNameAttribute("EForm Report Required ")]
        public int IsEFormReportRequired { get; set; }

        [AuditNameAttribute("Work Location Country ")]
        public string WorkLocationCountry { get; set; }

        [AuditNameAttribute("Work Location CountyId ")]
        public int? WorkLocationCountyId { get; set; }

        [AuditNameAttribute("Work Location County ")]
        public string WorkLocationCounty { get; set; }

        [AuditNameAttribute("Work Location CityId ")]
        public int? WorkLocationCityId { get; set; }

        [AuditNameAttribute("Work Location City ")]
        public string WorkLocationCity { get; set; }

        [AuditNameAttribute("Work Location Pin Code ")]
        public string WorkLocationPinCode { get; set; }

        [AuditNameAttribute("Contract Type ")]
        public string AssignmentContractType { get; set; }

        [AuditNameAttribute("Parent Contract Discount ")]
        public decimal? AssignmentParentContractDiscount { get; set; }

        [AuditNameAttribute("AssignmentParentContractCompany ")]
        public string AssignmentParentContractCompany { get; set; }

        [AuditNameAttribute("Parent Contract Company Code ")]
        public string AssignmentParentContractCompanyCode { get; set; }

        [AuditNameAttribute("Pre Assignment Id ")]
        public int? PreAssignmentId { get; set; }

        [AuditNameAttribute("Resource Search Id ")]
        public int? ResourceSearchId { get; set; }

        [AuditNameAttribute("ARS Task Type ")]
        public string ARSTaskType { get; set; }
        
        public string ProjectInvoiceInstructionNotes { get; set; }

        public string LastSequence { get; set; }

        public bool IsExtranetSummaryReportVisible { get; set; }

        public List<string> InterCompCodes { get; set; }

        public bool IsSupplierPOChanged { get; set; }

        public bool IsFirstVisit { get; set; }

        public bool IsOverrideOrPLO { get; set; }

        public bool IsOverrideOrPLOForPage { get; set; }

        public bool? IsVisitOnPopUp { get; set; }

        public string AssignmentOperatingCompanyCoordinatorEmail { get; set; }

        public string AssignmentContractCompanyCoordinatorEmail { get; set; }
    }

    public class AssignmentDashboard : Assignment
    {
        
    }

    public class AssignmentVisitTimesheet : Assignment
    {

    }
    //public class ProjectCollection
    //{
    //    public DbModel.Customer Customer { get; set; }
    //    public List<DbModel.CustomerAddress> CustomerAddress { get; set; }
    //    public List<DbModel.CustomerContact> CustomerContact { get; set; }
    //    public DbModel.Company Company { get; set; }
    //    public DbModel.Contract Contract { get; set; }
    //    public List<DbModel.ContractSchedule> ContractSchedule { get; set; }
    //    public List<DbModel.ContractRate> ContractRate { get; set; }
    //    public DbModel.Project Project { get; set; }
    //    public List<DbModel.Supplier> Supplier { get; set; }
    //    public List<DbModel.SupplierContact> SupplierContact { get; set; }
    //    public List<DbModel.SupplierPurchaseOrder> SupplierPurchaseOrder { get; set; }
    //    public List<DbModel.SupplierPurchaseOrderSubSupplier> SupplierPurchaseOrderSubSupplier { get; set; }
    //}
}
