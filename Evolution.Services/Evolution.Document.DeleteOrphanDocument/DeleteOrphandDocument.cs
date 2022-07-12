using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Evolution.Document.DeleteOrphanDocument
{
    public class DeleteOrphandDocument
    { 
        private readonly IDocumentService _documentService = null; 
        private readonly AppEnvVariableBaseModel _syncSetting = null;
        private readonly IAppLogger<DeleteOrphandDocument> _logger = null;

        public DeleteOrphandDocument(IDocumentService documentService,
                               IOptions<AppEnvVariableBaseModel> syncSetting, 
                               IAppLogger<DeleteOrphandDocument> logger)
        { 
            _documentService = documentService;
            _syncSetting = syncSetting.Value; 
            _logger = logger;
        }

        public void DeleteOrphandDocuments(int deleteRowMaxLimit)
        {
            Exception exception = null;
            IList<string> orphanDocuments = null;
            try
            {
                PrintMessage("Fetching Orphan Document"); 
                orphanDocuments = _documentService.GetOrphandDocuments(deleteRowMaxLimit).Result.Populate<IList<string>>();
                PrintMessage(string.Format("Total orphan document found :- {0}", orphanDocuments?.Count));

                if (orphanDocuments?.Count <= 0) return;

                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                // Pass the handler to httpclient(to call api)
                using (var httpClient = new HttpClient(clientHandler))
                {
                    string url = _syncSetting.ApplicationGatewayURL + "DocumentContent/DeleteOrphandDocuments";
                    var uri = new Uri(url);
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(orphanDocuments), Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(uri, content);
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        PrintMessage(response?.Result?.ReasonPhrase);
                        _logger.LogError(ResponseType.Exception.ToId(), response?.Result?.ReasonPhrase, orphanDocuments);
                    }
                    else 
                    {
                        var responseBody = JObject.Parse(response?.Result?.Content.ReadAsStringAsync()?.Result)?.ToObject<Response>();
                        if (!string.Equals(responseBody?.Code, "1"))
                        {
                            PrintMessage(responseBody?.Messages?.FirstOrDefault()?.Message);
                            _logger.LogError(ResponseType.Exception.ToId(), responseBody?.Messages?.FirstOrDefault()?.Message);
                        }
                        else
                        {
                            PrintMessage("Task Completed.");
                        }  
                    } 
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                PrintMessage(ex.ToFullString());
            }
        }

        private void PrintMessage(string message)
        { 
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0}  DateTime :- {1}", message, DateTime.Now.ToString()));  
        }
    }
}
