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
    public class TechnicalSpecialistAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            TechnicalSpecialistAggregatorResponse result = new TechnicalSpecialistAggregatorResponse();

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
                                        var techSpecialist = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                        result.TechnicalSpecialistInfo = techSpecialist.Count > 0 ? techSpecialist[0] : null;
                                    }
                                    else if (rowIndex == 1) 
                                        result.TechnicalSpecialistStamp = microServiceResponse.Result;
                                    else if (rowIndex == 2) 
                                        result.TechnicalSpecialistContact = microServiceResponse.Result;
                                    else if (rowIndex == 3) 
                                        result.TechnicalSpecialistPaySchedule = microServiceResponse.Result;
                                    else if (rowIndex == 4) 
                                        result.TechnicalSpecialistPayRate = microServiceResponse.Result;
                                    else if (rowIndex == 5) 
                                        result.TechnicalSpecialistTaxonomy = microServiceResponse.Result;
                                    else if (rowIndex == 6)
                                        result.TechnicalSpecialistInternalTraining = microServiceResponse.Result;
                                    else if (rowIndex == 7)
                                        result.TechnicalSpecialistCompetancy = microServiceResponse.Result;
                                    else if (rowIndex == 8)
                                        result.TechnicalSpecialistCustomerApproval = microServiceResponse.Result;
                                    else if (rowIndex == 9)
                                        result.TechnicalSpecialistWorkHistory = microServiceResponse.Result;
                                    else if (rowIndex == 10)
                                        result.TechnicalSpecialistEducation = microServiceResponse.Result;
                                    else if (rowIndex == 11)
                                        result.TechnicalSpecialistCodeAndStandard = microServiceResponse.Result;
                                    else if (rowIndex == 12)
                                        result.TechnicalSpecialistComputerElectronicKnowledge = microServiceResponse.Result;
                                    else if (rowIndex == 13)
                                        result.TechnicalSpecialistLanguageCapabilities = microServiceResponse.Result;
                                    else if (rowIndex == 14)
                                        result.TechnicalSpecialistCommodityAndEquipment = microServiceResponse.Result;
                                    else if (rowIndex == 15)
                                        result.TechnicalSpecialistTraining = microServiceResponse.Result;
                                    else if (rowIndex == 16)
                                        result.TechnicalSpecialistCertification = microServiceResponse.Result;
                                    else if (rowIndex == 17)
                                        result.TechnicalSpecialistDocuments = microServiceResponse.Result;
                                    else if (rowIndex == 18)
                                      result.TechnicalSpecialistSensitiveDocuments = microServiceResponse.Result;
                                    else if (rowIndex == 19)
                                        result.TechnicalSpecialistNotes = microServiceResponse.Result;

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
