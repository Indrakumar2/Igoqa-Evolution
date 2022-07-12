using AutoMapper;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Documents;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Models;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace Evolution.Document.Api
{
    public class ServiceRegistraion
    {
        private AppEnvVariableBaseModel envVariableBaseModel = null;
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_DocumentUpload = "DocumentUpload";
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

            //ServiceCollection.AddEntityFrameworkSqlServer()
            //                 .AddDbContext<EvolutionSqlDbContext>(options => options.UseSqlServer(sqlConnection));

            ServiceCollection.AddEntityFrameworkSqlServer()
                            .AddDbContext<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(sqlConnection));
            //Registering Mongo Setting
            ServiceCollection.Configure<MongoSetting>(x =>
            {
                x.ConnectionString = string.Format("mongodb://{0}:{1}", envVariableBaseModel.MongoDbIp, envVariableBaseModel.MongoDbPort);
                x.DatabaseName = envVariableBaseModel.MongoDbName;
                x.DocumentTypes = envVariableBaseModel.MongoSyncTypes;
            });

            //Registering Document Upload Setting
            ServiceCollection.Configure<DocumentUploadSetting>(Configuration.GetSection(Key_DocumentUpload));

            ServiceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            ServiceCollection.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            this.DocumentServiceRegistration();

            ServiceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));
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
         
        public void RegisterAutoMapperPofile()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Evolution.Document.Core.Mappers.DomainMapper());
            });
        }

        public void RegisterNLog(ILoggerFactory loggerFactory)
        {
            ValidateAppRuntimeValue();

            GlobalDiagnosticsContext.Set("connectionString", sqlConnection);
            
            // Uncomment this line if file logging is required
            //GlobalDiagnosticsContext.Set("configDir", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            loggerFactory.AddNLog();

            LogManager.Configuration.Variables["microservicename"] = "Evolution.Document.Api";
        }

        private void DbRepositoryServiceRegistration()
        {
            new Evolution.DbRepository.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void DocumentServiceRegistration()
        {
            new Evolution.Document.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            
            new Evolution.Document.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Evolution.Admin.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Evolution.Admin.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Evolution.TechnicalSpecialist.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Evolution.TechnicalSpecialist.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Company.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Company.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Evolution.DbRepository.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Evolution.DbRepository.BootStrappers.BootStrapper().RegisterDataService(ServiceCollection);
            new Master.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Master.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Home.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Home.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Security.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Security.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new AuditLog.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new AuditLog.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Email.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Email.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Visit.Core.BootStrappers.BootStrapper().Register(ServiceCollection);

            new Visit.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
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
            else if (string.IsNullOrEmpty(envVariableBaseModel.MongoDbIp))
                messages = "Required Runtime Parameter (MongoDbIp) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.MongoDbPort))
                messages = "Required Runtime Parameter (MongoDbPort) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.MongoDbName))
                messages = "Required Runtime Parameter (MongoDbName) Has Not Passed.";

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