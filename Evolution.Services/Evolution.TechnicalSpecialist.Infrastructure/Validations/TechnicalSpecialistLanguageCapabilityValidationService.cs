using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.ValidationService.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ManateeSchema = Manatee.Json.Schema;

namespace Evolution.TechnicalSpecialist.Infrastructure.Validations
{
    public class TechnicalSpecialistLanguageCapabilityValidationService : ITechnicalSpecialistLanguageCapabilityValidationService
    {
        private IValidationService _validation = null;
        private const string _expected = "expected";
        private const string _actual = "actual";

        public TechnicalSpecialistLanguageCapabilityValidationService(IValidationService validation)
        {
            this._validation = validation;
        }

        public IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type)
        {
            //string basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "TechnicalSpecialist");
            ManateeSchema.SchemaValidationResults results = null;
            string basePath = string.Empty;
            var moduleValidationResults = new List<JsonPayloadValidationResult>();

            if (type == ValidationType.Add)
                basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "TechnicalSpecialist", "Save", "TechnicalSpecialistLanguageCapabilityAdd.json");
            else if (type == ValidationType.Update)
                basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "TechnicalSpecialist", "Update", "TechnicalSpecialistLanguageCapabilityUpdate.json");
            else if (type == ValidationType.Delete)
                basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "TechnicalSpecialist", "Delete", "TechnicalSpecialistLanguageCapabilityDelete.json");
            else
                return moduleValidationResults;

            this._validation.Validate(jsonModel, basePath, ref results);
            if (results?.NestedResults?.Count > 0)
                for (int i = 0; i < results.NestedResults.Count; i++)
                {
                    if (results.NestedResults[i].RelativeLocation.Count > 2 && results.NestedResults[i].AdditionalInfo.Count > 0)
                        moduleValidationResults.Add(new JsonPayloadValidationResult(results.NestedResults[i].RelativeLocation[2],
                                                                                     string.Format("{0} {1} Error,Expected - {2} - Actual - {3}",
                                                                                     results.NestedResults[i].RelativeLocation[2],
                                                                                     results.NestedResults[i].RelativeLocation[3],
                                                                                     results.NestedResults[i].AdditionalInfo[_expected].ToString().Trim('\"'),
                                                                                     results.NestedResults[i].AdditionalInfo[_actual].ToString().Trim('\"'))));
                }
            else
                if (results?.RelativeLocation?.Count > 0 && results?.AdditionalInfo?.Count > 0)
                moduleValidationResults.Add(new JsonPayloadValidationResult(results.RelativeLocation[2],
                                                                             string.Format("{0} {1} Error,Expected - {2} - Actual - {3}",
                                                                             results.RelativeLocation[2],
                                                                             results.RelativeLocation[3],
                                                                             results.AdditionalInfo[_expected].ToString().Trim('\"'),
                                                                             results.AdditionalInfo[_actual].ToString().Trim('\"'))));
            return moduleValidationResults;
        }
    }
}
