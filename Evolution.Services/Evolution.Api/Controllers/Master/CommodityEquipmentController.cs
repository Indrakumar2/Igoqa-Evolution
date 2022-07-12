﻿using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;


namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/commodityequipment")]
    public class CommodityEquipmentController : Controller
    {
        private readonly ICommodityEquipmentService _service = null;
        private readonly IAppLogger<CommodityEquipmentController> _logger = null;

        public CommodityEquipmentController(ICommodityEquipmentService service,IAppLogger<CommodityEquipmentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(CommodityEquipment search)
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
