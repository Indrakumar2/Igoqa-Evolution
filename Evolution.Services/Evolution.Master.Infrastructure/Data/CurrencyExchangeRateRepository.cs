using Evolution.Common.Enums;
using Evolution.Common.Models.ExchangeRate;
using Dbmodel=Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class CurrencyExchangeRateRepository : GenericRepository<DbRepository.Models.SqlDatabaseContext.CurrencyExchangeRate>, ICurrencyExchangeRateRepository
    {
        private readonly Dbmodel.EvolutionSqlDbContext _dbContext = null;

        public CurrencyExchangeRateRepository(Dbmodel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<CurrencyExchangeRates> Search(CurrencyExchangeRates search)
        {
            return this.FindBy(x => (string.IsNullOrEmpty(search.Currency) || x.Currency.Name == search.Currency))
                 .Select(x => new CurrencyExchangeRates()
                 {
                     Currency = x.Currency.Name,
                     EffectiveDate = x.EffectiveDate,
                     ExchangeRateUSD = x.ExchangeRateUsd,
                     ExchangeRateGBP = x.ExchangeRateGbp,
                     AverageGBP = x.AverageGbp,
                     AverageUSD = x.AverageUsd,
                     CurrencyExchangeRateId = x.Id
                 }).ToList();

        }

        public IList<Domain.Models.CurrencyExchangeRates> GetCurrencyExchangeRates(IList<Domain.Models.SearchCurrencyExchangeRate> models, IList<Dbmodel.Data> dbCurrency = null)
        {
            List<Dbmodel.Data> curencyIds = null;
            List<Domain.Models.CurrencyExchangeRates> currencyExchangeRates = new List<Domain.Models.CurrencyExchangeRates>();

            var uniqueCurrency = models.Select(x1 => x1.Currency).Distinct().ToList();

            var currExchRateDbDetails = _dbContext.CurrencyExchangeRate
                                                .Where(x => uniqueCurrency.Contains(x.Currency.Code))
                                                .ToList();
            if (dbCurrency?.Count > 0)
                 curencyIds = dbCurrency.ToList()
                                        .Where(x => uniqueCurrency.Contains(x.Code) &&
                                               x.MasterDataTypeId == Convert.ToInt32(MasterType.Currency))
                                        .ToList();
            else
                curencyIds = _dbContext.Data
                                       .Where(x => uniqueCurrency.Contains(x.Code) &&
                                              x.MasterDataTypeId == Convert.ToInt32(MasterType.Currency))
                                       .ToList();

            var modelWithIds = models.Join(curencyIds,
                                            m => m.Currency,
                                            ids => ids.Code,
                                            (m, ids) => new
                                            {
                                                ids.Id,
                                                m.Currency,
                                                m.EffectiveDate
                                            });

            var modelWithMaxEffDate = modelWithIds.GroupJoin(currExchRateDbDetails,
                                                            m => m.Id,
                                                            cer => cer.CurrencyId,
                                                            (m, cer) => new
                                                            {
                                                                CurrencyId = m.Id,
                                                                Currency = m.Currency,
                                                                EffectiveDate = m.EffectiveDate,
                                                                MaxEffDate = cer.Where(f => f.EffectiveDate <= m.EffectiveDate &&
                                                                                       f.ExchangeRateUsd > 0)
                                                                                .Max(x => x.EffectiveDate)
                                                            });

            currencyExchangeRates = modelWithMaxEffDate.GroupJoin(currExchRateDbDetails,
                                                            m => m.CurrencyId,
                                                            cer => cer.CurrencyId,
                                                            (m, cer) => new CurrencyExchangeRates()
                                                            {
                                                                Currency = m.Currency,
                                                                EffectiveDate = m.EffectiveDate,
                                                                ExchangeRateUSD = cer.FirstOrDefault(x => x.EffectiveDate == m.MaxEffDate).ExchangeRateUsd,
                                                                AverageUSD = cer.FirstOrDefault(x => x.EffectiveDate == m.MaxEffDate).AverageUsd,
                                                                ExchangeRateGBP = cer.FirstOrDefault(x => x.EffectiveDate == m.MaxEffDate).ExchangeRateGbp,
                                                                AverageGBP = cer.FirstOrDefault(x => x.EffectiveDate == m.MaxEffDate).AverageGbp,
                                                                CurrencyExchangeRateId = cer.FirstOrDefault(x => x.EffectiveDate == m.MaxEffDate).Id
                                                            }).ToList();

            return currencyExchangeRates.ToList();
        }

        public IList<ExchangeRate> GetExchangeRates(IList<ExchangeRate> models, IList<Dbmodel.Data> dbCurrency = null)
        {
            var fromCurrencyInfo = models?.Select(x => new { x.CurrencyFrom, x.EffectiveDate }).Distinct().Select(x => new SearchCurrencyExchangeRate { Currency = x.CurrencyFrom, EffectiveDate = x.EffectiveDate }).ToList();
            var toCurrencyInfo = models?.Select(x => new { x.CurrencyTo, x.EffectiveDate }).Distinct().Select(x => new SearchCurrencyExchangeRate { Currency = x.CurrencyTo, EffectiveDate = x.EffectiveDate }).ToList();

            var currencies = fromCurrencyInfo.Union(toCurrencyInfo).ToList();

            IList<Domain.Models.CurrencyExchangeRates> currencyExchangeRates = GetCurrencyExchangeRates(currencies, dbCurrency);

            models.ToList().ForEach(x =>
                {
                    var fromToBaseExchangeRate = currencyExchangeRates?.FirstOrDefault(x1 => x1.Currency == x.CurrencyFrom && x1.EffectiveDate == x.EffectiveDate)?.ExchangeRateUSD ?? 0;
                    var toToBaseExchangeRate = currencyExchangeRates?.FirstOrDefault(x1 => x1.Currency == x.CurrencyTo && x1.EffectiveDate == x.EffectiveDate)?.ExchangeRateUSD ?? 0;
                    x.Rate = Math.Round((fromToBaseExchangeRate <= 0 ? 1 : fromToBaseExchangeRate) / (toToBaseExchangeRate <= 0 ? 1 : toToBaseExchangeRate),6);
                }
            );

            return models;
        }

        public IList<ExchangeRate> GetMiiwaExchangeRates(IList<ExchangeRate> models)
        {
            var fromCurrencyInfo = models?.Select(x => new { x.CurrencyFrom, x.EffectiveDate }).Distinct().Select(x => new SearchCurrencyExchangeRate { Currency = x.CurrencyFrom, EffectiveDate = x.EffectiveDate }).ToList();
            var toCurrencyInfo = models?.Select(x => new { x.CurrencyTo, x.EffectiveDate }).Distinct().Select(x => new SearchCurrencyExchangeRate { Currency = x.CurrencyTo, EffectiveDate = x.EffectiveDate }).ToList();

            var currencies = fromCurrencyInfo.Union(toCurrencyInfo).ToList();

            IList<Domain.Models.CurrencyExchangeRates> currencyExchangeRates = GetCurrencyExchangeRates(currencies);
            models.ToList().ForEach(x =>
                {
                    string[] option = {"GBP", "USD"};
                    decimal fromToBaseExchangeRate = default(decimal),toToBaseExchangeRate = default(decimal);
                    if (!option.Any(cur => cur == x.CurrencyFrom) && !option.Any(cur => cur == x.CurrencyTo)){
                    fromToBaseExchangeRate = currencyExchangeRates.FirstOrDefault(x1 => x1.Currency == x.CurrencyFrom).ExchangeRateUSD;
                    toToBaseExchangeRate = currencyExchangeRates.FirstOrDefault(x1 => x1.Currency == x.CurrencyTo).ExchangeRateUSD;
                    }
                    else if(x.CurrencyFrom == "USD" || x.CurrencyTo == "USD" ){
                    fromToBaseExchangeRate = currencyExchangeRates.FirstOrDefault(x1 => x1.Currency == x.CurrencyFrom).ExchangeRateUSD;
                    toToBaseExchangeRate = currencyExchangeRates.FirstOrDefault(x1 => x1.Currency == x.CurrencyTo).ExchangeRateUSD;
                    }
                    else{
                    fromToBaseExchangeRate = currencyExchangeRates.FirstOrDefault(x1 => x1.Currency == x.CurrencyFrom).ExchangeRateGBP;
                    toToBaseExchangeRate = currencyExchangeRates.FirstOrDefault(x1 => x1.Currency == x.CurrencyTo).ExchangeRateGBP;
                    }
                    // x.Rate = (fromToBaseExchangeRate<=0?1:fromToBaseExchangeRate) / (toToBaseExchangeRate <= 0 ? 1 : toToBaseExchangeRate);
                    x.Rate = (toToBaseExchangeRate <= 0 ? 1 : toToBaseExchangeRate)/(fromToBaseExchangeRate<=0?1:fromToBaseExchangeRate);
                }
            );
            return models;
        }
    }
}
