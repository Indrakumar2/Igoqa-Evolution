using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SupplierPurchaseOrderDocumentOrpToDelete
    {
        public int Id { get; set; }
        public int SupplierPurchaseOrderId { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public long? DocumentSize { get; set; }
        public DateTime? UploadedOn { get; set; }
        public bool? IsInProgress { get; set; }
        public bool IsVisibleToCustomer { get; set; }
        public bool IsVisibleToTechnicalSpecialist { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string UploadedDataId { get; set; }
        public string DocumentStatus { get; set; }

        public virtual SupplierPurchaseOrder SupplierPurchaseOrder { get; set; }
    }
}
