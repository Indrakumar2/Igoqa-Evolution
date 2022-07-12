using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistTimeOffRequest : BaseModel
    {
        [AuditNameAttribute("Resource Timeoff Request Id")]
        public int? TechnicalSpecialistTimeOffRequestId { get; set; }
        [AuditNameAttribute("Technical Specialist Time Of fRequest Technical Specialist Id")]
        public int TechnicalSpecialistId { get; set; }
        [AuditNameAttribute("Resource Name ")]
        public string ResourceName { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Leave Category Type")]
        public string LeaveCategoryType { get; set; }
        [AuditNameAttribute("Time Off From", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime TimeOffFrom { get; set; }
        [AuditNameAttribute("Time Off To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime TimeOffThrough { get; set; }
        [AuditNameAttribute("Comments")]
        public string Comments { get; set; }
        [AuditNameAttribute("Approval Status")]
        public string ApprovalStatus { get; set; }
        [AuditNameAttribute("Approved By")]
        public string ApprovedBy { get; set; }

        //from token
        [AuditNameAttribute("User Types")]
        public IList<string> UserTypes { get; set; }
        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }
        [AuditNameAttribute("Requested By")]
        public string RequestedBy { get; set; }
        [AuditNameAttribute("Requested On", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime RequestedOn { get; set; }
    }
}
