using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentContractSchedule
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int ContractScheduleId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual ContractSchedule ContractSchedule { get; set; }
    }
}
