using AutoMapper;
using Evolution.Admin.Core.Services;
using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Interfaces.Batch;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Admin.Infrastructure.Data;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Contract.Core.Services;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Infrastructure.Data;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
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
using Evolution.DbRepository.Services.Master;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Infrastructure.Data;
using Evolution.Contract.Domain.Interfaces.Validations;
using Evolution.Contract.Infrastructure.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.AuditLog.Core.Services;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Infrastructure.Data;
using Evolution.AuditLog.Core;
using Evolution.AuditLog.Domain.Interfaces.Validations;
using Evolution.AuditLog.Infrastructure.Validations;

namespace Evolution.Batch
{
    public class Program
    {
        private const string Key_SqlConnection = "SQLDbConnection";
        private static AppEnvVariableBaseModel _envVariableBaseModel = null;
        private static IServiceCollection _serviceCollection = null;
        private static IConfigurationRoot _configuration = null;
        private static string _sqlConnection = string.Empty;
        private static bool isCancelKeyPressed = false;
        private const string Key_SqlSetting = "SQLSetting";

        static void Main(string[] args)
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider();
                //nlog settings
                var logFactory = serviceProvider.GetService<ILoggerFactory>();
                logFactory.AddNLog();
                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnCancel);
                ProcessBatch(serviceProvider);
            }
            catch (Exception ex)
            {
                PrintMessage(ex.ToFullString());
            }
            finally
            {
                _envVariableBaseModel = null;
                _sqlConnection = string.Empty;
                _serviceCollection = null;
                _configuration = null;
            }
        }

        private static void PrintMessage(string message)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(string.Format("{0}  DateTime :- {1}", message, DateTime.Now.ToString()));
        }

        private static void ProcessBatch(ServiceProvider serviceProvider)
        {
            try
            {
                using (var batchProcessor = serviceProvider.GetService<BatchProcessor>())
                {
                    Console.WriteLine("Batch Process Started");
                    batchProcessor.StartBatch();
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(15000);
                Console.WriteLine(ex.ToFullString());
            }
        }

        protected static void OnCancel(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Canceling Application.");
            isCancelKeyPressed = true;
            Environment.Exit(0);
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            IConfigurationBuilder configurationBuilder = null;
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (!string.IsNullOrEmpty(environmentName))
            {
                configurationBuilder = new ConfigurationBuilder()
                                               .AddJsonFile($"appsettings.json", optional: false)
                                               .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                                               .AddEnvironmentVariables();
            }
            else
            {
                configurationBuilder = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                            .AddEnvironmentVariables();
            }
            _configuration = configurationBuilder.Build();

            _serviceCollection.Configure<AppEnvVariableBaseModel>(_configuration.GetSection(Key_SqlSetting));

            //Registering Environment Variable.
            _serviceCollection.Configure<AppEnvVariableBaseModel>(_configuration);

            //Validating Required Runtime Value
            ValidateAppRuntimeValue();

            //SQLdb Configuration
            ConfigureSqlDbContext();

            // nlog Settings
            GlobalDiagnosticsContext.Set("connectionString", _sqlConnection);
            GlobalDiagnosticsContext.Set("configDir", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            LogManager.Configuration.Variables["microservicename"] = AppDomain.CurrentDomain.FriendlyName;

            serviceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));

            new Evolution.Admin.Infrastructure.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Admin.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            serviceCollection.AddScoped<IBatchProcessService, BatchProcessService>();
            serviceCollection.AddScoped<IBatchProcess, BatchRepository>();
            serviceCollection.AddScoped<IContractScheduleService, ContractScheduleService>();
            serviceCollection.AddScoped<IContractScheduleRepository, ContractScheduleRepository>();
            serviceCollection.AddScoped<IDataRepository, DataRepository>();
            serviceCollection.AddScoped<IContractRepository, ContractRepository>();
            serviceCollection.AddScoped<IContractScheduleRateRepository, ContractScheduleRateRepository>();
            serviceCollection.AddScoped<IContractScheduleRateService, ContractScheduleRateService>();
            serviceCollection.AddScoped<ICompanyInspectionTypeChargeRateRepository, CompanyInspectionTypeChargeRateRepository>();
            serviceCollection.AddScoped<IContractScheduleRateValidationService, ContractScheduleRateValidationService>();
            serviceCollection.AddScoped<IValidationService, Evolution.ValidationService.Services.ValidationService>();
            serviceCollection.AddScoped<IAuditSearchService, AuditSearchService>();
            serviceCollection.AddScoped<IAuditSearchRepository, AuditSearchReposiotry>();
            serviceCollection.AddScoped<IAuditLogger, AuditLogger>();
            serviceCollection.AddScoped<ISqlAuditLogEventInfoService, SqlAuditLogEventInfoService>();
            serviceCollection.AddScoped<ISqlAuditLogEventReposiotry, SqlAuditLogEventReposiotry>();
            serviceCollection.AddScoped<ISqlAuditLogEventValidationService, SqlAuditLogEventValidationService>();
            serviceCollection.AddScoped<ISqlAuditModuleService, SqlAuditModuleService>();
            serviceCollection.AddScoped<ISqlAuditModuleRepository, SqlAuditModuleRepository>();
            serviceCollection.AddScoped<ISqlAuditLogDetailInfoService, SqlAuditLogDetailInfoService>();
            serviceCollection.AddScoped<ISqlAuditLogDetailRepository, SqlAuditLogDetailRepository>();
            serviceCollection.AddScoped<ISqlAditLogDetailValidationService, SqlAditLogDetailValidationService>();
            serviceCollection.AddScoped<IContractScheduleValidationService, ContractScheduleValidationService>();
            serviceCollection.AddScoped<IContractBatchRepository, ContractBatchRepository>();
            serviceCollection.AddScoped<IBatchProcessService, BatchProcessService>();
            //Registering File Extractor Service
            serviceCollection.AddScoped<IBatchService, BatchService>();

            serviceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            serviceCollection.AddAutoMapper(mapperConfig => mapperConfig.AddProfile(new Admin.Core.Mappers.DomainMapper()));

            // add application file
            serviceCollection.AddScoped<BatchProcessor>();
        }

        private static void ConfigureSqlDbContext()
        {
            ValidateAppRuntimeValue();
            //_serviceCollection.AddEntityFrameworkSqlServer()
            //                   .AddDbContext<EvolutionSqlDbContext>(options => options.UseSqlServer(_sqlConnection).EnableSensitiveDataLogging(true));
            _serviceCollection.AddEntityFrameworkSqlServer()
                               .AddDbContext<EvolutionSqlDbContext>(options => options.UseSqlServer(_sqlConnection));
        }

        private static void ValidateAppRuntimeValue()
        {
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
            string sqlConnectionFormat = _configuration.GetConnectionString(Key_SqlConnection);
            _sqlConnection = string.Format((string.IsNullOrEmpty(sqlConnectionFormat) ? "Server={0};Database={1};user={2};password={3};" : sqlConnectionFormat),
                                            _envVariableBaseModel.SQLServer,
                                            _envVariableBaseModel.SQLDatabase,
                                            _envVariableBaseModel.SQLUser,
                                            _envVariableBaseModel.SQLPassword);
        }
    }
}