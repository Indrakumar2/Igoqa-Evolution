using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Document.Domain.Interfaces.Documents
{
    public interface IDocumentApprovalService
    {
        Response SaveDocumentForApproval(IList<Models.Document.DocumentApproval> documentApprovals);
        
        Response ModifyDocumentForApproval(Models.Document.DocumentApproval documentApproval);
        
        Response GetDocumentForApproval(Models.Document.DocumentApproval searchModel);
    }
}
