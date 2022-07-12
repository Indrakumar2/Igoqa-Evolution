using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/industrysectors")]
    public class IndustrySectorController : Controller
    {
        public readonly IIndustrySectorService _service = null;
        private readonly IAppLogger<IndustrySectorController> _logger = null;

        public IndustrySectorController(IIndustrySectorService service, IAppLogger<IndustrySectorController> logger)
        {
           this._service = service;
            this._logger = logger;
        }

      
        [HttpGet]
        public Response Get(IndustrySector model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 return this._service.Search(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
            
        }

     
    }
}
