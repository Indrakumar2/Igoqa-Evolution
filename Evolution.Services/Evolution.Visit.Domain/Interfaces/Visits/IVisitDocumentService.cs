using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit Document.
    /// </summary>
    public interface IVisitDocumentService
    {
        /// <summary>
        /// Save Visit Documents
        /// </summary>
        /// <param name="visitDocuments">List of Visit Documents</param>
        /// <returns>All the Saved Company Note Details</returns>
        Response SaveVisitDocument(long visitId, IList<DomainModel.VisitDocuments> visitDocuments, bool commitChanges);

        /// <summary>
        /// Modify list of Company Documents
        /// </summary>
        /// <param name="visitDocuments">List of Company Documents which need to update.</param>
        Response ModifyVisitDocument(long visitId, IList<DomainModel.VisitDocuments> visitDocuments, bool commitChanges);

        Response DeleteVisitDocument(long visitId, IList<DomainModel.VisitDocuments> visitDocuments, bool commitChanges);

        /// <summary>
        /// Return all the match search Visit Documents
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetVisitDocument(Visit.Domain.Models.Visits.VisitDocuments searchModel);

        Response GetSupplierPoVisitIds(int? supplierPOId);

        Response GetAssignmentVisitDocuments(int? assignmentId);
    }
}
