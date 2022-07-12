
using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentContractRateSchedule:BaseModel
   {
        [AuditNameAttribute("Contract Rate Schedule Id ")]
        public int? AssignmentContractRateScheduleId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Contract Schedule Id")]
        public int? ContractScheduleId { get; set; }
        
        [AuditNameAttribute("Contract Schedule Name ")]
        public string ContractScheduleName { get; set; }
    }

}
