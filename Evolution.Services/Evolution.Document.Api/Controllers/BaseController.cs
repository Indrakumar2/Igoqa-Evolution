using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Evolution.Document.Api.Controllers
{
    public class BaseController : Controller
    {
        public string UserName => GetClaimValue(JwtRegisteredClaimNames.UniqueName.ToString());

        public string UserDisplayName => GetClaimValue(JwtRegisteredClaimNames.Sub.ToString());

        public string UserCompanyCode => GetClaimValue("ccode");

        private string GetClaimValue(string claimType)
        {
            string claimValue = string.Empty;
            string authorizationKey = "Authorization";
            try
            {
                if (Request.HttpContext.Request.Headers.Keys.Contains(authorizationKey))
                {
                    string token = Request.HttpContext.Request.Headers[authorizationKey];

                    var jwtHandler = new JwtSecurityTokenHandler();
                    var decodedToken = jwtHandler.ReadToken(token.Replace("Bearer", "").Replace("BEARER", "").Trim()) as JwtSecurityToken;

                    claimValue = decodedToken?.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return claimValue;
        }
    }
}
