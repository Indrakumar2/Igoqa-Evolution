using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentReferenceType : BaseModel
    {
        [AuditNameAttribute("Assignment Reference Id")]
        public int? AssignmentReferenceTypeId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Reference Type")]
        public string ReferenceType { get; set; }
        
        [AuditNameAttribute("Reference Value ")]
        public string ReferenceValue { get; set; }


        public int? AssignmentReferenceTypeMasterId { get; set; } //For audit to show ReferenceType

    }
}