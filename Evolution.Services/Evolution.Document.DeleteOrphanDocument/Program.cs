using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.DeleteOrphanDocument;
using Evolution.Document.Domain.Models;
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

namespace Evolution.Document.MongoSync
{
    public class Program
    {
        private const string Key_SqlConnection = "SQLDbConnection";
        private const string Key_SqlSetting = "SQLSetting";   
        private static AppEnvVariableBaseModel _envVariableBaseModel = null; 
        private static IServiceCollection _serviceCollection = null;
        private static IConfigurationRoot _configuration = null; 
        private static string _sqlConnection = string.Empty;
        private static bool isCancelKeyPressed = false;

        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            try
            {
                ServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

                ILoggerFactory logFactory = serviceProvider.GetService<ILoggerFactory>();
                logFactory.AddNLog();

                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnClose);
                int taskRunInterVal = _envVariableBaseModel.TaskRunIntervalInMinute;

                //Console.Clear();  
                while (!isCancelKeyPressed)
                {
                    try
                    {
                        serviceProvider.GetService<DeleteOrphandDocument>().DeleteOrphandDocuments(50000);
                        Thread.Sleep(taskRunInterVal * 60000);
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToFullString());
                        Thread.Sleep(15000);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToFullString()); 
            }
            finally {
                _envVariableBaseModel = null;
                _serviceCollection = null;
                _configuration = null;
            }      
        }

        protected static void OnClose(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Canceling Application.");
            _closing.Set();
            isCancelKeyPressed = true;
            Environment.Exit(0);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection; 
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                        .SetBasePath(Directory.GetCurrentDirectory()) 
                                                        .AddEnvironmentVariables(); 
            _configuration = configurationBuilder.Build(); 
            _serviceCollection.Configure<AppEnvVariableBaseModel>(_configuration);//Registering Environment Variable. 
            ValidateAppRuntimeValue();//Validating Required Runtime Value 
            //SQLdb Configuration
            _serviceCollection.AddEntityFrameworkSqlServer()
                              .AddDbContext<EvolutionSqlDbContext>(options => options.UseSqlServer(_sqlConnection, sqlServerOptions => sqlServerOptions.CommandTimeout(1200)));
            
            // nlog Settings
            GlobalDiagnosticsContext.Set("connectionString", _sqlConnection);
            GlobalDiagnosticsContext.Set("configDir", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            LogManager.Configuration.Variables["microservicename"] = AppDomain.CurrentDomain.FriendlyName;

            serviceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));
            serviceCollection.AddAutoMapper(mapperConfig => mapperConfig.AddProfile(new Core.Mappers.DomainMapper()));

            new Evolution.Document.Infrastructe.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Document.Core.BootStrappers.BootStrapper().Register(serviceCollection);
             
            serviceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));  
            serviceCollection.AddScoped<DeleteOrphandDocument>(); 
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
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SQLConnectionTimeout))
                messages = "Required Runtime Parameter (SQLConnectionTimeout) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.SQLMaxPoolSize))
                messages = "Required Runtime Parameter (SQLMaxPoolSize) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.ApplicationGatewayURL))
                messages = "Required Runtime Parameter (ApplicationGatewayURL) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            if (_envVariableBaseModel?.TaskRunIntervalInMinute == null || _envVariableBaseModel.TaskRunIntervalInMinute <= 0)
                _envVariableBaseModel.TaskRunIntervalInMinute = 1;

            string sqlConnectionFormat = _configuration.GetConnectionString(Key_SqlConnection);
            _sqlConnection = string.Format((string.IsNullOrEmpty(sqlConnectionFormat) ? "Server={0};Database={1};user={2};password={3};Connection Timeout={4};Max Pool Size={5};" : sqlConnectionFormat),
                                            _envVariableBaseModel.SQLServer,
                                            _envVariableBaseModel.SQLDatabase,
                                            _envVariableBaseModel.SQLUser,
                                            _envVariableBaseModel.SQLPassword,
                                            _envVariableBaseModel.SQLConnectionTimeout,
                                            _envVariableBaseModel.SQLMaxPoolSize);
        }
         
    }
}