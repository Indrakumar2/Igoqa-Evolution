using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.Validations
{
    /// <summary>
    /// Customer Validation Result
    /// </summary>
    public class TechSpecialistValidationResult : BaseMessage
    {
        public TechSpecialistValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
