using AutoMapper;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Evolution.ValidationService.Interfaces;
using Google.Maps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Evolution.Api
{
    public class ServiceRegistration
    {
        private AppEnvVariableBaseModel _envVariableBaseModel = null;
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_Cors = "Keys:CORS_NAME";
        private string _sqlConnection = string.Empty;

        public ServiceRegistration(IConfiguration configuration,
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
                             .AddDbContextPool<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(_sqlConnection,
                             sqlServerOptions => sqlServerOptions.CommandTimeout(Convert.ToInt16(_envVariableBaseModel.SQLConnectionTimeout))));
            //.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)));
            //.AddDbContext<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(sqlConnection));

            ServiceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            ServiceCollection.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            ServiceCollection.AddScoped<IValidationService, Evolution.ValidationService.Services.ValidationService>();

            //Registering Mongodb Setting
            ServiceCollection.Configure<Document.Domain.Models.MongoSetting>(x =>
            {
                x.ConnectionString = $"mongodb://{_envVariableBaseModel.MongoDbIp}:{_envVariableBaseModel.MongoDbPort}";
                x.DatabaseName = _envVariableBaseModel.MongoDbName;
                x.DocumentTypes = _envVariableBaseModel.MongoSyncTypes;
            });

            SetupGoogleService();

            DbRepositoryServiceRegistration();
            SecurityServiceRegistration();
            MasterServiceRegistration();
            CompanyServiceRegistration();
            CustomerServiceRegistration();
            ContractServiceRegistration();
            ProjectServiceRegistration();
            SupplierRegistration();
            SupplierPORegistration();
            AssignmentServiceRegistration();
            VisitServiceRegistration();
            TimesheetServiceRegistration();
            DocumentServiceRegistration();
            TechSpecialistServiceRegistration();
            DraftServiceRegistration();
            EmailServiceRegistration();
            AdminServiceRegistration();
            HomeServiceRegistration();
            ResourceSearchServiceRegistration();
            GoogleServiceRegistration();
            AuditLogServiceRegistration();
            NumberSequenceServiceRegistration();
            ReportServiceRegistration();
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
            // ServiceCollection.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            ServiceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void RegisterSecurityContext(HttpContext context)
        {
            if (!context.Response.Headers.ContainsKey("Strict-Transport-Security"))
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000, includeSubDomains");
            if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
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

        public void RegisterSwaggerDoc()
        {
            ServiceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Swagger:info:version"].ToString(),
                    new Info
                    {
                        Version = Configuration["Swagger:info:version"].ToString(),
                        Title = Configuration["Swagger:info:title"].ToString(),
                        Description = Configuration["Swagger:info:description"].ToString(),
                        TermsOfService = Configuration["Swagger:info:termsofService"].ToString(),
                        Contact = new Contact
                        {
                            Name = Configuration["Swagger:info:contact:name"].ToString(),
                            Email = Configuration["Swagger:info:contact:email"].ToString()
                        }
                    });
                c.OrderActionsBy(description => description.HttpMethod);
            });
        }

        public void RegisterAutoMapperProfile()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Assignment.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Admin.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Master.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Company.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Customer.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Contract.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Project.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Supplier.Core.Mappers.DomainMapper());
                cfg.AddProfile(new SupplierPO.Core.Mappers.DomainMapper());
                cfg.AddProfile(new TechnicalSpecialist.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Document.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Visit.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Timesheet.Core.Mapper.DomainMapper());
                cfg.AddProfile(new Security.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Home.Core.Mappers.DomainMapper());
                cfg.AddProfile(new AuditLog.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Draft.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Email.Core.Mappers.DomainMapper());
                cfg.AddProfile(new ResourceSearch.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Reports.Core.Mappers.DomainMapper());
            });
        }

        public void RegisterNLog(ILoggerFactory loggerFactory)
        {
            ValidateAppRuntimeValue();

            GlobalDiagnosticsContext.Set("connectionString", _sqlConnection);

            // Uncomment this line if file logging is required
            //GlobalDiagnosticsContext.Set("configDir", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            loggerFactory.AddNLog();

            LogManager.Configuration.Variables["microservicename"] = "Evolution.API";
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
            new AuditLog.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection, auditDbContext, mapper);
        }

        private void MasterServiceRegistration()
        {
            new Master.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Master.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void CompanyServiceRegistration()
        {
            new Company.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Company.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void CustomerServiceRegistration()
        {
            new Customer.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Customer.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void ContractServiceRegistration()
        {
            new Contract.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Contract.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void ProjectServiceRegistration()
        {
            new Project.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Project.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void SupplierRegistration()
        {
            new Supplier.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Supplier.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void SupplierPORegistration()
        {
            new SupplierPO.Core.BootStrappers.Bootstrapper().Register(ServiceCollection);
            new SupplierPO.Infrastructure.BootStrappers.Bootstrapper().Register(ServiceCollection);
        }

        private void AssignmentServiceRegistration()
        {
            new Assignment.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Assignment.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void NumberSequenceServiceRegistration()
        {
            new NumberSequence.InfraStructure.BootStrapper.BootStrapper().Register(ServiceCollection);
        }

        private void ReportServiceRegistration()
        {
            new Reports.Core.BootStrapper.BootStrapper().Register(ServiceCollection);
            new Reports.Infrastructure.BootStrapper.BootStrapper().Register(ServiceCollection);
        }


        private void TimesheetServiceRegistration()
        {
            new Timesheet.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Timesheet.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void VisitServiceRegistration()
        {
            new Visit.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Visit.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void DocumentServiceRegistration()
        {
            new Document.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Document.Core.BootStrappers.BootStrapper().RegisterMongoDocumentService(ServiceCollection);
            new Document.Infrastructe.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void GoogleServiceRegistration()
        {
            new Google.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void TechSpecialistServiceRegistration()
        {
            new TechnicalSpecialist.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new TechnicalSpecialist.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void DraftServiceRegistration()
        {
            new Draft.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Draft.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void EmailServiceRegistration()
        {
            new Email.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Email.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void AdminServiceRegistration()
        {
            new Admin.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Admin.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void HomeServiceRegistration()
        {
            new Home.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Home.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void ResourceSearchServiceRegistration()
        {
            new ResourceSearch.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new ResourceSearch.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void SecurityServiceRegistration()
        {
            new Security.Core.BootStrappers.BootStrapper().Register(ServiceCollection);
            new Security.Infrastructure.BootStrappers.BootStrapper().Register(ServiceCollection);
        }

        private void ValidateAppRuntimeValue()
        {
            //Reading Environment Variable and validating either required runtime params has been passed or not.
            _envVariableBaseModel = ServiceCollection.BuildServiceProvider()
                                  .GetService<IOptions<AppEnvVariableBaseModel>>()?.Value;

            string messages = string.Empty;

            if (_envVariableBaseModel == null)
                messages = "Required Runtime Parameter Has Not Passed Like SQL Connection.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SQLDatabase))
                messages = "Required Runtime Parameter (SQLDatabase) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SQLPassword))
                messages = "Required Runtime Parameter (SQLPassword) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SQLServer))
                messages = "Required Runtime Parameter (SQLServer) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SQLUser))
                messages = "Required Runtime Parameter (SQLUser) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SecurityAppName))
                messages = "Required Runtime Parameter (SecurityAppName) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            if (_envVariableBaseModel != null)
                _sqlConnection = string.Format(Configuration.GetConnectionString(Key_SqlConnection),
                    _envVariableBaseModel.SQLServer,
                    _envVariableBaseModel.SQLDatabase,
                    _envVariableBaseModel.SQLUser,
                    _envVariableBaseModel.SQLPassword,
                    _envVariableBaseModel.SQLConnectionTimeout,
                    _envVariableBaseModel.SQLMaxPoolSize);
        }

        private void SetupGoogleService()
        {
            //Reading Environment Variable and validating either required runtime params has been passed or not.
            if (_envVariableBaseModel != null)
            {
                //always need to use YOUR_API_KEY for requests.  Do this in App_Start.
                GoogleSigned.AssignAllServices(new GoogleSigned(_envVariableBaseModel.GoogleApiKey));
            }
        }
    }
}