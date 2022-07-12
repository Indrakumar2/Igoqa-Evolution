using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Models.Assignments;
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
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Evolution.ResourceSearch.Domain.Interfaces.Validations;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Core.Services
{
    public class ResourceSearchService : IResourceSearchService
    {
        private readonly IResourceSearchRepository _resourceSearchRepository = null;
        private readonly ICustomerService _customerService = null;
        private readonly ICompanyService _companyService = null;
        private readonly IUserService _userService = null;
        private readonly IAppLogger<ResourceSearchService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMapper _mapper = null;
        private readonly IResourceSearchValidation _validationService = null;
        private readonly IMasterService _masterService = null;
        private readonly ITaxonomyCategoryService _taxonomyCatService = null;
        private readonly ITaxonomySubCategoryService _taxonomySubCatService = null;
        private readonly ITaxonomyServices _taxonomyServices = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IResourceSearchNoteService _resourceSearchNoteService = null;
        private readonly IOverrideResourceService _overrideResourceService = null;
        private readonly IAssignmentTechnicalSpecilaistService _assignmentTechnicalSpecilaistService = null;
        private readonly IAssignmentSubSupplerTSRepository _assignmentSubSupplerTSRepository = null;
        private readonly IAuditLogger _auditLogger = null;
        private ITechnicalSpecialistCalendarService _technicalSpecialistCalendarService = null;
        private IVisitTechnicalSpecialistsAccountRepository _visitTechSpecRepository = null;
        private ITimesheetTechnicalSpecialistRepository _timesheetTechSpecRepository = null;

        public ResourceSearchService(IResourceSearchRepository resourceSearchRepository,
                                    IAssignmentRepository assignmentRepository,
                                    JObject messages,
                                    IAppLogger<ResourceSearchService> logger,
                                    IMapper mapper,
                                    IResourceSearchValidation validationService,
                                    IUserService userService,
                                    ICustomerService customerService,
                                    ICompanyService companyService,
                                    IMasterService masterService,
                                    ITaxonomyCategoryService taxonomyCatService,
                                    ITaxonomySubCategoryService taxonomySubCatService,
                                    ITaxonomyServices taxonomyServices,
                                    IEmailQueueService emailService,
                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                    IMyTaskService myTaskService,
                                    IResourceSearchNoteService resourceSearchNoteService,
                                    IOverrideResourceService overrideResourceService,
                                    IAssignmentTechnicalSpecilaistService assignmentTechnicalSpecilaistService,
                                    ITechnicalSpecialistCalendarService technicalSpecialistCalendarService,
                                    IAssignmentSubSupplerTSRepository assignmentSubSupplerTSRepository,
                                    IVisitTechnicalSpecialistsAccountRepository visitTechSpecRepository,
                                    ITimesheetTechnicalSpecialistRepository timesheetTechSpecRepository,
                                    IOptions<AppEnvVariableBaseModel> environment,
                                    IAuditLogger auditLogger)

        {
            _resourceSearchRepository = resourceSearchRepository;
            _assignmentRepository = assignmentRepository;
            _logger = logger;
            _mapper = mapper;
            _masterService = masterService;
            _validationService = validationService;
            _userService = userService;
            _taxonomyCatService = taxonomyCatService;
            _taxonomySubCatService = taxonomySubCatService;
            _taxonomyServices = taxonomyServices;
            _customerService = customerService;
            _companyService = companyService;
            _emailService = emailService;
            _messageDescriptions = messages;
            _environment = environment.Value;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _myTaskService = myTaskService;
            _resourceSearchNoteService = resourceSearchNoteService;
            _overrideResourceService = overrideResourceService;
            _assignmentTechnicalSpecilaistService = assignmentTechnicalSpecilaistService;
            _technicalSpecialistCalendarService = technicalSpecialistCalendarService;
            _assignmentSubSupplerTSRepository = assignmentSubSupplerTSRepository;
            _visitTechSpecRepository = visitTechSpecRepository;
            _timesheetTechSpecRepository = timesheetTechSpecRepository;
            _auditLogger = auditLogger;
        }

        #region  Public Methods

        public Response Delete(int resourceId, bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;

            try
            {
                _resourceSearchRepository.AutoSave = false;
                IList<DbModel.ResourceSearch> dbResourceSearch = null;

                if (IsValidResouceService(new List<int?> { resourceId }, ref dbResourceSearch, ref validationMessages))
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        _resourceSearchRepository.Delete(dbResourceSearch);

                        if (commitChange)
                        {
                            _resourceSearchRepository.ForceSave();
                            tranScope.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceId);
            }
            finally
            {
                _resourceSearchRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), null, exception);
        }

        public Response Get(DomainModel.BaseResourceSearch searchModel)
        {
            IList<DomainModel.ResourceSearchResult> result = null;
            Exception exception = null;

            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _resourceSearchRepository.Search(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);

        }

        public Response Get(IList<KeyValuePair<string, IList<string>>> mySearch, string assignedTo, string companyCode, bool isAllCoordinator)
        {
            IList<DomainModel.ResourceSearchResult> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _resourceSearchRepository.Search(mySearch, assignedTo, companyCode, isAllCoordinator);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), mySearch);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Modify(DomainModel.ResourceSearch resource, bool commitChange = true)
        {

            IList<DbModel.ResourceSearch> dbResourceSearch = null;
            return Modify(resource, ref dbResourceSearch, commitChange);
        }

        public Response Modify(DomainModel.ResourceSearch resource, ref IList<DbModel.ResourceSearch> dbResourceSearch, bool commitChange = true)
        {

            IList<DbModel.Customer> dbCustomers = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.User> dbCoordinators = null;
            IList<DbModel.Data> dbCategory = null;
            IList<DbModel.TaxonomySubCategory> dbSubCategory = null;
            IList<DbModel.TaxonomyService> dbService = null;
            IList<DbModel.OverrideResource> dbOverrideResources = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            DbModel.Assignment dbAssignment = null;
            return Modify(resource, ref dbResourceSearch, ref dbCustomers, ref dbCompanies, ref dbCoordinators, ref dbCategory, ref dbSubCategory, ref dbService, ref dbOverrideResources, ref dbTechnicalSpecialist, ref dbAssignment, commitChange);
        }

        public Response Modify(DomainModel.ResourceSearch resource,
                                ref IList<DbModel.ResourceSearch> dbResourceSearch,
                                ref IList<DbModel.Customer> dbCustomers,
                                ref IList<DbModel.Company> dbCompanies,
                                ref IList<DbModel.User> dbCoordinators,
                                ref IList<DbModel.Data> dbCategory,
                                ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                ref IList<DbModel.TaxonomyService> dbService,
                                ref IList<DbModel.OverrideResource> dbOverrideResources,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                ref DbModel.Assignment dbAssignment,
                                bool isDBValidationRequire = true,
                                bool commitChange = true,
                                IList<DbModel.SqlauditModule> dbModule = null)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            int? resourceSearchId = null;
            Response response = null;
            long? eventId = null;

            try
            {
                IList<DbModel.Customer> dbCustomersTemp = dbCustomers;
                IList<DbModel.Company> dbCompaniesTemp = dbCompanies;
                IList<DbModel.Data> dbCategoryTemp = dbCategory;
                IList<DbModel.TaxonomySubCategory> dbSubCategoryTemp = dbSubCategory;
                IList<DbModel.TaxonomyService> dbServiceTemp = dbService;


                if (resource != null)
                {
                    if (resource.RecordStatus.IsRecordStatusModified() && CheckRecordValidForProcess(resource, ValidationType.Update, ref dbResourceSearch, ref dbCoordinators, ref dbCompanies, ref dbCustomers, ref dbCategory, ref dbSubCategory, ref dbService, ref dbOverrideResources, ref dbAssignment, ref dbTechnicalSpecialist, ref validationMessages, isDBValidationRequire))
                    {
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            IList<DomainModel.ResourceSearch> domResourceSearch = new List<DomainModel.ResourceSearch>();
                            if (dbResourceSearch?.Count > 0)
                            {
                                _resourceSearchRepository.AutoSave = false;
                                AssigneSearchToUser(ref resource);

                                dbResourceSearch.ToList().ForEach(x =>
                                {
                                    domResourceSearch.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.ResourceSearch>(x)));
                                });
                                dbResourceSearch.ToList().ForEach(dbRes =>
                                {
                                    if (resource != null && dbRes.Id == resource?.Id)
                                    {
                                        _mapper.Map(resource, dbRes, opt =>
                                        {
                                            opt.Items["isAssignId"] = true;
                                            opt.Items["serializationType"] = SerializationType.JSON;
                                            opt.Items["dbCustomer"] = dbCustomersTemp;
                                            opt.Items["dbCompany"] = dbCompaniesTemp;
                                            opt.Items["dbCategory"] = dbCategoryTemp;
                                            opt.Items["dbSubCategory"] = dbSubCategoryTemp;
                                            opt.Items["dbService"] = dbServiceTemp;
                                        });
                                        dbRes.LastModification = DateTime.UtcNow;
                                        dbRes.UpdateCount = resource?.UpdateCount.CalculateUpdateCount();
                                        dbRes.ModifiedBy = resource?.ModifiedBy;
                                    }
                                });

                                _resourceSearchRepository.Update(dbResourceSearch);
                            }
                            if (commitChange)
                            {
                                int value = _resourceSearchRepository.ForceSave();
                                if (value > 0)
                                {
                                    resourceSearchId = dbResourceSearch?.FirstOrDefault()?.Id;
                                    if (value > 0)
                                    {
                                        domResourceSearch?.ToList().ForEach(x => AuditLog(resource,
                                                                                          ValidationType.Update.ToAuditActionType(),
                                                                                          SqlAuditModuleType.ResourceSearch,
                                                                                          domResourceSearch?.FirstOrDefault(x1 => x1.MyTaskId == x.MyTaskId),
                                                                                          resource,
                                                                                          resourceSearchId.ToString(),
                                                                                          ref eventId,
                                                                                          dbModule,
                                                                                          "ResourceSearch"
                                                                                          ));
                                        if (eventId != null && eventId > 0)
                                            ObjectExtension.SetPropertyValue(resource, "EventId", eventId);
                                    }

                                    if (resource?.OverridenPreferredResources?.Count > 0)
                                    {
                                        ObjectExtension.SetPropertyValue(resource?.OverridenPreferredResources, "ActionByUser", resource.ActionByUser);
                                        ObjectExtension.SetPropertyValue(resource?.OverridenPreferredResources, "EventId", resource.EventId);
                                        ObjectExtension.SetPropertyValue(resource?.OverridenPreferredResources, "ModifiedBy", resource.ModifiedBy);
                                        response = ProcessOverridenResources(resource?.OverridenPreferredResources, resourceSearchId.Value, resource?.ActionByUser, ref dbOverrideResources, ref dbTsInfos, ref validationMessages);
                                        if (response != null && response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }
                                    if (resource.SearchType == ResourceSearchType.ARS.ToString() && resource.SearchAction.ToString() == ResourceSearchAction.AR.ToString())
                                    {
                                        response = AssignResourceToAssignment(resource, ref dbAssignment, dbTechnicalSpecialist, isDBValidationRequire);
                                        if (response != null && response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }
                                    if (dbAssignment != null && resource.SearchType == ResourceSearchType.ARS.ToString() || resource.SearchType == ResourceSearchType.PreAssignment.ToString())
                                    {
                                        response = AssignResourceSearchToAssignment(resourceSearchId, resource, ref dbAssignment, isDBValidationRequire);
                                        if (response != null && response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }

                                    if (resource.SearchType == ResourceSearchType.PreAssignment.ToString())
                                    {
                                        response = ProcessTsCalanderInfo(resourceSearchId.Value, resource);
                                        if (response != null && response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }
                                    ProcessSearchWorkflowEvents(resource, resourceSearchId, ref dbCustomers, ref dbTsInfos);
                                    ProcessResourceSearchNotes(new List<DomainModel.ResourceSearch> { resource }, dbResourceSearch, ValidationType.Update, ref validationMessages);
                                    tranScope.Complete();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resource);
            }
            finally
            {
                _resourceSearchRepository.AutoSave = true;

            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), resourceSearchId, exception);
        }

        public Response Save(DomainModel.ResourceSearch resource, bool commitChange = true)
        {
            IList<DbModel.ResourceSearch> dbResourceSearch = null;
            return Save(resource, ref dbResourceSearch, commitChange);
        }

        public Response Save(DomainModel.ResourceSearch resource, ref IList<DbModel.ResourceSearch> dbResourceSearch, bool commitChange = true)
        {
            IList<DbModel.Customer> dbCustomers = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.User> dbCoordinators = null;
            IList<DbModel.Data> dbCategory = null;
            IList<DbModel.TaxonomySubCategory> dbSubCategory = null;
            IList<DbModel.TaxonomyService> dbService = null;
            IList<DbModel.OverrideResource> dbOverrideResources = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            DbModel.Assignment dbAssignment = null;
            return Save(resource, ref dbResourceSearch, ref dbCustomers, ref dbCompanies, ref dbCoordinators, ref dbCategory, ref dbSubCategory, ref dbService, ref dbOverrideResources, ref dbTechnicalSpecialist, ref dbAssignment, commitChange);
        }

        public Response Save(DomainModel.ResourceSearch resource,
                                ref IList<DbModel.ResourceSearch> dbResourceSearch,
                                ref IList<DbModel.Customer> dbCustomers,
                                ref IList<DbModel.Company> dbCompanies,
                                ref IList<DbModel.User> dbCoordinators,
                                ref IList<DbModel.Data> dbCategory,
                                ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                ref IList<DbModel.TaxonomyService> dbService,
                                ref IList<DbModel.OverrideResource> dbOverrideResources,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                ref DbModel.Assignment dbAssignment,
                                bool isDBValidationRequire = true,
                                bool commitChange = true,
                                IList<DbModel.SqlauditModule> dbModule = null)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            int? resourceSearchId = null;
            Response response = null;
            long? eventId = null;
            IList<DbModel.Customer> dbCustomersTemp = dbCustomers;
            IList<DbModel.Company> dbCompaniesTemp = dbCompanies;
            IList<DbModel.Data> dbCategoryTemp = dbCategory;
            IList<DbModel.TaxonomySubCategory> dbSubCategoryTemp = dbSubCategory;
            IList<DbModel.TaxonomyService> dbServiceTemp = dbService;
            try
            {
                if (resource != null)
                {
                    if (resource.RecordStatus.IsRecordStatusNew() && CheckRecordValidForProcess(resource, ValidationType.Add, ref dbResourceSearch, ref dbCoordinators, ref dbCompanies, ref dbCustomers, ref dbCategory, ref dbSubCategory, ref dbService, ref dbOverrideResources, ref dbAssignment, ref dbTechnicalSpecialists, ref validationMessages, isDBValidationRequire))
                    {
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        {
                            _resourceSearchRepository.AutoSave = false;
                            AssigneSearchToUser(ref resource);

                            if (string.IsNullOrEmpty(resource.SearchParameter.AssignmentNumber) && dbAssignment != null)
                                resource.SearchParameter.AssignmentNumber = dbAssignment.AssignmentNumber.ToString();

                            var recordToBeInserted = _mapper.Map<IList<DbModel.ResourceSearch>>(new List<DomainModel.ResourceSearch> { resource }, opt =>
                            {
                                opt.Items["isAssignId"] = false;
                                opt.Items["serializationType"] = SerializationType.JSON;
                                opt.Items["dbCustomer"] = dbCustomersTemp;
                                opt.Items["dbCompany"] = dbCompaniesTemp;
                                opt.Items["dbCategory"] = dbCategoryTemp;
                                opt.Items["dbSubCategory"] = dbSubCategoryTemp;
                                opt.Items["dbService"] = dbServiceTemp;
                            });

                            _resourceSearchRepository.Add(recordToBeInserted);
                            if (commitChange)
                            {
                                int value = _resourceSearchRepository.ForceSave();
                                if (value > 0)
                                {
                                    dbResourceSearch = recordToBeInserted;
                                    resourceSearchId = dbResourceSearch?.FirstOrDefault()?.Id;
                                    //AuditLog(resource,
                                    //         ValidationType.Add.ToAuditActionType(),
                                    //         SqlAuditModuleType.ResourceSearch,
                                    //         null,
                                    //         _mapper.Map<List<DomainModel.ResourceSearch>>(dbResourceSearch),
                                    //         resourceSearchId.ToString(),
                                    //         ref eventId,
                                    //         dbModule);
                                    if (eventId != null && eventId > 0)
                                        ObjectExtension.SetPropertyValue(resource, "EventId", eventId);
                                    if (resource?.OverridenPreferredResources?.Count > 0)
                                    {
                                        ObjectExtension.SetPropertyValue(resource?.OverridenPreferredResources, "ActionByUser", resource.ActionByUser);
                                        ObjectExtension.SetPropertyValue(resource?.OverridenPreferredResources, "EventId", resource.EventId);
                                        ObjectExtension.SetPropertyValue(resource?.OverridenPreferredResources, "ModifiedBy", resource.ModifiedBy);
                                        response = ProcessOverridenResources(resource?.OverridenPreferredResources, resourceSearchId.Value, resource?.ActionByUser, ref dbOverrideResources, ref dbTsInfos, ref validationMessages);
                                        if (response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }
                                    if (resource.SearchType == ResourceSearchType.ARS.ToString())
                                    {
                                        response = AssignResourceSearchToAssignment(resourceSearchId, resource, ref dbAssignment, isDBValidationRequire);
                                        if (response != null && response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }

                                    if (resource.SearchType == ResourceSearchType.PreAssignment.ToString())
                                    {
                                        response = ProcessTsCalanderInfo(resourceSearchId.Value, resource);
                                        if (response != null && response?.Code != MessageType.Success.ToId())
                                        {
                                            return response;
                                        }
                                    }

                                    ProcessSearchWorkflowEvents(resource, resourceSearchId, ref dbCustomers, ref dbTsInfos);
                                    ProcessResourceSearchNotes(new List<DomainModel.ResourceSearch> { resource }, dbResourceSearch, ValidationType.Add, ref validationMessages);

                                    tranScope.Complete();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resource);
            }
            finally
            {
                _resourceSearchRepository.AutoSave = true;
                dbCustomersTemp = null;
                dbCompaniesTemp = null;
                dbCategoryTemp = null;
                dbSubCategoryTemp = null;
                dbServiceTemp = null;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), resourceSearchId, exception);
        }

        public Response GetARSSearchAssignmentDetail(int assignmentId)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            DomainModel.ResourceSearch resourceSearch = null;
            try
            {
                validationMessages = validationMessages ?? new List<ValidationMessage>();
                var include = new string[] {
                                            "ContractCompany",
                                            "OperatingCompany",
                                            "ContractCompanyCoordinator",
                                            "OperatingCompanyCoordinator",
                                            "Project",
                                            "Project.Contract",
                                            "Project.Contract.Customer",
                                            "SupplierPurchaseOrder" ,
                                            "SupplierPurchaseOrder.Supplier",
                                            "AssignmentTaxonomy",
                                            "AssignmentTaxonomy.TaxonomyService.TaxonomySubCategory.TaxonomyCategory",
                                            "AssignmentTaxonomy.TaxonomyService.TaxonomySubCategory",
                                            "AssignmentTaxonomy.TaxonomyService",
                                            "AssignmentSubSupplier",
                                            "AssignmentSubSupplier.AssignmentSubSupplierTechnicalSpecialist",
                                            "AssignmentSubSupplier.AssignmentSubSupplierTechnicalSpecialist.TechnicalSpecialist",
                                            "AssignmentSubSupplier.SupplierContact.Supplier",
                                            "Visit",
                                            "Timesheet"
                                            };

                var assignmentInfo = _assignmentRepository.SearchAssignment(assignmentId, include);

                resourceSearch = _mapper.Map<DomainModel.ResourceSearch>(assignmentInfo);
                var assignmentTaxonomyService = assignmentInfo?.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.TaxonomyServiceName;
                if (resourceSearch?.SearchParameter?.AssignedResourceInfos == null)
                {
                    resourceSearch.SearchParameter.AssignedResourceInfos = new List<AssignedResourceInfo>();
                }
                if (assignmentInfo.SupplierPurchaseOrderId.HasValue)
                {
                    assignmentInfo?.AssignmentSubSupplier?.ToList()?.ForEach(x =>
                    {
                        resourceSearch?.SearchParameter?.AssignedResourceInfos.Add(new AssignedResourceInfo
                        {
                            AssignedTechSpec = x.AssignmentSubSupplierTechnicalSpecialist?.Where(x4 => x4.IsDeleted != true)?.Select(x1 => new BaseResourceSearchTechSpecInfo
                            {
                                Epin = Convert.ToInt32(x1?.TechnicalSpecialist?.TechnicalSpecialist.Pin),
                                FirstName = x1?.TechnicalSpecialist?.TechnicalSpecialist?.FirstName,
                                LastName = x1?.TechnicalSpecialist?.TechnicalSpecialist?.LastName,
                                ProfileStatus = x1?.TechnicalSpecialist?.TechnicalSpecialist?.ProfileStatus.Name,
                                IsTechSpecFromAssignmentTaxonomy = (assignmentInfo?.AssignmentTechnicalSpecialist?
                                                                   .SelectMany(x2 => x2.TechnicalSpecialist?.TechnicalSpecialistTaxonomy)?
                                                                   .Where(x3 => x3.TechnicalSpecialist?.Pin == Convert.ToInt32(x1?.TechnicalSpecialist?.TechnicalSpecialist.Pin) && x3.TaxonomyServicesId == assignmentInfo?.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.Id)?
                                                                   .ToList()?.Count > 0) ? true : false
                            }).ToList(),
                            SupplierName = x.Supplier?.SupplierName,
                            SupplierId = (int)x.Supplier?.Id,
                            TaxonomyServiceName = assignmentTaxonomyService
                        });
                    });
                }
                else
                {
                    if (assignmentInfo?.AssignmentTechnicalSpecialist?.Count > 0)
                    {
                        var resources = new AssignedResourceInfo()
                        {
                            AssignedTechSpec = assignmentInfo?.AssignmentTechnicalSpecialist?.Select(x1 => new BaseResourceSearchTechSpecInfo
                            {
                                Epin = Convert.ToInt32(x1?.TechnicalSpecialist?.Pin),
                                FirstName = x1?.TechnicalSpecialist?.FirstName,
                                LastName = x1?.TechnicalSpecialist?.LastName,
                                ProfileStatus = x1?.TechnicalSpecialist?.ProfileStatus.Name,
                                IsTechSpecFromAssignmentTaxonomy = x1?.TechnicalSpecialist?.TechnicalSpecialistTaxonomy?
                                                                                               .Where(x3 => x3.TechnicalSpecialist?.Pin == Convert.ToInt32(x1?.TechnicalSpecialist?.Pin) && x3.TaxonomyServicesId == assignmentInfo?.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.Id)?
                                                                                               .ToList()?.Count > 0 ? true : false
                            }).ToList(),
                            TaxonomyServiceName = assignmentTaxonomyService
                        };
                        var assignedResource = resourceSearch?.SearchParameter?.AssignedResourceInfos;
                        resourceSearch?.SearchParameter?.AssignedResourceInfos.Add(resources);
                    }
                }
                if (assignmentInfo?.Visit?.Count > 0)
                {
                    var firstVisit = assignmentInfo.Visit?.FirstOrDefault(x => x.FromDate == assignmentInfo.Visit.Min(x1 => x1.FromDate));
                    if (firstVisit != null)
                    {
                        resourceSearch.SearchParameter.FirstVisitFromDate = firstVisit.FromDate;
                        resourceSearch.SearchParameter.FirstVisitToDate = firstVisit.ToDate;
                        resourceSearch.SearchParameter.FirstVisitStatus = firstVisit.VisitStatus?.Trim();
                    }
                }
                else if (assignmentInfo.Timesheet?.Count > 0)
                {
                    var firstTimeSheet = assignmentInfo.Timesheet?.FirstOrDefault(x => x.FromDate == assignmentInfo.Timesheet.Min(x1 => x1.FromDate));
                    if (firstTimeSheet != null)
                    {
                        resourceSearch.SearchParameter.FirstVisitFromDate = firstTimeSheet.FromDate;
                        resourceSearch.SearchParameter.FirstVisitToDate = firstTimeSheet.ToDate;
                        resourceSearch.SearchParameter.FirstVisitStatus = firstTimeSheet.TimesheetStatus?.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), string.Format("assignmentId : {0}", assignmentId.ToString()));
            }
            finally
            {
                _resourceSearchRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), resourceSearch, exception);
        }

        public Response ChangeStatusOfPreAssignment(IList<int?> preAssignmentIds, int? assignmentId, string ModifiedByUser, ref IList<ValidationMessage> validationMessages
                                                    , bool commitChange = true)
        {
            Exception exception = null;
            DomainModel.ResourceSearch resourceSearch = null;
            IList<DbModel.ResourceSearch> dbResourceSearches = null;
            List<ValidationMessage> messages = new List<ValidationMessage>();
            try
            {
                validationMessages = validationMessages ?? new List<ValidationMessage>();
                bool result = IsValidResouceService(preAssignmentIds, ref dbResourceSearches, ref validationMessages);
                if (result)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        if (dbResourceSearches?.Count > 0)
                        {
                            _resourceSearchRepository.AutoSave = false;

                            preAssignmentIds.ToList().ForEach(dbRes =>
                            {
                                var dbResourceSearch = dbResourceSearches?.FirstOrDefault(x => dbRes == x.Id);

                                if (string.Equals(dbResourceSearch?.SearchType, ResourceSearchType.PreAssignment.ToString()))
                                {
                                    dbResourceSearch.ActionStatus = ResourceSearchAction.APA.ToString();
                                    dbResourceSearch.LastModification = DateTime.UtcNow;
                                    dbResourceSearch.UpdateCount = dbResourceSearch.UpdateCount.CalculateUpdateCount();
                                    dbResourceSearch.ModifiedBy = ModifiedByUser;
                                    dbResourceSearch.AssignmentId = assignmentId;
                                    _resourceSearchRepository.Update(dbResourceSearch, a => a.ActionStatus,
                                                                                       b => b.LastModification,
                                                                                       c => c.UpdateCount,
                                                                                       d => d.ModifiedBy,
                                                                                       e => e.AssignmentId);
                                }
                                else
                                {
                                    messages.Add(_messageDescriptions, string.Format("preAssignmentId : {0}", dbRes), MessageType.AccountReferenceAlreadyExists);
                                }
                            });
                        }
                        if (commitChange)
                        {
                            int value = _resourceSearchRepository.ForceSave();
                            if (value > 0)
                            {
                                tranScope.Complete();
                            }
                        }
                        if (messages.Count > 0)
                            validationMessages.AddRange(messages);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), preAssignmentIds);
            }
            finally
            {
                _resourceSearchRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages.ToList(), resourceSearch, exception);
        }

        public Response IsRecordValidForProcess(DomainModel.ResourceSearch resourceSearch,
                                    ref IList<DbModel.ResourceSearch> dbResourceSearches,
                                    ref IList<DbModel.User> dbCoordinators,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<DbModel.Customer> dbCustomers,
                                    ref IList<DbModel.Data> dbCategory,
                                    ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                    ref IList<DbModel.TaxonomyService> dbService,
                                    ref IList<DbModel.OverrideResource> dbOverrideResources,
                                    ref DbModel.Assignment dbAssignment)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            try
            {
                if (resourceSearch.RecordStatus.IsRecordStatusNew())
                    result = IsRecordValidForAdd(resourceSearch, ref dbResourceSearches, ref dbCoordinators, ref dbCompanies, ref dbCustomers, ref dbCategory, ref dbSubCategory, ref dbService, ref dbAssignment, ref dbTechSpecialists, ref validationMessages);
                else if (resourceSearch.RecordStatus.IsRecordStatusDeleted())
                    result = IsRecordValidForRemove(resourceSearch, ref dbResourceSearches, ref validationMessages);
                else if (resourceSearch.RecordStatus.IsRecordStatusModified())
                    result = IsRecordValidForUpdate(resourceSearch, ref dbResourceSearches, ref dbCoordinators, ref dbCompanies, ref dbCustomers, ref dbCategory, ref dbSubCategory, ref dbService, ref dbOverrideResources, ref dbAssignment, ref dbTechSpecialists, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #region Private Methods

        private bool IsValidResouceService(IList<int?> resourceId, ref IList<DbModel.ResourceSearch> dbResourceSearch
                                           , ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbResourceInfos = _resourceSearchRepository.FindBy(x => resourceId.Contains(x.Id)).ToList();
            if (dbResourceInfos != null && dbResourceInfos.Any(x => x.ActionStatus == ResourceSearchAction.APA.ToString()))
            {
                string errorCode = MessageType.InvalidDraftID.ToId();
                messages.Add(_messageDescriptions, null, MessageType.PreAssignmentAssigned, resourceId);
            } //Changes For Sanity Def 158
            if (dbResourceSearch?.Count <= 0)
            {
                string errorCode = MessageType.InvalidDraftID.ToId();
                messages.Add(_messageDescriptions, null, MessageType.InvalidResourceSearchId, resourceId);
            }

            dbResourceSearch = dbResourceInfos;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool CheckRecordValidForProcess(DomainModel.ResourceSearch resourceSearch,
                                    ValidationType validationType,
                                    ref IList<DbModel.ResourceSearch> dbResourceSearches,
                                    ref IList<DbModel.User> dbCoordinators,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<DbModel.Customer> dbCustomers,
                                    ref IList<DbModel.Data> dbCategory,
                                    ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                    ref IList<DbModel.TaxonomyService> dbService,
                                    ref IList<DbModel.OverrideResource> dbOverrideResources,
                                    ref DbModel.Assignment dbAssignment,
                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                    ref IList<ValidationMessage> validationMessages,
                                    bool isDBValidationRequire = true)
        {
            Exception exception = null;
            bool result = false;
            try
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();
                if (!isDBValidationRequire)
                    return true;

                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(resourceSearch, ref dbResourceSearches, ref dbCoordinators, ref dbCompanies, ref dbCustomers, ref dbCategory, ref dbSubCategory, ref dbService, ref dbAssignment, ref dbTechnicalSpecialist, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(resourceSearch, ref dbResourceSearches, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(resourceSearch, ref dbResourceSearches, ref dbCoordinators, ref dbCompanies, ref dbCustomers, ref dbCategory, ref dbSubCategory, ref dbService, ref dbOverrideResources, ref dbAssignment, ref dbTechnicalSpecialist, ref validationMessages);

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }
            return result;
        }

        private bool IsRecordValidForAdd(DomainModel.ResourceSearch resourceSearch,
                                    ref IList<DbModel.ResourceSearch> dbResourceSearches,
                                    ref IList<DbModel.User> dbCoordinators,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<DbModel.Customer> dbCustomers,
                                    ref IList<DbModel.Data> dbCategory,
                                    ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                    ref IList<DbModel.TaxonomyService> dbService,
                                    ref DbModel.Assignment dbAssignment,
                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                    ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (resourceSearch != null)
            {

                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (IsValidPayload(new List<DomainModel.ResourceSearch> { resourceSearch }, ValidationType.Add, ref validationMessages))
                {
                    if (!string.IsNullOrEmpty(resourceSearch.CustomerCode) || resourceSearch.SearchType != ResourceSearchType.Quick.ToString())//Defe 625 #11 fix
                    {
                        result = _customerService.IsValidCustomer(new List<string> { resourceSearch.CustomerCode }, ref dbCustomers, ref validationMessages);
                    }
                    else
                    {
                        result = true;
                    }

                    if (result)
                    {
                        if (resourceSearch.SearchType != ResourceSearchType.Quick.ToString() && resourceSearch.SearchType != ResourceSearchType.PreAssignment.ToString())//Added for Scenario Fix PreAssignment/QuickSearch is not Saving
                        {
                            result = IsValidAssignment(Convert.ToInt32(resourceSearch?.SearchParameter?.AssignmentNumber),
                                                                                           Convert.ToInt32(resourceSearch?.SearchParameter?.ProjectNumber),
                                                                                           ref dbAssignment,
                                                                                           ref validationMessages,
                                                                                           null,
                                                                                           (int)resourceSearch?.AssignmentId);
                        }
                        else
                        {
                            result = true;
                        }

                        if (result)
                        {
                            result = IsValidCHAndOPCompany(resourceSearch, ref dbCompanies, ref validationMessages);
                            if (result)
                            {
                                result = IsValidCHAndOPCoordinator(resourceSearch, ref dbCoordinators, ref validationMessages);

                                if (result)
                                {
                                    result = IsValidTaxonomy(resourceSearch, ref dbCategory, ref dbSubCategory, ref dbService, ref validationMessages);
                                    if (result)
                                        GetTechSpec(resourceSearch, ref dbTechnicalSpecialist);
                                }
                            }
                        }

                    }
                }

            }
            return result;
        }

        private bool IsRecordValidForUpdate(DomainModel.ResourceSearch resourceSearch,
                                    ref IList<DbModel.ResourceSearch> dbResourceSearches,
                                    ref IList<DbModel.User> dbCoordinators,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<DbModel.Customer> dbCustomers,
                                    ref IList<DbModel.Data> dbCategory,
                                    ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                    ref IList<DbModel.TaxonomyService> dbService,
                                    ref IList<DbModel.OverrideResource> dbOverrideResources,
                                    ref DbModel.Assignment dbAssignment,
                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                    ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (resourceSearch != null)
            {
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (IsValidPayload(new List<DomainModel.ResourceSearch> { resourceSearch }, ValidationType.Update, ref validationMessages))
                {
                    IList<int?> resSearchIds = new List<int?> { resourceSearch?.Id };

                    result = IsValidResouceService(resSearchIds, ref dbResourceSearches, ref validationMessages);
                    if (result)
                    {
                        if (IsRecordUpdateCountMatching(resourceSearch, dbResourceSearches, ref validationMessages))
                        {
                            IList<string> custCodes = !string.IsNullOrEmpty(resourceSearch?.CustomerCode) ? new List<string> { resourceSearch?.CustomerCode } : null;

                            if (custCodes?.Count > 0 || resourceSearch.SearchType != ResourceSearchType.Quick.ToString())//Defe 625 #11 fix
                            {
                                result = _customerService.IsValidCustomer(custCodes, ref dbCustomers, ref validationMessages);
                            }
                            else
                            {
                                result = true;
                            }

                            if (result)
                            {
                                if (resourceSearch.SearchType != ResourceSearchType.Quick.ToString() && resourceSearch.SearchType != ResourceSearchType.PreAssignment.ToString()) //Added for Scenario Fix PreAssignment/QuickSearch is not Updating
                                {
                                    result = IsValidAssignment(Convert.ToInt32(resourceSearch?.SearchParameter?.AssignmentNumber),
                                                                                Convert.ToInt32(resourceSearch?.SearchParameter?.ProjectNumber),
                                                                                ref dbAssignment,
                                                                                ref validationMessages,
                                                                                null,
                                                                                (int)resourceSearch?.AssignmentId);
                                }
                                else
                                {
                                    result = true;
                                }
                                if (result)
                                {
                                    result = IsValidCHAndOPCompany(resourceSearch, ref dbCompanies, ref validationMessages);
                                    if (result)
                                    {
                                        result = IsValidCHAndOPCoordinator(resourceSearch, ref dbCoordinators, ref validationMessages);
                                        if (result)
                                        {
                                            result = IsValidTaxonomy(resourceSearch, ref dbCategory, ref dbSubCategory, ref dbService, ref validationMessages);

                                            if (result)
                                            {
                                                GetTechSpec(resourceSearch, ref dbTechnicalSpecialist);
                                                if (resourceSearch.SearchType == ResourceSearchType.ARS.ToString() && resourceSearch?.OverridenPreferredResources != null && (resourceSearch.SearchAction == ResourceSearchAction.OPR.ToString() || resourceSearch.SearchAction == ResourceSearchAction.ARR.ToString()))
                                                {
                                                    var arsSearchIds = resourceSearch?.OverridenPreferredResources.Where(x => x.RecordStatus == RecordStatus.Modify.FirstChar()).Select(x => (int)x.Id).ToList();
                                                    if (arsSearchIds?.Count > 0)
                                                    {
                                                        result = _overrideResourceService.IsRecordExistInDb(arsSearchIds, ref dbOverrideResources, ref validationMessages);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return result;
        }

        private bool IsValidAssignment(int assignmentNumber,
                                        int assignmentProjectNumber,
                                        ref DbModel.Assignment dbAssignment,
                                        ref IList<ValidationMessage> messages,
                                        string[] includes,
                                        int assignmentId = 0)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            DbModel.Assignment dbAssign = null;
            if ((assignmentNumber > 0 && assignmentProjectNumber > 0) || assignmentId > 0)
            {
                if (dbAssignment == null)
                {
                    if (includes == null)
                        includes = new string[] { "Project" };
                    else
                        includes = includes.Append("Project").ToArray();
                    if (assignmentId > 0)
                        dbAssign = _assignmentRepository?.FindBy(x => x.Id == assignmentId, includes)?.FirstOrDefault();
                    else
                        dbAssign = _assignmentRepository?.FindBy(x => x.AssignmentNumber == assignmentNumber && x.Project.ProjectNumber == assignmentProjectNumber, includes)?.FirstOrDefault();
                    if (dbAssign == null)
                        message.Add(_messageDescriptions, null, MessageType.AssignmentWithNumberNotExists, assignmentNumber, assignmentProjectNumber);
                    dbAssignment = dbAssign;
                    messages = message;
                }
            }
            return messages?.Count <= 0;
        }

        private bool IsRecordValidForRemove(DomainModel.ResourceSearch resourceSearch,
                                   ref IList<DbModel.ResourceSearch> dbResourceSearches,
                                   ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (resourceSearch != null)
            {
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (IsValidPayload(new List<DomainModel.ResourceSearch> { resourceSearch }, ValidationType.Delete, ref validationMessages))
                {
                    IList<int?> resSearchIds = new List<int?> { resourceSearch?.Id };
                    result = IsValidResouceService(resSearchIds, ref dbResourceSearches, ref validationMessages);
                }

            }
            return result;
        }

        private bool IsValidPayload(IList<DomainModel.ResourceSearch> resourceSearches,
                      ValidationType validationType,
                      ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(resourceSearches), validationType);

            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.ResourceSearch, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsValidCHAndOPCompany(DomainModel.ResourceSearch resourceSearch,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<string> CompCodes = new List<string>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            bool result = false;

            if (dbCompanies != null)
            {
                return true;
            }

            if (dbCompanies?.Count <= 1)
            {  // To populate both CH and OP companies
                dbCompanies = new List<DbModel.Company>();
            }

            if (!string.IsNullOrEmpty(resourceSearch?.SearchParameter?.CHCompanyCode))
            {
                CompCodes.Add(resourceSearch?.SearchParameter?.CHCompanyCode);
            }
            else
            {
                validationMessages.Add(_messageDescriptions, resourceSearch, MessageType.ContractholdingCompanyCodeRequired);
                return result;
            }

            if (!string.IsNullOrEmpty(resourceSearch?.SearchParameter?.OPCompanyCode))
            {
                CompCodes.Add(resourceSearch?.SearchParameter?.OPCompanyCode);
            }
            else
            {
                validationMessages.Add(_messageDescriptions, resourceSearch, MessageType.OperatingCompanyCodeRequired);
                return result;
            }

            return _companyService.IsValidCompany(CompCodes.Distinct().ToList(), ref dbCompanies, ref validationMessages);
        }

        private bool IsValidCHAndOPCoordinator(DomainModel.ResourceSearch resourceSearch,
                                ref IList<DbModel.User> dbCoordinators,
                                ref IList<ValidationMessage> validationMessages)
        {
            var appUserNames = new List<KeyValuePair<string, string>>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            if (resourceSearch?.SearchType == ResourceSearchType.PreAssignment.ToString())
            {
                if (string.IsNullOrEmpty(resourceSearch?.SearchParameter?.OPCoordinatorLogOnName))
                {
                    validationMessages.Add(_messageDescriptions, resourceSearch, MessageType.OperatingCompanyCoordinatorRequired);
                    return false;
                }
                else
                {
                    appUserNames.Add(new KeyValuePair<string, string>(_environment.SecurityAppName, resourceSearch?.SearchParameter?.OPCoordinatorLogOnName));
                }
                if (string.IsNullOrEmpty(resourceSearch?.SearchParameter?.CHCoordinatorLogOnName))
                {
                    validationMessages.Add(_messageDescriptions, resourceSearch, MessageType.ContractholdingCompanyCoordinatorRequired);
                    return false;
                }
                else
                {
                    appUserNames.Add(new KeyValuePair<string, string>(_environment.SecurityAppName, resourceSearch?.SearchParameter?.CHCoordinatorLogOnName));
                }
            }

            IList<string> userNotExist = null;
            var response = _userService.IsRecordExistInDb(appUserNames, ref dbCoordinators, ref userNotExist);
            if (response.Code != ResponseType.Success.ToId() && Convert.ToBoolean(response.Result) == false)
            {
                validationMessages.AddRange(response.ValidationMessages);
                return false;
            }
            return true;
        }

        private bool IsValidTaxonomy(DomainModel.ResourceSearch resourceSearch,
                                    ref IList<DbModel.Data> dbCategory,
                                    ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                    ref IList<DbModel.TaxonomyService> dbService,
                                    ref IList<ValidationMessage> validationMessages)
        {
            bool isValidResult = true;
            if (resourceSearch != null)
            {
                IList<string> categoryNames = !string.IsNullOrEmpty(resourceSearch.CategoryName) ? new List<string> { resourceSearch.CategoryName } : null;
                IList<string> subcatNames = !string.IsNullOrEmpty(resourceSearch.SubCategoryName) ? new List<string> { resourceSearch.SubCategoryName } : null;
                IList<string> servNames = !string.IsNullOrEmpty(resourceSearch.ServiceName) ? new List<string> { resourceSearch.ServiceName } : null;

                if (categoryNames?.Count > 0 && dbCategory == null && dbSubCategory == null && dbService == null)
                {
                    var masterTypes = new List<MasterType>()
                    {
                        MasterType.TaxonomyCategory
                    };

                    var includes = new string[] { "TaxonomySubCategory",
                                                   "TaxonomySubCategory.TaxonomyService"
                                                };
                    var dbMaster = _masterService.Get(masterTypes, null, categoryNames, null, includes);
                    if (dbMaster?.Count > 0)
                    {
                        dbCategory = dbMaster?.Where(x => x.MasterDataTypeId == (int)MasterType.TaxonomyCategory).ToList();
                        dbSubCategory = dbCategory?.SelectMany(x => x.TaxonomySubCategory).ToList();
                        dbService = dbSubCategory?.SelectMany(x => x.TaxonomyService).ToList();
                    }

                    isValidResult = _taxonomyCatService.IsValidCategoryName(categoryNames, ref dbCategory, ref validationMessages);
                    if (isValidResult && subcatNames?.Count > 0)
                    {
                        var catAndSubCat = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(resourceSearch.CategoryName, resourceSearch.SubCategoryName) };
                        isValidResult = _taxonomySubCatService.IsValidSubCategoryName(catAndSubCat, ref dbSubCategory, ref validationMessages);

                        if (isValidResult && servNames?.Count > 0)
                        {
                            var subcatAndServices = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(resourceSearch.SubCategoryName, resourceSearch.ServiceName) };
                            isValidResult = _taxonomyServices.IsValidServiceName(subcatAndServices, ref dbService, ref validationMessages);
                        }
                    }
                }

            }

            return isValidResult;
        }

        private bool IsRecordUpdateCountMatching(DomainModel.ResourceSearch resourceSearch,
                        IList<DbModel.ResourceSearch> dbResourceSearches,
                        ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            if (resourceSearch != null)
            {
                if (!dbResourceSearches.Any(x1 => x1.UpdateCount.ToInt() == resourceSearch.UpdateCount.ToInt() && x1.Id == resourceSearch.Id))
                {
                    messages.Add(_messageDescriptions, resourceSearch, MessageType.ResourceSearchUpdatedByOther);
                }
            }
            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private void ProcessSearchWorkflowEvents(DomainModel.ResourceSearch resourceSearch, int? ResourceSearchId
                                                 , ref IList<DbModel.Customer> dbCustomers
                                                , ref IList<DbModel.TechnicalSpecialist> dbTsInfos)
        {
            if (resourceSearch != null)
            {
                switch (resourceSearch.SearchType.ToEnum<ResourceSearchType>())
                {
                    case ResourceSearchType.PreAssignment:
                        if (resourceSearch.SearchParameter != null && !string.Equals(resourceSearch?.SearchParameter?.CHCompanyCode, resourceSearch?.SearchParameter?.OPCompanyCode))
                        {
                            switch (resourceSearch.SearchAction.ToEnum<ResourceSearchAction>())
                            {

                                case ResourceSearchAction.W: //WON
                                    ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailPreAssignmentWon, ResourceSearchId, dbCustomers: dbCustomers);
                                    break;

                                case ResourceSearchAction.L: //Lost
                                    ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailPreAssignmentLost, ResourceSearchId, dbCustomers: dbCustomers);
                                    break;
                                case ResourceSearchAction.CUP: //Create/Update Pre-Assignment

                                    if (resourceSearch.RecordStatus.IsRecordStatusNew() || (resourceSearch.RecordStatus.IsRecordStatusModified() && string.Equals(resourceSearch?.SearchParameter?.CHCompanyCode, resourceSearch?.CompanyCode)))
                                    {
                                        ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailInterCompanyPreAssignmentCreate, ResourceSearchId, dbCustomers: dbCustomers);
                                    }
                                    else if (resourceSearch.RecordStatus.IsRecordStatusModified() && string.Equals(resourceSearch?.SearchParameter?.OPCompanyCode, resourceSearch?.CompanyCode))
                                    {
                                        ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailInterCompanyPreAssignmentUpdate, ResourceSearchId, dbCustomers: dbCustomers);
                                    }

                                    break;
                            }
                        }
                        break;
                    //case ResourceSearchType.Quick:
                    //    break;
                    case ResourceSearchType.ARS:
                        switch (resourceSearch.SearchAction.ToEnum<ResourceSearchAction>())
                        {
                            case ResourceSearchAction.OPR: //Override Preferred Resource   
                                ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailResourceOverrideApproval, ResourceSearchId, dbTsInfos: dbTsInfos);
                                break;

                            case ResourceSearchAction.ARR: //Approve/Reject Resource 
                                ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailPreferredResourceRejected, ResourceSearchId, dbTsInfos: dbTsInfos); //To send email with rejected Resource Info
                                ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailPreferredResourceApproved, ResourceSearchId, dbTsInfos: dbTsInfos); //To send email with Approved Resource Info
                                break;
                            case ResourceSearchAction.PLO: //Potential Lost Opportunity
                                //ProcessEmailNotifications(resourceSearch, EmailTemplate.PendingApproval, ResourceSearchId);
                                ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailSearchAssistanceRequested, ResourceSearchId);
                                break;
                            case ResourceSearchAction.NMG: //No match in GRM
                                ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailPotentialResourceNotFound, ResourceSearchId);
                                break;
                            case ResourceSearchAction.SSSPC: //Save Search and Send to PC
                                ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailPotentialResourceFound, ResourceSearchId);
                                break;
                            case ResourceSearchAction.SD: //Search Disposition
                                if (resourceSearch?.SearchParameter?.PLOTaxonomyInfo != null)
                                {
                                    ProcessEmailNotifications(resourceSearch, EmailTemplate.EmailSearchDisposition, ResourceSearchId, dbCustomers: dbCustomers);
                                }
                                break;
                                //case ResourceSearchAction.AR: // Assign Resource
                                //    if (resourceSearch?.SearchParameter?.PLOTaxonomyInfo == null && resourceSearch?.SearchParameter?.OverrideTaxonomyInfo == null)
                                //    {
                                //        ProcessEmailNotifications(resourceSearch, EmailTemplate.ResourceDispositionStatusChange, ResourceSearchId, dbCustomers: dbCustomers);
                                //    }
                                //    break;


                        }

                        ProcessARSTasks(resourceSearch, ResourceSearchId);

                        break;
                }
            }
        }

        private Response ProcessEmailNotifications(DomainModel.ResourceSearch resourceSearches, EmailTemplate emailTemplateType
                                                   , int? ResourceSearchId, IList<DbModel.Customer> dbCustomers = null,
                                                    IList<DbModel.TechnicalSpecialist> dbTsInfos = null)
        {
            IList<EmailAddress> coordinatorEmail = null;
            string emailSubject = string.Empty;
            string customerName = string.Empty;
            string cHCoordinatorName = string.Empty;
            string opCoordinatorName = string.Empty;
            string assignmentId = string.Empty;
            string overrideResourceNames = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = null;
            List<EmailAddress> fromAddresses = null;
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            IList<UserInfo> userInfos = null;
            try
            {
                if (resourceSearches != null)
                {
                    userInfos = GetUserInfo(resourceSearches, emailTemplateType);

                    if (userInfos?.Count > 0)
                    {
                        if (resourceSearches.SearchType == ResourceSearchType.PreAssignment.ToString())
                        {
                            if (resourceSearches.SearchParameter != null && !string.Equals(resourceSearches?.SearchParameter?.CHCompanyCode, resourceSearches?.SearchParameter?.OPCompanyCode))
                            {   //Inter company Scenario  
                                if (resourceSearches.RecordStatus.IsRecordStatusNew() || (resourceSearches.RecordStatus.IsRecordStatusModified() && string.Equals(resourceSearches?.SearchParameter?.CHCoordinatorLogOnName, resourceSearches?.ModifiedBy)))
                                {
                                    fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.CHCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                    toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                }
                                else if (resourceSearches.RecordStatus.IsRecordStatusModified() && string.Equals(resourceSearches?.SearchParameter?.OPCoordinatorLogOnName, resourceSearches?.ModifiedBy))
                                {
                                    toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.CHCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                    fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                }
                            }
                            else
                            {   //domestic company Scenario 
                                coordinatorEmail = userInfos?.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.CHCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                            }
                        }

                        assignmentId = string.Format("{0}-{1}", resourceSearches?.SearchParameter?.ProjectNumber, resourceSearches?.SearchParameter?.AssignmentNumber);
                        cHCoordinatorName = userInfos?.FirstOrDefault(x => x.LogonName == resourceSearches?.SearchParameter?.CHCoordinatorLogOnName)?.UserName;
                        opCoordinatorName = userInfos?.FirstOrDefault(x => x.LogonName == resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)?.UserName;

                        switch (emailTemplateType)
                        {
                            case EmailTemplate.EmailInterCompanyPreAssignmentCreate:

                                customerName = dbCustomers?.FirstOrDefault(x => x.Code == resourceSearches.CustomerCode)?.Name;
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Pre_Assignment_Created_Subject, ResourceSearchId.ToString());
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_Customer_Name, customerName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_CH_Coordinator_Name, cHCoordinatorName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_PRE_ASSIGNMENT_ID, ResourceSearchId.ToString()),
                                };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.IPAC, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses);
                                break;

                            case EmailTemplate.EmailInterCompanyPreAssignmentUpdate:

                                customerName = dbCustomers?.FirstOrDefault(x => x.Code == resourceSearches.CustomerCode)?.Name;
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Pre_Assignment_Updated_Subject, ResourceSearchId.ToString());
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_Customer_Name, customerName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_OC_Coordinator_Name, opCoordinatorName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_PRE_ASSIGNMENT_ID, ResourceSearchId.ToString()),
                                };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.IPAU, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses);
                                break;

                            //case EmailTemplate.ResourceNotFound:

                            //    toAddresses = coordinatorEmail.ToList();
                            //    emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Resource_NotFound, resourceSearches.SearchParameter.AssignmentNumber);
                            //    emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.Notification, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses);
                            //    break;

                            case EmailTemplate.EmailPreAssignmentWon:

                                customerName = dbCustomers?.FirstOrDefault(x => x.Code == resourceSearches.CustomerCode)?.Name;
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_PreAssignment_Won, ResourceSearchId.ToString());
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_Customer_Name, customerName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_CH_Coordinator_Name, cHCoordinatorName),
                                     new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_PRE_ASSIGNMENT_ID, ResourceSearchId.ToString()),
                                    };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.IPAW, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses);

                                break;

                            case EmailTemplate.EmailPreAssignmentLost:

                                customerName = dbCustomers?.FirstOrDefault(x => x.Code == resourceSearches.CustomerCode)?.Name;
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_PreAssignment_Lost, ResourceSearchId.ToString());
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_Customer_Name, customerName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_CH_Coordinator_Name, cHCoordinatorName),
                                     new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_PRE_ASSIGNMENT_ID, ResourceSearchId.ToString()),
                                };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.IPAL, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses);
                                break;

                            case EmailTemplate.EmailSearchDisposition:

                                var dbMaster = _masterService.Get(new List<MasterType>() { MasterType.DispositionType }, codes: new List<string> { resourceSearches?.DispositionType });

                                var dispositionTypeName = dbMaster?.FirstOrDefault(x => string.Equals(x.Code, resourceSearches?.DispositionType)).Name;

                                fromAddresses = userInfos?.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                toAddresses = userInfos?.Where(x => !string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName) && !string.Equals(x.LogonName, resourceSearches?.SearchParameter?.CHCoordinatorLogOnName))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Search_Disposition, assignmentId);
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_DISPOSITION_DETAIL, dispositionTypeName),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.SDS, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);

                                break;

                            case EmailTemplate.EmailResourceOverrideApproval:

                                //Get all resource names withoght considering IsApproved status info
                                overrideResourceNames = GetOverrideResourceNames(resourceSearches.OverridenPreferredResources, null, dbTsInfos);
                                var operationsManagerInfo = userInfos.FirstOrDefault(x => string.Equals(x.LogonName, resourceSearches?.AssignedToOmLognName));
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Override_Approval_Required, assignmentId);
                                fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                toAddresses = new List<EmailAddress> { new EmailAddress { DisplayName = operationsManagerInfo?.UserName, Address = operationsManagerInfo?.Email } };
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_COORDINATOR_FULL_NAME, opCoordinatorName),// def 978 SLNO 30 issue fix
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_RESOURCE_FULLNAME_LIST, overrideResourceNames),
                                    };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.OPRR, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);

                                break;

                            case EmailTemplate.EmailPreferredResourceRejected:

                                //Get only resource names with IsApproved=false 
                                overrideResourceNames = GetOverrideResourceNames(resourceSearches.OverridenPreferredResources, false, dbTsInfos);
                                if (!string.IsNullOrEmpty(overrideResourceNames))
                                {
                                    emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Preferred_Resource_Rejected, assignmentId);
                                    fromAddresses = new List<EmailAddress> { new EmailAddress { DisplayName = userInfos?.FirstOrDefault(x => string.Equals(x.LogonName, resourceSearches?.AssignedToOmLognName))?.UserName, Address = userInfos.FirstOrDefault(x => string.Equals(x.LogonName, resourceSearches?.AssignedToOmLognName))?.Email } };
                                    toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_RESOURCE_FULLNAME_LIST, overrideResourceNames)
                                    };
                                    emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.OPRJ, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);
                                }
                                break;

                            case EmailTemplate.EmailPreferredResourceApproved:

                                //Get only resource names with IsApproved=true 
                                overrideResourceNames = GetOverrideResourceNames(resourceSearches.OverridenPreferredResources, true, dbTsInfos);
                                if (!string.IsNullOrEmpty(overrideResourceNames))
                                {
                                    emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Preferred_Resource_Approved, assignmentId);
                                    fromAddresses = new List<EmailAddress> { new EmailAddress { DisplayName = userInfos?.FirstOrDefault(x => string.Equals(x.LogonName, resourceSearches?.AssignedToOmLognName))?.UserName, Address = userInfos.FirstOrDefault(x => string.Equals(x.LogonName, resourceSearches?.AssignedToOmLognName))?.Email } };
                                    toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_RESOURCE_FULLNAME_LIST, overrideResourceNames)
                                    };
                                    emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.OPRA, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);
                                }
                                break;

                            case EmailTemplate.EmailSearchAssistanceRequested:

                                fromAddresses = userInfos?.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName)).Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                toAddresses = userInfos?.Where(x => !string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList(); // def 978 PLO #2 issue
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Search_Assistance_Requested, assignmentId);
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_COORDINATOR_FULL_NAME, opCoordinatorName),
                                    };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.PLOR, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);

                                break;

                            case EmailTemplate.EmailPotentialResourceNotFound:
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Potential_Resource_not_Found, assignmentId);
                                fromAddresses = userInfos?.Where(x => string.Equals(x.LogonName, resourceSearches?.AssignedBy))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email })?.ToList();
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                     new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.PLON, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);

                                break;

                            case EmailTemplate.EmailPotentialResourceFound:
                                emailSubject = string.Format(ResourceSearchConstants.Email_Notification_Potential_Resource_Found, assignmentId);
                                fromAddresses = userInfos?.Where(x => string.Equals(x.LogonName, resourceSearches?.AssignedBy))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email })?.ToList();
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_COORDINATOR_FULL_NAME, opCoordinatorName),
                                     new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    };
                                emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.PLOF, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, fromAddresses: fromAddresses);

                                break;

                            //case EmailTemplate.PendingApproval:
                            //    emailSubject = string.Format(ResourceSearchConstants.Email_Notification_PendingApproval, assignmentId);
                            //    toAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email })?.ToList();
                            //    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                            //         new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                            //        };
                            //    emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.Notification, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses);

                            //    break;

                            case EmailTemplate.EmailResourceDispositionStatusChange:
                                if (resourceSearches?.SearchParameter?.SelectedTechSpecInfo != null && resourceSearches?.SearchParameter?.SelectedTechSpecInfo?.Count > 0)
                                {
                                    var assingedResources = string.Join(" ", resourceSearches?.SearchParameter?.SelectedTechSpecInfo?.Select(x => string.Format("<p> {0},{1} <{2}> </p>", x.FirstName, x.LastName, x.Epin)));
                                    emailSubject = string.Format(ResourceSearchConstants.Email_Notification_ResourceDispositionStatusChange, assignmentId);
                                    toAddresses = userInfos?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email }).ToList();
                                    ccAddresses = userInfos.Where(x => string.Equals(x.LogonName, resourceSearches?.SearchParameter?.OPCoordinatorLogOnName))?.Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email })?.ToList();
                                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                     new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_RESOURCE_FULLNAME_LIST, assingedResources),
                                      new KeyValuePair<string, string>(ResourceSearchConstants.Email_Content_ASSIGNMENT_ID, assignmentId),
                                    };
                                    emailMessage = ProcessEmailMessage(ModuleType.ResourceSearch, emailTemplateType, EmailType.Notification, ModuleCodeType.RSEARCH, ResourceSearchId.ToString(), emailSubject, emailContentPlaceholders, toAddresses);

                                }
                                break;

                        }
                        if (emailMessage != null)
                            return _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessARSTasks(DomainModel.ResourceSearch resourceSearch, int? ResourceSearchId)
        {
            Exception exception = null;
            IList<Evolution.Home.Domain.Models.Homes.MyTask> myTaskToBeCreated = null;
            IList<Evolution.Home.Domain.Models.Homes.MyTask> myTaskToBeDeleted = null;
            try
            {
                if (resourceSearch != null)
                {
                    myTaskToBeCreated = new List<Evolution.Home.Domain.Models.Homes.MyTask>();
                    myTaskToBeDeleted = new List<Evolution.Home.Domain.Models.Homes.MyTask>();
                    if (!string.IsNullOrEmpty(resourceSearch.SearchType)
                        && resourceSearch.SearchType.ToEnum<ResourceSearchType>() == ResourceSearchType.ARS
                        && !string.IsNullOrEmpty(resourceSearch.SearchAction))
                    {
                        var taskType = string.Empty;
                        var taskDescription = string.Empty;
                        var taskAssignedTo = string.Empty;

                        switch (resourceSearch.SearchAction.ToEnum<ResourceSearchAction>())
                        {
                            case ResourceSearchAction.OPR: //Override Preferred Resource    
                                taskType = ResourceSearchConstants.Task_Type_Override_Approval_Request;
                                taskDescription = string.Format(ResourceSearchConstants.Task_Description_OM_Verify_And_Validate, ResourceSearchId.ToString());
                                taskAssignedTo = resourceSearch.AssignedToOmLognName;
                                break;

                            case ResourceSearchAction.ARR: //Approve/Reject Resource   
                                taskType = ResourceSearchConstants.Task_Type_OM_Approve_Reject_Resource;
                                taskDescription = string.Format(ResourceSearchConstants.Task_Description_OM_Validated, ResourceSearchId.ToString());
                                taskAssignedTo = resourceSearch?.SearchParameter?.OPCoordinatorLogOnName;
                                break;
                            case ResourceSearchAction.PLO: //Potential Lost Opportunity
                                List<string> userTypes = new List<string> { ResourceSearchConstants.User_Type_RC, ResourceSearchConstants.User_Type_RM };
                                var userInfos = _userService.GetByUserType(resourceSearch?.CompanyCode, userTypes, true).Result.Populate<IList<UserInfo>>();

                                userInfos.ToList().ForEach(x =>
                                {
                                    // Create task for all RC and RM Users
                                    myTaskToBeCreated.Add(new Evolution.Home.Domain.Models.Homes.MyTask()
                                    {
                                        Moduletype = ModuleCodeType.RSEARCH.ToString(),
                                        TaskType = ResourceSearchConstants.Task_Type_PLO_To_RC,
                                        Description = string.Format(ResourceSearchConstants.Task_Description_PLO_to_RC, ResourceSearchId.ToString()),
                                        TaskRefCode = ResourceSearchId.ToString(),
                                        AssignedBy = resourceSearch?.ActionByUser,
                                        AssignedTo = x.LogonName,
                                        CreatedOn = DateTime.UtcNow,
                                        RecordStatus = RecordStatus.New.FirstChar(),
                                        CompanyCode = resourceSearch?.CompanyCode // D363 CR Change
                                    });
                                });

                                break;

                            case ResourceSearchAction.NMG: //No match in GRM
                                taskType = ResourceSearchConstants.Task_Type_PLO_No_Match_GRM;
                                taskDescription = string.Format(ResourceSearchConstants.Task_Description_PLO_No_Match_GRM, ResourceSearchId.ToString());
                                taskAssignedTo = resourceSearch?.SearchParameter?.OPCoordinatorLogOnName;
                                break;

                            case ResourceSearchAction.SSSPC:
                                taskType = ResourceSearchConstants.Task_Type_PLO_Search_And_Save_Resources;
                                taskDescription = string.Format(ResourceSearchConstants.Task_Description_PLO_Search_And_Save_Resources, ResourceSearchId.ToString());
                                taskAssignedTo = resourceSearch?.SearchParameter?.OPCoordinatorLogOnName;
                                break;
                        }
                        if (!string.IsNullOrEmpty(taskAssignedTo))
                        {
                            myTaskToBeCreated.Add(new Evolution.Home.Domain.Models.Homes.MyTask()
                            {
                                Moduletype = ModuleCodeType.RSEARCH.ToString(),
                                TaskType = taskType,
                                Description = taskDescription,
                                TaskRefCode = ResourceSearchId.ToString(),
                                AssignedBy = resourceSearch?.ActionByUser,
                                AssignedTo = taskAssignedTo,
                                CreatedOn = DateTime.UtcNow,
                                RecordStatus = RecordStatus.New.FirstChar(),
                                CompanyCode = resourceSearch?.CompanyCode//D661 issue1 myTask CR
                            });
                        }

                        if (resourceSearch?.MyTaskId > 0)
                        {
                            myTaskToBeDeleted.Add(new Evolution.Home.Domain.Models.Homes.MyTask()
                            {
                                MyTaskId = resourceSearch?.MyTaskId,
                                RecordStatus = RecordStatus.Delete.FirstChar()
                            });
                        }
                        else
                        {
                            myTaskToBeDeleted = _myTaskService.Get(new Home.Domain.Models.Homes.MyTask { TaskRefCode = ResourceSearchId.ToString(), Moduletype = ModuleCodeType.RSEARCH.ToString() }).Result.Populate<IList<Evolution.Home.Domain.Models.Homes.MyTask>>();
                            myTaskToBeDeleted.ToList()?.ForEach(x => x.RecordStatus = RecordStatus.Delete.FirstChar());
                        }
                    }

                    if (myTaskToBeDeleted?.Count > 0)
                    {
                        ObjectExtension.SetPropertyValue(myTaskToBeDeleted, "ActionByUser", resourceSearch.ActionByUser);
                        ObjectExtension.SetPropertyValue(myTaskToBeDeleted, "EventId", resourceSearch.EventId);
                        ObjectExtension.SetPropertyValue(myTaskToBeDeleted, "ModifiedBy", resourceSearch.ModifiedBy);
                        _myTaskService.Delete(myTaskToBeDeleted);
                    }

                    if (myTaskToBeCreated?.Count > 0)
                    {
                        ObjectExtension.SetPropertyValue(myTaskToBeCreated, "ActionByUser", resourceSearch.ActionByUser);
                        ObjectExtension.SetPropertyValue(myTaskToBeCreated, "EventId", resourceSearch.EventId);
                        ObjectExtension.SetPropertyValue(myTaskToBeDeleted, "ModifiedBy", resourceSearch.ModifiedBy);
                        _myTaskService.Add(myTaskToBeCreated, true);
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }

        private void AssigneSearchToUser(ref DomainModel.ResourceSearch resourceSearch)
        {
            if (resourceSearch != null)
            {
                if (resourceSearch.SearchType == ResourceSearchType.PreAssignment.ToString())
                {
                    if (resourceSearch.SearchParameter != null && string.Equals(resourceSearch?.SearchParameter?.CHCompanyCode, resourceSearch?.SearchParameter?.OPCompanyCode))
                    {
                        // domestic company
                        resourceSearch.AssignedTo = resourceSearch?.SearchParameter?.CHCoordinatorLogOnName;
                    }
                    else
                    {//Inter company 
                        resourceSearch.AssignedTo = resourceSearch?.SearchParameter?.OPCoordinatorLogOnName;
                    }
                }
                else if (resourceSearch.SearchType == ResourceSearchType.Quick.ToString())
                {
                    resourceSearch.AssignedTo = resourceSearch.CreatedBy;
                }
            }

        }

        private string GetOverrideResourceNames(IList<DomainModel.OverridenPreferredResource> OverridenPreferredResources
                                                , bool? isApproved, IList<DbModel.TechnicalSpecialist> dbTsInfos = null)
        {
            string tsNames = string.Empty;
            var tsEpin = OverridenPreferredResources?.Where(x => x.TechSpecialist?.Epin != null && (isApproved == null || x.IsApproved == isApproved)).Select(x => x.TechSpecialist?.Epin.ToString()).ToList();

            if (dbTsInfos == null)
            {
                dbTsInfos = dbTsInfos ?? new List<DbModel.TechnicalSpecialist>();
            }

            if (tsEpin?.Count > 0) //Changes for Live D733
            {
                var dbTS = _technicalSpecialistRepository.Get(tsEpin);
                if (dbTS != null && dbTS.Any())
                {
                    dbTsInfos.AddRange(dbTS);
                    tsNames = string.Join(" ", dbTsInfos?.Where(x => tsEpin.Contains(x.Pin.ToString())).Select(x => string.Format("<p> {0} {1} </p>", x.FirstName, x.LastName))?.Distinct()?.ToList());
                }
            }
            return tsNames;
        }

        private void GetTechSpec(DomainModel.ResourceSearch resourceSearch,
                                 ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist)
        {
            var mainSupplierTsEpins = resourceSearch.SearchParameter?.SelectedTechSpecInfo?.Select(x => x.Epin.ToString()).ToList();
            var subSupplierTsEpin = resourceSearch.SearchParameter?.SubSupplierInfos?.Where(x => x.SelectedSubSupplierTS != null)?.SelectMany(x => x.SelectedSubSupplierTS?.Select(x1 => x1.Epin.ToString()))?.ToList();
            var epins = mainSupplierTsEpins;
            if (subSupplierTsEpin?.Count > 0)
            {
                epins = epins?.Union(subSupplierTsEpin)?.ToList();
            }
            //if (dbTechnicalSpecialist == null)
            //    dbTechnicalSpecialist = new List<DbModel.TechnicalSpecialist>();

            if (dbTechnicalSpecialist?.Count == 0)
                dbTechnicalSpecialist = _technicalSpecialistRepository.Get(epins);
        }


        private EmailQueueMessage ProcessEmailMessage(ModuleType moduleType, EmailTemplate emailTemplateType, EmailType emailType
                                                    , ModuleCodeType moduleCodeType, string moduleRefCode, string emailSubject
                                                    , IList<KeyValuePair<string, string>> emailContentPlaceholders, List<EmailAddress> toAddresses
                                                    , List<EmailAddress> ccAddresses = null, List<EmailAddress> bccAddresses = null, List<EmailAddress> fromAddresses = null)
        {
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            try
            {
                var emailTemplateContent = _emailService.GetEmailTemplate(new List<string> { emailTemplateType.ToString() })?.FirstOrDefault(x => x.KeyName == emailTemplateType.ToString())?.KeyValue;

                if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                {
                    emailContentPlaceholders.ToList().ForEach(x =>
                    {
                        emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                    });
                }

                emailMessage.FromAddresses = fromAddresses ?? new List<EmailAddress>();
                emailMessage.BccAddresses = bccAddresses ?? new List<EmailAddress>();
                emailMessage.CcAddresses = ccAddresses ?? new List<EmailAddress>();
                emailMessage.ToAddresses = toAddresses;
                emailMessage.CreatedOn = DateTime.UtcNow;
                emailMessage.EmailType = emailType.ToString();
                emailMessage.ModuleCode = moduleCodeType.ToString();
                emailMessage.ModuleEmailRefCode = moduleRefCode.ToString();
                emailMessage.Subject = emailSubject;
                emailMessage.Content = emailTemplateContent;
                emailMessage.IsMailSendAsGroup = true;//def 1163 fix
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return emailMessage;
        }

        private Response ProcessResourceSearchNotes(IList<DomainModel.ResourceSearch> resourceSearches
                                                    , IList<DbModel.ResourceSearch> dbResourceSearches, ValidationType validationType
                                                    , ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<DomainModel.ResourceSearchNote> tsNewComptNotes = new List<DomainModel.ResourceSearchNote>();
            try
            {
                if (resourceSearches?.Count == dbResourceSearches?.Count)
                {
                    if (validationType == ValidationType.Add)
                    {
                        //TODO : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                        for (int Cnt = 0; Cnt < resourceSearches?.Count; Cnt++)
                        {
                            if (resourceSearches[Cnt] != null && dbResourceSearches[Cnt] != null)
                            {
                                if (!string.IsNullOrEmpty(resourceSearches[Cnt]?.Description))
                                {
                                    tsNewComptNotes.Add(
                                        new DomainModel.ResourceSearchNote
                                        {
                                            ResourceSearchId = dbResourceSearches[Cnt].Id,
                                            Notes = resourceSearches[Cnt]?.Description,
                                            RecordStatus = RecordStatus.New.FirstChar(),
                                            CreatedBy = resourceSearches[Cnt]?.ActionByUser,
                                            CreatedOn = DateTime.UtcNow,
                                        });
                                }

                            }
                        }
                    }
                    else if (validationType == ValidationType.Update)
                    {
                        var resSearchIds = resourceSearches.Select(x => (int)x.Id).Distinct().ToList();
                        var tsResourceSearchNotes = _resourceSearchNoteService.Get(resSearchIds, true).Result?.Populate<IList<DomainModel.ResourceSearchNote>>();

                        tsNewComptNotes = resourceSearches.GroupJoin(tsResourceSearchNotes,
                             tsc => new { ID = (int)tsc.Id },
                            note => new { ID = note.ResourceSearchId },
                             (NewNote, OldNote) => new { OldNote, NewNote }).Where(x => !string.Equals(x.OldNote.FirstOrDefault()?.Notes, x.NewNote.Description)).Select(x =>
                             new DomainModel.ResourceSearchNote
                             {
                                 ResourceSearchId = (int)x.NewNote.Id,
                                 Notes = x.NewNote?.Description,
                                 RecordStatus = RecordStatus.New.FirstChar(),
                                 CreatedBy = x.NewNote.ActionByUser,
                                 CreatedOn = DateTime.UtcNow,
                             }).ToList();

                    }
                    ObjectExtension.SetPropertyValue(tsNewComptNotes, "ActionByUser", resourceSearches?.FirstOrDefault().ActionByUser);
                    ObjectExtension.SetPropertyValue(tsNewComptNotes, "EventId", resourceSearches?.FirstOrDefault().EventId);
                    ObjectExtension.SetPropertyValue(tsNewComptNotes, "ModifiedBy", resourceSearches?.FirstOrDefault().ModifiedBy);
                    return _resourceSearchNoteService.Save(tsNewComptNotes);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearches);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        private Response ProcessOverridenResources(IList<DomainModel.OverridenPreferredResource> overridenPreferredResources
                                            , int resourceSearchId
                                            , string actionByUser
                                            , ref IList<DbModel.OverrideResource> dbOverrideResources
                                            , ref IList<DbModel.TechnicalSpecialist> dbTsInfos
                                            , ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            IList<DomainModel.OverridenPreferredResource> prefResources = null;
            try
            {
                if (overridenPreferredResources?.Count > 0)
                {

                    var overrideResToInsert = overridenPreferredResources.Where(x => x.RecordStatus == RecordStatus.New.FirstChar()).Select(x =>
                             new DomainModel.OverridenPreferredResource
                             {
                                 ResourceSearchId = resourceSearchId,
                                 TechSpecialist = x.TechSpecialist,
                                 RecordStatus = RecordStatus.New.FirstChar(),
                                 CreatedBy = actionByUser,
                                 CreatedDate = DateTime.UtcNow,
                             }).ToList();

                    if (overrideResToInsert?.Count > 0)
                    {
                        var res = _overrideResourceService.Save(overrideResToInsert, out dbOverrideResources, out dbTsInfos);
                        if (res != null && res.Code != MessageType.Success.ToId())
                        {
                            return res;
                        }
                    }

                    var resIds = overridenPreferredResources.Where(x => x.RecordStatus == RecordStatus.Modify.FirstChar()).Select(x => (int)x.Id).Distinct().ToList();

                    if (resIds?.Count > 0)
                    {
                        if (dbOverrideResources == null)
                        {
                            prefResources = _overrideResourceService.Get(resIds).Result?.Populate<IList<DomainModel.OverridenPreferredResource>>();
                        }
                        else
                        {
                            prefResources = _mapper.Map<IList<DomainModel.OverridenPreferredResource>>(dbOverrideResources);
                        }

                        var overrideResToUpdate = overridenPreferredResources.Join(prefResources,
                             res => new { ID = res.Id },
                             tsc => new { ID = tsc.Id },
                             (NewOverRes, OldOverRes) => new { NewOverRes, OldOverRes }).Select(x =>
                              {
                                  x.OldOverRes.RecordStatus = RecordStatus.Modify.FirstChar();
                                  x.OldOverRes.IsApproved = x.NewOverRes.IsApproved;
                                  x.OldOverRes.ModifiedBy = actionByUser;
                                  x.OldOverRes.LastModification = DateTime.UtcNow;
                                  return x.OldOverRes;
                              }).ToList();
                        if (overrideResToUpdate?.Count > 0)
                        {
                            var res = _overrideResourceService.Modify(overrideResToUpdate, ref dbOverrideResources, out dbTsInfos);
                            if (res != null && res.Code != MessageType.Success.ToId())
                            {
                                return res;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), overridenPreferredResources);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response AssignResourceToAssignment(DomainModel.ResourceSearch resourceSearch, ref DbModel.Assignment dbAssignment, IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist, bool isDBValidationRequired = true)
        {
            IList<ValidationMessage> messages = null;
            DbModel.Assignment dbAssignt = null;
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist = null;
            List<DomainModel.BaseResourceSearchTechSpecInfo> newMainSupplierTS = null;
            List<DomainModel.BaseResourceSearchTechSpecInfo> newSubSupplierTS = null;
            Exception exception = null;
            dbTechnicalSpecialist = dbTechnicalSpecialist ?? new List<DbModel.TechnicalSpecialist>();
            try
            {
                var mainSupplierEpin = resourceSearch?.SearchParameter?.SelectedTechSpecInfo?.Select(x => x.Epin).ToList();
                var subSupplierEpin = resourceSearch?.SearchParameter?.SubSupplierInfos?.SelectMany(x => x.SelectedSubSupplierTS)?.Select(x => x.Epin).ToList();

                var epins = mainSupplierEpin?.Count > 0 && subSupplierEpin?.Count > 0 ? mainSupplierEpin.Concat(subSupplierEpin).ToList()
                            : mainSupplierEpin?.Count > 0 ? mainSupplierEpin.ToList() : subSupplierEpin.ToList();

                if (epins != null && epins?.Count > 0)
                {
                    var dbTS = _technicalSpecialistRepository.Get(epins);
                    if (dbTS != null && dbTS.Any())
                    {
                        dbTechnicalSpecialist.AddRange(dbTS);
                    }
                }

                if (dbTechnicalSpecialist?.Count > 0)
                {
                    dbAssignt = dbAssignment;
                    var assignmentTS = dbTechnicalSpecialist?.ToList()?.Select(x => new AssignmentTechnicalSpecialist
                    {
                        AssignmentId = dbAssignt?.Id,
                        Epin = x.Pin,
                        IsActive = true,
                        RecordStatus = RecordStatus.New.FirstChar()
                    }).ToList();

                    var assignedTS = dbAssignment?.AssignmentTechnicalSpecialist.Select(x => x.TechnicalSpecialist?.Pin).ToList();
                    if (assignedTS?.Count > 0)
                    {
                        assignmentTS = assignmentTS.Where(x => !assignedTS.Contains(x.Epin)).ToList();
                    }
                    IList<DbModel.Assignment> dbAssignments = new List<DbModel.Assignment> { dbAssignment };
                    ObjectExtension.SetPropertyValue(assignmentTS, "ActionByUser", resourceSearch.ActionByUser);
                    ObjectExtension.SetPropertyValue(assignmentTS, "ModifiedBy", resourceSearch.ModifiedBy);
                    ObjectExtension.SetPropertyValue(assignmentTS, "EventId", resourceSearch.EventId);
                    var response = _assignmentTechnicalSpecilaistService.Add(assignmentTS, ref dbAssignmentTechnicalSpecialist, ref dbTechnicalSpecialist, ref dbAssignments, isDbValidationRequired: false);
                    if (response.Code == ResponseType.Success.ToId() && assignmentTS?.Count> 0)
                    {
                        var newData = _mapper.Map<List<AssignmentTechnicalSpecialist>>(dbAssignment.AssignmentTechnicalSpecialist);
                        GenerateEventAndLogData(null, ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.AssignmentTechnicalSpecialist, null, newData,
                                                "{" + AuditSelectType.Id + ":" + dbAssignment?.Id + "}${" + AuditSelectType.ProjectNumber + ":"
                                                + dbAssignment?.ProjectId + "}${" + AuditSelectType.ProjectAssignment + ":"
                                                + dbAssignment?.ProjectId + "-" + dbAssignment?.AssignmentNumber + "}",
                                                 resourceSearch.ActionByUser, null, SqlAuditModuleType.Assignment.ToString()
                                                 );
                       
                    }
                    if (response.Code == ResponseType.Success.ToId())
                        response = ProcessAssignmentSubSupplierTs(dbAssignments, resourceSearch, ref newMainSupplierTS, ref newSubSupplierTS);
                    if (response.Code == ResponseType.Success.ToId())
                        ProcessVisitTimesheetTechnicalSpecialist(dbAssignments, dbTechnicalSpecialist, resourceSearch, newMainSupplierTS, newSubSupplierTS);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), null, exception);
        }

        private Response ProcessVisitTimesheetTechnicalSpecialist(IList<DbModel.Assignment> dbAssignment, IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist, DomainModel.ResourceSearch resourceSearch, List<DomainModel.BaseResourceSearchTechSpecInfo> newMainSupplierTS, List<DomainModel.BaseResourceSearchTechSpecInfo> newSubSupplierTS)
        {
            Exception exception = null;
            IList<ValidationMessage> messages = null;
            try
            {
                var visitId = dbAssignment?.ToList()?.SelectMany(x => x.Visit)?.Where(x => x.IsSkeltonVisit == true)?.FirstOrDefault()?.Id;
                List<DomainModel.BaseResourceSearchTechSpecInfo> newTS = new List<DomainModel.BaseResourceSearchTechSpecInfo>();
                if (resourceSearch.SearchParameter.FirstVisitSupplierId == resourceSearch.SearchParameter.SupplierId)
                {
                    newTS = newMainSupplierTS;
                }
                else
                {
                    var firstVisitSubSupplier = resourceSearch.SearchParameter.SubSupplierInfos?.Where(x => x.SubSupplierId == resourceSearch.SearchParameter.FirstVisitSupplierId)?.ToList();
                    if (firstVisitSubSupplier?.Count > 0)
                    {
                        var subSupplierTSids = firstVisitSubSupplier.SelectMany(x => x.SelectedSubSupplierTS)?.ToList()?.Select(x1 => x1.Epin)?.ToList();
                        var newSubSupplierResources = newSubSupplierTS?.Where(x => subSupplierTSids != null && subSupplierTSids.Contains(x.Epin))?.ToList();
                        newTS = newSubSupplierResources;
                    }
                }
                if (visitId > 0)
                    AddVisitResource(visitId, dbAssignment, dbTechnicalSpecialist, newTS, resourceSearch);
                else
                {
                    var timesheetId = dbAssignment?.ToList()?.SelectMany(x => x.Timesheet)?.Where(x => x.IsSkeletonTimesheet == true)?.FirstOrDefault()?.Id;
                    if (timesheetId > 0)
                        AddTimesheetResource(timesheetId, dbAssignment, dbTechnicalSpecialist, resourceSearch);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
                return new Response().ToPopulate(ResponseType.Exception, null, null, messages?.ToList(), null, exception);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), null, exception);
        }
        private Response AddTimesheetResource(long? timesheetId, IList<DbModel.Assignment> dbAssignment, IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist, DomainModel.ResourceSearch resourceSearch)
        {
            Exception exception = null;
            IList<ValidationMessage> messages = null;
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechSpec = null;
            try
            {
                var dbTimesheetTechnicalSpecialist = dbAssignment?.ToList()?.SelectMany(x => x.Timesheet)?.SelectMany(x1 => x1.TimesheetTechnicalSpecialist)?.ToList()?.Select(x2 => x2.TechnicalSpecialistId)?.ToList();
                if (dbTimesheetTechnicalSpecialist?.Count == 0)
                {
                    if (dbTechnicalSpecialist?.Count > 0)
                    {
                        var dbTechSpec = dbTechnicalSpecialist.Where(x => !dbTimesheetTechnicalSpecialist.Contains(x.Id)).ToList();
                        if (dbTechSpec?.Count > 0)
                        {
                            dbTimesheetTechSpec = dbTechSpec?.Select((x1, i) => new DbModel.TimesheetTechnicalSpecialist
                            {
                                TechnicalSpecialistId = x1.Id,
                                TimesheetId = (long)timesheetId,
                                UpdateCount = 0,
                            }).ToList();
                            _timesheetTechSpecRepository.Update(dbTimesheetTechSpec);
                            _timesheetTechSpecRepository.ForceSave();
                        }
                        ProcessCalendarInfo(dbAssignment, dbTechSpec, resourceSearch);
                        if (dbTimesheetTechSpec != null && dbTimesheetTechSpec?.Count > 0)
                        {
                            var newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetTechnicalSpecialist>>(dbTimesheetTechSpec);
                            string timesheetDescription = dbAssignment?.ToList()?.SelectMany(x => x.Timesheet)?.Where(x => x.IsSkeletonTimesheet == true)?.FirstOrDefault()?.TimesheetDescription;
                            int? timesheetNumber = dbAssignment?.ToList()?.SelectMany(x => x.Timesheet)?.Where(x => x.IsSkeletonTimesheet == true)?.FirstOrDefault()?.TimesheetNumber;
                            int? projectNumber = dbAssignment?.ToList()?.FirstOrDefault()?.ProjectId;
                            int? assignmentNumber = dbAssignment?.ToList()?.FirstOrDefault()?.AssignmentNumber;
                            GenerateEventAndLogData(null, ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialist, null, newData,
                                                      "{" + AuditSelectType.Id + ":" + timesheetId + "}${" +
                                                            AuditSelectType.TimesheetDescription + ":" + timesheetDescription?.Trim() + "}${" +
                                                            AuditSelectType.JobReferenceNumber + ":" + projectNumber + "-" + assignmentNumber + "-" + timesheetNumber + "}${" +
                                                            AuditSelectType.ProjectAssignment + ":" + projectNumber + "-" + assignmentNumber + "}",
                                                   resourceSearch.ActionByUser, null, SqlAuditModuleType.Timesheet.ToString()
                                                   );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
                return new Response().ToPopulate(ResponseType.Exception, null, null, messages?.ToList(), null, exception);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), null, exception);
        }

        private Response AddVisitResource(long? visitId, IList<DbModel.Assignment> dbAssignment, IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist, 
                                          List<DomainModel.BaseResourceSearchTechSpecInfo> newTS, DomainModel.ResourceSearch resourceSearch)
        {
            Exception exception = null;
            IList<ValidationMessage> messages = null;
            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechSpec = null;
            try
            {
                var dbVisitTechnicalSpecialist = dbAssignment?.ToList()?.SelectMany(x => x.Visit)?.SelectMany(x1 => x1.VisitTechnicalSpecialist)?.ToList()?.Select(x2 => x2.TechnicalSpecialistId)?.ToList();
                if (dbVisitTechnicalSpecialist?.Count == 0)
                {
                    if (newTS?.Count > 0)
                    {
                        var dbTechSpec = newTS.Where(x => !dbVisitTechnicalSpecialist.Contains(x.Epin)).Select(x => x.Epin)?.ToList();
                        var dbNewTechSpec = dbTechnicalSpecialist.Where(x => dbTechSpec.Contains(x.Id))?.ToList();
                        if (dbTechSpec?.Count > 0)
                        {
                            dbVisitTechSpec = dbTechSpec?.Select((x1, i) => new DbModel.VisitTechnicalSpecialist
                            {
                                TechnicalSpecialistId = x1,
                                VisitId = (long)visitId,
                                UpdateCount = 0,
                            }).ToList();
                            _visitTechSpecRepository.Update(dbVisitTechSpec);
                            _visitTechSpecRepository.ForceSave();
                        }
                        ProcessCalendarInfo(dbAssignment, dbNewTechSpec, resourceSearch);
                        if (dbVisitTechSpec != null && dbVisitTechSpec?.Count > 0)
                        {
                            var newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitTechnicalSpecialist>>(dbVisitTechSpec);
                            string reportNumber=dbAssignment?.ToList()?.SelectMany(x => x.Visit)?.Where(x => x.IsSkeltonVisit == true)?.FirstOrDefault()?.ReportNumber;
                            int? visitNumber = dbAssignment?.ToList()?.SelectMany(x => x.Visit)?.Where(x => x.IsSkeltonVisit == true)?.FirstOrDefault()?.VisitNumber;
                            int? projectNumber = dbAssignment?.ToList()?.FirstOrDefault()?.ProjectId;
                            int? assignmentNumber = dbAssignment?.ToList()?.FirstOrDefault()?.AssignmentNumber;
                            GenerateEventAndLogData(null, ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.VisitSpecialistAccount, null, newData,
                                                   "{" + AuditSelectType.Id + ":" + visitId + "}${" + AuditSelectType.ReportNumber + ":" + reportNumber?.Trim()
                                                   + "}${" + AuditSelectType.JobReferenceNumber + ":" + projectNumber + "-" + assignmentNumber + "-" + visitNumber
                                                   + "}${" + AuditSelectType.ProjectAssignment + ":" + projectNumber + "-" + assignmentNumber + "}",
                                                   resourceSearch.ActionByUser,null, SqlAuditModuleType.Visit.ToString()
                                                   );

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
                return new Response().ToPopulate(ResponseType.Exception, null, null, messages?.ToList(), null, exception);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), null, exception);
        }

        private Response ProcessCalendarInfo(IList<DbModel.Assignment> dbAssignment, IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist, DomainModel.ResourceSearch resourceSearch)
        {
            Exception exception = null;
            IList<ValidationMessage> messages = null;
            try
            {
                var dbCalendar = dbTechnicalSpecialist?.Select((x1, i) => new DbModel.TechnicalSpecialistCalendar
                {
                    TechnicalSpecialistId = x1.Id,
                    CompanyId = (dbAssignment?.FirstOrDefault()?.ContractCompanyId == dbAssignment?.FirstOrDefault()?.OperatingCompanyId) ? dbAssignment?.FirstOrDefault()?.ContractCompanyId : dbAssignment?.FirstOrDefault()?.OperatingCompanyId,
                    CalendarType = dbAssignment?.FirstOrDefault()?.SupplierPurchaseOrderId > 0 ? CalendarType.VISIT.ToString() : CalendarType.TIMESHEET.ToString(),
                    CalendarStatus = CalendarStatus.Confirmed.DisplayName(),
                    CalendarRefCode = dbAssignment?.ToList()?.SelectMany(x => x.Visit)?.FirstOrDefault()?.Id ?? dbAssignment?.ToList()?.SelectMany(x => x.Timesheet)?.FirstOrDefault()?.Id,
                    StartDateTime = dbAssignment?.FirstOrDefault()?.FirstVisitTimesheetStartDate?.Date.AddHours(9),
                    EndDateTime = dbAssignment?.FirstOrDefault()?.FirstVisitTimesheetEndDate?.Date.AddHours(17),
                    CreatedBy = resourceSearch.ActionByUser,
                    IsActive = true,
                    UpdateCount = 0,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

                var response = _technicalSpecialistCalendarService.Save(dbCalendar);
                if (response != null && response?.Code != MessageType.Success.ToId())
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), null, exception);
        }

        private Response ProcessAssignmentSubSupplierTs(IList<DbModel.Assignment> dbAssignment, DomainModel.ResourceSearch resourceSearch, ref List<DomainModel.BaseResourceSearchTechSpecInfo> newMainSupplierTS, ref List<DomainModel.BaseResourceSearchTechSpecInfo> newSubSupplierTS)
        {
            Exception exception = null;
            IList<ValidationMessage> messages = null;
            List<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTS = new List<DbModel.AssignmentSubSupplierTechnicalSpecialist>();
            List<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbTempAssignmentSubSupplierTS = new List<DbModel.AssignmentSubSupplierTechnicalSpecialist>();
            var dbMainSupplier = dbAssignment?.ToList()?.SelectMany(x => x.AssignmentSubSupplier)?.Where(x => x.SupplierId == resourceSearch.SearchParameter?.SupplierId && x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false)?.ToList();
            var dbSubSupplier = dbAssignment?.ToList()?.SelectMany(x => x.AssignmentSubSupplier)?.Where(x => x.SupplierId != resourceSearch.SearchParameter?.SupplierId && x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false)?.ToList();
            var dbAssignmentTechnicalSpecialist = dbAssignment?.ToList()?.SelectMany(x => x.AssignmentTechnicalSpecialist);

            if (resourceSearch.SearchParameter.SupplierId > 0 && resourceSearch.SearchParameter.SelectedTechSpecInfo?.Count > 0 && dbMainSupplier?.Count > 0)
            {
                // Added for Duplicate check and updating the main supplier data
                var dbMainSupplierEpin = dbMainSupplier?.SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist)?.Select(x => x.TechnicalSpecialist?.TechnicalSpecialist?.Pin).ToList();
                newMainSupplierTS = resourceSearch?.SearchParameter?.SelectedTechSpecInfo?.Where(x => !dbMainSupplierEpin.Contains(x.Epin))?.Select(x => x)?.ToList();
                dbAssignmentSubSupplierTS = newMainSupplierTS.Select(x1 => new DbModel.AssignmentSubSupplierTechnicalSpecialist
                {
                    AssignmentSubSupplierId = (int)dbMainSupplier?.ToList().FirstOrDefault(x2 => x2.SupplierId == resourceSearch.SearchParameter.SupplierId)?.Id,
                    TechnicalSpecialistId = (int)dbAssignmentTechnicalSpecialist?.ToList().FirstOrDefault(x2 => x2.TechnicalSpecialistId == x1.Epin)?.Id,
                    UpdateCount = 0,
                    IsDeleted = false
                })?.ToList();
            }

            if (resourceSearch.SearchParameter.SubSupplierInfos?.Count > 0)
            {
                var subSupplierTs = resourceSearch.SearchParameter.SubSupplierInfos.SelectMany(x => x.SelectedSubSupplierTS)?.ToList();

                if (subSupplierTs?.Count > 0 && dbSubSupplier?.Count > 0)
                {
                    foreach (var subSupplier in resourceSearch.SearchParameter.SubSupplierInfos)
                    {
                        if (subSupplier.SelectedSubSupplierTS?.Any() == true)
                        {
                            // Added for Duplicate check and updating the sub supplier data
                            var dbSubSupplierEpin = dbSubSupplier?.Where(x => x.SupplierId == subSupplier.SubSupplierId)?.SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist)?.Select(x => x.TechnicalSpecialist.TechnicalSpecialist.Pin)?.ToList();
                            newSubSupplierTS = subSupplier?.SelectedSubSupplierTS?.Where(x => !dbSubSupplierEpin.Contains(x.Epin))?.Select(x => x)?.ToList();
                            var assignmentSubSupplierTS = newSubSupplierTS.Select(x1 => new DbModel.AssignmentSubSupplierTechnicalSpecialist
                            {
                                AssignmentSubSupplierId = (int)dbSubSupplier?.ToList().FirstOrDefault(x2 => x2.SupplierId == subSupplier.SubSupplierId)?.Id,
                                TechnicalSpecialistId = (int)dbAssignmentTechnicalSpecialist?.ToList().FirstOrDefault(x2 => x2.TechnicalSpecialistId == x1.Epin)?.Id,
                                UpdateCount = 0,
                                IsDeleted = false
                            })?.ToList();

                            dbTempAssignmentSubSupplierTS = dbTempAssignmentSubSupplierTS.Union(assignmentSubSupplierTS).ToList();
                        }
                    }
                }
            }

            dbAssignmentSubSupplierTS = dbAssignmentSubSupplierTS?.Count > 0 && dbTempAssignmentSubSupplierTS?.Count > 0 ? dbAssignmentSubSupplierTS.Concat(dbTempAssignmentSubSupplierTS).ToList()
                            : dbAssignmentSubSupplierTS?.ToList()?.Count > 0 ? dbAssignmentSubSupplierTS.ToList() : dbTempAssignmentSubSupplierTS.ToList();

            if (dbAssignmentSubSupplierTS?.Count > 0)
            {
                _assignmentSubSupplerTSRepository.Add(dbAssignmentSubSupplierTS);
                _assignmentSubSupplerTSRepository.ForceSave();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), null, exception);
        }

        private Response AssignResourceSearchToAssignment(int? resourceSearchId, DomainModel.ResourceSearch resource, ref DbModel.Assignment dbAssignment, bool isDBValidationRequire)
        {
            if (dbAssignment != null && dbAssignment?.ResourceSearchId != resourceSearchId)
            {
                dbAssignment.ResourceSearchId = resourceSearchId;
                _assignmentRepository.Update(dbAssignment, x => x.ResourceSearchId);
                //_assignmentRepository.Update();
            }
            return null;
        }

        private IList<UserInfo> GetUserInfo(DomainModel.ResourceSearch resourceSearch, EmailTemplate emailTemplateType)
        {
            IList<UserInfo> userInfos = null;
            List<string> users = new List<string>
                    {
                       resourceSearch?.SearchParameter.CHCoordinatorLogOnName,
                       resourceSearch?.SearchParameter.OPCoordinatorLogOnName,
                    };

            if (emailTemplateType == EmailTemplate.EmailResourceOverrideApproval || emailTemplateType == EmailTemplate.EmailPreferredResourceRejected || emailTemplateType == EmailTemplate.EmailPreferredResourceApproved)
            {
                users.Add(resourceSearch?.AssignedToOmLognName);
            }
            if ((emailTemplateType == EmailTemplate.EmailPotentialResourceFound || emailTemplateType == EmailTemplate.EmailPotentialResourceNotFound) && !string.IsNullOrEmpty(resourceSearch?.AssignedBy))
            {
                users.Add(resourceSearch?.AssignedBy);
            }
            if (emailTemplateType == EmailTemplate.EmailResourceDispositionStatusChange || emailTemplateType == EmailTemplate.EmailSearchAssistanceRequested || emailTemplateType == EmailTemplate.EmailSearchDisposition)
            {
                List<string> userTypes = new List<string> { ResourceSearchConstants.User_Type_RC, ResourceSearchConstants.User_Type_RM };
                var userUserTypeData = _userService.GetUsersByTypeAndCompany(resourceSearch?.CompanyCode, userTypes);
                var toUsers = userUserTypeData?.Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();

                userInfos = _mapper.Map<IList<UserInfo>>(toUsers)?.Populate<IList<UserInfo>>();
            }

            if (users?.Count > 0)
            {
                userInfos = userInfos ?? new List<UserInfo>();
                var usrInfos = _userService.Get(users).Result.Populate<IList<UserInfo>>();
                if (usrInfos != null && usrInfos.Count > 0)
                    userInfos.AddRange(usrInfos);
            }
            return userInfos.GroupBy(x => x.LogonName).Select(x => x.First()).ToList();
        }

        private Response ProcessTsCalanderInfo(int resourceSearchId, DomainModel.ResourceSearch resource)
        {
            Response response = null;
            if (resource != null)
            {
                var resCal = _technicalSpecialistCalendarService.Get(new TechnicalSpecialistCalendar { CalendarRefCode = resourceSearchId, CalendarType = CalendarType.PRE.ToString(), IsActive = true }, false).Result.Populate<IList<TechnicalSpecialistCalendar>>();

                List<int> preAssignmentTsIds = new List<int>();
                var newTsIds = resource.SearchParameter.SelectedTechSpecInfo?.Select(x => x.Epin).ToList();
                var newSubSuplierTsIds = resource.SearchParameter.SubSupplierInfos.Where(x => x.SelectedSubSupplierTS?.Count > 0).SelectMany(x => x.SelectedSubSupplierTS).Select(x => x.Epin).ToList();

                if (newTsIds?.Count > 0)
                {
                    preAssignmentTsIds.AddRange(newTsIds);
                }
                if (newSubSuplierTsIds?.Count > 0)
                {
                    preAssignmentTsIds.AddRange(newSubSuplierTsIds);
                }
                preAssignmentTsIds = preAssignmentTsIds.Distinct().ToList();

                if (resource.SearchAction.ToEnum<ResourceSearchAction>() == ResourceSearchAction.L)
                {
                    var calToDelete = resCal?.Where(x => x.CalendarRefCode == resourceSearchId && preAssignmentTsIds.Contains(x.TechnicalSpecialistId))
                                            ?.Select(x =>
                                                    {
                                                        x.RecordStatus = RecordStatus.Delete.FirstChar();
                                                        x.LastModification = DateTime.UtcNow;
                                                        x.ModifiedBy = resource?.ActionByUser;
                                                        return x;
                                                    }).ToList();

                    if (calToDelete != null && calToDelete.Count > 0)
                    {
                        response = DeleteCalendarInfo(calToDelete);
                        if (response != null && response?.Code != MessageType.Success.ToId())
                        {
                            return response;
                        }
                    }
                }
                if (resource.SearchAction.ToEnum<ResourceSearchAction>() == ResourceSearchAction.W || resource.SearchAction.ToEnum<ResourceSearchAction>() == ResourceSearchAction.CUP)
                {

                    var calToDelete = resCal?.Where(x => x.CalendarRefCode == resourceSearchId && !preAssignmentTsIds.Contains(x.TechnicalSpecialistId))
                                         ?.Select(x =>
                                         {
                                             x.RecordStatus = RecordStatus.Delete.FirstChar();
                                             x.LastModification = DateTime.UtcNow;
                                             x.ModifiedBy = resource?.ActionByUser;
                                             return x;
                                         }).ToList();
                    if (calToDelete != null && calToDelete.Count > 0)
                    {
                        response = DeleteCalendarInfo(calToDelete);
                        if (response != null && response?.Code != MessageType.Success.ToId())
                        {
                            return response;
                        }
                    }

                    response = UpdateCalendarInfo(resourceSearchId, resource, resCal, preAssignmentTsIds);
                    if (response != null && response?.Code != MessageType.Success.ToId())
                    {
                        return response;
                    }

                    response = InsertCalendarInfo(resourceSearchId, resource, resCal, preAssignmentTsIds);
                    if (response != null && response?.Code != MessageType.Success.ToId())
                    {
                        return response;
                    }
                }

            }
            return response;
        }

        private Response InsertCalendarInfo(int resourceSearchId, DomainModel.ResourceSearch resource, IList<TechnicalSpecialistCalendar> resourceCalendar, List<int> selectedEpins)
        {
            Response response = null;
            List<int> newTsIds = null;
            List<TechnicalSpecialistCalendar> newCalendarData = new List<TechnicalSpecialistCalendar>();
            if (resource != null)
            {
                if (resourceCalendar?.Count > 0 && selectedEpins?.Count > 0)
                {
                    newTsIds = selectedEpins?.Where(x => !resourceCalendar.Where(x1 => x1.CalendarRefCode == resourceSearchId).Select(x1 => x1.TechnicalSpecialistId).Contains(x))
                                            .Select(x => x).Distinct()
                                            .ToList();
                }
                else
                {
                    newTsIds = selectedEpins;
                }
                var calToInsert = resource.SearchParameter.SelectedTechSpecInfo?.Where(x => newTsIds.Contains(x.Epin))
                                                                                .Select(x => new TechnicalSpecialistCalendar
                                                                                {
                                                                                    TechnicalSpecialistId = x.Epin,
                                                                                    CompanyCode = resource?.CompanyCode,
                                                                                    CalendarType = CalendarType.PRE.ToString(),
                                                                                    CalendarRefCode = resourceSearchId,
                                                                                    CalendarStatus = CalendarStatus.PREASGMNT.ToString(),
                                                                                    StartDateTime = (resource?.SearchParameter?.FirstVisitFromDate ?? DateTime.UtcNow.Date).AddHours(9),
                                                                                    EndDateTime = (resource?.SearchParameter?.FirstVisitToDate != null ? Convert.ToDateTime(resource.SearchParameter.FirstVisitToDate) : (resource?.SearchParameter?.FirstVisitFromDate == null && resource?.SearchParameter?.FirstVisitToDate == null ? DateTime.UtcNow.Date : (resource?.SearchParameter?.FirstVisitToDate == null && resource?.SearchParameter?.FirstVisitFromDate != null ? Convert.ToDateTime(resource.SearchParameter.FirstVisitFromDate) : DateTime.UtcNow.Date))).AddHours(17), //def 1257 fix : Whenever user insert future start date and null enddate . inserting current date as enddate is invalid as this wont be listed any where in calender and resource search in ARS ,quick search ans pre-assignment search. So changed enddate to start date when user enterd enddate is null;
                                                                                    CreatedBy = resource?.ActionByUser,
                                                                                    IsActive = true,
                                                                                    RecordStatus = RecordStatus.New.FirstChar(),
                                                                                }).ToList();

                var subSupCalToInsert = resource.SearchParameter.SubSupplierInfos?.Where(x => x.SelectedSubSupplierTS?.Count > 0).SelectMany(x => x.SelectedSubSupplierTS)?.Where(x => newTsIds.Contains(x.Epin))
                                                                                    .Select(x => new TechnicalSpecialistCalendar
                                                                                    {
                                                                                        TechnicalSpecialistId = x.Epin,
                                                                                        CompanyCode = resource?.CompanyCode,
                                                                                        CalendarType = CalendarType.PRE.ToString(),
                                                                                        CalendarRefCode = resourceSearchId,
                                                                                        CalendarStatus = CalendarStatus.PREASGMNT.ToString(),
                                                                                        StartDateTime = (resource?.SearchParameter?.FirstVisitFromDate ?? DateTime.UtcNow.Date).AddHours(9),
                                                                                        EndDateTime = (resource?.SearchParameter?.FirstVisitToDate != null ? Convert.ToDateTime(resource.SearchParameter.FirstVisitToDate) : (resource?.SearchParameter?.FirstVisitFromDate == null && resource?.SearchParameter?.FirstVisitToDate == null ? DateTime.UtcNow.Date : (resource?.SearchParameter?.FirstVisitToDate == null && resource?.SearchParameter?.FirstVisitFromDate != null ? Convert.ToDateTime(resource.SearchParameter.FirstVisitFromDate) : DateTime.UtcNow.Date))).AddHours(17), //def 1257 fix : Whenever user insert future start date and null enddate . inserting current date as enddate is invalid as this wont be listed any where in calender and resource search in ARS ,quick search ans pre-assignment search. So changed enddate to start date when user enterd enddate is null;
                                                                                        CreatedBy = resource?.ActionByUser,
                                                                                        IsActive = true,
                                                                                        RecordStatus = RecordStatus.New.FirstChar(),
                                                                                    }).ToList();

                if (calToInsert?.Count > 0)
                {
                    newCalendarData.AddRange(calToInsert);
                }
                if (subSupCalToInsert?.Count > 0)
                {
                    newCalendarData.AddRange(subSupCalToInsert);
                }

                if (newCalendarData != null && newCalendarData.Count > 0)
                {
                    response = _technicalSpecialistCalendarService.Save(newCalendarData);
                    if (response != null && response?.Code != MessageType.Success.ToId())
                    {
                        return response;
                    }
                }
            }
            return response;
        }

        private Response UpdateCalendarInfo(int resourceSearchId, DomainModel.ResourceSearch resource, IList<TechnicalSpecialistCalendar> resourceCalendar, List<int> selectedEpins)
        {
            Response response = null;
            if (resource != null)
            {
                var calToupdate = resourceCalendar?.Where(x => x.CalendarRefCode == resourceSearchId && selectedEpins.Contains(x.TechnicalSpecialistId))
                                           ?.Select(x =>
                                           {
                                               x.StartDateTime = (resource?.SearchParameter?.FirstVisitFromDate ?? DateTime.UtcNow.Date).AddHours(9);
                                               x.EndDateTime = (resource?.SearchParameter?.FirstVisitToDate != null ? Convert.ToDateTime(resource.SearchParameter.FirstVisitToDate) : (resource?.SearchParameter?.FirstVisitFromDate == null && resource?.SearchParameter?.FirstVisitToDate == null ? DateTime.UtcNow.Date : (resource?.SearchParameter?.FirstVisitToDate == null && resource?.SearchParameter?.FirstVisitFromDate != null ? Convert.ToDateTime(resource.SearchParameter.FirstVisitFromDate) : DateTime.UtcNow.Date))).AddHours(17); //def 1257 fix : Whenever user insert future start date and null enddate . inserting current date as enddate is invalid as this wont be listed any where in calender and resource search in ARS ,quick search ans pre-assignment search. So changed enddate to start date when user enterd enddate is null;
                                               x.RecordStatus = RecordStatus.Modify.FirstChar();
                                               x.CompanyCode = resource?.CompanyCode;
                                               x.LastModification = DateTime.UtcNow;
                                               x.ModifiedBy = resource?.ActionByUser;
                                               return x;
                                           }).ToList();


                if (calToupdate != null && calToupdate.Count > 0)
                {
                    response = _technicalSpecialistCalendarService.Update(calToupdate);
                    if (response != null && response?.Code != MessageType.Success.ToId())
                    {
                        return response;
                    }
                }
            }
            return response;
        }

        private Response DeleteCalendarInfo(List<TechnicalSpecialistCalendar> calToDelete)
        {
            Response response = null;
            if (calToDelete != null && calToDelete.Count > 0)
            {
                response = _technicalSpecialistCalendarService.Delete(calToDelete);
                if (response != null && response?.Code != MessageType.Success.ToId())
                {
                    return response;
                }
            }
            return response;
        }

        private Response AuditLog(DomainModel.ResourceSearch resourceSearch,
                           SqlAuditActionType sqlAuditActionType,
                           SqlAuditModuleType sqlAuditModuleType,
                           object oldData,
                           object newData,
                           string searchRef,
                           ref long? eventId,
                           IList<DbModel.SqlauditModule> dbModule,
                           string strModule)
        {

            Exception exception = null;
            if (resourceSearch != null && !string.IsNullOrEmpty(resourceSearch.ActionByUser))
            {
                string actionBy = resourceSearch.ActionByUser;
                if (resourceSearch.EventId > 0)
                    eventId = resourceSearch.EventId;

                GenerateEventAndLogData(eventId, sqlAuditActionType, sqlAuditModuleType, oldData, newData, searchRef, actionBy, dbModule, strModule);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void GenerateEventAndLogData(long? eventId,
                                   SqlAuditActionType sqlAuditActionType,
                                   SqlAuditModuleType sqlAuditModuleType,
                                   object oldData,
                                   object newData,
                                   string searchRef,
                                   string actionBy,
                                   IList<DbModel.SqlauditModule> dbModule,
                                   string strModule)
        {
            LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
            if ((eventId == null || eventId < 0) && !string.IsNullOrEmpty(actionBy))
                eventId = logEventGeneration.GetEventLogId(eventId,
                                                              sqlAuditActionType,
                                                              actionBy,
                                                              searchRef,
                                                              strModule,
                                                              dbModule);
            if(eventId > 0)
             _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData, dbModule);
        }

        #endregion

    }
}
