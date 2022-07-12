using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Evolution.Common.Enums;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Evolution.Admin.Api.Filters
{
    public class AuthorisationFilter : TypeFilterAttribute
    {
        public AuthorisationFilter(SecurityModule module, params SecurityPermission[] activity) : base(
            typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { module, activity };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private const string KEY_AUTHORIZATION = "Authorization";
        private readonly List<string> _activity;
        private readonly SecurityModule _module;
        private readonly IUserDetailService _userDetailService;

        public AuthorizeActionFilter(SecurityModule module, IUserDetailService userDetailService,
            params SecurityPermission[] activity)
        {
            _module = module;
            _activity = activity.Select(c => c.ToString()).ToList();
            _userDetailService = userDetailService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = UserPermission(context, _module, _activity);
            if (!isAuthorized) context.Result = new ObjectResult("Access denied.") { StatusCode = 403 };
        }

        private bool UserPermission(AuthorizationFilterContext context, SecurityModule module, List<string> activity)
        {
            string userName = string.Empty;
            GetUserName(context, out userName);
            return _userDetailService.GetUserPermission(
                Convert.ToInt16(context.HttpContext.Request.Headers["company_id"]),
                userName, module.ToString(), activity);
        }

        private void GetUserName(AuthorizationFilterContext context, out string userName)
        {
            if (context.HttpContext.Request.Headers.Keys.Contains(KEY_AUTHORIZATION))
            {
                string token = context.HttpContext.Request.Headers[KEY_AUTHORIZATION];
                var jwtHandler = new JwtSecurityTokenHandler();
                var decodedToken =
                    jwtHandler.ReadToken(token.Replace("Bearer", "").Replace("BEARER", "").Trim()) as JwtSecurityToken;
                userName = decodedToken?.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName)
                    ?.Value;
            }
            else
            {
                userName = null;
            }
        }
    }
}
