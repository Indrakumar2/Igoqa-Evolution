using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models;

namespace Evolution.Security.Infrastructure.Data
{
    public class ModuleRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Module>, IModuleRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        public readonly IMapper _mapper = null;

        public ModuleRepository(IMapper mapper,EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<Module> Search(DomainModel.Security.ModuleInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Module>(model);
            var whereClause = dbSearchModel.ToExpression();
            if (whereClause == null)
                return this._dbContext.Module.ToList();
            else
                return this._dbContext.Module.Where(whereClause).ToList();
        }

        public IList<Module> Get(IList<int> ids)
        {
            if (ids?.Count > 0)
            {
                ids = ids.ToList();
                return this._dbContext.Module.Where(x => ids.Contains(x.Id)).ToList();
            }
            else
                return null;
        }

        public IList<Module> Get(IList<string> names)
        {
            if (names?.Count > 0)
            {
                names = names.ToList();
                return this._dbContext.Module.Where(x => names.Contains(x.Name)).ToList();
            }
            else
                return null;
        }
    }
}
