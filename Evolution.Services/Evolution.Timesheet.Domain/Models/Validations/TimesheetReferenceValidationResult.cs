using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of Timesheet Reference
    /// </summary>
    public class TimesheetReferenceValidationResult : BaseMessage
    {
        public TimesheetReferenceValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
