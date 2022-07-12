using Evolution.Email.Models;
using System;
using System.Collections.Generic;
namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitEmailData
    {
       public VisitDetail VisitDetail {get;set;}
       public bool isSendClientReportingNotification {get;set;}
       public string ReasonForRejection {get;set;}
       public bool? IsValidationRequired { get; set; }
       public string EmailContent { get; set; }
       public bool IsProcessNotification { get; set; }
       public List<Attachment> Attachments { get; set; }
       public List<EmailAddress> ToAddress { get; set; }
       public string EmailSubject { get; set; }
    }

    public class CustomerReportingNotification{
       public Visit VisitInfo {get;set;} 
       public string EmailContent {get;set;}
       public string EmailSubject {get;set;}
       public List<EmailAddress> ToAddress {get;set;}
       public List<Attachment> Attachments { get; set; }
    }

    public class VisitTechInfo
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
        private string _VisitJobReference { get; set; }
        public int? VisitProjectNumber { get; set; }
        public int VisitAssignmentId { get; set; }
        public int VisitAssignmentNumber { get; set; }
        public long VisitNumber { get; set; }
        public Int64 VisitId { get; set; }
        public int VisitSupplierPOId{ get; set; }
        public string VisitJobReference
        {
            get
            {
                return _VisitJobReference = VisitProjectNumber + "-" + VisitAssignmentNumber + "-" + VisitNumber;
            }
        }
    }
}