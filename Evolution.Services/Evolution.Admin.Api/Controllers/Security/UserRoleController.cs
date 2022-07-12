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
    [Route("api/userrole")]
    public class UserRoleController : BaseController
    {
        private readonly IAppLogger<UserRoleController> _logger;
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService, IAppLogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] Model.UserRoleInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _userRoleService.Get(model);
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

        [HttpGet("{userLogonName}")]
        public Response Get(string userLogonName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _userRoleService.Get(new Model.UserRoleInfo {UserLogonName = userLogonName});
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userLogonName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet("{applicationName}/{userLogonName}")]
        public Response Get(string applicationName, string userLogonName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _userRoleService.Get(new Model.UserRoleInfo
                    {ApplicationName = applicationName, UserLogonName = userLogonName});
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), applicationName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody] IList<Model.UserRoleInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null; // Manage Security Audit changes
                AssignValuesFromToken(ref model);
                return _userRoleService.Add(model, ref eventId); // Manage Security Audit changes
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
        public Response Delete([FromBody] IList<Model.UserRoleInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null; // Manage Security Audit changes
                AssignValuesFromToken(ref model);
                return _userRoleService.Delete(model, ref eventId); // Manage Security Audit changes
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

        private void AssignValuesFromToken(ref IList<Model.UserRoleInfo> models)
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