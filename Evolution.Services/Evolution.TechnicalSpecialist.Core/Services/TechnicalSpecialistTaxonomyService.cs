using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistTaxonomyInfoService : ITechnicalSpecialistTaxonomyService
    {
        private readonly IAppLogger<TechnicalSpecialistTaxonomyInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistTaxonomyRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistTaxonomyValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly ITaxonomyCategoryService _taxonomyCatService = null;
        private readonly ITaxonomySubCategoryService _taxonomySubCatService = null;
        private readonly ITaxonomyServices _taxonomyServices = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ITechnicalSpecialistTaxonomyHistoryRepository _historyRepository = null; 
        private readonly IUserService _userService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        #region Constructor
        public TechnicalSpecialistTaxonomyInfoService(IMapper mapper,
                                                    ITechnicalSpecialistTaxonomyRepository repository,
                                                    IAppLogger<TechnicalSpecialistTaxonomyInfoService> logger,
                                                    ITechnicalSpecialistTaxonomyValidationService validationService,
                                                    JObject messages, 
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    IMasterService masterService, ITaxonomyCategoryService taxonomyCatService,
                                                    ITaxonomySubCategoryService taxonomySubCatService, ITaxonomyServices taxonomyServices,
                                                    ITechnicalSpecialistTaxonomyHistoryRepository historyRepository,
                                                    IAuditSearchService auditSearchService,
                                                    IUserService userService,
                                                    IEmailQueueService emailService,
                                                    IOptions<AppEnvVariableBaseModel> environment)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            _masterService = masterService;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _taxonomyCatService = taxonomyCatService;
            _taxonomySubCatService = taxonomySubCatService;
            _taxonomyServices = taxonomyServices;
            _historyRepository = historyRepository;
            _auditSearchService = auditSearchService;
            _userService = userService;
            _emailService = emailService;
            _environment = environment.Value;
        }
        #endregion


        #region Public Methods

        #region Get
        public Response Get(TechnicalSpecialistTaxonomyInfo searchModel)
        {
            IList<TechnicalSpecialistTaxonomyInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistTaxonomyInfo>>(_repository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
//D684
        public Response IsTaxonomyHistoryExists(int  epin)
        {
            bool result = false;
            Exception exception = null;
            try
            {
                result = _repository.IsTaxonomyHistoryExists(epin);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), epin);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response Get(IList<int> taxonomyIds)
        {
            IList<TechnicalSpecialistTaxonomyInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistTaxonomyInfo>>(GetTaxonomyInfoById(taxonomyIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), taxonomyIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinId(IList<string> pinIds)
        {
            IList<TechnicalSpecialistTaxonomyInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistTaxonomyInfo>>(GetTaxonomyInfoByPin(pinIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pinIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        #endregion

        #region Add 

        public Response Add(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                             bool commitChange = true,
                             bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomies = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCategories = null;
            IList<DbModel.TaxonomySubCategory> dbSubcategories = null;
            IList<DbModel.TaxonomyService> dbTsServices = null;

            return AddTechSpecialistTaxonomy(tsTaxonomies, ref dbTsTaxonomies, ref dbTechnicalSpecialists, ref dbCategories, ref dbSubcategories, ref dbTsServices, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                        ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCategories,
                        ref IList<DbModel.TaxonomySubCategory> dbSubCategories,
                        ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                        bool commitChange = true,
                        bool isDbValidationRequired = true)
        {
            return AddTechSpecialistTaxonomy(tsTaxonomies, ref dbtsTaxonomies, ref dbTechnicalSpecialists, ref dbCategories, ref dbSubCategories, ref dbTaxonomyService, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                               bool commitChange = true,
                               bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTaxonomy> dbTaxonomies = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCategories = null;
            IList<DbModel.TaxonomySubCategory> dbSubcategories = null;
            IList<DbModel.TaxonomyService> dbTsServices = null;

            return UpdateTechSpecialistTaxonomy(tsTaxonomies, ref dbTaxonomies, ref dbTechnicalSpecialists, ref dbCategories, ref dbSubcategories, ref dbTsServices, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                                ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                ref IList<DbModel.Data> dbCategories,
                                ref IList<DbModel.TaxonomySubCategory> dbSubCategories,
                                ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                                bool commitChange = true,
                                bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistTaxonomy(tsTaxonomies, ref dbtsTaxonomies, ref dbTechnicalSpecialists, ref dbCategories, ref dbSubCategories, ref dbTaxonomyService, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies = null;
            return RemoveTechSpecialistTaxonomy(tsTaxonomies, ref dbtsTaxonomies, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
              ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                     bool commitChange = true,
                     bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistTaxonomy(tsTaxonomies, ref dbtsTaxonomies, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomies = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCategories = null;
            IList<DbModel.TaxonomySubCategory> dbSubcategories = null;
            IList<DbModel.TaxonomyService> dbTsServices = null;
            return IsRecordValidForProcess(tsTaxonomies, validationType, ref dbTechnicalSpecialists, ref dbCategories, ref dbSubcategories, ref dbTsServices, ref dbTsTaxonomies);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies, ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCategories,
                                                ref IList<DbModel.TaxonomySubCategory> dbSubCategories,
                                                ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                                                ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomies,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistTaxonomyInfo> filteredTSTaxonomies = null;

            return CheckRecordValidForProcess(tsTaxonomies, validationType, ref filteredTSTaxonomies, ref dbTsTaxonomies,
                                                   ref dbTechnicalSpecialists, ref dbCategories, ref dbSubCategories, ref dbTaxonomyService, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomies)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCategories = null;
            IList<DbModel.TaxonomySubCategory> dbSubcategories = null;
            IList<DbModel.TaxonomyService> dbTsServices = null;
            return IsRecordValidForProcess(tsTaxonomies, validationType, ref dbTechnicalSpecialists, ref dbCategories, ref dbSubcategories, ref dbTsServices, ref dbTsTaxonomies);
        }

        public Response IsRecordExistInDb(IList<int> tsTaxonomiesIds,
                                        ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomies,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsTaxonomyIdNotExists = null;
            return IsRecordExistInDb(tsTaxonomiesIds, ref dbTsTaxonomies, ref tsTaxonomyIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsTaxonomiesIds,
                                          ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomies,
                                          ref IList<int> tsTaxonomyIdNotExists,
                                          ref IList<ValidationMessage> validationMessages)
        {

            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsTaxonomies == null && tsTaxonomiesIds?.Count > 0)
                    dbTsTaxonomies = GetTaxonomyInfoById(tsTaxonomiesIds);

                result = IsTSTaxonomyExistInDb(tsTaxonomiesIds, dbTsTaxonomies, ref tsTaxonomyIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTaxonomiesIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #endregion


        #region Private Methods

        #region Get
        private IList<DbModel.TechnicalSpecialistTaxonomy> GetTaxonomyInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomyInfos = null;
            if (pins?.Count > 0)
            {
                dbTsTaxonomyInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsTaxonomyInfos;
        }

        private IList<DbModel.TechnicalSpecialistTaxonomy> GetTaxonomyInfoById(IList<int> tsTaxonomyIds)
        {
            IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomyInfos = null;
            if (tsTaxonomyIds?.Count > 0)
                dbTsTaxonomyInfos = _repository.FindBy(x => tsTaxonomyIds.Contains((int)x.Id)).ToList();

            return dbTsTaxonomyInfos;
        }
        #endregion

        private Response AddTechSpecialistTaxonomy(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                                   ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                   ref IList<DbModel.Data> dbCategory,
                                                   ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                                    ref IList<DbModel.TaxonomyService> dbService,
                                                   bool commitChange = true,
                                                   bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
           
            try
            {
                eventId = tsTaxonomy?.FirstOrDefault().EventId;
                Response valdResponse = null;
                IList<TechnicalSpecialistTaxonomyInfo> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                IList<DbModel.Data> dbCategories = null;
                IList<DbModel.TaxonomySubCategory> dbSubCategories = null;
                IList<DbModel.TaxonomyService> dbServices = null;

                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsTaxonomy, ValidationType.Add, ref recordToBeAdd,
                                                              ref dbTsTaxonomy, ref dbTechnicalSpecialists, ref dbCategory,
                                                              ref dbSubCategory, ref dbService);
                }

                if (!isDbValidationRequired && tsTaxonomy?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsTaxonomy, ValidationType.Add);
                }

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || Convert.ToBoolean(valdResponse?.Result)))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbCategories = dbCategory;
                    dbSubCategories = dbSubCategory;
                    dbServices = dbService;

                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    _repository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistTaxonomy>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                        opt.Items["DBCategories"] = dbCategories;
                        opt.Items["DBSubCategories"] = dbSubCategories;
                        opt.Items["DBServices"] = dbServices;

                    });
                    _repository.Add(mappedRecords);
                    if (commitChange)
                    {
                      int value=  _repository.ForceSave();
                        dbTsTaxonomy = mappedRecords;
                        if (mappedRecords?.Count > 0 && value>0)
                        { 
                            ProcessEmailNotifications(dbTechSpecialists.FirstOrDefault(x1 => x1.Id == mappedRecords?.FirstOrDefault()?.TechnicalSpecialistId), EmailTemplate.EmailTmApproval, null);
                            
                            mappedRecords?.ToList().ForEach(x =>
                            { 
                                  mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsTaxonomy?.FirstOrDefault()?.ActionByUser,
                                                                                                    null,
                                                                                                    ValidationType.Add.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.TechnicalSpecialistTaxonomy,
                                                                                                    null,
                                                                                                    _mapper.Map<TechnicalSpecialistTaxonomyInfo>(x1)));
                            });
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTaxonomy);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistTaxonomy(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                                               ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                               ref IList<DbModel.Data> dbCategory,
                                                               ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                                               ref IList<DbModel.TaxonomyService> dbService,
                                                               bool commitChange = true,
                                                               bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbCategories = null;
            IList<DbModel.TaxonomySubCategory> dbSubCategories = null;
            IList<DbModel.TaxonomyService> dbServices = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistTaxonomyInfo> recordToBeModify = null;
             long? eventId = 0; 
            bool valdResult = true;
            try
            {
                eventId = tsTaxonomy?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsTaxonomy, ValidationType.Update, ref recordToBeModify, ref dbTsTaxonomy, ref dbTechnicalSpecialists, ref dbCategory, ref dbSubCategory, ref dbService);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (!isDbValidationRequired && tsTaxonomy?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsTaxonomy, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {

                    if ((dbTsTaxonomy == null || (dbTsTaxonomy?.Count <= 0 && !valdResult)))
                    {
                        dbTsTaxonomy = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if ((dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult)))
                    {
                        //valdResult = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if ((dbCategory == null || (dbCategory?.Count <= 0 &&! valdResult)))
                    {
                        valdResult = _taxonomyCatService.IsValidCategoryName(recordToBeModify.Select(x => x.TaxonomyCategoryName).ToList(), ref dbCategory, ref validationMessages);
                    }

                    if ((dbSubCategory == null || (dbSubCategory?.Count <= 0 && !valdResult)))
                    {
                        var tsCatAndSubCat = recordToBeModify.Select(x => new KeyValuePair<string, string>(x.TaxonomyCategoryName.ToString(), x.TaxonomySubCategoryName))
                                                          .Distinct()
                                                          .ToList();
                        valdResult = _taxonomySubCatService.IsValidSubCategoryName(tsCatAndSubCat, ref dbSubCategory, ref validationMessages);
                    }
                    if ((dbService == null || (dbService?.Count <= 0 && !valdResult)))
                    {
                        var tsSubcatAndServices = recordToBeModify.Select(x => new KeyValuePair<string, string>(x.TaxonomySubCategoryName.ToString(), x.TaxonomyServices))
                                                          .Distinct()
                                                          .ToList();
                        valdResult = _taxonomyServices.IsValidServiceName(tsSubcatAndServices, ref dbService, ref validationMessages);
                    }


                    if (valdResult && dbTsTaxonomy?.Count > 0)
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbCategories = dbCategory;
                        dbSubCategories = dbSubCategory;
                        dbServices = dbService;
                        IList<DbModel.TechnicalSpecialistTaxonomyHistory> dbTsTaxonomyHistory = new List<DbModel.TechnicalSpecialistTaxonomyHistory>();
                        IList<TechnicalSpecialistTaxonomyInfo> domExsistanceTaxonomyInfo = new List<TechnicalSpecialistTaxonomyInfo>();
                       
                        var taxonomyIds = dbTsTaxonomy.Select(x => x.Id).Distinct().ToList();
                        var dbTaxonomyHistory = _historyRepository.FindBy(x => taxonomyIds.Contains((int)x.TaxonomyId)).ToList();
                        IList<DbModel.TechnicalSpecialistTaxonomy> dbTaxonomyToDelete = new List<DbModel.TechnicalSpecialistTaxonomy>();
                        IList<DbModel.TechnicalSpecialistTaxonomyHistory> dbTaxonomyHistoryToDelete = new List<DbModel.TechnicalSpecialistTaxonomyHistory>();
                        var taxonmyStatusReject = true;
                        dbTsTaxonomy.ToList().ForEach(tsTaxonomyInfo =>
                        {
                            domExsistanceTaxonomyInfo.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistTaxonomyInfo>(tsTaxonomyInfo)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsTaxonomyInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsTaxonomyInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                    opt.Items["DBCategories"] = dbCategories;
                                    opt.Items["DBSubCategories"] = dbSubCategories;
                                    opt.Items["DBServices"] = dbServices;

                                });
                                tsTaxonomyInfo.LastModification = DateTime.UtcNow;
                                tsTaxonomyInfo.UpdateCount = tsTaxonomyInfo.UpdateCount.CalculateUpdateCount();
                                tsTaxonomyInfo.ModifiedBy = tsToBeModify.ModifiedBy;

                                ///RC/RM Taxonomy Accept / Reject Functionality start
                                if (tsToBeModify.TaxonomyStatus == "Accept")
                                {
                                    /** Commented for D954 (Ref Francina mail on 17-04-2020 (Pending Items from ITK- Taxonomy Query)) Start */
                                    //if (tsToBeModify.ApprovalStatus == "Reject")
                                    //{
                                    //    dbTaxonomyToDelete.Add(tsTaxonomyInfo);
                                    //    var taxonomyHistoryToDelete = dbTaxonomyHistory?.Where(x => x.TaxonomyId == tsTaxonomyInfo.Id).ToList();
                                    //    dbTaxonomyHistoryToDelete.AddRange(taxonomyHistoryToDelete);
                                    //}
                                    //else
                                    //{
                                    //    dbTsTaxonomyHistory.Add(_mapper.Map<DbModel.TechnicalSpecialistTaxonomyHistory>(tsTaxonomyInfo));
                                    // }
                                    /** End */
                                    dbTsTaxonomyHistory.Add(_mapper.Map<DbModel.TechnicalSpecialistTaxonomyHistory>(tsTaxonomyInfo));
                                }
                                else if (tsToBeModify.TaxonomyStatus == "Reject")
                                {
                                    int? lastUpdatedTaxonomyHistoryId = null;
                                    var historyTaxonomy = dbTaxonomyHistory?.Where(x => x.TaxonomyId == tsTaxonomyInfo.Id).ToList();
                                    if (historyTaxonomy != null && historyTaxonomy.Count > 0)
                                        lastUpdatedTaxonomyHistoryId = historyTaxonomy?.Max(x => x.HistoryId);
                                    var lastApprovedRecord = dbTaxonomyHistory?.FirstOrDefault(x1 => (x1.HistoryId == lastUpdatedTaxonomyHistoryId));
                                    if (lastApprovedRecord != null)
                                    {
                                        tsTaxonomyInfo.ApprovalStatus = lastApprovedRecord.ApprovalStatus;
                                        tsTaxonomyInfo.Comments = lastApprovedRecord.Comments;
                                        tsTaxonomyInfo.DisplayOrder = lastApprovedRecord.DisplayOrder;
                                        tsTaxonomyInfo.FromDate = lastApprovedRecord.FromDate;
                                        tsTaxonomyInfo.LastModification = lastApprovedRecord.LastModification;
                                        tsTaxonomyInfo.TaxonomyCategoryId = (int)lastApprovedRecord.TaxonomyCategoryId;
                                        tsTaxonomyInfo.TaxonomyServicesId = (int)lastApprovedRecord.TaxonomyServicesId;
                                        tsTaxonomyInfo.TaxonomySubCategoryId = (int)lastApprovedRecord.TaxonomySubCategoryId;
                                        tsTaxonomyInfo.TechnicalSpecialistId = (int)lastApprovedRecord.TechnicalSpecialistId;
                                        tsTaxonomyInfo.ToDate = lastApprovedRecord.ToDate;
                                        tsTaxonomyInfo.Interview = lastApprovedRecord.Interview;
                                        tsTaxonomyInfo.TaxonomyStatus = "Accept";
                                        tsTaxonomyInfo.ApprovedBy = lastApprovedRecord.ApprovedBy;
                                        _repository.Update(tsTaxonomyInfo);
                                        taxonmyStatusReject = false;
                                    }
                                    else
                                    {
                                        dbTaxonomyToDelete.Add(tsTaxonomyInfo);
                                    }

                                }
                            } 
                        });

                        _repository.AutoSave = false;
                        _historyRepository.AutoSave = false;
                        ///RC/RM Taxonomy Accept / Reject Functionality End
                        if (taxonmyStatusReject == true)
                            _repository.Update(dbTsTaxonomy);

                        if (dbTaxonomyToDelete?.Count > 0)
                            _repository.Delete(dbTaxonomyToDelete);


                        if (commitChange)
                        {
                           int value =  _repository.ForceSave();


                            if (dbTsTaxonomyHistory?.Count > 0)
                                _historyRepository.Add(dbTsTaxonomyHistory);

                            if (dbTaxonomyHistoryToDelete?.Count > 0)
                                _historyRepository.Delete(dbTaxonomyHistoryToDelete);

                            _historyRepository.ForceSave();

                            if (recordToBeModify?.Count > 0 && value>0)
                            {
                                if (taxonmyStatusReject == true && recordToBeModify.Any(x=>x.TaxonomyStatus == "IsAcceptRequired"))//def 978
                                {
                                    ProcessEmailNotifications(dbTechSpecialists.FirstOrDefault(x1 => x1.Pin == recordToBeModify?.FirstOrDefault()?.Epin), EmailTemplate.EmailTmApproval, null);
                                }
                                 
                                recordToBeModify?.ToList().ForEach(x =>
                                {
                                    recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                           null,
                                                                                                           ValidationType.Update.ToAuditActionType(),
                                                                                                           SqlAuditModuleType.TechnicalSpecialistTaxonomy,
                                                                                                           domExsistanceTaxonomyInfo?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                                                            x1
                                                                                                          ));
                                });
                            }
                       
                        }

                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTaxonomy);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response RemoveTechSpecialistTaxonomy(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
             ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                                     bool commitChange = true,
                                                     bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbCategories = null;
            IList<DbModel.TaxonomySubCategory> dbSubCategories = null;
            IList<DbModel.TaxonomyService> dbServices = null;
            IList<TechnicalSpecialistTaxonomyInfo> recordToBeDeleted = null;
            long? eventId = 0;
            
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsTaxonomy?.FirstOrDefault().EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsTaxonomy, ValidationType.Delete, ref dbTechSpecialists, ref dbCategories, ref dbSubCategories, ref dbServices, ref dbTsTaxonomy);

                if (tsTaxonomy?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsTaxonomy, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsTaxonomy?.Count > 0)
                {
                    var dbTsTaxonomyToBeDeleted = dbTsTaxonomy?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsTaxonomyToBeDeleted);
                    if (commitChange)
                    {
                       int value= _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0 && value>0)
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                    null,
                                                                                                   ValidationType.Delete.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.TechnicalSpecialistTaxonomy,
                                                                                                      x1,
                                                                                                      null
                                                                                                      ));
                        }
                    }

                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTaxonomy);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                                   ValidationType validationType,
                                                   ref IList<TechnicalSpecialistTaxonomyInfo> filteredTsTaxonomy,
                                                   ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                                   ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                   ref IList<DbModel.Data> dbCategory,
                                                   ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                                   ref IList<DbModel.TaxonomyService> dbService,
                                                   bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsTaxonomy, ref filteredTsTaxonomy, ref dbTsTaxonomy, ref dbTechnicalSpecialists, ref dbCategory, ref dbSubCategory, ref dbService, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsTaxonomy, ref filteredTsTaxonomy, ref dbTsTaxonomy, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsTaxonomy, ref filteredTsTaxonomy, ref dbTsTaxonomy, ref dbTechnicalSpecialists, ref dbCategory, ref dbSubCategory, ref dbService, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTaxonomy);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                        ref IList<TechnicalSpecialistTaxonomyInfo> filteredTsTaxonomy,
                                        ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Data> dbCategory,
                                        ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                        ref IList<DbModel.TaxonomyService> dbService,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsTaxonomy != null && tsTaxonomy.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsTaxonomy == null || filteredTsTaxonomy.Count <= 0)
                    filteredTsTaxonomy = FilterRecord(tsTaxonomy, validationType);

                if (filteredTsTaxonomy?.Count > 0 && IsValidPayload(filteredTsTaxonomy, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsTaxonomy.Select(x => x.Epin.ToString()).ToList();
                    IList<string> categories = filteredTsTaxonomy.Select(x => x.TaxonomyCategoryName).ToList();
                    IList<string> subcategories = filteredTsTaxonomy.Select(x => x.TaxonomySubCategoryName).ToList();
                    IList<string> services = filteredTsTaxonomy.Select(x => x.TaxonomyServices).ToList();

                    //IList<DbModel.Data> dbMaster = null;
                    if (dbCategory == null || dbSubCategory == null || dbService == null) //TODO : Temp Solution.
                        GetMasterData(filteredTsTaxonomy, ref dbCategory, ref dbSubCategory, ref dbService);

                    if (tsEpin?.Count > 0)
                    {
                        //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    }
                    if (result && categories?.Count > 0)
                        result = _taxonomyCatService.IsValidCategoryName(categories, ref dbCategory, ref validationMessages);
                    if (result && subcategories?.Count > 0)
                    {
                        var tsCatAndSubCat = filteredTsTaxonomy.Select(x => new KeyValuePair<string, string>(x.TaxonomyCategoryName.ToString(), x.TaxonomySubCategoryName))
                                                     .Distinct()
                                                     .ToList();
                        result = _taxonomySubCatService.IsValidSubCategoryName(tsCatAndSubCat, ref dbSubCategory, ref validationMessages);
                    }
                    if (result && services?.Count > 0)
                    {
                        var tsSubcatAndServices = filteredTsTaxonomy.Select(x => new KeyValuePair<string, string>(x.TaxonomySubCategoryName.ToString(), x.TaxonomyServices))
                                                        .Distinct()
                                                        .ToList();
                        result = _taxonomyServices.IsValidServiceName(tsSubcatAndServices, ref dbService, ref validationMessages);
                    }
                    if (result)
                        result = IsTSTaxonomyUnique(filteredTsTaxonomy, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                         ref IList<TechnicalSpecialistTaxonomyInfo> filteredTsTaxonomy,
                                         ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbCategory,
                                         ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                         ref IList<DbModel.TaxonomyService> dbService,
                                         ref IList<ValidationMessage> validationMessages,
                                          bool isDraft = false)
        {
            bool result = false;
            if (tsTaxonomy != null && tsTaxonomy.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsTaxonomy == null || filteredTsTaxonomy.Count <= 0)
                    filteredTsTaxonomy = FilterRecord(tsTaxonomy, validationType);

                if (filteredTsTaxonomy?.Count > 0 && IsValidPayload(filteredTsTaxonomy, validationType, ref messages))
                {
                    GetTsTaxonomyDbInfo(filteredTsTaxonomy, ref dbTsTaxonomy);
                    IList<int> tsTaxonomyIds = filteredTsTaxonomy.Select(x => x.Id).ToList();
                    IList<int> tsDbTaxonomyIds = dbTsTaxonomy.Select(x => x.Id).ToList();
                    if (tsTaxonomyIds.Any(x=> !tsDbTaxonomyIds.Contains(x))) //Invalid TechSpecialist  Id found.
                    {
                        var dbTsInfosByIds = dbTsTaxonomy;
                        var idNotExists = tsTaxonomyIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistTaxonomyList = filteredTsTaxonomy;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsTaxonomyID = techSpecialistTaxonomyList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsTaxonomyID, MessageType.InvalidTaxonomyInfo);
                        });
                    }
                    else
                    {
                        result = isDraft? true : IsRecordUpdateCountMatching(filteredTsTaxonomy, dbTsTaxonomy, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsTaxonomy.Select(x => x.Epin.ToString()).ToList();
                            IList<string> cats = filteredTsTaxonomy.Select(x => x.TaxonomyCategoryName).ToList();
                            IList<string> subcats = filteredTsTaxonomy.Select(x => x.TaxonomySubCategoryName).ToList();
                            IList<string> servs = filteredTsTaxonomy.Select(x => x.TaxonomyServices).ToList();

                            if (tsEpin?.Count > 0)
                            {
                                //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                                result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            }
                            if (result && cats?.Count > 0)
                                result = _taxonomyCatService.IsValidCategoryName(cats, ref dbCategory, ref validationMessages);
                            if (result && subcats?.Count > 0)
                            {
                                var tsCatAndSubCat = filteredTsTaxonomy.Select(x => new KeyValuePair<string, string>(x.TaxonomyCategoryName.ToString(), x.TaxonomySubCategoryName))
                                                       .Distinct()
                                                       .ToList();
                                result = _taxonomySubCatService.IsValidSubCategoryName(tsCatAndSubCat, ref dbSubCategory, ref validationMessages);
                            }
                            if (result && servs?.Count > 0)
                            {
                                var tsSubcatAndServices = filteredTsTaxonomy.Select(x => new KeyValuePair<string, string>(x.TaxonomySubCategoryName.ToString(), x.TaxonomyServices))
                                                      .Distinct()
                                                      .ToList();
                                result = _taxonomyServices.IsValidServiceName(tsSubcatAndServices, ref dbService, ref validationMessages);
                            }
                            if (result)
                                result = IsTSTaxonomyUnique(filteredTsTaxonomy, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                           ref IList<TechnicalSpecialistTaxonomyInfo> filteredTsTaxonomy,
                                           ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsTaxonomy != null && tsTaxonomy.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsTaxonomy == null || filteredTsTaxonomy.Count <= 0)
                    filteredTsTaxonomy = FilterRecord(tsTaxonomy, validationType);

                if (filteredTsTaxonomy?.Count > 0 && IsValidPayload(filteredTsTaxonomy, validationType, ref validationMessages))
                {
                    GetTsTaxonomyDbInfo(filteredTsTaxonomy, ref dbTsTaxonomy);
                    IList<int> tsTaxonomyIdNotExists = null;
                    var tsTaxonomyIds = filteredTsTaxonomy.Select(x => x.Id).Distinct().ToList();
                    result = IsTSTaxonomyExistInDb(tsTaxonomyIds, dbTsTaxonomy, ref tsTaxonomyIdNotExists, ref validationMessages);

                }
            }
            return result;
        }
        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                                              IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                              ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = tsTaxonomy.Where(x => !dbTsTaxonomy.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.TaxonomyUpdatedByOtherUser, x.TaxonomyCategoryName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private IList<TechnicalSpecialistTaxonomyInfo> FilterRecord(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy, ValidationType filterType)
        {
            IList<TechnicalSpecialistTaxonomyInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsTaxonomy?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsTaxonomy?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsTaxonomy?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }
        private bool IsValidPayload(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomy,
                           ValidationType validationType,
                           ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsTaxonomy), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private void GetTsTaxonomyDbInfo(IList<TechnicalSpecialistTaxonomyInfo> filteredTsTaxonomy,
                                         ref IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy)
        {
            dbTsTaxonomy = dbTsTaxonomy ?? new List<DbModel.TechnicalSpecialistTaxonomy>();
            IList<int> tsTaxonomyIds = filteredTsTaxonomy?.Select(x => x.Id).Distinct().ToList();
            if (tsTaxonomyIds?.Count > 0 && ( dbTsTaxonomy.Count <= 0 || dbTsTaxonomy.Any(x=> !tsTaxonomyIds.Contains(x.Id))))
            {
                var tsTaxonomies = GetTaxonomyInfoById(tsTaxonomyIds);
                if (tsTaxonomies != null && tsTaxonomies.Any())
                {
                    dbTsTaxonomy.AddRange(tsTaxonomies);
                }
            }    
        }

        private bool IsTSTaxonomyExistInDb(IList<int> tsTaxonomyIds,
                                              IList<DbModel.TechnicalSpecialistTaxonomy> dbTsTaxonomy,
                                              ref IList<int> tsTaxonomyIdNotExists,
                                              ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsTaxonomy == null)
                dbTsTaxonomy = new List<DbModel.TechnicalSpecialistTaxonomy>();

            var validMessages = validationMessages;

            if (tsTaxonomyIds?.Count > 0)
            {
                tsTaxonomyIdNotExists = tsTaxonomyIds.Where(id => !dbTsTaxonomy.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsTaxonomyIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidTaxonomyInfo, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
        private bool IsTSTaxonomyUnique(IList<TechnicalSpecialistTaxonomyInfo> filteredTsTaxonomy,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var tsTaxonomy = filteredTsTaxonomy.Select(x => new { x.Epin, x.TaxonomyCategoryName, x.TaxonomySubCategoryName, x.TaxonomyServices, x.Id });
            var dbTsTaxnomy = _repository.FindBy(x => tsTaxonomy.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.TaxonomyCategoryName == x.TaxonomyCategory.Name && x1.TaxonomySubCategoryName == x.TaxonomySubCategory.TaxonomySubCategoryName && x1.TaxonomyServices == x.TaxonomyServices.TaxonomyServiceName && x1.Id != x.Id)).ToList();
            if (dbTsTaxnomy?.Count > 0)
            {
                var tsTaxonomyAlreadyExist = filteredTsTaxonomy.Where(x => dbTsTaxnomy.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin && x.TaxonomyCategoryName == x1.TaxonomyCategory.Name && x.TaxonomySubCategoryName == x1.TaxonomySubCategory.TaxonomySubCategoryName && x1.TaxonomyServices.TaxonomyServiceName == x.TaxonomyServices));
                tsTaxonomyAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.TsTaxonomyAlreadyExist, x.TaxonomyServices,x.Epin, x.TaxonomyCategoryName, x.TaxonomySubCategoryName); //D943(ref by ALM Doc 13-03-2020)
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private IList<DbModel.Data> GetMasterData(IList<TechnicalSpecialistTaxonomyInfo> tsInfos,
                                              ref IList<DbModel.Data> dbCategory,
                                              ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                              ref IList<DbModel.TaxonomyService> dbService)

        {
            IList<string> categories = tsInfos.Select(x => x.TaxonomyCategoryName).ToList();
            var masterNames = categories.ToList();
            var masterTypes = new List<MasterType>() { MasterType.TaxonomyCategory };
            var dbMaster = _masterService.Get(masterTypes, null, masterNames, null, tsx => tsx.TaxonomySubCategory);
            if (dbMaster?.Count > 0)
            {
                dbCategory = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.TaxonomyCategory).ToList();
                dbSubCategory = dbCategory.SelectMany(x => x.TaxonomySubCategory).ToList();
                dbService = dbSubCategory.SelectMany(x => x.TaxonomyService).ToList();//TODO : NEED to check
            }
            return dbMaster;
        }

        private Response ProcessEmailNotifications(DbModel.TechnicalSpecialist technicalSpecialistInfo, EmailTemplate emailTemplateType, List<KeyValuePair<string, string>> emailContentPlaceholders)
        {
            string emailSubject = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = null;
            List<EmailAddress> fromAddresses = null;
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            try
            {
                emailContentPlaceholders = emailContentPlaceholders ?? new List<KeyValuePair<string, string>>();
                if (technicalSpecialistInfo != null)
                {
                    switch (emailTemplateType)
                    {
                        case EmailTemplate.EmailTmApproval:
                            var userInfos = _userService.Get(_environment.SecurityAppName, new List<string> {
                                                                technicalSpecialistInfo?.AssignedToUser,
                                                                technicalSpecialistInfo?.AssignedByUser
                                                            }).Result.Populate<IList<UserInfo>>();
                            if(userInfos!=null && userInfos.Any())
                            {
                                emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_TmApproval_Subject, technicalSpecialistInfo?.Pin.ToString());
                                fromAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x?.LogonName == technicalSpecialistInfo?.AssignedByUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x?.LogonName == technicalSpecialistInfo?.AssignedByUser)?.UserName } };
                                toAddresses = new List<EmailAddress> { new EmailAddress { Address = userInfos?.FirstOrDefault(x => x?.LogonName == technicalSpecialistInfo?.AssignedToUser)?.Email, DisplayName = userInfos?.FirstOrDefault(x => x?.LogonName == technicalSpecialistInfo?.AssignedToUser)?.UserName } };
                                emailContentPlaceholders.AddRange(new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, technicalSpecialistInfo?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, technicalSpecialistInfo?.LastName),
                                     new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, technicalSpecialistInfo?.Pin.ToString()),
                                });
                                emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.TMA, ModuleCodeType.TS, technicalSpecialistInfo?.Pin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses);
                            } 
                            break;

                    }

                    return _emailService.Add(new List<EmailQueueMessage> { emailMessage });

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString())).ToList();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;
            var dbTechSpecs = dbTsInfos;

            if (tsPins?.Count > 0)
            {
                IList<string> tsPinNotExists = tsPins.Where(pin => !dbTechSpecs.Any(x1 => x1.Pin.ToString() == pin))
                                        .Select(pin => pin)
                                        .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
        #endregion


    }
}
