using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Evolution.Web.Gateway.Aggregators;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using System;
using System.Text;

namespace Evolution.Web.Gateway
{
    public class ServiceRegistration
    {
        private const string Key_AuthName = "Evo2Api_Auth";
        private const string Key_JWT = "JWT";
        private const string Key_Secret = "Secret";
        private const string Key_Issue = "Iss";
        private const string Key_Audinece = "Aud";

        private AppEnvVariableBaseModel envVariableBaseModel = null;
        private string sqlConnection = string.Empty;

        public ServiceRegistration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void RegisterJwt(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection(Key_JWT);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig[Key_Secret]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig[Key_Issue],
                ValidateAudience = true,
                ValidAudience = audienceConfig[Key_Audinece],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = Key_AuthName;// JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = Key_AuthName;// JwtBearerDefaults.AuthenticationScheme;
                //o.DefaultAuthenticateScheme = Key_AuthName;
            })
            .AddJwtBearer(Key_AuthName, x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
                //x.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                //{
                //    OnAuthenticationFailed = async ctx =>
                //    {
                //        //for debug pupose only
                //        int i = 0;
                //    },
                //    OnTokenValidated = async ctx =>
                //    {
                //        //for debug pupose only
                //        int i = 0;
                //    },

                //    OnMessageReceived = async ctx =>
                //    {
                //        //for debug pupose only
                //        int i = 0;
                //    }
                //};
            });

            //services.AddOcelot(Configuration);
        }

        public void RegisterDependency(IServiceCollection services)
        {
            ValidateAppRuntimeValue(services);

            services.AddEntityFrameworkSqlServer().AddDbContextPool<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(sqlConnection,
                             sqlServerOptions => sqlServerOptions.CommandTimeout(Convert.ToInt16(envVariableBaseModel.SQLConnectionTimeout))));

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            new Evolution.Security.Core.BootStrappers.BootStrapper().Register(services);
            new Evolution.Security.Infrastructure.BootStrappers.BootStrapper().Register(services);
        }

        /// <summary>
        /// To mention request size 
        /// </summary>
        /// <param name="services"></param>
        public void AddRequestSize(IServiceCollection services)
        {
            // Added this section to support max file size upload in IIS.  -->
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        public void RegisterOcelotAggregatorAndDeComposer(IServiceCollection services)
        {
            services.AddOcelot()
                    .AddTransientDefinedAggregator<CustomerAggregator>()
                    .AddTransientDefinedAggregator<DashboardCountAggregator>()
                    .AddTransientDefinedAggregator<CompanyAggreator>()
                    .AddTransientDefinedAggregator<ContractAggregator>()
                    .AddTransientDefinedAggregator<ProjectAggregator>()
                    .AddTransientDefinedAggregator<SupplierAggregator>()
                    .AddTransientDefinedAggregator<SupplierPOAggregator>()
                    .AddTransientDefinedAggregator<TechnicalSpecialistAggregator>()
                    .AddTransientDefinedAggregator<AssignmentAggregator>()
                    .AddTransientDefinedAggregator<TimesheetAggregator>()
                    .AddTransientDefinedAggregator<VisitAggregator>();
        }

        private void ValidateAppRuntimeValue(IServiceCollection services)
        {
            envVariableBaseModel = new AppEnvVariableBaseModel
            {
                SQLDatabase = Environment.GetEnvironmentVariable("SQLDatabase"),
                SQLServer = Environment.GetEnvironmentVariable("SQLServer"),
                SQLUser = Environment.GetEnvironmentVariable("SQLUser"),
                SQLPassword = Environment.GetEnvironmentVariable("SQLPassword"),
                SQLMaxPoolSize = Environment.GetEnvironmentVariable("SQLMaxPoolSize"),
                SQLConnectionTimeout = Environment.GetEnvironmentVariable("SQLConnectionTimeout")
            };

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
            else if (string.IsNullOrEmpty(envVariableBaseModel.SQLConnectionTimeout))
                messages = "Required Runtime Parameter (SQLUser) Has Not Passed.";
            else if (string.IsNullOrEmpty(envVariableBaseModel.SQLMaxPoolSize))
                messages = "Required Runtime Parameter (SQLUser) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            sqlConnection = string.Format(Configuration.GetConnectionString("SQLDbConnection"),
                                            envVariableBaseModel.SQLServer,
                                            envVariableBaseModel.SQLDatabase,
                                            envVariableBaseModel.SQLUser,
                                            envVariableBaseModel.SQLPassword,
                                            envVariableBaseModel.SQLConnectionTimeout,
                                            envVariableBaseModel.SQLMaxPoolSize);

        }
    }
}
