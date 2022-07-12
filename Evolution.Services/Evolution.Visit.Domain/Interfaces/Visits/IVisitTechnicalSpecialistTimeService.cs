using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    public interface IVisitTechnicalSpecialistTimeService
    {
        Response Get(DomainModel.VisitSpecialistAccountItemTime accountItemTime);

        Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemTime accountItemTime);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? visitId = null);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                            ref IList<DbModel.Visit> dbVisit,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null);

        Response Delete(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null);


        Response Delete(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                ref IList<DbModel.Visit> dbVisit,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                    ref IList<DbModel.Visit> dbVisit,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
