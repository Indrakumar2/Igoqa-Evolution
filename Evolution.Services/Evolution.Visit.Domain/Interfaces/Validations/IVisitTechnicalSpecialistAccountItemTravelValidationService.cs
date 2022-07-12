using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Visit.Domain.Interfaces.Validations
{
    public interface IVisitTechnicalSpecialistAccountItemTravelValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
