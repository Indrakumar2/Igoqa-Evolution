using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.DbRepository.Services.Master
{
    public class TimeOffRequestCategoryRepository : GenericRepository<TimeOffRequestCategory>,ITimeOffRequestCategoryRepository
    {

        public TimeOffRequestCategoryRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
