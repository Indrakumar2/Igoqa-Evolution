using Evolution.Common.Enums;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;
using System.Threading.Tasks;
using Evolution.Common.Models.ExchangeRate;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    public interface ITimesheetService
    {
        Response GetTimesheets(DomainModel.BaseTimesheet searchModel, AdditionalFilter filter);

        Response GetTimesheetData(DomainModel.BaseTimesheet searchModel);
            
        Response GetTimesheetForDocumentApproval(DomainModel.BaseTimesheet searchModel);

        Task<Response> GetTimesheet(DomainModel.TimesheetSearch searchModel);

        Response Get(DomainModel.Timesheet searchModel);

        Response Add(IList<DomainModel.Timesheet> timesheets,
                      ref long? eventId,
                     bool commitChange = true,
                      bool isValidationRequire = true,
                      bool isProcessNumberSequence = false);

        Response Add(IList<DomainModel.Timesheet> timesheets,
                           ref IList<DbModel.Timesheet> dbTimesheet,
                           ref IList<DbModel.Assignment> dbAssignment,
                           IList<DbModel.SqlauditModule> dbModule,
                           ref long? eventId,
                           bool commitChange = true,
                           bool isValidationRequire = true,
                           bool isProcessNumberSequence = false);

        Response Modify(IList<DomainModel.Timesheet> timesheets,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true);

        Response Modify(IList<DomainModel.Timesheet> timesheets,
                               ref IList<DbModel.Timesheet> dbTimesheet,
                               ref IList<DbModel.Assignment> dbAssignment,
                               IList<DbModel.SqlauditModule> dbModule,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isValidationRequire = true);

        Response Delete(IList<DomainModel.Timesheet> timesheets,
                         ref long? eventId,
                         bool commitChange = true,
                         bool isValidationRequire = true);

        Response Delete(IList<DomainModel.Timesheet> timesheets,
                         IList<DbModel.SqlauditModule> dbModule,
                         ref long? eventId,
                         bool commitChange = true,
                         bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                 ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                ValidationType validationType,
                                                ref IList<DbModel.Timesheet> dbTimesheets,
                                                ref IList<DbModel.Assignment> dbAssignments);

        Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                 ValidationType validationType,
                                                 IList<DbModel.Timesheet> dbTimesheets,
                                                 IList<DbModel.Assignment> dbAssignments);

        bool IsValidTimesheet(IList<long> timesheetId,
                                    ref IList<DbModel.Timesheet> dbTimesheets,
                                    ref IList<ValidationMessage> messages,
                                    params string[] includes);

        bool IsValidTimesheetData(IList<long> timesheetId,
                                  ref IList<DbModel.Timesheet> dbTimesheets,
                                  ref IList<ValidationMessage> messages);

        Response GetTimesheetValidationData(DomainModel.BaseTimesheet searchModel);

        Response AddSkeletonTimesheet(DbModel.Timesheet dbTimesheet, ref DbModel.Timesheet dbSavedTimesheet, bool commitChange);

        void AddNumberSequence(DbModel.NumberSequence data, int parentId, int parentRefId, int assignmentId, ref List<DbModel.NumberSequence> numberSequence);

        Response GetExpenseLineItemChargeExchangeRates(IList<ExchangeRate> exchangeRates,string ContractId);

        int ProcessNumberSequence(int assignmentId, ref DbModel.NumberSequence timesheetNumberSequence);
        
        Response SaveNumberSequence(DbModel.NumberSequence timesheetNumberSequence);

        void AddTimesheetHistory(long timesheetId, string historyItemCode, string changedBy);
    }
}