using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{
    /// <summary>
    /// Contains the Implementation of  Timesheet document
    /// </summary>
    public class TimesheetDocumentValidationResult : BaseMessage
    {
        public TimesheetDocumentValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
