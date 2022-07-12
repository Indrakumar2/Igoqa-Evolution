using Evolution.Common.Enums;
using Evolution.Document.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Document.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate Document Approval Model which will come from client. 
    /// </summary>
    public interface IDocumentApprovalValidationService
    {
        /// <summary>
        /// Validate JSON model of Document Approval
        /// </summary>
        /// <param name="jsonModel"> Document Approval Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<DocumentApprovalValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
