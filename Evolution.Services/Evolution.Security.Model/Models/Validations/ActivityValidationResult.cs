﻿using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Validations
{
 public class ActivityValidationResult: BaseMessage
    {
        public ActivityValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
