using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Supplier.Domain.Models.Supplier
{
    public class SupplierContact : BaseModel
    {
        [AuditNameAttribute("Supplier Contact Id")]
        public int? SupplierContactId { get; set; }
        [AuditNameAttribute("Supplier Contact Name ")]
        public string SupplierContactName { get; set; }
        [AuditNameAttribute("Telephone Number")]
        public string SupplierTelephoneNumber { get; set; }
        [AuditNameAttribute("Fax Number")]
        public string SupplierFaxNumber { get; set; }
        [AuditNameAttribute("Mobile Number")]
        public string SupplierMobileNumber { get; set; }
        [AuditNameAttribute("Supplier Email")]
        public string SupplierEmail { get; set; }
        [AuditNameAttribute(" Other Contact Details")]
        public string OtherContactDetails { get; set; }
        [AuditNameAttribute("Supplier Id")]
        public int? SupplierId { get; set; }
        [AuditNameAttribute("Supplier Name")]
        public string SupplierName { get; set; }
    }
}
