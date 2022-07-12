using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DomianModel = Evolution.Master.Domain.Models;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/taxonomyBusinessUnits")]
    public class TaxonomyBusinessUnitController : Controller
    {
        private readonly IAppLogger<TaxonomyBusinessUnitController> _logger;
        private readonly ITaxonomyBusinessUnitService _service;

        public TaxonomyBusinessUnitController(ITaxonomyBusinessUnitService service,
            IAppLogger<TaxonomyBusinessUnitController> logger)
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