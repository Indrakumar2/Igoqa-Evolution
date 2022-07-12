using System;
using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/assignments/lifecycles")]
    public class AssignmentLifeCycleController : Controller
    {
        private readonly IAppLogger<AssignmentLifeCycleController> _logger;
        private readonly IAssignmentLifeCycleService _service;

        public AssignmentLifeCycleController(IMapper mapper, IAssignmentLifeCycleService service,
            IAppLogger<AssignmentLifeCycleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(MasterData search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}