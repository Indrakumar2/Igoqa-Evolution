using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Timesheets
{
    public interface ITimesheetDocumentService
    {
        Response GetAssignmentTimesheetDocuments(int? assignmentId);
    }
}
