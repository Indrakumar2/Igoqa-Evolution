using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Api.Controllers.Security
{
    [Route("api/roleactivities")]
    public class RoleDetailController : BaseController
    {
        private readonly IRoleDetailService _moduleDetailService = null;
        private readonly IAppLogger<RoleDetailController> _logger = null;
        public RoleDetailController(IRoleDetailService service, IAppLogger<RoleDetailController> logger)
        {
            this._moduleDetailService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] RoleInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._moduleDetailService.Get(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]IList<RoleDetail> model)
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
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]IList<RoleDetail> model)
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
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]IList<RoleDetail> model)
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
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValuesFromToken(ref IList<RoleDetail> models)
        {
            if (models != null)
                models = models.Select(x =>
                  {
                      if (x.Role != null)
                      {
                          x.Role.ModifiedBy = UserName;
                          x.Role.ActionByUser = UserName;
                      }

                      if (x.Modules != null && x.Modules.Count > 0)
                      {
                          x.Modules = x.Modules.Select(x1 =>
                     {
                             if (x1.Module != null)
                             {
                                 x1.Module.ModifiedBy = UserName;
                                 x1.Module.ActionByUser = UserName;
                             }

                             if (x1.Activities != null && x1.Activities.Count > 0)
                             {
                                 x1.Activities=x1.Activities.Select(x2 =>
                                {
                                    x2.ModifiedBy = UserName;
                                    x2.ActionByUser = UserName;
                                    return x2;
                                }).ToList();
                             }
                             return x1;
                         }).ToList();
                      }
                      return x;
                  }).ToList();
        }
    }
}
