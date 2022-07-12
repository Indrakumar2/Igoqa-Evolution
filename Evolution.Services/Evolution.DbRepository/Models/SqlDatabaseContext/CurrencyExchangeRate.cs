using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CurrencyExchangeRate
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal ExchangeRateUsd { get; set; }
        public decimal ExchangeRateGbp { get; set; }
        public decimal? AverageUsd { get; set; }
        public decimal? AverageGbp { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual Data Currency { get; set; }
    }
}
