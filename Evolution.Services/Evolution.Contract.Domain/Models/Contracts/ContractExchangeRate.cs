using Evolution.Common.Models.Base;
using System;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractExchangeRate : BaseModel
    {
        [AuditNameAttribute("Exchange Rate Id")]
        public int ExchangeRateId { get; set; }

        [AuditNameAttribute("Contract Number")]
        public string ContractNumber { get; set; }
        
        [AuditNameAttribute("From Currency")]
        public string FromCurrency { get; set; }

        [AuditNameAttribute(" To Currency")]
        public string ToCurrency { get; set; }

        [AuditNameAttribute(" Effective From","dd-MMM-yyyy",AuditNameformatDataType.DateTime)]
        public DateTime EffectiveFrom { get; set; }

        [AuditNameAttribute(" Exchange Rate")]
        public decimal? ExchangeRate { get; set; }
        
    }
}
