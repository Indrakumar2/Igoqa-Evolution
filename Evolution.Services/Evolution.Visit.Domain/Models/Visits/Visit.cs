using System;
using System.Collections.Generic;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class Visit : BaseVisit
    {
        [AuditNameAttribute("Visit Assignment Reference")]
        public string VisitAssignmentReference { get; set; }
        [AuditNameAttribute("Visit Assignment CreatedDate", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitAssignmentCreatedDate { get; set; }
        public long ParentVisitId { get; set; }
        [AuditNameAttribute("Skelton Visit")]
        public string SkeltonVisit { get; set; }
        [AuditNameAttribute("Approved By Contract Company")]
        public bool? VisitApprovedByContractCompany { get; set; }
        [AuditNameAttribute("Visit Review Status By Client")]
        public string VisitReviewStatusByClient { get; set; }
        [AuditNameAttribute("Visit Reviewed By Client")]
        public string VisitReviewedByClient { get; set; }
        [AuditNameAttribute("Visit Review Date By Client","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitReviewDateByClient { get; set; }
        [AuditNameAttribute("Percentage Complete")]
        public int? VisitCompletedPercentage { get; set; }
        [AuditNameAttribute("Date Period")]
        public string VisitDatePeriod { get; set; }
        [AuditNameAttribute("Visit To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitEndDate { get; set; }
        [AuditNameAttribute("Expected Complete Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitExpectedCompleteDate { get; set; }
        [AuditNameAttribute("Visit Ref 1")]
        public string VisitReference1 { get; set; }
        [AuditNameAttribute("Visit Ref 2")]
        public string VisitReference2 { get; set; }
        [AuditNameAttribute("Visit Ref 3")]
        public string VisitReference3 { get; set; }
        [AuditNameAttribute("Rejection Reason")]
        public string VisitRejectionReason { get; set; }
       
        [AuditNameAttribute("Assignment Client Reporting Requirements")]
        public string AssignmentClientReportingRequirements { get; set; }
        [AuditNameAttribute("Assignment Project Business Unit")]
        public string AssignmentProjectBusinessUnit { get; set; }

        /* Commented as becuase this is ambiguos, if need please discuss*/
        //[AuditNameAttribute("Visit Project Invoice Instruction Notes")]
        //public string ProjectInvoiceInstructionNotes { get; set; }
        [AuditNameAttribute("Visit Summary Of Report")]
        public string SummaryOfReport { get; set; }
        
        public bool IsExtranetSummaryReportVisible { get; set; }

        public bool? IsVisitOnPopUp { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Contract Holding Company Active")]
        public bool IsContractHoldingCompanyActive { get; set; }

        public string AmendmentReason { get; set; }

    }
    //this class is used to maintain visit search results
    public class VisitSearchResults : Visit
    {

    }
}
