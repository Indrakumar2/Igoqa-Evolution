using Evolution.Common.Models.Responses;
using Evolution.Timesheet.Domain.Models.Timesheets;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    /// <summary>
    /// Contains the predefined Function that relating to Timesheet Note
    /// </summary>
    public interface ITimesheetNoteService
    {
        Response Get(TimesheetNote searchModel);

        Response Add(IList<TimesheetNote> timesheetNotes, 
                     bool commitChange = true, 
                     bool isValidationRequire = true, 
                     long? timesheetId = null);

        Response Add(IList<TimesheetNote> timesheetNotes, 
                     ref IList<DbModel.TimesheetNote> dbTimesheetNotes, 
                     ref IList<DbModel.Timesheet> dbTimesheets,
                     IList<DbModel.SqlauditModule> dbModule,
                     bool commitChange = true, 
                     bool isValidationRequire = true,
                     long? timesheetId = null);
       //D661 issue 8 Start
        Response Update(IList<TimesheetNote> timesheetNotes,
                     ref IList<DbModel.TimesheetNote> dbTimesheetNotes,
                     ref IList<DbModel.Timesheet> dbTimesheets,
                     IList<DbModel.SqlauditModule> dbModule,
                     bool commitChange = true,
                     bool isValidationRequire = true,
                     long? timesheetId = null);
       //D661 issue 8 End
        Response Delete(IList<TimesheetNote> timesheetNotes, 
                        bool commitChange = true, 
                        bool isDbValidationRequired = true,
                        long? timesheetId = null);

        Response Delete(IList<TimesheetNote> timesheetNotes, 
                        ref IList<DbModel.TimesheetNote> dbTimesheetNotes, 
                        ref IList<DbModel.Timesheet> dbTimesheets, 
                        bool commitChange = true, 
                        bool isDbValidationRequired = true,
                        long? assignmentIds = null);

        Response IsRecordValidForProcess(IList<TimesheetNote> timesheetNotes, 
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<TimesheetNote> timesheetNotes, 
                                         ValidationType validationType, 
                                         ref IList<DbModel.TimesheetNote> dbTimesheetNotes, 
                                         ref IList<DbModel.Timesheet> dbTimesheets);

        Response IsRecordValidForProcess(IList<TimesheetNote> timesheetNotes, 
                                         ValidationType validationType, 
                                         IList<DbModel.TimesheetNote> dbTimesheetNotes, 
                                         ref IList<DbModel.Timesheet> dbTimesheets);

    }
}
