using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class VisitDocument
    {
        public int Id { get; set; }
        public long VisitId { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public bool? IsVisibleToCustomer { get; set; }
        public bool? IsVisibleToTechnicalSpecialist { get; set; }
        public bool? IsInProgress { get; set; }
        public long? DocumentSize { get; set; }
        public DateTime? LastModification { get; set; }
        public DateTime? UploadedOn { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string UploadedDataId { get; set; }
        public string DocumentStatus { get; set; }

        public virtual Visit Visit { get; set; }
    }
}
