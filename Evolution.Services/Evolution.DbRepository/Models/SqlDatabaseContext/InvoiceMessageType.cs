using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceMessageType
    {
        public InvoiceMessageType()
        {
            InvoiceMessage = new HashSet<InvoiceMessage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<InvoiceMessage> InvoiceMessage { get; set; }
    }
}
