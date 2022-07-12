using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;


namespace Evolution.Master.Domain.Interfaces.Validations
{
    public interface ILanguageRefTypeValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}