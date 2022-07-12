using Evolution.Common.Models.Messages;

namespace Evolution.Timesheet.Domain.Models.Validations
{
    /// <summary>
    /// Contains the Implementation of TechSpec Account Item Expense
    /// </summary>
    public class TechSpecItemExpenseValidationResult : BaseMessage
    {
        public TechSpecItemExpenseValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
