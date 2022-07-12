using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Enums;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Core.Services
{
    public class CompanyInvoiceService : ICompanyInvoiceService
    {
        private readonly ICompanyInvoiceRepository _repository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IAppLogger<CompanyInvoiceService> _logger = null;
        private readonly JObject _MessageDescriptions = null;

        public CompanyInvoiceService(ICompanyInvoiceRepository repository, ICompanyRepository companyRepository, IAppLogger<CompanyInvoiceService> logger,JObject messages)
        {
            this._repository = repository;
            this._logger = logger;
            this._companyRepository = companyRepository;
            this._MessageDescriptions = messages;
        }

        #region Public Exposed Methods

        public Response GetCompanyInvoice(string companyCode)
        {
            DomainModel.CompanyInvoice result = null;
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

        public Response AddCompanyInvoice(string companyCode, CompanyInvoice companyInvoice, ref List<DbModels.CompanyMessage> msgToBeInsert ,ref List<DbModels.CompanyMessage> msgToBeUpdate,
                                                   ref List<DbModels.CompanyMessage> msgToBeDelete, bool commitChange = true)
        {
            return ProcessCompanyInvoice(companyCode, companyInvoice,ref msgToBeInsert,ref msgToBeUpdate,ref msgToBeDelete,  commitChange);
        }

        public Response ModifyCompanyInvoice(string companyCode, CompanyInvoice companyInvoice, ref List<DbModels.CompanyMessage> msgToBeInsert ,ref List<DbModels.CompanyMessage> msgToBeUpdate,
                                                   ref List<DbModels.CompanyMessage> msgToBeDelete, bool commitChange = true)
        {
            return this.ProcessCompanyInvoice(companyCode, companyInvoice,ref msgToBeInsert,ref msgToBeUpdate,ref msgToBeDelete, commitChange);
        }

        #endregion

        #region Private Exposed Methods

        private Response ProcessCompanyInvoice(string companyCode, DomainModel.CompanyInvoice companyInvoice,  
                                                ref List<DbModels.CompanyMessage> msgToBeInsert ,ref List<DbModels.CompanyMessage> msgToBeUpdate,
                                                   ref List<DbModels.CompanyMessage> msgToBeDelete,bool commitChange)
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
                    msgToBeDelete = this.ProcessRemovedRemittanceAndFooterMessage(dbCompany, companyInvoice, ref errorMessages).ToList();
                    msgToBeInsert = this.ProcessNewInvoiceMessage(dbCompany, companyInvoice, ref errorMessages);
                    msgToBeUpdate = this.ProcessExistingInvoiceMessage(dbCompany, companyInvoice,ref msgToBeDelete, ref errorMessages).ToList();
                    List<DbModels.CompanyMessage> msgToInsertRF = this.ProcessNewRemittanceFooterMessage(dbCompany, companyInvoice, ref errorMessages); //Changes for IGO D905
                    List<DbModels.CompanyMessage> messageToInsert = new List<DbModels.CompanyMessage>(); //Changes for IGO D905
                    if (msgToBeInsert?.Count > 0)
                        messageToInsert = msgToBeInsert;
                    if (msgToInsertRF?.Count > 0)
                        messageToInsert.AddRange(msgToInsertRF);
                    if (errorMessages.Count <= 0)
                    {
                        //if (msgToBeInsert?.Count > 0)
                        //    _repository.Add(msgToBeInsert);

                        if (messageToInsert?.Count > 0)
                            _repository.Add(messageToInsert); //Changes for IGO D905

                        if (msgToBeUpdate?.Count > 0)
                            _repository.Update(msgToBeUpdate);

                        if (msgToBeDelete?.Count > 0)
                            _repository.Delete(msgToBeDelete);

                        if (commitChange && errorMessages.Count <= 0 && (msgToBeInsert?.Count > 0 || msgToBeUpdate?.Count > 0 || msgToBeDelete?.Count > 0))
                            _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyInvoice);
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

        private List<DbModels.CompanyMessage> ProcessNewInvoiceMessage(DbModels.Company dbCompany, DomainModel.CompanyInvoice companyInvoice, ref List<MessageDetail> errorMessages)
        {
            List<DbModels.CompanyMessage> dbMessages = new List<DbModels.CompanyMessage>();
            DbModels.CompanyMessage companyMessage = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceDraftText);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceDraftText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceDraftText, companyInvoice.InvoiceDraftText));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InterCompanyText);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceInterCompText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InterCompanyText, companyInvoice.InvoiceInterCompText));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.FreeText);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceSummarryText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.FreeText, companyInvoice.InvoiceSummarryText));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InterCompanyDraftText);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceInterCompDraftText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InterCompanyDraftText, companyInvoice.InvoiceInterCompDraftText));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceDescriptionText);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceDescriptionText))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceDescriptionText, companyInvoice.InvoiceDescriptionText));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InterCompanyDescription);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceInterCompDescription))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InterCompanyDescription, companyInvoice.InvoiceInterCompDescription));

            //companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.LogoText);
            //if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceLogoName))
            //    dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.LogoText, companyInvoice.InvoiceLogoName));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceHeader);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.InvoiceHeader))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceHeader, companyInvoice.InvoiceHeader));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.ReverseChargeDisclaimer);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.ReverseChargeDisclaimer))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.ReverseChargeDisclaimer, companyInvoice.ReverseChargeDisclaimer));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.TechSpecialistExtranetComment);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.TechSpecialistExtranetComment))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.TechSpecialistExtranetComment, companyInvoice.TechSpecialistExtranetComment));

            companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.CustomerExtranetComment);
            if (companyMessage == null && !string.IsNullOrEmpty(companyInvoice?.CustomerExtranetComment))
                dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.CustomerExtranetComment, companyInvoice.CustomerExtranetComment));

            //if (companyInvoice?.InvoiceRemittances?.Count > 0)
            //{
            //    foreach (var item in companyInvoice.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusNew()))
            //    {
            //        companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceRemittanceText && x.Identifier == item?.MsgIdentifier && x.Message == item?.MsgText);
            //        if (companyMessage == null && !string.IsNullOrEmpty(item?.MsgText))
            //            dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceRemittanceText, msgText: item.MsgText, modifyBy: item.ModifiedBy, identifier: item.MsgIdentifier, isDefaultMsg: item.IsDefaultMsg, isActive: item.IsActive));
            //    }
            //}

            //if (companyInvoice?.InvoiceFooters?.Count > 0)
            //{
            //    foreach (var item in companyInvoice.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusNew()))
            //    {
            //        companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceFooterText && x.Identifier== item?.MsgIdentifier && x.Message == item?.MsgText);
            //        if (companyMessage == null && !string.IsNullOrEmpty(item?.MsgText))
            //            dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceFooterText, msgText: item.MsgText, modifyBy: item.ModifiedBy, identifier: item.MsgIdentifier, isDefaultMsg: item.IsDefaultMsg, isActive: item.IsActive));
            //    }
            //}

            return dbMessages;
        }

        private List<DbModels.CompanyMessage> ProcessNewRemittanceFooterMessage(DbModels.Company dbCompany, DomainModel.CompanyInvoice companyInvoice, ref List<MessageDetail> errorMessages) //Changes for IGO D905
        {
            List<DbModels.CompanyMessage> dbMessages = new List<DbModels.CompanyMessage>();
            DbModels.CompanyMessage companyMessage = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (companyInvoice?.InvoiceRemittances?.Count > 0)
            {
                foreach (var item in companyInvoice.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusNew()))
                {
                    companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceRemittanceText && x.Identifier == item?.MsgIdentifier && x.Message == item?.MsgText);
                    if (companyMessage == null && !string.IsNullOrEmpty(item?.MsgText))
                        dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceRemittanceText, msgText: item.MsgText, modifyBy: item.ModifiedBy, identifier: item.MsgIdentifier, isDefaultMsg: item.IsDefaultMsg, isActive: item.IsActive));
                }
            }

            if (companyInvoice?.InvoiceFooters?.Count > 0)
            {
                foreach (var item in companyInvoice.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusNew()))
                {
                    companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceFooterText && x.Identifier== item?.MsgIdentifier && x.Message == item?.MsgText);
                    if (companyMessage == null && !string.IsNullOrEmpty(item?.MsgText))
                        dbMessages.Add(this.ConvertToDbCompanyMessage(dbCompany.Id, CompanyMessageType.InvoiceFooterText, msgText: item.MsgText, modifyBy: item.ModifiedBy, identifier: item.MsgIdentifier, isDefaultMsg: item.IsDefaultMsg, isActive: item.IsActive));
                }
            }

            return dbMessages;
        }

        private IList<DbModels.CompanyMessage> ProcessExistingInvoiceMessage(DbModels.Company dbCompany, DomainModel.CompanyInvoice companyInvoice,ref List<DbModels.CompanyMessage> recordToBeDelete, ref List<MessageDetail> errorMessages)
        {
            DbModels.CompanyMessage companyMessage = null;
            IList<DbModels.CompanyMessage> dbMessages = new List<DbModels.CompanyMessage>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (recordToBeDelete == null)
                recordToBeDelete = new List<DbModels.CompanyMessage>();

            if (companyInvoice != null)
            {
                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceDraftText);
                if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.InvoiceDraftText) && companyMessage.Message != companyInvoice.InvoiceDraftText) //Changes for IGO D905
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceDraftText, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InterCompanyText);
                if (companyMessage != null && companyMessage.Message != companyInvoice.InvoiceInterCompText)// && !string.IsNullOrEmpty(companyInvoice.InvoiceInterCompText))
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceInterCompText, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.FreeText);
                if (companyMessage != null && companyMessage.Message != companyInvoice.InvoiceSummarryText)// && !string.IsNullOrEmpty(companyInvoice.InvoiceSummarryText))
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceSummarryText, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InterCompanyDraftText);
                if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.InvoiceInterCompDraftText) && companyMessage.Message != companyInvoice.InvoiceInterCompDraftText)
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceInterCompDraftText, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceDescriptionText);
                if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.InvoiceDescriptionText) && companyMessage.Message != companyInvoice.InvoiceDescriptionText)
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceDescriptionText, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InterCompanyDescription);
                if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.InvoiceInterCompDescription) && companyMessage.Message != companyInvoice.InvoiceInterCompDescription)
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceInterCompDescription, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                //companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.LogoText);
                //if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.InvoiceLogoName))
                //    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceLogoName, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceHeader);
                if (companyMessage != null && companyMessage.Message != companyInvoice.InvoiceHeader)// && !string.IsNullOrEmpty(companyInvoice.InvoiceHeader))
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.InvoiceHeader, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.ReverseChargeDisclaimer);
                if (companyMessage != null && companyMessage.Message != companyInvoice.ReverseChargeDisclaimer)// && !string.IsNullOrEmpty(companyInvoice.ReverseChargeDisclaimer))
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.ReverseChargeDisclaimer, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.TechSpecialistExtranetComment);
                if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.TechSpecialistExtranetComment) && companyMessage.Message != companyInvoice.TechSpecialistExtranetComment)
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.TechSpecialistExtranetComment, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.CustomerExtranetComment);
                if (companyMessage != null && !string.IsNullOrEmpty(companyInvoice.CustomerExtranetComment) && companyMessage.Message != companyInvoice.CustomerExtranetComment)
                    dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: companyInvoice.CustomerExtranetComment, modifyBy: companyInvoice.ModifiedBy));
                //else if (companyMessage != null)
                //    recordToBeDelete.Add(companyMessage);

                if (companyInvoice?.InvoiceRemittances?.Count > 0)
                {
                    foreach (var item in companyInvoice.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusModified()))
                    {
                        companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceRemittanceText && x.Id == item.Id);
                        if (companyMessage != null && !string.IsNullOrEmpty(item.MsgText))
                            dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: item.MsgText, modifyBy: item.ModifiedBy, identifier: item.MsgIdentifier, isDefaultMsg: item.IsDefaultMsg, isActive: item.IsActive));
                        else if (companyMessage != null)
                            recordToBeDelete.Add(companyMessage);
                    }
                }

                if (companyInvoice?.InvoiceFooters?.Count > 0)
                {
                    foreach (var item in companyInvoice.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusModified()))
                    {
                        companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceFooterText && x.Id == item.Id);
                        if (companyMessage != null && !string.IsNullOrEmpty(item.MsgText))
                            dbMessages.Add(this.UpdateDbCompanyMessage(companyMessage, msgText: item.MsgText, modifyBy: item.ModifiedBy, identifier: item.MsgIdentifier, isDefaultMsg: item.IsDefaultMsg, isActive: item.IsActive));
                        else if (companyMessage != null)
                            recordToBeDelete.Add(companyMessage);
                    }
                }
            }
            return dbMessages;
        }

        private IList<DbModels.CompanyMessage> ProcessRemovedRemittanceAndFooterMessage(DbModels.Company dbCompany, DomainModel.CompanyInvoice companyInvoice, ref List<MessageDetail> errorMessages)
        {
            DbModels.CompanyMessage companyMessage = null;
            IList<DbModels.CompanyMessage> dbMessages = new List<DbModels.CompanyMessage>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (companyInvoice?.InvoiceRemittances?.Count > 0)
            {
                foreach (var item in companyInvoice.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusDeleted()))
                {
                    companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceRemittanceText && x.Id == item.Id);
                    if (companyMessage != null && this.RemittanceCanBeRemove(companyMessage, ref errorMessages))
                        dbMessages.Add(companyMessage);
                }
            }

            if (companyInvoice?.InvoiceFooters?.Count > 0 )
            {
                foreach (var item in companyInvoice.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusDeleted()))
                {
                    companyMessage = dbCompany?.CompanyMessage?.FirstOrDefault(x => x.MessageTypeId == (int)CompanyMessageType.InvoiceFooterText && x.Id == item.Id);
                    if (companyMessage != null && this.FooterCanBeRemove(companyMessage, ref errorMessages))
                        dbMessages.Add(companyMessage);
                }
            }

            return dbMessages;
        }

        private bool RemittanceCanBeRemove(DbModels.CompanyMessage companyMessage, ref List<MessageDetail> errorMessages)
        {
            if (companyMessage != null)
            {
                if (errorMessages == null)
                    errorMessages = new List<MessageDetail>();

                if (companyMessage.ProjectInvoiceRemittanceText?.Count > 0)
                {
                    var errorCode = MessageType.RemittanceCanNotBeDelete.ToId();
                    errorMessages.Add(new MessageDetail(errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), companyMessage.Identifier,"Project")));
                }
                else if (companyMessage.ContractDefaultRemittanceText?.Count > 0)
                {
                    var errorCode = MessageType.RemittanceCanNotBeDelete.ToId();
                    errorMessages.Add(new MessageDetail(errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), companyMessage.Identifier, "Contract")));
                }
            }
            return errorMessages?.Count <= 0;
        }

        private bool FooterCanBeRemove(DbModels.CompanyMessage companyMessage, ref List<MessageDetail> errorMessages)
        {
            if (companyMessage != null)
            {
                if (errorMessages == null)
                    errorMessages = new List<MessageDetail>();

                if (companyMessage.ProjectInvoiceFooterText?.Count > 0)
                {
                    var errorCode = MessageType.FooterCanNotBeDelete.ToId();
                    errorMessages.Add(new MessageDetail(errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), companyMessage.Identifier, "Project")));
                }
                else if (companyMessage.ContractDefaultFooterText?.Count > 0)
                {
                    var errorCode = MessageType.FooterCanNotBeDelete.ToId();
                    errorMessages.Add(new MessageDetail(errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), companyMessage.Identifier, "Contract")));
                }
            }

            return errorMessages?.Count <= 0;
        }

        private DbModels.CompanyMessage ConvertToDbCompanyMessage(int companyId, CompanyMessageType compnyMessageType, string msgText, string identifier = null,
                                        bool? isDefaultMsg = null, bool isActive = true, DateTime? lastModification = null, string modifyBy = null, int? updateCount = null)
        {
            var dbMessage = new DbModels.CompanyMessage()
            {
                CompanyId = companyId,
                Identifier = identifier,
                Message = msgText,
                IsDefaultMessage = isDefaultMsg,
                IsActive = isActive,
                MessageTypeId = (int)compnyMessageType,
                LastModification = lastModification,
                ModifiedBy = modifyBy,
                UpdateCount = Convert.ToByte(updateCount).CalculateUpdateCount()
        };

            return dbMessage;
        }

        private DbModels.CompanyMessage UpdateDbCompanyMessage(DbModels.CompanyMessage dbMessage, string msgText, string identifier = null,
                                        bool? isDefaultMsg = null, bool isActive = true, DateTime? lastModification = null, string modifyBy = null, int? updateCount = null)
        {
            dbMessage.Identifier = identifier;
            dbMessage.Message = msgText;
            dbMessage.IsDefaultMessage = isDefaultMsg;
            dbMessage.IsActive = isActive;
            dbMessage.LastModification = DateTime.UtcNow;
            dbMessage.ModifiedBy = modifyBy;
            dbMessage.UpdateCount = Convert.ToByte(updateCount).CalculateUpdateCount();
            return dbMessage;
        }

        #endregion
    }
}