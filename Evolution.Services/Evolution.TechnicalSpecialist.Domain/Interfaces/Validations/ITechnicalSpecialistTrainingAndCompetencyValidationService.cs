using Evolution.Common.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{ 
  public interface ITechnicalSpecialistTrainingAndCompetencyValidationService
    {
        /// <summary>
        /// Validate JSON model of Certification And Training Business Info
        /// </summary>
        /// <param name="jsonModel">TechnicalSpecialist Certification And Training Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<TechSpecialistValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
