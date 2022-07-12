using Evolution.Common.Models.Base;

namespace Evolution.Project.Domain.Models.Projects
{
    public class ProjectClientNotification : BaseModel
    {
        [AuditNameAttribute("Project Client Notification Id")]
        public int ProjectClientNotificationId { get; set; }

        [AuditNameAttribute("Project Number")]
        public int? ProjectNumber { get; set; }

        [AuditNameAttribute("Customer Contact")]
        public string CustomerContact { get; set; }

        [AuditNameAttribute("Is Send Inspection Release Notes Notification ")]
        public bool IsSendInspectionReleaseNotesNotification { get; set; }

        [AuditNameAttribute("Is Send Flash Reporting Notification ")]
        public bool IsSendFlashReportingNotification { get; set; }
    
        [AuditNameAttribute("Is Send NCR Reporting Notification ")]
        public bool IsSendNCRReportingNotification { get; set; }

        [AuditNameAttribute("Is Send Customer Reporting Notification")]
        public bool IsSendCustomerReportingNotification { get; set; }
        
        [AuditNameAttribute("Is Send Customer Direct Reporting Notification")]
        public bool IsSendCustomerDirectReportingNotification { get; set; }

        public string EmailAddress { get; set; }

    }
}
