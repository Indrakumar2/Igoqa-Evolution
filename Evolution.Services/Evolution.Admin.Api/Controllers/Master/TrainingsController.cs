using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/Trainings")]
    public class TrainingsController : Controller
    {
        private readonly IAppLogger<TrainingsController> _logger;
        private readonly ITrainingsService _service;

        public TrainingsController(ITrainingsService service, IAppLogger<TrainingsController> logger)
        {
            _service = service;
            _logger = logger;
        }


        // GET: api/<controller>
        [HttpGet]
        public Response Get(Trainings search)
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