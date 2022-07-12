using AutoMapper;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Models;
using Evolution.Google.Model.Interfaces;
using Evolution.Google.Model.Models;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Evolution.ValidationService.Interfaces;
using Google.Maps;
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
using Microsoft.AspNetCore.Identity;

namespace Evolution.TechSpecialist.GeoCoordinate.Extractor
{
    class Program
    {
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_MongoSetting = "MongoSetting";
        private const string Key_SyncSetting = "SyncSetting";
        private static AppEnvVariableBaseModel _envVariableBaseModel = null;
        private static string _sqlConnection = string.Empty;

        private static IServiceCollection _serviceCollection = null;
        private static IConfigurationRoot _configuration = null;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //nlog settings
            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            logFactory.AddNLog();

            //Insuring Index for Geospatial
            InsureGeospatialIndex(serviceProvider);

            // entry to run Sync Service
            serviceProvider.GetService<SyncService>().PrformExtractOperation();

            //var searchModel = new LocationSearchInfo() { Country = "China", State= "Shanghai", City= "Shanghai", Zip = "" };
            //var test= serviceProvider.GetService<IGeoCoordinateService>();
            //var result=test.GetLocationCoordinate(searchModel);
            //if (result != null)
            //{
            //    Console.WriteLine("Full Address: " + result.Address);         // "1600 Pennsylvania Ave NW, Washington, DC 20500, USA"
            //    Console.WriteLine("Latitude: " + result.Coordinate.Latitude);   // 38.8976633
            //    Console.WriteLine("Longitude: " + result.Coordinate.Longitude); // -77.0365739
            //    Console.WriteLine();
            //}
            Console.ReadLine();
            //var tsGeoService = serviceProvider.GetService<ITsGeoCoordinateService>();
            //tsGeoService.Add(new TechnicalSpecialist.Domain.Models.TechSpecialist.Mongos.TsGeoCoordinateInfo()
            //{
            //    Epin = "123444444",
            //    Coordinate = new TechnicalSpecialist.Domain.Models.TechSpecialist.Mongos.GeoCoordinateInfo(86.959727000000001, 33.588436999999999)
            //});
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                            .SetBasePath(Directory.GetCurrentDirectory())
                                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                            .AddEnvironmentVariables();
            _configuration = configurationBuilder.Build();

            //Registering Environment Variable.
            _serviceCollection.Configure<AppEnvVariableBaseModel>(_configuration);

            //Validating Required Runtime Value
            ValidateAppRuntimeValue();

            //SQLdb Configuration
            ConfigureSqlDbContext();

            //Google API Key Setup
            SetupGoogleService(_envVariableBaseModel);


            // nlog Settings
            GlobalDiagnosticsContext.Set("connectionString", _sqlConnection);
            GlobalDiagnosticsContext.Set("configDir", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            LogManager.Configuration.Variables["microservicename"] = AppDomain.CurrentDomain.FriendlyName;

            //Registering Mongo Setting
            serviceCollection.Configure<MongoSetting>(x =>
            {
                x.ConnectionString = string.Format("mongodb://{0}:{1}", _envVariableBaseModel.MongoDbIp, _envVariableBaseModel.MongoDbPort);
                x.DatabaseName = _envVariableBaseModel.MongoDbName;
                x.DocumentTypes = _envVariableBaseModel.MongoSyncTypes;
            });

            serviceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));

            new Evolution.Google.Core.BootStrappers.BootStrapper().Register(serviceCollection);

            new Evolution.DbRepository.BootStrappers.BootStrapper().RegisterDataService(serviceCollection);

            new Evolution.Master.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Master.Core.BootStrappers.BootStrapper().Register(serviceCollection);

            new Evolution.Company.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Company.Core.BootStrappers.BootStrapper().Register(serviceCollection);

            new Evolution.TechnicalSpecialist.Core.BootStrappers.BootStrapper().RegisterTsService(serviceCollection);
            new Evolution.TechnicalSpecialist.Infrastructure.BootStrappers.BootStrapper().RegisterTsService(serviceCollection);

            new Evolution.TechnicalSpecialist.Core.BootStrappers.BootStrapper().RegisterContactService(serviceCollection);
            new Evolution.TechnicalSpecialist.Infrastructure.BootStrappers.BootStrapper().RegisterContactService(serviceCollection);
            
            new Evolution.Document.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Document.Infrastructe.BootStrappers.BootStrapper().Register(serviceCollection);

            new Evolution.Home.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Home.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);

            new Evolution.Security.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Security.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);

            new Evolution.AuditLog.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.AuditLog.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);

            serviceCollection.AddScoped<IValidationService, ValidationService.Services.ValidationService>();
            serviceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            serviceCollection.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            serviceCollection.AddScoped<SyncService>();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Evolution.TechnicalSpecialist.Core.Mappers.DomainMapper());
            });

            serviceCollection.AddAutoMapper();
        }

        private static void InsureGeospatialIndex(ServiceProvider provider)
        {
            var service = provider.GetService<IMongoGeoCoordinateService>();
            service.InsureIndex();
        }

        private static void ConfigureSqlDbContext()
        {
            ValidateAppRuntimeValue();
            _serviceCollection.AddEntityFrameworkSqlServer()
                               .AddDbContext<EvolutionSqlDbContext>(options => options.UseSqlServer(_sqlConnection));
        }

        private static void ValidateAppRuntimeValue()
        {
            //Reading Environment Variable and validating either required runtime params has been passed or not.
            _envVariableBaseModel = _serviceCollection.BuildServiceProvider()
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
            else if (string.IsNullOrEmpty(_envVariableBaseModel.MongoDbIp))
                messages = "Required Runtime Parameter (MongoDbIp) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.MongoDbPort))
                messages = "Required Runtime Parameter (MongoDbPort) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.MongoDbName))
                messages = "Required Runtime Parameter (MongoDbName) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.GoogleApiKey))
                messages = "Required Runtime Parameter (GoogleApiKey) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            if (_envVariableBaseModel != null)
                _sqlConnection = string.Format(_configuration.GetConnectionString(Key_SqlConnection),
                    _envVariableBaseModel.SQLServer,
                    _envVariableBaseModel.SQLDatabase,
                    _envVariableBaseModel.SQLUser,
                    _envVariableBaseModel.SQLPassword);
        }

        private static void SetupGoogleService(AppEnvVariableBaseModel appEnvVariableBaseModel)
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
