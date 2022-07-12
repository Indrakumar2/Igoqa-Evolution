using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Document.Domain.Models.Validations
{
    /// <summary>
    /// Document Validation Result
    /// </summary>
    public class DocumentApprovalValidationResult : BaseMessage
    {
        public DocumentApprovalValidationResult(string code, string message):base(code,message)
        {
        }
    }
}
