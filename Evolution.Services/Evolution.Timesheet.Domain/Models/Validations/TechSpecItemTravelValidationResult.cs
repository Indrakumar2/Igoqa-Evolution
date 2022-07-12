using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of TechSpec Account Travel Consumable
    /// </summary>
    public class TechSpecItemTravelValidationResult : BaseMessage
    {
        public TechSpecItemTravelValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
