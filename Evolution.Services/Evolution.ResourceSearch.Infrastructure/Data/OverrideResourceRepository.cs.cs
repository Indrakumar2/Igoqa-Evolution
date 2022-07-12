using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.GenericDbRepository.Services;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Infrastructure.Data
{
    public class OverrideResourceRepository : GenericRepository<DbModel.OverrideResource>, IOverrideResourceRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public OverrideResourceRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.OverridenPreferredResource> Get(IList<int> ResourceSearchIds)
        {
            if (ResourceSearchIds?.Count > 0)
            {
                return _dbContext.OverrideResource.Where(x => ResourceSearchIds.Contains(x.ResourceSearchId)).ProjectTo<DomainModel.OverridenPreferredResource>().ToList();
            }
            return null;
        }
    }
}
