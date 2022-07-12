using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.SupplierPO.Domain.Models.SupplierPO
{
    public class SupplierPODetail
    {
         [AuditNameAttribute("Supplier Info")]
        public SupplierPO SupplierPOInfo { get; set; }

        public IList<SupplierPOSubSupplier> SupplierPOSubSupplier { get; set; }

        public IList<ModuleDocument> SupplierPODocuments { get; set; }

        public IList<SupplierPONote> SupplierPONotes { get; set; }
    }
}
