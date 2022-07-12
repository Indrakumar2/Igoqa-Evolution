using Evolution.Common.Models.Messages;

namespace Evolution.Project.Domain.Models.Validations
{
    /// <summary>
    /// Contains the Implementation of project Invoice Reference validations
    /// </summary>
    public class InvoiceReferenceValildationResult :BaseMessage
    {
        public InvoiceReferenceValildationResult(string code, string message) : base(code, message)
        {
        }
    }
}
