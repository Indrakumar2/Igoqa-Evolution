using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit references.
    /// </summary>
    public interface IVisitReferenceService
    {
        Response Get(DomainModel.VisitReference visitReference);

        Response Add(IList<DomainModel.VisitReference> visitReferences,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     long? visitId = null);

        Response Add(IList<DomainModel.VisitReference> visitReferences,
                    ref IList<DbModel.VisitReference> dbVisitReferences,
                    ref IList<DbModel.Visit> dbVisit,
                    ref IList<DbModel.Data> dbReferenceType,
                    IList<DbModel.SqlauditModule> dbModule,
                    bool commitChange = true,
                    bool isDbValidationRequired = true,
                    long? visitId = null);

        Response Modify(IList<DomainModel.VisitReference> visitReferences,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Modify(IList<DomainModel.VisitReference> visitReferences,
                        ref IList<DbModel.VisitReference> dbVisitReferences,
                        ref IList<DbModel.Visit> dbVisit,
                        ref IList<DbModel.Data> dbReferenceType,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Delete(IList<DomainModel.VisitReference> visitReferences,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Delete(IList<DomainModel.VisitReference> visitReferences,
                        ref IList<DbModel.VisitReference> dbVisitReferences,
                        ref IList<DbModel.Visit> dbVisit,
                        ref IList<DbModel.Data> dbReferenceType,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true,
                        bool isDbValidationRequire = true,
                        long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                         ValidationType validationType,
                                         ref IList<DbModel.VisitReference> dbVisitReferences,
                                         ref IList<DbModel.Visit> dbAssignment,
                                         ref IList<DbModel.Data> dbReferenceType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                         ValidationType validationType,
                                         IList<DbModel.VisitReference> dbVisitReferences,
                                         ref IList<DbModel.Visit> dbVisit,
                                         ref IList<DbModel.Data> dbReferenceType);
    }
}
