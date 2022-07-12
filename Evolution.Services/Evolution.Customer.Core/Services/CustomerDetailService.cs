using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Core.Services
{
    public class CustomerDetailService : ICustomerDetailService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerDetailService> _logger = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly ICustomerContactRepository _customerContactRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IDocumentService _documentService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ICustomerAddressValidationService _customerAddressValidationService = null;
        private readonly ICustomerAssignmentReferenceValidationService _customerAssignmentReferenceValidationService = null;
        private readonly ICustomerContactValidationService _customerContactValidationService = null;
        private readonly ICustomerNoteValidationService _customerNoteValidationService = null;
        private readonly ICustomerValidationService _customerValidationService = null;
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IUserDetailService _userDetailService = null;
        private readonly IUserService _userService = null;

        //private readonly IAuditLogger _auditLogger = null;

        public CustomerDetailService(IMapper mapper,
                                        IAppLogger<CustomerDetailService> logger,
                                        ICustomerRepository customerRepository,
                                        ICustomerContactRepository customerContactRepository,
                                        IDocumentService documentService,
                                        ICustomerAccountReferenceValidationService customerAccountReferenceValidationService,
                                        ICustomerAddressValidationService customerAddressValidationService,
                                        ICustomerAssignmentReferenceValidationService customerAssignmentReferenceValidationService,
                                        ICustomerContactValidationService customerContactValidationService,
                                        ICustomerNoteValidationService customerNoteValidationService,
                                        ICustomerValidationService customerValidationService, JObject messages,
                                        EvolutionSqlDbContext dbContext, IAuditSearchService auditSearchService,
                                        IOptions<AppEnvVariableBaseModel> environment,
                                        IUserDetailService userDetailService,
                                        IUserService userService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._customerRepository = customerRepository;
            this._customerContactRepository = customerContactRepository;
            _documentService = documentService;
            _customerAddressValidationService = customerAddressValidationService;
            _customerAssignmentReferenceValidationService = customerAssignmentReferenceValidationService;
            _customerContactValidationService = customerContactValidationService;
            _customerNoteValidationService = customerNoteValidationService;
            _customerValidationService = customerValidationService;
            _dbContext = dbContext;
            this._auditSearchService = auditSearchService;
            this._messageDescriptions = messages;
            _environment = environment.Value;
            _userDetailService = userDetailService;
            _userService = userService;
        }

        public Response SaveCustomerDetails(IList<DomainModel.CustomerDetail> customerDetailsModel)
        {
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessage = new List<ValidationMessage>();
            List<DBModel.Customer> result = new List<DBModel.Customer>();
            List<string> responseStatuses = new List<string>();

            try
            {
                if (customerDetailsModel != null && customerDetailsModel.Any(x => x.Detail != null))
                {
                    foreach (var customerDetail in customerDetailsModel)
                    {
                        Response saveResult = ProcessCustomerDetail(customerDetail);
                        responseStatuses.Add(saveResult.Code);
                        validationMessage.AddRange(saveResult.ValidationMessages?.ToList());
                        errorMessages.AddRange(saveResult.Messages?.ToList());
                        if (saveResult.Result != null && saveResult.Result.IsAnyCollectionPropertyContainValue())
                            result.AddRange((List<DBModel.Customer>)saveResult.Result);
                    }

                    if (responseStatuses.Count(x => x == ResponseType.Success.ToId()) == customerDetailsModel.Count)
                        responseType = ResponseType.Success;
                    else if (responseStatuses.Any(x => x == ResponseType.PartiallySuccess.ToId()))
                        responseType = ResponseType.PartiallySuccess;
                    else
                        responseType = ResponseType.Error;
                }

                else if (customerDetailsModel == null || customerDetailsModel.Any(x => x.Detail == null))
                {
                    Exception exception = null;
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, customerDetailsModel, MessageType.InvalidPayLoad, customerDetailsModel }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDetailsModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<List<DBModel.Customer>, List<DomainModel.Customer>>(result));
        }

        #region Customer Detail

        private Response ProcessCustomerDetail(DomainModel.CustomerDetail customerDetailModel)
        {
            ResponseType responseType = ResponseType.Success;
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessage = new List<ValidationMessage>();
            IList<DBModel.Customer> result = new List<DBModel.Customer>();
            DBModel.Customer dbCustomer = null;
            List<DomainModel.CustomerDetail> domCustDetail = new List<DomainModel.CustomerDetail>();
            List<DBModel.Document> dbDocuments = null;
            List<UserInfo> users = null;
            long? eventID = null;
            try
            {
                //To-Do: Will create helper method get TransactionScope instance based on requirement
                using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new System.Transactions.TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                //using (var tranScope = new TransactionScope())
                {
                    this._customerRepository.AutoSave = false;
                    if (customerDetailModel.Detail.RecordStatus.IsRecordStatusNew())
                    {
                        dbCustomer = AddCustomerDetails(customerDetailModel, ref errorMessages, ref validationMessage, ref dbDocuments);
                        if (dbCustomer != null)
                        {
                            dbCustomer.Code = string.Format("{0}{1}", customerDetailModel.Detail.CustomerName.Substring(0, 2).ToUpper(), customerDetailModel.Detail.MIIWAId.ToString("D5"));
                            this._customerRepository.Add(dbCustomer);
                        }

                    }
                    else
                    {
                        dbCustomer = UpdateCustomerDetails(customerDetailModel, ref domCustDetail, ref errorMessages, ref validationMessage, ref dbDocuments, ref users);
                    }
                    if (errorMessages?.Count <= 0 && validationMessage?.Count <= 0)
                    {

                        int isSaved = _customerRepository.ForceSave();
                        customerDetailModel.dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                                                                                    SqlAuditModuleType.Customer.ToString(),
                                                                                                                    SqlAuditModuleType.CustomerAddress.ToString(),
                                                                                                                    SqlAuditModuleType.CustomerAssignmentReference.ToString(),
                                                                                                                    SqlAuditModuleType.CustomerAccountReference.ToString(),
                                                                                                                    SqlAuditModuleType.CustomerNote.ToString(),
                                                                                                                    SqlAuditModuleType.CustomerDocument.ToString(),
                                                                                                                    SqlAuditModuleType.CustomerContact.ToString()
                                                                                                              });
                        if (isSaved >= 0)
                        {
                            response = ProcessExtranetDetails(customerDetailModel,ref validationMessage);
                            if (response.Code != ResponseType.Success.ToId())
                            {
                                return response;
                            }
                            result.Add(_customerRepository.FindBy(x => x.Code == dbCustomer.Code).FirstOrDefault());

                            if (errorMessages.Count > 0 && validationMessage.Count > 0)
                                responseType = ResponseType.Error;
                            else if (errorMessages.Count == 0 && validationMessage.Count > 0)
                                responseType = ResponseType.Validation;
                            //dbCust = _mapper.Map<List<DBModel.Customer>>(domCustDetail).FirstOrDefault();
                            if (customerDetailModel.Detail.RecordStatus.IsRecordStatusNew())
                            {                                     
                                if (result?.Count > 0)
                                {

                                    result?.ToList().ForEach(x =>
                                                                  AuditLog(customerDetailModel,
                                                                  dbCustomer,
                                                                  ValidationType.Add.ToAuditActionType(),
                                                                  SqlAuditModuleType.Customer,
                                                                  null,
                                                                  null,
                                                                  ref eventID, domCustDetail.FirstOrDefault(),
                                                                  users,
                                                                  dbDocuments));

                                }

                            }
                            else
                            {                                 
                                if (result != null)
                                {
                                    result?.ToList().ForEach(x =>
                                                              AuditLog(customerDetailModel,
                                                              dbCustomer,
                                                              ValidationType.Update.ToAuditActionType(),
                                                              SqlAuditModuleType.Customer,
                                                              null,
                                                              null,
                                                              ref eventID, domCustDetail.FirstOrDefault(),
                                                              users,
                                                              dbDocuments));
                                }


                            }
                        }
                        else if (isSaved <= 0 && errorMessages.Count > 0)
                        {
                            responseType = ResponseType.Error;
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), this._messageDescriptions[responseType.ToId()].ToString()));
                        }
                        else
                        {
                            AuditLog(customerDetailModel,
                                   dbCustomer,
                                   ValidationType.Update.ToAuditActionType(),
                                   SqlAuditModuleType.Customer,
                                    null,
                                    null,
                                   ref eventID, domCustDetail.FirstOrDefault(),
                                   users,
                                   dbDocuments);
                        }
                        tranScope.Complete();
                    }
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.ToFullString(), customerDetailModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
            }
            catch (Exception ex)
            {
                if (ex.InnerException is DbUpdateException dbUpdateEx)
                {
                    if (dbUpdateEx.InnerException != null)
                    {
                        if (dbUpdateEx.InnerException is SqlException sqlException)
                        {
                            switch (sqlException.Number)
                            {
                                case 2627:
                                    if (sqlException.Message.Contains("IX_Customer1_Code"))
                                    {
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_DuplicateCustomer.ToId(), _messageDescriptions[MessageType.Customer_DuplicateCustomer.ToId()].ToString()) }));
                                    }
                                    if (sqlException.Message.Contains("IX_Customer_CustomerCompanyAccountReference"))
                                    {
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_AlreadyExists.ToId(), _messageDescriptions[MessageType.Customer_ACRef_AlreadyExists.ToId()].ToString()) }));
                                    }
                                    if (sqlException.Message.Contains("IX_Customer_AssignmentReferenceId"))
                                    {
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_AlreadyExists.ToId(), _messageDescriptions[MessageType.Customer_AssRef_AlreadyExists.ToId()].ToString()) }));
                                    }
                                    if (sqlException.Message.Contains("IX_Customer_CustomerAddress"))
                                    {
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.CustAddr_ExisitingAddress.ToId(), _messageDescriptions[MessageType.CustAddr_ExisitingAddress.ToId()].ToString()) }));
                                    }
                                    break;
                                case 547:
                                    if (sqlException.Message.Contains("FK_Contract_CustomerContact_DefaultCustomerContractContactId")
                                        || sqlException.Message.Contains("FK_Contract_CustomerContact_DefaultCustomerInvoiceContactId"))
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_InUse_Contract.ToId(), _messageDescriptions[MessageType.Customer_Contact_InUse_Contract.ToId()].ToString()) }));

                                    if (sqlException.Message.Contains("FK_Project_CustomerContact_CustomerContactId")
                                        || sqlException.Message.Contains("FK_Project_CustomerContact_CustomerProjectContactId")
                                        || sqlException.Message.Contains("FK_ProjectClientNotification_CustomerContact_CustomerContactId"))
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_InUse_Project.ToId(), _messageDescriptions[MessageType.Customer_Contact_InUse_Project.ToId()].ToString()) }));

                                    if (sqlException.Message.Contains("FK_Assignment_CustomerContact"))
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_InUse_Assignment.ToId(), _messageDescriptions[MessageType.Customer_Contact_InUse_Assignment.ToId()].ToString()) }));

                                    if (sqlException.Message.Contains("FK_CustomerContact_AddressId"))
                                        validationMessage.Add(new ValidationMessage(customerDetailModel, new List<MessageDetail> { new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InUse_Contact.ToId(), _messageDescriptions[MessageType.CustAddr_InUse_Contact.ToId()].ToString()) }));

                                    break;
                            }
                        }
                    }
                }
                else
                    errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));

                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), customerDetailModel);
            }
            finally
            {
                this._customerRepository.AutoSave = true;
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        private DBModel.Customer AddCustomerDetails(DomainModel.CustomerDetail customerDetailModel, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages, ref List<DBModel.Document> dbDocuments)
        {
            DBModel.Customer dbCustomer = null;
            try
            {
                var customerValidationResult = ValidateCustomerDetails(dbCustomer, customerDetailModel);
                if (customerValidationResult != null && customerValidationResult.Count > 0)
                {
                    errorMessages.AddRange(customerValidationResult);
                }
                else
                {
                    dbCustomer = _mapper.Map<DomainModel.CustomerDetail, DBModel.Customer>(customerDetailModel);
                    if (customerDetailModel?.Documents?.Count > 0)
                        ProcessCustomerDocuments(customerDetailModel.Documents, dbCustomer.Code, ref errorMessages, ref validationMessages, ref dbDocuments);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDetailModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomer;
        }

        private DBModel.Customer UpdateCustomerDetails(DomainModel.CustomerDetail customerDetailModel, ref List<DomainModel.CustomerDetail> domCustomerDetail, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages, ref List<DBModel.Document> dbDocuments, ref List<UserInfo> users)
        {
            DBModel.Customer dbCustomer = null;
            try
            {
                dbCustomer = this._customerRepository.FindBy(x => x.Code == customerDetailModel.Detail.CustomerCode, new string[]{
                     "CustomerAddress",
                     "CustomerAddress.CustomerContact",
                     "CustomerAddress.City",
                     "CustomerAddress.City.County",
                     "CustomerAddress.City.County.Country",
                     "CustomerAssignmentReferenceType",
                     "CustomerAssignmentReferenceType.AssignmentReference",
                     "CustomerCompanyAccountReference",
                     "CustomerCompanyAccountReference.Company"
                 }).FirstOrDefault();

                var contactUsers = dbCustomer.CustomerAddress?.SelectMany(x => x.CustomerContact)?.ToList()?.Where(a=>!string.IsNullOrEmpty(a.LoginName)).Select(a=>a.LoginName)?.ToList();
              
                var portalUsers = _dbContext.User.Where(a => contactUsers.Contains(a.SamaccountName))?.ToList();

                users = _mapper.Map<List<UserInfo>>(portalUsers); //Changes for Customer Portal Audit issue

                domCustomerDetail.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.CustomerDetail>(dbCustomer)));
                if (dbCustomer == null)
                {
                    errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_InvalidCustomerCode.ToId(), _messageDescriptions[MessageType.Customer_InvalidCustomerCode.ToId()].ToString()));
                }
                else
                {
                    var customerValidationResult = ValidateCustomerDetails(dbCustomer, customerDetailModel);
                    if (customerValidationResult != null && customerValidationResult.Count > 0)
                    {
                        errorMessages.AddRange(customerValidationResult);
                    }
                    else
                    {
                        if (customerDetailModel.Detail.RecordStatus.IsRecordStatusModified())
                            dbCustomer = UpdateCustomer(dbCustomer, customerDetailModel.Detail, ref errorMessages, ref validationMessages);
                        if (customerDetailModel.AssignmentReferences != null && customerDetailModel.AssignmentReferences.Count > 0)
                            dbCustomer.CustomerAssignmentReferenceType = ProcessCustomerAssignmentReferenceType(dbCustomer.CustomerAssignmentReferenceType.ToList(), customerDetailModel.AssignmentReferences, ref errorMessages, ref validationMessages);
                        if (customerDetailModel.AccountReferences != null && customerDetailModel.AccountReferences.Count > 0)
                            dbCustomer.CustomerCompanyAccountReference = ProcessCustomerAccountReference(dbCustomer.CustomerCompanyAccountReference.ToList(), customerDetailModel.AccountReferences, ref errorMessages, ref validationMessages);
                        if (customerDetailModel.Notes != null && customerDetailModel.Notes.Count > 0)
                            dbCustomer.CustomerNote = ProcessCustomerNotes(dbCustomer.CustomerNote.ToList(), customerDetailModel.Notes, ref errorMessages, ref validationMessages);
                        if (customerDetailModel.Addresses != null && customerDetailModel.Addresses.Count > 0)
                            dbCustomer.CustomerAddress = ProcessCustomerAddresses(dbCustomer.CustomerAddress.ToList(), customerDetailModel.Addresses, ref errorMessages, ref validationMessages);
                        if (customerDetailModel.Documents != null && customerDetailModel.Documents.Count > 0)
                        {
                            var customerDetailModelClone = ObjectExtension.Clone(customerDetailModel); //To avoid reference variable update
                            ProcessCustomerDocuments(customerDetailModelClone.Documents, dbCustomer.Code, ref errorMessages, ref validationMessages, ref dbDocuments);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDetailModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomer;
        }

        #endregion

        #region Customer 

        private DBModel.Customer UpdateCustomer(DBModel.Customer dbCustomer, DomainModel.Customer customer, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerValidationService.Validate(JsonConvert.SerializeObject(new List<DomainModel.Customer> { customer }), ValidationType.Update);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customer, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    if (customer.RecordStatus.IsRecordStatusModified())
                    {
                        if (dbCustomer.UpdateCount == customer.UpdateCount)
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
                            dbCustomer.UpdateCount = dbCustomer.UpdateCount.CalculateUpdateCount();
                        }
                        else
                        {
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_UpdateCountMismatch.ToId(), this._messageDescriptions[MessageType.Customer_UpdateCountMismatch.ToId()].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customer);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomer;
        }

        #endregion

        #region CustomerAssignmentReferenceType

        private List<DBModel.CustomerAssignmentReferenceType> ProcessCustomerAssignmentReferenceType(List<DBModel.CustomerAssignmentReferenceType> dbCustomerAssignmentReferenceType, IList<DomainModel.CustomerAssignmentReference> customerAssignmentReferences, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<DBModel.CustomerAssignmentReferenceType> custAssignmentReferenceType = null;
            try
            {
                custAssignmentReferenceType = DeleteCustomerAssignmentReferences(dbCustomerAssignmentReferenceType, customerAssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), ref errorMessages, ref validationMessages);
                custAssignmentReferenceType = UpdateCustomerAssignmentReferences(custAssignmentReferenceType, customerAssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), ref errorMessages, ref validationMessages);
                custAssignmentReferenceType = AddCustomerAssignmentReferences(custAssignmentReferenceType, customerAssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), ref errorMessages, ref validationMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerAssignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return custAssignmentReferenceType;
        }

        private List<DBModel.CustomerAssignmentReferenceType> AddCustomerAssignmentReferences(List<DBModel.CustomerAssignmentReferenceType> dbCustomerAssignmentReferenceType, IList<DomainModel.CustomerAssignmentReference> assignmentReferences, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerAssignmentReferenceValidationService.Validate(JsonConvert.SerializeObject(assignmentReferences), ValidationType.Add);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(assignmentReferences, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var dbAssignmentReferences = _mapper.Map<IEnumerable<DomainModel.CustomerAssignmentReference>, IEnumerable<DBModel.CustomerAssignmentReferenceType>>(assignmentReferences);
                    dbCustomerAssignmentReferenceType.AddRange(dbAssignmentReferences);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAssignmentReferenceType;
        }

        private List<DBModel.CustomerAssignmentReferenceType> UpdateCustomerAssignmentReferences(List<DBModel.CustomerAssignmentReferenceType> dbCustomerAssignmentReferenceType, IList<DomainModel.CustomerAssignmentReference> assignmentReferences, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerAssignmentReferenceValidationService.Validate(JsonConvert.SerializeObject(assignmentReferences), ValidationType.Update);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(assignmentReferences, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    foreach (var assgReff in assignmentReferences)
                    {
                        var dbAssignmentReference = dbCustomerAssignmentReferenceType.FirstOrDefault(x => x.Id == assgReff.CustomerAssignmentReferenceId);

                        if (dbAssignmentReference != null)
                        {
                            if (dbAssignmentReference.UpdateCount == assgReff.UpdateCount)
                            {
                                int? AssignmentRefId = this._customerRepository.GetAssignmentReferenceIdForAssignmentRefferenceType(assgReff.AssignmentRefType);

                                if (AssignmentRefId.HasValue)
                                {
                                    if (dbCustomerAssignmentReferenceType.Exists(x => x.AssignmentReferenceId == AssignmentRefId))
                                    {
                                        errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_AlreadyExists.ToId(), this._messageDescriptions[MessageType.Customer_AssRef_AlreadyExists.ToId()].ToString()));
                                    }
                                    else
                                    {
                                        dbAssignmentReference.LastModification = DateTime.UtcNow;
                                        dbAssignmentReference.ModifiedBy = assgReff.ModifiedBy;
                                        dbAssignmentReference.UpdateCount = dbAssignmentReference.UpdateCount.CalculateUpdateCount();
                                        dbAssignmentReference.AssignmentReferenceId = AssignmentRefId.Value;
                                        dbCustomerAssignmentReferenceType[dbCustomerAssignmentReferenceType.ToList().FindIndex(ind => ind.Id == assgReff.CustomerAssignmentReferenceId)] = dbAssignmentReference;
                                    }
                                }
                                else
                                {
                                    errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_InvalidAssignmentReferenceType.ToId(), this._messageDescriptions[MessageType.Customer_AssRef_InvalidAssignmentReferenceType.ToId()].ToString()));
                                }

                            }
                            else
                            {
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_UpdateCountMismatch.ToId(), this._messageDescriptions[MessageType.Customer_AssRef_UpdateCountMismatch.ToId()].ToString()));
                            }
                        }
                        else
                        {
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_InvalidAssignmentReferenceId.ToId(), this._messageDescriptions[MessageType.Customer_AssRef_InvalidAssignmentReferenceId.ToId()].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAssignmentReferenceType;
        }

        private List<DBModel.CustomerAssignmentReferenceType> DeleteCustomerAssignmentReferences(List<DBModel.CustomerAssignmentReferenceType> dbCustomerAssignmentReferenceType, IList<DomainModel.CustomerAssignmentReference> assignmentReferences, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerAssignmentReferenceValidationService.Validate(JsonConvert.SerializeObject(assignmentReferences), ValidationType.Delete);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(assignmentReferences, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    if (assignmentReferences?.Count > 0)
                    {
                        var assRefIds = assignmentReferences.Select(x => x.CustomerAssignmentReferenceId.ToString()).Distinct().ToList();
                        var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Customer_AssignmentReference, SQLModuleActionType.Delete), string.Join(",", assRefIds));
                        var count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAssignmentReferenceType;
        }

        #endregion

        #region CustomerCompanyAccountReferences

        private List<DBModel.CustomerCompanyAccountReference> ProcessCustomerAccountReference(List<DBModel.CustomerCompanyAccountReference> dbCustomerCompanyAccountReferences, IList<DomainModel.CustomerCompanyAccountReference> accountReferences, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<DBModel.CustomerCompanyAccountReference> custAccountReferenceType = null;
            try
            {
                custAccountReferenceType = DeleteCustomerCompanyAccountReferences(dbCustomerCompanyAccountReferences, accountReferences.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), ref errorMessages);
                custAccountReferenceType = UpdateCustomerCompanyAccountReferences(custAccountReferenceType, accountReferences.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), ref errorMessages, ref validationMessages);
                custAccountReferenceType = AddCustomerCompanyAccountReferences(custAccountReferenceType, accountReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), ref errorMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return custAccountReferenceType;
        }

        private List<DBModel.CustomerCompanyAccountReference> AddCustomerCompanyAccountReferences(List<DBModel.CustomerCompanyAccountReference> dbCustomerCompanyAccountReferences, IList<DomainModel.CustomerCompanyAccountReference> accountReferences, ref List<MessageDetail> errorMessages)
        {
            try
            {
                var dbAccountReferences = _mapper.Map<IEnumerable<DomainModel.CustomerCompanyAccountReference>, IEnumerable<DBModel.CustomerCompanyAccountReference>>(accountReferences);
                dbCustomerCompanyAccountReferences.AddRange(dbAccountReferences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerCompanyAccountReferences;
        }

        private List<DBModel.CustomerCompanyAccountReference> UpdateCustomerCompanyAccountReferences(List<DBModel.CustomerCompanyAccountReference> dbCustomerCompanyAccountReferences, IList<DomainModel.CustomerCompanyAccountReference> accountReferences, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                foreach (var accReff in accountReferences)
                {
                    var dbAccountReferences = dbCustomerCompanyAccountReferences.FirstOrDefault(x => x.Id == accReff.CustomerCompanyAccountReferenceId);

                    if (dbAccountReferences != null)
                    {
                        int companyId = this._customerRepository.GetCompanyIdForCompanyCode(accReff.CompanyCode);
                        if (companyId > 0)
                        {
                            if (dbAccountReferences.UpdateCount == accReff.UpdateCount)
                            {
                                dbAccountReferences.AccountReference = accReff.AccountReferenceValue;
                                dbAccountReferences.CompanyId = companyId;
                                dbAccountReferences.LastModification = DateTime.UtcNow;
                                dbAccountReferences.ModifiedBy = accReff.ModifiedBy;
                                dbAccountReferences.UpdateCount = dbAccountReferences.UpdateCount.CalculateUpdateCount();
                                dbCustomerCompanyAccountReferences[dbCustomerCompanyAccountReferences.ToList().FindIndex(ind => ind.Id == accReff.CustomerCompanyAccountReferenceId)] = dbAccountReferences;
                            }
                            else
                            {
                                validationMessages.Add(new ValidationMessage(accReff, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_UpdateCountMismatch.ToId(), this._messageDescriptions[MessageType.Customer_ACRef_UpdateCountMismatch.ToId()].ToString()) }));
                            }
                        }
                        else
                        {
                            validationMessages.Add(new ValidationMessage(accReff, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_InvalidCompanyCode.ToId(), this._messageDescriptions[MessageType.Customer_ACRef_InvalidCompanyCode.ToId()].ToString()) }));
                        }
                    }
                    else
                    {
                        validationMessages.Add(new ValidationMessage(accReff, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_InvalidAccountReferenceId.ToId(), this._messageDescriptions[MessageType.Customer_ACRef_InvalidAccountReferenceId.ToId()].ToString()) }));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerCompanyAccountReferences;
        }

        private List<DBModel.CustomerCompanyAccountReference> DeleteCustomerCompanyAccountReferences(List<DBModel.CustomerCompanyAccountReference> dbCustomerCompanyAccountReferences, IList<DomainModel.CustomerCompanyAccountReference> accountReferences, ref List<MessageDetail> errorMessages)
        {

            try
            {
                if (accountReferences?.Count > 0)
                {
                    var accRefIds = accountReferences.Select(x => x.CustomerCompanyAccountReferenceId.ToString()).Distinct().ToList();
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Customer_AccountReference, SQLModuleActionType.Delete), string.Join(",", accRefIds));
                    var count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);


                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountReferences);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerCompanyAccountReferences;
        }

        #endregion

        #region CustomerDocuments

        private void ProcessCustomerDocuments(IList<ModuleDocument> customerDocuments, string customerCode, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages, ref List<DBModel.Document> dbDocuments)
        {
            Response response = null;
            try
            {
                if (customerDocuments != null)
                {
                    customerDocuments?.ToList().ForEach(x => { x.ModuleRefCode = customerCode; x.ModuleCode = ModuleCodeType.CUST.ToString(); });

                    response = _documentService.Delete(customerDocuments.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), false);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = _documentService.Save(customerDocuments.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), ref dbDocuments, false);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = _documentService.Modify(customerDocuments.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), ref dbDocuments, false);
                        }
                    }

                    validationMessages.AddRange(response.ValidationMessages);
                    errorMessages.AddRange(response.Messages);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDocuments);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }

        }

        #endregion

        #region CustomerNotes

        private List<DBModel.CustomerNote> ProcessCustomerNotes(List<DBModel.CustomerNote> dbCustomerNotes, IList<DomainModel.CustomerNote> customerNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<DBModel.CustomerNote> dbNotes = null;
            try
            {
                dbNotes = DeleteCustomerNotes(dbCustomerNotes, customerNotes.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), ref errorMessages, ref validationMessages);
                dbNotes = UpdateCustomerNotes(dbNotes, customerNotes.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), ref errorMessages, ref validationMessages);
                dbNotes = AddCustomerNotes(dbNotes, customerNotes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), ref errorMessages, ref validationMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbNotes;
        }

        private List<DBModel.CustomerNote> AddCustomerNotes(List<DBModel.CustomerNote> dbCustomerNotes, IList<DomainModel.CustomerNote> customerNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerNoteValidationService.Validate(JsonConvert.SerializeObject(customerNotes), ValidationType.Add);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customerNotes, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var dbNotes = _mapper.Map<IEnumerable<DomainModel.CustomerNote>, IEnumerable<DBModel.CustomerNote>>(customerNotes);
                    dbCustomerNotes.AddRange(dbNotes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerNotes;
        }

        private List<DBModel.CustomerNote> UpdateCustomerNotes(List<DBModel.CustomerNote> dbCustomerNotes, IList<DomainModel.CustomerNote> customerNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerNoteValidationService.Validate(JsonConvert.SerializeObject(customerNotes), ValidationType.Update);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customerNotes, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    foreach (var note in customerNotes)
                    {
                        var dbNotes = dbCustomerNotes.FirstOrDefault(x => x.Id == note.CustomerNoteId);
                        if (dbNotes != null)
                        {
                            if (dbNotes.UpdateCount == note.UpdateCount)
                            {
                                dbNotes.CreatedBy = note.CreatedBy;
                                dbNotes.Note = note.Note;
                                dbNotes.LastModification = DateTime.UtcNow;
                                dbNotes.ModifiedBy = note.ModifiedBy;
                                dbNotes.UpdateCount = dbNotes.UpdateCount.CalculateUpdateCount();
                                dbCustomerNotes[dbCustomerNotes.ToList().FindIndex(ind => ind.Id == note.CustomerNoteId)] = dbNotes;
                            }
                            else
                            {
                                validationMessages.Add(new ValidationMessage(note, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_Note_UpdateCountMismatch.ToId(), this._messageDescriptions[MessageType.Customer_Note_UpdateCountMismatch.ToId()].ToString()) }));
                            }
                        }
                        else
                        {
                            validationMessages.Add(new ValidationMessage(note, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_Note_InvalidNoteId.ToId(), this._messageDescriptions[MessageType.Customer_Note_InvalidNoteId.ToId()].ToString()) }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerNotes;
        }

        private List<DBModel.CustomerNote> DeleteCustomerNotes(List<DBModel.CustomerNote> dbCustomerNotes, IList<DomainModel.CustomerNote> customerNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerNoteValidationService.Validate(JsonConvert.SerializeObject(customerNotes), ValidationType.Delete);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customerNotes, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    if (customerNotes?.Count > 0)
                    {
                        var noteIds = customerNotes.Select(x => x.CustomerNoteId.ToString()).Distinct().ToList();
                        var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Customer_Note, SQLModuleActionType.Delete), string.Join(",", noteIds));
                        var count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerNotes;
        }

        #endregion

        #region CustomerAddress

        private List<DBModel.CustomerAddress> ProcessCustomerAddresses(List<DBModel.CustomerAddress> dbCustomerAddresses, IList<DomainModel.CustomerAddress> customerAddress, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<DBModel.CustomerAddress> dbAddresses = null;
            try
            {
                dbAddresses = DeleteCustomerAddress(dbCustomerAddresses, customerAddress.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), ref errorMessages, ref validationMessages);
                dbAddresses = UpdateCustomerAddress(dbAddresses, customerAddress.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), ref errorMessages, ref validationMessages);
                dbAddresses = AddCustomerAddress(dbAddresses, customerAddress.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), ref errorMessages, ref validationMessages);
                dbAddresses = SaveContactsInAddressWithStatusNull(dbAddresses, customerAddress.Where(x => string.IsNullOrEmpty(x.RecordStatus)).ToList(), ref errorMessages, ref validationMessages);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerAddress);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbAddresses;
        }

        private List<DBModel.CustomerAddress> AddCustomerAddress(List<DBModel.CustomerAddress> dbCustomerAddresses, IList<DomainModel.CustomerAddress> customerAddress, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerAddressValidationService.Validate(JsonConvert.SerializeObject(customerAddress), ValidationType.Add);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customerAddress, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var dbAddresses = _mapper.Map<IEnumerable<DomainModel.CustomerAddress>, IEnumerable<DBModel.CustomerAddress>>(customerAddress);
                    dbCustomerAddresses.AddRange(dbAddresses);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerAddress);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAddresses;
        }

        private List<DBModel.CustomerAddress> UpdateCustomerAddress(List<DBModel.CustomerAddress> dbCustomerAddresses, IList<DomainModel.CustomerAddress> customerAddress, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerAddressValidationService.Validate(JsonConvert.SerializeObject(customerAddress), ValidationType.Update);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customerAddress, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    foreach (var address in customerAddress)
                    {
                        var dbAddress = dbCustomerAddresses.FirstOrDefault(x => x.Id == address.AddressId);
                        if (dbAddress != null)
                        {
                            if (dbAddress.UpdateCount == address.UpdateCount)
                            {
                                dbAddress.Address = address.Address;
                                dbAddress.CityId = this._customerRepository.GetCityIdForName(address.City, address.County);
                                dbAddress.PostalCode = address.PostalCode;
                                dbAddress.Euvatprefix = address.EUVatPrefix;
                                dbAddress.VatTaxRegistrationNo = address.VatTaxRegNumber;
                                dbAddress.LastModification = DateTime.UtcNow;
                                dbAddress.ModifiedBy = address.ModifiedBy;
                                dbAddress.UpdateCount = dbAddress.UpdateCount.CalculateUpdateCount();
                                if (address.Contacts != null && address.Contacts.Count > 0)
                                    dbAddress.CustomerContact = SaveCustomerContacts(dbAddress.CustomerContact.ToList(), address.Contacts, ref errorMessages, ref validationMessages);

                                dbCustomerAddresses[dbCustomerAddresses.ToList().FindIndex(ind => ind.Id == address.AddressId)] = dbAddress;
                            }
                            else
                            {
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.CustAddr_UpdateCountMismatch.ToId(), this._messageDescriptions[MessageType.CustAddr_UpdateCountMismatch.ToId()].ToString()));
                            }
                        }
                        else
                        {
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InvalidAddressId.ToId(), this._messageDescriptions[MessageType.CustAddr_InvalidAddressId.ToId()].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerAddress);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAddresses;
        }

        private List<DBModel.CustomerAddress> DeleteCustomerAddress(List<DBModel.CustomerAddress> dbCustomerAddresses, IList<DomainModel.CustomerAddress> customerAddress, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerAddressValidationService.Validate(JsonConvert.SerializeObject(customerAddress), ValidationType.Delete);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(customerAddress, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    foreach (var addr in customerAddress)
                    {
                        var dbAddress = dbCustomerAddresses.FirstOrDefault(x => x.Id == addr.AddressId);
                        if (dbAddress != null && dbAddress.ContractDefaultCustomerInvoiceAddress.Count <= 0)
                        {
                            if (addr.Contacts != null && addr.Contacts.Count > 0)
                                dbAddress.CustomerContact = SaveCustomerContacts(dbAddress.CustomerContact.ToList(), addr.Contacts.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), ref errorMessages, ref validationMessages);

                            if (addr.AddressId > 0)
                            {
                                var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Customer_Address, SQLModuleActionType.Delete), addr.AddressId.ToString());
                                var count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);

                            }
                        }
                        else if (dbAddress != null && dbAddress.ContractDefaultCustomerInvoiceAddress.Count > 0)
                        {
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.CustAddr_InUse_Contract.ToId(), string.Format(_messageDescriptions[MessageType.CustAddr_InUse_Contract.ToId()].ToString(), dbAddress.Address)));
                        }
                        else
                        {
                            string errorCode = MessageType.CustAddr_PartialDelete.ToId();
                            errorMessages.Add(new MessageDetail(ModuleType.Customer, errorCode, _messageDescriptions[errorCode].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerAddress);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAddresses;
        }

        private List<DBModel.CustomerAddress> SaveContactsInAddressWithStatusNull(List<DBModel.CustomerAddress> dbCustomerAddresses, IList<DomainModel.CustomerAddress> customerAddress, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                foreach (var addr in customerAddress)
                {
                    var dbAddress = dbCustomerAddresses.FirstOrDefault(x => x.Id == addr.AddressId);
                    if (dbAddress != null)
                    {
                        if (addr.Contacts != null && addr.Contacts.Count > 0)
                            dbAddress.CustomerContact = SaveCustomerContacts(dbAddress.CustomerContact.ToList(), addr.Contacts, ref errorMessages, ref validationMessages);

                        dbCustomerAddresses[dbCustomerAddresses.ToList().FindIndex(ind => ind.Id == addr.AddressId)] = dbAddress;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerAddress);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerAddresses;
        }

        #endregion

        #region CustomerAddressContact

        private List<DBModel.CustomerContact> SaveCustomerContacts(List<DBModel.CustomerContact> dbCustomerContacts, IList<DomainModel.Contact> contacts, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<DBModel.CustomerContact> dbContacts = null;
            try
            {
                dbContacts = DeleteCustomerContact(dbCustomerContacts, contacts.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), ref errorMessages, ref validationMessages);
                dbContacts = UpdateCustomerContact(dbContacts, contacts.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), ref errorMessages, ref validationMessages);
                dbContacts = AddCustomerContact(dbContacts, contacts.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), ref errorMessages, ref validationMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbContacts;
        }

        private List<DBModel.CustomerContact> AddCustomerContact(List<DBModel.CustomerContact> dbCustomerContacts, IList<DomainModel.Contact> contacts, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerContactValidationService.Validate(JsonConvert.SerializeObject(contacts), ValidationType.Add);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(contacts, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    var dbContacts = _mapper.Map<IEnumerable<DomainModel.Contact>, IEnumerable<DBModel.CustomerContact>>(contacts);
                    dbCustomerContacts.AddRange(dbContacts);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerContacts;
        }

        private List<DBModel.CustomerContact> UpdateCustomerContact(List<DBModel.CustomerContact> dbCustomerContacts, IList<DomainModel.Contact> contacts, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerContactValidationService.Validate(JsonConvert.SerializeObject(contacts), ValidationType.Update);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(contacts, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    foreach (var custContact in contacts)
                    {
                        var dbContacts = dbCustomerContacts.FirstOrDefault(x => x.Id == custContact.ContactId);
                        if (dbContacts != null)
                        {
                            if (dbContacts.UpdateCount == custContact.UpdateCount)
                            {
                                dbContacts.Salutation = custContact.Salutation;
                                dbContacts.Position = custContact.Position;
                                dbContacts.ContactName = custContact.ContactPersonName;
                                dbContacts.TelephoneNumber = custContact.Landline;
                                dbContacts.FaxNumber = custContact.Fax;
                                dbContacts.MobileNumber = custContact.Mobile;
                                dbContacts.EmailAddress = custContact.Email;
                                dbContacts.OtherContactDetails = custContact.OtherDetail;
                                dbContacts.LastModification = DateTime.UtcNow;
                                dbContacts.ModifiedBy = custContact.ModifiedBy;
                                dbContacts.LoginName = custContact.LogonName;
                                dbContacts.CustomerAddressId = custContact.CustomerAddressId;  // To provide solution to defect id=80 [This needs to be checked properly]
                                dbContacts.UpdateCount = dbContacts.UpdateCount.CalculateUpdateCount();
                                dbCustomerContacts[dbCustomerContacts.ToList().FindIndex(ind => ind.Id == custContact.ContactId)] = dbContacts;
                            }
                            else
                            {
                                validationMessages.Add(new ValidationMessage(custContact, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_UpdateCountMismatch.ToId(), this._messageDescriptions[MessageType.Customer_Contact_UpdateCountMismatch.ToId()].ToString()) }));
                            }
                        }
                        else
                        {
                            validationMessages.Add(new ValidationMessage(custContact, new List<MessageDetail>() { new MessageDetail(ModuleType.Customer, MessageType.Customer_Contact_InvalidContactId.ToId(), this._messageDescriptions[MessageType.Customer_Contact_InvalidContactId.ToId()].ToString()) }));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerContacts;
        }

        private List<DBModel.CustomerContact> DeleteCustomerContact(List<DBModel.CustomerContact> dbCustomerContacts, IList<DomainModel.Contact> contacts, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            try
            {
                var valdResult = this._customerContactValidationService.Validate(JsonConvert.SerializeObject(contacts), ValidationType.Delete);
                if (valdResult.Count > 0)
                {
                    validationMessages.Add(new ValidationMessage(contacts, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                }
                else
                {
                    if (contacts?.Count > 0)
                    {
                        var contactIds = contacts.Select(x => x.ContactId.ToString()).Distinct().ToList();
                        var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Customer_Contact, SQLModuleActionType.Delete), string.Join(",", contactIds));
                        var count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contacts);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, ResponseType.Exception.ToId(), ex.Message));
            }
            return dbCustomerContacts;
        }

        #endregion

        #region Validation
        public List<MessageDetail> ValidateCustomerDetails(DBModel.Customer dbCustomer, DomainModel.CustomerDetail customerDetailModel)
        {
            List<MessageDetail> results = new List<MessageDetail>();
            if (dbCustomer != null)
            {
                if (customerDetailModel.Addresses != null && customerDetailModel.Addresses.Count > 0)
                {
                    var contactIds = customerDetailModel.Addresses.Where(c => c.Contacts != null).SelectMany(x => x.Contacts).Where(x => x.RecordStatus.IsRecordStatusDeleted()).Select(x2 => x2.ContactId).ToList();
                    var contactValidationResult = CheckCustomerContactIsInUse(contactIds);
                    if (contactValidationResult != null && contactValidationResult.Count > 0)
                    {
                        results.AddRange(contactValidationResult);
                    }

                    // IGO commented as per ITK Request #803 - allow duplicate Address as per EVO1 - Start 
                    //var addressDuplicateValidationResult = CheckIsDuplicateCustomerAddress(dbCustomer, customerDetailModel.Addresses.Where(x => x.RecordStatus.IsRecordStatusModified() || x.RecordStatus.IsRecordStatusNew()).ToList());
                    //if (addressDuplicateValidationResult != null && addressDuplicateValidationResult.Count > 0)
                    //{
                    //    results.AddRange(addressDuplicateValidationResult);
                    //}
                    // IGO commented as per ITK Request #803 - allow duplicate Address as per EVO1 - END

                    //TODO Need to check How to handle validation if both address and its child deleted
                    //var addressIds = customerDetailModel.Addresses.Where(x=>x.RecordStatus.IsRecordStatusDeleted()).Select(x2 => x2.AddressId).ToList();
                    //var addressValidationResult=CheckCustomerAddressIsInUse(dbCustomer.CustomerAddress.ToList(), addressIds);
                    //if (addressValidationResult != null && addressValidationResult.Count > 0)
                    //{
                    //    results.AddRange(addressValidationResult);
                    //}
                }

                if (customerDetailModel.AssignmentReferences != null && customerDetailModel.AssignmentReferences.Count > 0)
                {
                    var assRefDuplicateValidationResult = CheckIsDuplicateCustomerAssignmentReference(dbCustomer, customerDetailModel.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusModified() || x.RecordStatus.IsRecordStatusNew()).ToList());
                    if (assRefDuplicateValidationResult != null && assRefDuplicateValidationResult.Count > 0)
                    {
                        results.AddRange(assRefDuplicateValidationResult);
                    }
                }

                if (customerDetailModel.AccountReferences != null && customerDetailModel.AccountReferences.Count > 0)
                {
                    var accRefDuplicateValidationResult = CheckIsDuplicateCustomerAccountReference(dbCustomer, customerDetailModel.AccountReferences.Where(x => x.RecordStatus.IsRecordStatusModified() || x.RecordStatus.IsRecordStatusNew()).ToList());
                    if (accRefDuplicateValidationResult != null && accRefDuplicateValidationResult.Count > 0)
                    {
                        results.AddRange(accRefDuplicateValidationResult);
                    }
                }
            }
            return results.Count() > 0 ? results : null;
        }

        public List<MessageDetail> CheckCustomerContactIsInUse(List<int> contactIds)
        {
            List<MessageDetail> results = new List<MessageDetail>();
            string message = string.Empty;
            if (contactIds.Count > 0)
            {
                var contacts = _customerContactRepository.FindBy(x => contactIds.Contains(x.Id)).ToList();
                foreach (var contact in contacts)
                {
                    MessageType messageType = MessageType.Success;
                    if (contact.ContractDefaultCustomerContractContact.Any() || contact.ContractDefaultCustomerInvoiceContact.Any())
                        messageType = MessageType.Customer_Contact_InUse_Contract;
                    else if (contact.ProjectCustomerContact.Any() || contact.ProjectCustomerProjectContact.Any() || contact.ProjectClientNotification.Any())
                        messageType = MessageType.Customer_Contact_InUse_Project;
                    else if (contact.Assignment.Any())
                        messageType = MessageType.Customer_Contact_InUse_Assignment;
                    else if (contact.LoginName != null)
                        messageType = MessageType.Customer_Contact_Portal_Exists; //Changes for ITK D1469

                    if (messageType != MessageType.Success)
                        results.Add(new MessageDetail(ModuleType.Customer, messageType.ToId(), string.Format(_messageDescriptions[messageType.ToId()].ToString(), contact.ContactName)));

                }
            }
            return results.Count() > 0 ? results : null;
        }

        public List<MessageDetail> CheckIsDuplicateCustomerAddress(DBModel.Customer dbCustomer, IList<DomainModel.CustomerAddress> customerAddresses)
        {
            List<MessageDetail> validationResults = new List<MessageDetail>();
            if (customerAddresses.Count > 0)
            {
                foreach (var address in customerAddresses)
                {
                    if (dbCustomer.CustomerAddress.Count > 0 && dbCustomer.CustomerAddress.Any(x1 => x1.Address == address.Address && x1.Id != address.AddressId))
                    {
                        validationResults.Add(new MessageDetail(ModuleType.Customer, MessageType.CustAddr_ExisitingAddress.ToId(), this._messageDescriptions[MessageType.CustAddr_ExisitingAddress.ToId()].ToString()));

                    }
                }
            }
            return validationResults.Count() > 0 ? validationResults : null;
        }

        public List<MessageDetail> CheckIsDuplicateCustomerAssignmentReference(DBModel.Customer dbCustomer, IList<DomainModel.CustomerAssignmentReference> customerAssgnReferences)
        {
            List<MessageDetail> validationResults = new List<MessageDetail>();

            if (customerAssgnReferences.Count > 0)
            {
                foreach (var assRef in customerAssgnReferences)
                {
                    var dbCustomerAssgnReferences = _mapper.Map<DomainModel.CustomerAssignmentReference, DBModel.CustomerAssignmentReferenceType>(assRef);
                    if (dbCustomer.CustomerAssignmentReferenceType.Count > 0 && dbCustomer.CustomerAssignmentReferenceType.Any(x1 => x1.AssignmentReferenceId == dbCustomerAssgnReferences.AssignmentReferenceId))
                    {
                        validationResults.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_AssRef_AlreadyExists.ToId(), this._messageDescriptions[MessageType.Customer_AssRef_AlreadyExists.ToId()].ToString()));
                    }
                }
            }
            return validationResults.Count() > 0 ? validationResults : null;
        }

        public List<MessageDetail> CheckIsDuplicateCustomerAccountReference(DBModel.Customer dbCustomer, IList<DomainModel.CustomerCompanyAccountReference> customerAccountRefernces)
        {
            List<MessageDetail> validationResults = new List<MessageDetail>();

            if (customerAccountRefernces.Count > 0)
            {
                foreach (var accRef in customerAccountRefernces)
                {
                    var dbCustomerAccountReferences = _mapper.Map<DomainModel.CustomerCompanyAccountReference, DBModel.CustomerCompanyAccountReference>(accRef);
                    if (dbCustomer.CustomerAssignmentReferenceType.Count > 0 && dbCustomer.CustomerCompanyAccountReference.Any(x1 => x1.AccountReference == dbCustomerAccountReferences.AccountReference && x1.CompanyId == dbCustomerAccountReferences.CompanyId))
                    {
                        validationResults.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_ACRef_AlreadyExists.ToId(), this._messageDescriptions[MessageType.Customer_ACRef_AlreadyExists.ToId()].ToString()));
                    }
                }
            }
            return validationResults.Count() > 0 ? validationResults : null;
        }

        //public List<ValidationMessage> CheckCustomerAddressIsInUse(List<DBModel.CustomerAddress> dbCustomerAddresses, List<int> addressIds)
        //{
        //    List<ValidationMessage> validationResults = new List<ValidationMessage>();  
        //    if (addressIds.Count > 0)
        //    {
        //        var addresses = dbCustomerAddresses.Where(x => addressIds.Contains(x.Id)).ToList(); 
        //        foreach (var address in addresses)
        //        {
        //            if (address.CustomerContact.Any())
        //                validationResults.Add(new ValidationMessage(address, new List<MessageDetail>()
        //                {
        //                    new MessageDetail(MessageType.CustAddr_DeleteHasReference.ToId(), string.Format(_messageDescriptions[MessageType.CustAddr_DeleteHasReference.ToId()].ToString(), address.Address))
        //                }));
        //        }
        //    }
        //    return validationResults.Count() > 0 ? validationResults : null;
        //}


        private Response AuditLog(DomainModel.CustomerDetail customerDetail,
                              DBModel.Customer dbCustomer,
                              SqlAuditActionType sqlAuditActionType,
                              SqlAuditModuleType sqlAuditModuleType,
                              object oldData,
                              object newData,
                              ref long? eventId,
                              DomainModel.CustomerDetail dbCust,
                              List<UserInfo> users,
                              List<DBModel.Document> dbDocuments = null)
        {

            Exception exception = null;
            Response result = new Response();
            result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), "{" + AuditSelectType.Id + ":" + dbCustomer?.Id + "}${" + AuditSelectType.CustomerCode + ":" + dbCustomer?.Code?.Trim() + "}${" + AuditSelectType.Name + ":" + dbCustomer?.Name?.Trim() + "}${" + AuditSelectType.ParentName + ":" + dbCustomer?.ParentName?.Trim() + "}", sqlAuditActionType, SqlAuditModuleType.Customer, oldData, newData, customerDetail?.dbModule);
            result = customerDetail.Detail.RecordStatus.IsRecordStatusNew() ? this.ProcessAuditSave(customerDetail, dbCustomer, sqlAuditActionType, sqlAuditModuleType, oldData, newData, ref eventId, dbCust, dbDocuments) : this.AuditUpdate(customerDetail, dbCustomer, sqlAuditActionType, sqlAuditModuleType, oldData, newData, ref eventId, dbCust, users, dbDocuments);


            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }


        private Response ProcessAuditSave(DomainModel.CustomerDetail customerDetail,
                               DBModel.Customer dbCustomer,
                               SqlAuditActionType sqlAuditActionType,
                               SqlAuditModuleType sqlAuditModuleType,
                               object oldData,
                               object newData,
                               ref long? eventId,
                               DomainModel.CustomerDetail dbCust, 
                               List<DBModel.Document> dbDocuments = null)
        {

            Exception exception = null;
            Response result = null;

            if (customerDetail.Detail != null)
            {
                var newCustomerInfo = customerDetail.Detail.RecordStatus.IsRecordStatusNew();
                if (newCustomerInfo)
                {
                    newData = customerDetail.Detail;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.Customer, null, newData, customerDetail?.dbModule);
                }
            }

            if (customerDetail.Detail.RecordStatus.IsRecordStatusNew())
            {
                if (customerDetail.Detail != null)
                {
                    var newCompanies = customerDetail.Detail.RecordStatus.IsRecordStatusNew();
                    newData = customerDetail.Detail;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.Customer, oldData, newData, customerDetail?.dbModule);
                }
                if (customerDetail?.Addresses?.Count > 0)
                {
                    var newCustAddresses = customerDetail.Addresses.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCustAddresses.Count > 0)
                    {
                        var newaddress = _mapper.Map<List<DomainModel.CustomerAddress>>(dbCustomer?.CustomerAddress);
                        newData = newaddress?.Where(x => !dbCust.Addresses.Any(x1 => x1.AddressId == x.AddressId)).ToList();
                        result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.CustomerAddress, oldData, null, customerDetail?.dbModule);
                    }
                }
                if (customerDetail?.AssignmentReferences?.Count > 0)
                {
                    var newCustAssignmentRefs = customerDetail.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCustAssignmentRefs.Count > 0)
                    {
                        newData = newCustAssignmentRefs.Where(x => !dbCust.AssignmentReferences.Any(x1 => x1.CustomerAssignmentReferenceId == x.CustomerAssignmentReferenceId)).ToList();
                        result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.CustomerAssignmentReference, null, newData, customerDetail?.dbModule);
                    }
                }
                if (customerDetail?.AccountReferences?.Count > 0)
                {
                    var newCustAccountRefs = customerDetail.AccountReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCustAccountRefs.Count > 0)
                    {
                        var newAccRef = _mapper.Map<List<DomainModel.CustomerCompanyAccountReference>>(dbCustomer?.CustomerCompanyAccountReference);
                        newData = newAccRef?.Where(x => !dbCust.AccountReferences.Any(x1 => x1.CustomerCompanyAccountReferenceId == x.CustomerCompanyAccountReferenceId)).ToList();
                        result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.CustomerAccountReference, oldData, newData, customerDetail?.dbModule);
                    }
                }
                if (customerDetail?.Notes?.Count > 0)
                {
                    var newCustNotes = customerDetail.Notes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCustNotes.Count > 0)
                    {
                        var newNotes = _mapper.Map<List<DomainModel.CustomerNote>>(dbCustomer?.CustomerNote);
                        newData = newNotes?.Where(x => !dbCust.Notes.Any(x1 => x1.CustomerNoteId == x.CustomerNoteId)).ToList();
                        result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.CustomerNote, oldData, null, customerDetail?.dbModule); // Changes for Cutomer Note Audit
                    }
                }

                if (customerDetail?.Documents?.Count > 0)
                {
                    var newCustDocumentss = customerDetail.Documents.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCustDocumentss.Count > 0)
                    {
                        newData = newCustDocumentss.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                        result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.CustomerDocument, null, newData, customerDetail?.dbModule);
                    }
                }

            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }


        private Response AuditUpdate(DomainModel.CustomerDetail customerDetail,
                               DBModel.Customer dbCustomer,
                               SqlAuditActionType sqlAuditActionType,
                               SqlAuditModuleType sqlAuditModuleType,
                               object oldData,
                               object newData,
                               ref long? eventId,
                                DomainModel.CustomerDetail dbCust,
                                List<UserInfo> users,
                                List<DBModel.Document> dbDocuments = null)
        {
            Exception exception = null;
            Response result = null;
            var customerContact = customerDetail?.Addresses?.Where(x1=>x1.Contacts !=null)?.SelectMany(x => x.Contacts)?.ToList();
            if (customerDetail.Detail != null)
            {
                newData = customerDetail.Detail;
                oldData = dbCust?.Detail;
                result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.Customer, oldData, newData, customerDetail?.dbModule);
            }
            if (customerDetail?.Addresses?.Count > 0)
            {
                var newCustAddresses = customerDetail.Addresses.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiediCustAddresses = customerDetail.Addresses.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedAddresses = customerDetail.Addresses.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newCustAddresses.Count > 0)
                {
                    var newaddress = _mapper.Map<List<DomainModel.CustomerAddress>>(dbCustomer?.CustomerAddress);
                    newData = newaddress?.Where(x => !dbCust.Addresses.Any(x1 => x1.AddressId == x.AddressId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), dbCustomer?.Id + "$" + dbCustomer?.Code?.Trim() + "$" + dbCustomer?.Name?.Trim() + "$" + dbCustomer?.ParentName?.Trim(), sqlAuditActionType, SqlAuditModuleType.CustomerAddress, null, newData, customerDetail?.dbModule);
                }
                if (modifiediCustAddresses.Count > 0)
                {
                    newData = modifiediCustAddresses;
                    var Ids = modifiediCustAddresses?.ToList().Select(x => x.AddressId).ToList();
                    oldData = dbCust?.Addresses?.Where(x => Ids.Contains(x.AddressId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAddress, oldData, newData, customerDetail?.dbModule);
                }
                if (deletedAddresses.Count > 0)
                {
                    oldData = deletedAddresses;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAddress, oldData, null, customerDetail?.dbModule);
                }
            }

            if (customerDetail?.AssignmentReferences?.Count > 0)
            {
                var newCustAssignmentRefs = customerDetail.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiediAssignmentRefs = customerDetail.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedAssignmentRefs = customerDetail.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (newCustAssignmentRefs.Count > 0)
                {
                    var nweRef = _mapper.Map<List<DomainModel.CustomerAssignmentReference>>(dbCustomer?.CustomerAssignmentReferenceType);
                    newData = nweRef?.Where(x => !dbCust.AssignmentReferences.Any(x1 => x1.CustomerAssignmentReferenceId == x.CustomerAssignmentReferenceId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAssignmentReference, null, newData, customerDetail?.dbModule);
                }
                if (modifiediAssignmentRefs.Count > 0)
                {
                    newData = modifiediAssignmentRefs;
                    var Ids = modifiediAssignmentRefs?.ToList().Select(x => x.CustomerAssignmentReferenceId).ToList();
                    oldData = dbCust?.AssignmentReferences?.Where(x => Ids.Contains(x.CustomerAssignmentReferenceId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAssignmentReference, oldData, newData, customerDetail?.dbModule);
                }

                if (deletedAssignmentRefs.Count > 0)
                {
                    oldData = deletedAssignmentRefs;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAssignmentReference, oldData, null, customerDetail?.dbModule);
                }
            }
            if (customerDetail?.AccountReferences?.Count > 0)
            {
                var newCustAccountRefs = customerDetail.AccountReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifediAccountRefs = customerDetail.AccountReferences.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedAccountRefs = customerDetail.AccountReferences.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (newCustAccountRefs.Count > 0)
                {
                    var newAccRef = _mapper.Map<List<DomainModel.CustomerCompanyAccountReference>>(dbCustomer?.CustomerCompanyAccountReference);
                    newData = newAccRef?.Where(x => !dbCust.AccountReferences.Any(x1 => x1.CustomerCompanyAccountReferenceId == x.CustomerCompanyAccountReferenceId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAccountReference, null, newData, customerDetail?.dbModule);
                }
                if (modifediAccountRefs.Count > 0)
                {
                    newData = modifediAccountRefs;
                    var Ids = modifediAccountRefs?.ToList().Select(x => x.CustomerCompanyAccountReferenceId).ToList();
                    oldData = dbCust?.AccountReferences?.Where(x => Ids.Contains(x.CustomerCompanyAccountReferenceId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAccountReference, oldData, newData, customerDetail?.dbModule);
                }
                if (deletedAccountRefs.Count > 0)
                {
                    oldData = deletedAccountRefs;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerAccountReference, oldData, null, customerDetail?.dbModule);
                }
            }
            if (customerDetail?.Notes?.Count > 0)
            {
                var newCustNotes = customerDetail.Notes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedNotes = customerDetail.Notes.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedNotes = customerDetail.Notes.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newCustNotes.Count > 0)
                {
                    var newNotes = _mapper.Map<List<DomainModel.CustomerNote>>(dbCustomer?.CustomerNote);
                    newData = newNotes?.Where(x => !dbCust.Notes.Any(x1 => x1.CustomerNoteId == x.CustomerNoteId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerNote, null, newData, customerDetail?.dbModule);
                }
                if (modifiedNotes.Count > 0)
                {
                    newData = modifiedNotes;
                    var Ids = modifiedNotes?.ToList().Select(x => x.CustomerNoteId).ToList();
                    oldData = dbCust?.Notes?.Where(x => Ids.Contains(x.CustomerNoteId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerNote, oldData, newData, customerDetail?.dbModule);
                }
                if (deletedNotes.Count > 0)
                {
                    oldData = deletedNotes;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerNote, oldData, null, customerDetail?.dbModule);
                }
            }
            if (customerContact?.Count > 0)
            {
                var newSubSupplierTs = customerContact.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modify = customerContact.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var Delete = customerContact.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var oldContacts = dbCust?.Addresses?.SelectMany(x => x.Contacts)?.ToList();
                oldContacts?.ForEach(x =>
                {
                    //var isActive = _customerRepository.GetIsPortalUser(x.LogonName);
                    var isActive = users.Count(a => a.LogonName.Equals(x.LogonName)) > 0 ? users.FirstOrDefault(a => a.LogonName.Equals(x.LogonName)).IsActive : false;
                    x.IsPortalUser = isActive;
                });
                if (newSubSupplierTs.Count > 0)
                {
                    var newcustomerContact = _mapper.Map<List<DomainModel.Contact>>(dbCustomer?.CustomerAddress.SelectMany(x => x.CustomerContact));
                    newData = newcustomerContact?.Where(x => !oldContacts.Any(x1 => x1.ContactId == x.ContactId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerContact, null, newData, customerDetail?.dbModule); // D - 790
                }
                if (modify.Count > 0)
                {
                    newData = modify;
                    var Ids = modify?.Select(x => x.ContactId);
                    oldData = oldContacts?.Where(x => Ids.Contains(x.ContactId)).ToList();
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerContact, oldData, newData, customerDetail?.dbModule);
                }
                if (Delete.Count > 0)
                {
                    oldData = Delete;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerContact, oldData, null, customerDetail?.dbModule); // D - 790
                }
            }

            if (customerDetail?.Documents?.Count > 0)
            {
                var newCustDocuments = customerDetail.Documents.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedDocuments = customerDetail.Documents.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedDocuments = customerDetail.Documents.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newCustDocuments.Count > 0)
                {
                    newData = newCustDocuments.Select(x =>
                    {
                        x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                        return x;
                    }).ToList(); // audit create date issue fix
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerDocument, null, newData, customerDetail?.dbModule);
                }
                if (modifiedDocuments.Count > 0)
                {
                    newData = modifiedDocuments?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerDocument, oldData, newData, customerDetail?.dbModule);
                }
                if (deletedDocuments.Count > 0)
                {
                    oldData = deletedDocuments;
                    result = _auditSearchService.AuditLog(customerDetail, ref eventId, customerDetail?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CustomerDocument, oldData, null, customerDetail?.dbModule);
                }
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }


        #endregion

        private Response ProcessExtranetDetails(DomainModel.CustomerDetail customerDetailModel,ref List<ValidationMessage> messages)
        {
            Exception exception = null;
            long? eventId = null;
            try
            {
                if (customerDetailModel.Addresses != null && customerDetailModel.Addresses.Any(x => x.Contacts != null))
                {
                    var userCustomers = customerDetailModel?.Addresses?.Where(x => x.Contacts != null).SelectMany(x => x.Contacts)?.Where(x => x.UserInfo != null && (x.UserInfo.RecordStatus.IsRecordStatusNew() || x.UserInfo.RecordStatus.IsRecordStatusModified()))?.Select(x => x.UserInfo).ToList();
                    //Added Duplicate Logon Names Validation
                    if (userCustomers !=null)
                    {
                        var duplicates = userCustomers.GroupBy(x => x.LogonName).Where(x1 => x1.Count() > 1).Select(s => s.Key);
                        if (duplicates?.ToList()?.Count != 0)
                            messages.Add(_messageDescriptions, duplicates.FirstOrDefault(), MessageType.UserAlreadyExist, duplicates.FirstOrDefault());
                        
                    }
                    //Added Duplicate Logon Names Validation -End
                    if (userCustomers != null && userCustomers.Any(x => x.RecordStatus.IsRecordStatusNew()) && messages?.Count == 0)
                    {
                        var newrUserDetails = userCustomers.Where(x => x.RecordStatus.IsRecordStatusNew()).Select(x =>
                        {
                            x.ApplicationName = this._environment.SecurityAppName;
                            x.AuthenticationMode = LogonMode.UP.ToString();
                            x.RecordStatus = RecordStatus.New.FirstChar();
                            x.IsPasswordNeedToBeChange = false;
                            x.Culture = string.IsNullOrEmpty(x.Culture) ? "en-GB" : x.Culture;
                            return x;
                        }).Select(ud => new UserDetail()
                        {
                            User = ud,
                            CompanyUserTypes = new List<CompanyUserType>
                            {
                        new CompanyUserType {
                            CompanyCode = ud.CompanyCode,
                             UserTypes = new List<UserTypeInfo>
                            {
                                 new UserTypeInfo {
                                  UserType=Common.Enums.UserType.Customer.ToString(),
                                  RecordStatus = RecordStatus.New.FirstChar(),
                                  UserLogonName=ud.LogonName,
                                  CompanyCode = ud.CompanyCode,
                                  IsActive=true,
                                 }
                            }
                        }
                            }
                        }).ToList();

                        var res = _userDetailService.Add(newrUserDetails);

                        if (res.Code != MessageType.Success.ToId())
                        {
                            return res;
                        }
                    }
                    if (userCustomers != null && userCustomers.Any(x => x.RecordStatus.IsRecordStatusModified()) && messages?.Count == 0)
                    {
                        var updUserDetails = _mapper.Map<IList<UserInfo>>(userCustomers?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList())?.Select(x =>
                        {
                            x.ApplicationName = this._environment.SecurityAppName;
                            x.AuthenticationMode = LogonMode.UP.ToString();
                            x.IsPasswordNeedToBeChange = false;
                            return x;
                        }).ToList();
                        var res = _userService.Modify(updUserDetails, ref eventId);
                        if (res.Code != MessageType.Success.ToId())
                        {
                            return res;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDetailModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }
    }
}
