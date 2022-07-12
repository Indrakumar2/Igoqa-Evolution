
using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Supplier.Domain.Models.Validations
{
   public class SupplierValidationResult : BaseMessage
    {
        public SupplierValidationResult(string code,string message): base(code,message) 
        {

        }
    }
}
