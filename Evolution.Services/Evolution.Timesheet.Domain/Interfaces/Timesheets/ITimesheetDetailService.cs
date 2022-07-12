using Evolution.Timesheet.Domain.Models.Timesheets;
using Evolution.Common.Models.Responses;
using System.Threading.Tasks;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    public interface ITimesheetDetailService
    {
        Response Get(TimesheetDetail searchModel);

        Task<Response> GetTechnicalSpecialistWithGrossMargin(TimesheetTechnicalSpecialist technicalSpecialist);

        Response Add(TimesheetDetail timesheetDetails, bool IsAPIValidationRequired=false);

        Response Modify(TimesheetDetail timesheetDetails, bool IsAPIValidationRequired=false);

        Response Delete(TimesheetDetail timesheetDetails, bool IsAPIValidationRequired=false);
        Response ApproveTimesheet(TimesheetEmailData timesheetEmailData);
        Response RejectTimesheet(TimesheetEmailData timesheetEmailData);
        Response ApprovalCustomerReportNotification(CustomerReportingNotification clientReportingNotification);
}

}