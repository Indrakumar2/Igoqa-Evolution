using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Models.Security;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Security.Infrastructure.Data
{
    public class ApplicationMenuRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.ApplicationMenu>, IApplicationMenuRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public ApplicationMenuRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<ApplicationMenuInfo> Search(string applicationName)
        {
            var menus = this._dbContext.ApplicationMenu
                                       .Where(x => x.Application.Name == applicationName)
                                       .Include(x=>x.Application)
                                       .Include(x=>x.Module);
            return _mapper.Map<IList<ApplicationMenuInfo>>(menus);
        }
    }
}
