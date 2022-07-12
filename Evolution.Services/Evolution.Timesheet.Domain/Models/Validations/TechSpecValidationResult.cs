using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of TechSpec in Timesheet
    /// </summary>
    public class TechSpecValidationResult : BaseMessage
    {
        public TechSpecValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
