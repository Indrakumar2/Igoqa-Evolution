using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.Validations;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Validations
{
   
    public interface ITechnicalSpecialistCustomerApprovalValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
