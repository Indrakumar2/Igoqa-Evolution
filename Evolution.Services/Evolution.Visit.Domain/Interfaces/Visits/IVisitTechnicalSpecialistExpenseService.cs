using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    public interface IVisitTechnicalSpecialistExpenseService
    {
        Response Get(DomainModel.VisitSpecialistAccountItemExpense accountItemTime);

        Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemExpense accountItemExpense);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? visitId = null);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                            ref IList<DbModel.Visit> dbVisit,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null);

        Response Delete(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null);


        Response Delete(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                ref IList<DbModel.Visit> dbVisit,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                    ref IList<DbModel.Visit> dbVisit,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
