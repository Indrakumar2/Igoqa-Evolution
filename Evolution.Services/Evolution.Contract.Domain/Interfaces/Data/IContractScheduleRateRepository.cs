using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractScheduleRateRepository: IGenericRepository<DbModel.ContractRate>
    {
        IList<DomainModel.ContractScheduleRate> Search(DomainModel.ContractScheduleRate searchModel);

        int DeleteContractRate(List<int> rateIds);

        int DeleteContractRate(List<DomainModel.ContractScheduleRate> contractScheduleRates);

        int DeleteContractRate(List<DbModel.ContractRate> contractRates);
    }
}
