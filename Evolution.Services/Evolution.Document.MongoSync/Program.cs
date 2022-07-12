using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Evolution.Document.MongoSync
{
    public class Program
    {
        private const string Key_SqlConnection = "SQLDbConnection";
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

                //nlog settings
                ILoggerFactory logFactory = serviceProvider.GetService<ILoggerFactory>();
                logFactory.AddNLog();

                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnCancel);

                string arguments = args[0].Trim();
                if (!arguments.ToLower().Equals("migrated"))
                    ProcessLiveRecords(arguments, serviceProvider);
                else
                    ProcessMigratedRecords(arguments, serviceProvider);
            }
            catch (Exception ex)
            {
                PrintMessage(ex.ToFullString(), null);
            }
            finally
            {
                _envVariableBaseModel = null;
                _sqlConnection = string.Empty;
                _serviceCollection = null;
                _configuration = null;
            }
        }

        private static void ProcessMigratedRecords(string arguments, ServiceProvider serviceProvider)
        {
            int syncInterVal = _envVariableBaseModel.TaskRunIntervalInMinute;
            IDocumentMongoSyncService docMongoSyncService = serviceProvider.GetService<IDocumentMongoSyncService>();
            // Console.Clear();
            PrintMessage($"Task cammand/argument value :[{ arguments}]", null);
            while (!isCancelKeyPressed)
            {
                try
                {
                    ModuleDocument docToProsess = null;
                    docToProsess = docMongoSyncService.FetchTop1DocumentToBeProcessed(_envVariableBaseModel.DocumentTitle, _envVariableBaseModel.ModifiedBy);

                    if (docToProsess == null)
                    {
                        PrintMessage("Document not found to sync.", null);
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (docMongoSyncService.IsDocumentValidForSync(docToProsess?.DocumentName, out string message))
                    {
                        PrintMessage($"Fetching document [{ docToProsess.DocumentUniqueName }] text content.", null);

                        Response fileResult = FetchOnlyTextFromFile(docToProsess.FilePath);
                        if (!string.Equals(fileResult.Code, "1"))
                        {
                            if (fileResult.Messages.Any() && fileResult.Messages[0].Message.Contains("Message: Failed to process file, As file is password protected"))
                            {
                                message = "Failed to process file, As file is password protected";
                                docMongoSyncService.SaveSyncDocumentAuditDataUpdateDocumentStatus(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Non_Retryable, true, message);
                            }
                            else
                            {
                                message = fileResult.Messages?.FirstOrDefault()?.Message;
                                docMongoSyncService.SaveSyncDocumentAuditDataUpdateDocumentStatus(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false, message);
                            }
                            PrintMessage(message, docToProsess);
                            continue;
                        }
                        else
                        {
                            string fileText = fileResult.Result?.ToString();

                            if (string.IsNullOrEmpty(fileText))
                            {
                                string printMsg = string.Empty;
                                if (docToProsess.DocumentSize > 0)
                                {
                                    printMsg = "Failed to fetch text data from file";
                                    docMongoSyncService.SaveSyncDocumentAuditDataUpdateDocumentStatus(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false, printMsg);
                                }
                                else
                                {
                                    printMsg = "File has no text data";
                                    docMongoSyncService.SaveSyncDocumentAuditDataUpdateDocumentStatus(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Non_Retryable, true, printMsg);
                                }
                                PrintMessage(printMsg, docToProsess);
                                continue;
                            }
                            else
                            {
                                // string filterdText =RemoveStopwordsFromText(fileText);// Removing NLTK Stop words
                                docMongoSyncService.ProcessDocumentMigratedDocuments(docToProsess, fileText);
                            }
                        }
                    }
                    else
                    {
                        docMongoSyncService.SaveSyncDocumentAuditDataUpdateDocumentStatus(docToProsess?.DocumentUniqueName, message.Contains("Exception") ? MongoSyncStatusType.Failed_Retryable : MongoSyncStatusType.Failed_Non_Retryable, !message.Contains("Exception"), message);
                        PrintMessage(message, docToProsess);
                        continue;
                    }
                    PrintMessage($"Document [{ docToProsess.DocumentUniqueName }] Text content synched", null);
                    Thread.Sleep(syncInterVal * 60000);
                }
                catch (Exception ex)
                {
                    PrintMessage(ex.ToFullString(), null);
                    Thread.Sleep(15000);
                }
            }
        }

        private static void ProcessLiveRecords(string arguments, ServiceProvider serviceProvider)
        {
            int syncInterVal = _envVariableBaseModel.TaskRunIntervalInMinute;
            bool isProcessNewDocs = arguments.Equals("New", StringComparison.InvariantCultureIgnoreCase);
            IDocumentMongoSyncService docMongoSyncService = serviceProvider.GetService<IDocumentMongoSyncService>();
            // Console.Clear();
            PrintMessage($"Task cammand/argument value :[{ arguments}]", null);
            while (!isCancelKeyPressed)
            {
                try
                {
                    ModuleDocument docToProsess = null;
                    docToProsess = docMongoSyncService.FetchTop1DocumentToBeProcessed(isProcessNewDocs);

                    if (docToProsess == null)
                    {
                        PrintMessage("Document not found to sync.", null);
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (docMongoSyncService.IsDocumentValidForSync(docToProsess?.DocumentUniqueName, out string message))
                    {
                        docMongoSyncService.SaveSyncDocumentAuditData(docToProsess?.DocumentUniqueName, MongoSyncStatusType.In_Progress, false);

                        PrintMessage($"Fetching document [{ docToProsess.DocumentUniqueName }] text content.", null);

                        Response fileResult = FetchOnlyTextFromFile(docToProsess.FilePath);
                        if (!string.Equals(fileResult.Code, "1"))
                        {
                            if (fileResult.Messages.Any() && fileResult.Messages[0].Message.Contains("Message: Failed to process file, As file is password protected"))
                            {
                                message = "Failed to process file, As file is password protected";
                                docMongoSyncService.SaveSyncDocumentAuditData(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Non_Retryable, true, message);
                            }
                            else
                            {
                                message = fileResult.Messages?.FirstOrDefault()?.Message;
                                docMongoSyncService.SaveSyncDocumentAuditData(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false, message);
                            }
                            PrintMessage(message, docToProsess);
                            continue;
                        }
                        else
                        {
                            string fileText = fileResult.Result?.ToString();

                            if (string.IsNullOrEmpty(fileText))
                            {
                                string printMsg = string.Empty;
                                if (docToProsess.DocumentSize > 0)
                                {
                                    printMsg = "Failed to fetch text data from file";
                                    docMongoSyncService.SaveSyncDocumentAuditData(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false, printMsg);
                                }
                                else
                                {
                                    printMsg = "File has no text data";
                                    docMongoSyncService.SaveSyncDocumentAuditData(docToProsess?.DocumentUniqueName, MongoSyncStatusType.Failed_Non_Retryable, true, printMsg);
                                }
                                PrintMessage(printMsg, docToProsess);
                                continue;
                            }
                            else
                            {
                                // string filterdText =RemoveStopwordsFromText(fileText);// Removing NLTK Stop words
                                docMongoSyncService.ProcessDocument(docToProsess, fileText);
                            }
                        }
                    }
                    else
                    {
                        docMongoSyncService.SaveSyncDocumentAuditData(docToProsess?.DocumentUniqueName, message.Contains("Exception") ? MongoSyncStatusType.Failed_Retryable : MongoSyncStatusType.Failed_Non_Retryable, !message.Contains("Exception"), message);
                        PrintMessage(message, docToProsess);
                        continue;
                    }
                    PrintMessage($"Document [{ docToProsess.DocumentUniqueName }] Text content synched", null);
                    Thread.Sleep(syncInterVal * 60000);
                }
                catch (Exception ex)
                {
                    PrintMessage(ex.ToFullString(), null);
                    Thread.Sleep(15000);
                }
            }
        }

        protected static void OnCancel(object sender, ConsoleCancelEventArgs args)
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
                                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                        .AddEnvironmentVariables();

            _configuration = configurationBuilder.Build();
            _serviceCollection.Configure<AppEnvVariableBaseModel>(_configuration);//Registering Environment Variable. 
            ValidateAppRuntimeValue();//Validating Required Runtime Value
            //SQLdb Configuration
            _serviceCollection.AddEntityFrameworkSqlServer()
                            .AddDbContext<EvolutionSqlDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(_sqlConnection));

            // nlog Settings
            GlobalDiagnosticsContext.Set("connectionString", _sqlConnection);
            GlobalDiagnosticsContext.Set("configDir", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            LogManager.Configuration.Variables["microservicename"] = AppDomain.CurrentDomain.FriendlyName;

            //Registering Mongo Setting
            serviceCollection.Configure<MongoSetting>(x =>
            {
                x.ConnectionString = string.Format("mongodb://{0}:{1}", _envVariableBaseModel.MongoDbIp, _envVariableBaseModel.MongoDbPort);
                x.DatabaseName = _envVariableBaseModel.MongoDbName;
                x.DocumentTypes = _envVariableBaseModel.MongoSyncTypes;
            });

            serviceCollection.AddSingleton<JObject>(new JObject(JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")))));

            new Evolution.Document.Infrastructe.BootStrappers.BootStrapper().Register(serviceCollection);
            new Evolution.Document.Core.BootStrappers.BootStrapper().Register(serviceCollection);
            serviceCollection.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            serviceCollection.AddAutoMapper(mapperConfig => mapperConfig.AddProfile(new Core.Mappers.DomainMapper()));
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
            else if (string.IsNullOrEmpty(_envVariableBaseModel.MongoDbIp))
                messages = "Required Runtime Parameter (MongoDbIp) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.MongoDbPort))
                messages = "Required Runtime Parameter (MongoDbPort) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.MongoDbName))
                messages = "Required Runtime Parameter (MongoDbName) Has Not Passed.";
            else if (string.IsNullOrEmpty(_envVariableBaseModel.ApplicationGatewayURL))
                messages = "Required Runtime Parameter (ApplicationGatewayURL) Has Not Passed.";

            if (!string.IsNullOrEmpty(messages))
                throw new SystemException(messages);

            if (_envVariableBaseModel?.TaskRunIntervalInMinute == null || _envVariableBaseModel.TaskRunIntervalInMinute <= 0)
                _envVariableBaseModel.TaskRunIntervalInMinute = 1;

            _envVariableBaseModel.MongoSyncTypes = ".pdf,.doc,.docx,.txt,.msg,.xls,.xlsx";

            string sqlConnectionFormat = _configuration.GetConnectionString(Key_SqlConnection);
            _sqlConnection = string.Format((string.IsNullOrEmpty(sqlConnectionFormat) ? "Server={0};Database={1};user={2};password={3};Connection Timeout={4};Max Pool Size={5};" : sqlConnectionFormat),
                                            _envVariableBaseModel.SQLServer,
                                            _envVariableBaseModel.SQLDatabase,
                                            _envVariableBaseModel.SQLUser,
                                            _envVariableBaseModel.SQLPassword,
                                            _envVariableBaseModel.SQLConnectionTimeout,
                                            _envVariableBaseModel.SQLMaxPoolSize);
        }

        private static void PrintMessage(string message, ModuleDocument document)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(string.Format("{0}  DateTime :- {1}", message, DateTime.Now.ToString()));
            if (document != null)
            {
                Console.WriteLine("Document Detail :-");
                Console.WriteLine(document?.ToText());
            }
        }

        private static Response FetchOnlyTextFromFile(string filePath)
        {
            try
            {

                if (string.IsNullOrEmpty(filePath)) throw new Exception("Invalid file Path passed in parameter.");

                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                // Pass the handler to httpclient(to call api)
                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    string url = _envVariableBaseModel.ApplicationGatewayURL + "DocumentContent/FetchOnlyTextFromFile";
                    Uri uri = new Uri(url);
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(filePath), Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(uri, content);
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        PrintMessage(response?.Result?.ReasonPhrase, null);
                        throw new Exception(response?.Result?.ReasonPhrase);
                    }
                    else
                    {
                        var responseBody = response?.Result?.Content.ReadAsStringAsync();
                        return JObject.Parse(responseBody?.Result)?.ToObject<Response>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static string RemoveStopwordsFromText(string inputText)
        //{  
        //    try
        //    {
        //        StringBuilder resultText = new StringBuilder();
        //        MLContext context = new MLContext();
        //        List<TextData> emptyData = new List<TextData>();
        //        var data = context.Data.LoadFromEnumerable(emptyData);
        //        var tokenization = context.Transforms.Text.TokenizeIntoWords("Tokens", "Text", separators: new[] { ' ', ',', '.' })
        //            .Append(context.Transforms.Text.RemoveDefaultStopWords("Tokens", "Tokens",
        //            Microsoft.ML.Transforms.Text.StopWordsRemovingEstimator.Language.English));

        //        var model = tokenization.Fit(data);
        //        var engine = context.Model.CreatePredictionEngine<TextData, TextTokens>(model);
        //        var text = engine.Predict(new TextData { Text = inputText });
        //        text.Tokens.ToList().ForEach(x => resultText.AppendLine(x));
        //        return resultText.ToString().RemoveEmptyRowAndControlChar();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }

    //public class TextData
    //{
    //    public string Text { get; set; }
    //}

    //public class TextTokens
    //{
    //    public string[] Tokens { get; set; }
    //}
}