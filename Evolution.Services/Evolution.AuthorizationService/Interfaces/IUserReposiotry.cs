using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;

namespace Evolution.AuthorizationService.Interfaces
{
    public interface IUserReposiotry : IGenericRepository<User>
    {
        IList<Announcement> CheckEvoutionLocked();

        IList<Role> GetRoles(List<UserRole> dbUserRole);

        int GetRoleCount(List<UserRole> dbUserRole);
    }
}
