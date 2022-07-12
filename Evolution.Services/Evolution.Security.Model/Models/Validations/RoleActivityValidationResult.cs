using Evolution.Common.Models.Messages;

namespace Evolution.Security.Domain.Models.Validations
{
    public class RoleActivityValidationResult : BaseMessage
    {
        public RoleActivityValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}