using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    /// <summary>
    /// Contains the predefined functionalities relating to Timesheet Reference Service
    /// </summary>
    public interface ITimesheetReferenceService
    {

        Response Get(DomainModel.TimesheetReferenceType timesheetReference);

        Response Add(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                     bool commitChange = true, 
                     bool isDbValidationRequired = true,
                     long? timesheetId = null);

        Response Add(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                    ref IList<DbModel.TimesheetReference> dbTimesheetReferences, 
                    ref IList<DbModel.Timesheet> dbTimesheet, 
                    ref IList<DbModel.Data> dbReferenceType,
                    IList<DbModel.SqlauditModule> dbModule,
                    bool commitChange = true, 
                    bool isDbValidationRequired = true,
                    long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                        bool commitChange = true, 
                        bool isDbValidationRequired = true,
                        long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                        ref IList<DbModel.TimesheetReference> dbTimesheetReferences, 
                        ref IList<DbModel.Timesheet> dbTimesheet, 
                        ref IList<DbModel.Data> dbReferenceType,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true, 
                        bool isDbValidationRequired = true,
                        long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                        bool commitChange = true, 
                        bool isDbValidationRequired = true,
                        long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                        ref IList<DbModel.TimesheetReference> dbTimesheetReferences, 
                        ref IList<DbModel.Timesheet> dbTimesheet, 
                        ref IList<DbModel.Data> dbReferenceType,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true, 
                        bool isDbValidationRequire = true,
                        long? timesheetId = null);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences, 
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                         ValidationType validationType,
                                         ref IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                         ref IList<DbModel.Timesheet> dbAssignment,
                                         ref IList<DbModel.Data> dbReferenceType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                         ValidationType validationType,
                                         IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                         ref IList<DbModel.Timesheet> dbTimesheet,
                                         ref IList<DbModel.Data> dbReferenceType);

    }
}
