////using Evolution.Common.Models.Responses;
////using Evolution.Document.Domain.Interfaces.Documents;
////using Evolution.Document.Domain.Models.Document;
////using Microsoft.AspNetCore.Mvc;
////using System.Collections.Generic;

////namespace Evolution.Api.Controllers.Customer
////{
////    [Route("api/customers/{customerCode}/documents")]
////    [ApiController]
////    public class CustomerDocumentController : ControllerBase
////    {        
////        private readonly IDocumentService _service =null;
        
////        public CustomerDocumentController(IDocumentService service)
////        {
////            this._service = service;
////        }

////        // GET api/customers/A00004/document        
////        [HttpGet]
////        public Response Get([FromRoute]string customerCode, [FromQuery] ModuleDocument model)
////        {
////            return this._service.Get(model);
////        }

////        // POST api/customers/A00004/document        
////        [HttpPost]
////        public Response Post([FromRoute]string customerCode, [FromBody] IList<ModuleDocument> model)
////        {
////            return this._service.Save(model);
////        }

////        // PUT api/customers/A00004/document
////        [HttpPut]
////        public Response Put([FromRoute]string customerCode, [FromBody] IList<ModuleDocument> model)
////        {
////            return this._service.Modify(model);
////        }

////        // DELETE api/customers/A00004/document        
////        [HttpDelete]
////        public Response Delete([FromRoute]string customerCode, [FromBody] IList<ModuleDocument> model)
////        {
////            return this._service.Delete(model);
////        }

////        [HttpPut]
////        [Route("ChangeDocumentStatus")]
////        public Response ChangeDocumentStatus([FromBody] IList<ModuleDocument> model)
////        {
////            return this._service.ChangeDocumentStatus(model);
////        }

////        [HttpPost]
////        [Route("GenrateDocumentUniqueName")]
////        public Response GenrateDocumentUniqueName([FromBody] IList<DocumentUniqueNameDetail> model)
////        {
////            return this._service.GenerateDocumentUniqueName(model);
////        }
////    }
////}