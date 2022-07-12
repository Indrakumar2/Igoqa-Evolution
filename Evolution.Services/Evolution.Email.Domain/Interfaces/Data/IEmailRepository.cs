using Evolution.GenericDbRepository.Interfaces;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Email.Domain.Interfaces.Data
{
    public interface IEmailRepository : IGenericRepository<DbModel.Email>
    {
    }
}
