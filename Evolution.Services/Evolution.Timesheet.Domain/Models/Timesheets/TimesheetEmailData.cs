using Evolution.Email.Models;
using System;
using System.Collections.Generic;
namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetEmailData
    {
        public TimesheetDetail TimesheetDetail { get; set; }
        public string ReasonForRejection { get; set; }
        public string RejectionDate { get; set; }
        public bool? IsValidationRequired { get; set; }
        public string EmailContent { get; set; }
        public bool IsProcessNotification { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<EmailAddress> ToAddress { get; set; }
        public string EmailSubject { get; set; }
        public bool IsAuditUpdate { get; set; }
    }

    public class CustomerReportingNotification
    {
        public Timesheet TimesheetInfo { get; set; }
        public string EmailContent { get; set; }
        public string EmailSubject { get; set; }
        public List<EmailAddress> ToAddress { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class TimesheetTechInfo
    {
        private string _FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return _FullName = FirstName + " " + MiddleName + " " + LastName;
            }
        }
        public int Pin { get; set; }
        public ResourceAdditionalInfo resourceAdditionalInfo { get; set; }
    }

    public class ResourceInfo
    {
        public string TechSpecName { get; set; }
        public int Pin { get; set; }
        public List<ResourceAdditionalInfo> ResourceAdditionalInfos { get; set; }
    }

    public class ResourceAdditionalInfo
    {
        private string _TimesheetJobReference { get; set; }
        public int? TimesheetProjectNumber { get; set; }
        public int TimesheetAssignmentId { get; set; }
        public int TimesheetAssignmentNumber { get; set; }
        public int TimesheetNumber { get; set; }
        public Int64 TimesheetId { get; set; }
        public string TimesheetJobReference
        {
            get
            {
                return _TimesheetJobReference = TimesheetProjectNumber + "-" + TimesheetAssignmentNumber + "-" + TimesheetNumber;
            }
        }
    }
}