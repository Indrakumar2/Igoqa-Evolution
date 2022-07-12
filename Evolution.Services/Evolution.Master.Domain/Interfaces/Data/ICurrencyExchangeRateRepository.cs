using Evolution.Common.Models.ExchangeRate;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using Dbmodel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ICurrencyExchangeRateRepository : IGenericRepository<DbRepository.Models.SqlDatabaseContext.CurrencyExchangeRate>
    {
        IList<Models.CurrencyExchangeRates> Search(Models.CurrencyExchangeRates search);

        IList<Domain.Models.CurrencyExchangeRates> GetCurrencyExchangeRates(IList<Domain.Models.SearchCurrencyExchangeRate> models, IList<Dbmodel.Data> dbCurrency = null);

        IList<ExchangeRate> GetExchangeRates(IList<ExchangeRate> models, IList<Dbmodel.Data> dbCurrency = null);
        IList<ExchangeRate> GetMiiwaExchangeRates(IList<ExchangeRate> models);
    }
}
