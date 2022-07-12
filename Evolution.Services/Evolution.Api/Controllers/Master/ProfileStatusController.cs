﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/profilestatus")]
    public class ProfileStatusController : Controller
    {
        private readonly IProfileStatusService _service = null;
        private readonly IAppLogger<ProfileStatusController> _logger = null;

        public ProfileStatusController(IProfileStatusService service,IAppLogger<ProfileStatusController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(ProfileStatus search)
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
