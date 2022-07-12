using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Validations
{
   public class RoleValidationResult: BaseMessage
    {
        public RoleValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
