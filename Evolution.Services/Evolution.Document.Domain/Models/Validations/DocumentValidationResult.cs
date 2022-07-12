using Evolution.Common.Models.Messages;

namespace Evolution.Document.Domain.Models.Validations
{
    /// <summary>
    /// Document Validation Result
    /// </summary>
    public class DocumentValidationResult : BaseMessage
    {
        public DocumentValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
