using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using Dbmodel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICurrencyExchangeRateService :IMasterService
    {
        Response Search(Models.CurrencyExchangeRates search);
        Response GetExchangeRates(IList<ExchangeRate> models, IList<Dbmodel.Data> dbCurrency = null);
        Response GetCurrencyExchangeRates(IList<Domain.Models.SearchCurrencyExchangeRate> models);
        Response GetMiiwaExchangeRates(IList<ExchangeRate> models);
    }
}
