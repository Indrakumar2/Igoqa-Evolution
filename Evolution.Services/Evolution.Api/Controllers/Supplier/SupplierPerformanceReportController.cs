using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;


namespace Evolution.Api.Controllers.Supplier
{
    [Route("api/suppliers/performancereport")]
    public class SupplierPerformanceReportController : Controller
    {
        private ISupplierPerformanceReportService _service = null;
        private readonly IAppLogger<SupplierPerformanceReportController> _logger = null;

        public SupplierPerformanceReportController(ISupplierPerformanceReportService service, IAppLogger<SupplierPerformanceReportController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] DomainModel.SupplierPerformanceReportsearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Get(searchModel);
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
