using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Common.Models.Responses;
using DomainModel = Evolution.Master.Domain.Models;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/regions")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService = null;
        private readonly IAppLogger<RegionController> _logger = null;

        public RegionController(IRegionService regionService,IAppLogger<RegionController> logger)
        {
            _regionService = regionService;
            _logger = logger;
        }

        
        [HttpGet]
        public Response Get([FromQuery]DomainModel.Region search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _regionService.Search(search);
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
