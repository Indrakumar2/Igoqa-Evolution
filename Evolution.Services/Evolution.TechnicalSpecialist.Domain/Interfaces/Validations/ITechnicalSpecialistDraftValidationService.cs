using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
    public interface ITechnicalSpecialistDraftValidationService
    {
        /// <summary>
        /// Validate JSON model of TechnicalSpecialist Classification Reference
        /// </summary>
        /// <param name="jsonModel">TechnicalSpecialist Classification Reference Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
