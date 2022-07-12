using AutoMapper;
using Evolution.Common.Models.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Reflection;

namespace Evolution.AuthorizationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceCollection ServiceCollection { get; set; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Registering Environment Variable.
            services.Configure<AppEnvVariableBaseModel>(Configuration);
            services.Configure<ADConfiguration>(Configuration);

            ServiceCollection = services;

            var registration = new ServiceRegistraion(Configuration, null, ServiceCollection);
             
            registration.RegisterCors(); //CORS policy registration

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
             
            registration.RegisterDependencies(); //Dependency Service Registration
             
            registration.RegisterAutoMapperPofile(); //Automapper Registration
            ServiceCollection.AddAutoMapper();
             
            registration.RegisterJwt(); //Jwt Registration

            services.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));

            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            var registration = new ServiceRegistraion(Configuration, loggerFactory, ServiceCollection);
            registration.RegisterNLog();

            app.UseCors(Configuration["Keys:CORS_NAME"]);
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage(); 
            else 
                app.UseExceptionHandler("/error");  
             
            app.UseMvc(); 
            
        }
    }
}