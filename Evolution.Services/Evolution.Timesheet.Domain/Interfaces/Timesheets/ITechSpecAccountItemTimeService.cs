using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    public interface ITechSpecAccountItemTimeService
    {
        Response Get(DomainModel.TimesheetSpecialistAccountItemTime accountItemTime);
 
        Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemTime accountItemTime);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? timesheetId = null);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? timesheetId = null);


        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                    ref IList<DbModel.Timesheet> dbTimesheet,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? timesheetId = null);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
    