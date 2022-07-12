using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Common.Helpers
{
    public static class ExchangeRateClaculations
    {
        public static void CalculateExchangeRate(IList<ContractExchangeRate> contractExchangeRates,
                                           IList<ExchangeRate> currencyExchangeRate,
                                           IList<BudgetAccountItem> budgetAccountItems)
        {
            if (budgetAccountItems?.Count > 0)  //  && contractExchangeRates?.Count > 0 && currencyExchangeRate?.Count > 0
            {
                budgetAccountItems = budgetAccountItems
                                    .Where(x => x.ContractExchangeRate <= 0)
                                    .GroupJoin(contractExchangeRates,
                                            item => new
                                            {
                                                item.ContractId,
                                                CurrencyFrom = item.ChargeRateCurrency,
                                                CurrencyTo = item.BudgetCurrency
                                            },
                                            rate => new { rate.ContractId, rate.CurrencyFrom, rate.CurrencyTo },
                                            (item, rate) => new { item, rate })
                                    .Select(x =>
                                    {
                                        x.item.ContractExchangeRate = (x.rate.FirstOrDefault() != null ? x.rate.FirstOrDefault().Rate : 0);
                                        return x.item;
                                    }).ToList();
                //.Where(x => x.ExpenseType == "R" && x.AssignmentId == 280816)?
                budgetAccountItems = budgetAccountItems//.Where(x => x.ExpenseType == "R" && x.AssignmentId == 410677)
                                    .GroupJoin(currencyExchangeRate,
                                            item => new { CurrencyFrom = item.ChargeRateCurrency, CurrencyTo = item.BudgetCurrency ,VisitDate=item.VisitDate },
                                            rate => new { rate.CurrencyFrom, rate.CurrencyTo, VisitDate = rate.EffectiveDate },
                                            (item, rate) => new { item, rate })
                                    .Select(x =>
                                    {
                                        if (x.item.ContractExchangeRate <= 0)
                                            x.item.ContractExchangeRate = (x.rate.FirstOrDefault() != null ? x.rate.FirstOrDefault().Rate : 1);
                                        return x.item;
                                    }).ToList();
            }
        }
    }
}
