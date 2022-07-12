using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Web.Gateway.Middlleware;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using Ocelot.Middleware;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Evolution.Web.Gateway
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog-file.config").GetCurrentClassLogger();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                { 
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"ocelotConfig.{hostingContext.HostingEnvironment.EnvironmentName}.json", false, false)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(service =>
                {
                    var builder = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    IConfigurationRoot configuration = builder.Build();

                    ServiceRegistration serviceRegistration = new ServiceRegistration(configuration);  
                    service.AddCors(); 
                    serviceRegistration.RegisterDependency(service);
                    serviceRegistration.RegisterOcelotAggregatorAndDeComposer(service);
                    serviceRegistration.RegisterJwt(service);
                    serviceRegistration.AddRequestSize(service);
                     
                    ServicePointManager.DefaultConnectionLimit = int.MaxValue;
                    
                }).Configure(a =>
                {   
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                    {
                        string allowedHosts = Environment.GetEnvironmentVariable("AllowedOrigins");
                        if (!String.IsNullOrEmpty(allowedHosts))
                        {
                            var origins = allowedHosts.Split(";");
                            a.UseCors(x => x.WithOrigins(origins) 
                                      .SetPreflightMaxAge(TimeSpan.FromDays(30))
                                      .AllowAnyMethod() 
                                      .AllowAnyHeader());
                        }
                        else
                            a.UseCors(x => x.AllowAnyOrigin()
                                      .SetPreflightMaxAge(TimeSpan.FromDays(30))
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
                    }
                    else
                    {
                        a.UseCors(x => x.AllowAnyOrigin()
                                      .SetPreflightMaxAge(TimeSpan.FromDays(30))
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
                    }  
                    a.UseMiddleware<RequestHeaderValidationMiddlleware>();
                    a.UseAuthentication().UseMiddleware<TokenValidationMiddlleware>();  
                     
                    //Cyber security issue fix : To Hide google api key in UI
                    var configuration = new OcelotPipelineConfiguration
                    {
                        PreQueryStringBuilderMiddleware = async (ctx, next) =>
                        {
                            if (ctx.DownstreamReRoute.Key == "GoogleMapDirectionService")
                            {
                                string gKey = System.Environment.GetEnvironmentVariable("GoogleApiKey");
                                ctx.HttpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("apikey", gKey));
                            }

                            await next.Invoke();
                        }
                    };
                    a.UseOcelot(configuration).Wait();

                    a.UseExceptionHandler(appError =>
                    {
                        appError.Run(async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";

                            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (contextFeature != null)
                            {
                                Console.WriteLine(contextFeature.Error.ToFullString());
                                //logger.LogError(ResponseType.Exception.ToId(), contextFeature.Error.ToFullString()); 
                                await context.Response.WriteAsync(
                                            JsonConvert.SerializeObject(new Response().ToPopulate(ResponseType.Exception, null, null, null, null, contextFeature.Error)).ToString()
                                    );
                            }
                        });
                    });
                })
            .UseKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 524288000; //500MB(1024 X 1024 X 500) - Request size
            }).UseNLog().Build();
    }
}
