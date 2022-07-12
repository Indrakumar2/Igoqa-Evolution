using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Web.Gateway.Interfaces;
using Evolution.Web.Gateway.Models;
using Evolution.Web.Gateway.Models.Customers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Evolution.Web.Gateway.Decomposers.Processors
{
    public class CustomerProcessor : ICustomerProcessor
    {
        ICustomerClient _client;

        public CustomerProcessor(ICustomerClient client)
        {
            _client = client;
        }

        public Response Process(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            ResponseType responseType = ResponseType.Success;
            Response saveResponse = null;
            List<MessageDetail> messages = null;
            List<ValidationMessage> validationMessages = null;
            List<string> responseCodes = null;
            try
            {
                messages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                responseCodes = new List<string>();
                saveResponse = new Response().ToPopulate(ResponseType.Success);

                saveResponse = this.SaveCustomer(request, type);
                this.MergeResponseMesseages(saveResponse, ref messages, ref validationMessages, ref responseCodes);

                saveResponse = this.SaveCustomerAddress(request, type);
                this.MergeResponseMesseages(saveResponse, ref messages, ref validationMessages, ref responseCodes);

                saveResponse = this.SaveCustomerAccountRef(request, type);
                this.MergeResponseMesseages(saveResponse, ref messages, ref validationMessages, ref responseCodes);

                saveResponse = this.SaveCustomerAssignmentRef(request, type);
                this.MergeResponseMesseages(saveResponse, ref messages, ref validationMessages, ref responseCodes);

                saveResponse = this.SaveCustomerDocument(request, type);
                this.MergeResponseMesseages(saveResponse, ref messages, ref validationMessages, ref responseCodes);

                saveResponse = this.SaveCustomerNote(request, type);
                this.MergeResponseMesseages(saveResponse, ref messages, ref validationMessages, ref responseCodes);
            }
            catch (Exception ex)
            {
                //TODO : Add logger
                responseCodes.Add(ResponseType.Exception.ToId());
                messages.Add(new MessageDetail(ResponseType.Error.ToId(), ex.Message + Environment.NewLine + ex.InnerException?.Message));
            }

            if (!responseCodes.Any(x => x != ResponseType.Success.ToId()))
                responseType = ResponseType.Success;
            else if (!responseCodes.Any(x => x == ResponseType.PartiallySuccess.ToId()))
                responseType = ResponseType.PartiallySuccess;
            else
                responseType = ResponseType.Error;

            return new Response().ToPopulate(responseType, validationMessages,messages,null);
        }

        private Response SaveCustomer(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            var saveResponse = new Response().ToPopulate(ResponseType.Success);
            var details = request.Select(x => x.Detail).ToList();
            if (details != null && details.Count > 0)
            {
                saveResponse = this._client.SaveCustomer(details, type).Result;
                if (saveResponse.Code == ResponseType.Success.ToId() || saveResponse.Code == ResponseType.PartiallySuccess.ToId())
                {
                    List<CustomerDetail> customers = null;
                    if (type == HttpMethod.Put)
                        customers = new List<CustomerDetail>() { request.FirstOrDefault().Detail };
                    else
                        customers = saveResponse.Result != null ? JsonConvert.DeserializeObject<List<CustomerDetail>>(saveResponse.Result.ToString()) : null;

                    if (customers != null && customers.Count > 0)
                        this.AssignCustomerCode(request, customers);
                }
            }

            return saveResponse;
        }

        private Response SaveCustomerAddress(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            var saveResponse = new Response().ToPopulate(ResponseType.Success);
            var addresses = request.SelectMany(x => x.Addresses).ToList();
            if (addresses != null && addresses.Count > 0)
                saveResponse = this._client.SaveCustomerAddress(addresses, type).Result;

            return saveResponse;
        }

        private Response SaveCustomerAccountRef(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            var saveResponse = new Response().ToPopulate(ResponseType.Success);
            var accountRefs = request.SelectMany(x => x.AccountReferences).ToList();
            if (accountRefs != null && accountRefs.Count > 0)
                saveResponse = this._client.SaveCustomerAccountRef(accountRefs, type).Result;
            return saveResponse;
        }

        private Response SaveCustomerAssignmentRef(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            var saveResponse = new Response().ToPopulate(ResponseType.Success);
            var assignmentRefs = request.SelectMany(x => x.AssignmentReferences).ToList();
            if (assignmentRefs != null && assignmentRefs.Count > 0)
                saveResponse = this._client.SaveCustomerAssignmentRef(assignmentRefs, type).Result;
            return saveResponse;
        }

        private Response SaveCustomerDocument(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            var saveResponse = new Response().ToPopulate(ResponseType.Success);
            var documents = request.SelectMany(x => x.Documents).ToList();
            if (documents != null && documents.Count > 0)
                saveResponse = this._client.SaveCustomerDocument(documents, type).Result;
            return saveResponse;
        }

        private Response SaveCustomerNote(IList<CustomerDecompositionRequest> request, HttpMethod type)
        {
            var saveResponse = new Response().ToPopulate(ResponseType.Success);
            var notes = request.SelectMany(x => x.Notes).ToList();
            if (notes != null && notes.Count > 0)
                saveResponse = this._client.SaveCustomerNote(notes, type).Result;
            return saveResponse;
        }

        private void AssignCustomerCode(IList<CustomerDecompositionRequest> request, IList<CustomerDetail> customers)
        {   
            request.ToList().ForEach(x =>
            {
                var customerCode = customers?.FirstOrDefault(f => f.MIIWAId == x.Detail.MIIWAId).CustomerCode;
                x.AccountReferences?.ToList().ForEach(x1 => { x1.CustomerCode = customerCode; });
                x.Addresses?.ToList().ForEach(x1 => { x1.CustomerCode = customerCode; });
                x.AssignmentReferences?.ToList().ForEach(x1 => { x1.CustomerCode = customerCode; });
                x.Documents?.ToList().ForEach(x1 => { x1.CustomerCode = customerCode; });
                x.Notes?.ToList().ForEach(x1 => { x1.CustomerCode = customerCode; });
            });
        }

        private void MergeResponseMesseages(Response response, ref List<MessageDetail> messages, ref List<ValidationMessage> validationMessages, ref List<string> responseCodes)
        {
            if (messages == null)
                messages = new List<MessageDetail>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (responseCodes == null)
                responseCodes = new List<string>();

            if (response != null)
            {
                responseCodes.Add(response.Code);

                if (response.Messages != null && response.Messages.Count > 0)
                    messages.AddRange(response.Messages);

                if (response.ValidationMessages != null && response.ValidationMessages.Count > 0)
                    validationMessages.AddRange(response.ValidationMessages);
            }
        }
    }
}
