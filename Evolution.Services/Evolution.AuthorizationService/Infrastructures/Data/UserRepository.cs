using Evolution.AuthorizationService.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.AuthorizationService.Infrastructures.Data
{
    public class UserRepository : GenericRepository<User>, IUserReposiotry
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public UserRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<Announcement> CheckEvoutionLocked()
        {
            return _dbContext.Announcement.Where(x => x.IsEvolutionLocked == true).Select(x=> new Announcement { IsEvolutionLocked=x.IsEvolutionLocked , EvolutionLockMessage =x.EvolutionLockMessage}).ToList();
        }

        public IList<Role> GetRoles(List<UserRole> dbUserRole)
        {
            return dbUserRole.Join(_dbContext.Role.ToList(),
                                               dbUsrRole => new { dbUsrRole.RoleId },
                                               dbRole => new { RoleId = dbRole.Id },
                                              (dbUsrRole, dbRole) => new { dbUsrRole, dbRole })
                                              .Where(x => x.dbRole.IsAllowedDuringLock == true)
                                              .Select(x => x.dbRole)
                                              .ToList();
        }


        public int GetRoleCount(List<UserRole> dbUserRole)
        {
            if(dbUserRole?.Any()== true)
            return dbUserRole.Join(_dbContext.Role.ToList(),
                                               dbUsrRole => new { dbUsrRole.RoleId },
                                               dbRole => new { RoleId = dbRole.Id },
                                              (dbUsrRole, dbRole) => new { dbUsrRole, dbRole })
                                              .Where(x => x.dbRole.IsAllowedDuringLock == true)
                                              .Select(x=> x.dbRole.Id)
                                              .Count();
            else return 0;
        }
    }
}
