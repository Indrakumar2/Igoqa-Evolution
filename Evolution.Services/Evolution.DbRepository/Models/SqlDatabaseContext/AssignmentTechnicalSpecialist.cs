using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentTechnicalSpecialist
    {
        public AssignmentTechnicalSpecialist()
        {
            AssignmentSubSupplierTechnicalSpecialist = new HashSet<AssignmentSubSupplierTechnicalSpecialist>();
            AssignmentTechnicalSpecialistSchedule = new HashSet<AssignmentTechnicalSpecialistSchedule>();
        }

        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public bool? IsSupervisor { get; set; }
        public DateTime? LastModification { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
        public virtual ICollection<AssignmentSubSupplierTechnicalSpecialist> AssignmentSubSupplierTechnicalSpecialist { get; set; }
        public virtual ICollection<AssignmentTechnicalSpecialistSchedule> AssignmentTechnicalSpecialistSchedule { get; set; }
    }
}
