using AutoMapper;
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
using Evolution.Master.Core;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Evolution.Company.Core.Services
{
    public class CompanyDivisionService : ICompanyDivisionService
    {
        private readonly ICompanyDivisionRepository _repository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IDivisionService _divisionService = null;
        private readonly IAppLogger<CompanyDivisionService> _logger = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly ICompanyDivisionValidationService _validationService = null;

        public CompanyDivisionService(IDivisionService divisionService,
            ICompanyRepository companyRepository, 
            ICompanyDivisionRepository repository, 
            IAppLogger<CompanyDivisionService> logger,
            ICompanyDivisionValidationService validationService,JObject messages)
        {
            _divisionService = divisionService;
            this._repository = repository;
            this._companyRepository = companyRepository;
            this._logger = logger;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        #region Public Exposed Method

        public Response DeleteCompanyDivision(string companyCode, IList<CompanyDivision> companyDivisions, bool commitChange = true)
        {
            return RemoveCompanyDivision(companyCode, companyDivisions, commitChange);
        }

        public Response GetCompanyDivision(CompanyDivision searchModel)
        {
            IList<CompanyDivision> result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
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

        public Response ModifyCompanyDivision(string companyCode, IList<CompanyDivision> companyDivisions, bool commitChange = true)
        {
            var result = this.UpdateCompanyDivision(companyCode, companyDivisions, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyDivision(new CompanyDivision() { CompanyCode = companyCode });
            else
                return result;
        }

        public Response SaveCompanyDivision(string companyCode, IList<CompanyDivision> companyDivisions, bool commitChange = true)
        {
            var result = this.AddCompanyDivision(companyCode, companyDivisions, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyDivision(new CompanyDivision() { CompanyCode = companyCode });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods
        private Response RemoveCompanyDivision(string companyCode, IList<CompanyDivision> companyDivisions, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbRepository.Models.SqlDatabaseContext.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<CompanyDivision> recordToBeDelete = null;
                if (this.IsRecordValidForProcess(companyDivisions, ValidationType.Delete, ref recordToBeDelete, ref errorMessages,ref validationMessages))
                {
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.CompanyDivision> dbCompanyDivisions = null;
                        if (IsValidCompanyDivision(dbCompany, recordToBeDelete, ref dbCompanyDivisions, ref errorMessages))
                        {
                            if (this.IsRecordCanBeDelete(companyDivisions, dbCompanyDivisions, ref errorMessages))
                            {
                                var divIds = dbCompanyDivisions.Select(x => x.DivisionId).ToList();
                                 _repository.Delete(dbCompanyDivisions); 

                                if (commitChange && !_repository.AutoSave && recordToBeDelete?.Count > 0 && errorMessages?.Count <= 0)
                                {
                                    var saveCnt=_repository.ForceSave();
                                    //if (saveCnt >= 0 && divIds?.Count >0)
                                    //{
                                    //    var resp=  _divisionService.Delete(divIds);
                                    //    if(resp.Code!= ResponseType.Success.ToId()) {
                                    //        return resp;
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyDivisions);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response AddCompanyDivision(string companyCode, IList<CompanyDivision> companyDivisions, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbRepository.Models.SqlDatabaseContext.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<CompanyDivision> recordToBeInserted = null;
                if (this.IsRecordValidForProcess(companyDivisions, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        IList<Division> dbDivisions = null;
                        IList<string> divisionNotExists = null;
                        if (this._divisionService.IsValidDivision(recordToBeInserted?.Select(x=>x.DivisionName).ToList(),ref divisionNotExists, ref dbDivisions, ref errorMessages))
                        {
                            if (!this.IsDivisionAlreadyAssociatedToCompany(dbCompany, recordToBeInserted, ValidationType.Add, ref errorMessages))
                            {
                                var dbDivisionToBeInserted = recordToBeInserted.Select(x => new DbRepository.Models.SqlDatabaseContext.CompanyDivision()
                                {
                                    CompanyId = dbCompany.Id,
                                    DivisionId = (int)dbDivisions.FirstOrDefault(x1 => x1.Name.ToLower() == x.DivisionName.ToLower()).Id,
                                    AccountReference = x.DivisionAcReference
                                }).ToList();

                                this._repository.AutoSave = false;
                                _repository.Add(dbDivisionToBeInserted);

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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyDivisions);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateCompanyDivision(string companyCode, IList<CompanyDivision> companyDivisions, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbRepository.Models.SqlDatabaseContext.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<CompanyDivision> recordToBeModify = null;
                if (this.IsRecordValidForProcess(companyDivisions, ValidationType.Update, ref recordToBeModify, ref errorMessages,ref validationMessages))
                {
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        IList<Division> dbDivisions = null;
                        IList<string> divisionNotExists = null;
                        if (this._divisionService.IsValidDivision(recordToBeModify?.Select(x => x.DivisionName).ToList(), ref divisionNotExists, ref dbDivisions, ref errorMessages))
                        {
                            if (!this.IsDivisionAlreadyAssociatedToCompany(dbCompany, recordToBeModify, ValidationType.Update, ref errorMessages))
                            {
                                IList<DbRepository.Models.SqlDatabaseContext.CompanyDivision> dbCompanyDivisions = null;
                                if (IsValidCompanyDivision(dbCompany, recordToBeModify, ref dbCompanyDivisions, ref errorMessages))
                                {
                                    if (this.IsRecordUpdateCountMatching(recordToBeModify, dbCompanyDivisions, ref errorMessages))
                                    {
                                        foreach (var companyDivision in dbCompanyDivisions)
                                        {
                                            var division = recordToBeModify.FirstOrDefault(x => x.CompanyDivisionId == companyDivision.Id);
                                            companyDivision.CompanyId = dbCompany.Id;
                                            companyDivision.DivisionId = (int)dbDivisions.FirstOrDefault(x => x.Name == division.DivisionName).Id;
                                            companyDivision.AccountReference = division.DivisionAcReference;
                                            companyDivision.LastModification = DateTime.UtcNow;
                                            companyDivision.UpdateCount = division.UpdateCount.CalculateUpdateCount();
                                            companyDivision.ModifiedBy = division.ModifiedBy;
                                            _repository.Update(companyDivision);
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyDivisions);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidCompany(string companyCode, ref DbRepository.Models.SqlDatabaseContext.Company company, ref List<MessageDetail> errorMessages)
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

        private bool IsRecordValidForProcess(IList<CompanyDivision> companyDivisions, ValidationType validationType, ref IList<CompanyDivision> filteredDivision, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredDivision = companyDivisions?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredDivision = companyDivisions?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredDivision = companyDivisions?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredDivision?.Count > 0?IsCompanyDivisionHasValidSchema(filteredDivision,validationType,ref validationMessages ):false;
        }

        private bool IsDivisionAlreadyAssociatedToCompany(DbRepository.Models.SqlDatabaseContext.Company company, IList<CompanyDivision> companyDivisions, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbRepository.Models.SqlDatabaseContext.CompanyDivision> divisionExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var divisionNames = companyDivisions.Select(x => x.DivisionName).ToList();
            if (divisionNames?.Count > 0)
            {
                if (validationType == ValidationType.Add)
                    divisionExists = company?.CompanyDivision?.Where(x => divisionNames.Contains(x.Division.Name)).ToList();
                else if (validationType == ValidationType.Update)
                    divisionExists = company?.CompanyDivision?.Where(x => divisionNames.Contains(x.Division.Name))
                                                              .ToList()
                                                              .Where(x1 => companyDivisions.Any(x2 => x2.CompanyDivisionId != x1.Id)).ToList();
                divisionExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.DivisionDataAlreadyExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Division.Name)));
                });
            }
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }
        
        private bool IsRecordUpdateCountMatching(IList<CompanyDivision> companyDivisions, IList<DbRepository.Models.SqlDatabaseContext.CompanyDivision> dbCompanyDivisions, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyDivisions.Where(x => !dbCompanyDivisions.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyDivisionId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.DivisionHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.DivisionName)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordCanBeDelete(IList<CompanyDivision> companyDivisions, IList<DbRepository.Models.SqlDatabaseContext.CompanyDivision> dbCompanyDivisions, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbCompanyDivisions.Where(x => x.CompanyDivisionCostCenter?.Count > 0 || x.Project?.Count > 0).ToList()
                                .ForEach(x =>
                                {
                                    string errorCode = MessageType.DivisionCannotBeDeleted.ToId();
                                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Division.Name)));
                                });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }
        
        private bool IsValidCompanyDivision(DbRepository.Models.SqlDatabaseContext.Company company, IList<CompanyDivision> companyDivisions, ref IList<DbRepository.Models.SqlDatabaseContext.CompanyDivision> dbCompanyDivisions, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var divisionIds = companyDivisions.Select(x1 => x1.CompanyDivisionId).ToList();
            var dbCompDivisions = company?.CompanyDivision?.Where(x => divisionIds.Contains(x.Id) && x.Company.Code == company.Code).ToList();
            var notMatchedRecords = companyDivisions.Where(x => !dbCompDivisions.ToList().Any(x1 => x1.Id == x.CompanyDivisionId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.DivisonIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.CompanyDivisionId)));
            });

            dbCompanyDivisions = dbCompDivisions;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsCompanyDivisionHasValidSchema(IList<CompanyDivision> companyDivisions, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companyDivisions), validationType);

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