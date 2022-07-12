using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Web.Gateway.Models;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evolution.Web.Gateway.Aggregators
{
    public class ProjectAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            ProjectAggregatorResponse result = new ProjectAggregatorResponse();

            if (responses != null && responses.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < responses.Count; rowIndex++)
                {
                    try
                    {
                        if (responses[rowIndex]?.StatusCode != System.Net.HttpStatusCode.OK) //Scenario 71 fix
                        {
                            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { Content = new StringContent("unauthenticated access") }));
                        }
                        var microServiceResponse = responses[rowIndex]?.StatusCode == System.Net.HttpStatusCode.OK ? JsonConvert.DeserializeObject<Response>(responses[rowIndex].Content.ReadAsStringAsync().Result) : null;
                        if (microServiceResponse != null)
                        {
                            if (microServiceResponse.Code == MessageType.Success.ToId())
                            {
                                if (microServiceResponse.Result != null && ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result).Count > 0)
                                {
                                    if (rowIndex == 0) //project Detail
                                    {
                                        var projs = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                        result.ProjectInfo = projs.Count > 0 ? projs[0] : null;
                                    }
                                    else if (rowIndex == 1) // Project Invoice Attachments
                                        result.ProjectInvoiceAttachments = microServiceResponse.Result;
                                    else if (rowIndex == 2) // Project Invoice Reference
                                        result.ProjectInvoiceReferences = microServiceResponse.Result;
                                    else if (rowIndex == 3) // Project Documents
                                        result.ProjectDocuments = microServiceResponse.Result;
                                    else if (rowIndex == 4) // Project Notes
                                        result.ProjectNotes = microServiceResponse.Result;
                                    else if (rowIndex == 5) // Project Notifications
                                        result.ProjectNotifications = microServiceResponse.Result;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToFullString());
                        //TODO : Logg into logger
                    }
                }

            }
            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(result)) }));
        }
    }
}
