using Evolution.Common.Enums;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;
using System.Threading.Tasks;
using Evolution.Company.Domain.Enums;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit.
    /// </summary>
    public interface IVisitService
    {
        /// <summary>
        /// Save Visit
        /// </summary>
        /// <param name="customers">List of Visits</param>
        /// <returns>All the Saved Visit Details</returns>
        Response SaveVisit(Models.Visits.Visit visits, bool commitChange = true);

        /// <summary>
        /// Modify list of Visits
        /// </summary>
        /// <param name="customers">List of Visit which need to update.</param>
        Response ModifyVisit(Models.Visits.Visit visits, bool commitChange = true);

        Response DeleteVisit(Models.Visits.Visit visits, bool commitChange = true);

        /// <summary>
        /// Return all the match search Visit.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetVisit(Models.Visits.BaseVisit searchModel, AdditionalFilter filter);

        Response GetVisitData(Models.Visits.BaseVisit searchModel);

        Task<Response> GetSearchVisits(DomainModel.VisitSearch searchModel);

        Response GetVisitByID(Models.Visits.BaseVisit searchModel);

        Response GetVisitForDocumentApproval(DomainModel.BaseVisit searchModel);

        Response GetHistoricalVisits(Models.Visits.BaseVisit searchModel);

        Response AddSkeletonVisit(DbModel.Visit dbVisit,ref DbModel.Visit dbSavedVisit,bool commitChange);

        Response GetSupplierList(Models.Visits.BaseVisit searchModel);

        Response GetTechnicalSpecialistList(Models.Visits.BaseVisit searchModel);

        Response Add(IList<DomainModel.Visit> visits,
                       ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true,
                            bool isProcessNumberSequence = false);

        Response Add(IList<DomainModel.Visit> visits,
                           ref IList<DbModel.Visit> dbVisit,
                           ref IList<DbModel.Assignment> dbAssignment,
                           IList<DbModel.SqlauditModule> dbModule,
                           ref long? eventId,
                           bool commitChange = true,
                           bool isValidationRequire = true,
                           bool isProcessNumberSequence = false);

        Response Modify(IList<DomainModel.Visit> visits,
                         ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true);
        
        int ProcessNumberSequence(int assignmentId, ref DbModel.NumberSequence visitNumberSequence);
        
        Response SaveNumberSequence(DbModel.NumberSequence visitNumberSequence);

        Response Modify(IList<DomainModel.Visit> visits,
                        ref IList<DbModel.Visit> dbVisit,
                        ref IList<DbModel.Assignment> dbAssignment,
                        IList<DbModel.SqlauditModule> dbModule,
                        ref long? eventId,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<DomainModel.Visit> visits, 
                        ref long? eventId,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<DomainModel.Visit> visits,
                        IList<DbModel.SqlauditModule> dbModule,
                         ref long? eventId,
                         bool commitChange = true,
                         bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                 ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                ValidationType validationType,
                                                ref IList<DbModel.Visit> dbVisits,
                                                ref IList<DbModel.Assignment> dbAssignments);

        Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                 ValidationType validationType,
                                                 IList<DbModel.Visit> dbVisits,
                                                 IList<DbModel.Assignment> dbAssignments);

        bool IsValidVisit(IList<long> visitId,
                                    ref IList<DbModel.Visit> dbVisits,
                                    ref IList<ValidationMessage> messages,
                                    params string[] includes);

        bool IsValidVisitData(IList<long> visitId,
                                   ref IList<DbModel.Visit> dbVisits,
                                   ref IList<ValidationMessage> messages);

        Response GetFinalVisitId(DomainModel.BaseVisit searchModel);

        Response GetVisitValidationData(DomainModel.BaseVisit searchModel);
        
        Response GetVistListByIds(IList<long> visitIds);

        Response GetTemplate(string companyCode, CompanyMessageType companyMessageType, EmailKey emailKey);

        void AddNumberSequence(DbModel.NumberSequence data, int parentId, int parentRefId, int assignmentId, ref List<DbModel.NumberSequence> numberSequence);

        Response GetIntertekWorkHistoryReport(int epin);

        void AddVisitHistory(long visitId, string historyItemCode, string changedBy);
    }
}
