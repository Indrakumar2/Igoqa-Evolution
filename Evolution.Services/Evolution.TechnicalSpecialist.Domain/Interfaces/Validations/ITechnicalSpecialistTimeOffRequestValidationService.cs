using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
   public interface ITechnicalSpecialistTimeOffRequestValidationService
    {

        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
