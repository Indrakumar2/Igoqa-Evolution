using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractExchangeRateRepository: IGenericRepository<DbModel.ContractExchangeRate>
    {
        IList<DomainModel.ContractExchangeRate> Search(DomainModel.ContractExchangeRate searchModel);

        IList<Common.Models.ExchangeRate.ContractExchangeRate> GetContractExchangeRates(IList<int> contractId);

        int DeleteExchangeRate(List<int> exchangeRateIds);

        int DeleteExchangeRate(List<DbModel.ContractExchangeRate> contractExchangeRates);

        int DeleteExchangeRate(List<DomainModel.ContractExchangeRate> contractExchangeRates);

    }
}
