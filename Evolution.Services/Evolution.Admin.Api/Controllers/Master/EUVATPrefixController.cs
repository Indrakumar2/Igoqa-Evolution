using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/EUVATPrefixes")]
    public class EUVATPrefixController : Controller
    {
        private readonly IAppLogger<EUVATPrefixController> _logger;
        private readonly ICountryService _service;

        public EUVATPrefixController(ICountryService service, IAppLogger<EUVATPrefixController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/<controller>
        [HttpGet]
        public Response Get()
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetCountryEUVatPrefix();
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}