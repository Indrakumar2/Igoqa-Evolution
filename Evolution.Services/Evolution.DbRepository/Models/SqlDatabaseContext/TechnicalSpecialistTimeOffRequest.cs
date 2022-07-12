using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistTimeOffRequest
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Comments { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }

        public virtual Data LeaveType { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
