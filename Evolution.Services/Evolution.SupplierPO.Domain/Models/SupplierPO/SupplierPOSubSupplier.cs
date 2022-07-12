using Evolution.Common.Models.Base;

namespace Evolution.SupplierPO.Domain.Models.SupplierPO
{
    public class SupplierPOSubSupplier : BaseModel
    {
        [AuditNameAttribute("Sub-Supplier Id")]
        public int? SubSupplierId { get; set; }

        [AuditNameAttribute("Supplier PO Id")]
        public int? SupplierPOId { get; set; }

        [AuditNameAttribute("Supplier Id")]
        public int? SupplierId { get; set; }
        [AuditNameAttribute("Sub-Supplier Name")]
        public string SubSupplierName { get; set; }
        [AuditNameAttribute("Sub-Supplier Address")]
        public string SubSupplierAddress { get; set; }
        [AuditNameAttribute("Supplier PO Number")]
        public string SupplierPONumber { get; set; }
        [AuditNameAttribute("Country")]
        public string Country { get; set; }
        [AuditNameAttribute("State")]
        public string State { get; set; }
        [AuditNameAttribute("City")]
        public string City { get; set; }
        [AuditNameAttribute("Postal Code")]
        public string PostalCode { get; set; }
    }
}
