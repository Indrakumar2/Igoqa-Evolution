using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;


namespace Evolution.Draft.Domain.Interfaces.Validations
{
    public interface IDraftValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
