using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceMessage
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public DateTime LastModification { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual InvoiceMessageType MessageType { get; set; }
    }
}
