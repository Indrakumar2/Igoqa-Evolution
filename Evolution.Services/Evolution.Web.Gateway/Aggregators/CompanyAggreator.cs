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
    public class CompanyAggreator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            CompanyAggregatorResponse result = new CompanyAggregatorResponse();

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
                                    if (rowIndex == 0) // Company Detail
                                    {
                                        var custs = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                        result.CompanyInfo = custs.Count > 0 ? custs[0] : null;
                                    }
                                    else if (rowIndex == 1) //Company Office / Address
                                        result.CompanyOffices = microServiceResponse.Result;
                                    else if (rowIndex == 2) // Company Division
                                        result.CompanyDivisions = microServiceResponse.Result;
                                    else if (rowIndex == 3) //Company Division Cost Center
                                        result.CompanyDivisionCostCenters = microServiceResponse.Result;
                                    else if (rowIndex == 4) // Company Documents
                                        result.CompanyDocuments = microServiceResponse.Result;
                                    else if (rowIndex == 5) //Company Email Template
                                        result.CompanyEmailTemplates = microServiceResponse.Result;
                                    else if (rowIndex == 6) //Company Expected Margin
                                        result.CompanyExpectedMargins = microServiceResponse.Result;
                                    else if (rowIndex == 7) //Company Invoice Info
                                        result.CompanyInvoiceInfo = microServiceResponse.Result;
                                    else if (rowIndex == 8) //Company Note
                                        result.CompanyNotes = microServiceResponse.Result;
                                    else if (rowIndex == 9) //Compny Payrolls
                                        result.CompanyPayrolls = microServiceResponse.Result;
                                    else if (rowIndex == 10) //Company Payroll Period
                                        result.CompanyPayrollPeriods = microServiceResponse.Result;
                                    //else if (rowIndex == 11) //Company Qualification
                                    //    result.CompanyQualifications = microServiceResponse.Result;
                                    else if (rowIndex == 11) //Company Taxes
                                        result.CompanyTaxes = microServiceResponse.Result;
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
