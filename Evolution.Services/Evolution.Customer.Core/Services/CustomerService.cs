using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.Document.Domain.Interfaces.Documents;
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
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;
namespace Evolution.Customer.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerService> _logger = null;
        private readonly ICustomerRepository _repository = null;
        private readonly ICustomerValidationService _validationService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;

        public CustomerService(IMapper mapper,
                                IAppLogger<CustomerService> logger,
                                ICustomerRepository repository,
                                ICustomerValidationService validationService,
                                IMongoDocumentService mongoDocumentService,
                                JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._validationService = validationService;
            this._mongoDocumentService = mongoDocumentService;
            this._messageDescriptions = messages;
        }

        public async Task<Response> GetCustomerAsync(DomainModel.CustomerSearch searchModel)
        {
            IList<DomainModel.Customer> result = null;
            Exception exception = null;
            IList<string> mongoSearch = null;
            try
            {
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    { 
                        searchModel.CustomerCodes = mongoSearch;
                        // result = this._repository.Search(searchModel);
                        result = GetCustomers(searchModel);
                        if (result?.Count > 0)
                            result = result.Where(x => mongoSearch.Contains(x.CustomerCode.Trim())).ToList();
                    }
                    else
                        result = new List<DomainModel.Customer>();
                }
                else{
                    //result = this._repository.Search(searchModel);
                    result = GetCustomers(searchModel);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetApprovedVistCustomers(int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {
            IList<DomainModel.CustomerSearch> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.SearchApprovedVisitCustomers(ContractHolderCompanyId, isVisit, isNDT);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetUnApprovedVisitCustomers(int ContractHolderCompanyId, int? CoordinatorId, bool isVisit, bool isOperating)
        {
            IList<DomainModel.CustomerSearch> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.SearchUnApprovedVisitCustomers(ContractHolderCompanyId, CoordinatorId, isVisit, isOperating);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetVisitTimesheetKPICustomers(int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            IList<DomainModel.CustomerSearch> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.GetVisitTimesheetKPICustomers(ContractHolderCompanyId, isVisit, isOperating);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetCustomerBasedOnCoordinator(DomainModel.CustomerSearch searchModel)
        {
            IList<DomainModel.Customer> result = null;
            Exception exception = null;
            try
            {

                if (searchModel.CoordinatorId != 0 && (searchModel.OperatingCompanyId != 0 || searchModel.ContractHolderCompanyId != 0))
                {
                    // result = GetCustomersBasedOnCoordinators(searchModel);
                    // IList<DomainModel.Customer> result = null;
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        result = this._repository.SearchCustomersBasedOnCoordinators(searchModel);
                        tranScope.Complete();
                    }

                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        private IList<DomainModel.Customer> GetCustomers(DomainModel.CustomerSearch searchModel)
         {
                IList<DomainModel.Customer> result = null;
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                       new TransactionOptions{IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = this._repository.Search(searchModel);
                    tranScope.Complete();
                }
                return result;
         }

        public Response ModifyCustomer(IList<DomainModel.Customer> customers)
        {
            Response newCustomerResponse = null;
            Response updateCustomerResponse = null;
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            List<DomainModel.Customer> result = null;

            try
            {
                newCustomerResponse = new Response().ToPopulate(ResponseType.Success);
                updateCustomerResponse = new Response().ToPopulate(ResponseType.Success);
                validationMessage = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                result = new List<DomainModel.Customer>();

                var customerToBeInsertes = customers.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList();
                if (customerToBeInsertes.Count > 0)
                    newCustomerResponse = this.AddCustomers(customerToBeInsertes);

                var customerToBeUpdate = customers.Where(c => c.RecordStatus.IsRecordStatusModified()).ToList();
                if (customerToBeUpdate.Count > 0)
                    updateCustomerResponse = this.UpdateCustomers(customerToBeUpdate);

                if (newCustomerResponse.ValidationMessages != null && newCustomerResponse.ValidationMessages.Count > 0)
                    validationMessage.AddRange(newCustomerResponse.ValidationMessages);

                if (newCustomerResponse.Messages != null && newCustomerResponse.Messages.Count > 0)
                    errorMessages.AddRange(newCustomerResponse.Messages);

                if (newCustomerResponse.Result != null && ((List<DomainModel.Customer>)newCustomerResponse.Result).Count > 0)
                    result.AddRange((List<DomainModel.Customer>)newCustomerResponse.Result);

                if (updateCustomerResponse.ValidationMessages != null && updateCustomerResponse.ValidationMessages.Count > 0)
                    validationMessage.AddRange(updateCustomerResponse.ValidationMessages);

                if (updateCustomerResponse.Messages != null && updateCustomerResponse.Messages.Count > 0)
                    errorMessages.AddRange(updateCustomerResponse.Messages);

                if (updateCustomerResponse.Result != null && ((List<DomainModel.Customer>)updateCustomerResponse.Result).Count > 0)
                    result.AddRange((List<DomainModel.Customer>)updateCustomerResponse.Result);

                if (validationMessage.Count > 0)
                    responseType = ResponseType.Validation;

                if (errorMessages.Count > 0)
                    responseType = ResponseType.Error;

                if (newCustomerResponse.Code == ResponseType.PartiallySuccess.ToId() || updateCustomerResponse.Code == ResponseType.PartiallySuccess.ToId())
                    responseType = ResponseType.PartiallySuccess;
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
                _logger.LogError(responseType.ToId(), ex.Message, customers);
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response SaveCustomer(IList<DomainModel.Customer> customers)
        {
            return AddCustomers(customers);
        }

        private Response AddCustomers(IList<DomainModel.Customer> models)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DomainModel.Customer> result = null;
            bool anyCustomerSaved = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(models), ValidationType.Add);
                if (valdResult.Count > 0)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(models, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var miiwaCustomer = _repository.GetAll().Select(x => new { MiiwaId = x.Miiwaid, x.Code, x.UpdateCount, Name = x.Name }).ToList();

                    foreach (var customer in models)
                    {
                        bool isMiiwaIdAlreadyExists = miiwaCustomer.Any(x => x.MiiwaId == customer.MIIWAId);
                        bool isCustomerNameAlreadyExists = miiwaCustomer.Any(x => x.Name == customer.CustomerName);

                        if (customer.MIIWAId > 0 && (isMiiwaIdAlreadyExists || isCustomerNameAlreadyExists))
                        {
                            responseType = ResponseType.Validation;
                            validationMessage.Add(new ValidationMessage(customer, new List<MessageDetail>()
                                {
                                    new MessageDetail(ModuleType.Customer, MessageType.Customer_DuplicateCustomer.ToId(),this._messageDescriptions[MessageType.Customer_DuplicateCustomer.ToId()].ToString())
                                }));
                        }
                        else
                        {
                            var dbCustomer = _mapper.Map<DomainModel.Customer, DbModel.Customer>(customer);
                            customer.CustomerCode = string.Format("{0}{1}", dbCustomer.Name.Trim().Substring(0, 2).ToUpper(), dbCustomer.Miiwaid.ToString().PadLeft(5, '0'));
                            dbCustomer.Id = 0;
                            dbCustomer.Code = customer.CustomerCode;
                            dbCustomer.IsActive = true;
                            dbCustomer.UpdateCount = 0;
                            this._repository.Add(dbCustomer);
                            anyCustomerSaved = true;
                        }
                    }
                }

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerSaved)
                    responseType = ResponseType.PartiallySuccess;

                var codes = models.Select(x => x.CustomerCode).ToList();
                result = _repository.FindBy(x => codes.Contains(x.Code)).ToList().Select(x1 => _mapper.Map<DomainModel.Customer>(x1)).ToList();
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), models);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        private Response UpdateCustomers(IList<DomainModel.Customer> models)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DomainModel.Customer> result = null;
            bool anyCustomerUpdated = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(models), ValidationType.Update);
                if (valdResult.Count > 0)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(models, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var miiwaCustomer = _repository.GetAll().Select(x => new { MiiwaId = x.Miiwaid, x.Code, x.UpdateCount }).ToList();
                    foreach (var customer in models)
                    {

                        var dbCustomer = _repository.FindBy(x => x.Code == customer.CustomerCode).FirstOrDefault();
                        if (dbCustomer == null)
                        {
                            responseType = ResponseType.Validation;
                            validationMessage.Add(new ValidationMessage(customer, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_InvalidCustomerCode.ToId(), _messageDescriptions[MessageType.Customer_InvalidCustomerCode.ToId()].ToString()) }));
                        }
                        else
                        {
                            if (!string.Equals(dbCustomer.Name.Trim().ToUpper(), customer.CustomerName.Trim().ToUpper()) && dbCustomer.Miiwaid != customer.MIIWAId)
                            {
                                dbCustomer.Name = customer.CustomerName;
                                dbCustomer.Miiwaid = customer.MIIWAId;
                                dbCustomer.Code = string.Format("{0}{1}", customer.CustomerName.Substring(0, 2).ToUpper(), customer.MIIWAId.ToString("D5"));
                            }
                            dbCustomer.ParentName = customer.ParentCompanyName;
                            dbCustomer.MiiwaparentId = customer.MIIWAParentId;
                            dbCustomer.IsActive = customer.Active.ToTrueFalse();
                            dbCustomer.LastModification = DateTime.UtcNow;
                            dbCustomer.ModifiedBy = customer.ModifiedBy;
                            dbCustomer.UpdateCount = customer.UpdateCount.CalculateUpdateCount();
                            this._repository.Update(dbCustomer);
                            anyCustomerUpdated = true;
                        }
                    }

                }

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerUpdated)
                    responseType = ResponseType.PartiallySuccess;

                var codes = models.Select(x => x.CustomerCode).ToList();
                result = _repository.FindBy(x => codes.Contains(x.Code)).ToList().Select(x1 => _mapper.Map<DomainModel.Customer>(x1)).ToList();
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), models);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public bool IsValidCustomer(IList<string> customerCodes, ref IList<DbModel.Customer> dbCustomers, ref IList<ValidationMessage> messages, params Expression<Func<DbModel.Customer, object>>[] includes)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (customerCodes?.Count() > 0)
            {
                if (dbCustomers == null || dbCustomers?.Count <= 0)
                    dbCustomers = _repository?.FindBy(x => customerCodes.Contains(x.Code), includes).ToList();

                var dbCustomer = dbCustomers;
                var companyCodeNotExists = customerCodes?.Where(x => !dbCustomer.Any(x2 => x2.Code == x))?.ToList();
                companyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCustomerCode.ToId();
                    valdMessage.Add(_messageDescriptions, x, MessageType.InvalidCustomerCode, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }

        //Third party integration - iConnect which handles both add/modyify customers based on the MIIWA ID existence in DB
        public Response IConnectIntegration(IList<DomainModel.Customer> models)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DomainModel.Customer> result = null;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(models), ValidationType.Add);
                if (valdResult.Count > 0)
                {
                    responseType = ResponseType.Validation;
                    validationMessage.Add(new ValidationMessage(models, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var miiwaIds = models.Select(x => x.MIIWAId).ToList();
                    IList<DbModel.Customer> miiwaCustomer = _repository.FindBy(x => miiwaIds.Contains(x.Miiwaid)).ToList();

                    IList<DomainModel.Customer> newCustomers = models.Where(x => !miiwaCustomer.Any(x1 => x1.Miiwaid == x.MIIWAId)).ToList();
                    IList<DbModel.Customer> childCustomers = _repository.FindBy(x => miiwaIds.Any(y => y == x.MiiwaparentId)).ToList(); // Getting the Child Customers based on MiiwaParentId
                    if (miiwaCustomer?.Count > 0)
                    {
                        var customerData = models?.Join(miiwaCustomer,
                                           m => new { MIIWAId = m.MIIWAId },
                                           c => new { MIIWAId = c.Miiwaid },
                                           (m, c) => new { m, c })
                                            .Select(x => {
                                                x.c.Name = x.m.CustomerName?.Length > 80? x.m.CustomerName?.Substring(0, 80): x.m.CustomerName;
                                                x.c.Code = string.Format("{0}{1}", x.m.CustomerName.Substring(0, 2).ToUpper(), x.m.MIIWAId.ToString().Length > 5 ? x.m.MIIWAId.ToString().Substring(x.m.MIIWAId.ToString().Length - 5) : x.m.MIIWAId.ToString().PadLeft(5, '0'));
                                                x.c.ParentName = x.m.ParentCompanyName?.Length > 60 ? x.m.ParentCompanyName?.Substring(0, 60) : x.m.ParentCompanyName;
                                                x.c.MiiwaparentId = x.m.MIIWAParentId;
                                                x.c.IsActive = x.m.Active.ToTrueFalse();
                                                x.c.LastModification = DateTime.UtcNow;
                                                x.c.ModifiedBy = x.m.ModifiedBy;
                                                x.c.UpdateCount = x.c.UpdateCount.CalculateUpdateCount();
                                                return x.c;
                                            }).ToList();
                        this._repository.Update(customerData);
                    }

                    if (childCustomers?.Count > 0) 
                    {
                        //Child Customers Parent Name will Update , if Parent Company Name is changed
                        var childCustomer = models?.Join(childCustomers,
                                            m => m.MIIWAId,
                                            c => c.MiiwaparentId,
                                            (m, c) => new { m, c })
                                            .Where(x => x.m.ParentCompanyName != x.c.ParentName)
                                            .Select(x => {
                                                x.c.ParentName = x.m.ParentCompanyName?.Length > 60 ? x.m.ParentCompanyName?.Substring(0, 60) : x.m.ParentCompanyName;
                                                x.c.LastModification = DateTime.UtcNow;
                                                x.c.ModifiedBy = x.m.ModifiedBy;
                                                x.c.UpdateCount = x.c.UpdateCount.CalculateUpdateCount();
                                                return x.c;
                                            }).ToList();
                        this._repository.Update(childCustomer);
                    }
                    if (newCustomers?.Count > 0)
                    {
                        var customers = newCustomers.Select(c => new DbModel.Customer
                        {
                            Id = 0,
                            Code = string.Format("{0}{1}", c.CustomerName.Trim().Substring(0, 2).ToUpper(), c.MIIWAId.ToString().Length > 5 ? c.MIIWAId.ToString().Substring(c.MIIWAId.ToString().Length - 5) : c.MIIWAId.ToString().PadLeft(5, '0')),
                            Name = c.CustomerName?.Length > 80 ? c.CustomerName?.Substring(0, 80) : c.CustomerName,
                            ParentName = c.ParentCompanyName?.Length > 60 ? c.ParentCompanyName?.Substring(0, 60) : c.ParentCompanyName,
                            Miiwaid = c.MIIWAId,
                            MiiwaparentId = c.MIIWAParentId,
                            LastModification = DateTime.UtcNow,
                            IsActive = c.Active.ToTrueFalse(),
                            UpdateCount = 0,
                            ModifiedBy = c.ModifiedBy
                        }).ToList();
                        this._repository.Add(customers);
                    }
                }
                var miiwaIdList = models.Select(x => x.MIIWAId).ToList();
                result = _repository.FindBy(x => miiwaIdList.Contains(x.Miiwaid)).ProjectTo<DomainModel.Customer>().ToList();

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), models);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }
    }
}