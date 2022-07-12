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
    [Route("api/master/supplierperformancetype")]
    public class SupplierPerformanceTypeController : Controller
    {
        private readonly IAppLogger<SupplierPerformanceTypeController> _logger;
        private readonly ISupplierPerformanceTypeService _service;


        public SupplierPerformanceTypeController(ISupplierPerformanceTypeService service,
            IAppLogger<SupplierPerformanceTypeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/master/assignment/referencetype//
        [HttpGet]
        public Response Get(SupplierPerformanceType search)
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