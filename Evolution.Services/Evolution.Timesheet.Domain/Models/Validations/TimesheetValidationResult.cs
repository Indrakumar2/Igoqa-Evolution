using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{
    /// <summary>
    /// Contains the Implementation of Timesheet
    /// </summary>
    public class TimesheetValidationResult : BaseMessage
    {
        public TimesheetValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
