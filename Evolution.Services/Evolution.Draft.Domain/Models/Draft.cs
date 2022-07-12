using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Draft.Domain.Models
{
    public class Draft : BaseModel
    {

        public int Id { get; set; }
        [AuditNameAttribute("Draft Id")]
        public string DraftId { get; set; }
        [AuditNameAttribute("Module type")]
        public string Moduletype { get; set; }
        [AuditNameAttribute("Description")]
        public string Description { get; set; }
        [AuditNameAttribute("Serilizable Object")]
        public string SerilizableObject { get; set; }
        public string SerilizationType { get; set; }
        [AuditNameAttribute("Assigned By")]
        public string AssignedBy { get; set; }
        [AuditNameAttribute("Assigned To")]
        public string AssignedTo { get; set; } 
        [AuditNameAttribute("Assigned On")]
        public DateTime? AssignedOn { get; set; }
        [AuditNameAttribute("CreatedBy")]
        public string CreatedBy { get; set; }
        [AuditNameAttribute("CreatedOn")]
        public DateTime? CreatedOn { get; set; } 
        public string DraftType  { get; set; }
        public IList<string> AssignedToUsers{get; set;}

        [AuditNameAttribute("CompanyCode")]
        public string CompanyCode { get; set; } //D661 issue1 myTask CR
        public string CompanyName { get; set; } //D363 CR Change

        public string PendingWithUser { get; set; }//D1301
    }
}
