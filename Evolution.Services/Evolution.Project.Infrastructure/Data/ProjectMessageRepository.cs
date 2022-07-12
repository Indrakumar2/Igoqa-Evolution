using Evolution.GenericDbRepository.Services;
using Evolution.Project.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Infrastructure.Data
{
    public class ProjectMessageRepository : GenericRepository<DbModel.ProjectMessage>, IProjectMessageRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public ProjectMessageRepository(DbModel.EvolutionSqlDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
