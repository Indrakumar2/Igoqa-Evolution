using Evolution.Common.Enums;
using Evolution.Common.Models.Validations;
using Evolution.Document.Domain.Validations;
using Evolution.ValidationService.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ManateeSchema = Manatee.Json.Schema;

namespace Evolution.Document.Infrastructure.Validations
{
    public class DocumentValidationService : IDocumentValidationService
    {
        private IValidationService _validation = null;
        private const string _expected = "expected";
        private const string _actual = "actual";
        private const string _docValidationMessage = "Document Type Not Supported – Following Document Types Supported - Doc,Docx,Pdf,Txt,Msg,Xls,Xlsx,Jpg,Jpeg,Png,Bmp,Gif,Csv,Zip,Rar";

        public DocumentValidationService(IValidationService validation)
        {
            this._validation = validation;
        }

        public IList<JsonPayloadValidationResult> Validate(string jsonModel, ValidationType type)
        {
            string jsonSchema = string.Empty;
            string path = string.Empty;
            ManateeSchema.SchemaValidationResults results = null;
            IList<JsonPayloadValidationResult> moduleValidationResults = new List<JsonPayloadValidationResult>();

            if (type == ValidationType.Add)
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Save", "DocumentAdd.json");
            if (type == ValidationType.Update)
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Update", "DocumentUpdate.json");
            if (type == ValidationType.Delete)
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Delete", "DocumentDelete.json");


            this._validation.Validate(jsonModel, path, ref results);
            if (results?.NestedResults?.Count > 0)
                for (int i = 0; i < results.NestedResults.Count; i++)
                {
                    if (results.NestedResults[i].RelativeLocation.Count > 2 && results.NestedResults[i].AdditionalInfo.Count > 0)
                    {
                        if (results.NestedResults[i].AdditionalInfo.ContainsKey(_expected) && results.NestedResults[i].AdditionalInfo.ContainsKey(_actual))
                        {
                            moduleValidationResults.Add(new JsonPayloadValidationResult(results.NestedResults[i].RelativeLocation[2],
                                                                                         string.Format("{0} {1} Error,Expected - {2} - Actual - {3}",
                                                                                         results.NestedResults[i].RelativeLocation[2],
                                                                                         results.NestedResults[i].RelativeLocation[3],
                                                                                         results.NestedResults[i].AdditionalInfo[_expected].ToString().Trim('\"'),
                                                                                         results.NestedResults[i].AdditionalInfo[_actual].ToString().Trim('\"'))));
                        }
                        else {
                            moduleValidationResults.Add(new JsonPayloadValidationResult(results.NestedResults[i].RelativeLocation[2],
                                                                                         _docValidationMessage));
                        }
                    }
                }
            else
            {
                if (results?.RelativeLocation?.Count > 0 && results?.AdditionalInfo?.Count > 0)
                {
                    if (results.AdditionalInfo.ContainsKey(_expected) && results.AdditionalInfo.ContainsKey(_actual))
                    {
                        moduleValidationResults.Add(new JsonPayloadValidationResult(results.RelativeLocation[2],
                                                                                 string.Format("{0} {1} Error",
                                                                                 results.RelativeLocation[2],
                                                                                 results.RelativeLocation[3],
                                                                                 results.AdditionalInfo[_expected].ToString().Trim('\"'),
                                                                                 results.AdditionalInfo[_actual].ToString().Trim('\"')
                                                                                 )));
                    }
                    else
                    {
                        moduleValidationResults.Add(new JsonPayloadValidationResult(results.RelativeLocation[2],
                                                                                     _docValidationMessage));
                    }
                }
            }
            return moduleValidationResults;
        }
    }
}
