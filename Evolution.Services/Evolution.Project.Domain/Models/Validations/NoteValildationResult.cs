using Evolution.Common.Models.Messages;

namespace Evolution.Project.Domain.Models.Validations
{

    /// <summary>
    /// Contains the Implementation of project Note validations
    /// </summary>
    public class NoteValildationResult :BaseMessage
    {
        public NoteValildationResult(string code, string message) : base(code, message)
        {
        }
    }
}
