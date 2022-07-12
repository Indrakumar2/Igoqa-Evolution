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
    [Route("api/master/logo")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        private readonly IAppLogger<LogoController> _logger;
        private readonly ILogoService _service;

        public LogoController(ILogoService service, IAppLogger<LogoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Search([FromQuery] MasterData models)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Search(models, MasterType.Logo);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}