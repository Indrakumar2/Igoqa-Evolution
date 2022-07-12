using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceExchangeRate
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
