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
    public class DashboardCountAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            HomeAggregatorResponse result =  new HomeAggregatorResponse();

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
                                if (microServiceResponse.RecordCount != null)
                                {
                                    if (rowIndex == 0) // Contract
                                        result.ContractCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 1) //Assignment
                                        result.AssignmentCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 2) //InActiveAssignment
                                        result.InactiveAssignmentCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 3) //visit
                                        result.VisitCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 4) //Timesheet
                                        result.TimesheetCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 5) //Document approval
                                        result.DocumentApprovalCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 6) //My Tasks
                                        result.MyTaskCount = microServiceResponse.RecordCount;
                                    else if (rowIndex == 7) //My Search
                                        result.MySearchCount = microServiceResponse.RecordCount;
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