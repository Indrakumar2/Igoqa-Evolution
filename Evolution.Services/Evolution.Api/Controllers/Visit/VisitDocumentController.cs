using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Evolution.Api.Controllers.Visit
{
    [Produces("application/json")]
    [Route("api/visits/documents")]
    public class VisitDocumentController : Controller
    {
        private IDocumentService _service = null;
        private IVisitDocumentService _visitDocumentService = null;
        private readonly IAppLogger<VisitDocumentController> _logger = null;

        public VisitDocumentController(IDocumentService service,IVisitDocumentService visitDocumentService, IAppLogger<VisitDocumentController> logger)
        {
            _service = service;
            _visitDocumentService = visitDocumentService;
            _logger=logger;
        }

        [HttpGet]
        [Route("GetAssignmentVisitDocuments")]
        public Response GetListOfVisitDocuments([FromQuery] int assignmentId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _visitDocumentService.GetAssignmentVisitDocuments(assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpGet]
        [Route("GetSupplierPoVisitDocuments")]
        public Response GetSupplierPoVisitIds([FromQuery] int supplierPOId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _visitDocumentService.GetSupplierPoVisitIds(supplierPOId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        public Response Get([FromQuery]ModuleDocument searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}
