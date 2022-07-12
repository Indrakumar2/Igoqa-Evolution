using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SupplierContact
    {
        public SupplierContact()
        {
            AssignmentSubSupplier = new HashSet<AssignmentSubSupplier>();
        }

        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierContactName { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string OtherContactDetails { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<AssignmentSubSupplier> AssignmentSubSupplier { get; set; }
    }
}
