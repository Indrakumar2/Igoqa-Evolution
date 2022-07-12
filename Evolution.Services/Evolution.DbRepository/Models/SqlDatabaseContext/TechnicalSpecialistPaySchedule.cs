using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistPaySchedule
    {
        public TechnicalSpecialistPaySchedule()
        {
            AssignmentTechnicalSpecialistSchedule = new HashSet<AssignmentTechnicalSpecialistSchedule>();
            TechnicalSpecialistPayRate = new HashSet<TechnicalSpecialistPayRate>();
        }

        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string PayScheduleName { get; set; }
        public string PayScheduleNote { get; set; }
        public string PayCurrency { get; set; }
        public bool IsActive { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
        public virtual ICollection<AssignmentTechnicalSpecialistSchedule> AssignmentTechnicalSpecialistSchedule { get; set; }
        public virtual ICollection<TechnicalSpecialistPayRate> TechnicalSpecialistPayRate { get; set; }
    }
}
