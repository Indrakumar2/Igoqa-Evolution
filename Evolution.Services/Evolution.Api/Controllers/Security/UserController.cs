using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = Evolution.Security.Domain.Models.Security;

namespace Evolution.Api.Controllers.Security
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService = null;
        private readonly IAppLogger<UserController> _logger = null;

        public UserController(IUserService service, IAppLogger<UserController> logger)
        {
            this._userService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]Model.UserInfo model, [FromQuery] string[] excludeUserTypes, [FromQuery] bool isGetAllUser = false)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._userService.Get(model, excludeUserTypes, isGetAllUser);
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

        [Route("BasicInfo")]
        [HttpGet]
        public Response Get([FromQuery] string companyCode, [FromQuery] string userType, [FromQuery] bool isActive = true)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._userService.GetUser(companyCode, userType, isActive);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { companyCode, userType, isActive });
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        /* Note- This method is used for getting Assignment with View All Assignment Role */
        [HttpGet]
        [Route("GetViewAllAssignments")]
        public Response GetViewAllAssignments([FromQuery]string samAccountName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._userService.GetViewAllAssignments(samAccountName);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), samAccountName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]IList<Model.UserInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null; // Manage Security Audit changes
                AssignValuesFromToken(ref model);
                return _userService.Add(model, ref eventId); // Manage Security Audit changes
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
        public Response Put([FromBody]IList<Model.UserInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null; // Manage Security Audit changes
                AssignValuesFromToken(ref model);
                return _userService.Modify(model, ref eventId); // Manage Security Audit changes
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
        public Response Delete([FromBody]IList<Model.UserInfo> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                return _userService.Delete(model);
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


        [HttpGet]
        [Route("{userName}/usercompanyroles/{menuname}")]
        public Response Get([FromRoute]string userName, [FromRoute] string menuname)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._userService.GetUserCompanyRoles(userName, menuname);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValuesFromToken(ref IList<Model.UserInfo> models)
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
