using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Enums;
using Evolution.Logging.Interfaces;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/charges/schedules/inspection/types")]
    public class CompanyChgSchInspGrpInspectionTypeController : Controller
    {
        private readonly ICompanyChgSchInspGrpInspectionType _service = null;
        private readonly IAppLogger<CompanyChgSchInspGrpInspectionTypeController> _logger = null;
        public CompanyChgSchInspGrpInspectionTypeController(ICompanyChgSchInspGrpInspectionType service,IAppLogger<CompanyChgSchInspGrpInspectionTypeController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        public Response Get(CompanyChgSchInspGrpInspectionType search)
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