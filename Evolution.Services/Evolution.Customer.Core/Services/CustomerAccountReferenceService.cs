using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.Customer.Domain.Models.Customers;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

//TODO: Removed error list from the return type  when an error occurs
namespace Evolution.Customer.Core.Services
{
    public class CustomerAccountReferenceService : ICustomerAccountReferenceService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerAccountReferenceService> _logger = null;
        private readonly ICustomerAccountReferenceRepository _customerAccountReferenceRepository = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly ICustomerAccountReferenceValidationService _validationService = null;
        private readonly JObject _MessageDescriptions = null;

        public CustomerAccountReferenceService(IMapper mapper, IAppLogger<CustomerAccountReferenceService> logger, ICustomerRepository customerRepository, 
                                        ICustomerAccountReferenceRepository customerAccountReferenceRepository, 
                             ICustomerAccountReferenceValidationService validationService,JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._customerAccountReferenceRepository = customerAccountReferenceRepository;
            this._customerRepository = customerRepository;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        public Response GetCustomerAccountReference(string customerCode, CustomerCompanyAccountReference searchModel)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<DomainModel.CustomerCompanyAccountReference> result = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                // result = this._customerAccountReferenceRepository.Search(customerCode, searchModel);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
                {
                    result = this._customerAccountReferenceRepository.Search(customerCode, searchModel);
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(sqlE.ErrorCode.ToString(),  sqlE.Message, searchModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, searchModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, errorMessages, result);
        }

        public Response ModifyCustomerAccountReference(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> customerAccountReferences)
        {
            Response newCustAccRefResponse = null;
            Response updateCustAccRefResponse = null;
            Response deleteCustAccRefResponse = null;
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            List<DomainModel.CustomerCompanyAccountReference> result = null;

            try
            {
                newCustAccRefResponse = new Response().ToPopulate(ResponseType.Success);
                updateCustAccRefResponse = new Response().ToPopulate(ResponseType.Success);
                deleteCustAccRefResponse = new Response().ToPopulate(ResponseType.Success);
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                result = new List<DomainModel.CustomerCompanyAccountReference>();

                var customerAccountRefToBeInsertes = customerAccountReferences.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList();
                if (customerAccountRefToBeInsertes.Count > 0)
                    newCustAccRefResponse = this.AddCustomerAccountRefs(customerCode, customerAccountRefToBeInsertes);

                var customerAccountRefToBeUpdate = customerAccountReferences.Where(c => c.RecordStatus.IsRecordStatusModified()).ToList();
                if (customerAccountRefToBeUpdate.Count > 0)
                    updateCustAccRefResponse = this.UpdateCustomerAccountRefs(customerCode, customerAccountRefToBeUpdate);


                var deleteAccountRefToBeDeleted = customerAccountReferences.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (deleteAccountRefToBeDeleted.Count > 0)
                    deleteCustAccRefResponse = this.DeleteCustomerAccountRefs(customerCode, deleteAccountRefToBeDeleted);

                if (newCustAccRefResponse.ValidationMessages != null && newCustAccRefResponse.ValidationMessages.Count > 0)
                    validationMessage.AddRange(newCustAccRefResponse.ValidationMessages);

                if (newCustAccRefResponse.Messages != null && newCustAccRefResponse.Messages.Count > 0)
                    errorMessages.AddRange(newCustAccRefResponse.Messages);

                if (newCustAccRefResponse.Result != null && ((List<DomainModel.CustomerCompanyAccountReference>)newCustAccRefResponse.Result).Count > 0)
                    result.AddRange((List<DomainModel.CustomerCompanyAccountReference>)newCustAccRefResponse.Result);

                if (updateCustAccRefResponse.ValidationMessages != null && updateCustAccRefResponse.ValidationMessages.Count > 0)
                    validationMessage.AddRange(updateCustAccRefResponse.ValidationMessages);

                if (updateCustAccRefResponse.Messages != null && updateCustAccRefResponse.Messages.Count > 0)
                    errorMessages.AddRange(updateCustAccRefResponse.Messages);

                if (updateCustAccRefResponse.Result != null && ((List<DomainModel.CustomerCompanyAccountReference>)updateCustAccRefResponse.Result).Count > 0)
                    result.AddRange((List<DomainModel.CustomerCompanyAccountReference>)updateCustAccRefResponse.Result);

                if (deleteCustAccRefResponse.Messages != null && deleteCustAccRefResponse.Messages.Count > 0)
                    errorMessages.AddRange(deleteCustAccRefResponse.Messages);

                if (validationMessage.Count > 0)
                    responseType = ResponseType.Validation;

                if (errorMessages.Count > 0)
                    responseType = ResponseType.Error;

                if (newCustAccRefResponse.Code == ResponseType.PartiallySuccess.ToId() || updateCustAccRefResponse.Code == ResponseType.PartiallySuccess.ToId())
                    responseType = ResponseType.PartiallySuccess;

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                _logger.LogError(responseType.ToId(), ex.Message, customerAccountReferences);
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response SaveCustomerAccountReference(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> customerAccountReferences)
        {
            return AddCustomerAccountRefs(customerCode, customerAccountReferences.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList());
        }

        private Response AddCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models)
        {
            return this._customerAccountReferenceRepository.AddCustomerAccountRefs(customerCode, models);
        }

        private Response UpdateCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models)
        {
            return this._customerAccountReferenceRepository.UpdateCustomerAccountRefs(customerCode, models);
        }

        public Response DeleteCustomerAccountReference(string customerCode, IList<CustomerCompanyAccountReference> customerAccountReferences)
        {
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            Response deleteCustAccRefResponse = null;
            List<DomainModel.CustomerCompanyAccountReference> result = null;
            try
            {
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                deleteCustAccRefResponse = new Response().ToPopulate(ResponseType.Success);
                var customerAccountRefToBeDeleted = customerAccountReferences.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (customerAccountRefToBeDeleted.Count > 0)
                    deleteCustAccRefResponse = this.DeleteCustomerAccountRefs(customerCode, customerAccountRefToBeDeleted);

                if (deleteCustAccRefResponse.Messages != null && deleteCustAccRefResponse.Messages.Count > 0)
                    errorMessages.AddRange(deleteCustAccRefResponse.Messages);

                if (deleteCustAccRefResponse.Result != null && ((List<DomainModel.CustomerCompanyAccountReference>)deleteCustAccRefResponse.Result).Count > 0)
                    result.AddRange((List<DomainModel.CustomerCompanyAccountReference>)deleteCustAccRefResponse.Result);

                if (errorMessages.Count > 0)
                    responseType = ResponseType.Error;

                if (deleteCustAccRefResponse.Code == ResponseType.PartiallySuccess.ToId())
                    responseType = ResponseType.PartiallySuccess;
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                _logger.LogError(responseType.ToId(), ex.Message, customerAccountReferences);
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        private Response DeleteCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> customerAccountReferences)
        {
            return this._customerAccountReferenceRepository.DeleteCustomerAccountRefs(customerCode, customerAccountReferences);

        }
    }
}
