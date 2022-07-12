using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Interfaces.Validations
{
    public interface IAssignmentContractRateScheduleValidationService
    {
        /// <summary>
        /// Validate JSON model of Assignment Contract RateSchedule
        /// </summary>
        /// <param name="jsonModel">Assignment Contract RateSchedule Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }

}
