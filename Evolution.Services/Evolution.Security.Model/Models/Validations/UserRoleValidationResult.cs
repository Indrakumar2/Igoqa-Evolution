
using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Validations
{
    public class UserRoleValidationResult : BaseMessage
    {
        public UserRoleValidationResult(string code, string message) : base(code, message)
        {
        }
    }
}
