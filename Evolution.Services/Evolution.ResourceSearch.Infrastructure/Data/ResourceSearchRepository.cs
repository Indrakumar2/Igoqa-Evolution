using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using System.Linq.Expressions;
using Evolution.Logging.Interfaces;

namespace Evolution.ResourceSearch.Infrastructure.Data
{
    public class ResourceSearchRepository : GenericRepository<DbModel.ResourceSearch>, IResourceSearchRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IResourceSearchNoteRepository _resourceSearchNoteRepository = null;
        private readonly IAppLogger<ResourceSearchRepository> _logger = null;

        public ResourceSearchRepository(EvolutionSqlDbContext dbContext, IMapper mapper, IResourceSearchNoteRepository resourceSearchNoteRepository, IAppLogger<ResourceSearchRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._resourceSearchNoteRepository = resourceSearchNoteRepository;
            _logger = logger;
        }

        public IList<DomainModel.ResourceSearchResult> Search(DomainModel.BaseResourceSearch searchModel)
        {
            IList<DomainModel.ResourceSearchResult> resourceSearchResults = null;

            var dbSearchModel = this._mapper.Map<DbModel.ResourceSearch>(searchModel);

            IQueryable<DbModel.ResourceSearch> whereClause = _dbContext.ResourceSearch;

            if (!string.IsNullOrEmpty(searchModel.CompanyCode))
                whereClause = whereClause.Where(x => x.Company.Code == searchModel.CompanyCode);

            if (!string.IsNullOrEmpty(searchModel.CustomerCode))
                whereClause = whereClause.Where(x => x.Customer.Code == searchModel.CustomerCode);

            if (!string.IsNullOrEmpty(searchModel.SearchAction))
                whereClause = whereClause.Where(x => x.ActionStatus == searchModel.SearchAction);

            if (!string.IsNullOrEmpty(searchModel.SearchType))
                whereClause = whereClause.Where(x => x.SearchType == searchModel.SearchType);

            if (!string.IsNullOrEmpty(searchModel.CategoryName))
                whereClause = whereClause.Where(x => x.Category.Name == searchModel.CategoryName);

            if (!string.IsNullOrEmpty(searchModel.SubCategoryName))
                whereClause = whereClause.Where(x => x.SubCategory.TaxonomySubCategoryName == searchModel.SubCategoryName);

            if (!string.IsNullOrEmpty(searchModel.ServiceName))
                whereClause = whereClause.Where(x => x.Service.TaxonomyServiceName == searchModel.ServiceName);

            if (!string.IsNullOrEmpty(searchModel.AssignedTo))
                whereClause = whereClause.Where(x => x.AssignedTo == searchModel.AssignedTo);

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                resourceSearchResults = whereClause.ProjectTo<DomainModel.ResourceSearchResult>().ToList();
            else
                resourceSearchResults = whereClause.Where(expression).ProjectTo<DomainModel.ResourceSearchResult>().ToList();


            if (!string.IsNullOrEmpty(searchModel.MainSupplierName) && resourceSearchResults?.Count > 0)
            {
                var supResIds = resourceSearchResults.Select(x => new
                {
                    id = x.Id,
                    searchParam = x.SearchParameter.DeSerialize<ResourceSearchParameter>(SerializationType.JSON)
                }).Where(x => x.searchParam?.Supplier?.Trim() == searchModel.MainSupplierName?.Trim()).Select(x => x.id).ToList();

                resourceSearchResults = resourceSearchResults.Where(x => supResIds.Contains(x.Id)).ToList();
            }

            var resourceSearchIds = resourceSearchResults?.Select(x => (int)x.Id)?.Distinct().ToList();

            if (resourceSearchIds?.Count > 0)
            {
                var resNotes = this._resourceSearchNoteRepository.Get(resourceSearchIds, true);

                resourceSearchResults = resourceSearchResults.GroupJoin(resNotes,
                    res => new { ID = (int)res.Id },
                    resNote => new { ID = resNote.ResourceSearchId },
                    (res, resNote) => new { res, resNote }).Select(x =>
                    {
                        x.res.Description = x.resNote.FirstOrDefault()?.Notes;
                        return x.res;
                    }).ToList();
            }


            return resourceSearchResults;

        }

        public IList<DomainModel.ResourceSearchResult> Search(IList<KeyValuePair<string, IList<string>>> mySearch, string assignedTo, string companyCode, bool isAllCoordinator)
        {
            IList<DomainModel.ResourceSearchResult> resourceSearchResults = null;
            IQueryable<DbModel.ResourceSearch> whereClause = _dbContext.ResourceSearch;
            var filterExpressions = new List<Expression<Func<DbModel.ResourceSearch, bool>>>();
            Expression<Func<DbModel.ResourceSearch, bool>> predicate = null;
            Expression<Func<DbModel.ResourceSearch, bool>> containsExpression = null;

            string[] includes = new string[] {
            "Category",
             "Service",
             "SubCategory",
             "Company",
             "Customer",
             "OverrideResource",
             "OverrideResource.TechnicalSpecialist" };

            // if (mySearch?.Count > 0)
            // {
            //     whereClause = whereClause.Where(x => mySearch.Any(x1 => x1.Key == x.SearchType && x1.Value.Contains(x.ActionStatus)));
            // }

            if (mySearch?.Count > 0)
            {
                foreach (var search in mySearch)
                {
                    containsExpression = a => a.SearchType == search.Key && search.Value.Contains(a.ActionStatus);
                    filterExpressions.Add(containsExpression);
                }
                predicate = filterExpressions.CombinePredicates<DbModel.ResourceSearch>(Expression.OrElse);
            }

            if (!string.IsNullOrEmpty(companyCode))
            {
                var chCompanyCode = string.Format(@"""chCompanyCode"":""{0}"",", companyCode);
                var opCompanyCode = string.Format(@"""opCompanyCode"":""{0}"",", companyCode);
                //whereClause = whereClause.Where(x => x.SerilizableObject.Contains(chCompanyCode) || x.SerilizableObject.Contains(opCompanyCode));

                if (predicate != null)
                {
                    containsExpression = a => a.SerilizableObject.Contains(chCompanyCode) || a.SerilizableObject.Contains(opCompanyCode);
                    predicate = predicate.CombineWithAndAlso(containsExpression);
                }
                else
                {
                    predicate = a => a.SerilizableObject.Contains(chCompanyCode) || a.SerilizableObject.Contains(opCompanyCode);
                }
            }

            whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));
            // resourceSearchResults =  whereClause.ProjectTo<DomainModel.ResourceSearchResult>().ToList();
            resourceSearchResults = whereClause.Where(predicate).ProjectTo<DomainModel.ResourceSearchResult>().ToList();

            if (!string.IsNullOrEmpty(assignedTo) && !isAllCoordinator)
            {
                var resourceQuickSearchResults = resourceSearchResults.Where(x => x.AssignedTo == assignedTo && x.CompanyCode == companyCode && x.SearchType == ResourceSearchType.Quick.ToString()).ToList();
                var resourcePreSearchResults = resourceSearchResults.Where(x => ((x.SearchParameter.Contains(string.Format(@"""chCoordinatorLogOnName"":""{0}"",", assignedTo)) && x.SearchParameter.Contains(string.Format(@"""chCompanyCode"":""{0}"",", companyCode)))
                                                                                || (x.SearchParameter.Contains(string.Format(@"""opCoordinatorLogOnName"":""{0}"",", assignedTo)) && x.SearchParameter.Contains(string.Format(@"""opCompanyCode"":""{0}"",", companyCode)))) && x.SearchType == ResourceSearchType.PreAssignment.ToString()).ToList();
                resourceSearchResults = new List<DomainModel.ResourceSearchResult>();
                resourceSearchResults.AddRange(resourceQuickSearchResults);
                resourceSearchResults.AddRange(resourcePreSearchResults);
            }
            if (isAllCoordinator)
            {
                var resourceQuickSearchResults = resourceSearchResults.Where(x => x.CompanyCode == companyCode && x.SearchType == ResourceSearchType.Quick.ToString()).ToList();
                var resourcePreSearchResults = resourceSearchResults.Where(x => x.SearchType == ResourceSearchType.PreAssignment.ToString()).ToList();

                resourceSearchResults = new List<DomainModel.ResourceSearchResult>();
                resourceSearchResults.AddRange(resourceQuickSearchResults);
                resourceSearchResults.AddRange(resourcePreSearchResults);

            }

            var resourceSearchIds = resourceSearchResults?.Select(x => (int)x.Id)?.Distinct().ToList();

            if (resourceSearchIds?.Count > 0)
            {
                var resNotes = this._resourceSearchNoteRepository.Get(resourceSearchIds, true);

                resourceSearchResults = resourceSearchResults.GroupJoin(resNotes,
                    res => new { ID = (int)res.Id },
                    resNote => new { ID = resNote.ResourceSearchId },
                    (res, resNote) => new { res, resNote }).Select(x =>
                    {
                        x.res.Description = x.resNote.FirstOrDefault()?.Notes;
                        return x.res;
                    }).ToList();
            }

            return resourceSearchResults;

        }

        public IList<ResourceSearchTechSpecInfo> SearchTechSpech(DomainModel.ResourceSearch searchModel)
        {
            bool isFilterTaxonomy = true;
            IList<int> epins = null;
            IEnumerable<int> profSearchTsIds = null;
            IEnumerable<DbModel.TechnicalSpecialist> tsSearchResult = null;

            var fromDate = searchModel?.SearchParameter.FirstVisitFromDate ?? DateTime.UtcNow;
            var toDate = searchModel?.SearchParameter.FirstVisitToDate ?? DateTime.UtcNow;

            var tsResult = _dbContext.TechnicalSpecialist.Where(x => x.Company.Code == searchModel.SearchParameter.OPCompanyCode);
            //var overrideResource = tsResult;

            tsResult = tsResult.Include(x => x.EmploymentType)
                       .Include(x => x.ProfileStatus)
                       .Include(x => x.SubDivision);

            // Added for PLO scenario
            var mainSupplierEpin = searchModel?.SearchParameter?.SelectedTechSpecInfo?.Select(x => x.Epin).ToList();
            var subSupplierEpin = searchModel?.SearchParameter?.SubSupplierInfos?.Where(x => x.SelectedSubSupplierTS != null)?.SelectMany(x => x.SelectedSubSupplierTS)?.Select(x => x.Epin).ToList();

            epins = (mainSupplierEpin?.Count > 0 && subSupplierEpin?.Count > 0) ? mainSupplierEpin.Concat(subSupplierEpin).ToList()
                        : mainSupplierEpin?.Count > 0 ? mainSupplierEpin?.ToList() : subSupplierEpin?.ToList();

            //epins = searchModel?.SearchParameter?.SelectedTechSpecInfo?.Select(x => x.Epin)?.ToList();

            //PLO taxonomy filter
            if (searchModel?.SearchParameter?.PLOTaxonomyInfo != null && !string.IsNullOrEmpty(searchModel?.SearchParameter?.PLOTaxonomyInfo?.CategoryName) && !string.IsNullOrEmpty(searchModel?.SearchParameter?.PLOTaxonomyInfo?.SubCategoryName) && !string.IsNullOrEmpty(searchModel?.SearchParameter?.PLOTaxonomyInfo?.ServiceName))
            {
                isFilterTaxonomy = false;
                if (searchModel?.SearchAction == ResourceSearchAction.SSSPC.ToString())
                {
                    if (epins?.Count > 0)
                    {
                        tsResult = tsResult.Where(x => epins.Contains(x.Pin));
                    }
                }

                tsResult = tsResult.Join(_dbContext.TechnicalSpecialistTaxonomy,
                                                             x1 => x1.Id,
                                                             y1 => y1.TechnicalSpecialistId,
                                                             (ts, tsTax) => new { ts, tsTax })
                                                     .Where(x => x.tsTax.TaxonomyCategory.Name == searchModel.SearchParameter.PLOTaxonomyInfo.CategoryName
                                                          && x.tsTax.TaxonomySubCategory.TaxonomySubCategoryName == searchModel.SearchParameter.PLOTaxonomyInfo.SubCategoryName
                                                          && x.tsTax.TaxonomyServices.TaxonomyServiceName == searchModel.SearchParameter.PLOTaxonomyInfo.ServiceName
                                                          && x.tsTax.ApprovalStatus == ResourceSearchConstants.TSTaxonomy_ApprovalStatus_Approve
                                                          && x.tsTax.FromDate <= DateTime.UtcNow
                                                          && (!x.tsTax.ToDate.HasValue || x.tsTax.ToDate > DateTime.UtcNow)
                                                        )
                                                    .Select(x => x.ts);

            }
            
            // Defect ITK 520   
            if (searchModel?.SearchType == ResourceSearchType.ARS.ToString() || searchModel?.SearchType == ResourceSearchType.PreAssignment.ToString())
            {
                tsResult = tsResult.Where(x => x.ProfileStatus.Name.ToLower() == ResourceSearchConstants.TS_Profile_Status_Active.ToLower()
                                                && x.EmploymentType.Name.ToLower() != ResourceSearchConstants.Employment_Type_Former.ToLower());
            }
            
            //Override issues - D1497
            if (searchModel?.TaskType == ResourceSearchConstants.Task_Type_OM_Approve_Reject_Resource && searchModel?.OverridenPreferredResources?.Count > 0)
            {
                if (searchModel.OverridenPreferredResources.Any(x => x.IsApproved.HasValue && x.IsApproved == true))
                {
                    epins = searchModel?.OverridenPreferredResources.Where(x => x.IsApproved.HasValue && x.IsApproved == true).Select(x => x.TechSpecialist.Epin).ToList();
                    if (epins?.Count > 0)
                    {
                        isFilterTaxonomy = false;
                        tsResult = tsResult.Where(x => epins.Contains(x.Pin));
                    }
                }
            }

            if (isFilterTaxonomy && !string.IsNullOrEmpty(searchModel?.CategoryName) && !string.IsNullOrEmpty(searchModel?.SubCategoryName) && !string.IsNullOrEmpty(searchModel?.ServiceName))
            {
                tsResult = tsResult.Join(_dbContext.TechnicalSpecialistTaxonomy,
                  x1 => x1.Id,
                  y1 => y1.TechnicalSpecialistId,
                  (ts, tsTax) => new { ts, tsTax })
                  .Where(x => x.tsTax.TaxonomyCategory.Name == searchModel.CategoryName
                       && x.tsTax.TaxonomySubCategory.TaxonomySubCategoryName == searchModel.SubCategoryName
                       && x.tsTax.TaxonomyServices.TaxonomyServiceName == searchModel.ServiceName
                       && x.tsTax.ApprovalStatus == ResourceSearchConstants.TSTaxonomy_ApprovalStatus_Approve
                       && x.tsTax.FromDate <= DateTime.UtcNow
                       && (!x.tsTax.ToDate.HasValue || x.tsTax.ToDate > DateTime.UtcNow)
                     ).Select(x => x.ts);
            }

            if (this.IsOperationSearchObjectLoaded(searchModel))
            {
                //filter based on optional parameter 

                if (searchModel?.SearchParameter?.OptionalSearch?.Certification?.Count > 0)
                {

                    var certAndTrain = _dbContext.TechnicalSpecialistCertificationAndTraining.Where(x => x.RecordType == CompCertTrainingType.Ce.ToString())
                       .Join(_dbContext.Data,
                       tscert => new { Id = tscert.CertificationAndTrainingId },
                       dt => new { dt.Id },
                       (tscert, dt) => new
                       {
                           tscert.TechnicalSpecialistId,
                           tscert.EffeciveDate,
                           tscert.ExpiryDate,
                           dt.Name
                       }
                       ).Where(x => searchModel.SearchParameter.OptionalSearch.Certification.Contains(x.Name)
                                && (searchModel.SearchParameter.OptionalSearch.CertificationExpiryFrom == null || x.ExpiryDate >= searchModel.SearchParameter.OptionalSearch.CertificationExpiryFrom)
                                && (searchModel.SearchParameter.OptionalSearch.CertificationExpiryTo == null || x.ExpiryDate <= searchModel.SearchParameter.OptionalSearch.CertificationExpiryTo));

                    tsResult = tsResult.GroupJoin(certAndTrain,
                                              x1 => x1.Id,
                                              y1 => y1.TechnicalSpecialistId,
                                              (ts, tsCert) => new { ts, tsCert })
                                              .Where(x1 => !searchModel.SearchParameter.OptionalSearch.Certification.Except(x1.tsCert.Select(x2 => x2.Name)).Any())
                                             .Select(x => x.ts);
                }
                IList<string> langParm = new List<string>();

                if (searchModel?.SearchParameter?.OptionalSearch?.LanguageSpeaking?.Count > 0)
                {
                    langParm.AddRange(searchModel?.SearchParameter?.OptionalSearch?.LanguageSpeaking);
                }
                if (searchModel?.SearchParameter?.OptionalSearch?.LanguageWriting?.Count > 0)
                {
                    langParm.AddRange(searchModel?.SearchParameter?.OptionalSearch?.LanguageWriting);
                }
                if (searchModel?.SearchParameter?.OptionalSearch?.LanguageComprehension?.Count > 0)
                {
                    langParm.AddRange(searchModel?.SearchParameter?.OptionalSearch?.LanguageComprehension);
                }

                if (langParm?.Count > 0)
                {
                    langParm = langParm.Distinct().ToList();

                    var languages = _dbContext.TechnicalSpecialistLanguageCapability
                       .Join(_dbContext.Data,
                       tslan => new { Id = tslan.LanguageId },
                       dt => new { dt.Id },
                       (tseq, dt) => new
                       {
                           tseq.TechnicalSpecialistId,
                           dt.Name
                       }
                       ).Where(x => langParm.Contains(x.Name));

                    tsResult = tsResult.GroupJoin(languages,
                                         x1 => x1.Id,
                                         y1 => y1.TechnicalSpecialistId,
                                         (ts, tsLang) => new { ts, tsLang })
                                         .Where(x1 => !langParm.Except(x1.tsLang.Select(x2 => x2.Name)).Any())
                                         .Select(x => x.ts);
                }

                IList<string> eqipParm = new List<string>();

                if (searchModel?.SearchParameter?.OptionalSearch?.EquipmentMaterialDescription?.Count > 0)
                {
                    eqipParm.AddRange(searchModel?.SearchParameter?.OptionalSearch?.EquipmentMaterialDescription);
                }

                if (eqipParm?.Count > 0)
                {
                    eqipParm = eqipParm.Distinct().ToList();

                    var equipments = _dbContext.TechnicalSpecialistCommodityEquipmentKnowledge
                        .Join(_dbContext.Data,
                        tseq => new { Id = tseq.EquipmentKnowledgeId },
                        dt => new { dt.Id },
                        (tseq, dt) => new
                        {
                            tseq.TechnicalSpecialistId,
                            dt.Name
                        }
                        ).Where(x => eqipParm.Contains(x.Name));

                    tsResult = tsResult.GroupJoin(equipments,
                                       x1 => x1.Id,
                                       y1 => y1.TechnicalSpecialistId,
                                       (ts, tsEquip) => new { ts, tsEquip })
                                       .Where(x1 => !eqipParm.Except(x1.tsEquip.Select(x2 => x2.Name)).Any())
                                       .Select(x => x.ts);

                }

                IList<string> customerApprovalParm = new List<string>();

                if (searchModel?.SearchParameter?.OptionalSearch?.CustomerApproval?.Count > 0)
                {
                    customerApprovalParm.AddRange(searchModel?.SearchParameter?.OptionalSearch?.CustomerApproval);
                }

                if (customerApprovalParm?.Count > 0)
                {
                    customerApprovalParm = customerApprovalParm.Distinct().ToList();
                    var customerAppr = _dbContext.TechnicalSpecialistCustomerApproval
                       .Join(_dbContext.TechnicalSpecialistCustomers,
                       tscustAp => new { Id = (int)tscustAp.CustomerId },
                       tsCust => new { tsCust.Id },
                       (tscustAp, tsCust) => new
                       {
                           tscustAp.TechnicalSpecialistId,
                           tsCust.Name
                       }).Where(x => customerApprovalParm.Contains(x.Name));


                    tsResult = tsResult.GroupJoin(customerAppr,
                                         x1 => x1.Id,
                                         y1 => y1.TechnicalSpecialistId,
                                         (ts, tsCustomer) => new { ts, tsCustomer })
                                         .Where(x1 => !customerApprovalParm.Except(x1.tsCustomer.Select(x2 => x2.Name)).Any())
                                         .Select(x => x.ts);
                }

                if (!string.IsNullOrEmpty(searchModel?.SearchParameter?.OptionalSearch?.SearchInProfile))
                {
                    // Temporary Solution for profile search, In future solar search should be implemented
                    var strCompare = searchModel?.SearchParameter?.OptionalSearch?.SearchInProfile;
                    var tsProfResult = _dbContext.TechnicalSpecialist.Where(x => x.Company.Code == searchModel.SearchParameter.OPCompanyCode);
                    var tsTrainings = tsProfResult
                        .Join(_dbContext.TechnicalSpecialistCertificationAndTraining,
                            ts => ts.Id,
                            tsCer => tsCer.TechnicalSpecialistId,
                            (ts, tsCert) => new { ts, tsCert })
                            .Where(x =>
                        x.ts.FirstName.ToLower().Contains(strCompare)
                        || x.ts.MiddleName.ToLower().Contains(strCompare)
                        || x.ts.LastName.ToLower().Contains(strCompare)
                        || x.ts.DrivingLicenseNumber.ToLower().Contains(strCompare)
                        || x.ts.PassportNumber.ToLower().Contains(strCompare)
                        || x.ts.TaxReference.ToLower().Contains(strCompare)
                        || x.ts.PayrollReference.ToLower().Contains(strCompare)
                        || x.ts.PayrollNote.ToLower().Contains(strCompare)
                        || x.ts.ProfessionalAfiliation.ToLower().Contains(strCompare)
                        || x.ts.ProfessionalSummary.ToLower().Contains(strCompare)
                        || x.ts.BusinessInformationComment.ToLower().Contains(strCompare)
                        || x.tsCert.CertificationAndTraining.Name.ToLower().Contains(strCompare)
                        || x.tsCert.Description.ToLower().Contains(strCompare))
                        .Select(x => x.ts.Id);

                    var tsContacts = tsProfResult.Join(_dbContext.TechnicalSpecialistContact,
                            ts => ts.Id,
                            tsCont => tsCont.TechnicalSpecialistId,
                            (ts, tsCont) => new { ts, tsCont })
                            .Where(x => x.tsCont.Address.ToLower().Contains(strCompare)
                            || x.tsCont.PostalCode.ToLower().Contains(strCompare)
                            || x.tsCont.EmailAddress.ToLower().Contains(strCompare)
                            || x.tsCont.TelephoneNumber.ToLower().Contains(strCompare)
                            || x.tsCont.FaxNumber.ToLower().Contains(strCompare)
                            || x.tsCont.MobileNumber.ToLower().Contains(strCompare)
                            || x.tsCont.EmergencyContactName.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsEduQuals = tsProfResult.Join(_dbContext.TechnicalSpecialistEducationalQualification,
                            ts => ts.Id,
                            tsQal => tsQal.TechnicalSpecialistId,
                            (ts, tsQal) => new { ts, tsQal })
                            .Where(x => x.tsQal.Qualification.ToLower().Contains(strCompare) || x.tsQal.Institution.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsWorkHistory = tsProfResult.Join(_dbContext.TechnicalSpecialistWorkHistory,
                            ts => ts.Id,
                            tsWrkh => tsWrkh.TechnicalSpecialistId,
                            (ts, tsWrkh) => new { ts, tsWrkh })
                            .Where(x => x.tsWrkh.ClientName.ToLower().Contains(strCompare)
                            || x.tsWrkh.ProjectName.ToLower().Contains(strCompare)
                            || x.tsWrkh.JobTitle.ToLower().Contains(strCompare)
                            || x.tsWrkh.JobResponsibility.ToLower().Contains(strCompare)
                            || x.tsWrkh.JobDescription.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsTaxon = tsProfResult.Join(_dbContext.TechnicalSpecialistTaxonomy,
                            ts => ts.Id,
                            tsTxn => tsTxn.TechnicalSpecialistId,
                            (ts, tsTxn) => new { ts, tsTxn })
                            .Where(x => x.tsTxn.Comments.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsCustApp = tsProfResult.Join(_dbContext.TechnicalSpecialistCustomerApproval,
                            ts => ts.Id,
                            tsCustAp => tsCustAp.TechnicalSpecialistId,
                            (ts, tsCustAp) => new { ts, tsCustAp })
                            .Where(x => x.tsCustAp.Comments.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsCodeStd = tsProfResult.Join(_dbContext.TechnicalSpecialistCodeAndStandard,
                            ts => ts.Id,
                            tsCdStd => tsCdStd.TechnicalSpecialistId,
                            (ts, tsCdStd) => new { ts, tsCdStd })
                            .Where(x => x.tsCdStd.CodeStandard.Name.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsCompElec = tsProfResult.Join(_dbContext.TechnicalSpecialistComputerElectronicKnowledge,
                            ts => ts.Id,
                            tsCompEl => tsCompEl.TechnicalSpecialistId,
                            (ts, tsCompEl) => new { ts, tsCompEl })
                            .Where(x => x.tsCompEl.ComputerKnowledge.Name.ToLower().Contains(strCompare))
                            .Select(x => x.ts.Id);

                    var tsNotes = tsProfResult.Join(_dbContext.TechnicalSpecialistNote,
                            ts => ts.Id,
                            tsNts => tsNts.TechnicalSpecialistId,
                            (ts, tsNts) => new { ts, tsNts })
                            .Where(x => x.tsNts.Note.ToLower().Contains(strCompare) && (x.tsNts.RecordType == "TSComment" || x.tsNts.RecordType == "TD" || x.tsNts.RecordType == "CD"))
                            .Select(x => x.ts.Id);

                    profSearchTsIds = tsTrainings?.Union(tsNotes)?.Union(tsCompElec)?.Union(tsCodeStd)?.Union(tsCustApp)?.Union(tsTaxon)?.Union(tsWorkHistory)?.Union(tsEduQuals)?.Union(tsContacts).ToList()?.Distinct();

                }

            }

            //if (searchModel?.OverridenPreferredResources?.Count > 0)
            //{
            //    epins = searchModel?.OverridenPreferredResources.Where(x => x.IsApproved.HasValue && x.IsApproved == true).Select(x => x.TechSpecialist.Epin).ToList();

            //    if (epins?.Count > 0)
            //    {
            //        overrideResource = overrideResource.Where(x => epins.Contains(x.Pin));
            //    }
            //}

            IQueryable<DbModel.TechnicalSpecialist> finalResources = tsResult;

            //if (searchModel?.SearchType?.ToString() == ResourceSearchType.ARS.ToString() && 
            //    (searchModel?.SearchAction?.ToString() == ResourceSearchAction.ARR.ToString() ||
            //    searchModel?.SearchAction?.ToString() == ResourceSearchAction.SSSPC.ToString()))
            //{
            //    finalResources = finalResources?.Union(overrideResource);
            //}

            if (profSearchTsIds != null && profSearchTsIds.Any())
            {
                tsSearchResult = finalResources?.Where(x => profSearchTsIds != null && profSearchTsIds.Contains(x.Id)).ToList()?.GroupBy(x => x.Pin).Select(x => x.FirstOrDefault());
            }
            else
            {
                tsSearchResult = finalResources.ToList()?.GroupBy(x => x.Pin).Select(x => x.FirstOrDefault());
            }

            var tsIds = tsSearchResult?.Select(x => x.Id).ToList();
            var contactTypes = new string[] { "PrimaryEmail", "PrimaryMobile", "PrimaryAddress" };
            var tsContactSearchResult = _dbContext.TechnicalSpecialistContact.Include("City").Include("Country").Include("County")
                                                  .Where(x => tsIds.Contains(x.TechnicalSpecialistId) && contactTypes.Contains(x.ContactType)).ToList();

            var tsCalendarSearchResult = _dbContext.TechnicalSpecialistCalendar.Where(x => tsIds.Contains(x.TechnicalSpecialistId)
                                                                                       && fromDate.Date >= x.StartDateTime.Value.Date
                                                                                       && fromDate.Date <= x.EndDateTime.Value.Date
                                                                                       && x.IsActive
                                                                                       ).ToList();

            return tsSearchResult.Select(x => new ResourceSearchTechSpecInfo
            {
                Epin = x.Pin,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmploymentType = x.EmploymentType?.Name,
                MobileNumber = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Id && x1?.ContactType == ContactType.PrimaryMobile.ToString())?.MobileNumber,
                Email = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Id && x1?.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress,
                Country = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Id && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.Country?.Name,
                State = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Id && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.County?.Name,
                City = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Id && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.City?.Name,
                Zip = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Id && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.PostalCode,
                ProfileStatus = x.ProfileStatus?.Name,
                SubDivision = x.SubDivision?.Name,
                ScheduleStatus = string.IsNullOrEmpty(tsCalendarSearchResult?.Where(x1 => x1.TechnicalSpecialistId == x.Id)?.OrderBy(x1 => Convert.ToInt32(x1.CalendarStatus.ToEnum<CalendarStatus>()))?.FirstOrDefault()?.CalendarStatus) ? "Available" : EnumExtension.DisplayName(tsCalendarSearchResult?.Where(x1 => x1.TechnicalSpecialistId == x.Id)?.OrderBy(x1 => x1.CalendarStatus.ToEnum<CalendarStatus>())?.FirstOrDefault()?.CalendarStatus.ToEnum<CalendarStatus>()),
                DistanceFromVenderInMile = -1 // 1328 - Not to show resources under non OC company Supplier.
                //IsSelected = (epins != null && epins.Contains(x.Pin))
            }).ToList();


            //TODO : Need to check why contact is not grouping based on TS data 
            //    var contactTypes = new string[] { "PrimaryEmail", "PrimaryMobile", "PrimaryAddress" };  
            //    return tsResult.GroupJoin(_dbContext.TechnicalSpecialistContact.Where(x => contactTypes.Contains(x.ContactType)),
            //x1 => new { TechnicalSpecialistId = x1.Id },
            //y1 => new { y1.TechnicalSpecialistId },
            //(ts, tsCont) => new ResourceSearchTechSpecInfo
            //{
            //    Epin = ts.Pin,
            //    FirstName = ts.FirstName,
            //    LastName =ts.LastName,
            //    EmploymentType = ts.EmploymentType.Name,
            //    MobileNumber =tsCont.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryMobile.ToString()).MobileNumber,
            //    Email =tsCont.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString()).EmailAddress,
            //    ProfileStatus = ts.ProfileStatus.Name,
            //    SubDivision = ts.SubDivision.Name
            //}).ToList();


        }

        private bool IsOperationSearchObjectLoaded(DomainModel.ResourceSearch searchModel)
        {
            ResourceSearchOptionalParameter resourceSearchOptionalParameter = searchModel?.SearchParameter?.OptionalSearch;
            if (resourceSearchOptionalParameter != null)
            {
                if (resourceSearchOptionalParameter?.EquipmentMaterialDescription != null && resourceSearchOptionalParameter?.EquipmentMaterialDescription.Count > 0)
                    return true;
                if (resourceSearchOptionalParameter?.CustomerApproval != null &&  resourceSearchOptionalParameter?.CustomerApproval.Count > 0)
                    return true;
                if (resourceSearchOptionalParameter?.Certification != null && resourceSearchOptionalParameter?.Certification.Count > 0)
                    return true;
                if (resourceSearchOptionalParameter?.LanguageSpeaking != null && resourceSearchOptionalParameter?.LanguageSpeaking.Count > 0)
                    return true;
                if (resourceSearchOptionalParameter?.LanguageWriting != null && resourceSearchOptionalParameter?.LanguageWriting.Count > 0)
                    return true;
                if (resourceSearchOptionalParameter?.LanguageComprehension != null && resourceSearchOptionalParameter?.LanguageComprehension.Count > 0)
                    return true;
                if (resourceSearchOptionalParameter.Radius.HasValue && resourceSearchOptionalParameter?.Radius.Value > 0)
                    return true;
                if (!string.IsNullOrEmpty(resourceSearchOptionalParameter?.SearchInProfile))
                    return true;
            }
            else
                return false;

            return false;
        }

        public IList<ResourceSearchTechSpecInfo> GetExceptionTSList(DomainModel.ResourceSearch searchModel)
        {
            IList<ResourceSearchTechSpecInfo> tsSearchResult = null;
            if (!string.IsNullOrEmpty(searchModel.CategoryName) && !string.IsNullOrEmpty(searchModel.SubCategoryName) && !string.IsNullOrEmpty(searchModel.ServiceName))
            {
                var fromDate = searchModel?.SearchParameter.FirstVisitFromDate ?? DateTime.UtcNow;
                var toDate = searchModel?.SearchParameter.FirstVisitToDate ?? DateTime.UtcNow;

                var tsResult = _dbContext.TechnicalSpecialist.Where(x => x.Company.Code == searchModel.SearchParameter.OPCompanyCode);

                tsResult = tsResult.Include(x => x.EmploymentType)
                           .Include(x => x.ProfileStatus)
                           .Include(x => x.SubDivision);

                if (searchModel?.SearchType == ResourceSearchType.ARS.ToString() || searchModel?.SearchType == ResourceSearchType.PreAssignment.ToString())
                {
                    tsResult = tsResult.Where(x => x.ProfileStatus.Name.ToLower() == ResourceSearchConstants.TS_Profile_Status_Active.ToLower()
                                                    && x.EmploymentType.Name.ToLower() != ResourceSearchConstants.Employment_Type_Former.ToLower());
                }


                tsSearchResult = tsResult.Join(_dbContext.TechnicalSpecialistTaxonomy,
                  x1 => x1.Id,
                  y1 => y1.TechnicalSpecialistId,
                  (ts, tsTax) => new { ts, tsTax })
                  .Where(x => x.tsTax.TaxonomyCategory.Name == searchModel.CategoryName
                       && x.tsTax.TaxonomySubCategory.TaxonomySubCategoryName == searchModel.SubCategoryName
                       && x.tsTax.TaxonomyServices.TaxonomyServiceName == searchModel.ServiceName
                       && (x.tsTax.ApprovalStatus != ResourceSearchConstants.TSTaxonomy_ApprovalStatus_Approve
                           || (x.tsTax.ToDate.HasValue && x.tsTax.ToDate < DateTime.UtcNow))
                     ).Select(x => new ResourceSearchTechSpecInfo
                     {
                         Epin = x.ts.Pin,
                         FirstName = x.ts.FirstName,
                         LastName = x.ts.LastName,
                         EmploymentType = x.ts.EmploymentType.Name,
                         ProfileStatus = x.ts.ProfileStatus.Name,
                         SubDivision = x.ts.SubDivision.Name,
                         ExceptionComment = ProcessExceptionComment(x.tsTax),
                     }).ToList();

                var tsEpins = tsSearchResult?.Select(x => x.Epin).ToList();
                var contactTypes = new string[] { "PrimaryEmail", "PrimaryMobile", "PrimaryAddress" };
                var tsContactSearchResult = _dbContext.TechnicalSpecialistContact.Include("City").Include("Country").Include("County")
                                                        .Where(x => tsEpins.Contains(x.TechnicalSpecialistId) && contactTypes.Contains(x.ContactType)).ToList();

                var tsCalendarSearchResult = _dbContext.TechnicalSpecialistCalendar.Where(x => tsEpins.Contains(x.TechnicalSpecialistId)
                                                                                           && x.StartDateTime <= fromDate.Date
                                                                                           && x.EndDateTime >= toDate.Date
                                                                                           && x.IsActive
                                                                                           ).ToList();
                var tsFinalSearchResult = tsSearchResult.Select(x => new ResourceSearchTechSpecInfo()
                {
                    Epin = x.Epin,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmploymentType = x.EmploymentType,
                    ProfileStatus = x.ProfileStatus,
                    SubDivision = x.SubDivision,
                    ExceptionComment = x.ExceptionComment,
                    MobileNumber = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Epin && x1?.ContactType == ContactType.PrimaryMobile.ToString())?.MobileNumber,
                    Email = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Epin && x1?.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress,
                    Country = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Epin && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.Country?.Name,
                    State = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Epin && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.County?.Name,
                    City = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Epin && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.City?.Name,
                    Zip = tsContactSearchResult?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == x.Epin && x1?.ContactType == ContactType.PrimaryAddress.ToString())?.PostalCode,
                    ScheduleStatus = tsCalendarSearchResult?.Where(x1 => x1.TechnicalSpecialistId == x.Epin)?.OrderBy(x1 => x1.CalendarStatus.ToEnum<CalendarStatus>())?.FirstOrDefault()?.CalendarStatus ?? "Available",
                }).ToList();
                return tsFinalSearchResult;
            }
            return new List<ResourceSearchTechSpecInfo>();
        }

        private string ProcessExceptionComment(TechnicalSpecialistTaxonomy tsTaxonomy)
        {
            List<string> result = new List<string>();
            if (tsTaxonomy.ApprovalStatus != ResourceSearchConstants.TSTaxonomy_ApprovalStatus_Approve)
            {
                result.Add(string.Format(ResourceSearchConstants.TSTaxonomy_Exception_Comment, tsTaxonomy.ApprovalStatus));
            }

            if (!tsTaxonomy.ToDate.HasValue || tsTaxonomy.ToDate < DateTime.UtcNow)
            {
                result.Add(string.Format(ResourceSearchConstants.TSTaxonomy_Exception_Comment, "Expired"));
            }

            return string.Join(",", result);
        }

        public List<AssignedResourceInfo> GetAssignmentTechSpec(int assignmentId, int supplierPoId)
        {
            List<AssignedResourceInfo> baseResource = new List<AssignedResourceInfo>();
            if (supplierPoId > 0)
            {
                var data = _dbContext.AssignmentSubSupplier?.Where(x => x.AssignmentId == assignmentId)
                .Select(x => new DbModel.AssignmentSubSupplier
                {
                    Id = x.Id,
                    Supplier = new DbModel.Supplier { SupplierName = x.Supplier.SupplierName },
                    Assignment = new DbModel.Assignment
                    {
                        AssignmentTaxonomy = new List<DbModel.AssignmentTaxonomy>
                                             {
                                                    new DbModel.AssignmentTaxonomy{
                                                    TaxonomyService=new DbModel.TaxonomyService {
                                                        Id= x.Assignment.AssignmentTaxonomy.FirstOrDefault().TaxonomyServiceId,
                                                        TaxonomyServiceName = x.Assignment.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomyServiceName
                                                    }
                                                }
                                            }
                    },

                    AssignmentSubSupplierTechnicalSpecialist = x.AssignmentSubSupplierTechnicalSpecialist.Select(x1 => new DbModel.AssignmentSubSupplierTechnicalSpecialist
                    {
                        TechnicalSpecialist = new DbModel.AssignmentTechnicalSpecialist
                        {
                            TechnicalSpecialist = new DbModel.TechnicalSpecialist
                            {
                                Pin = x1.TechnicalSpecialist.TechnicalSpecialist.Pin,
                                FirstName = x1.TechnicalSpecialist.TechnicalSpecialist.FirstName,
                                LastName = x1.TechnicalSpecialist.TechnicalSpecialist.LastName,
                                ProfileStatus = new DbModel.Data
                                {
                                    Id = x1.TechnicalSpecialist.TechnicalSpecialist.ProfileStatusId,
                                    Name = x1.TechnicalSpecialist.TechnicalSpecialist.ProfileStatus.Name,
                                },
                                TechnicalSpecialistTaxonomy = x1.TechnicalSpecialist.TechnicalSpecialist.TechnicalSpecialistTaxonomy != null ?
                                                                                x1.TechnicalSpecialist.TechnicalSpecialist.TechnicalSpecialistTaxonomy
                                                                                .Select(x2 => new DbModel.TechnicalSpecialistTaxonomy { TaxonomyServicesId = x2.TaxonomyServicesId }).ToList()
                                                                                : null
                            }
                        }
                    }).ToList()
                }).ToList();

                data?.ForEach(x =>
                {
                    var value = data.SelectMany(x1 => x1.AssignmentSubSupplierTechnicalSpecialist);
                    if (x != null)
                    {
                        baseResource.Add(new AssignedResourceInfo
                        {
                            AssignedTechSpec = value.Where(x1 => x1.AssignmentSubSupplierId == x.Id)?
                            .Select(x1 => new BaseResourceSearchTechSpecInfo
                            {
                                Epin = x1.TechnicalSpecialist.TechnicalSpecialist.Pin,
                                FirstName = x1.TechnicalSpecialist.TechnicalSpecialist.FirstName,
                                LastName = x1.TechnicalSpecialist.TechnicalSpecialist.LastName,
                                ProfileStatus = x1.TechnicalSpecialist.TechnicalSpecialist.ProfileStatus.Name,
                                IsTechSpecFromAssignmentTaxonomy = x1.TechnicalSpecialist.TechnicalSpecialist.TechnicalSpecialistTaxonomy != null ? x1.TechnicalSpecialist.TechnicalSpecialist.TechnicalSpecialistTaxonomy
                                                             .Where(x2 => x2.TaxonomyServicesId == x.Assignment.AssignmentTaxonomy.FirstOrDefault().TaxonomyServiceId).Any() ? true : false : false
                            }).ToList(),
                            SupplierName = x.Supplier.SupplierName,
                            TaxonomyServiceName = x.Assignment?.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.TaxonomyServiceName
                        });
                    }
                });
            }
            else
            {
                var data = _dbContext.AssignmentTechnicalSpecialist?.Where(x => x.AssignmentId == assignmentId)
                                          .Select(x => new DbModel.AssignmentTechnicalSpecialist
                                          {
                                              TechnicalSpecialist = new DbModel.TechnicalSpecialist
                                              {
                                                  Pin = x.TechnicalSpecialistId,
                                                  FirstName = x.TechnicalSpecialist.FirstName,
                                                  LastName = x.TechnicalSpecialist.LastName,
                                                  ProfileStatus = new DbModel.Data
                                                  {
                                                      Id = x.TechnicalSpecialist.ProfileStatusId,
                                                      Name = x.TechnicalSpecialist.ProfileStatus.Name
                                                  },
                                                  TechnicalSpecialistTaxonomy = x.TechnicalSpecialist.TechnicalSpecialistTaxonomy != null ?
                                                                                     x.TechnicalSpecialist.TechnicalSpecialistTaxonomy.Select(x1 => new DbModel.TechnicalSpecialistTaxonomy { TaxonomyServicesId = x1.TaxonomyServicesId }).ToList()
                                                                                     : null
                                              },

                                              Assignment = new DbModel.Assignment
                                              {
                                                  AssignmentTaxonomy = new List<DbModel.AssignmentTaxonomy>
                                                      {
                                                        new DbModel.AssignmentTaxonomy{
                                                        TaxonomyService=new DbModel.TaxonomyService {
                                                            Id= x.Assignment.AssignmentTaxonomy.FirstOrDefault().TaxonomyServiceId,
                                                            TaxonomyServiceName = x.Assignment.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomyServiceName
                                                          }
                                                        }
                                                      }
                                              }
                                          })?.ToList();
                if (data != null && data.Any())
                {
                    baseResource.Add(new AssignedResourceInfo
                    {
                        AssignedTechSpec = data?.Select(x1 => new BaseResourceSearchTechSpecInfo
                        {
                            Epin = x1.TechnicalSpecialistId,
                            FirstName = x1.TechnicalSpecialist.FirstName,
                            LastName = x1.TechnicalSpecialist.LastName,
                            ProfileStatus = x1.TechnicalSpecialist.ProfileStatus.Name,
                            IsTechSpecFromAssignmentTaxonomy = x1.TechnicalSpecialist.TechnicalSpecialistTaxonomy.Where(x2 => x2.TechnicalSpecialistId == x1.TechnicalSpecialistId
                                                                                      && x2.TaxonomyServicesId == x1.Assignment.AssignmentTaxonomy.FirstOrDefault().TaxonomyServiceId)
                                                                                      .Any() ? true : false
                        }).ToList(),
                        TaxonomyServiceName = data.FirstOrDefault().Assignment.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.TaxonomyServiceName
                    });
                }
            }

            return baseResource;
        }

        public List<DbModel.Supplier> GetSupplier(List<int> supplierId)  //693- Subsupplier Postal code is not getting passed because of MS-TS implementation
        {
            return _dbContext.Supplier.Where(x => supplierId.Contains(x.Id))?.Select(x2=> new DbModel.Supplier { Id= x2.Id,SupplierName =x2.SupplierName, PostalCode = x2.PostalCode}).ToList();
        }
    }
}
