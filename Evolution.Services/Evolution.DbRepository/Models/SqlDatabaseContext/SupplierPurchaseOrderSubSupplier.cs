using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SupplierPurchaseOrderSubSupplier
    {
        public int Id { get; set; }
        public int SupplierPurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual SupplierPurchaseOrder SupplierPurchaseOrder { get; set; }
    }
}
