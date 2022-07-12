using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of TechSpec Account Item Consumable
    /// </summary>
    public class TechSpecItemConsumableValidationResult : BaseMessage
    {
        public TechSpecItemConsumableValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
