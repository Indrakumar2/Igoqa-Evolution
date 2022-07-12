using System;
using System.Collections.Generic;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class Timesheet : BaseTimesheet
    {
        private string _timesheetJobReferenceNumber { get; set; }
        [AuditNameAttribute("Date Period")]
        public string TimesheetDatePeriod { get; set; }
        [AuditNameAttribute("Expected Complete Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetExpectedCompleteDate { get; set; }
        [AuditNameAttribute("Timesheet Ref 1")]
        public string TimesheetReference1 { get; set; }
        [AuditNameAttribute("Timesheet Ref 2")]
        public string TimesheetReference2 { get; set; }
        [AuditNameAttribute("Timesheet Ref 3")]
        public string TimesheetReference3 { get; set; }
        [AuditNameAttribute("Percentage Complete")]
        public int? TimesheetCompletedPercentage { get; set; }
        [AuditNameAttribute("Approved By Contract Company")]
        public bool? IsApprovedByContractCompany { get; set; }
        [AuditNameAttribute("Approved By")]
        public string TimesheetReviewBy { get; set; }
        [AuditNameAttribute("Review Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetReviewDate { get; set; }

        //public int AssignmentId { get; set; }
        [AuditNameAttribute("Assignment Client Reporting Requirements")]
        public string AssignmentClientReportingRequirements { get; set; }
        [AuditNameAttribute("Assignment Project BusinessUnit")]
        public string AssignmentProjectBusinessUnit { get; set; }

        public IList<TechnicalSpecialist> assignmentTechSpecialists { get; set; }

        // Will be removed after sync process
        [AuditNameAttribute("Evo Id")]
        public long? EvoId { get; set; }
        [AuditNameAttribute("Project Invoice Instruction Notes")]
        public string ProjectInvoiceInstructionNotes { get; set; }
        [AuditNameAttribute("Timesheet Job Reference")]
        public string TimesheetJobReference
        {
            get
            {
                return _timesheetJobReferenceNumber = TimesheetProjectNumber + "-" + TimesheetAssignmentNumber + "-" + TimesheetNumber;
            }
        }
        [AuditNameAttribute("Assignment Project Work Flow")]
        public string AssignmentProjectWorkFlow { get; set; }
        public string AssignmentStatus { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Contract Holding Company Active")]
        public bool IsContractHoldingCompanyActive { get; set; }

        public bool? IsVisitOnPopUp { get; set; }

        public bool IsAuditUpdate { get; set; }
    }
}
