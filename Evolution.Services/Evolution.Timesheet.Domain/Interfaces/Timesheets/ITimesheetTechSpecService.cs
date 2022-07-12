using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Timesheet.Domain.Models.Timesheets;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{

    public interface ITimesheetTechSpecService
    {

        Response Get(TimesheetTechnicalSpecialist technicalSpecialist);

        Response Add(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     long? timesheetId = null);

        Response Add(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                     ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                     ref IList<DbModel.Timesheet> dbTimesheet,
                     IList<DbModel.SqlauditModule> dbModule,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                        ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialist,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                        ref IList<DbModel.Timesheet> dbTimesheet,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? timesheetId = null);


        Response Delete(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                    bool commitChange = true,
                    bool isDbValidationRequired = true,
                    long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                         ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                         ref IList<DbModel.Timesheet> dbTimesheet,
                         IList<DbModel.SqlauditModule> dbModule,
                         bool commitChange = true,
                         bool isDbValidationRequired = true,
                         long? timesheetId = null);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timeTechnicalSpecialists,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timeTechnicalSpecialists,
                                         ValidationType validationType,
                                         ref IList<DbModel.Timesheet> dbTimesheets,
                                         ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timeTechnicalSpecialists,
                                         ValidationType validationType,
                                         IList<DbModel.Timesheet> dbTimesheets,
                                         IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                         IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        bool IsTimesheetTechnicalSpecialistExistInDb(IList<long> timesheetTechnicalSpecialistIds,
                                                     IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                     ref IList<long> timesheetTechnicalSpecialistNotExists,
                                                     ref IList<ValidationMessage> validationMessages);
    }
}
