using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    public interface IVisitTechnicalSpecialistTravelService
    {
        Response Get(DomainModel.VisitSpecialistAccountItemTravel accountItemTravel);

        Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemTravel accountItemTravel);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? visitId = null);

        Response Add(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                            ref IList<DbModel.Visit> dbVisit,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null);

        Response Delete(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null);


        Response Delete(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                ref IList<DbModel.Visit> dbVisit,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null);

        Response Modify(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                    ref IList<DbModel.Visit> dbVisit,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType);
    }
}
