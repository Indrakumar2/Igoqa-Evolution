using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.ValidationService.Models;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Evolution.Assignment.Infrastructure.Validations
{
    public class AssignmentContributionRevenueCostValidationService : IAssignmentContributionRevenueCostValidationService
    {
        private readonly IValidationService _validation = null;

        public AssignmentContributionRevenueCostValidationService(IValidationService validation) { this._validation = validation; }

        public IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type)
        {
            string basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Assignment");
            string jsonSchema = string.Empty;
            var moduleValidationResults = new List<JsonPayloadValidationResult>();

            if (type == ValidationType.Add)
                basePath = Path.Combine(basePath, "Save", "AssignmentContributionRevenueCostAdd.json");
            else if (type == ValidationType.Update)
                basePath = Path.Combine(basePath, "Update", "AssignmentContributionRevenueCostUpdate.json");
            else if (type == ValidationType.Delete)
                basePath = Path.Combine(basePath, "Delete", "AssignmentContributionRevenueCostDelete.json");
            else
                return moduleValidationResults;

            using (StreamReader reader = new StreamReader(basePath))
            {
                jsonSchema = reader.ReadToEnd();
                ValidationResult validationResult = this._validation.Validate(jsonModel, jsonSchema);
                if (validationResult.ValidationMessages != null)
                {
                    foreach (ValidationError errors in validationResult.ValidationMessages)
                    {
                        moduleValidationResults.Add(new JsonPayloadValidationResult(errors.Path.ToString(), errors.Message));
                    }
                }
            }
            return moduleValidationResults;
        }
    }
}

