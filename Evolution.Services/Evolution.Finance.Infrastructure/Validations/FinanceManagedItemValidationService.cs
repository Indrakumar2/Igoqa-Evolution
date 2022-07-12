using Evolution.Common.Enums;
using Evolution.Finance.Domain.Interfaces.Validations;
using Evolution.Finance.Domain.Models.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.ValidationService.Models;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Evolution.Finance.Infrastructure.Validations
{
    public class FinanceManagedItemValidationService : IFinanceManageItemValidationService
    {
        private IValidationService _validation = null;

        public FinanceManagedItemValidationService(IValidationService validation)
        {
            _validation = validation;
        }

        public IList<FinanceManagedItemValidationResult> Validate(string jsonModel, ValidationType type)
        {
            string jsonSchema = string.Empty;
            string jsonValidationSchemaPath = string.Empty;

            IList<FinanceManagedItemValidationResult> financeValidationService = new List<FinanceManagedItemValidationResult>();

            if (type == ValidationType.Add)
                jsonValidationSchemaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Finance", "Save", "FinanceManagedItemAdd.json");
            if (type == ValidationType.Update)
                jsonValidationSchemaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Finance", "Update", "FinanceManagedItemUpdate.json");
            if (type == ValidationType.Delete)
                jsonValidationSchemaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonValidationSchemas", "Finance", "Delete", "FinanceManagedItemDelete.json");


            using (StreamReader reader = new StreamReader(jsonValidationSchemaPath))
            {
                jsonSchema = reader.ReadToEnd();
            }

            ValidationResult validationResult = _validation.Validate(jsonModel, jsonSchema);
            if (validationResult.ValidationMessages != null)
            {
                foreach (ValidationError error in validationResult.ValidationMessages)
                {
                    financeValidationService.Add(new FinanceManagedItemValidationResult(error.Path.ToString(), error.Message));
                }
            }
            return financeValidationService;
        }
 
    }
}
