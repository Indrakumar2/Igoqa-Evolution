using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ProjectInvoiceAttachment
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data DocumentType { get; set; }
        public virtual Project Project { get; set; }
    }
}
