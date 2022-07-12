using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{

    public interface ITechSpecAccountItemExpenseService
    {
        Response Get(DomainModel.TimesheetSpecialistAccountItemExpense accountItemTime);

        Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemExpense accountItemTime);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? timesheetId = null);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? timesheetId = null);


        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                    ref IList<DbModel.Timesheet> dbTimesheet,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? timesheetId = null);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
