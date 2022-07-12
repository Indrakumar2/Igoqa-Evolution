using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit Supplier perfromance.
    /// </summary>
    public interface IVisitSupplierPerformanceService
    {
        Response Get(DomainModel.VisitSupplierPerformanceType visitSupplierPerformanceType);

        Response Add(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     long? visitId = null);

        Response Add(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                    ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformanceTypes,
                    ref IList<DbModel.Visit> dbVisit,
                    IList<DbModel.SqlauditModule> dbModule,
                    bool commitChange = true,
                    bool isDbValidationRequired = true,
                    long? visitId = null);

        Response Modify(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Modify(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                        ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformanceTypes,
                        ref IList<DbModel.Visit> dbVisit,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Delete(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Delete(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                        ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformanceTypes,
                        ref IList<DbModel.Visit> dbVisit,
                        IList<DbModel.SqlauditModule> dbModule,
                        bool commitChange = true,
                        bool isDbValidationRequire = true,
                        long? visitId = null);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                                         ValidationType validationType,
                                         ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformanceTypes,
                                         ref IList<DbModel.Visit> dbAssignment);

        Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformanceTypes,
                                         ValidationType validationType,
                                         IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformanceTypes,
                                         ref IList<DbModel.Visit> dbVisit);
    }
}
