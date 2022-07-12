using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DomainModel = Evolution.Master.Domain.Models;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/regions")]
    public class RegionController : ControllerBase
    {
        private readonly IAppLogger<RegionController> _logger;
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService, IAppLogger<RegionController> logger)
        {
            _regionService = regionService;
            _logger = logger;
        }


        [HttpGet]
        public Response Get([FromQuery] DomainModel.Region search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _regionService.Search(search);
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