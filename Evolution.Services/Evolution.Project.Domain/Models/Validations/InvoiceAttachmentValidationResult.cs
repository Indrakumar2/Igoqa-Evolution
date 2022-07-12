using Evolution.Common.Models.Messages;

namespace Evolution.Project.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of project Invoice Attachment validations
    /// </summary>
    public class InvoiceAttachmentValidationResult: BaseMessage
    {
        public InvoiceAttachmentValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}

