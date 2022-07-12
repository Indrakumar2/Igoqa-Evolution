using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Document.Domain.Models.Document;

namespace Evolution.Api.Controllers.Document
{
    [Route("api/documents/approval")]
    [ApiController]
    [Produces("application/json")]
    public class DocumentApprovalController : ControllerBase
    {
        IDocumentApprovalService _documentApprovalService = null;

        public DocumentApprovalController(IDocumentApprovalService service)
        {
            this._documentApprovalService = service;
        }
        
        [HttpGet]
        public Response Get([FromQuery]DocumentApproval model)
        {
            return this._documentApprovalService.GetDocumentForApproval(model);
        }
        
        [HttpPost]
        public Response Post([FromBody]IList<DocumentApproval> model)
        {
            return _documentApprovalService.SaveDocumentForApproval(model);
        }
        
        [HttpPut]
        public Response Put([FromBody]DocumentApproval model)
        {
            return _documentApprovalService.ModifyDocumentForApproval(model);
        }
    }
}