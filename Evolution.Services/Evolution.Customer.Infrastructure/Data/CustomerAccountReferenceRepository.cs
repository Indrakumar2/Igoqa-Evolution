using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Infrastructure.Data
{
    public class CustomerAccountReferenceRepository : GenericRepository<DbModel.CustomerCompanyAccountReference>, ICustomerAccountReferenceRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IAppLogger<CustomerAccountReferenceRepository> _logger = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly IMapper _mapper = null;
        private readonly ICustomerAccountReferenceValidationService _validationService = null;

        public CustomerAccountReferenceRepository(DbModel.EvolutionSqlDbContext dbContext, IAppLogger<CustomerAccountReferenceRepository> logger, ICustomerAccountReferenceValidationService validationService, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._logger = logger;
            this._validationService = validationService;
            this._mapper = mapper;
            this._MessageDescriptions = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));

        }

        public IList<DomainModel.CustomerCompanyAccountReference> Search(string customerCode, DomainModel.CustomerCompanyAccountReference model)
        {
            if (this._dbContext == null)
                throw new System.InvalidOperationException("Datacontext is not intitialized.");

            var joinResult = _dbContext.CustomerCompanyAccountReference
                        .Join(_dbContext.Customer,
                                add => add.CustomerId,
                                data => data.Id,
                                (custAccount, cust) => new { custAccount, cust })
                        .Join(_dbContext.Company,
                                left => left.custAccount.CompanyId,
                                comp => comp.Id,
                                (cust, comp) => new { cust, comp })
                         .Where(x => (string.IsNullOrEmpty(model.CompanyCode) || x.comp.Code == model.CompanyCode)
                         && (string.IsNullOrEmpty(model.CompanyName) || x.comp.Name == model.CompanyName)
                         && (string.IsNullOrEmpty(model.AccountReferenceValue) || x.cust.custAccount.AccountReference == model.AccountReferenceValue)
                         && (string.IsNullOrEmpty(customerCode) || x.cust.cust.Code == customerCode))
                        .Select(x => new
                        {
                            x.cust.custAccount,
                            Company = x.comp
                        }).ToList();

            return joinResult.Select(x => new DomainModel.CustomerCompanyAccountReference()
            {
                CustomerCompanyAccountReferenceId = x.custAccount.Id,
                AccountReferenceValue = x.custAccount.AccountReference,
                CompanyCode = x.Company.Code,
                CompanyName = x.Company.Name,
                ModifiedBy = x.custAccount.ModifiedBy,
                LastModifiedOn = (DateTime)x.custAccount.LastModification,
                UpdateCount = x.custAccount.UpdateCount
            }).ToList();
        }

        public Response AddCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models)
        {
            return PerformOperations(customerCode, models);
        }

        public Response UpdateCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models)
        {
            return PerformOperations(customerCode, models);
        }

        public Response DeleteCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models)
        {
            return PerformOperations(customerCode, models);
        }

        private Response PerformOperations(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DomainModel.CustomerCompanyAccountReference> result = null;
            bool anyCustomerSaved = false;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DomainModel.CustomerCompanyAccountReference>();
                var dbCustomer = _dbContext.Customer.Where(x => x.Code == customerCode).FirstOrDefault();
                if (dbCustomer != null)
                {
                    var customerAccountRef = this._dbContext.Set<CustomerCompanyAccountReference>().Select(x => new
                    {
                        companyAccountRefId = x.Id,
                        accountRef = x.AccountReference,
                        companyId = x.CompanyId,
                        companyCode = x.Company.Code,
                        customerId = x.CustomerId,
                        UpdateCount = x.UpdateCount.CalculateUpdateCount()
                }).ToList();

                    foreach (var customerAccountReference in models)
                    {
                        try
                        {
                            if (customerAccountReference.RecordStatus.IsRecordStatusDeleted())
                            {
                                var dbCustomerAccountReference = _dbContext.CustomerCompanyAccountReference.Where(x => x.Id == customerAccountReference.CustomerCompanyAccountReferenceId).FirstOrDefault();
                                this._dbContext.CustomerCompanyAccountReference.Remove(dbCustomerAccountReference);
                                this._dbContext.SaveChanges();
                            }
                            else
                            {
                                var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(customerAccountReference), customerAccountReference.RecordStatus.IsRecordStatusNew() ? ValidationType.Add : customerAccountReference.RecordStatus.IsRecordStatusModified() ? ValidationType.Update : ValidationType.None);
                                if (valdResult.Count > 0)
                                {
                                    responseType = ResponseType.Validation;
                                    validationMessage.Add(new ValidationMessage(customerAccountReference, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                                }
                                else
                                {
                                    var dbCompany = this._dbContext.Set<Company>().Where(x => x.Code == customerAccountReference.CompanyCode).FirstOrDefault();
                                    if (dbCompany != null)
                                    {
                                        var dbCustomerAccountReferences = _mapper.Map<DomainModel.CustomerCompanyAccountReference, DbModel.CustomerCompanyAccountReference>(customerAccountReference);
                                        if (customerAccountRef.Any(x => x.companyId == dbCompany.Id
                                        && x.customerId == dbCustomer.Id && x.accountRef == dbCustomerAccountReferences.AccountReference) && customerAccountReference.RecordStatus.IsRecordStatusNew())
                                        {
                                            responseType = ResponseType.Validation;
                                            validationMessage.Add(new ValidationMessage(customerAccountReference, new List<MessageDetail>()
                                            {
                                                new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_AlreadyExists.ToId(),this._MessageDescriptions[MessageType.Customer_ACRef_AlreadyExists.ToId()].ToString())
                                            }));
                                        }
                                        else if (customerAccountRef.Any(x => x.companyId == dbCompany.Id
                                        && x.customerId == dbCustomer.Id && x.accountRef == dbCustomerAccountReferences.AccountReference && x.companyAccountRefId != dbCustomerAccountReferences.Id) && customerAccountReference.RecordStatus.IsRecordStatusModified())
                                        {
                                            responseType = ResponseType.Validation;
                                            validationMessage.Add(new ValidationMessage(customerAccountReference, new List<MessageDetail>()
                                            {
                                                new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_AlreadyExists.ToId(),this._MessageDescriptions[MessageType.Customer_ACRef_AlreadyExists.ToId()].ToString())
                                            }));
                                        }
                                        else
                                        {
                                            dbCustomerAccountReferences.CustomerId = dbCustomer.Id;
                                            dbCustomerAccountReferences.CompanyId = dbCompany.Id;
                                            dbCustomerAccountReferences.LastModification = DateTime.UtcNow;
                                            dbCustomerAccountReferences.UpdateCount = 0;

                                            if (customerAccountReference.RecordStatus.IsRecordStatusNew())
                                            {
                                                dbCustomerAccountReferences.Id = 0;
                                                this._dbContext.CustomerCompanyAccountReference.Add(dbCustomerAccountReferences);
                                                this._dbContext.SaveChanges();
                                            }
                                            else if (customerAccountReference.RecordStatus.IsRecordStatusModified())
                                            {
                                                if (customerAccountRef.Any(x => x.companyAccountRefId == dbCustomerAccountReferences.Id && x.UpdateCount == customerAccountReference.UpdateCount))
                                                {
                                                    dbCustomerAccountReferences.UpdateCount = customerAccountReference.UpdateCount.CalculateUpdateCount();
                                                    this._dbContext.Update(dbCustomerAccountReferences);
                                                    this._dbContext.CustomerCompanyAccountReference.Update(dbCustomerAccountReferences);
                                                    this._dbContext.SaveChanges();
                                                }
                                                else
                                                {
                                                    responseType = ResponseType.Validation;
                                                    this._logger.LogError(responseType.ToId(), this._MessageDescriptions[MessageType.Customer_ACRef_UpdateCountMismatch.ToId()].ToString(), customerAccountReference);
                                                    validationMessage.Add(new ValidationMessage(customerAccountReference, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_UpdateCountMismatch.ToId(), this._MessageDescriptions[MessageType.Customer_ACRef_UpdateCountMismatch.ToId()].ToString()) }));
                                                }
                                            }

                                            customerAccountReference.CustomerCompanyAccountReferenceId = dbCustomerAccountReferences.Id;
                                            result.Add(customerAccountReference);
                                        }
                                    }
                                    else {
                                        responseType = ResponseType.Error;
                                        this._logger.LogError(responseType.ToId(), this._MessageDescriptions[MessageType.Customer_ACRef_InvalidCompanyCode.ToId()].ToString(), customerAccountReference);
                                        errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_InvalidCompanyCode.ToId(), this._MessageDescriptions[MessageType.Customer_ACRef_InvalidCompanyCode.ToId()].ToString()));
                                    }
                                }
                            }
                        }
                        catch (SqlException sqlE)
                        {
                            responseType = ResponseType.Exception;
                            this._logger.LogError(sqlE.ErrorCode.ToString(),  sqlE.Message, models);
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
                        }
                    }
                }
                else
                {
                    responseType = ResponseType.Error;
                    this._logger.LogError(responseType.ToId(), this._MessageDescriptions[MessageType.Customer_ACRef_InvalidCustomerCode.ToId()].ToString(), customerCode);
                    errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_InvalidCustomerCode.ToId(), this._MessageDescriptions[MessageType.Customer_ACRef_InvalidCustomerCode.ToId()].ToString()));
                }

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerSaved)
                    responseType = ResponseType.PartiallySuccess;

            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(sqlE.ErrorCode.ToString(),  sqlE.Message, models);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, models);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }
    }
}
