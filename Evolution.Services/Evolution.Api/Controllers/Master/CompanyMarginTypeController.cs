using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Evolution.Common.Models.Responses;
using Evolution.Master.Core;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;


namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/companymargintypes")]
    public class CompanyMarginTypeController : Controller
    {
        private readonly ICompanyMarginType _service = null;
        private readonly IAppLogger<CompanyMarginTypeController> _logger = null; 

        public CompanyMarginTypeController(ICompanyMarginType service,IAppLogger<CompanyMarginTypeController> logger)
        {
            _service = service; 
            _logger = logger;
        }
         
        [HttpGet]
        public Response Get(CompanyMarginType search)
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
