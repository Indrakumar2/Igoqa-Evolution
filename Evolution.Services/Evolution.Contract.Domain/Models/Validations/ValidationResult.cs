using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Contract.Domain.Models.Validations
{
    /// <summary>
    /// Contract Validation Result
    /// </summary>
    public class ContractValidationResult : BaseMessage
    {
        public ContractValidationResult(string code, string message):base(code,message)
        {

        }
        
    }
    
}
