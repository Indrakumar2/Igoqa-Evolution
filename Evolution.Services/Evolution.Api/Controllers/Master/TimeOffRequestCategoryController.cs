using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evolution.Master.Core.Services;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
   
        [Route("api/master/timeoffrequest/category")]
        public class TimeOffRequestCategoryController : ControllerBase
        {
            private readonly ITimeOffRequestService _service = null;
            private readonly IAppLogger<TimeOffRequestCategoryController> _logger = null;


            public TimeOffRequestCategoryController(ITimeOffRequestService service,IAppLogger<TimeOffRequestCategoryController> logger)
            {
                _service = service;
                _logger = logger;
            }


            // GET: api/<controller>
            [HttpGet]
            public Response Get(TimeOffRequestCategory search)
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
