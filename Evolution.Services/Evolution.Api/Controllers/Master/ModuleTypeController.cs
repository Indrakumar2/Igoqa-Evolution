using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using DomModel = Evolution.Master.Domain.Models;
using Evolution.Api.Controllers.Base;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/moduletype")]
    public class ModuleTypeController : ControllerBase
    {
        private readonly IModuleTypeService _moduleTypeService = null;
        private readonly IAppLogger<ModuleTypeController> _logger = null;

        public ModuleTypeController(IModuleTypeService moduleTypeService, IAppLogger<ModuleTypeController> logger)
        {
            _moduleTypeService = moduleTypeService;
            this._logger = logger;
        }

        
        [HttpGet]
        public Response Get([FromQuery]DomModel.MasterModuleTypes search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _moduleTypeService.Search(search);
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
