using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of TechSpec Account Item Time
    /// </summary>
    public class TechSpecItemTimeValidationResult : BaseMessage
    {
        public TechSpecItemTimeValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
