using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.Security.Infrastructure.Data
{
    public class RoleDetailRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.RoleActivity>,
                                        IRoleDetailRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public RoleDetailRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<RoleDetail> Search(RoleInfo model)
        {
            var dbSearchRole = this._mapper.Map<Role>(model);
            var whereClause = dbSearchRole.ToExpression();
            IQueryable<Role> roles = null;

            if (whereClause == null)
                roles = this._dbContext.Role;
            else
                roles = this._dbContext.Role.Where(whereClause);

            return SearchRoleDetail(roles);
        }

        public IList<RoleDetail> Search(IList<string> roleNames)
        {
            IQueryable<Role> roles = null;
            roleNames = roleNames?.Select(x => x.Trim()).ToList();
            if (roleNames == null && roleNames.Count <= 0)
                roles = this._dbContext.Role;
            else
                roles = this._dbContext.Role.Where(x => roleNames.Contains(x.Name));

            return SearchRoleDetail(roles);
        }

        private IList<RoleDetail> SearchRoleDetail(IQueryable<Role> roles)
        {
            var roleModuleActivityIds = roles.Join(_dbContext.RoleActivity,
                                         r => r.Id,
                                         ra => ra.RoleId,
                                         (r, ra) => new
                                         {
                                             Role = r,
                                             ActivityId = ra.ActivityId,
                                             ModuleId = ra.ModuleId
                                         });

            var roleByModuleActivity = roleModuleActivityIds
                                         .Join(_dbContext.Module,
                                                ram => ram.ModuleId,
                                                m => m.Id,
                                                (ram, m) => new
                                                {
                                                    Role = ram.Role,
                                                    Module = m,
                                                    ActivityId = ram.ActivityId
                                                })
                                         .Join(_dbContext.Activity,
                                                rm => rm.ActivityId,
                                                a => a.Id,
                                                (rm, a) => new { Role = rm.Role, Module = rm.Module, Activity = a });

            var groupByRoleModule = roleByModuleActivity
                                    .GroupBy(x => x.Role).ToList()
                                    .Select(x1 => new RoleDetail()
                                    {
                                        Role = _mapper.Map<RoleInfo>(x1.Key),
                                        Modules = x1.GroupBy(x2 => x2.Module)
                                                  .Select(x3 => new RoleModuleActivity()
                                                  {
                                                      Module = _mapper.Map<ModuleInfo>(x3.Key),
                                                      Activities = _mapper.Map<IList<ActivityInfo>>(x3.Select(x4 => x4.Activity))
                                                  }).ToList()
                                    })
                                    .ToList();
            return groupByRoleModule;
        }

    }
}
