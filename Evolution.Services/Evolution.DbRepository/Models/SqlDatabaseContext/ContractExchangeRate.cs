using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ContractExchangeRate
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
