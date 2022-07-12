using Evolution.Common.Models.Messages;

namespace Evolution.Project.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of project Document validations
    /// </summary>
    public class DocumentValidationResut : BaseMessage
    {
        public DocumentValidationResut(string code, string message) : base(code, message)
        {
        }
    }
}
