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
    public class TimesheetAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            TimesheetAggregatorResponse result = new TimesheetAggregatorResponse();

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
                        if (microServiceResponse != null && microServiceResponse.Code == MessageType.Success.ToId() && microServiceResponse.Result != null)
                        {
                            if (((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result).Count > 0)
                            {
                                if (rowIndex == 0)
                                {
                                    var timesheets = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                    result.TimesheetInfo = timesheets.Count > 0 ? timesheets[0] : null;
                                }
                                else if (rowIndex == 1)
                                    result.TimesheetTechnicalSpecialists = microServiceResponse.Result;
                                else if (rowIndex == 2)
                                    result.TimesheetTechnicalSpecialistTimes = microServiceResponse.Result;
                                else if (rowIndex == 3)
                                    result.TimesheetTechnicalSpecialistExpenses = microServiceResponse.Result;
                                else if (rowIndex == 4)
                                    result.TimesheetTechnicalSpecialistTravels = microServiceResponse.Result;
                                else if (rowIndex == 5)
                                    result.TimesheetTechnicalSpecialistConsumables = microServiceResponse.Result;
                                else if (rowIndex == 6)
                                    result.TimesheetReferences = microServiceResponse.Result;
                                else if (rowIndex == 7)
                                    result.TimesheetDocuments = microServiceResponse.Result;
                                else if (rowIndex == 8)
                                    result.TimesheetNotes = microServiceResponse.Result;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToFullString());
                    }
                }
            }

            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(result)) }));
        }
    }
}
