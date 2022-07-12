using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using System.Collections.Generic;
using System;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITimesheetNoteRepository : IGenericRepository<TimesheetNote>, IDisposable
    {
        IList<DomainModel.TimesheetNote> Search(DomainModel.TimesheetNote model);
        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
