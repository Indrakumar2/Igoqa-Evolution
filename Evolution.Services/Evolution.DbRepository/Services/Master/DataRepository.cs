using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Linq;
using Evolution.Common.Extensions;

namespace Evolution.DbRepository.Services.Master
{
    public class DataRepository : GenericRepository<Data>, IDataRepository
    {
        EvolutionSqlDbContext _dbContext = null;

        public DataRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
