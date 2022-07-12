using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;


namespace Evolution.Visit.Domain.Interfaces.Visits
{
    public interface IVisitTechnicalSpecialistConsumableService
    {
        Response Get(DomainModel.VisitSpecialistAccountItemConsumable accountItemTime);

        Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemConsumable accountItemConsumable);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? visitId = null);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                            ref IList<DbModel.Visit> dbVisit,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null);

        Response Delete(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null);


        Response Delete(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                ref IList<DbModel.Visit> dbVisit,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                    ref IList<DbModel.Visit> dbVisit,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumable,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
