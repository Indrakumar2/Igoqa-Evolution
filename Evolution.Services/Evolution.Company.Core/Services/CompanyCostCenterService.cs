using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using System;
using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Linq;
using Evolution.Common.Models.Messages;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using AutoMapper;
using Evolution.Company.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Transactions;

namespace Evolution.Company.Core.Services
{
    public class CompanyCostCenterService : ICompanyDivisionCostCenterService
    {
        private readonly ICompanyCostCenterRepository _repository = null;
        private readonly ICompanyDivisionRepository _divisionRepository = null;
        private readonly IAppLogger<CompanyCostCenterService> _logger = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly ICompanyCostCenterValidationService _validationService = null;

        public CompanyCostCenterService(ICompanyCostCenterRepository repository,
                                        ICompanyDivisionRepository divisionRepository,
                                        IAppLogger<CompanyCostCenterService> logger,
                                        ICompanyCostCenterValidationService validationService,JObject messages)
        {
            this._repository = repository;
            this._divisionRepository = divisionRepository;
            this._logger = logger;
            this._MessageDescriptions = messages;
            _validationService = validationService;
        }

        public Response DeleteCompanyCostCenter(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, bool commitChange = true)
        {
            return RemoveCompanyCostCenter(companyCode, divisionName, companyCostCenters, commitChange);
        }

        public Response GetCompanyCostCenter(CompanyDivisionCostCenter searchModel)
        {
            IList<DomainModel.CompanyDivisionCostCenter> result = null;
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

        public Response ModifyCompanyCostCenter(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, bool commitChange = true)
        {
            var result = this.UpdateCompanyCostCenter(companyCode, divisionName, companyCostCenters, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyCostCenter(new CompanyDivisionCostCenter() { CompanyCode = companyCode, Division = divisionName });
            else
                return result;
        }

        public Response SaveCompanyCostCenter(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, bool commitChange = true)
        {
            var result = this.AddCompanyCostCenter(companyCode, divisionName, companyCostCenters, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyCostCenter(new CompanyDivisionCostCenter() { CompanyCode = companyCode, Division = divisionName });
            else
                return result;
        }
        
        #region Private Exposed Methods

        private Response RemoveCompanyCostCenter(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbRepository.Models.SqlDatabaseContext.CompanyDivision dbDivision = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<CompanyDivisionCostCenter> recordToBeDelete = null;
                if (this.IsRecordValidForProcess(companyCostCenters, ValidationType.Delete, ref recordToBeDelete, ref errorMessages,ref validationMessages)) 
                {
                    if (this.IsValidDivision(companyCode, divisionName, ref dbDivision, ref errorMessages))
                    {
                        var dbCompanyCostCenter = _repository.FindBy(x => recordToBeDelete.Select(x1 => x1.CompanyDivisionCostCenterId).Contains(x.Id)).ToList();
                        if (IsValidCompanyCostCenter(recordToBeDelete, dbCompanyCostCenter.ToList(), ref errorMessages))
                        {
                            if (IsRecordCanBeDelete(recordToBeDelete, dbCompanyCostCenter.ToList(), ref errorMessages))
                            {
                                foreach (var costCenter in dbCompanyCostCenter)
                                {
                                    _repository.Delete(costCenter);
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCostCenters);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response AddCompanyCostCenter(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbRepository.Models.SqlDatabaseContext.CompanyDivision dbDivision = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<CompanyDivisionCostCenter> recordToBeInserted = null;
                if (this.IsRecordValidForProcess(companyCostCenters, ValidationType.Add, ref recordToBeInserted, ref errorMessages,ref validationMessages))
                {
                    if (this.IsValidDivision(companyCode, divisionName, ref dbDivision, ref errorMessages))
                    {
                        if (!this.IsCostCenterAlreadyAssociatedToCompany(companyCode, divisionName, recordToBeInserted, ValidationType.Add, ref errorMessages))
                        {
                            var dbCostCenterToBeInserted = recordToBeInserted?.Select(x => new DbRepository.Models.SqlDatabaseContext.CompanyDivisionCostCenter()
                            {
                                CompanyDivisionId = dbDivision.Id,
                                Code = x.CostCenterCode,
                                Name = x.CostCenterName
                            }).ToList();

                            if (dbCostCenterToBeInserted != null)
                                _repository.Add(dbCostCenterToBeInserted);

                            if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                _repository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCostCenters);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateCompanyCostCenter(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbRepository.Models.SqlDatabaseContext.CompanyDivision dbDivision = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<CompanyDivisionCostCenter> recordToBeModify = null;
                if (this.IsRecordValidForProcess(companyCostCenters, ValidationType.Update, ref recordToBeModify, ref errorMessages,ref validationMessages)) 
                {
                    if (this.IsValidDivision(companyCode, divisionName, ref dbDivision, ref errorMessages))
                    {
                        if (!this.IsCostCenterAlreadyAssociatedToCompany(companyCode, divisionName, recordToBeModify, ValidationType.Update, ref errorMessages))
                        {
                            var compCostCenter = _repository.FindBy(x => x.CompanyDivision.Division.Name == divisionName && recordToBeModify.Select(x1 => x1.CompanyDivisionCostCenterId).Contains(x.Id));
                            if (IsValidCompanyCostCenter(recordToBeModify, compCostCenter.ToList(), ref errorMessages))
                            {
                                if (this.IsRecordUpdateCountMatching(recordToBeModify, compCostCenter.ToList(), ref errorMessages))
                                {
                                    foreach (var companyCostCenter in compCostCenter)
                                    {
                                        var costCenter = recordToBeModify.FirstOrDefault(x => x.CompanyDivisionCostCenterId == companyCostCenter.Id);
                                        companyCostCenter.CompanyDivisionId = dbDivision.Id;
                                        companyCostCenter.Code = costCenter.CostCenterCode;
                                        companyCostCenter.Name = costCenter.CostCenterName;
                                        companyCostCenter.LastModification = DateTime.UtcNow;
                                        companyCostCenter.UpdateCount = costCenter.UpdateCount.CalculateUpdateCount();
                                        companyCostCenter.ModifiedBy = costCenter.ModifiedBy;
                                        _repository.Update(companyCostCenter);
                                    }

                                    if (commitChange && recordToBeModify?.Count > 0)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCostCenters);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidDivision(string companyCode, string divisionName, ref DbRepository.Models.SqlDatabaseContext.CompanyDivision division, ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;

            if (string.IsNullOrEmpty(companyCode) && string.IsNullOrEmpty(divisionName))
                messageType = MessageType.CostCenterDivisionInvalid;
            else
            {
                division = _divisionRepository.FindBy(x => x.Company.Code == companyCode && x.Division.Name == divisionName).FirstOrDefault();
                if (division == null)
                    messageType = MessageType.CostCenterDivisionInvalid;
            }

            if (messageType != MessageType.Success)
                errorMessages.Add(new MessageDetail(ModuleType.Company, MessageType.CostCenterDivisionInvalid.ToId(), _MessageDescriptions[MessageType.CostCenterDivisionInvalid.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private bool IsRecordValidForProcess(IList<CompanyDivisionCostCenter> companyCostCenters, ValidationType validationType, ref IList<CompanyDivisionCostCenter> filteredCostCenter, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredCostCenter = companyCostCenters?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredCostCenter = companyCostCenters?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredCostCenter = companyCostCenters?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredCostCenter == null)
                filteredCostCenter = new List<CompanyDivisionCostCenter>();

            return filteredCostCenter?.Count > 0 ? IsCostCenterHasValidSchema(filteredCostCenter, validationType, ref validationMessages) : false;
        }

        private bool IsCostCenterAlreadyAssociatedToCompany(string companyCode, string divisionName, IList<CompanyDivisionCostCenter> companyCostCenters, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.CompanyDivisionCostCenter> costCenterExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var costCenter = companyCostCenters?.Select(x => new { x.CostCenterName, x.CostCenterCode, x.CompanyDivisionCostCenterId }).ToList();
            if(costCenter?.Count > 0)
            {
                var filterExpressions = new List<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>();
                Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate = null;
                Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> containsExpression = null;

                if (validationType == ValidationType.Add)
                {
                    foreach (var cost in costCenter)
                    {
                        containsExpression = a => a.Name == cost.CostCenterName && a.Code == cost.CostCenterCode;
                        filterExpressions.Add(containsExpression);
                    }
                }
                else if (validationType == ValidationType.Update && costCenter != null)
                {
                    foreach (var cost in costCenter)
                    {
                        containsExpression = a => a.Name == cost.CostCenterName && a.Code == cost.CostCenterCode && a.Id != cost.CompanyDivisionCostCenterId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.CompanyDivisionCostCenter>(Expression.OrElse);
                if (predicate != null)
                {
                    containsExpression = a => a.CompanyDivision.Division.Name == divisionName && a.CompanyDivision.Company.Code == companyCode;
                    predicate =containsExpression.CombineWithAndAlso(predicate);
                }
                costCenterExists = this._repository?.FindBy(predicate).ToList();
                costCenterExists?.ToList().ForEach(x =>
                    {
                        string errorCode = MessageType.CostCenterDataAlreadyExists.ToId();
                        messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Name)));
                    });
            }
            //if (validationType == ValidationType.Add && costCenter != null)
            //    costCenterExists = _repository.FindBy(x1 => costCenter.Any(x2 => x2.CostCenterName == x1.Name && x2.CostCenterCode == x1.Code) &&
            //                                                   x1.CompanyDivision.Division.Name == divisionName &&
            //                                                   x1.CompanyDivision.Company.Code == companyCode).ToList();

            //else if (validationType == ValidationType.Update && costCenter != null)
            //{
            //    costCenterExists = _repository?.FindBy(x1 => costCenter.Any(x2 => x2.CostCenterName == x1.Name && x2.CostCenterCode == x1.Code
            //                                                && x2.CompanyDivisionCostCenterId != x1.Id)
            //                                                && x1.CompanyDivision.Division.Name == divisionName &&
            //                                                   x1.CompanyDivision.Company.Code == companyCode
            //                                                   ).ToList();
            //}

            //costCenterExists?.ToList().ForEach(x =>
            //    {
            //        string errorCode = MessageType.CostCenterDataAlreadyExists.ToId();
            //        messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Name)));
            //    });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordUpdateCountMatching(IList<CompanyDivisionCostCenter> companyCostCenters, IList<DbRepository.Models.SqlDatabaseContext.CompanyDivisionCostCenter> dbCompanyCostCenter, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyCostCenters?.Where(x => !dbCompanyCostCenter.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyDivisionCostCenterId)).ToList();
            notMatchedRecords?.ForEach(x =>
            {
                string errorCode = MessageType.CostCenterHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.CostCenterName)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyCostCenter(IList<CompanyDivisionCostCenter> companyCostCenters, IList<DbRepository.Models.SqlDatabaseContext.CompanyDivisionCostCenter> dbCompanyCostCenter, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (dbCompanyCostCenter == null)
                dbCompanyCostCenter = new List<DbRepository.Models.SqlDatabaseContext.CompanyDivisionCostCenter>();

            var notMatchedRecords = companyCostCenters?.Where(x => !dbCompanyCostCenter.ToList().Any(x1 => x1.Id == x.CompanyDivisionCostCenterId)).ToList();
            notMatchedRecords?.ForEach(x =>
            {
                string errorCode = MessageType.CostCenterIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.CompanyDivisionCostCenterId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordCanBeDelete(IList<CompanyDivisionCostCenter> companyCostCenters, IList<DbRepository.Models.SqlDatabaseContext.CompanyDivisionCostCenter> dbCompanyCostCenter, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbCompanyCostCenter?.Where(x => x.Project?.Count > 0).ToList()
                                .ForEach(x =>
                                {
                                    string errorCode = MessageType.CostCenterCannotBeDeleted.ToId();
                                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Name)));
                                });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsCostCenterHasValidSchema(IList<CompanyDivisionCostCenter> companyDivisionCostCenters,ValidationType type,ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companyDivisionCostCenters), type);

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
