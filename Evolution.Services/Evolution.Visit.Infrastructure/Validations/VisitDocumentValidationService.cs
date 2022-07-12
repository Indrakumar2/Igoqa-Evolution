using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.Visit.Domain.Interfaces.Validations;
using System.Collections.Generic;

namespace Evolution.Visit.Infrastructure.Validations
{
    public class VisitDocumentValidationService : IVisitDocumentValidationService
    {
        private readonly IValidationService _validation = null;
        public VisitDocumentValidationService(IValidationService validation)
        {
            _validation = validation;
        }

        public IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type)
        {
            string jsonSchema = string.Empty;
            string jsonSchemaValidationPath = string.Empty;
            IList<JsonPayloadValidationResult> validationResults = new List<JsonPayloadValidationResult>();

            //if (type == ValidationType.Add)
            //    jsonSchemaValidationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Visit", "Save", "VisitDocument.json");
            //if (type == ValidationType.Update)
            //    jsonSchemaValidationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Visit", "Update", "VisitDocument.json");
            //if (type == ValidationType.Delete)
            //    jsonSchemaValidationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Visit", "Delete", "VisitDocument.json");

            //using (StreamReader reader = new StreamReader(jsonSchemaValidationPath))
            //{
            //    jsonSchema = reader.ReadToEnd();
            //}
            //ValidationResult validationResult = _validation.Validate(jsonModel, jsonSchema);

            //if (validationResult.ValidationMessages != null)
            //{
            //    foreach (ValidationError error in validationResult.ValidationMessages)
            //    {
            //        validationResults.Add(new JsonPayloadValidationResult(error.Path.ToString(), error.Message));
            //    }
            //}
            return validationResults;
        }
    }
}
