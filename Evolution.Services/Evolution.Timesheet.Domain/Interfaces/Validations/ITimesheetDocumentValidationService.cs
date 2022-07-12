using Evolution.Common.Enums;
using Evolution.Timesheet.Domain.Models.Validations;
using System.Collections.Generic;

namespace Evolution.Timesheet.Domain.Interfaces.Validations
{

    /// <summary>
    /// This will be responsible for Validating Timesheet Document Model which will be from the client
    /// </summary>
    public interface ITimesheetDocumentValidationService
    {
        /// <summary>
        /// Validate JSON Model of Timesheet Document
        /// </summary>
        /// <param name="jsonModel"></param>
        /// <param name="type"></param>
        /// <returns>Returns the List of validation Results of Timesheet Document</returns>
        IList<TimesheetDocumentValidationResult> Validate(string jsonModel, ValidationType type);

    }
}
