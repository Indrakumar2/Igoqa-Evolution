using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Visit.Domain.Interfaces.Validations
{
    public interface IVisitDocumentValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
