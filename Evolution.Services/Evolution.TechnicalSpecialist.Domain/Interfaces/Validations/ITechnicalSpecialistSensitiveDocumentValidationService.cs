using Evolution.Common.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
    public interface ITechnicalSpecialistSensitiveDocumentValidationService
    {
        /// <summary>
        /// Validate JSON model of TechnicalSpecialist Business Info
        /// </summary>
        /// <param name="jsonModel">TechnicalSpecialist BusinessInfo Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<TechSpecialistValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
