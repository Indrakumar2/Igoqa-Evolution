using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Project.Domain.Interfaces.Validations
{
    public interface IProjectValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    } 
}
