using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Enums;
using Evolution.GenericDbRepository.Interfaces;
using Evolution.Visit.Domain.Models.Visits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitRepository : IGenericRepository<DbModel.Visit>, IDisposable
    {
        IList<DomainModel.VisitSearchResults> Search(DomainModel.BaseVisit model);

        int GetCount(DomainModel.BaseVisit model);

        IQueryable<DbModel.Visit> GetVisitForDocumentApproval(DomainModel.BaseVisit searchModel);

        List<DomainModel.BaseVisit> GetVisit(DomainModel.BaseVisit searchModel);

        DomainModel.Visit GetVisitByID(DomainModel.BaseVisit searchModel);
        DomainModel.Visit GetVisitByID1(DbModel.Visit visitdata);

        List<DomainModel.Visit> GetHistoricalVisits(DomainModel.BaseVisit searchModel);

        DbModel.Visit GetDBVisitByID(DomainModel.BaseVisit searchModel);

        List<DbModel.SystemSetting> MailTemplateForVisitInterCompanyAmendment();

        List<DomainModel.Visit> GetSupplierList(DomainModel.BaseVisit searchModel);

        List<DomainModel.Visit> GetTechnicalSpecialistList(DomainModel.BaseVisit searchModel);

        int DeleteVisit(long visitId);

        long? GetFinalVisitId(DomainModel.BaseVisit searchModel);

        DomainModel.VisitValidationData GetVisitValidationData(DomainModel.BaseVisit searchModel);        

        List<DomainModel.Visit> GetVisitsByAssignment(DomainModel.BaseVisit searchModel);

        
        bool UpdateAssignmentFinalVisit(int assignmentId, string assignmentStatus);

        string GetTemplate(string companyCode, CompanyMessageType companyMessageType, string emailKey);

        //List<DbModel.SystemSetting> MailTemplate(string emailKey);

        //to be taken out after sync
        long? GetMaxEvoId();

        IList<DbModel.Assignment> GetDBVisitAssignments(IList<int> assignmentId, params string[] includes);

        IList<DbModel.Visit> FetchVisits(IList<long> lstVisitId, params string[] includes);

        List<DbModel.Visit> GetSupplierPoVisitIds(int? supplierPoId);

        List<DbModel.Visit> GetAssignmentVisitIds(int? assignmentId);

        IList<TechSpecIntertekWorkHistory> GetIntertekWorkHistoryReport(int epin);

        IList<TechSpecIntertekWorkHistory> GetStandardCVIntertekWorkHistoryReport(int epin);

        void AddVisitHistory(long visitId, int? historyItemId, string changedBy);

        Result SearchVisits(DomainModel.VisitSearch searchModel);
    }
}
