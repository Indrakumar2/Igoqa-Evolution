using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Interfaces.Validations
{
    public interface IAssignmentContributionCalculationValidationService
    {
        /// <summary>
        /// Validate JSON model of Assignment Contribution Calculation
        /// </summary>
        /// <param name="jsonModel">Assignment Contribution Calculation Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
