using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Core.Services
{
    public class CustomerContactService : ICustomerContactService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerContactService> _logger = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly ICustomerContactValidationService _validationService = null;

        private readonly ICustomerContactRepository _customerContactRepository = null;

        public CustomerContactService(IMapper mapper, IAppLogger<CustomerContactService> logger, ICustomerContactRepository customerContactRepository, 
                              ICustomerContactValidationService validationService,JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._customerContactRepository = customerContactRepository;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        public Response DeleteCustomerContact(string customerCode, int addressId, IList<DomainModel.Contact> customerContacts)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            object result = null;
            try
            {
                result = new List<DomainModel.CustomerAddress>();
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();

                Response deleteResponse = DeleteCustomerContacts(addressId, customerContacts.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList());
                if (deleteResponse.Code != ResponseType.Success.ToId())
                    return deleteResponse;
                else
                    result = deleteResponse.Result;

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerContacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response GetCustomerContact(string customerCode, int? addressId, DomainModel.Contact searchModel)
        {
            IList<ValidationMessage> validationMessages = null;
            IList<MessageDetail> errorMessages = null;
            ResponseType responseType = ResponseType.Success;
            IList<DomainModel.Contact> result = null;
            try
            {
                validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                // result = this._customerContactRepository.Search(customerCode, addressId, searchModel);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
                {
                    result = this._customerContactRepository.Search(customerCode, addressId, searchModel);
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.Message, searchModel);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                this._logger.LogError(MessageType.Error.ToId(), ex.Message, searchModel);
            }
            return new Response().ToPopulate(responseType, validationMessages, errorMessages, result);
        }

        public Response GetCustomerContact(string customerCode)
        {
            IList<ValidationMessage> validationMessages = null;
            IList<MessageDetail> errorMessages = null;
            ResponseType responseType = ResponseType.Success;
            IList<DomainModel.Contact> result = null;
            try
            {
                validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
                {
                    result = this._customerContactRepository.SearchContact(customerCode);
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.Message, customerCode);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                this._logger.LogError(MessageType.Error.ToId(), ex.Message, customerCode);
            }
            return new Response().ToPopulate(responseType, validationMessages, errorMessages, result);
        }

        public Response ModifyCustomerContact(string customerCode, int addressId, IList<DomainModel.Contact> customerContacts)
        {
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            List<DomainModel.Contact> result = null;
            List<string> responseStatuses = null;
            try
            {
                result = new List<DomainModel.Contact>();
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                responseStatuses = new List<string>();

                Response insertResult = AddCustomerContacts(addressId, customerContacts.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList());
                responseStatuses.Add(insertResult.Code);
                validationMessage.AddRange(insertResult.ValidationMessages?.ToList());
                errorMessages.AddRange(insertResult.Messages?.ToList());
                if (insertResult.Result != null)
                    result.AddRange((List<DomainModel.Contact>)insertResult.Result);

                Response updateResult = UpdateCustomerContacts(addressId, customerContacts.Where(c => c.RecordStatus.IsRecordStatusModified()).ToList());
                responseStatuses.Add(updateResult.Code);
                validationMessage.AddRange(updateResult.ValidationMessages?.ToList());
                errorMessages.AddRange(updateResult.Messages?.ToList());
                if (updateResult.Result != null)
                    result.AddRange((List<DomainModel.Contact>)updateResult.Result);

                Response deleteResult = DeleteCustomerContacts(addressId, customerContacts.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList());
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
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerContacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);

        }

        public Response SaveCustomerContact(string customerCode, int addressId, IList<DomainModel.Contact> customerContacts)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            object result = null;
            try
            {
                result = new List<DomainModel.Contact>();
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();

                Response insertResponse = AddCustomerContacts(addressId, customerContacts.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList());
                if (insertResponse.Code != ResponseType.Success.ToId())
                    return insertResponse;
                else
                    result = insertResponse.Result;
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerContacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);

        }

        private Response AddCustomerContacts(int addressId, IList<DomainModel.Contact> contactModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerContact> result = null;
            bool anyCustomerContactSaved = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DbModel.CustomerContact>();

                if (contactModels.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(contactModels), ValidationType.Add);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(contactModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var contact in contactModels)
                        {
                            try
                            {
                                var dbCustomerContact = _mapper.Map<DomainModel.Contact, DbModel.CustomerContact>(contact);
                                dbCustomerContact.Id = 0;
                                dbCustomerContact.CustomerAddressId = addressId;
                                dbCustomerContact.UpdateCount = 0;
                                dbCustomerContact.LastModification = DateTime.UtcNow;

                                this._customerContactRepository.Add(dbCustomerContact);
                                result.Add(dbCustomerContact);
                                anyCustomerContactSaved = true;
                            }
                            catch (SqlException sqlE)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.Message, contactModels);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
                            }
                        }
                    }

                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerContactSaved)
                        responseType = ResponseType.PartiallySuccess;
                }

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), contactModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.Contact>>(result).ToList());

        }

        private Response UpdateCustomerContacts(int addressId, IList<DomainModel.Contact> contactModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerContact> result = new List<DbModel.CustomerContact>();
            bool anyCustomerContactUpdated = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                if (addressId > 0 && contactModels?.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(contactModels), ValidationType.Update);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(contactModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var contact in contactModels)
                        {
                            try
                            {
                                var custContactEntity = this._customerContactRepository.FindBy(c => c.CustomerAddressId == addressId && c.Id == contact.ContactId).FirstOrDefault();

                                if (custContactEntity != null)
                                {
                                    if (custContactEntity.UpdateCount == contact.UpdateCount)
                                    {
                                        custContactEntity.Salutation = contact.Salutation;
                                        custContactEntity.Position = contact.Position;
                                        custContactEntity.ContactName = contact.ContactPersonName;
                                        custContactEntity.TelephoneNumber = contact.Landline;
                                        custContactEntity.FaxNumber = contact.Fax;
                                        custContactEntity.MobileNumber = contact.Mobile;
                                        custContactEntity.EmailAddress = contact.Email;
                                        custContactEntity.OtherContactDetails = contact.OtherDetail;
                                        custContactEntity.LastModification = DateTime.UtcNow;
                                        custContactEntity.ModifiedBy = contact.ModifiedBy;
                                        custContactEntity.UpdateCount = custContactEntity.UpdateCount.CalculateUpdateCount();

                                        this._customerContactRepository.Update(custContactEntity);
                                        result.Add(custContactEntity);
                                        anyCustomerContactUpdated = true;
                                    }
                                    else
                                    {
                                        responseType = ResponseType.Error;
                                        validationMessage.Add(new ValidationMessage(contact, new List<MessageDetail>() {
                                        new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_UpdateCountMismatch.ToId(),this._MessageDescriptions[MessageType.Customer_Contact_UpdateCountMismatch.ToId()].ToString())}));
                                    }

                                }
                                else
                                {
                                    responseType = ResponseType.Validation;
                                    validationMessage.Add(new ValidationMessage(contact, new List<MessageDetail>() {
                                    new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_InvalidContactId.ToId(),this._MessageDescriptions[MessageType.Customer_Contact_InvalidContactId.ToId()].ToString())
                                }));
                                }

                            }
                            catch (Exception ex)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), ex.ToFullString(), contact);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                            }
                        }
                    }
                }
                else
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(addressId, new List<MessageDetail>()
                                {
                                    new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_InvalidAddressId.ToId(), this._MessageDescriptions[MessageType.Customer_Contact_InvalidAddressId.ToId()].ToString())
                                }));
                }

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerContactUpdated)
                    responseType = ResponseType.PartiallySuccess;

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), contactModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), _MessageDescriptions[responseType.ToId()].ToString()));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.Contact>>(result).ToList());
        }

        private Response DeleteCustomerContacts(int addressId, IList<DomainModel.Contact> contactModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerContact> result = null;
            bool anyCustomerContactDeleted = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DbModel.CustomerContact>();

                if (contactModels.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(contactModels), ValidationType.Delete);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(contactModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {

                        foreach (var custContact in contactModels)
                        {
                            try
                            {
                                var custContactEntity = this._customerContactRepository.FindBy(c => c.CustomerAddressId == addressId && c.Id == custContact.ContactId).FirstOrDefault();

                                this._customerContactRepository.Delete(custContactEntity);
                                result.Add(custContactEntity);
                                anyCustomerContactDeleted = true;
                            }
                            catch (Exception ex)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), ex.ToFullString(), contactModels);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                            }
                        }
                    }

                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerContactDeleted)
                        responseType = ResponseType.PartiallySuccess;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), contactModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.Contact>>(result).ToList());
        }
    }
}
