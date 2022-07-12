using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SupplierPurchaseOrder
    {
        public SupplierPurchaseOrder()
        {
            Assignment = new HashSet<Assignment>();
            SupplierPurchaseOrderNote = new HashSet<SupplierPurchaseOrderNote>();
            SupplierPurchaseOrderSubSupplier = new HashSet<SupplierPurchaseOrderSubSupplier>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string SupplierPonumber { get; set; }
        public string MaterialDescription { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
        public decimal? BudgetValue { get; set; }
        public int? BudgetWarning { get; set; }
        public decimal BudgetHoursUnit { get; set; }
        public int? BudgetHoursUnitWarning { get; set; }
        public int SupplierId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Project Project { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<SupplierPurchaseOrderNote> SupplierPurchaseOrderNote { get; set; }
        public virtual ICollection<SupplierPurchaseOrderSubSupplier> SupplierPurchaseOrderSubSupplier { get; set; }
    }
}
