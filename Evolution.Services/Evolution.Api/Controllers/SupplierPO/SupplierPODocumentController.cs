using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Evolution.Api.Controllers.SupplierPO
{
    [Produces("application/json")]
    [Route("api/SupplierPO/documents")]
    public class SupplierPODocumentController : Controller
    {
        private IDocumentService _service = null;
        private readonly IAppLogger<SupplierPODocumentController> _logger = null;

        public SupplierPODocumentController(IDocumentService service, IAppLogger<SupplierPODocumentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("GetAssignmentSupplierPOocuments")]
        public Response GetListOfSupplierPODocuments([FromBody]List<int> supplierPOIds)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                IList<ModuleDocument> searchModel = new List<ModuleDocument>();
                supplierPOIds?.ForEach(x =>
                {
                    searchModel.Add(new ModuleDocument { ModuleCode = "SUPPO", ModuleRefCode = x.ToString() });

                });
                return _service.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOIds);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}
