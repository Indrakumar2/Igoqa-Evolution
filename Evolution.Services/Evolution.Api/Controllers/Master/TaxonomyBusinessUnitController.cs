using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Models.Responses;
using DomianModel = Evolution.Master.Domain.Models;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/taxonomyBusinessUnits")]
    public class TaxonomyBusinessUnitController : Controller
    {
        private readonly ITaxonomyBusinessUnitService _service = null;
        private readonly IAppLogger<TaxonomyBusinessUnitController> _logger = null;

        public TaxonomyBusinessUnitController(ITaxonomyBusinessUnitService service,IAppLogger<TaxonomyBusinessUnitController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public Response Get(DomianModel.TaxonomyBusinessUnit model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _service.Search(model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
       
    }
}
