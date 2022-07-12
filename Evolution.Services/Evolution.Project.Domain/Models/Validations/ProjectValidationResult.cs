using Evolution.Common.Models.Messages;

namespace Evolution.Project.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of project validations
    /// </summary>
    public class ProjectValidationResult : BaseMessage
    {
        public ProjectValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
