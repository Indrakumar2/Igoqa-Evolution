using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Supplier.Domain.Interfaces.Validations
{
    public interface ISupplierNoteValidationService
    {
       IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
