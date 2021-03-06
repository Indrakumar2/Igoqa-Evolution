using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/managedservicetypes")]
    public class ManagedServiceTypeController : Controller
    {
        private readonly IManagedServiceTypeService _service = null;
        private readonly IAppLogger<ManagedServiceTypeController> _logger = null;

        public ManagedServiceTypeController(IManagedServiceTypeService service, IAppLogger<ManagedServiceTypeController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get(ManagedServiceType search)
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
