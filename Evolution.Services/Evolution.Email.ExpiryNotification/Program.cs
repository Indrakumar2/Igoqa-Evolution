using AutoMapper;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Evolution.ValidationService.Interfaces;
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
using System.Threading;

namespace Evolution.Email.ExpiryNotification
{
    class Program
    {
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_DocumentUpload = "DocumentUpload";
        private static string _sqlConnection = string.Empty; 
        private static AppEnvVariableBaseModel _envVariableBaseModel = null; 
        private static IServiceCollection _serviceCollection = null;
        private static IConfigurationRoot _configuration = null;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //nlog settings
            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            logFactory.AddNLog();

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    // entry to run Sync Service
                    serviceProvider.GetService<ExpiryNotificationService>().FindingExpiredRecords();

                    int syncInterVal = 1440;// one day 
                    Thread.Sleep(syncInterVal  * 60000);
                }
            });

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

            Console.ReadLine();
            _closing.WaitOne();
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Gracefull Shoutdown of Application.");
            _closing.Set();
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

            // nlog Settings
            GlobalDiagnosticsContext.Set("connectionString", _sqlConnection);
            GlobalDiagnosticsContext.Set("configDir", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            LogManager.Configuration.Variables["microservicename"] = AppDomain.CurrentDomain.FriendlyName;

            serviceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));
            serviceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            serviceCollection.AddScoped<ExpiryNotificationService>();
            serviceCollection.AddScoped<IValidationService, ValidationService.Services.ValidationService>();

            new Evolution.TechnicalSpecialist.Infrastructure.BootStrappers.BootStrapper().RegisterTsService(serviceCollection);
            new Evolution.Email.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Email.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Security.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Evolution.Email.Core.Mappers.DomainMapper());
            });
            serviceCollection.AddAutoMapper();
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

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            if (_envVariableBaseModel != null)
                _sqlConnection = string.Format(_configuration.GetConnectionString(Key_SqlConnection),
                    _envVariableBaseModel.SQLServer,
                    _envVariableBaseModel.SQLDatabase,
                    _envVariableBaseModel.SQLUser,
                    _envVariableBaseModel.SQLPassword);
        }
    }
}
