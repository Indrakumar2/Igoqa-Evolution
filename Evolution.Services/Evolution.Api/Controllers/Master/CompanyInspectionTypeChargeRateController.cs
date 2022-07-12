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
    [Route("api/master/charges/inspection/types/rates")]
    public class CompanyInspectionTypeChargeRateController : Controller
    {
        private readonly ICompanyInspectionTypeChargeRate _service = null;
        private readonly IAppLogger<CompanyInspectionTypeChargeRateController> _logger = null;

        public CompanyInspectionTypeChargeRateController(ICompanyInspectionTypeChargeRate service,IAppLogger<CompanyInspectionTypeChargeRateController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(CompanyInspectionTypeChargeRate search)
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
