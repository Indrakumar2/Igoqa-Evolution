using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;

namespace Evolution.AuditLog.Infrastructure.Data
{
    public class SqlAuditModuleRepository : GenericRepository<SqlauditModule>, ISqlAuditModuleRepository
    {
        public SqlAuditModuleRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
           
        }
    }
}