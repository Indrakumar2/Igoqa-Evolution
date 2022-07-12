using System;
using System.Collections.Generic;
using System.Linq;
using Evolution.Admin.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;
using Model = Evolution.Security.Domain.Models.Security;

namespace Evolution.Admin.Api.Controllers.Security
{
    [Route("api/activities")]
    [ApiController]
    public class ActivityController : BaseController
    {
        private readonly IActivityService _activityService;
        private readonly IAppLogger<ActivityController> _logger;

        public ActivityController(IActivityService service, IAppLogger<ActivityController> logger)
        {
            _activityService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] Model.ActivityInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _activityService.Get(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody] IList<Model.ActivityInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _activityService.Add(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody] IList<Model.ActivityInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _activityService.Modify(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody] IList<Model.ActivityInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _activityService.Delete(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValuesFromToken(ref IList<Model.ActivityInfo> models)
        {
            if (models != null)
                models = models.Select(x =>
                {
                    x.ActionByUser = UserName;
                    x.ModifiedBy = UserName;
                    return x;
                }).ToList();
        }
    }
}