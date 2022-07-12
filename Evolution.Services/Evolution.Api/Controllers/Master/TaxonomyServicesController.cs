using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Master.Core.Services;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/TaxonomyServices")]
    public class TaxonomyServicesController : ControllerBase
    {
        private readonly ITaxonomyServices _service = null;
        private readonly IAppLogger<TaxonomyServicesController> _logger = null;

        public TaxonomyServicesController(ITaxonomyServices service,IAppLogger<TaxonomyServicesController> logger)
        {
            _service = service;
            _logger = logger;
        }


        // GET: api/<controller>
        [HttpGet]
        public Response Get(TaxonomyService search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._service.Search(search);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

    }
}