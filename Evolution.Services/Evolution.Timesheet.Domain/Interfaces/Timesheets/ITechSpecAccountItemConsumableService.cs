using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    public interface ITechSpecAccountItemConsumableService
    {
        Response Get(DomainModel.TimesheetSpecialistAccountItemConsumable accountItemTime);

        Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemConsumable accountItemConsumable);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? timesheetId = null);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? timesheetId = null);


        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                    ref IList<DbModel.Timesheet> dbTimesheet,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? timesheetId = null);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumable,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}

