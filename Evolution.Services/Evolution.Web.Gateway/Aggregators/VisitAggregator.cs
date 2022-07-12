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
    public class VisitAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            VisitAggregatorResponse result = new VisitAggregatorResponse();

            if(responses != null && responses.Count > 0)
            {
                for(int rowIndex = 0; rowIndex < responses.Count; rowIndex++)
                {
                    try
                    {
                        if (responses[rowIndex]?.StatusCode != System.Net.HttpStatusCode.OK) //Scenario 71 fix
                        {
                            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { Content = new StringContent("unauthenticated access") }));
                        }
                        var microServiceResponse = responses[rowIndex]?.StatusCode == System.Net.HttpStatusCode.OK ? JsonConvert.DeserializeObject<Response>(responses[rowIndex].Content.ReadAsStringAsync().Result) : null;
                        if(microServiceResponse != null && microServiceResponse.Code == MessageType.Success.ToId() && microServiceResponse.Result != null)
                        {
                            if(((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result).Count > 0)
                            {
                                if (rowIndex == 0)
                                {
                                    //var visits = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                    result.VisitInfo = microServiceResponse.Result;
                                }
                                //else if (rowIndex == 1)
                                //    result.HistoricalVisits = microServiceResponse.Result;
                                else if (rowIndex == 1)
                                    result.VisitTechnicalSpecialists = microServiceResponse.Result;
                                else if (rowIndex == 2)
                                    result.VisitTechnicalSpecialistTimes = microServiceResponse.Result;
                                else if (rowIndex == 3)                                    
                                    result.VisitTechnicalSpecialistExpenses = microServiceResponse.Result;
                                else if (rowIndex == 4)                                    
                                    result.VisitTechnicalSpecialistTravels = microServiceResponse.Result;
                                else if (rowIndex == 5)
                                    result.VisitTechnicalSpecialistConsumables = microServiceResponse.Result;
                                else if (rowIndex == 6)
                                    result.VisitReferences = microServiceResponse.Result;
                                else if (rowIndex == 7)
                                    result.VisitSupplierPerformances = microServiceResponse.Result;
                                else if (rowIndex == 8)
                                    result.VisitInterCompanyDiscounts = microServiceResponse.Result;
                                else if (rowIndex == 9)
                                    result.VisitDocuments = microServiceResponse.Result;
                                else if (rowIndex == 10)
                                    result.VisitNotes = microServiceResponse.Result;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        //Console.WriteLine(ex.ToFullString());
                    }
                }
            }

            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(result)) }));
        }
    }
}
