using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Security.Infrastructure.Data
{
    public class ModuleDetailRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.ModuleActivity>, IModuleDetailRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public ModuleDetailRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<ModuleDetail> Search(ModuleInfo model)
        {
            var dbSearchModel = this._mapper.Map<Module>(model);
            var whereClause = dbSearchModel.ToExpression();
            IQueryable<Module> modules = null;

            if (whereClause == null)
                modules = this._dbContext.Module;
            else
                modules = this._dbContext.Module.Where(whereClause);

            var moduleActivities = _dbContext.ModuleActivity.Join(modules,
                                                     outer => outer.MouduleId,
                                                     inner => inner.Id,
                                                     (moduleActivity, module) => new
                                                     {
                                                         module,
                                                         moduleActivity
                                                     })
                                               .Join(_dbContext.Activity,
                                                     outer => outer.moduleActivity.ActivityId,
                                                     inner => inner.Id,
                                                     (parent, activity) => new
                                                     {
                                                         parent.module,
                                                         activity
                                                     }).ToList();
            return moduleActivities.Select(x => x.module)
                         .Distinct()
                         .Select(module => new ModuleDetail()
                         {
                             Module = _mapper.Map<ModuleInfo>(module),
                             Activities = _mapper.Map<IList<ActivityInfo>>(moduleActivities
                                                                                .Where(x1 => x1.module.Id == module.Id)
                                                                                .Select(x1 => x1.activity)
                                                                          ).ToList()
                         }).ToList();
        }
    }
}
