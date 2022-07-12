using Evolution.Web.Gateway.Interfaces;
using Evolution.Web.Gateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Evolution.Web.Gateway.Decomposers.Handlers
{
    public class CustomerDecompositionHandler : DelegatingHandler
    {
        ICustomerProcessor _processor;
        private const string MEDIATYPE_JSON = "application/json";

        public CustomerDecompositionHandler(ICustomerProcessor processor)
        {
            _processor = processor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                try
                {
                    if (request.Content.Headers.ContentType.MediaType.ToLower() != MEDIATYPE_JSON)
                        throw new UnsupportedMediaTypeException("Request body content type is not supported.", new System.Net.Http.Headers.MediaTypeHeaderValue(request.Content.Headers.ContentType.MediaType));

                    var customerDecompositionRequest = JsonConvert.DeserializeObject<IList<CustomerDecompositionRequest>>(request.Content.ReadAsStringAsync().Result);
                    var processorResponse = _processor.Process(customerDecompositionRequest, request.Method);
                    response.Content = new StringContent(JsonConvert.SerializeObject(processorResponse));
                }
                catch (Exception ex)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = new StringContent(ex.Message);
                }
            }
            else if (request.Method == HttpMethod.Get)
                return await base.SendAsync(request, cancellationToken);
            else
                response.Content = new StringContent("Ok");

            var task = new TaskCompletionSource<HttpResponseMessage>();
            task.SetResult(response);
            return await task.Task;
        }
    }
}
