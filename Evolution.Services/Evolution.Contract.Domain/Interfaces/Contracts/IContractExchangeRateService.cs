using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractExchangeRateService
    { 
        Response SaveContractExchangeRate(string contractNumber, IList<Models.Contracts.ContractExchangeRate> contractExchangeRatess, bool commitChange = true, bool isResultSetRequired = false);

        Response SaveContractExchangeRate(string contractNumber, IList<Models.Contracts.ContractExchangeRate> contractExchangeRatess, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule,bool IsFixedExchangeRateUsed = true,bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyContractExchangeRate(string contractNumber, IList<Models.Contracts.ContractExchangeRate> contractExchangeRatess, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyContractExchangeRate(string contractNumber, IList<Models.Contracts.ContractExchangeRate> contractExchangeRatess, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response GetContractExchangeRate(Models.Contracts.ContractExchangeRate searchModel);

        Response DeleteContractExchangeRate(string contractNumber, IList<Models.Contracts.ContractExchangeRate> deleteModel,bool commitChange = true, bool isResultSetRequired = false);

        Response DeleteContractExchangeRate(string contractNumber, IList<Models.Contracts.ContractExchangeRate> deleteModel, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool IsFixedExchangeRateUsed = true,  bool commitChange = true, bool isResultSetRequired = false);

        Response GetContractExchangeRates(IList<int> contractIds);

        decimal FetchExchangeRate(int contractId, string currencyFrom, string currencyTo, ref IList<ExchangeRate> exchangeRates, ref IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates);
    }
}
