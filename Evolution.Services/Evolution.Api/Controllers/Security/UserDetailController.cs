using Evolution.Api.Controllers.Base;
using Evolution.Api.Filters;
using Evolution.Api.Models;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
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
    [Route("api/userdetails")]
    public class UserDetailController : BaseController
    {
        private readonly IUserDetailService _userDetailService = null;
        private readonly IAppLogger<UserDetailController> _logger = null;
        private readonly Newtonsoft.Json.Linq.JObject _messages = null;

        public UserDetailController(IUserDetailService service,
            IAppLogger<UserDetailController> logger,
            Newtonsoft.Json.Linq.JObject messages )
        {
            _userDetailService = service;
            _logger = logger;
            _messages = messages;
        }

        //[AuthorisationFilter(SecurityModule.Security, SecurityPermission.V00001)]
        [HttpGet]
        public Response Get([FromQuery] UserInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _userDetailService.Get(model);
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
        [Route("rolecompany")]
        public Response GetUserRoleCompany([FromBody] IList<string> userLogonNames)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var appUsers = new List<KeyValuePair<string, string>>();
                userLogonNames?.ToList().ForEach(x =>
                {
                    appUsers.Add(new KeyValuePair<string, string>(ApplicationName, x));
                });
                return _userDetailService.GetUserRoleCompany(appUsers);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userLogonNames);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("{userSamaName}/menulist")]
        public Response GetUserMenu([FromRoute] string userSamaName, [FromQuery] string compCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _userDetailService.GetUserMenu(ApplicationName, userSamaName, compCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userSamaName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("{userSamaName}/usertypelist")]
        public Response GetUserType([FromRoute] string userSamaName, [FromQuery] string compCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _userDetailService.GetUserType(userSamaName, compCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userSamaName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("{userSamaName}/permission")]
        public Response GetUserPermission([FromRoute] string userSamaName, [FromQuery] string module, [FromQuery] int companyId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                userSamaName = string.IsNullOrEmpty(userSamaName) ? UserName : userSamaName;
                return _userDetailService.GetUserPermission(ApplicationName, userSamaName, companyId, module);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userSamaName);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [AuthorisationFilter(SecurityModule.Security, SecurityPermission.N00001)]
        [HttpPost]
        public Response Post([FromBody]IList<UserDetail> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            { 
                if (model != null)
                { 
                    model = model.Where(x => x.User != null)
                                    .Select(x => { x.User.ApplicationName = ApplicationName; return x; }).ToList();

                    AssignValuesFromToken(ref model);
                    return _userDetailService.Add(model);
                }
                else
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.Security, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);

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
        [Route("status")]
        public Response Put([FromBody]UserStatusModel statusModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (statusModel == null)
                {
                    statusModel = new UserStatusModel();
                }

                string userSamaAccName = statusModel.UserSamaAccountName;
                bool newStatus = statusModel.NewStatus;
                return _userDetailService.ChangeUserStatus("Evolution2", userSamaAccName, newStatus);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), statusModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [AuthorisationFilter(SecurityModule.Security, SecurityPermission.M00001)]
        [HttpPut]
        public Response Put([FromBody]IList<UserDetail> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               
                if (model != null)
                { 
                    model = model.Where(x => x.User != null)
                                    .Select(x => { x.User.ApplicationName = ApplicationName; return x; }).ToList();

                    AssignValuesFromToken(ref model);
                    return _userDetailService.Modify(model);
                }
                else
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.Security, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);

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

        [AuthorisationFilter(SecurityModule.Security, SecurityPermission.D00001)]
        [HttpDelete]
        public Response Delete([FromBody]IList<string> userNames)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var userInfos = userNames?.Select(x => new UserInfo()
                {
                    ApplicationName = ApplicationName,
                    LogonName = x,
                    RecordStatus = RecordStatus.Delete.FirstChar()
                }).ToList();
                return _userDetailService.Delete(userInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userNames);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValuesFromToken(ref IList<UserDetail> models)
        {
            if (models != null)
            {
                models = models.Select(x =>
                              {
                                  if (x.User != null)
                                  {
                                      x.User.ModifiedBy = UserName;
                                      x.User.ActionByUser = UserName;
                                  }

                                  if (x.CompanyRoles != null && x.CompanyRoles.Count > 0)
                                  {
                                      x.CompanyRoles = x.CompanyRoles.Select(x1 =>
                                      {
                                          if (x1.Roles != null && x1.Roles.Count > 0)
                                          {
                                              x1.Roles = x1.Roles.Select(x2 =>
                                              {
                                                  x2.ModifiedBy = UserName;
                                                  x2.ActionByUser = UserName;
                                                  return x2;
                                              }).ToList();
                                          }
                                          return x1;
                                      }).ToList();
                                  }
                                  if (x.CompanyUserTypes != null && x.CompanyUserTypes.Count > 0)
                                  {
                                      x.CompanyUserTypes = x.CompanyUserTypes.Select(x1 =>
                                      {
                                          if (x1.UserTypes != null && x1.UserTypes.Count > 0)
                                          {
                                              x1.UserTypes = x1.UserTypes.Select(x2 =>
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
}
