using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.Security.Domain.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Validations
{
    public interface IUserValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
