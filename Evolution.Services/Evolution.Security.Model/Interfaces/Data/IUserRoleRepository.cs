using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IUserRoleRepository : IGenericRepository<DbModel.UserRole>
    {
        IList<DbModel.UserRole> Get(DomainModel.UserRoleInfo searchModel, params string[] includes);
    }
}
