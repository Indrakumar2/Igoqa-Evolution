using Evolution.Common.Models.Messages;

namespace Evolution.Project.Domain.Models.Validations
{
    public class FinanceManageItemValidationResult : BaseMessage
    {
        public FinanceManageItemValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
