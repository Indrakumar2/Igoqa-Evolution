using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentSubSupplier
    {
        public AssignmentSubSupplier()
        {
            AssignmentSubSupplierTechnicalSpecialist = new HashSet<AssignmentSubSupplierTechnicalSpecialist>();
        }

        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int? SupplierId { get; set; }
        public int? SupplierContactId { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsFirstVisit { get; set; }
        public string SupplierType { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual SupplierContact SupplierContact { get; set; }
        public virtual ICollection<AssignmentSubSupplierTechnicalSpecialist> AssignmentSubSupplierTechnicalSpecialist { get; set; }
    }
}
