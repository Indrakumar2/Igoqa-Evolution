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
    [Route("api/master/equipment")]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _service = null;
        private readonly IAppLogger<EquipmentController> _logger = null;

        public EquipmentController(IEquipmentService service, IAppLogger<EquipmentController> logger)
        {
            _service = service;
            this._logger=logger;
        }

        [HttpGet]
        public Response Get(Equipment search)
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
