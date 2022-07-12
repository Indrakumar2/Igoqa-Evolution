using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentRepository : IGenericRepository<DbModel.Assignment>, IDisposable
    {
        IList<DomainModel.AssignmentDashboard> Search(DomainModel.AssignmentSearch model);

        IQueryable<DbModel.Assignment> GetAssignment(DomainModel.AssignmentSearch searchModel);

        Int32 GetCount(DomainModel.AssignmentSearch searchModel);

        DbModel.Assignment SearchAssignment(int assignmentId, params string[] includes);

        IList<DomainModel.Assignment> AssignmentSearch(int flag,DomainModel.AssignmentSearch searchModel, params string[] includes);

        //IList<DomainModel.AssignmentSearch> AssignmentSearch(DomainModel.AssignmentSearch searchModel,params string[] includes);

        int DeleteAssignment(int assignmentId);
        List <DbModel.Assignment> GetAssignmentData(int supplierPOId);

        List<DbModel.SystemSetting> MailTemplate();
        List<DbModel.SystemSetting> MailTemplateForInterCompanyAmendment();

        string GetCompanyMessage(string companyCode);

        IList<DbModel.Data> GetMasterData(IList<string> names, IList<string> description, IList<int> types, IList<string> codes);

        IList<DbModel.User> GetUser(IList<string> names);

        //IList<DomainModel.ProjectCollection> ProjectCollection(IList<int> projectNumber);
        void Update(int assignmentId, IList<KeyValuePair<string,object>> updateValueProps, Expression<Func<DbModel.Assignment, object>>[] updatedProperties);

        void AddAssignmentHistory(int assignmentId, IList<DbModel.Data> dbMasterData, string changedBy);

        bool GetTsVisible();

        IList<DbModel.SqlauditModule> GetAuditModule();

        string ValidateAssignmentBudget(DomainModel.Assignment assignments, DbModel.Project dbProjects); //Added for D-1304

        IList<DbModel.Country> GetCountry(IList<string> names, IList<string> cities);// Sanity Defect- 173

        IList<DomainModel.AssignmentEditSearch> AssignmentSearch(DomainModel.AssignmentEditSearch searchModel);
    }
}
