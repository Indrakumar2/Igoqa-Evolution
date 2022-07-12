using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{

    /// <summary>
    /// Contains the predefined functions for Timesheet Document repository
    /// </summary>
    public interface ITimesheetDocumentRepository : IGenericRepository<TimesheetDocument>
    {
       
    }
}
