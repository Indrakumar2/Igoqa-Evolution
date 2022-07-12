using Evolution.Document.Domain.Interfaces.Validations;
using System;
using Evolution.Common.Enums;
using Evolution.Document.Domain.Models.Validations;
using System.Collections.Generic;
using Evolution.ValidationService.Interfaces;
using System.IO;
using Evolution.ValidationService.Models;
using Newtonsoft.Json.Schema;
using System.Reflection;

namespace Evolution.Document.Infrastructe.Validations
{
    /// <summary>
    /// This will responsible to validate Document Approval Model
    /// </summary>
    public class DocumentApprovalValidationService : IDocumentApprovalValidationService
    {
        private IValidationService _validation = null;

        public DocumentApprovalValidationService(IValidationService validation) { this._validation = validation; }

        /// <summary>
        /// Validate JSON model of Document approval
        /// </summary>
        /// <param name="jsonModel">Document Approval Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        public IList<DocumentApprovalValidationResult> Validate(string jsonModel, ValidationType type)
        {
            string jsonSchema = string.Empty;
            IList<DocumentApprovalValidationResult> documentsValidationResults = new List<DocumentApprovalValidationResult>();
            if (type == ValidationType.Add)
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Save", "DocumentApprovalAdd.json");
                StreamReader reader = new StreamReader(path);
                jsonSchema = reader.ReadToEnd();
            }
            else
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Update", "DocumentApprovalUpdate.json");
                StreamReader reader = new StreamReader(path);
                jsonSchema = reader.ReadToEnd();
            }

            ValidationResult validationResult = this._validation.Validate(jsonModel, jsonSchema);
            if (validationResult.ValidationMessages != null)
            {
                foreach (ValidationError errors in validationResult.ValidationMessages)
                {
                    documentsValidationResults.Add(new DocumentApprovalValidationResult(errors.Path.ToString(), errors.Message));
                }
            }
            return documentsValidationResults;
        }
    }
}
