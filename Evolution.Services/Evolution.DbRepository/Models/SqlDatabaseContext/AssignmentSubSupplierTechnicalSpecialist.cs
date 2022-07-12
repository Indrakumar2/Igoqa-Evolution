using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentSubSupplierTechnicalSpecialist
    {
        public int Id { get; set; }
        public int AssignmentSubSupplierId { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual AssignmentSubSupplier AssignmentSubSupplier { get; set; }
        public virtual AssignmentTechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
