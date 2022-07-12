using AutoMapper;
using Evolution.AuditLog.Core;
using Evolution.AuditLog.Core.Services;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Interfaces.Validations;
using Evolution.AuditLog.Infrastructure.Data;
using Evolution.AuditLog.Infrastructure.Validations;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.ILearn.Core.Services;
using Evolution.ILearn.Domain.Interfaces;
using Evolution.ILearn.Infrastructure.Data;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Evolution.Master.Core.Services;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Infrastructure.Data;
using Evolution.TechnicalSpecialist.Core.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Infrastructure.Data;
using Evolution.ValidationService.Interfaces;
using Evolution.ValidationService.Services;
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


namespace Evolution.Data.ILearn
{
    public class Program
    {
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_SyncSetting = "SyncSetting";
        private static string _sqlConnection = string.Empty;

        private static IServiceCollection _serviceCollection = null;
        private static IConfigurationRoot _configuration = null;

        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            _configuration = new ConfigurationBuilder()
                                           .AddJsonFile($"appsettings.json", true, true)
                                           .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                                           .AddEnvironmentVariables()
                                           .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
             

            //nlog settings
            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            logFactory.AddNLog();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                int syncInterVal = Convert.ToInt32(_configuration.GetConnectionString("ILearnIntervalInMinute"));//24 hours once
                while (true)
                {
                    try
                    {
                        // entry to run Sync Service
                        serviceProvider.GetService<ILearnSync>().PrformIlearnSyncOperation(_configuration); //ILearn Sync Starting Point
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToFullString());
                    }
                    finally
                    {
                        Environment.Exit(0);
                        Thread.Sleep(syncInterVal * 60000);
                    }
                }
            });

            _closing.WaitOne();
            Console.ReadLine();
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Gracefull Shoutdown of Application.");
            _closing.Set();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            //                                                .SetBasePath(Directory.GetCurrentDirectory())
            //                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //                                                .AddEnvironmentVariables();
            //_configuration = configurationBuilder.Build();

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

            ////Registering Mongo Setting
            //serviceCollection.Configure<MongoSetting>(x =>
            //{
            //    x.ConnectionString = string.Format("mongodb://{0}:{1}", _envVariableBaseModel.MongoDbIp, _envVariableBaseModel.MongoDbPort);
            //    x.DatabaseName = _envVariableBaseModel.MongoDbName;
            //    x.DocumentTypes = _envVariableBaseModel.MongoSyncTypes;
            //});

            serviceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));

            //Registering Document Upload Setting
            //serviceCollection.Configure<DocumentUploadSetting>(_configuration.GetSection(Key_DocumentUpload));

            //   new Evolution.Document.Infrastructe.BootStrappers.BootStrapper().Register(serviceCollection);
            // new Evolution.Document.Core.BootStrappers.BootStrapper().Register(serviceCollection);

            //Registering File Extractor Service
            //serviceCollection.AddScoped<IFileExtractorService, PdfExtractorService>();
            //serviceCollection.AddScoped<IFileExtractorService, ExcelExtractorService>();
            //serviceCollection.AddScoped<IFileExtractorService, WordFileExtractorService>();
            //serviceCollection.AddScoped<IFileExtractorService, TextFileExtractorService>();
            //serviceCollection.AddScoped<IFileExtractorService, EmailExtractorService>();

            serviceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            serviceCollection.AddAutoMapper(mapperConfig => mapperConfig.AddProfile(new Evolution.ILearn.Core.Mappers.DomainMapper()));

            // add application file
            serviceCollection.AddScoped<ILearnInterface, ILearnService>();
            serviceCollection.AddScoped<ILearnRespositoryInterface, ILearnRepository>();
            serviceCollection.AddScoped<ITechnicalSpecialistCompetencyService, TechnicalSpecialistCompetencyService>();
            serviceCollection.AddScoped<ITechnicalSpecialistTrainingAndCompetencyRepository, TechnicalSpecialistTrainingAndCompetencyRepository>();
            serviceCollection.AddScoped<ITechnicalSpecialistTrainingAndCompetencyTypeRepository, TechnicalSpecialistTrainingAndCompetencyTypeRepository>();
            serviceCollection.AddScoped<ITechnicalSpecialistTrainingAndCompetancyTypeService, TechnicalSpecialistTrainingAndCompetencyTypeService>();
            serviceCollection.AddScoped<ITechnicalSpecialistCertificationAndTrainingRepository, TechnicalSpecialistCertificationAndTrainingRepository>();
            serviceCollection.AddScoped<IInternalTrainingService, InternalTrainingService>();
            serviceCollection.AddScoped<IMasterRepository,MasterRepository>();
            serviceCollection.AddScoped<ICompetencyService, CompetencyService>();
            serviceCollection.AddScoped<IAuditSearchService, AuditSearchService>();
            serviceCollection.AddScoped<IAuditSearchRepository, AuditSearchReposiotry>();
            serviceCollection.AddScoped<IAuditLogger, AuditLogger>();
            serviceCollection.AddScoped<ISqlAuditLogEventInfoService, SqlAuditLogEventInfoService>();
            serviceCollection.AddScoped<ISqlAuditLogEventReposiotry, SqlAuditLogEventReposiotry>();
            serviceCollection.AddScoped<ISqlAuditLogEventValidationService, SqlAuditLogEventValidationService>();
            serviceCollection.AddScoped<IValidationService, Evolution.ValidationService.Services.ValidationService>();
            serviceCollection.AddScoped<ISqlAuditModuleService, SqlAuditModuleService>();
            serviceCollection.AddScoped<ISqlAuditModuleRepository, SqlAuditModuleRepository>();
            serviceCollection.AddScoped<ISqlAuditLogDetailInfoService, SqlAuditLogDetailInfoService>();
            serviceCollection.AddScoped<ISqlAuditLogDetailRepository, SqlAuditLogDetailRepository>();
            serviceCollection.AddScoped<ISqlAditLogDetailValidationService, SqlAditLogDetailValidationService>();
            serviceCollection.AddScoped<ILearnSync>();
            
            
        }

        private static void ConfigureSqlDbContext()
        {
            ValidateAppRuntimeValue();
            _serviceCollection.AddEntityFrameworkSqlServer()
                               .AddDbContext<EvolutionSqlDbContext>(options => options.UseSqlServer(_sqlConnection));
        }

        private static void ValidateAppRuntimeValue()
        {
            string messages = string.Empty;

            if (string.IsNullOrEmpty(_configuration.GetConnectionString(Key_SqlConnection)))
                messages = "Required Runtime Parameter (connectionString) Has Not Passed.";
            else if (string.IsNullOrEmpty(_configuration.GetConnectionString("FtpAddress")))//ILearn
                messages = "Required Runtime Parameter (FtpAddress) Has Not Passed.";
            else if (string.IsNullOrEmpty(_configuration.GetConnectionString("FTPUserName")))//ILearn
                messages = "Required Runtime Parameter (FTPUserName) Has Not Passed.";
            else if (string.IsNullOrEmpty(_configuration.GetConnectionString("FTPPassword")))//ILearn
                messages = "Required Runtime Parameter (FTPPassword) Has Not Passed.";
            else if (string.IsNullOrEmpty(_configuration.GetConnectionString("TempServerPath")))//ILearn
               messages = "Required Runtime Parameter (TempServerPath) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            _sqlConnection = _configuration.GetConnectionString(Key_SqlConnection);
            string value = _configuration.GetConnectionString("isLocal");
        }
    }
}
