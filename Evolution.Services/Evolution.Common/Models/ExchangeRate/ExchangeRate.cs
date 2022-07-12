using System;
using System.Collections.Generic;

namespace Evolution.Common.Models.ExchangeRate
{
    public class ExchangeRate
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class ContractExchangeRate : ExchangeRate
    {
        public int ContractId { get; set; }
    }
     public class ExpenseExchangeRate 
    {
        public string ContractNumber { get; set; }
        public IList<ExchangeRate> ExchangeRates { get; set; }
    }
}
