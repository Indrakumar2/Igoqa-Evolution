using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Security.Infrastructure.Data
{
    public class ApplicationRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Application>, IApplicationRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public ApplicationRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<Application> Get(IList<int> ids)
        {
            if (ids?.Count > 0)
            {
                ids = ids.ToList();
                return this._dbContext.Application.Where(x => ids.Contains(x.Id)).ToList();
            }
            else
                return null;
        }

        public IList<Application> Get(IList<string> names)
        {
            if (names?.Count > 0)
            {
                names = names.ToList();
                return this._dbContext.Application.Where(x => names.Contains(x.Name)).ToList();
            }
            else
                return null;
        }
    }
}
