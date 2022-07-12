using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ContractInvoiceAttachment
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual Data DocumentType { get; set; }
    }
}
