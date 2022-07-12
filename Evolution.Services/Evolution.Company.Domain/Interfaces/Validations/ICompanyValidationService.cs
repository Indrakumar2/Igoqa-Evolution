using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Company.Domain.Interfaces.Validations
{
    public interface ICompanyValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
