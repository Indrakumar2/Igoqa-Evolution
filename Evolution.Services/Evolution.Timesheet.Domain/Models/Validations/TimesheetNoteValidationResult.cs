using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of Timesheet Note
    /// </summary>
    public class TimesheetNoteValidationResult : BaseMessage
    {
        public TimesheetNoteValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
