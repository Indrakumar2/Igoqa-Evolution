using System;
using System.Collections.Generic;
using Evolution.Common.Models.Base;
namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class BaseTimesheet : BaseModel
    {
        [AuditNameAttribute("TimeSheet Id")]
        public long? TimesheetId { get; set; }

        [AuditNameAttribute("Timesheet Number")]
        public int? TimesheetNumber { get; set; }
        
        [AuditNameAttribute("Assignment Number")]
        public int? TimesheetAssignmentNumber { get; set; }

        [AuditNameAttribute("Assignment Created Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? AssignmentCreatedDate { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int TimesheetAssignmentId { get; set; }

        [AuditNameAttribute("Contract Company Code")]
        public string TimesheetContractCompanyCode { get; set; }

        [AuditNameAttribute("Contract Company")]
        public string TimesheetContractCompany { get; set; }

        [AuditNameAttribute("Contract Coordinator")]
        public string TimesheetContractCoordinator { get; set; }

        [AuditNameAttribute("Contract Company Coordinator Code")]
        public string TimesheetContractCoordinatorCode { get; set; }

        [AuditNameAttribute("Customer Code")]
        public string TimesheetCustomerCode { get; set; }

        [AuditNameAttribute("Customer Name")]
        public string TimesheetCustomerName { get; set; }

        [AuditNameAttribute("Contract Number")]
        public string TimesheetContractNumber { get; set; }

        [AuditNameAttribute("Operating Company Code")]
        public string TimesheetOperatingCompanyCode { get; set; }

        [AuditNameAttribute("Operating Company")]
        public string TimesheetOperatingCompany { get; set; }

        [AuditNameAttribute("Operating Coordinator")]
        public string TimesheetOperatingCoordinator { get; set; }

        [AuditNameAttribute("Operating Coordinator Sam Acct Name")]
        public string TimesheetOperatingCoordinatorSamAcctName { get; set; }

        [AuditNameAttribute("Operating Company Coordinator Id")]
        public string TimesheetOperatingCoordinatorCode { get; set; }

        [AuditNameAttribute("Project Number")]
        public int? TimesheetProjectNumber { get; set; }

        [AuditNameAttribute("Customer Project Name")]
        public string CustomerProjectName { get; set; }

        [AuditNameAttribute("Timesheet Description")]
        public string TimesheetDescription { get; set; }

        [AuditNameAttribute("Timesheet Date To", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetEndDate { get; set; }

        [AuditNameAttribute("Future Days")]
        public int? TimesheetFutureDays { get; set; }

        [AuditNameAttribute("Timesheet Date From", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetStartDate { get; set; }

        [AuditNameAttribute("Timesheet Status")]
        public string TimesheetStatus { get; set; }
        [AuditNameAttribute("Unused Reason")]
        public string UnusedReason { get; set; }
        public IList<TechnicalSpecialist> TechSpecialists { get; set; }

        [AuditNameAttribute("Assignment Reference")]
        public string AssignmentReference { get; set; }

        public string DocumentApprovalVisitValue { get; set; }
    } 
}
