using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Models.Validations
{
    /// <summary>
    /// Customer Validation Result
    /// </summary>
    public class CustomerValidationResult : BaseMessage
    {
        public CustomerValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
