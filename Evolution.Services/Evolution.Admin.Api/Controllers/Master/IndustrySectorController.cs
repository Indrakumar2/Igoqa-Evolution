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
    [Route("api/master/industrysectors")]
    public class IndustrySectorController : Controller
    {
        private readonly IAppLogger<IndustrySectorController> _logger;
        public readonly IIndustrySectorService _service;

        public IndustrySectorController(IIndustrySectorService service, IAppLogger<IndustrySectorController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpGet]
        public Response Get(IndustrySector model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Search(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}