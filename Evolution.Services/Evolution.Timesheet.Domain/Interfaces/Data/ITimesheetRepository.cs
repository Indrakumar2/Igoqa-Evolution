using Evolution.Company.Domain.Enums;
using Evolution.GenericDbRepository.Interfaces;
using Evolution.Timesheet.Domain.Models.Timesheets;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;


namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITimesheetRepository : IGenericRepository<DbModel.Timesheet>, IDisposable
    {
        IList<DomainModel.BaseTimesheet> Search(DomainModel.BaseTimesheet searchModel);

        int GetCount(DomainModel.BaseTimesheet searchModel);

        List<DomainModel.BaseTimesheet> GetTimesheet(DomainModel.BaseTimesheet searchModel);

        IQueryable<DbModel.Timesheet> GetTimesheetForDocumentApproval(DomainModel.BaseTimesheet searchModel);

        IList<DomainModel.TimesheetSearch> GetSearchTimesheet(DomainModel.TimesheetSearch searchModel, params string[] includes);

        List<DomainModel.Timesheet> Get(DomainModel.Timesheet searchModel, params string[] includes);

        int DeleteTimesheet(long timesheetId);

        DomainModel.TimesheetValidationData GetTimesheetValidationData(DomainModel.BaseTimesheet searchModel);

        string GetTemplate(string companyCode, CompanyMessageType companyMessageType, string emailKey);

        //List<DbModel.SystemSetting> MailTemplate(string emailKey);

        //to be taken out after sync
        long? GetMaxEvoId();

        IList<DbModel.Assignment> GetDBTimesheetAssignments(IList<int> assignmentId, params string[] includes);

        IList<DbModel.Timesheet> FetchTimesheets(IList<long> lstTimesheetId, params string[] includes);

        List<DbModel.Timesheet> GetAssignmentTimesheetIds(int? assignmentId);

        void AddTimesheetHistory(long timesheetId, int? historyItemId, string changedBy);

        IList<DbModel.TimesheetInterCompanyDiscount> GetAssignmentInterCompanyDiscounts(int assignmentId, long timesheetId);

        Result SearchTimesheets(DomainModel.TimesheetSearch searchModel);
    }
}
