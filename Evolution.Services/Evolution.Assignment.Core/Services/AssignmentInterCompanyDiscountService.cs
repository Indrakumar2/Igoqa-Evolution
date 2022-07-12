using AutoMapper;
using Evolution.Assignment.Domain.Enums;
//using Evolution.Assignment.Domain.Enum;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Contract.Domain.Enums;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentInterCompanyDiscountService : IAssignmentInterCompanyDiscountService
    {
        private readonly IAppLogger<AssignmentInterCompanyDiscountService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentInterCompanyDiscountRepository _repository = null;
        private readonly IAssignmentInterCompanyDiscountValidationService _validationService;
        private readonly ICompanyService _companyService = null;
        private readonly IAssignmentService _assignmentService = null;

        #region Constructor

        public AssignmentInterCompanyDiscountService(IAppLogger<AssignmentInterCompanyDiscountService> logger,
                                JObject messgaes,
                                IAssignmentInterCompanyDiscountRepository repository,
                                IAssignmentInterCompanyDiscountValidationService validationService,
                                ICompanyService companyService,
                                IAssignmentService assignmentService
                                )
        {
            this._logger = logger;
            this._messageDescriptions = messgaes;
            this._repository = repository;
            this._validationService = validationService;
            this._assignmentService = assignmentService;
            this._companyService = companyService;
        }
        #endregion

        #region Get

        public Response Get(int assignmentId)
        {
            DomainModel.AssignmentInterCoDiscountInfo result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = this._repository.Search(assignmentId);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, null);
        }

        public Response IsRecordValidForProcess(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts = null;
            IList<DbModel.Company> dbCompanies = null;
            DbModel.Assignment dbAssignment = null;
            return IsRecordValidForProcess(interCompanyDiscounts, ref dbAssignment, ref dbCompanies, validationType, ref dbInterCompanyDiscounts);
        }

        public Response IsRecordValidForProcess(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                                ref DbModel.Assignment dbAssignment,
                                                ref IList<DbModel.Company> dbCompanies,
                                                ValidationType validationType,
                                                ref IList<DbModel.AssignmentInterCompanyDiscount> dbisnterCompanyDiscount)
        {
            DomainModel.AssignmentInterCoDiscountInfo filteredRecords = null;
            IList<ValidationMessage> validationMessages = null;
            return IsRecordValidForProcess(interCompanyDiscounts, validationType, ref dbAssignment, ref dbCompanies, ref filteredRecords, ref dbisnterCompanyDiscount, ref validationMessages);
        }

        public Response IsRecordValidForProcess(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                                ValidationType validationType,
                                                DbModel.Assignment dbAssignment,
                                                IList<DbModel.Company> dbCompanies,
                                                IList<DbModel.AssignmentInterCompanyDiscount> dbinterCompanyDiscounts)
        {
            return IsRecordValidForProcess(interCompanyDiscounts, ref dbAssignment, ref dbCompanies, validationType, ref dbinterCompanyDiscounts);
        }

        public Response Add(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> recordToAdd = null;
            DbModel.Assignment dbAssignment = null;
            IList<DbModel.Company> dbCompanies = null;
            return AddInterCompanyDiscounts(assignmentInterCompanyDiscounts, ref recordToAdd, ref dbAssignment, ref dbCompanies, commitChange, isValidationRequire);
        }

        public Response Add(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                            ref DbModel.Assignment dbAssignments,
                            ref IList<DbModel.Company> dbCompanies,
                            ref IList<DbModel.AssignmentInterCompanyDiscount> dbAssignmentInterCompanyDiscounts,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {

            return AddInterCompanyDiscounts(assignmentInterCompanyDiscounts,
                                            ref dbAssignmentInterCompanyDiscounts,
                                            ref dbAssignments,
                                            ref dbCompanies,
                                            commitChange,
                                            isValidationRequire);
        }

        public Response Modify(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> recordToUpdate = null;
            IList<DbModel.Company> dbCompanies = null;
            DbModel.Assignment dbAssignment = null;
            return UpdateInterCompanyDiscounts(assignmentInterCompanyDiscounts,
                                               ref recordToUpdate,
                                               ref dbAssignment,
                                               ref dbCompanies,
                                               commitChange,
                                               isValidationRequire);
        }

        public Response Modify(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                               ref DbModel.Assignment dbAssignments,
                               ref IList<DbModel.Company> dbCompanies,
                               ref IList<DbModel.AssignmentInterCompanyDiscount> dbAssignmentInterCompanyDiscounts,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            return UpdateInterCompanyDiscounts(assignmentInterCompanyDiscounts,
                                                ref dbAssignmentInterCompanyDiscounts,
                                                ref dbAssignments,
                                                ref dbCompanies,
                                                commitChange,
                                                isValidationRequire);
        }

        public Response Delete(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> recordToDelete = null;
            return DeleteInterCompanyDiscounts(interCompanyDiscounts,
                                                ref recordToDelete,
                                                commitChange,
                                                isValidationRequire);

        }

        #endregion

        #region private Methods


        private Response AddInterCompanyDiscounts(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                                  ref IList<DbModel.AssignmentInterCompanyDiscount> recordToAdd,
                                                  ref DbModel.Assignment dbAssignments,
                                                  ref IList<DbModel.Company> dbCompanies,
                                                  bool commitChange,
                                                  bool isValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            Response validResponse = null;
            try
            {
                var filteredRecords = FilterRecord(interCompanyDiscounts, ValidationType.Add);
                if (filteredRecords != null && filteredRecords.AssignmentId > 0)
                {
                    if (isValidationRequire)
                        validResponse = IsRecordValidForProcess(interCompanyDiscounts,
                                                                ValidationType.Add,
                                                                ref dbAssignments,
                                                                ref dbCompanies,
                                                                ref filteredRecords,
                                                                ref recordToAdd,
                                                                ref validationMessages);

                    if (!isValidationRequire || Convert.ToBoolean(validResponse.Result))
                    {
                        recordToAdd = PopulateRecordsToAdd((int)filteredRecords.AssignmentId,
                                                                filteredRecords,
                                                                dbAssignments,
                                                                dbCompanies,
                                                                ref validationMessages);
                        if (recordToAdd?.Count > 0)
                        {
                            _repository.AutoSave = false;
                            if (recordToAdd.Count > 0)
                                _repository.Add(recordToAdd);
                            if (commitChange)
                                _repository.ForceSave();
                        }
                    }
                    return validResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), interCompanyDiscounts);
            }
            finally
            {
                _repository.AutoSave = true;
                //_repository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateInterCompanyDiscounts(DomainModel.AssignmentInterCoDiscountInfo intercompanyDiscounts,
                                                    ref IList<DbModel.AssignmentInterCompanyDiscount> dbRecordToUpdate,
                                                    ref DbModel.Assignment dbAssignments,
                                                    ref IList<DbModel.Company> companies,
                                                    bool commitChange,
                                                    bool isValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            DbModel.Assignment dbAssignment = null;
            IList<DbModel.Company> dbCompanies = null;
            Response validResponse = null;
            try
            {
                var filteredRecords = FilterRecord(intercompanyDiscounts, ValidationType.Update);
                if (filteredRecords != null && filteredRecords.AssignmentId > 0)
                {
                    if (isValidationRequire)
                        validResponse = IsRecordValidForProcess(intercompanyDiscounts, ValidationType.Update, ref dbAssignment, ref dbCompanies, ref filteredRecords, ref dbRecordToUpdate, ref validationMessages);
                    if (!isValidationRequire || Convert.ToBoolean(validResponse.Result))
                    {

                        _repository.AutoSave = false;
                        dbRecordToUpdate = PopulateRecordsToUpdate((int)filteredRecords.AssignmentId, intercompanyDiscounts, dbRecordToUpdate, ref validationMessages);
                        if (dbRecordToUpdate.Count > 0)
                            _repository.Update(dbRecordToUpdate);
                        if (commitChange)
                            _repository.ForceSave();
                    }
                    return validResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), intercompanyDiscounts);
            }
            finally
            {
                _repository.AutoSave = true;
               // _repository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response DeleteInterCompanyDiscounts(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscount,
                                                    ref IList<DbModel.AssignmentInterCompanyDiscount> recordToDelete,
                                                    bool commitChange,
                                                    bool isValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            DbModel.Assignment dbAssignment = null;
            IList<DbModel.Company> dbCompanies = null;
            Response validResponse = null;
            try
            {
                var filteredRecords = FilterRecord(interCompanyDiscount, ValidationType.Delete);
                if (filteredRecords != null && filteredRecords.AssignmentId > 0)
                {
                    if (isValidationRequire)
                        validResponse = IsRecordValidForProcess(interCompanyDiscount, ValidationType.Delete, ref dbAssignment, ref dbCompanies, ref filteredRecords, ref recordToDelete, ref validationMessages);
                    if (!isValidationRequire || Convert.ToBoolean(validResponse.Result))
                    {
                        _repository.AutoSave = false;
                        recordToDelete = PopulateRecordsToDelete((int)filteredRecords.AssignmentId, interCompanyDiscount, ref validationMessages);
                        if (recordToDelete.Count > 0)
                            _repository.Delete(recordToDelete);
                        if (commitChange)
                            _repository.ForceSave();
                    }
                    return validResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), interCompanyDiscount);
            }
            finally
            {
                _repository.AutoSave = true;
              //  _repository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response IsRecordValidForProcess(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                                ValidationType validationType,
                                                ref DbModel.Assignment dbAssignment,
                                                ref IList<DbModel.Company> dbCompanies,
                                                ref DomainModel.AssignmentInterCoDiscountInfo filteredRecords,
                                                ref IList<DbModel.AssignmentInterCompanyDiscount> recordsToProcess,
                                                ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (filteredRecords == null)
                filteredRecords = FilterRecord(assignmentInterCoDiscountInfo, validationType);

            if (filteredRecords != null
                && IsValidPayload(filteredRecords, validationType, ref validationMessages))
            {
                if (IsValidAssignment((int)filteredRecords.AssignmentId, ref dbAssignment, ref validationMessages))
                {
                    IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts = dbAssignment.AssignmentInterCompanyDiscount.ToList();

                    if (validationType == ValidationType.Add)
                        result = IsRecordValidForAdd(filteredRecords, ref dbAssignment, ref dbCompanies, ref recordsToProcess, ref validationMessages);

                    if (validationType == ValidationType.Update)
                        result = IsRecordValidForUpdate(filteredRecords, ref dbInterCompanyDiscounts, ref recordsToProcess, ref validationMessages);

                    if (validationType == ValidationType.Delete)
                        result = IsRecordValidForDelete(filteredRecords, ref dbInterCompanyDiscounts, ref recordsToProcess, ref validationMessages);
                }

            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, null);
        }

        private DomainModel.AssignmentInterCoDiscountInfo FilterRecord(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                                                       ValidationType validationType)
        {
            DomainModel.AssignmentInterCoDiscountInfo filteredRecords = null;

            if (validationType == ValidationType.Add && assignmentInterCoDiscountInfo.RecordStatus.IsRecordStatusNew())
                filteredRecords = assignmentInterCoDiscountInfo;
            if (validationType == ValidationType.Update && assignmentInterCoDiscountInfo.RecordStatus.IsRecordStatusModified())
                filteredRecords = assignmentInterCoDiscountInfo;
            if (validationType == ValidationType.Delete && assignmentInterCoDiscountInfo.RecordStatus.IsRecordStatusDeleted())
                filteredRecords = assignmentInterCoDiscountInfo;
            return filteredRecords;
        }

        private bool IsValidPayload(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmentInterCoDiscountInfo), validationType);

            if (validationResults?.Count > 0)
                validMessages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            validationMessages = validMessages;

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                        ref DbModel.Assignment dbAssignment,
                                        ref IList<DbModel.Company> dbCompanies,
                                        ref IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;

            IList<string> companyCodes = new List<string>() {
                                                                    assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code,
                                                                    assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code,
                                                                    assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode,
                                                                    assignmentInterCoDiscountInfo.AssignmentHostcompanyCode,
                                                                    assignmentInterCoDiscountInfo.AssignmentOperatingCompanyCode,
                                                                    assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode}
                                                                    .Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();


            if (dbCompanies == null)
                GetCompaniesByCode(companyCodes, ref dbCompanies, ref validationMessages);

            dbInterCompanyDiscounts = dbAssignment?.AssignmentInterCompanyDiscount?.ToList();

            if (IsDiscountDoesNotExceed(assignmentInterCoDiscountInfo, ref validationMessages) && validationMessages?.Count == 0)
                result = true;
            return result;
        }

        private bool IsRecordValidForUpdate(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                            ref IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts,
                                            ref IList<DbModel.AssignmentInterCompanyDiscount> recordToUpdate,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;

            if (IsDiscountDoesNotExceed(assignmentInterCoDiscountInfo, ref validationMessages))
                result = true;

            return result;

        }

        private bool IsRecordValidForDelete(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                            ref IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts,
                                            ref IList<DbModel.AssignmentInterCompanyDiscount> recordToDelete,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (IsDiscountDoesNotExceed(assignmentInterCoDiscountInfo, ref validationMessages))
                result = true;

            return result;
        }

        private DbModel.Assignment GetAssignments(int assignmentId,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            List<int> assignmentIds = null;
            assignmentIds.Add(assignmentId);

            _assignmentService.IsValidAssignment(assignmentIds, ref dbAssignment, ref validationMessages);
            return dbAssignment?.Count > 0 ? dbAssignment?.FirstOrDefault() : null;
        }

        private DbModel.AssignmentInterCompanyDiscount GetDbRecordsToAdd(int assignmentId,
                                                                        string discountType,
                                                                        int companyId,
                                                                        string description,
                                                                        decimal discountPercentage)
        {

            return new DbModel.AssignmentInterCompanyDiscount()
            {
                AssignmentId = assignmentId,
                DiscountType = discountType,
                CompanyId = companyId,
                Description = description,
                Percentage = discountPercentage
            };
        }

        private bool IsDiscountDoesNotExceed(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                            ref IList<ValidationMessage> validationMessages)
        {

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;

            var discount = interCompanyDiscounts;

            if (discount.AssignmentAdditionalIntercompany1_Discount == null)
                discount.AssignmentAdditionalIntercompany1_Discount = 0;

            if (discount.AssignmentAdditionalIntercompany2_Discount == null)
                discount.AssignmentAdditionalIntercompany2_Discount = 0;

            if (discount.AssignmentContractHoldingCompanyDiscount == null)
                discount.AssignmentContractHoldingCompanyDiscount = 0;

            if (discount.AssignmentHostcompanyDiscount == null)
                discount.AssignmentHostcompanyDiscount = 0;

            if (discount.AssignmentOperatingCompanyDiscount == null)
                discount.AssignmentOperatingCompanyDiscount = 0;

            if (discount.ParentContractHoldingCompanyDiscount == null)
                discount.ParentContractHoldingCompanyDiscount = 0;

            decimal totalDiscount = (decimal)(discount.AssignmentAdditionalIntercompany1_Discount
                                    + discount.AssignmentAdditionalIntercompany2_Discount
                                    + discount.AssignmentContractHoldingCompanyDiscount
                                    + discount.AssignmentHostcompanyDiscount
                                    + discount.AssignmentOperatingCompanyDiscount
                                    + discount.ParentContractHoldingCompanyDiscount
                                    );
            if (totalDiscount > 100)
            {
                validMessages.Add(_messageDescriptions, ModuleType.Assignment, MessageType.DiscountSumExceeds);
            }


            validationMessages = validMessages;

            return validationMessages?.Count <= 0;


        }

        private bool IsValidCompany(IList<string> companyCodes,
                                    IList<DbModel.Company> dbCompanys,
                                    ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var validMessages = validationMessages;

            var companyNotExists = companyCodes.Where(x => !dbCompanys.Any(x1 => x1.Code == x)).ToList();

            companyNotExists?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, ModuleType.Assignment, MessageType.InterCompanyDiscountInvalidCompanyCode, x);
            });

            validationMessages = validMessages;
            return validationMessages?.Count <= 0;
        }

        private bool IsValidAssignment(int assignmentId,
                                       ref DbModel.Assignment dbAssignment,
                                       ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            if (dbAssignment == null)
                dbAssignment = GetAssignments(assignmentId, ref validMessages);

            if (dbAssignment == null)
                validMessages.Add(_messageDescriptions, ModuleType.Assignment, MessageType.InterCompanyDiscountInvalidAssignment, assignmentId);

            validationMessages = validMessages;

            return validationMessages?.Count <= 0;
        }

        private void GetCompaniesByCode(IList<string> companyCodes,
                                                          ref IList<DbModel.Company> dbCompanies,
                                                          ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (dbCompanies == null)
                _companyService.IsValidCompany(companyCodes,
                                           ref dbCompanies,
                                           ref validationMessages);
        }

        private DbModel.AssignmentInterCompanyDiscount GetRecordsToUpdate(string DiscountType,
                                                                          decimal discountPercentage,
                                                                          string description,
                                                                          string modifiedBy,String AmendmentReason,
                                                                          IList<DbModel.AssignmentInterCompanyDiscount> interCompanyDiscounts)
        {
            var dbInterCompanyDiscount = interCompanyDiscounts.Where(x => x.DiscountType == DiscountType)?.FirstOrDefault();
            if (dbInterCompanyDiscount != null)
            {
                dbInterCompanyDiscount.Description = description;
                dbInterCompanyDiscount.Percentage = discountPercentage;
                dbInterCompanyDiscount.LastModification = DateTime.UtcNow;
                dbInterCompanyDiscount.UpdateCount = dbInterCompanyDiscount.UpdateCount.CalculateUpdateCount();
                dbInterCompanyDiscount.ModifiedBy = modifiedBy;
            }
            return dbInterCompanyDiscount;
        }

        private IList<DbModel.AssignmentInterCompanyDiscount> PopulateRecordsToAdd(int assignmentId,
                                                                                   DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                                                                   DbModel.Assignment dbAssignments,
                                                                                   IList<DbModel.Company> dbCompanies,
                                                                                   ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts = dbAssignments?.AssignmentInterCompanyDiscount?.ToList();
            IList<DbModel.AssignmentInterCompanyDiscount> recordToAdd = new List<DbModel.AssignmentInterCompanyDiscount>();
            int? companyId = null;
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode)
                     && assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null
                     && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription)
                     && dbAssignments.Project.Contract.ContractType == ContractType.CHD.ToString()

                     && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract)) : null) == null)
            {
                companyId = dbCompanies?.ToList()?.Where(x => x.Code == assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode)?.FirstOrDefault()?.Id;

                var data = GetDbRecordsToAdd(dbAssignments.Id,
                                                  EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract),
                                                  (int)companyId,
                                                  assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription,
                                                  (decimal)assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentOperatingCompanyCode)
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)
                && assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription)
                && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany)) : null) == null
                && assignmentInterCoDiscountInfo.AssignmentOperatingCompanyCode != assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)
            {
                companyId = dbCompanies.Where(x => x.Code == assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)?.FirstOrDefault()?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id,
                                                  EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany),
                                                  (int)companyId,
                                                  assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription,
                                                  (decimal)assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description)

                && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId)) : null) == null)
            {
                companyId = dbCompanies.Where(x => x.Code == assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)?.FirstOrDefault()?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id,
                                                  EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId),
                                                  (int)companyId,
                                                  assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description,
                                                  (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount);
                if(data!=null)
                recordToAdd.Add(data);

            }


            if (assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code)
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description)
                && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2)) : null) == null)

            {
                companyId = dbCompanies.Where(x => x.Code == assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code)?.FirstOrDefault().Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id,
                                                 EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2),
                                                  (int)companyId,
                                                  assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description,
                                                  (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount);
                if(data!=null)
                recordToAdd.Add(data);

            }

            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode)
                && assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription)
                && dbAssignments.ContractCompanyId != dbAssignments.OperatingCompanyId
                 && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract)) : null) == null)
            {
                companyId = dbCompanies.Where(x => x.Code == assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode)?.FirstOrDefault().Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id,
                                                 EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract),
                                                  (int)companyId,
                                                  assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription,
                                                  (decimal)assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount);
                if(data!=null)
                recordToAdd.Add(data);

            }
            return recordToAdd;
        }

        private IList<DbModel.AssignmentInterCompanyDiscount> PopulateRecordsToUpdate(int assignmentId,
                                                                                      DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                                                                      IList<DbModel.AssignmentInterCompanyDiscount> dbRecordToUpdate,
                                                                                      ref IList<ValidationMessage> validationMessages)
        {
            if (dbRecordToUpdate == null)
                dbRecordToUpdate = GetAssignments(assignmentId, ref validationMessages)?.AssignmentInterCompanyDiscount?.ToList();

            IList<DbModel.AssignmentInterCompanyDiscount> recordToUpdate = new List<DbModel.AssignmentInterCompanyDiscount>();

            // Parent Contract
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode)
            && assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null
            && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription)
            && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract)) != null)
            {
                recordToUpdate.Add(GetRecordsToUpdate(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract),
                                                     (decimal)assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount,
                                                     assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription,
                                                     assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason,
                                                     dbRecordToUpdate));

            }

            // Additional InterCompany Office 1
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description)
                && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId)) != null)
            {
                var data = GetRecordsToUpdate(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId),
                                                      (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount,
                                                      assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description,
                                                      assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason,
                                                      dbRecordToUpdate);
                if (data != null) 
                recordToUpdate.Add(data);

            }

            // Additional InterCompany Office 2
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code)
                && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description)
                && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2)) != null)
            {
                var data = GetRecordsToUpdate(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2),
                                                      (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount,
                                                      assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description,
                                                      assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason,
                                                      dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Host Company

            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)
                && assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription)
                && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany)) != null)
            {
                var data = GetRecordsToUpdate(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany),
                                                      (decimal)assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount,
                                                      assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription,
                                                      assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason,
                                                      dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Contract Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode)
                  && assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null
                  && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription)
                  && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract)) != null)
            {
                var data = GetRecordsToUpdate(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract),
                                                     (decimal)assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount,
                                                     assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription,
                                                     assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason,
                                                     dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);

            }

            return recordToUpdate;
        }

        private IList<DbModel.AssignmentInterCompanyDiscount> PopulateRecordsToDelete(int assignmentId,
                                                                                      DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo,
                                                                                      ref IList<ValidationMessage> validationMessages)
        {

            IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts = GetAssignments(assignmentId, ref validationMessages)?.AssignmentInterCompanyDiscount?.ToList();
            IList<DbModel.AssignmentInterCompanyDiscount> recordToDelete = new List<DbModel.AssignmentInterCompanyDiscount>();

            // Parent Contract
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode)
            && assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null
            && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract)) != null)
            {
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract)));

            }

            // Additional InterCompany Office 1
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null
                && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId)) != null)
            {
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId)));
            }

            // Additional InterCompany Office 2
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code)
                && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null
                && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2)) != null)
            {
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2)));
            }


            // Host Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)
                && assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount != null
                && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany)) != null)
            {
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany)));
            }

            // Contract Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode)
                  && assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null
                  && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract)) != null)
            {
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract)));
            }

            return recordToDelete;
        }


        #endregion

    }
}