using System;
using System.Collections.Generic;
using System.Linq;
using Evolution.Admin.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Security
{
    [Route("api/moduleactivities")]
    public class ModuleDetailController : BaseController
    {
        private readonly IAppLogger<ModuleDetailController> _logger;
        private readonly IModuleDetailService _moduleDetailService;

        public ModuleDetailController(IModuleDetailService service, IAppLogger<ModuleDetailController> logger)
        {
            _moduleDetailService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] ModuleInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _moduleDetailService.Get(model);
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
        public Response Post([FromBody] IList<ModuleDetail> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _moduleDetailService.Add(model);
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
        public Response Put([FromBody] IList<ModuleDetail> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _moduleDetailService.Modify(model);
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
        public Response Delete([FromBody] IList<ModuleDetail> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _moduleDetailService.Delete(model);
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

        private void AssignValuesFromToken(ref IList<ModuleDetail> models)
        {
            if (models != null)
                models = models.Select(x =>
                {
                    if (x.Module != null)
                    {
                        x.Module.ModifiedBy = UserName;
                        x.Module.ActionByUser = UserName;
                    }

                    if (x.Activities != null && x.Activities.Count > 0)
                        x.Activities = x.Activities.Select(x1 =>
                        {
                            x1.ModifiedBy = UserName;
                            x1.ActionByUser = UserName;
                            return x1;
                        }).ToList();
                    return x;
                }).ToList();
        }
    }
}