using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistSensitiveDocumentOrpToDelete
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Comments { get; set; }
        public int DisplayOrder { get; set; }
        public string UploadedDataId { get; set; }
        public string DocumentStatus { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
