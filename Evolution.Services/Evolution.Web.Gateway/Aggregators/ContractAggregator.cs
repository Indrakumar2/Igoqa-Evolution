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
    public class ContractAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            ContractAggregatorResponse result = new ContractAggregatorResponse();

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
                                    if (rowIndex == 0) //contrcat Detail
                                    {
                                        var custs = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                        result.ContractInfo = custs.Count > 0 ? custs[0] : null;
                                    }
                                    else if (rowIndex == 1) // Contract ExchangeRate
                                        result.ContractExchangeRates = microServiceResponse.Result;
                                    else if (rowIndex == 2) // Contract InvoiceAttachment
                                        result.ContractInvoiceAttachments = microServiceResponse.Result;
                                    else if (rowIndex == 3) // Contract Invoice Reference
                                        result.ContractInvoiceReferences = microServiceResponse.Result; 
                                    else if (rowIndex == 4) // Contract Schedule
                                        result.ContractSchedules = microServiceResponse.Result;
                                    else if (rowIndex == 5) // Contract ScheduleRate
                                        result.ContractScheduleRates = microServiceResponse.Result;
                                    else if (rowIndex == 6) //Contract Document
                                        result.ContractDocuments = microServiceResponse.Result;
                                    else if (rowIndex == 7) // Contract Note
                                        result.ContractNotes = microServiceResponse.Result;
                                }
                            }
                        }
                    }
                    catch(Exception ex)
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
