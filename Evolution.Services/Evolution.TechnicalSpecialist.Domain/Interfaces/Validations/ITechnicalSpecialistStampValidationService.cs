using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
    public interface ITechnicalSpecialistStampValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}

