using AutoMapper;
using Evolution.Common.Models.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Evolution.Document.Api
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
            //Registering Environment Variable.
            services.Configure<AppEnvVariableBaseModel>(Configuration);

            ServiceCollection = services;

            var registration = new ServiceRegistraion(Configuration, null, ServiceCollection);

            //CORS policy registration
            registration.RegisterCors();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Added this section to support max file size upload in IIS.  -->
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            registration.RegisterAutoMapperPofile();

            registration.RegisterDependencies();

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var registration = new ServiceRegistraion(Configuration, loggerFactory, ServiceCollection);

            //Nlog Registration
            registration.RegisterNLog(loggerFactory);

            app.UseCors(Configuration["Keys:CORS_NAME"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
