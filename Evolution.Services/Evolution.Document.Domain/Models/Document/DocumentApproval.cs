using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Document.Domain.Models.Document
{
    public class DocumentApproval : BaseModel
    {
        public int Id { get; set; }

        public int DocumentSourceId { get; set; }

        public string DocumentSourceModule { get; set; }

        public int DocumentTargetId { get; set; }

        public string DocumentTargetModule { get; set; }

        public string DocumentName { get; set; }

        public string DocumentType { get; set; }

        public long? DocumentSize { get; set; }

        public DateTime DocumentUploadedDate { get; set; }

        public DateTime? DocumentApprovedDate { get; set; }

        public string DocumentApprovedBy { get; set; }

        public string DocumentUploadedBy { get; set; }

        public bool IsSpecialistVisible { get; set; }

        public bool IsApproved { get; set; }

        public string DocumentUploadId { get; set; }
    }
}
