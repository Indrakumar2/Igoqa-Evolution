using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using System.Collections.Generic;

namespace Evolution.AuditLog.Domain.Interfaces.Validations
{
    public interface ISqlAuditLogEventValidationService
    {
        IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
