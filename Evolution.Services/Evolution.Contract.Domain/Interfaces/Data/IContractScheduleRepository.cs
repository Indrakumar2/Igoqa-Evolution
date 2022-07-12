using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractScheduleRepository : IGenericRepository<DbModel.ContractSchedule>
    {
        IList<DomainModel.ContractSchedule> Search(DomainModel.ContractSchedule searchModel);

        int DeleteSchedule(List<int> scheduleIds);

        int DeleteSchedule(List<DbModel.ContractSchedule> contractSchedules);

        int DeleteSchedule(List<DomainModel.ContractSchedule> contractSchedules);
    }
}
