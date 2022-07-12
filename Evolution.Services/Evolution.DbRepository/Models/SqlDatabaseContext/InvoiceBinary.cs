using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceBinary
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Datetime { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
