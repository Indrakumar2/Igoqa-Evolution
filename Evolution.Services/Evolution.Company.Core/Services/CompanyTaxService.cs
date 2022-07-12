using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Core.Services
{
    public class CompanyTaxService : ICompanyTaxService
    {
        private readonly ICompanyTaxRepository _repository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IAppLogger<CompanyTaxService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ICompanyTaxValidationService _validationService = null;
        private readonly IMapper _mapper = null;

        public CompanyTaxService(
                                    ICompanyTaxRepository repository,
                                    IAppLogger<CompanyTaxService> logger, 
                                    ICompanyRepository companyRepository,
                                    ICompanyTaxValidationService validationService,
                                    JObject messages,
                                    IMapper mapper)
        {
            this._repository = repository;
            this._logger = logger;
            this._companyRepository = companyRepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            this._mapper = mapper;
            
        }

        #region Public Exposed Method

        public Response DeleteCompanyTax(string companyCode, IList<CompanyTax> companyTax, bool commitChange = true)
        {
            return RemoveCompanyTaxes(companyCode, companyTax, commitChange);
        }

        public Response GetCompanyTax(CompanyTax searchModel)
        {
            IList<DomainModel.CompanyTax> result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = this._repository.Search(searchModel);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response ModifyCompanyTax(string companyCode, IList<CompanyTax> companyTax, bool commitChange = true)
        {
            var result = this.UpdateCompanyTaxes(companyCode, companyTax, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyTax(new CompanyTax() { CompanyCode = companyCode });
            else
                return result;
        }

        public Response SaveCompanyTax(string companyCode, IList<CompanyTax> companyTax, bool commitChange = true)
        {
            var result = this.AddCompanyTaxes(companyCode, companyTax, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyTax(new CompanyTax() { CompanyCode = companyCode });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddCompanyTaxes(string companyCode, IList<CompanyTax> companyTaxes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyTax> recordToBeInserted = null;
                    if (this.IsRecordValidForProcess(companyTaxes, ValidationType.Add, ref recordToBeInserted, ref errorMessages,ref validationMessages))
                    {
                        if (!this.IsTaxAlreadyAssociatedToCompany(companyCode, recordToBeInserted, ValidationType.Add, ref errorMessages))
                        {
                          
                            var dbCompanyTaxToBeInserted = _mapper.Map<IList<DbModel.CompanyTax>>(recordToBeInserted);

                            dbCompanyTaxToBeInserted.ToList().ForEach(x =>
                            {
                                x.Id = 0;
                                x.CompanyId = dbCompany.Id;
                            });
                            _repository.Add(dbCompanyTaxToBeInserted);

                            if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0)
                                _repository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyTaxes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateCompanyTaxes(string companyCode, IList<CompanyTax> companyTaxes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyTax> recordToBeModify = null;
                    if (this.IsRecordValidForProcess(companyTaxes, ValidationType.Update, ref recordToBeModify, ref errorMessages,ref validationMessages))
                    {
                        IList<DbModel.CompanyTax> dbCompanyTaxes = null;
                       
                        if (IsValidCompanyTax(companyCode, recordToBeModify, ref dbCompanyTaxes, ref errorMessages))
                        {
                            if (!this.IsTaxAlreadyAssociatedToCompany(companyCode, recordToBeModify, ValidationType.Update, ref errorMessages))
                            {
                                if (this.IsRecordUpdateCountMatching(recordToBeModify, dbCompanyTaxes.ToList(), ref errorMessages))
                                {
                                    foreach (var companyTax in dbCompanyTaxes)
                                    {
                                        var tax = recordToBeModify.FirstOrDefault(x => x.CompanyTaxId == companyTax.Id);
                                        companyTax.CompanyId = dbCompany.Id;
                                            
                                        companyTax.LastModification = DateTime.UtcNow;
                                        companyTax.UpdateCount = tax.UpdateCount.CalculateUpdateCount();
                                        companyTax.ModifiedBy = tax.ModifiedBy;
                                        _repository.Update(companyTax);
                                    }

                                    if (commitChange && recordToBeModify?.Count > 0 && errorMessages.Count <= 0)
                                        _repository.ForceSave();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyTaxes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveCompanyTaxes(string companyCode, IList<CompanyTax> companyTaxes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyTax> recordToBeDelete = null;
                    if (this.IsRecordValidForProcess(companyTaxes, ValidationType.Delete, ref recordToBeDelete, ref errorMessages,ref validationMessages))
                    {
                        IList<DbModel.CompanyTax> dbCompanyTaxes = null;
                        if (IsValidCompanyTax(companyCode,recordToBeDelete, ref dbCompanyTaxes, ref errorMessages))
                        { 
                            if (this.IsRecordCanBeDelete(companyCode, companyTaxes, dbCompanyTaxes, ref errorMessages))
                            {
                                _repository.Delete(dbCompanyTaxes);

                                if (commitChange && !_repository.AutoSave && dbCompanyTaxes?.Count > 0 && errorMessages.Count <= 0)
                                    _repository.ForceSave();
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyTaxes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidCompany(string companyCode, ref DbModel.Company company, ref List<MessageDetail> errorMessages)
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
                errorMessages.Add(new MessageDetail(ModuleType.Company, MessageType.InvalidCompanyCode.ToId(), _messageDescriptions[MessageType.InvalidCompanyCode.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private bool IsRecordValidForProcess(IList<CompanyTax> companyTaxes, ValidationType validationType, ref IList<CompanyTax> filteredCompanyTaxes, ref List<MessageDetail> errorMessages,ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredCompanyTaxes = companyTaxes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredCompanyTaxes = companyTaxes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredCompanyTaxes = companyTaxes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredCompanyTaxes?.Count > 0?IsCompanyTaxHasValidSchema(filteredCompanyTaxes,validationType,ref validationMessages):false;
        }

        private bool IsRecordCanBeDelete(string companyCode, IList<CompanyTax> companyTaxes, IList<DbModel.CompanyTax> dbCompanyTaxes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbCompanyTaxes.Where(x => x.InvoiceWithholdingTaxNavigation?.Count > 0 
                                        || x.ContractDefaultSalesTax?.Count >0 
                                        || x.ProjectInvoiceSalesTax?.Count > 0
                                        || x.ProjectInvoiceWithholdingTax?.Count >0
                                        || x.InvoiceSalesTax?.Count > 0 
                                        || x.InvoiceItemBackup?.Count > 0 
                                        || x.InvoiceItem?.Count > 0 
                                        || x.InterCompanyInvoiceItemBackup?.Count > 0 
                                        || x.InterCompanyInvoiceItem?.Count > 0 
                                        || x.InterCompanyInvoice?.Count > 0).ToList()
                                .ForEach(x =>
                                {
                                    string errorCode = MessageType.TaxCannotBeDeleted.ToId();
                                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.Name)));
                                });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsTaxAlreadyAssociatedToCompany(string companyCode, IList<CompanyTax> companyTaxes, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var messages = errorMessages;
            IList<DbModel.CompanyTax> dbNotExistingTax = null;
            var filterExpressions = new List<Expression<Func<DbModel.CompanyTax, bool>>>();
            Expression<Func<DbModel.CompanyTax, bool>> predicate = null;
            Expression<Func<DbModel.CompanyTax, bool>> containsExpression = null;
            if(companyTaxes?.Count > 0){
            if(validationType == ValidationType.Update){
                // dbNotExistingTax = _repository?.FindBy(x => companyTaxes.Any(x1 => x1.TaxName == x.Name
                //                                                             && x.Company.Code == x1.CompanyCode
                //                                                             && x.Code == x1.TaxCode
                //                                                             && x1.TaxType == x.TaxType
                //                                                             && x1.TaxRate == x.Rate
                //                                                             && x.Id != x1.CompanyTaxId
                //                                                             )).ToList();
                foreach (var compTax in companyTaxes)
                    {
                        containsExpression = a => a.Name == compTax.TaxName  && a.Company.Code == compTax.CompanyCode
                                                    && a.Code == compTax.TaxCode && a.TaxType == compTax.TaxType
                                                    && a.Rate ==  compTax.TaxRate && a.Id != compTax.CompanyTaxId;

                        filterExpressions.Add(containsExpression);
                    }
            }
               
            if(validationType == ValidationType.Add){
                // dbNotExistingTax = _repository?.FindBy(x => companyTaxes.Any(x1 => x1.TaxName == x.Name
                //                                                            && x.Company.Code == x1.CompanyCode
                //                                                            && x.Code == x1.TaxCode
                //                                                            && x1.TaxType == x.TaxType
                //                                                            && x1.TaxRate == x.Rate
                //                                                            && x.Id != x1.CompanyTaxId
                //                                                            )).ToList();
                    foreach (var compTax in companyTaxes)
                    {
                        containsExpression = a => a.Name == compTax.TaxName  && a.Company.Code == compTax.CompanyCode
                                                    && a.Code == compTax.TaxCode && a.TaxType == compTax.TaxType
                                                    && a.Rate ==  compTax.TaxRate;

                        filterExpressions.Add(containsExpression);
                    }
            }
             predicate = filterExpressions.CombinePredicates<DbModel.CompanyTax>(Expression.OrElse);
             dbNotExistingTax = _repository?.FindBy(predicate).ToList();
         
            dbNotExistingTax?.ToList().ForEach(x =>
            {
                string errorCode = MessageType.TaxAlreadyExist.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.Name)));
            });
            
            errorMessages = messages;
            }
            return messages?.Count > 0;
        }

        private bool IsRecordUpdateCountMatching(IList<CompanyTax> companyTaxes, IList<DbModel.CompanyTax> dbCompanyTaxes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyTaxes.Where(x => !dbCompanyTaxes.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyTaxId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.TaxUpdateCountMismatch.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.TaxName)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }
         
        private bool IsValidCompanyTax(string companyCode, IList<CompanyTax> companyTaxes,ref IList<DbModel.CompanyTax> dbCompanyTaxes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var dbCompTaxes = _repository?.FindBy(x => companyTaxes.Select(x1 => x1.CompanyTaxId).Contains(x.Id) && x.Company.Code == companyCode).ToList();

            var notMatchedRecords = companyTaxes.Where(x => !dbCompTaxes.ToList().Any(x1 => x1.Id == x.CompanyTaxId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.TaxIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyTaxId)));
            });

            dbCompanyTaxes = dbCompTaxes;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsCompanyTaxHasValidSchema(IList<CompanyTax> companyTaxes, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companyTaxes), validationType);

            validationResults?.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Company, x.Code, x.Message) }));
            });
            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;
        }

        #endregion
    }
}
