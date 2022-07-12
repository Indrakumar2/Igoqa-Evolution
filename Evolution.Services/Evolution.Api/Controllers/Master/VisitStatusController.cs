using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/visitstatus")]
    public class VisitStatusController : Controller
    {
        private readonly IVisitStatusService _visitStatusService = null;
        private readonly IAppLogger<VisitStatusController> _logger = null;


        public VisitStatusController(IVisitStatusService visitStatusService, IAppLogger<VisitStatusController> logger)
        {
            _visitStatusService = visitStatusService;
            _logger = logger;
        }


        [HttpGet]
        public Response Get(VisitStatus searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _visitStatusService.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }


    }
}
