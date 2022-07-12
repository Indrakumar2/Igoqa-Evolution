using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SupplierDocumentOrpToDelete
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public long? DocumentSize { get; set; }
        public DateTime? UploadedOn { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsvisibleToCustomer { get; set; }
        public bool IsVisibleToTechnicalSpecialist { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string UploadedDataId { get; set; }
        public string DocumentStatus { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
