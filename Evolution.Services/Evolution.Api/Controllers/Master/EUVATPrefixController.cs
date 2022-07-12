using AutoMapper;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/EUVATPrefixes")]
    public class EUVATPrefixController : Controller
    {
        private readonly ICountryService _service = null;
         private readonly IAppLogger<EUVATPrefixController> _logger = null;

        public EUVATPrefixController(ICountryService service, IAppLogger<EUVATPrefixController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        // GET: api/<controller>
        [HttpGet]
        public Response Get()
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;            
            try
            {
                  return this._service.GetCountryEUVatPrefix();
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
           
        }
    }
}
