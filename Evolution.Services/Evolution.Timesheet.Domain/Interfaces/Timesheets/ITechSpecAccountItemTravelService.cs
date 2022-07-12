using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    public interface ITechSpecAccountItemTravelService
    {
        Response Get(DomainModel.TimesheetSpecialistAccountItemTravel accountItemTime);

        Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemTravel accountItemTime);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? timesheetId = null);

        Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null);

        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? timesheetId = null);


        Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? timesheetId = null);

        Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                    ref IList<DbModel.Timesheet> dbTimesheet,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? timesheetId = null);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
