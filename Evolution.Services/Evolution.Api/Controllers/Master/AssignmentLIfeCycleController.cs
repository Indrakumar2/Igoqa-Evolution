using AutoMapper;
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
    [Route("api/master/assignments/lifecycles")]
    public class AssignmentLifeCycleController : Controller
    {
        private readonly IAssignmentLifeCycleService _service = null;
        private readonly IAppLogger<AssignmentLifeCycleController> _logger = null;

        public AssignmentLifeCycleController(IMapper mapper, IAssignmentLifeCycleService service, IAppLogger<AssignmentLifeCycleController> logger)
        {
            this._service = service;
            _logger = logger;


        }
        
        [HttpGet]
        public Response Get(MasterData search)
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
