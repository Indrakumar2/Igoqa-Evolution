using AutoMapper;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Models;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Evolution.ValidationService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using System; 

namespace Evolution.Admin.Api
{
    public class ServiceRegistraion
    {
        private AppEnvVariableBaseModel envVariableBaseModel = null;
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_Cors = "Keys:CORS_NAME";
        private string sqlConnection = string.Empty;

        public ServiceRegistraion(IConfiguration configuration,
                                  ILoggerFactory loggerFactory,
                                  IServiceCollection serviceCollection)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            ServiceCollection = serviceCollection;
        }

        public IConfiguration Configuration { get; }

        public ILoggerFactory LoggerFactory { get; }

        public IServiceCollection ServiceCollection { get; }

        public void RegisterDependencies()
        {
            ValidateAppRuntimeValue();

            ServiceCollection.AddEntityFrameworkSqlServer()
                             .AddDbContextPool<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(sqlConnection,
                             sqlServerOptions => sqlServerOptions.CommandTimeout(Convert.ToInt16(envVariableBaseModel.SQLConnectionTimeout)))); 

            ServiceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            ServiceCollection.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            ServiceCollection.AddScoped<IValidationService, Evolution.ValidationService.Services.ValidationService>(); 

            //Registering Mongo Setting
            ServiceCollection.Configure<MongoSetting>(x =>
            {
                x.ConnectionString = string.Format("mongodb://{0}:{1}", envVariableBaseModel.MongoDbIp, envVariableBaseModel.MongoDbPort);
                x.DatabaseName = envVariableBaseModel.MongoDbName;
                x.DocumentTypes = envVariableBaseModel.MongoSyncTypes;
            });
             

            DbRepositoryServiceRegistration();
            CompanyServiceRegistration();
            DocumentServiceRegistration();
            SecurityServiceRegistration();
            MasterServiceRegistration(); 
            AdminServiceRegistration(); 
            AuditLogServiceRegistration(); 
        }

        public void RegisterCors()
        {
            ServiceCollection.AddCors(options =>
            {
                options.AddPolicy(Configuration[Key_Cors],
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        public void RegisterHttpContext()
        { 
            ServiceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void RegisterSecurityContext(HttpContext context)
        {
            if (!context.Response.Headers.ContainsKey("Strict-Transport-Security"))
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000, includeSubDomains");
            if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
                //context.Response.Headers.Add("Content-Security-Policy", "default-src https: 'unsafe-eval' 'unsafe-inline'; object-src 'none");
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
            if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            if (!context.Response.Headers.ContainsKey("Referrer-Policy"))
                context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade");
            if (!context.Response.Headers.ContainsKey("Permissions-Policy"))
                context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
        }
         
        public void RegisterAutoMapperPofile()
        {

            Mapper.Initialize(cfg =>
            { 
                cfg.AddProfile(new Core.Mappers.DomainMapper());
                cfg.AddProfile(new Master.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Security.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Company.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Document.Core.Mappers.DomainMapper());
            });
        }

        public void RegisterNLog(ILoggerFactory loggerFactory)
        {
            ValidateAppRuntimeValue();

            GlobalDiagnosticsContext.Set("connectionString", sqlConnection);

            // Uncomment this line if file logging is required
            //GlobalDiagnosticsContext.Set("configDir", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            loggerFactory.AddNLog();

            LogManager.Configuration.Variables["microservicename"] = "Evolution.Admin.API";
        }

        private void DbRepositoryServiceRegistration()
        {
            new DbRepository.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void AuditLogServiceRegistration()
        {
            var sp = ServiceCollection.BuildServiceProvider();
            var auditDbContext = sp.GetService<EvolutionSqlDbContext>();
            var mapper = sp.GetService<IMapper>();

            new AuditLog.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new AuditLog.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection,auditDbContext,mapper);
        }

        private void MasterServiceRegistration()
        {
            new Master.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Master.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }
          

        private void AdminServiceRegistration()
        {
            new Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void DocumentServiceRegistration()
        {
            new Document.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Document.Core.BootStrappers.BootStrapper().RegisterMongoDocumentService(ServiceCollection);
            new Document.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);
        }
        private void CompanyServiceRegistration()
        {
            new Company.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Company.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void SecurityServiceRegistration()
        {
            new Security.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Security.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void ValidateAppRuntimeValue()
        {
            //Reading Environment Variable and validating either required runtime params has been passed or not.
            envVariableBaseModel = ServiceCollection.BuildServiceProvider()
                                  .GetService<IOptions<AppEnvVariableBaseModel>>()?.Value;

            string messages = string.Empty;

            if (envVariableBaseModel == null)
                messages = "Required Runtime Parameter Has Not Passed Like SQL Connection.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.SQLDatabase))
                messages = "Required Runtime Parameter (SQLDatabase) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.SQLPassword))
                messages = "Required Runtime Parameter (SQLPassword) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.SQLServer))
                messages = "Required Runtime Parameter (SQLServer) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.SQLUser))
                messages = "Required Runtime Parameter (SQLUser) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.SecurityAppName))
                messages = "Required Runtime Parameter (SecurityAppName) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            sqlConnection = string.Format(Configuration.GetConnectionString(Key_SqlConnection),
                                            envVariableBaseModel.SQLServer,
                                            envVariableBaseModel.SQLDatabase,
                                            envVariableBaseModel.SQLUser,
                                            envVariableBaseModel.SQLPassword, 
                                            envVariableBaseModel.SQLConnectionTimeout,
                                            envVariableBaseModel.SQLMaxPoolSize);
        }
         
    }
}