using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Visit.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate Visit Model which will come from client. 
    /// </summary>
    public interface IVisitValidationService
    {
        /// <summary>
        /// Validate JSON model of Customer
        /// </summary>
        /// <param name="jsonModel">Customer Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
