using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Web.Gateway.Middlleware
{
    public class TokenValidationMiddlleware
    {
        private readonly RequestDelegate _next; 

        public TokenValidationMiddlleware(RequestDelegate next)
        {
            _next = next; 
        }
        public async Task Invoke(HttpContext context, IRefreshTokenService refreshTokenService, IConfiguration configuration, IAppLogger<TokenValidationMiddlleware> logger)
        {
            string authorizationHeader = string.Empty;
            try
            { 
                string[] excludePath = configuration.GetSection("TokenValidationExcludedAPI").Get<string[]>();

                if (excludePath.Contains(context.Request.Path.Value))
                { 
                    await _next(context);
                    return;
                }

                JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler(); 
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    await Unauthorized(context);
                    return;
                }

                authorizationHeader = context.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                {
                    await Unauthorized(context);
                    return;
                }

                JwtSecurityToken decodedToken = jwtHandler.ReadToken(authorizationHeader.Substring("bearer".Length).Trim()) as JwtSecurityToken;
                string username = decodedToken?.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName.ToString())?.Value;

                if (string.IsNullOrEmpty(decodedToken?.RawData))
                {
                    await Unauthorized(context);
                    return;
                }
                if (!refreshTokenService.isValidToken(decodedToken?.RawData, username))
                {
                    //Console.WriteLine("Detected unauthorized access using invalid token.[ InvalidToken : " + authorizationHeader + "]");
                    logger.LogError(ResponseType.Exception.ToId(), "Detected unauthorized access using invalid token.", new { InvalidToken = authorizationHeader });
                    await Unauthorized(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { authorizationHeader });
               // Console.WriteLine("error :[" + ex.ToFullString() + "] [ Token : " + authorizationHeader + " ]");
                await Unauthorized(context);
                return;
            }
            //pass request further if correct
            await _next(context);
        }

        private async Task Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
