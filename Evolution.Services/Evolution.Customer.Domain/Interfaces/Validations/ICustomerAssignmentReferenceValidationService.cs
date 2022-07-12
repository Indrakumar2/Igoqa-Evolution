using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Customer.Domain.Interfaces.Validations
{
    public interface ICustomerAssignmentReferenceValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
