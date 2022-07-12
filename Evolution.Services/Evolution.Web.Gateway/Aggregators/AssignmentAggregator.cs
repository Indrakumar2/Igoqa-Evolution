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
    public class AssignmentAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            AssignmentAggregatorResponse result = new AssignmentAggregatorResponse();
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
                                    if (rowIndex == 0) 
                                    {
                                        var assignment = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                        result.AssignmentInfo = assignment.Count > 0 ? assignment[0] : null;
                                    }
                                    else if (rowIndex == 1)
                                        result.AssignmentContractSchedules = microServiceResponse.Result;
                                    else if (rowIndex == 2)
                                        result.AssignmentTaxonomy = microServiceResponse.Result;
                                    else if (rowIndex == 3)
                                        result.AssignmentTechnicalSpecialists = microServiceResponse.Result;
                                    else if (rowIndex == 4) 
                                        result.AssignmentSubSuppliers = microServiceResponse.Result;
                                    else if (rowIndex == 5) 
                                        result.AssignmentReferences = microServiceResponse.Result;
                                    else if (rowIndex == 6)
                                        result.AssignmentInstructions = microServiceResponse.Result;
                                    else if (rowIndex == 7)
                                        result.AssignmentAdditionalExpenses = microServiceResponse.Result;
                                    else if (rowIndex == 8)
                                        result.AssignmentInterCompanyDiscounts = microServiceResponse.Result;
                                    else if (rowIndex == 9)
                                        result.AssignmentContributionCalculators = microServiceResponse.Result;
                                    else if (rowIndex == 10)
                                        result.AssignmentNotes = microServiceResponse.Result;
                                    else if (rowIndex == 11)
                                        result.AssignmentDocuments = microServiceResponse.Result;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO : Logg into logger
                        //Console.WriteLine(ex.ToFullString());
                       // _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), responses);
                    }
                }

            }
            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(result)) }));
        }
    }
}
