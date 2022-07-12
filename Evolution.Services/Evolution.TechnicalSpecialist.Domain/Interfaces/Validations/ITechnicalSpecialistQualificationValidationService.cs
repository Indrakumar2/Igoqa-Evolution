using Evolution.Common.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using Evolution.Common.Models.Validations;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
  
    public interface ITechnicalSpecialistQualificationValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
