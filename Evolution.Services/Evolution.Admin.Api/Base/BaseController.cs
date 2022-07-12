using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Evolution.Admin.Api.Controllers.Base
{
    public class BaseController : Controller
    {
        private const string APP_NAME = "Evolution2";
        private const string KEY_AUTHORIZATION = "Authorization";
        private const string CLAIM_NAME_UAPP = "uapp";
        private const string CLAIM_NAME_CCODE = "ccode";
        private const string CLAIM_NAME_UTYPE = "utype";
        private const string COMPANY_ID = "company_id";

        public string UserName => GetClaimValue(JwtRegisteredClaimNames.UniqueName.ToString());

        public string UserType => GetClaimValue(CLAIM_NAME_UTYPE);

        public string UserDisplayName => GetClaimValue(JwtRegisteredClaimNames.Sub.ToString());

        public string UserCompanyCode => GetClaimValue(CLAIM_NAME_CCODE);

        public string ApplicationName => GetClaimValue(CLAIM_NAME_UAPP);

        private string GetClaimValue(string claimType)
        {
            string claimValue = string.Empty;
            try
            {
                if (Request.HttpContext.Request.Headers.Keys.Contains(KEY_AUTHORIZATION))
                {
                    string token = Request.HttpContext.Request.Headers[KEY_AUTHORIZATION];
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var decodedToken = jwtHandler.ReadToken(token.Replace("Bearer", "").Replace("BEARER", "").Trim()) as JwtSecurityToken;
                    claimValue = decodedToken?.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
                }
            }
            catch (Exception ex)
            {
                string exception = ex.ToString();
                //TODO : Log error in logger
            }

            if (string.IsNullOrEmpty(claimValue))
            {
                if (claimType == CLAIM_NAME_UAPP)
                    return APP_NAME;
            }

            return claimValue;
        }

        public string ApplicationAudienceCode
        {
            get { return Request.HttpContext.Request.Headers["client_aud_code"].ToString(); } 
        }

        public int CompanyId
        {
            get { return Convert.ToInt16(Request.HttpContext.Request.Headers["company_id"]); }
        }
    }
}