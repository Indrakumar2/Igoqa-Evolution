using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class SearchCurrencyExchangeRate
    { 
        public string Currency { get; set; }

        public DateTime EffectiveDate { get; set; }
 
    }

    public class CurrencyExchangeRates: SearchCurrencyExchangeRate
    {
        public int? CurrencyExchangeRateId { get; set; }
          
        public decimal ExchangeRateUSD { get; set; }

        public decimal ExchangeRateGBP { get; set; }

        public decimal? AverageUSD { get; set; }

        public decimal? AverageGBP { get; set; }
    }
}