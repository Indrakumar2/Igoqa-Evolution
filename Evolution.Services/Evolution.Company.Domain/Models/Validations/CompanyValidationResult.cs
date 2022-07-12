using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Models.Validations
{
    /// <summary>
    /// Company Validation Result
    /// </summary>
    public class CompanyValidationResult : BaseMessage
    {
        public CompanyValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
