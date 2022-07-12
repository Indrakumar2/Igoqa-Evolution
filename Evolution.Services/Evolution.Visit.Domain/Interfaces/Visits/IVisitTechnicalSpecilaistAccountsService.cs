using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Visit.Domain.Models.Visits;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit Technical Specialist Accounts.
    /// </summary>
    public interface IVisitTechnicalSpecilaistAccountsService
    {
        Response Get(VisitTechnicalSpecialist technicalSpecialist);

        Response Add(IList<DomainModel.VisitTechnicalSpecialist> VisitTechnicalSpecialists,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     long? visitId = null);

        Response Add(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                     ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                     ref IList<DbModel.Visit> dbVisit,
                     IList<DbModel.SqlauditModule> dbModule,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     long? visitId = null);

        Response Modify(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Modify(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                        ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialist,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                        ref IList<DbModel.Visit> dbVisit,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);


        Response Delete(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                    bool commitChange = true,
                    bool isDbValidationRequired = true,
                    long? visitId = null);

        Response Delete(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                         ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                         ref IList<DbModel.Visit> dbVisit,
                         IList<DbModel.SqlauditModule> dbModule,
                         bool commitChange = true,
                         bool isDbValidationRequired = true,
                         long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> timeTechnicalSpecialists,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> timeTechnicalSpecialists,
                                         ValidationType validationType,
                                         ref IList<DbModel.Visit> dbVisits,
                                         ref IList<DbModel.VisitTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> timeTechnicalSpecialists,
                                         ValidationType validationType,
                                         IList<DbModel.Visit> dbVisits,
                                         IList<DbModel.VisitTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                         IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        bool IsVisitTechnicalSpecialistExistInDb(IList<long> visitTechnicalSpecialistIds,
                                                     IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                     ref IList<long> visitTechnicalSpecialistNotExists,
                                                     ref IList<ValidationMessage> validationMessages);

        /// <summary>
        /// Return all the match search Visit Technical Specialist Accounts.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetVisitTechnicalSpecialistAccount(VisitTechnicalSpecialist searchModel);
    }
}
