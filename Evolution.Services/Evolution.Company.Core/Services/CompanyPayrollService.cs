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
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Core.Services
{
    public class CompanyPayrollService : ICompanyPayrollService
    {
        private readonly ICompanyPayrollRepository _repository = null;
        private readonly IAppLogger<CompanyPayrollService> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly JObject _messages = null;
        private readonly ICompanyPayrollValidationService _validationService = null;

        public CompanyPayrollService(ICompanyPayrollRepository repository, 
                                     IAppLogger<CompanyPayrollService> logger, 
                                     IDataRepository dataRepository, 
                                     ICompanyRepository companyRepository,
                                     ICompanyPayrollValidationService validationService,
                                     JObject messages)
        {
            this._repository = repository;
            this._logger = logger;
            this._dataRepository = dataRepository;
            this._companyRepository = companyRepository;
            this._validationService = validationService;
            this._messages = messages;
        }

        #region Public Exposed Method

        public Response DeleteCompanyPayroll(string companyCode, IList<CompanyPayroll> companyPayroll, bool commitChange = true)
        {
            return RemoveCompanyPayrolls(companyCode, companyPayroll?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList(), commitChange);
        }

        public Response GetCompanyPayroll(CompanyPayroll searchModel)
        {
            IList<DomainModel.CompanyPayroll> result = null;
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

        public Response ModifyCompanyPayroll(string companyCode, IList<CompanyPayroll> companyPayroll, bool commitChange = true)
        {
            var result = this.UpdateCompanyPayrolls(companyCode, companyPayroll?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList(), commitChange, companyPayroll);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyPayroll(new CompanyPayroll() { CompanyCode = companyCode });
            else
                return result;
        }

        public Response SaveCompanyPayroll(string companyCode, IList<CompanyPayroll> companyPayroll, bool commitChange = true)
        {
            var result = this.AddCompanyPayrolls(companyCode, companyPayroll?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList(), commitChange, companyPayroll);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyPayroll(new CompanyPayroll() { CompanyCode = companyCode });
            else
                return result;
        }

        public bool IsValidCompanyPayroll(IList<KeyValuePair<string, string>> compPayrollNames, 
                                          ref IList<DbModel.CompanyPayroll> dbCompanyPayroll, 
                                          ref IList<ValidationMessage> valdMessages,
                                          params Expression<Func<DbModel.CompanyPayroll, object>>[] includes)
        {
            bool? result = false;
            var messages = new List<ValidationMessage>();
            if (compPayrollNames?.Count() > 0)
            {
                var compCodes = compPayrollNames.Select(x => x.Key).ToList();
                var payrollTypes = compPayrollNames.Select(x => x.Value).ToList();

                dbCompanyPayroll = _repository.FindBy(x => compCodes.Contains(x.Company.Code) && payrollTypes.Contains(x.Name), includes).ToList();//Defect 45 Changes

                IList <DbModel.CompanyPayroll> dbDatas = dbCompanyPayroll;
                var payrollNotExists = payrollTypes?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();//Defect 45 Changes
                payrollNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.PayrollNotExists, x);
                });
                valdMessages = messages;
                
            }
            
            return (bool)!result;
        }

        #endregion

        #region Private Exposed Methods

        private Response RemoveCompanyPayrolls(string companyCode, IList<CompanyPayroll> companyPayrolls, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<CompanyPayroll> recordToBeDelete = null;
                validationMessages = new List<ValidationMessage>();
                if (this.IsRecordValidForProcess(companyPayrolls, ValidationType.Delete, ref recordToBeDelete, ref errorMessages,ref validationMessages))                    
                {
                    if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                    {
                        //IList<DbModel.Data> dbPayrollTypes = null;
                        // if (this.IsValidPayrollType(companyPayrolls, ref dbPayrollTypes, ref errorMessages))
                        // {
                            IList<DbModel.CompanyPayroll> dbCompanyPayrolls = _repository?.FindBy(x => recordToBeDelete.Select(x1 => x1.PayrollType).Contains(x.Name) && x.Company.Code == companyCode).ToList();
                            if (IsValidCompanyPayroll(recordToBeDelete, dbCompanyPayrolls, ref errorMessages))
                            {
                                if (this.IsRecordCanBeDelete(dbCompanyPayrolls, ref errorMessages))
                                {
                                    foreach (var Payroll in dbCompanyPayrolls)
                                    {
                                        _repository.Delete(Payroll);
                                    }

                                    if (commitChange && !_repository.AutoSave && dbCompanyPayrolls?.Count > 0 && errorMessages.Count <= 0)
                                        _repository.ForceSave();
                                }
                            // }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyPayrolls);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response AddCompanyPayrolls(string companyCode, IList<CompanyPayroll> companyPayrolls, bool commitChange, IList<CompanyPayroll> allCompanyPayrolls)
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
                    IList<CompanyPayroll> recordToBeInserted = null;
                    if (this.IsRecordValidForProcess(companyPayrolls, ValidationType.Add, ref recordToBeInserted, ref errorMessages,ref validationMessages))
                    {
                        if (!this.IsPayrollTypeAlreadyAssociatedToCompany(companyCode, recordToBeInserted, ValidationType.Add, ref errorMessages, allCompanyPayrolls))
                        {
                            //IList<DbModel.Data> dbPayrollTypes = null;
                           // IList<DbModel.Data> dbExportType = null;
                            //if (this.IsValidPayrollType(companyPayrolls, ref dbPayrollTypes, ref errorMessages))
                            //{
                                //if (this.IsValidExportPrefix(companyPayrolls, ref dbExportType, ref errorMessages))
                                //{
                                    var dbPayrollTypeToBeInserted = recordToBeInserted.Select(x => new DbModel.CompanyPayroll()
                                    {
                                        CompanyId = dbCompany.Id,
                                        Name=x.PayrollType,//Defect 45 Changes
                                        Currency = x.Currency,
                                        ExportPrefix = x.ExportPrefix
                                    }).ToList();

                                    _repository.Add(dbPayrollTypeToBeInserted);

                                    if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                        _repository.ForceSave();
                                //}
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyPayrolls);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateCompanyPayrolls(string companyCode, IList<CompanyPayroll> companyPayrolls, bool commitChange, IList<CompanyPayroll> allCompanyPayrolls)
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
                    IList<CompanyPayroll> recordToBeModify = null;
                    if (this.IsRecordValidForProcess(companyPayrolls, ValidationType.Update, ref recordToBeModify, ref errorMessages,ref validationMessages))
                    {
                        if (!this.IsPayrollTypeAlreadyAssociatedToCompany(companyCode, recordToBeModify, ValidationType.Update, ref errorMessages, allCompanyPayrolls))
                        {
                            //IList<DbModel.Data> dbPayrollTypes = null;
                            //IList<DbModel.Data> dbExportPrefix = null;
                            
                            //if (this.IsValidPayrollType(companyPayrolls, ref dbPayrollTypes, ref errorMessages))
                            //{
                                //if (this.IsValidExportPrefix(companyPayrolls, ref dbExportPrefix, ref errorMessages))
                                //{
                                    var modRecordCompPayrollId = recordToBeModify.Select(x => x.CompanyPayrollId).ToList();
                                    var compPayrolls = _repository.FindBy(x => x.Company.Code == companyCode && modRecordCompPayrollId.Contains(x.Id));

                                if (IsValidCompanyPayroll(recordToBeModify, compPayrolls.ToList(), ref errorMessages))
                                    {
                                        if (this.IsRecordUpdateCountMatching(recordToBeModify, compPayrolls.ToList(), ref errorMessages))
                                        {
                                            foreach (var companyPayroll in compPayrolls)
                                            {
                                             var payroll = recordToBeModify.FirstOrDefault(x => x.CompanyPayrollId == companyPayroll.Id);
                                                    companyPayroll.CompanyId = dbCompany.Id;                                                    companyPayroll.Name = payroll.PayrollType; //Defect 45 Chanages
                                                    companyPayroll.ExportPrefix = payroll.ExportPrefix;
                                                    companyPayroll.Currency = payroll.Currency;
                                                    companyPayroll.LastModification = DateTime.UtcNow;
                                                    companyPayroll.UpdateCount = payroll.UpdateCount.CalculateUpdateCount();
                                                    companyPayroll.ModifiedBy = payroll.ModifiedBy;
                                                     _repository.Update(companyPayroll);
                                            
                                            }

                                            if (commitChange && recordToBeModify?.Count > 0)
                                                _repository.ForceSave();
                                        }
                                }
                                //}
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyPayrolls);
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
                errorMessages.Add(new MessageDetail(ModuleType.Company, MessageType.InvalidCompanyCode.ToId(), _messages[MessageType.InvalidCompanyCode.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private bool IsRecordValidForProcess(IList<CompanyPayroll> companyPayrolls, ValidationType validationType, ref IList<CompanyPayroll> filteredCompanyPayrolls, ref List<MessageDetail> errorMessages,ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredCompanyPayrolls = companyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredCompanyPayrolls = companyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredCompanyPayrolls = companyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredCompanyPayrolls?.Count > 0?IsPayrollHasvalidSchema(filteredCompanyPayrolls,validationType,ref validationMessages):false;
        }


        private bool IsValidPayrollType(IList<CompanyPayroll> companyPayrolls, ref IList<DbModel.Data> payrollTypes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var payrollTypeNames = companyPayrolls.Select(x => x.PayrollType).ToList();

            var dbPayrollTypes = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.PayrollType) &&
                                                                 payrollTypeNames.Contains(x.Name)).ToList();


            var payrollTypeNotExists = dbPayrollTypes.Count > 0 ? companyPayrolls.Where(x => !dbPayrollTypes.Any(x1 => x1.Name.ToLower() == x.PayrollType.ToLower())).ToList() : null;
            if (payrollTypeNotExists != null)
            {
                payrollTypeNotExists.ForEach(x =>
               {
                   string errorCode = MessageType.PayrollNotExists.ToId();
                   messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messages[errorCode].ToString(), x.PayrollType)));
               });
            }
            payrollTypes = dbPayrollTypes;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        //private bool IsValidExportPrefix(IList<CompanyPayroll> companyPayrolls, ref IList<DbModel.Data> exportTypes, ref List<MessageDetail> errorMessages)
        //{
        //    List<MessageDetail> messages = new List<MessageDetail>();
        //    if (errorMessages == null)
        //        errorMessages = new List<MessageDetail>();

        //    var exportPrefix = companyPayrolls.Select(x => x.ExportPrefix).ToList();

        //    var dbExportTypes = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.PayrollExportPrefix) &&
        //                                                         exportPrefix.Contains(x.Name)).ToList();

        //    var exportPrefixNotExists = companyPayrolls.Where(x => !dbExportTypes.Any(x1 => x1.Name.ToString().ToLower() ==x.ExportPrefix.ToString().ToLower())).ToList();
        //    exportPrefixNotExists.ForEach(x =>
        //    {
        //        string errorCode = MessageType.ExportPrefixNotExists.ToId();
        //        messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messages[errorCode].ToString(), x.ExportPrefix)));
        //    });

        //    exportTypes = dbExportTypes;

        //    if (messages.Count > 0)
        //        errorMessages.AddRange(messages);

        //    return errorMessages?.Count <= 0;
        //}

        private bool IsRecordCanBeDelete(IList<DbModel.CompanyPayroll> dbCompanyPayrolls, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
             
            dbCompanyPayrolls.Where(x => x.CompanyPayrollPeriod?.Count > 0 || x.TechnicalSpecialist?.Count > 0).ToList()
                                .ForEach(x =>
                                {
                                    string errorCode = MessageType.PayrollCannotBeDeleted.ToId();
                                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messages[errorCode].ToString(), x.Name)));
                                });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsPayrollTypeAlreadyAssociatedToCompany(string companyCode, IList<CompanyPayroll> companyPayrolls, ValidationType validationType, ref List<MessageDetail> errorMessages, IList<CompanyPayroll> allCompanyPayrolls)
        {
            IList<DbModel.CompanyPayroll> payrollTypeExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            var filterExpressions = new List<Expression<Func<DbModel.CompanyPayroll, bool>>>();
            Expression<Func<DbModel.CompanyPayroll, bool>> predicate = null;
            Expression<Func<DbModel.CompanyPayroll, bool>> containsExpression = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var allpayrollTypes= allCompanyPayrolls.Select(x => new { x.PayrollType, x.Currency, x.CompanyPayrollId, x.ExportPrefix }).ToList();//Chandra 
            var payrollTypes = companyPayrolls.Select(x => new { x.PayrollType, x.Currency,x.CompanyPayrollId,x.ExportPrefix}).ToList();
            List<int?> ids = null;
            if (allpayrollTypes?.Count > 0)
            {
                ids = allpayrollTypes.Select(x2 => x2.CompanyPayrollId).ToList();//chandra
            }
            if (payrollTypes?.Count > 0)
            {  
                if (validationType == ValidationType.Add  && ids != null){
                   
                    foreach (var payrollType in payrollTypes)
                    {
                        containsExpression = a => a.Name == payrollType.PayrollType && !(ids.Contains(a.Id));//chandra
                        filterExpressions.Add(containsExpression);
                    }
                }

                else if (validationType == ValidationType.Update && ids != null)
                {
                    foreach (var payrollType in payrollTypes)
                    {
                        containsExpression = a => a.Name == payrollType.PayrollType && a.Id != payrollType.CompanyPayrollId &&  !(ids.Contains(a.Id));//chandra
                        filterExpressions.Add(containsExpression);
                    }

                }
                predicate = filterExpressions.CombinePredicates<DbModel.CompanyPayroll>(Expression.OrElse);
                containsExpression = a => a.Company.Code == companyCode;
                predicate =containsExpression.CombineWithAndAlso(predicate);
                
                payrollTypeExists = this._repository?.FindBy(predicate).ToList();

                payrollTypeExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.PayrollTypeExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messages[errorCode].ToString(), x.Name, x.Currency)));
                });
            } 
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordUpdateCountMatching(IList<CompanyPayroll> companyPayrolls, IList<DbModel.CompanyPayroll> dbCompanyPayrolls, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyPayrolls.Where(x => !dbCompanyPayrolls.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyPayrollId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.PayrollRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messages[errorCode].ToString(), x.PayrollType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyPayroll(IList<CompanyPayroll> companyPayrolls, IList<DbModel.CompanyPayroll> dbCompanyPayrolls, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyPayrolls.Where(x => !dbCompanyPayrolls.ToList().Any(x1 => x1.Id == x.CompanyPayrollId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.PayrollIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messages[errorCode].ToString(), x.CompanyPayrollId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }


        private bool IsPayrollHasvalidSchema(IList<CompanyPayroll> filteredCompanyPayrolls, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(filteredCompanyPayrolls), validationType);

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
