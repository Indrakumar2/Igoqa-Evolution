using Evolution.Common.Enums;
using Evolution.SupplierPO.Domain.Models.Valildation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.SupplierPO.Domain.Interfaces.Validations
{
    public interface ISupplierPoDocumentValidationService
    {
        IList<SupplierPOValildationResult> Validate(string jsonModel, ValidationType type);
    }
}
