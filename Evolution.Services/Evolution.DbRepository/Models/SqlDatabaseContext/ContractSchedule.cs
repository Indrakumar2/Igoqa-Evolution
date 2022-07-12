using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ContractSchedule
    {
        public ContractSchedule()
        {
            AssignmentContractSchedule = new HashSet<AssignmentContractSchedule>();
            AssignmentTechnicalSpecialistSchedule = new HashSet<AssignmentTechnicalSpecialistSchedule>();
            ContractRate = new HashSet<ContractRate>();
        }

        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Name { get; set; }
        public string ScheduleNoteForInvoice { get; set; }
        public string Currency { get; set; }
        public int? BaseScheduleId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ICollection<AssignmentContractSchedule> AssignmentContractSchedule { get; set; }
        public virtual ICollection<AssignmentTechnicalSpecialistSchedule> AssignmentTechnicalSpecialistSchedule { get; set; }
        public virtual ICollection<ContractRate> ContractRate { get; set; }
    }
}
