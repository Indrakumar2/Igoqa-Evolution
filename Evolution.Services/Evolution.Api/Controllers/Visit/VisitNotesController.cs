using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using System;
using Model = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Api.Controllers.Visit
{
    [Produces("application/json")]
    [Route("api/visits/{visitId}/notes")]
    public class VisitNotesController : Controller
    {
        IVisitNoteService _service = null;
        private readonly IAppLogger<VisitNotesController> _logger = null;

        public VisitNotesController(IVisitNoteService service, IAppLogger<VisitNotesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]long visitId, [FromQuery]Model.VisitNote searchModel)
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
