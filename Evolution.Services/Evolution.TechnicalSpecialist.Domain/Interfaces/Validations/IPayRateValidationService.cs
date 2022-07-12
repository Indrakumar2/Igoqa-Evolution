using Evolution.Common.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate TechncalspecialistPayrate Model which will come from client. 
    /// </summary>
    public interface IPayRateValidationService
    {
        /// <summary>
        /// Validate JSON model of TechncalspecialistPayrate  Reference
        /// </summary>
        /// <param name="jsonModel">TechncalspecialistPayrate  Reference Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<TechSpecialistValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
