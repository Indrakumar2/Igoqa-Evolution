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
    public class ActivityRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Activity>, IActivityRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public ActivityRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<Activity> Search(DomainModel.Security.ActivityInfo model)
        {
            var dbSearchModel = this._mapper.Map<Activity>(model);
            var whereClause = dbSearchModel.ToExpression();
            if (whereClause == null)
                return this._dbContext.Activity.ToList();
            else
                return this._dbContext.Activity.Where(whereClause).ToList();
        }

        public IList<Activity> Get(IList<int> ids)
        {
            if (ids?.Count > 0)
            {
                ids = ids.ToList();
                return this._dbContext.Activity.Where(x => ids.Contains((int)x.Id)).ToList();
            }
            else
                return null;
        }

        public IList<Activity> Get(IList<string> names)
        {
            if (names?.Count > 0)
            {
                names = names.ToList();
                return this._dbContext.Activity.Where(x => names.Contains(x.Name)).ToList();
            }
            else
                return null;
        }
    }
}
