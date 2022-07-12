using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{ 
    public class CompanyEmailTemplate : BaseModel
    {
        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }
        

        [AuditNameAttribute("Customer Reporting Notification Email Text ")]
        public string CustomerReportingNotificationEmailText { get; set; }

        [AuditNameAttribute(" Customer Direct Reporting Email Text")]
        public string CustomerDirectReportingEmailText { get; set; }

        [AuditNameAttribute("Reject Visit Timesheet Email Text")]
        public string RejectVisitTimesheetEmailText { get; set; }

        [AuditNameAttribute(" Visit Completed Coordinator Email Text")]
        public string VisitCompletedCoordinatorEmailText { get; set; }

        [AuditNameAttribute(" Inter Company Operating Coordinator Email")]
        public string InterCompanyOperatingCoordinatorEmail { get; set; }

        [AuditNameAttribute(" Inter Company Discount Amendment Reason Email")]
        public string InterCompanyDiscountAmendmentReasonEmail { get; set; }



    }
}