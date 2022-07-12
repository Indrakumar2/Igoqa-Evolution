using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.Company.Domain.Models.Companies;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Linq.Expressions;
using System.Transactions;

namespace Evolution.Company.Core.Services
{
    public class CompanyExpectedMarginService : ICompanyExpectedMarginService
    {
        private readonly ICompanyExpectedMarginRepository _repository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly IAppLogger<CompanyExpectedMarginService> _logger = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly ICompanyExpectedMarginValidationService _validationService = null;

        public CompanyExpectedMarginService(IDataRepository dataRepository, ICompanyRepository companyRepository,
                                            ICompanyExpectedMarginRepository repository, IAppLogger<CompanyExpectedMarginService> logger, ICompanyExpectedMarginValidationService validationService, JObject messages)
        {
            this._dataRepository = dataRepository;
            this._repository = repository;
            this._companyRepository = companyRepository;
            this._logger = logger;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        #region Public Exposed Method

        public Response DeleteCompanyExpectedMargin(string companyCode, IList<CompanyExpectedMargin> companyExpectedMargins, bool commitChange = true)
        {
            return RemoveCompanyExpectedMargin(companyCode, companyExpectedMargins, commitChange);
        }

        public Response GetCompanyExpectedMargin(CompanyExpectedMargin searchModel)
        {
            IList<DomainModel.CompanyExpectedMargin> result = null;
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

        public Response ModifyCompanyExpectedMargin(string companyCode, IList<CompanyExpectedMargin> companyExpectedMargins, bool commitChange = true)
        {
            var result = this.UpdateCompanyExpectedMargin(companyCode, companyExpectedMargins, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyExpectedMargin(new CompanyExpectedMargin() { CompanyCode = companyCode });
            else
                return result;
        }

        public Response SaveCompanyExpectedMargin(string companyCode, IList<CompanyExpectedMargin> CompanyExpectedMargins, bool commitChange = true)
        {
            var result = this.AddCompanyExpectedMargin(companyCode, CompanyExpectedMargins, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyExpectedMargin(new CompanyExpectedMargin() { CompanyCode = companyCode });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response RemoveCompanyExpectedMargin(string companyCode, IList<CompanyExpectedMargin> companyExpectedMargins, bool commitChange)
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
                    IList<CompanyExpectedMargin> recordToBeDelete = null;
                    if (this.IsRecordValidForProcess(companyExpectedMargins, ValidationType.Delete, ref recordToBeDelete, ref errorMessages, ref validationMessages))
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.Data> dbfilteredExpectedMargins = null;
                        if (this.IsValidExpetectMargin(recordToBeDelete, ref dbfilteredExpectedMargins, ref errorMessages))
                        {
                            var dbCompanyExpectedMargins = _repository.FindBy(x => recordToBeDelete.Select(x1 => x1.CompanyExpectedMarginId).Contains(x.Id)).ToList();
                            if (IsValidExpectedMargin(recordToBeDelete, dbCompanyExpectedMargins, ref errorMessages))
                            {
                                foreach (var expectedMargin in dbCompanyExpectedMargins)
                                {
                                    _repository.Delete(expectedMargin);
                                }

                                if (commitChange && !_repository.AutoSave && recordToBeDelete?.Count > 0 && errorMessages?.Count <= 0)
                                    _repository.ForceSave();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyExpectedMargins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response AddCompanyExpectedMargin(string companyCode, IList<CompanyExpectedMargin> companyExpectedMargins, bool commitChange)
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
                    IList<CompanyExpectedMargin> recordToBeInserted = null;
                    if (this.IsRecordValidForProcess(companyExpectedMargins, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                    {
                        if (!this.IsExpectedMarginAlreadyAssociatedToCompany(companyCode, recordToBeInserted, ValidationType.Add, ref errorMessages))
                        {
                            IList<DbRepository.Models.SqlDatabaseContext.Data> dbExpectedMargins = null;
                            if (this.IsValidExpetectMargin(recordToBeInserted, ref dbExpectedMargins, ref errorMessages))
                            {
                                var dbExpectedMarginToBeInserted = recordToBeInserted.Select(x => new DbModel.CompanyExpectedMargin()
                                {
                                    CompanyId = dbCompany.Id,
                                    MarginTypeId = dbExpectedMargins.FirstOrDefault(x1 => x1.Name == x.MarginType).Id,
                                    MinimumMargin = x.MinimumMargin
                                }).ToList();

                                _repository.Add(dbExpectedMarginToBeInserted);

                                if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                    _repository.ForceSave();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyExpectedMargins);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateCompanyExpectedMargin(string companyCode, IList<CompanyExpectedMargin> companyExpectedmargins, bool commitChange)
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
                    IList<CompanyExpectedMargin> recordToBeModify = null;
                    if (this.IsRecordValidForProcess(companyExpectedmargins, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                    {
                        if (!this.IsExpectedMarginAlreadyAssociatedToCompany(companyCode, recordToBeModify, ValidationType.Update, ref errorMessages))
                        {
                            IList<DbRepository.Models.SqlDatabaseContext.Data> dbExpectedMargins = null;
                            if (this.IsValidExpetectMargin(recordToBeModify, ref dbExpectedMargins, ref errorMessages))
                            {
                                //db record
                                var compExpectedMargins = _repository.FindBy(x => x.Company.Code == companyCode && recordToBeModify.Select(x1 => x1.CompanyExpectedMarginId).Contains(x.Id));
                                if (IsValidExpectedMargin(recordToBeModify, compExpectedMargins.ToList(), ref errorMessages))
                                {
                                    if (this.IsRecordUpdateCountMatching(recordToBeModify, compExpectedMargins.ToList(), ref errorMessages))
                                    {
                                        //looping db record
                                        foreach (var companyExpectedMargin in compExpectedMargins)
                                        {
                                            var expectedMargin = recordToBeModify.FirstOrDefault(x => x.CompanyExpectedMarginId == companyExpectedMargin.Id);
                                            companyExpectedMargin.CompanyId = dbCompany.Id;
                                            companyExpectedMargin.MarginTypeId = dbExpectedMargins.FirstOrDefault(x => x.Name == expectedMargin.MarginType).Id;
                                            companyExpectedMargin.MinimumMargin = expectedMargin.MinimumMargin;
                                            companyExpectedMargin.LastModification = DateTime.UtcNow;
                                            companyExpectedMargin.UpdateCount = expectedMargin.UpdateCount.CalculateUpdateCount();
                                            companyExpectedMargin.ModifiedBy = expectedMargin.ModifiedBy;
                                            _repository.Update(companyExpectedMargin);
                                        }

                                        if (commitChange && recordToBeModify?.Count > 0)
                                            _repository.ForceSave();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyExpectedmargins);
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
                errorMessages.Add(new MessageDetail(ModuleType.Company, MessageType.InvalidCompanyCode.ToId(), _MessageDescriptions[MessageType.InvalidCompanyCode.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private bool IsRecordValidForProcess(IList<CompanyExpectedMargin> companyExpectedMargins, ValidationType validationType, ref IList<CompanyExpectedMargin> filteredExpectedMargin, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredExpectedMargin = companyExpectedMargins?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredExpectedMargin = companyExpectedMargins?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredExpectedMargin = companyExpectedMargins?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredExpectedMargin?.Count > 0 ? IsExpectedMarginHasValidSchema(filteredExpectedMargin, validationType, ref validationMessages) : false;
        }

        private bool IsExpectedMarginAlreadyAssociatedToCompany(string companyCode, IList<CompanyExpectedMargin> companyExpectedMargins, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.CompanyExpectedMargin> expectedMarginExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            var filterExpressions = new List<Expression<Func<DbModel.CompanyExpectedMargin, bool>>>();
            Expression<Func<DbModel.CompanyExpectedMargin, bool>> predicate = null;
            Expression<Func<DbModel.CompanyExpectedMargin, bool>> containsExpression = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var expetectMargin = companyExpectedMargins.Select(x => new { x.MarginType, x.MinimumMargin, x.CompanyExpectedMarginId }).ToList();
            if (expetectMargin?.Count > 0)
            {
                if (validationType == ValidationType.Add)
                {
                    //expectedMarginExists = _repository?.FindBy(x => expetectMargin.Any(x1 => x.MarginType.Name == x1.MarginType && x.MinimumMargin == x1.MinimumMargin &&
                    //                                          x.Company.Code == companyCode)).ToList();
                    foreach(var expectMar in expetectMargin)
                    {
                        containsExpression = a => a.MarginType.Name == expectMar.MarginType && a.MinimumMargin == expectMar.MinimumMargin;
                        filterExpressions.Add(containsExpression);
                    }
                }

                else if (validationType == ValidationType.Update)
                {
                    //expectedMarginExists = _repository?.FindBy(x => expetectMargin.Any(x1 => x.MarginType.Name == x1.MarginType && x.MinimumMargin == x1.MinimumMargin &&
                    //                                         x.Company.Code == companyCode && x.Id != x1.CompanyExpectedMarginId)).ToList();
                    foreach (var expectMar in expetectMargin)
                    {
                        containsExpression = a => a.MarginType.Name == expectMar.MarginType && a.MinimumMargin == expectMar.MinimumMargin && a.Id != expectMar.CompanyExpectedMarginId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.CompanyExpectedMargin>(Expression.OrElse);
                containsExpression = a => a.Company.Code == companyCode;
                predicate = containsExpression.CombineWithAndAlso(predicate);
                
                expectedMarginExists = _repository?.FindBy(predicate).ToList();
                expectedMarginExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.ExpectedMarginDataAlreadyExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.MarginType.Name)));
                });
            }

            //if (validationType == ValidationType.Add)
            //    expectedMarginExists = _repository?.FindBy(x => expetectMargin.Any(x1 => x.MarginType.Name == x1.MarginType && x.MinimumMargin == x1.MinimumMargin &&
            //                                              x.Company.Code == companyCode)).ToList();


            //else if (validationType == ValidationType.Update)
            //    expectedMarginExists = _repository?.FindBy(x => expetectMargin.Any(x1 => x.MarginType.Name == x1.MarginType && x.MinimumMargin == x1.MinimumMargin &&
            //                                              x.Company.Code == companyCode && x.Id != x1.CompanyExpectedMarginId)).ToList();

            //expectedMarginExists?.ToList().ForEach(x =>
            //{
            //    string errorCode = MessageType.ExpectedMarginDataAlreadyExists.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.MarginType.Name)));
            //});

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsValidExpetectMargin(IList<CompanyExpectedMargin> companyExpectedMargins, ref IList<DbRepository.Models.SqlDatabaseContext.Data> margins, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var expetectMargin = companyExpectedMargins.Select(x => x.MarginType).ToList();

            var dbExpectedMargins = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.CompanyMarginType) &&
                                                                 expetectMargin.Contains(x.Name)).ToList();

            var expetectMarginNotExists = companyExpectedMargins.Where(x => !dbExpectedMargins.Any(x1 => x1.Name == x.MarginType)).ToList();
            expetectMarginNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ExpectedMarginNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.MarginType)));
            });

            margins = dbExpectedMargins;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<CompanyExpectedMargin> companyExpectedMargins, IList<DbRepository.Models.SqlDatabaseContext.CompanyExpectedMargin> dbCompanyExpectedMargins, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyExpectedMargins.Where(x => !dbCompanyExpectedMargins.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyExpectedMarginId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ExpectedMarginHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.MarginType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidExpectedMargin(IList<CompanyExpectedMargin> companyExpectedMargins, IList<DbRepository.Models.SqlDatabaseContext.CompanyExpectedMargin> dbCompanyExpectedMargins, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyExpectedMargins.Where(x => !dbCompanyExpectedMargins.ToList().Any(x1 => x1.Id == x.CompanyExpectedMarginId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ExpectedIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.CompanyExpectedMarginId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsExpectedMarginHasValidSchema(IList<DomainModel.CompanyExpectedMargin> CompanyExpectedMargin, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(CompanyExpectedMargin), validationType);

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
