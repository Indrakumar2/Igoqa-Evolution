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

namespace Evolution.Admin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; } 

        public IServiceCollection ServiceCollection { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Registering Environment Variable.
            services.Configure<AppEnvVariableBaseModel>(Configuration);
            services.Configure<ADConfiguration>(Configuration);

            ServiceCollection = services;
            var registration = new ServiceRegistraion(Configuration, null, ServiceCollection);

            registration.RegisterCors(); //CORS policy registration 

            registration.RegisterHttpContext();  //Register to access session from non controllers

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            registration.RegisterAutoMapperPofile(); //Automapper Registration

            services.AddAutoMapper();

            registration.RegisterDependencies();

            services.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));

            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {

            var registration = new ServiceRegistraion(Configuration, loggerFactory, ServiceCollection);

            registration.RegisterNLog(loggerFactory); //Nlog Registration

            app.UseCors(Configuration["Keys:CORS_NAME"]);

            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage(); 
            else 
                app.UseExceptionHandler("/error"); 
             
            app.UseMvc(); 

            app.Use(async (context, next) =>
            {
                registration.RegisterSecurityContext(context);
                await next();
            });
        }
    }
}
