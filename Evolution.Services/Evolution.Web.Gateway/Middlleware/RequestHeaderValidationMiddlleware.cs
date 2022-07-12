using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Web.Gateway.Middlleware
{
    public class RequestHeaderValidationMiddlleware
    {
        private readonly RequestDelegate _next; 

        public RequestHeaderValidationMiddlleware(RequestDelegate next) 
        {
            _next = next; 
        }
        public async Task Invoke(HttpContext context, IConfiguration configuration, IAppLogger<RequestHeaderValidationMiddlleware> logger)
        { 
            try
            { 
                string[] mandatoryRequestHeaders=configuration.GetSection("MandatoryRequestHeaders").Get<string[]>();

                string[] excludePath = configuration.GetSection("MandatoryRequestHeadersValidationExcludedAPI").Get<string[]>();

                if (excludePath.Contains(context.Request.Path.Value))
                {
                    await _next(context);
                    return;
                }
                foreach (string requiredHeader in mandatoryRequestHeaders)
                { 
                    if (!context.Request.Headers.ContainsKey(requiredHeader) || string.IsNullOrEmpty(context.Request.Headers[requiredHeader]))
                    {
                        await Forbidden(context);
                        return;
                    }
                    
                    if (requiredHeader == "Origin")
                    {
                      var allowedOrigins= Environment.GetEnvironmentVariable("AllowedOrigins").Split(";");

                        if (!allowedOrigins.Contains(context.Request.Headers[requiredHeader].ToString()))
                        {
                            await Forbidden(context);
                            return;
                        } 
                    }
                }
                   
            }
            catch (Exception ex)
            {
                logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                //Console.WriteLine("error :[" + ex.ToFullString() + "]");
                await Forbidden(context);
                return;
            }

            await _next(context);
        }

        private async Task Forbidden(HttpContext context)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("forbidden");
        }

    }
}
