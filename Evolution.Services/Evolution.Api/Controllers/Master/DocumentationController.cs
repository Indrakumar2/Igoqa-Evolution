using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;


namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/documentation")]
    public class DocumentationController : Controller
    {
        private readonly IDocumentationService _documentationService = null;
        private readonly IAppLogger<DocumentationController> _logger = null; 

        public DocumentationController(IDocumentationService documentationService,IAppLogger<DocumentationController> logger)
        {
            _documentationService = documentationService;
            _logger =logger;
        }

        [HttpGet]
        public Response Get(Documentation searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._documentationService.search(searchModel);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

      
    }
}
