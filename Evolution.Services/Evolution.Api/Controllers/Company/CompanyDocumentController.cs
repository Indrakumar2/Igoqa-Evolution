//using Evolution.Common.Interfaces.Services;
//using Evolution.Common.Models.Responses;
//using Evolution.Document.Domain.Interfaces.Documents;
//using Evolution.Document.Domain.Models.Document;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;

//namespace Evolution.Api.Controllers.Company
//{
//    [Route("api/companies/{companyCode}/documents")]
//    [ApiController]
//    public class CompanyDocumentController : ControllerBase
//    {
//        private readonly IDocumentService _service = null;

//        public CompanyDocumentController(IDocumentService service)
//        {
//            this._service = service;
//        }
        
//        // GET api/companies/A00004/document        
//        [HttpGet]
//        public Response Get([FromRoute]string companyCode, [FromQuery]ModuleDocument model)
//        {
//            return this._service.Get(model);

//        }

//        // POST api/companies/A00004/document
//        [HttpPost]
//        public Response Post([FromRoute]string companyCode, [FromBody] IList<ModuleDocument> model)
//        {
//            return this._service.Save(model);
//        }

//        // PUT api/companies/A00004/document
//        [HttpPut]
//        public Response Put([FromRoute]string companyCode, [FromBody] IList<ModuleDocument> model)
//        {
//            return this._service.Modify(model);
//        }
        
//        // DELETE api/companies/A00004/document
//        [HttpDelete]
//        public Response Delete([FromRoute]string companyCode, [FromBody] IList<ModuleDocument> model)
//        {
//            return this._service.Modify(model);
//        }

//        [HttpPost]
//        [Route("GetCompanySpecificDocuments")]
//        public Response GetListOfCompanytDocuments([FromBody]List<string> CompanyCode)
//        {
//            //return _service.GetListOfContractDocuments(contractNumber);
//            return null;
//        }
//        [HttpPut]
//        [Route("ChangeDocumentStatus")]
//        public Response ChangeDocumentStatus([FromBody] IList<ModuleDocument> model)
//        {
//            return this._service.ChangeDocumentStatus(model);
//        }

//        [HttpPost]
//        [Route("GenrateDocumentUniqueName")]
//        public Response GenrateDocumentUniqueName([FromBody] IList<DocumentUniqueNameDetail> model)
//        {
//            return this._service.GenerateDocumentUniqueName(model);
//        }
//    }
//}