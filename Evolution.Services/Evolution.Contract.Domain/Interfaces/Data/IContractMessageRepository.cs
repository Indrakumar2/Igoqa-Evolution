using Evolution.GenericDbRepository.Interfaces;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractMessageRepository : IGenericRepository<DbModel.ContractMessage>
    {
    }
}
