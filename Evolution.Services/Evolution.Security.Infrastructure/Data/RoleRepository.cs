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
    public class RoleRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Role>, IRoleRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public RoleRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<Role> Search(DomainModel.Security.RoleInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Role>(model);
            var whereClause = dbSearchModel.ToExpression();
            if (whereClause == null)
                return this._dbContext.Role.ToList();
            else
                return this._dbContext.Role.Where(whereClause).ToList();
        }
        
        public IList<Role> Get(IList<int> ids)
        {
            if (ids?.Count > 0)
            {
                ids = ids.ToList();
                return this._dbContext.Role.Where(x => ids.Contains((int)x.Id)).ToList();
            }
            else
                return null;
        }

        public IList<Role> Get(IList<string> names)
        {
            if (names?.Count > 0)
            {
                names = names.ToList();
                return this._dbContext.Role.Where(x => names.Contains(x.Name)).ToList();
            }
            else
                return null;
        }
    }
}
