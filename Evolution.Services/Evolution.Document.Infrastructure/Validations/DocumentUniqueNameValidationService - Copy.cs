//using Evolution.Common.Enums;
//using Evolution.Document.Domain.Models.Validations;
//using Evolution.Document.Domain.Validations;
//using Evolution.ValidationService.Interfaces;
//using Evolution.ValidationService.Models;
//using Newtonsoft.Json.Schema;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;

//namespace Evolution.Document.Infrastructure.Validations
//{
//    public class DocumentUniqueNameValidationService : IDocumentUniqueNameValidationService
//    { 
//        private IValidationService _validation = null;

//        public DocumentUniqueNameValidationService(IValidationService validation)
//        {
//            this._validation = validation;
//        }

//        public IList<DocumentApprovalValidationResult> Validate(string jsonModel, ValidationType type)
//        {
//            string jsonSchema = string.Empty;
//            string path = string.Empty;
//            IList<DocumentApprovalValidationResult> documentsValidationResults = new List<DocumentApprovalValidationResult>();
//            if (type == ValidationType.Add)
//            {
//                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Save", "DocumentUniqueNameAdd.json");
//            }
//            else if (type == ValidationType.Update)
//            {
//                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Update", "DocumentUniqueNameUpdate.json");
//            }
//            else if (type == ValidationType.Delete)
//            {
//                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Document", "Delete", "DocumentUniqueNameDelete.json");
//            }

//            StreamReader reader = new StreamReader(path);
//            jsonSchema = reader.ReadToEnd();
//            ValidationResult validationResult = this._validation.Validate(jsonModel, jsonSchema);
//            if (validationResult.ValidationMessages != null)
//            {
//                foreach (ValidationError errors in validationResult.ValidationMessages)
//                {
//                    documentsValidationResults.Add(new DocumentApprovalValidationResult(errors.Path.ToString(), errors.Message));
//                }
//            }
//            return documentsValidationResults;
//        }
//    }
//}
