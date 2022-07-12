using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/documentation")]
    public class DocumentationController : Controller
    {
        private readonly IDocumentationService _documentationService;
        private readonly IAppLogger<DocumentationController> _logger;

        public DocumentationController(IDocumentationService documentationService,
            IAppLogger<DocumentationController> logger)
        {
            _documentationService = documentationService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(Documentation searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _documentationService.search(searchModel);
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