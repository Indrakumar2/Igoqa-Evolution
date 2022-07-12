using AutoMapper;
using Evolution.AuthorizationService.Interfaces;
using Evolution.AuthorizationService.Services;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
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

namespace Evolution.AuthorizationService
{
    /// <summary>
    /// This class will register dependencies.
    /// </summary>
    public class ServiceRegistraion
    {
        private AppEnvVariableBaseModel envVariableBaseModel = null;
        private string sqlConnection = string.Empty;
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_Cors = "Keys:CORS_NAME";

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
                             .AddDbContextPool<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(sqlConnection,
                             sqlServerOptions => sqlServerOptions.CommandTimeout(Convert.ToInt16(envVariableBaseModel.SQLConnectionTimeout))));

            ServiceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //ServiceCollection.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            ServiceCollection.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            ServiceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            ServiceCollection.AddScoped<IValidationService, ValidationService.Services.ValidationService>();
            ServiceCollection.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
            new BootStrappers.BootStrapper().Register(ServiceCollection);
            new Email.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Email.Core.BootStrappers.BootStrapper().Register(ServiceCollection); 
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
                cfg.AddProfile(new Evolution.AuthorizationService.Mappers.DomainMapper());
                cfg.AddProfile(new Evolution.Email.Core.Mappers.DomainMapper());
            });

            //var config = new AutoMapper.MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new Evolution.AuthorizationService.Mappers.DomainMapper());

            //    cfg.SourceMemberNamingConvention = new PascalCaseNamingConvention();
            //    cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            //    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            //    cfg.AllowNullCollections = true;
            //});
            //ServiceCollection.AddSingleton(config.CreateMapper());
        }

        public void RegisterNLog()
        {
            ValidateAppRuntimeValue();
            GlobalDiagnosticsContext.Set("connectionString", sqlConnection);
            LoggerFactory.AddNLog();
            LogManager.Configuration.Variables["microservicename"] = "Evolution.Authorizarion";
        }

        public void RegisterJwt()
        {
            //var jwtSection = Configuration.GetSection("jwt");
            //var jwtOptions = new Models.Tokens.TokenOption();
            //jwtSection.Bind(jwtOptions);
            //ServiceCollection.AddAuthentication()
            //    .AddJwtBearer(cfg =>
            //    {
            //        cfg.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            //            ValidIssuer = jwtOptions.Issuer,
            //            ValidateAudience = false,
            //            ValidateLifetime = true
            //        };
            //    });
            //ServiceCollection.Configure<Models.Tokens.TokenOption> (jwtSection);
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
