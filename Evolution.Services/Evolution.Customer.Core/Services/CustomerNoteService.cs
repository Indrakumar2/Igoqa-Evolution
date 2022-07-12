using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Core.Services
{
    public class CustomerNoteService : ICustomerNoteService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<CustomerNoteService> _logger = null;
        private readonly ICustomerNoteRepository _customerNoteRepository = null;
        private readonly ICustomerNoteValidationService _validationService = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly JObject _MessageDescriptions = null;

        public CustomerNoteService(IMapper mapper, IAppLogger<CustomerNoteService> logger, ICustomerNoteRepository customerNoteRepository,
                            ICustomerNoteValidationService validationService, ICustomerRepository customerRepository, JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._customerNoteRepository = customerNoteRepository;
            this._validationService = validationService;
            this._customerRepository = customerRepository;
            this._MessageDescriptions = messages;

        }

        public Response DeleteCustomerNote(string customerCode, IList<DomainModel.CustomerNote> customerNotes)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            object result = null;
            try
            {
                result = new List<DomainModel.CustomerNote>();
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
                    Response deleteResponse = DeleteCustomerNotes(dbCustomer.Id, customerNotes.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList());
                    if (deleteResponse.Code != ResponseType.Success.ToId())
                        return deleteResponse;
                    else
                        result = deleteResponse.Result;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response GetCustomerNote(string customerCode, DomainModel.CustomerNote searchModel)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<DomainModel.CustomerNote> result = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                DbModel.CustomerNote dbCustomerNote = null;

                if (!string.IsNullOrWhiteSpace(customerCode))
                {

                    var dbCustomer = this._customerRepository.FindBy(x => x.Code == customerCode).FirstOrDefault();

                    if (dbCustomer == null)
                    {
                        responseType = (ResponseType)MessageType.Customer_Note_InvalidCustomerCode;
                        this._logger.LogError(responseType.ToId(), this._MessageDescriptions[responseType.ToId()].ToString(), customerCode);
                        validationMessages.Add(new ValidationMessage(customerCode, new List<MessageDetail>()
                        {
                            new MessageDetail(ModuleType.Customer,responseType.ToId(),this._MessageDescriptions[responseType.ToId()].ToString())
                        }));
                        return new Response().ToPopulate(responseType, validationMessages, errorMessages, result);
                    }

                    //Get customer Id from customer code
                    int customerId = dbCustomer.Id;

                    if (searchModel != null)
                    {
                        dbCustomerNote = _mapper.Map<DomainModel.CustomerNote, DbModel.CustomerNote>(searchModel);
                        dbCustomerNote.CustomerId = customerId;
                    }
                    else
                    {
                        // To  get all customer address for specific customer Id
                        dbCustomerNote = new DbModel.CustomerNote
                        {
                            CustomerId = customerId
                        };
                    }
                    result = _customerNoteRepository.Get(dbCustomerNote);

                }
                else
                {
                    responseType = (ResponseType)MessageType.Customer_Note_InvalidCustomerCode;
                    this._logger.LogError(responseType.ToId(), this._MessageDescriptions[responseType.ToId()].ToString(), customerCode);
                    errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), this._MessageDescriptions[responseType.ToId()].ToString()));
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(sqlE.ErrorCode.ToString(), sqlE.Message, searchModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), sqlE.Message));
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, searchModel);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }

            return new Response().ToPopulate(responseType, validationMessages, errorMessages, result);

        }

        public Response ModifyCustomerNote(string customerCode, IList<DomainModel.CustomerNote> customerNotes)
        {
            ResponseType responseType = ResponseType.Success;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessage = null;
            List<DomainModel.CustomerNote> result = null;
            List<string> responseStatuses = null;
            try
            {
                result = new List<DomainModel.CustomerNote>();
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
                    Response insertResult = AddCustomerNotes(dbCustomer.Id, customerNotes.Where(c => c.RecordStatus.IsRecordStatusNew()).ToList());
                    responseStatuses.Add(insertResult.Code);
                    validationMessage.AddRange(insertResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(insertResult.Messages?.ToList());
                    if (insertResult.Result != null)
                        result.AddRange((List<DomainModel.CustomerNote>)insertResult.Result);

                    Response updateResult = UpdateCustomerNotes(dbCustomer.Id, customerNotes.Where(c => c.RecordStatus.IsRecordStatusModified()).ToList());
                    responseStatuses.Add(updateResult.Code);
                    validationMessage.AddRange(updateResult.ValidationMessages?.ToList());
                    errorMessages.AddRange(updateResult.Messages?.ToList());
                    if (updateResult.Result != null)
                        result.AddRange((List<DomainModel.CustomerNote>)updateResult.Result);

                    Response deleteResult = DeleteCustomerNotes(dbCustomer.Id, customerNotes.Where(c => c.RecordStatus.IsRecordStatusDeleted()).ToList());
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
                this._logger.LogError(responseType.ToId(), ex.GetBaseException().Message, customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.GetBaseException().Message));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        public Response SaveCustomerNote(string customerCode, IList<DomainModel.CustomerNote> customerNotes)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            object result = null;
            try
            {
                result = new List<DomainModel.CustomerNote>();
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
                    Response insertResponse = AddCustomerNotes(dbCustomer.Id, customerNotes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList());
                    if (insertResponse.Code != ResponseType.Success.ToId())
                        return insertResponse;
                    else
                        result = insertResponse.Result;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.Message, customerNotes);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), ex.Message));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, result);
        }

        private Response AddCustomerNotes(int customerId, IList<DomainModel.CustomerNote> noteModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerNote> result = null;
            bool anyCustomerNoteSaved = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                result = new List<DbModel.CustomerNote>();

                if (noteModels.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(noteModels), ValidationType.Add);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(noteModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var note in noteModels)
                        {
                            var dbCustomerNote = _mapper.Map<DomainModel.CustomerNote, DbModel.CustomerNote>(note);
                            dbCustomerNote.Id = 0;
                            dbCustomerNote.CustomerId = customerId;
                            dbCustomerNote.CreatedDate = note.CreatedOn;
                            dbCustomerNote.UpdateCount = 0;
                            _customerNoteRepository.Add(dbCustomerNote);
                            result.Add(dbCustomerNote);
                            anyCustomerNoteSaved = true;
                        }
                    }

                }

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerNoteSaved)
                    responseType = ResponseType.PartiallySuccess;
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), noteModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), _MessageDescriptions[responseType.ToId()].ToString()));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.CustomerNote>>(result).ToList());
        }

        private Response UpdateCustomerNotes(int customerId, IList<DomainModel.CustomerNote> noteModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            IList<DbModel.CustomerNote> result = new List<DbModel.CustomerNote>();
            bool anyCustomerNoteUpdated = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();

                if (customerId > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(noteModels), ValidationType.Update);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(noteModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var custNote in noteModels)
                        {
                            var custNotesEntity = this._customerNoteRepository.FindBy(c => c.CustomerId == customerId && c.Id == custNote.CustomerNoteId).FirstOrDefault();
                            if (custNotesEntity != null)
                            {
                                if (custNotesEntity.UpdateCount == custNote.UpdateCount)
                                {
                                    custNotesEntity.CreatedBy = custNote.CreatedBy;
                                    custNotesEntity.Note = custNote.Note;
                                    custNotesEntity.LastModification = DateTime.UtcNow;
                                    custNotesEntity.ModifiedBy = custNote.ModifiedBy;
                                    custNotesEntity.UpdateCount = custNotesEntity.UpdateCount.CalculateUpdateCount();
                                    this._customerNoteRepository.Update(custNotesEntity);
                                    result.Add(custNotesEntity);
                                    anyCustomerNoteUpdated = true;
                                }
                                else
                                {
                                    responseType = ResponseType.Error;
                                    errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_Note_UpdateCountMismatch.ToId(), _MessageDescriptions[MessageType.Customer_Note_UpdateCountMismatch.ToId()].ToString()));
                                }
                            }
                            else
                            {
                                responseType = ResponseType.Error;
                                errorMessages.Add(new MessageDetail(ModuleType.Customer, MessageType.Customer_Note_InvalidNoteId.ToId(), _MessageDescriptions[MessageType.Customer_Note_InvalidNoteId.ToId()].ToString()));
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

                if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerNoteUpdated)
                    responseType = ResponseType.PartiallySuccess;

            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), noteModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), _MessageDescriptions[responseType.ToId()].ToString()));
            }

            return new Response().ToPopulate(responseType, validationMessage, errorMessages, _mapper.Map<IList<DomainModel.CustomerNote>>(result).ToList());
        }

        private Response DeleteCustomerNotes(int customerId, IList<DomainModel.CustomerNote> noteModels)
        {
            ResponseType responseType = ResponseType.Success;
            IList<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessage = null;
            bool anyCustomerNoteDeleted = false;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessage = new List<ValidationMessage>();
                if (noteModels.Count > 0)
                {
                    var valdResult = this._validationService.Validate(JsonConvert.SerializeObject(noteModels), ValidationType.Delete);
                    if (valdResult.Count > 0)
                    {
                        responseType = ResponseType.Validation;
                        validationMessage.Add(new ValidationMessage(noteModels, valdResult.Select(x => new MessageDetail(ModuleType.Customer, x.Code, x.Message)).ToList()));
                    }
                    else
                    {
                        foreach (var custNote in noteModels)
                        {
                            var custNoteEntity = this._customerNoteRepository.FindBy(c => c.CustomerId == customerId && c.Id == custNote.CustomerNoteId).FirstOrDefault();
                            if (custNoteEntity != null)
                            {
                                this._customerNoteRepository.Delete(custNoteEntity);
                                anyCustomerNoteDeleted = true;
                            }
                        }
                    }

                    if ((validationMessage.Count > 0 || errorMessages.Count > 0) && anyCustomerNoteDeleted)
                        responseType = ResponseType.PartiallySuccess;
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), ex.ToFullString(), noteModels);
                errorMessages.Add(new MessageDetail(ModuleType.Customer, responseType.ToId(), _MessageDescriptions[responseType.ToId()].ToString()));
            }
            return new Response().ToPopulate(responseType, validationMessage, errorMessages, null);
        }
    }
}
