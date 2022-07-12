using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DomModel = Evolution.Master.Domain.Models;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/moduletype")]
    public class ModuleTypeController : ControllerBase
    {
        private readonly IAppLogger<ModuleTypeController> _logger;
        private readonly IModuleTypeService _moduleTypeService;

        public ModuleTypeController(IModuleTypeService moduleTypeService, IAppLogger<ModuleTypeController> logger)
        {
            _moduleTypeService = moduleTypeService;
            _logger = logger;
        }


        [HttpGet]
        public Response Get([FromQuery] DomModel.MasterModuleTypes search)
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
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}