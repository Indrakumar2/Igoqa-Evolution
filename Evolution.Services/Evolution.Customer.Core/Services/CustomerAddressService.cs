using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
//using Evolution.Customer.Domain.Interfaces.Loggers;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.Customer.Domain.Models.Validations;
using Evolution.DbRepository.Interfaces.Master;
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
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerAddressService> _logger = null;
        private readonly ICustomerAddressValidationService _validationService = null;
        private readonly JObject _MessageDescriptions = null;

        private readonly ICustomerRepository _customerRepository = null;
        private readonly ICustomerAddressRepository _custAddressRepository = null;
        private readonly ICityRepository _cityRepository = null;

        public CustomerAddressService(IMapper mapper, IAppLogger<CustomerAddressService> logger, ICustomerRepository customerRepository,
                                      ICustomerAddressRepository customerAddressRepository, ICityRepository cityRepository,
                                      ICustomerAddressValidationService validationService, JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._customerRepository = customerRepository;
            this._custAddressRepository = customerAddressRepository;
            this._cityRepository = cityRepository;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        public Response DeleteCustomerAddress(string customerCode, IList<DomainModel.CustomerAddress> customerAddresses)
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

                var dbCustomer = _customerRepository.FindBy(x => x.Code == customerCode).FirstOrDefault();
                if (dbCustomer == null)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(customerCode, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidCustomerCode.ToId(), _MessageDescriptions[MessageType.CustAddr_InvalidCustomerCode.ToId()].ToString()) }));
                }
                else
                {
                    Response deleteResponse = DeleteCustomerAddress(dbCustomer.Id, customerAddresses.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList());
                    if (deleteResponse.Code != ResponseType.Success.ToId())
                        return deleteResponse;
                    else
                        result = deleteResponse.Result;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerAddresses);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response GetCustomerAddress(DomainModel.CustomerAddress searchModel)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<DomainModel.CustomerAddress> result = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
                {
                    result = this._custAddressRepository.Search(searchModel);
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.GetBaseException().Message, searchModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.GetBaseException().Message));
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.GetBaseException().Message, searchModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }

            return new Response().ToPopulate(responseType, errorMessages, result);
        }

        public Response GetCustomerAddress(string customerCode)
        {
            IList<ValidationMessage> validationMessages = null;
            IList<MessageDetail> errorMessages = null;
            ResponseType responseType = ResponseType.Success;
            IList<DomainModel.CustomerAddress> result = null;
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
                    result = this._custAddressRepository.SearchAddress(customerCode);
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


        public Response ModifyCustomerAddress(string customerCode, IList<DomainModel.CustomerAddress> customerAddresses)
        {
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            List<DomainModel.CustomerAddress> result = null;
            List<string> responseStatuses = null;
            try
            {
                result = new List<DomainModel.CustomerAddress>();
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
                    Response insertResult = AddCustomerAddress(dbCustomer, customerAddresses.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList());
                    responseStatuses.Add(insertResult.Code);
                    validationMessage.AddRange(insertResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(insertResult.Messages?.ToList());
                    if (insertResult.Result != null)
                        result.AddRange((List<DomainModel.CustomerAddress>)insertResult.Result);

                    Response updateResult = UpdateCustomerAddress(dbCustomer.Id, customerAddresses.Where(c => c.RecordStatus.IsRecordStatusModified()).ToList());
                    responseStatuses.Add(updateResult.Code);
                    validationMessage.AddRange(updateResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(updateResult.Messages?.ToList());
                    if (updateResult.Result != null)
                        result.AddRange((List<DomainModel.CustomerAddress>)updateResult.Result);

                    Response deleteResult = DeleteCustomerAddress(dbCustomer.Id, customerAddresses.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList());
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
                this._logger.LogError(responseType.ToId(), ex.GetBaseException().Message, customerAddresses);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response SaveCustomerAddress(string customerCode, IList<DomainModel.CustomerAddress> customerAddresses)
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

                var dbCustomer = _customerRepository.FindBy(x => x.Code == customerCode).FirstOrDefault();
                if (dbCustomer == null)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(customerCode, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidCustomerCode.ToId(), _MessageDescriptions[MessageType.CustAddr_InvalidCustomerCode.ToId()].ToString()) }));
                }
                else
                {
                    var response = AddCustomerAddress(dbCustomer, customerAddresses.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList());
                    if (response.Code != ResponseType.Success.ToId())
                        return response;
                    else
                        result = response.Result;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.GetBaseException().Message, customerAddresses);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        private Response AddCustomerAddress(DbModel.Customer dbCustomer, IList<DomainModel.CustomerAddress> addressModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerAddress> result = null;
            bool anyCustomerAddressSaved = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DbModel.CustomerAddress>();

                if (addressModels.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(addressModels), ValidationType.Add);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(addressModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        var cities = _cityRepository.FindBy(x => addressModels.Any(x1 => x1.City == x.Name && x1.County == x.County.Name)).ToList();
                        foreach (var address in addressModels)
                        {
                            try
                            {
                                if (dbCustomer.CustomerAddress.Count > 0 && dbCustomer.CustomerAddress.Any(x1 => x1.Address == address.Address))
                                {
                                    responseType = ResponseType.Validation;
                                    validationMessage.Add(new ValidationMessage(address, new List<MessageDetail>()
                                    {
                                        new MessageDetail(ModuleType.Customer, MessageType.CustAddr_ExisitingAddress.ToId(),this._MessageDescriptions[MessageType.CustAddr_ExisitingAddress.ToId()].ToString())
                                    }));
                                }
                                else
                                {
                                    var dbCustomerAddress = _mapper.Map<DomainModel.CustomerAddress, DbModel.CustomerAddress>(address);
                                    dbCustomerAddress.Id = 0;
                                    dbCustomerAddress.CustomerId = dbCustomer.Id;
                                    //dbCustomerAddress.CityId = cities.FirstOrDefault(x1 => x1.Name == address.City)?.Id;
                                    dbCustomerAddress.CityId = address?.CityId;
                                    dbCustomerAddress.LastModification = null;
                                    this._custAddressRepository.Add(dbCustomerAddress);
                                    result.Add(dbCustomerAddress);
                                    anyCustomerAddressSaved = true;
                                }

                            }
                            catch (Exception sqlE)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), sqlE.ToFullString(), addressModels);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.GetBaseException().Message));
                            }
                        }
                    }
                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerAddressSaved)
                        responseType = ResponseType.PartiallySuccess;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), addressModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.CustomerAddress>>(result).ToList());
        }

        private Response UpdateCustomerAddress(int customerId, IList<DomainModel.CustomerAddress> addressModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerAddress> result = new List<DbModel.CustomerAddress>();
            bool anyCustomerAddressUpdated = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                if (customerId > 0 && addressModels?.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(addressModels), ValidationType.Update);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(addressModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        // Fetch City Id from City Name
                        var cities = _cityRepository.FindBy(x => addressModels.Any(x1 => x1.City == x.Name)).ToList();

                        foreach (var address in addressModels)
                        {
                            try
                            {

                                var custAddressEntity = this._custAddressRepository.FindBy(c => c.CustomerId == customerId && c.Id == address.AddressId).FirstOrDefault();
                                if (custAddressEntity != null)
                                {
                                    if (custAddressEntity.UpdateCount == address.UpdateCount)
                                    {
                                        custAddressEntity.Address = address.Address;
                                        //custAddressEntity.CityId = cities.FirstOrDefault(x1 => x1.Name == address.City)?.Id;
                                        custAddressEntity.CityId = address?.CityId;
                                        custAddressEntity.PostalCode = address.PostalCode;
                                        custAddressEntity.Euvatprefix = address.EUVatPrefix;
                                        custAddressEntity.VatTaxRegistrationNo = address.VatTaxRegNumber;
                                        custAddressEntity.LastModification = DateTime.UtcNow;
                                        custAddressEntity.ModifiedBy = address.ModifiedBy;
                                        custAddressEntity.UpdateCount = custAddressEntity.UpdateCount.CalculateUpdateCount();
                                        this._custAddressRepository.Update(custAddressEntity);
                                        result.Add(custAddressEntity);
                                        anyCustomerAddressUpdated = true;
                                    }
                                    else
                                    {
                                        responseType = ResponseType.Error;
                                        validationMessage.Add(new ValidationMessage(address, new List<MessageDetail>()
                                        {
                                            new MessageDetail(ModuleType.Customer, MessageType.CustAddr_UpdateCountMismatch.ToId(),this._MessageDescriptions[MessageType.CustAddr_UpdateCountMismatch.ToId()].ToString())
                                        }));
                                    }
                                }
                                else
                                {
                                    responseType = ResponseType.Error;
                                    validationMessage.Add(new ValidationMessage(address, new List<MessageDetail>()
                                    {
                                        new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidAddressId.ToId(),this._MessageDescriptions[MessageType.CustAddr_InvalidAddressId.ToId()].ToString())
                                    }));
                                }

                            }
                            catch (Exception ex1)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), ex1.ToFullString(), address);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex1.GetBaseException().Message));
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
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), addressModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message + Environment.NewLine + ex.InnerException?.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.CustomerAddress>>(result).ToList());
        }

        private Response DeleteCustomerAddress(int customerId, IList<DomainModel.CustomerAddress> addressModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            bool anyCustomerAddressDeleted = false;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                if (addressModels.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(addressModels), ValidationType.Delete);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(addressModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (DomainModel.CustomerAddress custAddr in addressModels)
                        {
                            try
                            {
                                var custAddrEntity = this._custAddressRepository.FindBy(c => c.CustomerId == customerId && c.Id == custAddr.AddressId).FirstOrDefault();
                                if (custAddrEntity != null && custAddrEntity.ContractDefaultCustomerInvoiceAddress.Count <= 0)
                                {
                                    this._custAddressRepository.Delete(custAddrEntity);
                                    anyCustomerAddressDeleted = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                responseType = ResponseType.Exception;
                                this._logger.LogError(responseType.ToId(), ex.ToFullString(), addressModels);
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
                            }
                        }

                    }
                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerAddressDeleted)
                        responseType = ResponseType.PartiallySuccess;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), addressModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, null);
        }
    }
}
