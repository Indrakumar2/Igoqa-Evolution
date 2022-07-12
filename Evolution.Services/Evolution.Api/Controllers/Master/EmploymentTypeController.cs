using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/employmenttype")]
    public class EmploymentTypeController : Controller
    {
        private readonly IEmploymentTypeService _service = null;
        private readonly IAppLogger<EmploymentTypeController> _logger = null;

        public EmploymentTypeController(IEmploymentTypeService service, IAppLogger<EmploymentTypeController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get(EmploymentType search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
            
        }


    }
}
