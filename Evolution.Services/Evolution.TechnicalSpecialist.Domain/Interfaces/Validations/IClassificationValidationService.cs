using Evolution.Common.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate TechnicalSpecialist Classification Reference Model which will come from client. 
    /// </summary>
    public interface IClassificationValidationService
    {
        /// <summary>
        /// Validate JSON model of TechnicalSpecialist Classification Reference
        /// </summary>
        /// <param name="jsonModel">TechnicalSpecialist Classification Reference Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<TechSpecialistValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
