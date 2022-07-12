using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.Validations
{
    public class TechnicalSpecialistCodeValidationResults : BaseMessage
    {
        public TechnicalSpecialistCodeValidationResults(string code, string message) : base(code, message)
        {
        }
    }
}
