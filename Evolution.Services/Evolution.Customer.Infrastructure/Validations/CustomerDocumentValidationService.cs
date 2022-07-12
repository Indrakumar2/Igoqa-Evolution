//using Evolution.Common.Enums;
//using Evolution.Customer.Domain.Interfaces.Validations;
//using Evolution.Customer.Domain.Models.Validations;
//using Evolution.ValidationService.Interfaces;
//using Evolution.ValidationService.Models;
//using Newtonsoft.Json.Schema;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Text;

//namespace Evolution.Customer.Infrastructure.Validations
//{
//    /// <summary>
//    /// This will responsible for validate Customer Document Model
//    /// </summary>
//    public class CustomerDocumentValidationService : ICustomerDocumentValidationService
//    {
//        private IValidationService _validation = null;

//        public CustomerDocumentValidationService(IValidationService validation) { this._validation = validation; }

//        /// <summary>
//        /// Validate JSON model of Customer Document
//        /// </summary>
//        /// <param name="jsonModel">Customer Document Model in Json format</param>
//        /// <param name="type">Type of Validation like Add or Modify</param>
//        /// <returns></returns>
//        public IList<CustomerValidationResult> Validate(string jsonModel, ValidationType type)
//        {
//            string jsonSchema = string.Empty;
//            IList<CustomerValidationResult> cuatomerValidationResults = new List<CustomerValidationResult>();

//            string jsonValidationSchemaPath = string.Empty;

//            if (type == ValidationType.Add)
//            {
//                jsonValidationSchemaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Customer", "Save", "CustomerDocument.json");
//            }
//            else
//            {
//                jsonValidationSchemaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Customer", "Update", "CustomerDocument.json");
//            }

//            using (StreamReader reader = new StreamReader(jsonValidationSchemaPath))
//            {
//                jsonSchema = reader.ReadToEnd();
//            }

//            ValidationResult validationResult = this._validation.Validate(jsonModel, jsonSchema);
//            if (validationResult.ValidationMessages != null)
//            {
//                foreach (ValidationError errors in validationResult.ValidationMessages)
//                {
//                    cuatomerValidationResults.Add(new CustomerValidationResult(errors.Path.ToString(), errors.Message));
//                }
//            }
//            return cuatomerValidationResults;
//        }
//    }
//}
