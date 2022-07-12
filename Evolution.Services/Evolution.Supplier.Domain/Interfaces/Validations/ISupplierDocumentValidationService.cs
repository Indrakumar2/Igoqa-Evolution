using Evolution.Common.Enums;
using Evolution.Supplier.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Supplier.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate Customer Model which will come from client. 
    /// </summary>
    public interface ISupplierDocumentValidationService
    {
        /// <summary>
        /// Validate JSON model of Contact
        /// </summary>
        /// <param name="jsonModel">Contact Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
       IList<SupplierValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
