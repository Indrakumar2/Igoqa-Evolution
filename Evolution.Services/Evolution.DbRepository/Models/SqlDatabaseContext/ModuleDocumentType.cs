using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ModuleDocumentType
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public bool? Tsvisible { get; set; }

        public virtual Data DocumentType { get; set; }
        public virtual Data Module { get; set; }
    }
}
