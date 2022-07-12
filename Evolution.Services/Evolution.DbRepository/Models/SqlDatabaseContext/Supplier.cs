using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Supplier
    {
        public Supplier()
        {
            AssignmentSubSupplier = new HashSet<AssignmentSubSupplier>();
            SupplierContact = new HashSet<SupplierContact>();
            SupplierNote = new HashSet<SupplierNote>();
            SupplierPurchaseOrder = new HashSet<SupplierPurchaseOrder>();
            SupplierPurchaseOrderSubSupplier = new HashSet<SupplierPurchaseOrderSubSupplier>();
            Visit = new HashSet<Visit>();
        }

        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string PostalCode { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? CountyId { get; set; }
        public int? CountryId { get; set; }

        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual County County { get; set; }
        public virtual ICollection<AssignmentSubSupplier> AssignmentSubSupplier { get; set; }
        public virtual ICollection<SupplierContact> SupplierContact { get; set; }
        public virtual ICollection<SupplierNote> SupplierNote { get; set; }
        public virtual ICollection<SupplierPurchaseOrder> SupplierPurchaseOrder { get; set; }
        public virtual ICollection<SupplierPurchaseOrderSubSupplier> SupplierPurchaseOrderSubSupplier { get; set; }
        public virtual ICollection<Visit> Visit { get; set; }
    }
}
