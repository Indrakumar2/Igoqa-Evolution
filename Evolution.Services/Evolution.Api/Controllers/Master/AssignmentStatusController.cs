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
    [Route("api/assignment/status")]
    public class AssignmentStatusController : Controller
    {
      private readonly IAssignmentStatusService _service =null;
      private readonly IAppLogger<AssignmentStatusController> _logger = null;

        public AssignmentStatusController(IAssignmentStatusService service, IAppLogger<AssignmentStatusController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(AssignmentStatus search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

       
    }
}
