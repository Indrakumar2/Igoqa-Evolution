using Evolution.Common.Enums;
using Evolution.Company.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Interfaces.Validations
{
    /// <summary>
    /// This will be responsible to validate Company Remittance Model which will come from client. 
    /// </summary>
    public interface ICompanyRemittanceValidationService
    {
        /// <summary>
        /// Validate JSON model of Company Remittance
        /// </summary>
        /// <param name="jsonModel">Company Remittance Model in Json format</param>
        /// <param name="type">Type of Validation like Add or Modify</param>
        /// <returns></returns>
        IList<CompanyValidationResult> Validate(string jsonModel, ValidationType type);
    }
}
