using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;

namespace Evolution.Project.Domain.Models.Projects
{
    public class Project : ProjectSearch
    {

        private decimal _projectRemainingBudgetValue = 0;

        private decimal _projectRemainingBudgetHours = 0;
        private decimal _projectBudgetValueWarningPercentage = 0;
        private decimal _projectBudgetHoursWarningPercentage = 0;

        [AuditNameAttribute("Creation Date","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [AuditNameAttribute("Company Cost Center Code")]
        public string CompanyCostCenterCode { get; set; }

        [AuditNameAttribute("Company Cost Center Name")]
        public string CompanyCostCenterName { get; set; }

        [AuditNameAttribute("Extranet Summary Visible To Client")]
        public bool IsExtranetSummaryVisibleToClient { get; set; }

        [AuditNameAttribute("Industry Sector")]
        public string IndustrySector { get; set; }
        
        [AuditNameAttribute("Managed Services")]
        public bool IsManagedServices { get; set; }

        [AuditNameAttribute("Project For New Facility")]
        public bool IsProjectForNewFacility { get; set; }

        [AuditNameAttribute("Remittance text")]
        public bool IsRemittanectext { get; set; }

        [AuditNameAttribute("Logo Text")]
        public string LogoText { get; set; }

        public int? ManagedServiceType { get; set; }
        [AuditNameAttribute("Managed Service Type")]
        public string ManagedServiceTypeName { get; set; }//Added for D-1035 

        [AuditNameAttribute("Managed Service Coordinator Name")]
        public string ManagedServiceCoordinatorName { get; set; }

        [AuditNameAttribute("Managed Service Coordinator Code")]
        public string ManagedServiceCoordinatorCode { get; set; } //IGO QC D-900 Issue 1

        [AuditNameAttribute("Budget Monetary Value ")]
        public decimal ProjectBudgetValue { get; set; }

        [AuditNameAttribute("Project Budget Warning")]
        public int ProjectBudgetWarning { get; set; }

        [AuditNameAttribute("Project Budget Hours Unit")]
        public decimal ProjectBudgetHoursUnit { get; set; }

        [AuditNameAttribute("Budget Hours Warning ")]
        public int ProjectBudgetHoursWarning { get; set; }

        [AuditNameAttribute("Project Client Reporting Requirement")]
        public string ProjectClientReportingRequirement { get; set; }

        [AuditNameAttribute("Project Assignment Operation Notes")]
        public string ProjectAssignmentOperationNotes { get; set; }

        [AuditNameAttribute("Project Invoice Payment Terms")]
        public string ProjectInvoicePaymentTerms { get; set; }

        [AuditNameAttribute("Customer Contact")]
        public string ProjectCustomerContact { get; set; }

        [AuditNameAttribute("Customer Contact Address")]
        public string ProjectCustomerContactAddress { get; set; }

        [AuditNameAttribute("Customer Invoice Contact")]
        public string ProjectCustomerInvoiceContact { get; set; }

        [AuditNameAttribute("Customer Invoice Address")]
        public string ProjectCustomerInvoiceAddress { get; set; }

        [AuditNameAttribute("Invoice Remittance Identifier")]
        public string ProjectInvoiceRemittanceIdentifier { get; set; }

        [AuditNameAttribute("Sales Tax")]
        public string ProjectSalesTax { get; set; }

        [AuditNameAttribute("Withholding Tax")]
        public string ProjectWithHoldingTax { get; set; }

        [AuditNameAttribute("Budget Currency")]
        public string ProjectBudgetCurrency { get; set; }
        
        [AuditNameAttribute("Invoicing Currency")]
        public string ProjectInvoicingCurrency { get; set; }

        [AuditNameAttribute("Invoice Grouping")]
        public string ProjectInvoiceGrouping { get; set; }
        
        [AuditNameAttribute("Invoice Footer Identifier")]
        public string ProjectInvoiceFooterIdentifier { get; set; }

        [AuditNameAttribute("Invoice Instruction Notes")]
        public string ProjectInvoiceInstructionNotes { get; set; }

        [AuditNameAttribute("Invoice Free Text")]
        public string ProjectInvoiceFreeText { get; set; }

        
        public decimal ProjectInvoicedToDate { get; set; }

        
        public decimal ProjectUninvoicedToDate { get; set; }
        
        
        public decimal ProjectHoursInvoicedToDate { get; set; }

        
        public decimal ProjectHoursUninvoicedToDate { get; set; }

        [AuditNameAttribute("Workflow Type")]
        public string WorkFlowType { get; set; }
        
        [AuditNameAttribute("VAT(Project)")]
        public string VatRegistrationNumber { get; set; }

        [AuditNameAttribute("EU VAT Prefix")]
        public string EUVATPrefix { get; set; }

        [AuditNameAttribute("Is Visit On Pop Up")]
        public bool? IsVisitOnPopUp { get; set; }

       
        public decimal ProjectRemainingBudgetValue
        {
            get
            {
                return _projectRemainingBudgetValue = (ProjectBudgetValue - Math.Round(ProjectInvoicedToDate,2) - Math.Round(ProjectUninvoicedToDate,2));
            }
        }
       
        public decimal ProjectRemainingBudgetHours
        {
            get
            {
                return _projectRemainingBudgetHours = (ProjectBudgetHoursUnit - Math.Round(ProjectHoursInvoicedToDate,2) - Math.Round(ProjectHoursUninvoicedToDate,2));
            }
        }
        
        public decimal ProjectBudgetValueWarningPercentage
        {
            get
            {
                var value = ProjectBudgetValue > 0 ? ((Math.Round(ProjectInvoicedToDate, 2) + Math.Round(ProjectUninvoicedToDate, 2)) / ProjectBudgetValue) * 100 : 0;
                return _projectBudgetValueWarningPercentage = value > 0 && value >= ProjectBudgetWarning ? Math.Round(value, 2) : 0;
            }
        }

       
        public decimal ProjectBudgetHourWarningPercentage
        {
            get
            {
                var value = ProjectBudgetHoursUnit > 0 ? ((Math.Round(ProjectHoursInvoicedToDate, 2) + Math.Round(ProjectHoursUninvoicedToDate, 2)) / ProjectBudgetHoursUnit) * 100 : 0;
                return _projectBudgetHoursWarningPercentage = value > 0 && value >= ProjectBudgetHoursWarning ? Math.Round(value, 2) : 0;
            }
        }
        [AuditNameAttribute("Assignment Parent Contract Discount")]
        public decimal? AssignmentParentContractDiscount { get; set; }

        [AuditNameAttribute("Assignment Parent Contract Company")]
        public string AssignmentParentContractCompany { get; set; }
        
        [AuditNameAttribute("Assignment Parent Contract Company Code")]
        public string AssignmentParentContractCompanyCode { get; set; }

        [AuditNameAttribute("Customer Direct Reporting Email Address")]
        public string CustomerDirectReportingEmailAddress { get; set; }

        public List<string> ProjectReferences { get; set; }

        public int? Evolution1Id { get; set; }

        public int? MasterDataTypeId { get; set; }
    }
}
