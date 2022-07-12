using Evolution.Common.Enums;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Evolution.Timesheet.Domain.Models.Validations;
using Evolution.ValidationService.Interfaces;
using System.Collections.Generic;

namespace Evolution.Timesheet.Infrastructure.Validations
{
    public class TimsheetDocumentValidationResult :ITimesheetDocumentValidationService
    {
        private readonly IValidationService _validation = null;
        public TimsheetDocumentValidationResult(IValidationService validation)
        {
            _validation = validation;
        }


        // <summary>
        /// Validates the JSON Schema of Timesheet document
        /// TODO: // JSON Schema Validation
        /// </summary>
        /// <param name="jsonModel"></param>
        /// <param name="type"></param>
        /// <returns>Returns The list of ValidationResult</returns>
        public IList<TimesheetDocumentValidationResult> Validate(string jsonModel, ValidationType type)
        {
            string jsonSchema = string.Empty;

            if (type == ValidationType.Add)
            {
                jsonSchema = "Read Json Schema for Add";
            }
            else
            {
                jsonSchema = "Read Json Schema for Update";
            }

           // var results = _validation.Validate(jsonModel, jsonSchema);
            return null;
        }
    }
}
