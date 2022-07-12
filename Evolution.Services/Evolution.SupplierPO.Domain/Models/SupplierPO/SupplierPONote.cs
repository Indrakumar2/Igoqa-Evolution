using Evolution.Common.Models.Notes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.SupplierPO.Domain.Models.SupplierPO
{
    public class SupplierPONote : Note 
    {
         [AuditNameAttribute("Supplier PO Id")]
         public int? SupplierPOId { get; set; }
        [AuditNameAttribute("Note Id")]
        public int? SupplierPONoteId { get; set; }
          [AuditNameAttribute("Supplier PO Number")]
        public string SupplierPONumber { get; set; }
    }
}
