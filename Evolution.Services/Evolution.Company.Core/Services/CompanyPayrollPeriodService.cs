using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Core.Services
{
    public class CompanyPayrollPeriodService : ICompanyPayrollPeriodService
    {
        private readonly ICompanyPayrollPeriodRepository _repository = null;
        private readonly IAppLogger<CompanyPayrollPeriodService> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly ICompanyPayrollRepository _companyPayrollRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ICompanyPayrollPeriodValidationService _validationService = null;

        public CompanyPayrollPeriodService(ICompanyPayrollPeriodRepository repository, IAppLogger<CompanyPayrollPeriodService> logger, ICompanyRepository companyRepository, ICompanyPayrollRepository companyPayrollrepository, ICompanyPayrollPeriodValidationService validationService, JObject messages)
        {
            this._repository = repository;
            this._logger = logger;
            this._companyRepository = companyRepository;
            this._companyPayrollRepository = companyPayrollrepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
        }

        #region Public Exposed Method

        public Response DeleteCompanyPayrollPeriod(string companyCode, string payrollType,int? payrollId, IList<CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange = true)
        {
            return RemoveCompanyPayrollPeriods(companyCode, payrollType, payrollId, companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), commitChange);
        }

        public Response GetCompanyPayrollPeriod(CompanyPayrollPeriod searchModel)
        {
            IList<DomainModel.CompanyPayrollPeriod> result = null;
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

        public Response ModifyCompanyPayrollPeriod(string companyCode, string payrollType,int? payrollId, IList<CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange = true)
        {
            var result = this.UpdateCompanyPayrollPeriods(companyCode, payrollType, payrollId, companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyPayrollPeriod(new CompanyPayrollPeriod() { CompanyCode = companyCode, PayrollType = payrollType });
            else
                return result;
        }

        public Response SaveCompanyPayrollPeriod(string companyCode, string payrollType, int? payrollId, IList<CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange = true)
        {
            var result = this.AddCompanyPayrollPeriods(companyCode, payrollType, payrollId, companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyPayrollPeriod(new CompanyPayrollPeriod() { CompanyCode = companyCode, PayrollType = payrollType });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response RemoveCompanyPayrollPeriods(string companyCode, string payrollType,int? payrollId, IList<CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

                IList<CompanyPayrollPeriod> recordToBeDelete = null;
                if (this.IsRecordValidForProcess(companyPayrollPeriods, ValidationType.Delete, ref recordToBeDelete, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        IList<DbModel.CompanyPayroll> dbCompanyPayrolls = null;
                        if (this.IsValidCompanyPayrolls(dbCompany.Id, companyPayrollPeriods , payrollType, payrollId, ValidationType.Delete, ref dbCompanyPayrolls, ref errorMessages))
                        {
                            var dbCompanyPayrollPeriods = _repository.FindBy(x => recordToBeDelete.Select(x1 => x1.CompanyPayrollPeriodId).Contains(x.Id)).ToList();
                            //if (!IsCompanyMapped(dbCompanyPayrollPeriods, ref errorMessages))
                            //{
                                if (IsValidCompanyPayrollPeriod(recordToBeDelete, dbCompanyPayrollPeriods.ToList(), ref errorMessages))
                                {
                                    foreach (var PayrollPeriod in dbCompanyPayrollPeriods)
                                    {   
                                    _repository.Delete(PayrollPeriod);//D800
                                    }

                                    if (commitChange && !_repository.AutoSave && dbCompanyPayrollPeriods?.Count > 0 && errorMessages.Count <= 0)
                                        _repository.ForceSave();
                                }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyPayrollPeriods);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsCompanyMapped(IList<DbModel.CompanyPayrollPeriod> companyPayrollPeriods, ref List<MessageDetail> errorMessages)
        {
            var mappedCompany = companyPayrollPeriods.FirstOrDefault(company => company?.VisitTechnicalSpecialistAccountItemTime?.Count > 0 || company?.VisitTechnicalSpecialistAccountItemConsumable?.Count > 0 || company?.VisitTechnicalSpecialistAccountItemExpense?.Count > 0 ||
                                                      company?.VisitTechnicalSpecialistAccountItemTravel?.Count > 0 || company?.TimesheetTechnicalSpecialistAccountItemConsumable?.Count > 0 || company?.TimesheetTechnicalSpecialistAccountItemExpense?.Count > 0 ||
                                                      company?.TimesheetTechnicalSpecialistAccountItemTime?.Count > 0 || company?.TimesheetTechnicalSpecialistAccountItemTravel?.Count > 0);
            return (mappedCompany != null);
        }

        private Response AddCompanyPayrollPeriods(string companyCode, string payrollType,int? payrollId, IList<CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange)
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
                IList<CompanyPayrollPeriod> recordToBeInserted = null;
                if (this.IsRecordValidForProcess(companyPayrollPeriods, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        IList<DbModel.CompanyPayroll> dbCompanyPayrolls = null;
                        if (this.IsValidCompanyPayrolls(dbCompany.Id, companyPayrollPeriods, payrollType, payrollId, ValidationType.Add, ref dbCompanyPayrolls, ref errorMessages))
                        {
                            IList<DbModel.CompanyPayrollPeriod> dbCompanyPayrollPeriods = null;
                            if (!this.IsPayrollPeriodAlreadyAssociatedToCompanyPayroll(dbCompany.Id, payrollType, recordToBeInserted, ref dbCompanyPayrollPeriods, ValidationType.Update, ref errorMessages))
                            {
                                if (!this.IsDateAlreadyAssociatedToPayRollPeriod(recordToBeInserted, dbCompanyPayrollPeriods, ValidationType.Update, ref errorMessages))
                                {
                                    var dbPayrollPeriodToBeInserted = recordToBeInserted.Select(x => new DbModel.CompanyPayrollPeriod()
                                    {
                                        CompanyPayrollId = dbCompanyPayrolls.FirstOrDefault(x1 => x1.Name.ToLower() == x.PayrollType.ToLower()).Id,//Defect 45 Changes
                                        PeriodName = x.PeriodName,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        PeriodStatus = x.PeriodStatus,
                                        //IsActive = (x.IsActive == false ? null : x.IsActive),//DEF 499
                                        IsActive = x.IsActive ?? false    //ITK D-727
                                    }).ToList();

                                    _repository.Add(dbPayrollPeriodToBeInserted);

                                    if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyPayrollPeriods);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateCompanyPayrollPeriods(string companyCode, string payrollType, int? payrollId, IList<CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                IList<CompanyPayrollPeriod> recordToBeModify = null;
                if (this.IsRecordValidForProcess(companyPayrollPeriods, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                {
                    IList<DbModel.CompanyPayroll> dbCompanyPayrolls = null;
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        if (this.IsValidCompanyPayrolls(dbCompany.Id, companyPayrollPeriods, payrollType, payrollId, ValidationType.Update, ref dbCompanyPayrolls, ref errorMessages))
                        {
                            IList<DbModel.CompanyPayrollPeriod> dbCompanyPayrollPeriods = null;
                            if (!this.IsPayrollPeriodAlreadyAssociatedToCompanyPayroll(dbCompany.Id, payrollType, recordToBeModify, ref dbCompanyPayrollPeriods, ValidationType.Update, ref errorMessages))
                            {
                                var modRecordCompPayrollPeriodId = recordToBeModify.Select(x => x.CompanyPayrollPeriodId).ToList();
                                var compPayrollPeriods = _repository.FindBy(x => modRecordCompPayrollPeriodId.Contains(x.Id));

                                if (IsValidCompanyPayrollPeriod(recordToBeModify, compPayrollPeriods.ToList(), ref errorMessages))
                                {
                                    if (!IsDateAlreadyAssociatedToPayRollPeriod(recordToBeModify, dbCompanyPayrollPeriods, ValidationType.Update, ref errorMessages))
                                    {
                                        if (this.IsRecordUpdateCountMatching(recordToBeModify, compPayrollPeriods.ToList(), ref errorMessages))
                                        {
                                            foreach (var companyPayrollPeriod in compPayrollPeriods)
                                            {
                                                var PayrollPeriod = recordToBeModify.FirstOrDefault(x => x.CompanyPayrollPeriodId == companyPayrollPeriod.Id);
                                                companyPayrollPeriod.CompanyPayrollId = dbCompanyPayrolls.FirstOrDefault(x => x.Name == PayrollPeriod.PayrollType).Id;//Defect 45 Changes
                                                companyPayrollPeriod.PeriodName = PayrollPeriod.PeriodName;
                                                companyPayrollPeriod.StartDate = PayrollPeriod.StartDate;
                                                companyPayrollPeriod.EndDate = PayrollPeriod.EndDate;
                                                companyPayrollPeriod.PeriodStatus = PayrollPeriod.PeriodStatus;
                                                companyPayrollPeriod.IsActive = PayrollPeriod.IsActive ?? companyPayrollPeriod.IsActive;   //ITK D-727
                                                //companyPayrollPeriod.IsActive = (PayrollPeriod.IsActive == false ? null : PayrollPeriod.IsActive); //DEF 499
                                                companyPayrollPeriod.LastModification = DateTime.UtcNow;
                                                companyPayrollPeriod.UpdateCount = PayrollPeriod.UpdateCount.CalculateUpdateCount();
                                                companyPayrollPeriod.ModifiedBy = PayrollPeriod.ModifiedBy;
                                                _repository.Update(companyPayrollPeriod);
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
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyPayrollPeriods);
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

        private bool IsRecordValidForProcess(IList<CompanyPayrollPeriod> companyPayrollPeriods, ValidationType validationType, ref IList<CompanyPayrollPeriod> filteredPayrollPeriods, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredPayrollPeriods = companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredPayrollPeriods = companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredPayrollPeriods = companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredPayrollPeriods?.Count > 0 ? IsCompanyPayrollPeriodHasValidSchema(filteredPayrollPeriods, validationType, ref validationMessages) : false;
        }

        private bool IsValidCompanyPayrolls(int companyId, IList<CompanyPayrollPeriod> companyPayrollPeriods, string payrollType, int? companyPayrollId, ValidationType validationType, ref IList<DbModel.CompanyPayroll> companyPayrolls, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var dbCompanyPayrolls = new List<DbModel.CompanyPayroll>();
            //var dbCompanyPayrolls = this._companyPayrollRepository.FindBy(x => x.CompanyId == companyId && x.Name == payrollType).ToList();//Defect 45 Changes
            if (validationType == ValidationType.Add)
                dbCompanyPayrolls = this._companyPayrollRepository.FindBy(x => x.CompanyId == companyId && x.Name == payrollType).ToList();
            else if(validationType == ValidationType.Update || validationType == ValidationType.Delete)
                dbCompanyPayrolls = this._companyPayrollRepository.FindBy(x => x.CompanyId == companyId && x.Id == companyPayrollId).ToList();
            if (dbCompanyPayrolls?.Count <= 0)
            {
                string errorCode = MessageType.PayrollNotExists.ToId();
                messages.Add(new MessageDetail(errorCode, string.Format(_messageDescriptions[errorCode].ToString(), payrollType)));
            }
            companyPayrolls = dbCompanyPayrolls;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsPayrollPeriodAlreadyAssociatedToCompanyPayroll(int companyId, string payrollType, IList<CompanyPayrollPeriod> companyPayrollPeriods, ref IList<DbModel.CompanyPayrollPeriod> dbCompanyPayrollPeriods, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.CompanyPayrollPeriod> payrollPeriodExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbCompanyPayrollPeriods = _repository?.FindBy(x => x.CompanyPayroll.Company.Id == companyId && x.CompanyPayroll.Name == payrollType && x.IsActive !=null).ToList();//Defect 45 Changes

            if (validationType == ValidationType.Add)
                payrollPeriodExists = dbCompanyPayrollPeriods?.Where(x => companyPayrollPeriods.Any(x1 => (x1.PeriodName == x.PeriodName)&&(bool)(x1.IsActive))).ToList();// && x1.StartDate == x.StartDate && x1.EndDate == x.EndDate && x1.PeriodStatus == x.PeriodStatus
            else if (validationType == ValidationType.Update)
                payrollPeriodExists = dbCompanyPayrollPeriods?.Where(x => companyPayrollPeriods.Any(x1 => x1.CompanyPayrollPeriodId != x.Id && x1.PeriodName == x.PeriodName && (bool)(x1.IsActive))).ToList();// && x1.StartDate == x.StartDate && x1.EndDate == x.EndDate && x1.PeriodStatus == x.PeriodStatus

            payrollPeriodExists?.ToList().ForEach(x =>
            {
                string errorCode = MessageType.CompPayrollPeriod_AlreadyExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyPayroll.Name, x.PeriodName, x.StartDate, x.EndDate, x.PeriodStatus)));//Defect 45 Changes
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordUpdateCountMatching(IList<CompanyPayrollPeriod> companyPayrollPeriods, IList<DbModel.CompanyPayrollPeriod> dbCompanyPayrollPeriods, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyPayrollPeriods.Where(x => !dbCompanyPayrollPeriods.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyPayrollPeriodId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.CompPayollPeriod_UpdateCountMismatch.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.PeriodName)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyPayrollPeriod(IList<CompanyPayrollPeriod> companyPayrollPeriods, IList<DbModel.CompanyPayrollPeriod> dbCompanyPayrollPeriods, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyPayrollPeriods.Where(x => !dbCompanyPayrollPeriods.ToList().Any(x1 => x1.Id == x.CompanyPayrollPeriodId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.CompPayrollPeriodIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyPayrollPeriodId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsDateAlreadyAssociatedToPayRollPeriod(IList<DomainModel.CompanyPayrollPeriod> companyPayrollPeriods, IList<DbModel.CompanyPayrollPeriod> dbCompanyPayrollPeriods, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            List<CompanyPayrollPeriod> matchingRecords = null;

            if (validationType == ValidationType.Add)
                matchingRecords = companyPayrollPeriods.Where(x => dbCompanyPayrollPeriods.Any(x1 => x1.StartDate == x.StartDate || x1.EndDate == x.EndDate)).Select(x => x).ToList();
            if (validationType == ValidationType.Update)
                matchingRecords = companyPayrollPeriods.Where(x => dbCompanyPayrollPeriods.Any(x1 => x1.Id != x.CompanyPayrollPeriodId && (x1.StartDate == x.StartDate || x1.EndDate == x.EndDate))).Select(x => x).ToList();

            matchingRecords?.ForEach(x =>
            {
                string errorCode = MessageType.PayrollPeriodOverlapsEachOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.StartDate.ToString(), x.EndDate.ToString())));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsCompanyPayrollPeriodHasValidSchema(IList<CompanyPayrollPeriod> companyPayrollPeriods, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companyPayrollPeriods), validationType);

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
