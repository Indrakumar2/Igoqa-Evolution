using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Messages;

namespace Evolution.SupplierPO.Domain.Models.Valildation
{
    public class SupplierPOValildationResult : BaseMessage
    {
        public SupplierPOValildationResult(string code, string message) : base(code, message)
        {
        }
    }
}
