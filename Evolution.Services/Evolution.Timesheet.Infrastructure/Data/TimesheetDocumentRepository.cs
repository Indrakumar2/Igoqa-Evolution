using Evolution.GenericDbRepository.Services;
using Evolution.Timesheet.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Timesheet.Infrastructure.Data
{
    public class TimesheetDocumentRepository : GenericRepository<DbModel.TimesheetDocument>, ITimesheetDocumentRepository
    {
        public TimesheetDocumentRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
