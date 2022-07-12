using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/logo")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        private ILogoService _service = null;
        private readonly IAppLogger<LogoController> _logger = null;
        
        public LogoController(ILogoService service, IAppLogger<LogoController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet]
        public Response Search([FromQuery]MasterData models)
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
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);           
        }
    }
}