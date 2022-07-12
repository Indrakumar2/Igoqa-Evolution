using System;
using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using AutoMapper;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using Evolution.Company.Domain.Enums;
using System.Transactions;

namespace Evolution.Company.Core.Services
{
    public class CompanyEmailTemplateService : ICompanyEmailTemplateService
    {
        private readonly ICompanyEmailTemplateRepository _repository = null;
        private readonly IAppLogger<CompanyEmailTemplateService> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly JObject _MessageDescriptions = null;

        public CompanyEmailTemplateService(ICompanyEmailTemplateRepository companyEmailTemplateRepository, ICompanyRepository companyRepository, IAppLogger<CompanyEmailTemplateService> logger,JObject messages)
        {
            this._repository = companyEmailTemplateRepository;
            this._logger = logger;
            this._companyRepository = companyRepository;
            this._MessageDescriptions = messages;

        }

        #region Public Exposed Methods

        public Response GetCompanyEmailTemplate(string companyCode)
        {
            DomainModel.CompanyEmailTemplate result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = this._repository.Search(companyCode);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result == null ? 0 : 1);
        }

        public Response AddCompanyEmailTemplate(string companyCode, CompanyEmailTemplate companyEmailTemplate,ref IList<DbModels.CompanyMessage> msgToBeInsert,ref IList<DbModels.CompanyMessage> msgToBeUpdate,ref IList<DbModels.CompanyMessage> msgToBeDelete, bool commitChange = true)
        {
            return ProcessCompanyEmailTemplate(companyCode, companyEmailTemplate,ref msgToBeInsert,ref msgToBeUpdate,ref msgToBeDelete, commitChange);
        }

        public Response ModifyCompanyEmailTemplate(string companyCode, CompanyEmailTemplate companyEmailTemplate,ref IList<DbModels.CompanyMessage> msgToBeInsert,ref IList<DbModels.CompanyMessage> msgToBeUpdate,ref IList<DbModels.CompanyMessage>msgToBeDelete, bool commitChange = true)
        {
            return this.ProcessCompanyEmailTemplate(companyCode, companyEmailTemplate,ref msgToBeInsert,ref msgToBeUpdate,ref msgToBeDelete, commitChange);
        }

        #endregion

        #region Private Exposed Methods

        private Response ProcessCompanyEmailTemplate(string companyCode, DomainModel.CompanyEmailTemplate companyEmailTemplate,ref IList<DbModels.CompanyMessage> msgToBeInsert,ref IList<DbModels.CompanyMessage> msgToBeUpdate,ref IList<DbModels.CompanyMessage> msgToBeDelete, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
           
            DbModels.Company dbCompany = null;

            try
            {
                this._repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    msgToBeInsert = this.ProcessNewEmailTemplateMessage(dbCompany, companyEmailTemplate, ref errorMessages);
                    msgToBeUpdate = this.ProcessExistingEmailTemplateessage(dbCompany, companyEmailTemplate, ref msgToBeDelete, ref errorMessages).ToList();
                    if (errorMessages.Count <= 0)
                    {
                        if (msgToBeInsert?.Count > 0)
                            _repository.Add(msgToBeInsert);

                        if (msgToBeUpdate?.Count > 0)
                            _repository.Update(msgToBeUpdate);

                        if (msgToBeDelete?.Count > 0)
                            _repository.Update(msgToBeDelete);

                        if (commitChange && errorMessages.Count <= 0 && (msgToBeInsert?.Count > 0 || msgToBeUpdate?.Count > 0 || msgToBeDelete?.Count>0))
                            _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyEmailTemplate);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private bool IsValidCompany(string companyCode, ref DbModels.Company company, ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;

            if (string.IsNullOrEmpty(companyCode))
                messageType = MessageType.InvalidCompanyCode;
            else
            {
                company = _companyRepository.FindBy(x => x.Code == companyCode).FirstOrDefault();
                if (company == null)
                    messageType = MessageType.InvalidCompanyCode;
            }

            if (messageType != MessageType.Success)
                errorMessages.Add(new MessageDetail(MessageType.InvalidCompanyCode.ToId(), _MessageDescriptions[MessageType.InvalidCompanyCode.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private List<DbModels.CompanyMessage> ProcessNewEmailTemplateMessage(DbModels.Company dbCompany, DomainModel.CompanyEmailTemplate companyEmailTemplate, ref List<MessageDetail> errorMessages)
        {
            List<DbModels.CompanyMessage> dbMessages = new List<DbModels.CompanyMessage>();
            DbModels.CompanyMessage companyMessage = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailCustomerDirectReporting);
            if (companyMessage == null && !string.IsNullOrEmpty(companyEmailTemplate?.CustomerDirectReportingEmailText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.EmailCustomerDirectReporting, companyEmailTemplate.CustomerDirectReportingEmailText,companyEmailTemplate.ActionByUser));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailCustomerReportingNotification);
            if (companyMessage == null && !string.IsNullOrEmpty(companyEmailTemplate?.CustomerReportingNotificationEmailText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.EmailCustomerReportingNotification, companyEmailTemplate.CustomerReportingNotificationEmailText, companyEmailTemplate.ActionByUser));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailInterCompanyAssignmentToCoordinator);
            if (companyMessage == null && !string.IsNullOrEmpty(companyEmailTemplate?.InterCompanyOperatingCoordinatorEmail))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.EmailInterCompanyAssignmentToCoordinator, companyEmailTemplate.InterCompanyOperatingCoordinatorEmail, companyEmailTemplate.ActionByUser));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailRejectedVisit);
            if (companyMessage == null && !string.IsNullOrEmpty(companyEmailTemplate?.RejectVisitTimesheetEmailText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.EmailRejectedVisit, companyEmailTemplate.RejectVisitTimesheetEmailText, companyEmailTemplate.ActionByUser));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailVisitCompletedToCoordinator);
            if (companyMessage == null && !string.IsNullOrEmpty(companyEmailTemplate?.VisitCompletedCoordinatorEmailText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.EmailVisitCompletedToCoordinator, companyEmailTemplate.VisitCompletedCoordinatorEmailText, companyEmailTemplate.ActionByUser));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailInterCompanyDiscountAmendmentReason);
            if (companyMessage == null && !string.IsNullOrEmpty(companyEmailTemplate?.InterCompanyDiscountAmendmentReasonEmail))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.EmailInterCompanyDiscountAmendmentReason, companyEmailTemplate.InterCompanyDiscountAmendmentReasonEmail, companyEmailTemplate.ActionByUser));


            return dbMessages;
        }

        private IList<DbModels.CompanyMessage> ProcessExistingEmailTemplateessage(DbModels.Company dbCompany, DomainModel.CompanyEmailTemplate companyEmailTemplate, ref IList<DbModels.CompanyMessage> recordToBeDelete, ref List<MessageDetail> errorMessages)
        {
            List<DbModels.CompanyMessage> dbMessages = new List<DbModels.CompanyMessage>();
            DbModels.CompanyMessage companyMessage = null;

            if (companyEmailTemplate != null)
            {
                if (errorMessages == null)
                    errorMessages = new List<MessageDetail>();

                if (recordToBeDelete == null)
                    recordToBeDelete = new List<DbModels.CompanyMessage>();

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailCustomerDirectReporting);
                if (companyMessage != null && companyMessage.Message != companyEmailTemplate.CustomerDirectReportingEmailText) //D-979
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, companyEmailTemplate, companyEmailTemplate.CustomerDirectReportingEmailText, companyEmailTemplate.ActionByUser));
                //Commented for Sanity Defect 112
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailCustomerReportingNotification);
                if (companyMessage != null && companyMessage.Message != companyEmailTemplate.CustomerReportingNotificationEmailText)//D-979
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, companyEmailTemplate,companyEmailTemplate.CustomerReportingNotificationEmailText,companyEmailTemplate.ActionByUser));
                //Commented for Sanity Defect 112
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailInterCompanyAssignmentToCoordinator);
                if (companyMessage != null && companyMessage.Message != companyEmailTemplate.InterCompanyOperatingCoordinatorEmail)//D-979
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, companyEmailTemplate, companyEmailTemplate.InterCompanyOperatingCoordinatorEmail,companyEmailTemplate.ActionByUser));
                //Commented for Sanity Defect 112
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailRejectedVisit);
                if (companyMessage != null && companyMessage.Message != companyEmailTemplate.RejectVisitTimesheetEmailText)//D-979
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, companyEmailTemplate, companyEmailTemplate.RejectVisitTimesheetEmailText, companyEmailTemplate.ActionByUser));
                //Commented for Sanity Defect 112
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.EmailVisitCompletedToCoordinator);
                if (companyMessage != null && companyMessage.Message != companyEmailTemplate.VisitCompletedCoordinatorEmailText)//D-979
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, companyEmailTemplate,companyEmailTemplate.VisitCompletedCoordinatorEmailText, companyEmailTemplate.ActionByUser));
                //Commented for Sanity Defect 112
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);
            }
            return dbMessages;
        }
        private DbModels.CompanyMessage ConvertToDbCompanyMessage(int companyId, CompanyMessageType compnyMessageType,string msgText,string modifyBy)
        {
            bool? isDefaultMsg = null;
            bool isActive = true;
            string identifier = null;
            int? updateCount = null;
            var dbMessage = new DbModels.CompanyMessage()
            {
                CompanyId = companyId,
                Identifier = identifier,
                Message = msgText,
                IsDefaultMessage = isDefaultMsg,
                IsActive = isActive,
                MessageTypeId = (int)compnyMessageType,
                LastModification = DateTime.UtcNow,//D-979
                ModifiedBy = modifyBy,
                UpdateCount = Convert.ToByte(updateCount).CalculateUpdateCount()
        };

            return dbMessage;
        }

        private DbModels.CompanyMessage UpdateDbCompanyMessage(DbModels.CompanyMessage dbMessage, DomainModel.CompanyEmailTemplate companyEmailTemplate,string msgText,string modifyBy)
        {
            bool? isDefaultMsg = null;
            string identifier = null;
            bool isActive = true;
            dbMessage.Identifier = identifier;
            dbMessage.Message = msgText;
            dbMessage.IsDefaultMessage = isDefaultMsg;
            dbMessage.IsActive = isActive;
            dbMessage.LastModification = DateTime.UtcNow;//D-979
            dbMessage.ModifiedBy = modifyBy;
            dbMessage.UpdateCount = Convert.ToByte(dbMessage.UpdateCount).CalculateUpdateCount(); //D-979
            return dbMessage;
        }

        #endregion

        //#region Public Exposed Methods

        //public Response GetCompanyEmailTemplate(CompanyEmailTemplate searchModel)
        //{
        //    DomainModel.CompanyEmailTemplate result = null;
        //    Exception exception = null;
        //    try
        //    {
        //        result = this._repository.Search(searchModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result == null ? 0 : 1);
        //}

        //public Response SaveCompanyEmailTemplate(string companyCode, IList<CompanyEmailTemplate> companyEmailTemplates, bool commitChange = true)
        //{
        //    var result = this.AddCompanyEmailTemplate(companyCode, companyEmailTemplates, commitChange);
        //    if (result.Code == MessageType.Success.ToId())
        //        return this.GetCompanyEmailTemplate(new CompanyEmailTemplate() { CompanyCode = companyCode });
        //    else
        //        return result;
        //}

        //public Response UpdateCompanyEmailTemplate(string companyCode, IList<CompanyEmailTemplate> companyEmailTemplates, bool commitChange = true)
        //{
        //    var result = this.ModifyCompanyEmailTemplate(companyCode, companyEmailTemplates, commitChange);
        //    if (result.Code == MessageType.Success.ToId())
        //        return this.GetCompanyEmailTemplate(new CompanyEmailTemplate() { CompanyCode = companyCode });
        //    else
        //        return result;
        //}

        ////public Response DeleteCompanyTemplate(string companyCode, IList<CompanyEmailTemplate> companyEmailTemplates, bool commitChange = true)
        ////{
        ////    return this.DeleteCompanyEmailTemplate(companyCode, companyEmailTemplates, commitChange);
        ////}

        //#endregion

        //#region Private Exposed Methods

        //private Response AddCompanyEmailTemplate(string companyCode, IList<CompanyEmailTemplate> companyEmailTemplates, bool commitChange)
        //{
        //    Exception exception = null;
        //    List<MessageDetail> errorMessages = null;
        //    DbModels.Company dbCompany = null;
        //    IList<KeyValuePair<CompanyMessageType, string>> companyMessages = new List<KeyValuePair<CompanyMessageType, string>>();
        //    try
        //    {

        //        errorMessages = new List<MessageDetail>();
        //        if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
        //        {
        //            IList<CompanyEmailTemplate> recordToBeInserted = null;
        //            if (this.IsRecordValidForProcess(companyEmailTemplates, ValidationType.Add, ref recordToBeInserted, ref errorMessages))
        //            {
        //                foreach (var record in recordToBeInserted)
        //                {
        //                    var exists = false;
        //                    if (!string.IsNullOrEmpty(record.CustomerDirectReportingEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailCustomerDirectReporting);
        //                        if (!exists)
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailCustomerDirectReporting, record.CustomerDirectReportingEmailText));
        //                        else
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageAlreadyExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.CustomerDirectReportingEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.CustomerReportingNotificationEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailCustomerReportingNotification);
        //                        if (!exists)
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailCustomerReportingNotification, record.CustomerDirectReportingEmailText));
        //                        else
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageAlreadyExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.CustomerDirectReportingEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.RejectVisitTimesheetEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailRejectedVisit);
        //                        if (!exists)
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailRejectedVisit, record.RejectVisitTimesheetEmailText));
        //                        else
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageAlreadyExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.RejectVisitTimesheetEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.VisitCompletedCoordinatorEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailVisitCompletedToCoordinator);
        //                        if (!exists)
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailVisitCompletedToCoordinator, record.VisitCompletedCoordinatorEmailText));
        //                        else
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageAlreadyExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.VisitCompletedCoordinatorEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.InterCompanyOperatingCoordinatorEmail))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailInterCompanyAssignmentToCoordinator);
        //                        if (!exists)
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailInterCompanyAssignmentToCoordinator, record.InterCompanyOperatingCoordinatorEmail));
        //                        else
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageAlreadyExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.InterCompanyOperatingCoordinatorEmail));
        //                    }
        //                    SaveRecordToDb(dbCompany.Id, companyMessages, null, ValidationType.Add);
        //                    companyMessages.Clear();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : validations on Exception
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyEmailTemplates);
        //    }
        //    finally
        //    {
        //        _repository.AutoSave = true;
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        //}

        //private Response DeleteCompanyEmailTemplate(string companyCode, IList<CompanyEmailTemplate> companyEmailTemplates, bool commitChange)
        //{
        //    Exception exception = null;
        //    List<MessageDetail> errorMessages = null;
        //    DbModels.Company dbCompany = null;
        //    IList<KeyValuePair<CompanyMessageType, string>> companyMessages = new List<KeyValuePair<CompanyMessageType, string>>();
        //    try
        //    {

        //        errorMessages = new List<MessageDetail>();
        //        if (IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
        //        {
        //            IList<CompanyEmailTemplate> recordsToDelete = null;
        //            if (IsRecordValidForProcess(companyEmailTemplates, ValidationType.Delete, ref recordsToDelete, ref errorMessages))
        //            {
        //                foreach (var record in recordsToDelete)
        //                {
        //                    var exists = false;
        //                    if (!string.IsNullOrEmpty(record.CustomerDirectReportingEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailCustomerDirectReporting);
        //                        // TODO: To Change Validation For the Company Email template Delete when Record Not exists
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageDoesNotExists].ToString(), record.CustomerDirectReportingEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailCustomerDirectReporting, record.CustomerDirectReportingEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.CustomerReportingNotificationEmailText))
        //                    {
        //                        // TODO: To Change Validation For the Company Email template Delete when Record Not exists

        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailCustomerReportingNotification);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageDoesNotExists].ToString(), record.CustomerReportingNotificationEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailCustomerReportingNotification, record.CustomerDirectReportingEmailText));

        //                    }
        //                    if (!string.IsNullOrEmpty(record.RejectVisitTimesheetEmailText))
        //                    {
        //                        // TODO: To Change the validation of Company Email template Delete when Record Not exists

        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailRejectedVisit);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageDoesNotExists].ToString(), record.RejectVisitTimesheetEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailRejectedVisit, record.RejectVisitTimesheetEmailText));

        //                    }
        //                    if (!string.IsNullOrEmpty(record.VisitCompletedCoordinatorEmailText))
        //                    {
        //                        // TODO: To Change the validation of Company Email template Delete when Record Not exists

        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailVisitCompletedToCoordinator);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageDoesNotExists].ToString(), record.VisitCompletedCoordinatorEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailVisitCompletedToCoordinator, record.VisitCompletedCoordinatorEmailText));

        //                    }
        //                    if (!string.IsNullOrEmpty(record.InterCompanyOperatingCoordinatorEmail))
        //                    {
        //                        // TODO: To Change the validation of Company Email template Delete when Record Not exists

        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailInterCompanyAssignmentToCoordinator);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageDoesNotExists].ToString(), record.InterCompanyOperatingCoordinatorEmail));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailInterCompanyAssignmentToCoordinator, record.InterCompanyOperatingCoordinatorEmail));
        //                    }
        //                    SaveRecordToDb(dbCompany.Id, companyMessages, null, ValidationType.Delete);
        //                    companyMessages.Clear();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyEmailTemplates);
        //    }
        //    finally
        //    {
        //        _repository.AutoSave = true;
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);

        //}

        //private Response ModifyCompanyEmailTemplate(string companyCode, IList<CompanyEmailTemplate> companyEmailTemplates, bool commitChange)
        //{
        //    Exception exception = null;
        //    List<MessageDetail> errorMessages = null;
        //    DbModels.Company dbCompany = null;
        //    IList<KeyValuePair<CompanyMessageType, string>> companyMessages = new List<KeyValuePair<CompanyMessageType, string>>();
        //    try
        //    {

        //        errorMessages = new List<MessageDetail>();
        //        if (IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
        //        {
        //            IList<CompanyEmailTemplate> recordsToUpdate = null;
        //            if (IsRecordValidForProcess(companyEmailTemplates, ValidationType.Update, ref recordsToUpdate, ref errorMessages))
        //            {
        //                foreach (var record in recordsToUpdate)
        //                {
        //                    var exists = false;
        //                    if (!string.IsNullOrEmpty(record.CustomerDirectReportingEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailCustomerDirectReporting);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.CustomerDirectReportingEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailCustomerDirectReporting, record.CustomerDirectReportingEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.CustomerReportingNotificationEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailCustomerReportingNotification);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.CustomerReportingNotificationEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailCustomerReportingNotification, record.CustomerDirectReportingEmailText));
        //                    }
        //                    if (!string.IsNullOrEmpty(record.RejectVisitTimesheetEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailRejectedVisit);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.RejectVisitTimesheetEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailRejectedVisit, record.RejectVisitTimesheetEmailText));

        //                    }
        //                    if (!string.IsNullOrEmpty(record.VisitCompletedCoordinatorEmailText))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailVisitCompletedToCoordinator);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.VisitCompletedCoordinatorEmailText));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailVisitCompletedToCoordinator, record.VisitCompletedCoordinatorEmailText));

        //                    }
        //                    if (!string.IsNullOrEmpty(record.InterCompanyOperatingCoordinatorEmail))
        //                    {
        //                        exists = IsCompanyEmailTemplateAlreadyAssociatedToCompany(companyCode, CompanyMessageType.EmailInterCompanyAssignmentToCoordinator);
        //                        if (!exists)
        //                            errorMessages.Add(new MessageDetail(MessageType.CompanyMessageDoesNotExists, _MessageDescriptions[MessageType.CompanyMessageAlreadyExists.ToId()].ToString(), record.InterCompanyOperatingCoordinatorEmail));
        //                        else
        //                            companyMessages.Add(new KeyValuePair<CompanyMessageType, string>(CompanyMessageType.EmailInterCompanyAssignmentToCoordinator, record.InterCompanyOperatingCoordinatorEmail));
        //                    }
        //                    SaveRecordToDb(dbCompany.Id, companyMessages, null, ValidationType.Update);
        //                    companyMessages.Clear();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyEmailTemplates);
        //    }
        //    finally
        //    {
        //        _repository.AutoSave = true;
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);

        //}

        //private bool IsCompanyEmailTemplateAlreadyAssociatedToCompany(string companyCode, CompanyMessageType type)
        //{
        //    var result = _repository.FindBy(x => x.Company.Code == companyCode && x.MessageTypeId == (int)type).FirstOrDefault();
        //    return result == null ? false : true;
        //}

        //private bool IsValidCompany(string companyCode, ref DbRepository.Models.Company company, ref List<MessageDetail> errorMessages)
        //{
        //    MessageType messageType = MessageType.Success;

        //    if (string.IsNullOrEmpty(companyCode))
        //        messageType = MessageType.InvalidCompanyCode;
        //    else
        //    {
        //        company = _companyRepository.FindBy(x => x.Code == companyCode).FirstOrDefault();
        //        if (company == null)
        //            messageType = MessageType.InvalidCompanyCode;
        //    }

        //    if (messageType != MessageType.Success)
        //        errorMessages.Add(new MessageDetail(MessageType.InvalidCompanyCode.ToId(), _MessageDescriptions[MessageType.InvalidCompanyCode.ToId()].ToString()));

        //    return messageType == MessageType.Success;
        //}

        //private bool IsRecordValidForProcess(IList<CompanyEmailTemplate> companyEmailTemplates, ValidationType validationType, ref IList<CompanyEmailTemplate> filteredCompanyEmailTemplate, ref List<MessageDetail> errorMessages)
        //{
        //    if (validationType == ValidationType.Add)
        //        filteredCompanyEmailTemplate = companyEmailTemplates?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
        //    else if (validationType == ValidationType.Delete)
        //        filteredCompanyEmailTemplate = companyEmailTemplates?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
        //    else if (validationType == ValidationType.Update)
        //        filteredCompanyEmailTemplate = companyEmailTemplates?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

        //    return filteredCompanyEmailTemplate?.Count > 0;
        //}

        //private void SaveRecordToDb(int companyId, IList<KeyValuePair<CompanyMessageType, string>> emailTemplates, byte? updateCount, ValidationType validationType)
        //{
        //    foreach (var emailTemplate in emailTemplates)
        //    {


        //        if (validationType == ValidationType.Add)
        //        {
        //            var dbEmailTemplate = new DbModels.CompanyMessage()
        //            {
        //                CompanyId = companyId,
        //                MessageTypeId = (int)emailTemplate.Key,
        //                Message = emailTemplate.Value,
        //                IsDefaultMessage = false,
        //                Identifier = null,
        //                LastModification = DateTime.Now,
        //                UpdateCount = null,
        //                ModifiedBy = null
        //            };
        //            _repository.Add(dbEmailTemplate);

        //        }

        //        else if (validationType == ValidationType.Update)
        //        {

        //            var dbEmailTemplate = _repository.FindBy(x => x.MessageTypeId == (int)emailTemplate.Key && x.CompanyId == companyId).FirstOrDefault();
        //            dbEmailTemplate.Message = emailTemplate.Value;
        //            dbEmailTemplate.IsDefaultMessage = false;
        //            dbEmailTemplate.Identifier = null;
        //            dbEmailTemplate.LastModification = DateTime.Now;
        //            dbEmailTemplate.UpdateCount = Convert.ToByte(dbEmailTemplate.UpdateCount == null ? 1 : dbEmailTemplate.UpdateCount + 1);
        //            dbEmailTemplate.ModifiedBy = null;
        //            _repository.Update(dbEmailTemplate);
        //        }
        //        else if (validationType == ValidationType.Delete)
        //        {

        //            var dbEmailTemplate = _repository.FindBy(x => x.MessageTypeId == (int)emailTemplate.Key && x.CompanyId == companyId).FirstOrDefault();

        //            _repository.Delete(dbEmailTemplate);
        //        }

        //    }
        //}
        //#endregion
    }
}
