using AutoMapper;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/supplierperformancetype")]
    public class SupplierPerformanceTypeController : Controller
    {
        private readonly ISupplierPerformanceTypeService _service = null;
        private readonly IAppLogger<SupplierPerformanceTypeController> _logger = null;


        public SupplierPerformanceTypeController(ISupplierPerformanceTypeService service,IAppLogger<SupplierPerformanceTypeController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        // GET: api/master/assignment/referencetype//
        [HttpGet]
        public Response Get(SupplierPerformanceType search)
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
