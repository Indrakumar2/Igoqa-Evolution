using Evolution.Common.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate TechnicalSpecialist IntertekWork History Report Model which will come from client. 
    /// </summary>
    public interface IIntertekWorkHistoryReportValidationService
    {
        /// <summary>
        /// Validate JSON model of TechnicalSpecialist Intertek Work History Report
        /// </summary>
        /// <param name="jsonModel">TechnicalSpecialist Intertek Work History Report Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<TechSpecialistValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
