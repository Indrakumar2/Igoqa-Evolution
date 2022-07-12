using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Web.Gateway.Models;
using Evolution.Web.Gateway.Models.Customers;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evolution.Web.Gateway.Aggregators
{
    public class CustomerAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            CustomerAggregatorResponse result;
            List<AddressDetail> addresses = null;
            List<Contact> contacts = null;

            try
            {
                result = new CustomerAggregatorResponse();

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
                                        if (rowIndex == 0) // Customer Detail
                                        {
                                            var custs = ((Newtonsoft.Json.Linq.JContainer)microServiceResponse.Result);
                                            result.Detail = custs.Count > 0 ? custs[0] : null;
                                        }
                                        else if (rowIndex == 1) //Customer Address
                                            addresses = ((Newtonsoft.Json.Linq.JArray)microServiceResponse.Result).ToObject<List<AddressDetail>>();
                                        else if (rowIndex == 2) // Customer Contact
                                            contacts = ((Newtonsoft.Json.Linq.JArray)microServiceResponse.Result).ToObject<List<Contact>>();
                                        else if (rowIndex == 3) //Customer Account Reference
                                            result.AccountReferences = microServiceResponse.Result;
                                        else if (rowIndex == 4) // Assignment Reference
                                            result.AssignmentReferences = microServiceResponse.Result;
                                        else if (rowIndex == 5) //Document
                                            result.Documents = microServiceResponse.Result;
                                        else if (rowIndex == 6) //Note
                                            result.Notes = microServiceResponse.Result;
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
                    result.Addresses = MergeAddressAndContact(addresses, contacts);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToFullString());
                throw;
                //TODO : Logg into logger
            }
            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(result)) }));
        }

        private IList<AddressDetail> MergeAddressAndContact(List<AddressDetail> addresses, List<Contact> contacts)
        {
            if (addresses != null && addresses.Count > 0 && contacts != null && contacts.Count > 0)
            {
                addresses.ForEach(x =>
                {
                    x.Contacts = contacts.Where(x1 => x1.CustomerAddressId == x.AddressId).ToList();
                });
            }

            return addresses;
        }
    }
}
