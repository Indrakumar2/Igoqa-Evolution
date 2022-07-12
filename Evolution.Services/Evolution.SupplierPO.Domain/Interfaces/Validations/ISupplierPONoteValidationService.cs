using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.SupplierPO.Domain.Models.Valildation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.SupplierPO.Domain.Interfaces.Validations
{
    public interface ISupplierPONoteValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
