using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Project.Domain.Models.Validations
{
    public class ProjectClientNotificationValidationResult: BaseMessage
    {
        public ProjectClientNotificationValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
