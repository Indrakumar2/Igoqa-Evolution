using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.DbRepository.Interfaces.Master;
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

namespace Evolution.Customer.Core.Services
{
    public class CustomerAssignmentReferenceService : ICustomerAssignmentReferenceService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerAssignmentReferenceService> _logger = null;
        private readonly ICustomerAssignmentReferenceValidationService _validationService = null;
        private readonly JObject _MessageDescriptions = null;

        private readonly ICustomerAssignmentReferenceRepository _customerAssignmentReferenceRepository = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly IDataRepository _dataRepository = null;

        public CustomerAssignmentReferenceService(IMapper mapper, IAppLogger<CustomerAssignmentReferenceService> logger, ICustomerRepository customerRepository, 
            IDataRepository dataRepository, ICustomerAssignmentReferenceRepository customerAssignmentReferenceRepository, 
            ICustomerAssignmentReferenceValidationService validationService,JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._customerAssignmentReferenceRepository = customerAssignmentReferenceRepository;
            this._customerRepository = customerRepository;
            this._dataRepository = dataRepository;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        public Response DeleteCustomerAssignmentReference(string customerCode, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            object result = null;
            try
            {
                result = new List<DomainModel.CustomerAssignmentReference>();
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();

                var dbCustomer = _customerRepository.FindBy(x => x.Code == customerCode).FirstOrDefault();
                if (dbCustomer == null)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(customerCode, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidCustomerCode.ToId(), _MessageDescriptions[MessageType.CustAddr_InvalidCustomerCode.ToId()].ToString()) }));
                }
                else
                {
                    Response deleteResponse = DeleteCustomerAssignmentReferences(dbCustomer.Id, customerAssignmentReferences.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList());
                    if (deleteResponse.Code != ResponseType.Success.ToId())
                        return deleteResponse;
                    else
                        result = deleteResponse.Result;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response GetCustomerAssignmentReference(string customerCode, DomainModel.CustomerAssignmentReference searchModel)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<DomainModel.CustomerAssignmentReference> result = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                // result = this._customerAssignmentReferenceRepository.Search(customerCode, searchModel);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
                {
                    result = this._customerAssignmentReferenceRepository.Search(customerCode, searchModel);
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.Message, searchModel);
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

        public Response ModifyCustomerAssignmentReference(string customerCode, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences)
        {
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            List<DomainModel.CustomerAssignmentReference> result = null;
            List<string> responseStatuses = null;

            try
            {
                result = new List<DomainModel.CustomerAssignmentReference>();
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                responseStatuses = new List<string>();

                var dbCustomer = _customerRepository.FindBy(x => x.Code == customerCode).FirstOrDefault();
                if (dbCustomer == null)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(customerCode, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidCustomerCode.ToId(), _MessageDescriptions[MessageType.CustAddr_InvalidCustomerCode.ToId()].ToString()) }));
                }
                else
                {
                    Response insertResult = AddCustomerAssignmentReferences(dbCustomer.Id, customerAssignmentReferences.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList());
                    responseStatuses.Add(insertResult.Code);
                    validationMessage.AddRange(insertResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(insertResult.Messages?.ToList());
                    if (insertResult.Result != null)
                        result.AddRange((List<DomainModel.CustomerAssignmentReference>)insertResult.Result);

                    Response updateResult = UpdateCustomerAssignmentReferences(dbCustomer.Id, customerAssignmentReferences.Where(c => c.RecordStatus.IsRecordStatusModified()).ToList());
                    responseStatuses.Add(updateResult.Code);
                    validationMessage.AddRange(updateResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(updateResult.Messages?.ToList());
                    if (updateResult.Result != null)
                        result.AddRange((List<DomainModel.CustomerAssignmentReference>)updateResult.Result);

                    Response deleteResult = DeleteCustomerAssignmentReferences(dbCustomer.Id, customerAssignmentReferences.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList());
                    responseStatuses.Add(deleteResult.Code);
                    validationMessage.AddRange(deleteResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(deleteResult.Messages?.ToList());

                    if (responseStatuses.Count(x => x == ResponseType.Success.ToId()) == 3)
                        responseType = ResponseType.Success;
                    else if (responseStatuses.Any(x => x == ResponseType.PartiallySuccess.ToId()))
                        responseType = ResponseType.PartiallySuccess;
                    else
                        responseType = ResponseType.Error;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.GetBaseException().Message, customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response SaveCustomerAssignmentReference(string customerCode, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            object result = null;
            try
            {
                result = new List<DomainModel.CustomerAssignmentReference>();
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();

                var dbCustomer = _customerRepository.FindBy(x => x.Code == customerCode).FirstOrDefault();
                if (dbCustomer == null)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(customerCode, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_InvalidCustomerCode.ToId(), _MessageDescriptions[MessageType.Customer_AssRef_InvalidCustomerCode.ToId()].ToString()) }));
                }
                else
                {
                    var response = AddCustomerAssignmentReferences(dbCustomer.Id, customerAssignmentReferences.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList());
                    if (response.Code != ResponseType.Success.ToId())
                        return response;
                    else
                        result = response.Result;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        private Response AddCustomerAssignmentReferences(int customerId, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerAssignmentReferenceType> result = null;
            bool anyCustomerAssignmentReferencSaved = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DbModel.CustomerAssignmentReferenceType>();

                if (customerAssignmentReferences.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(customerAssignmentReferences), ValidationType.Add);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(customerAssignmentReferences, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var assRef in customerAssignmentReferences)
                        {
                            try
                            {
                                int? AssignmentRefId = this._dataRepository.FindBy(x => x.IsActive == true && x.MasterDataTypeId == 30 && x.Name == assRef.AssignmentRefType).FirstOrDefault()?.Id;
                                if (AssignmentRefId.HasValue)
                                {
                                    if (this._customerAssignmentReferenceRepository.Exists(x => x.AssignmentReferenceId == AssignmentRefId && x.CustomerId == customerId).Result)
                                    {
                                        responseType = ResponseType.Validation;
                                        validationMessage.Add(new ValidationMessage(assRef, new List<MessageDetail>()
                                    {
                                        new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_AlreadyExists.ToId(),this._MessageDescriptions[MessageType.Customer_AssRef_AlreadyExists.ToId()].ToString())
                                    }));
                                    }
                                    else
                                    {
                                        var dbCustomerAssignmentReference = _mapper.Map<DomainModel.CustomerAssignmentReference, DbModel.CustomerAssignmentReferenceType>(assRef);
                                        dbCustomerAssignmentReference.Id = 0;
                                        dbCustomerAssignmentReference.CustomerId = customerId;
                                        dbCustomerAssignmentReference.AssignmentReferenceId = AssignmentRefId.Value;
                                        dbCustomerAssignmentReference.UpdateCount = 0;

                                        this._customerAssignmentReferenceRepository.Add(dbCustomerAssignmentReference);
                                        result.Add(dbCustomerAssignmentReference);
                                        anyCustomerAssignmentReferencSaved = true;
                                    }
                                }
                                else
                                {
                                    responseType = ResponseType.Error;
                                    validationMessage.Add(new ValidationMessage(assRef, new List<MessageDetail>()
                                    {
                                        new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_InvalidAssignmentReferenceType.ToId(),this._MessageDescriptions[MessageType.Customer_AssRef_InvalidAssignmentReferenceType.ToId()].ToString())
                                    }));
                                }
                            }
                            catch (Exception sqlE)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), sqlE.ToFullString(), assRef);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.GetBaseException().Message));
                            }
                        }
                    }
                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerAssignmentReferencSaved)
                        responseType = ResponseType.PartiallySuccess;
                }

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.CustomerAssignmentReference>>(result).ToList());

        }

        private Response UpdateCustomerAssignmentReferences(int customerId, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerAssignmentReferenceType> result = null;
            bool anyCustomerAddressUpdated = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DbModel.CustomerAssignmentReferenceType>();

                if (customerId > 0 && customerAssignmentReferences?.Count>0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(customerAssignmentReferences), ValidationType.Update);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(customerAssignmentReferences, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var assReff in customerAssignmentReferences)
                        {
                            try
                            { 
                                var custAssignmentReferenceEntity = this._customerAssignmentReferenceRepository.FindBy(c => c.CustomerId == customerId && c.Id == assReff.CustomerAssignmentReferenceId).FirstOrDefault();

                                if (custAssignmentReferenceEntity != null)
                                {
                                    if (custAssignmentReferenceEntity.UpdateCount == assReff.UpdateCount)
                                    {
                                        int? AssignmentRefId = this._dataRepository.FindBy(x => x.IsActive == true && x.MasterDataTypeId == 30 && x.Name == assReff.AssignmentRefType).FirstOrDefault()?.Id;

                                        if (AssignmentRefId.HasValue)
                                        {
                                            custAssignmentReferenceEntity.AssignmentReferenceId = AssignmentRefId.Value;
                                            custAssignmentReferenceEntity.LastModification = DateTime.UtcNow;
                                            custAssignmentReferenceEntity.ModifiedBy = assReff.ModifiedBy;
                                            custAssignmentReferenceEntity.UpdateCount = custAssignmentReferenceEntity.UpdateCount.CalculateUpdateCount();
                                            this._customerAssignmentReferenceRepository.Update(custAssignmentReferenceEntity);
                                            result.Add(custAssignmentReferenceEntity);
                                            anyCustomerAddressUpdated = true;
                                        }
                                        else
                                        {
                                            responseType = ResponseType.Error;
                                            validationMessage.Add(new ValidationMessage(assReff, new List<MessageDetail>(){
                                             new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_InvalidAssignmentReferenceType.ToId(),this._MessageDescriptions[MessageType.Customer_AssRef_InvalidAssignmentReferenceType.ToId()].ToString())
                                            }));
                                        }
                                    }
                                    else
                                    {
                                        responseType = ResponseType.Error;
                                        validationMessage.Add(new ValidationMessage(assReff, new List<MessageDetail>()
                                        {
                                            new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_UpdateCountMismatch.ToId(),this._MessageDescriptions[MessageType.Customer_AssRef_UpdateCountMismatch.ToId()].ToString())
                                        }));
                                    }
                                }
                                else
                                {
                                    responseType = ResponseType.Error;
                                    validationMessage.Add(new ValidationMessage(assReff, new List<MessageDetail>() {
                                        new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_InvalidAssignmentReferenceId.ToId(),this._MessageDescriptions[MessageType.Customer_AssRef_InvalidAssignmentReferenceId.ToId()].ToString())
                                    }));
                                }

                            }
                            catch (SqlException sqlE)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.ToFullString(), assReff);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.GetBaseException().Message));
                            }
                        }
                    }
                }
                else
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(customerId, new List<MessageDetail>()
                    {
                        new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidCustomerCode.ToId(), this._MessageDescriptions[MessageType.CustAddr_InvalidCustomerCode.ToId()].ToString())
                    }));
                }

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerAddressUpdated)
                    responseType = ResponseType.PartiallySuccess;

            } 
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.CustomerAssignmentReference>>(result).ToList());
        }

        private Response DeleteCustomerAssignmentReferences(int customerId, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            bool anyCustomerAssignmentReferenceDeleted = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                if (customerAssignmentReferences.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(customerAssignmentReferences), ValidationType.Delete);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(customerAssignmentReferences, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var custAssiRef in customerAssignmentReferences)
                        {
                            try
                            {
                                var custAssignmentRefTypeEntity = this._customerAssignmentReferenceRepository.FindBy(c => c.CustomerId == customerId && c.Id == custAssiRef.CustomerAssignmentReferenceId).FirstOrDefault();
                                if (custAssignmentRefTypeEntity != null)
                                {
                                    this._customerAssignmentReferenceRepository.Delete(custAssignmentRefTypeEntity);
                                    anyCustomerAssignmentReferenceDeleted = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), ex.ToFullString(), custAssiRef);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
                            }
                        }
                    }

                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerAssignmentReferenceDeleted)
                        responseType = ResponseType.PartiallySuccess;
                }
            } 
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, null);
        }
    }
}
