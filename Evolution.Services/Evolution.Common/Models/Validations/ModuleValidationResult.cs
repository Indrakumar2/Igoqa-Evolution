using Evolution.Common.Models.Messages;

namespace Evolution.Common.Models.Validations
{
    public class JsonPayloadValidationResult : BaseMessage
    {
        public JsonPayloadValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}