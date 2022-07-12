using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Assignment.Domain.Interfaces.Validations
{
   public interface IAssignmentReferenceTypeValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
