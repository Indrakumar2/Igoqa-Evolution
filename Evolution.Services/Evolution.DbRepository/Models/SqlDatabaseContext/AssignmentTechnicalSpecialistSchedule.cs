using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentTechnicalSpecialistSchedule
    {
        public int Id { get; set; }
        public int AssignmentTechnicalSpecialistId { get; set; }
        public int ContractChargeScheduleId { get; set; }
        public int TechnicalSpecialistPayScheduleId { get; set; }
        public string ScheduleNoteToPrintOnInvoice { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual AssignmentTechnicalSpecialist AssignmentTechnicalSpecialist { get; set; }
        public virtual ContractSchedule ContractChargeSchedule { get; set; }
        public virtual TechnicalSpecialistPaySchedule TechnicalSpecialistPaySchedule { get; set; }
    }
}
