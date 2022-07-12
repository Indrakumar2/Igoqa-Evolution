using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Model = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Api.Controllers.Visit
{
    [Produces("application/json")]
    [Route("api/visits/{visitId}/references")]    
    public class VisitReferencesController : Controller
    {
        IVisitReferenceService _service = null;
        private readonly IAppLogger<VisitReferencesController> _logger = null;

        public VisitReferencesController(IVisitReferenceService service, IAppLogger<VisitReferencesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]        
        public Response Get([FromRoute]long visitId, [FromQuery]Model.VisitReference searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.VisitId = visitId;
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
