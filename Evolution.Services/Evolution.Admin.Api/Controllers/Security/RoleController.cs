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
    [Route("api/roles")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IAppLogger<RoleController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(IRoleService service, IAppLogger<RoleController> logger)
        {
            _roleService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] Model.RoleInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _roleService.Get(model);
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
        public Response Post([FromBody] IList<Model.RoleInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null; // Manage Security Audit changes
                AssignValuesFromToken(ref model);
                return _roleService.Add(model, ref eventId); // Manage Security Audit changes
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
        public Response Put([FromBody] IList<Model.RoleInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null;
                AssignValuesFromToken(ref model);
                return _roleService.Modify(model, ref eventId);
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
        public Response Delete([FromBody] IList<Model.RoleInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _roleService.Delete(model);
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

        private void AssignValuesFromToken(ref IList<Model.RoleInfo> models)
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