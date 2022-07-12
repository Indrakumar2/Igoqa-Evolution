using Evolution.Common.Models.Notes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Supplier.Domain.Models.Supplier
{
    public class SupplierNote : Note
    {
        [AuditNameAttribute("Supplier Note Id ")]
        public int? SupplierNoteId { get; set; }
        [AuditNameAttribute("Supplier Id")]
        public int? SupplierId { get; set; }
        [AuditNameAttribute("Supplier Name")]
        public string SupplierName { get; set; }
    }
}
