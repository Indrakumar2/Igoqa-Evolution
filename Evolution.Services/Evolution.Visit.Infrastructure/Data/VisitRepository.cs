using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Company.Domain.Enums;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Models.Visits;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitRepository : GenericRepository<DbModel.Visit>, IVisitRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitRepository> _logger = null;
        private readonly IVisitTechnicalSpecialistsAccountRepository _visitTechnicalSpecialistsAccountRepository = null;

        public VisitRepository(DbModel.EvolutionSqlDbContext dbContext,
                                IMapper mapper,
                                IAppLogger<VisitRepository> logger,
                                IVisitTechnicalSpecialistsAccountRepository visitTechnicalSpecialistsAccountRepository) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
            _visitTechnicalSpecialistsAccountRepository = visitTechnicalSpecialistsAccountRepository;
        }

        public IList<DomainModel.VisitSearchResults> Search(DomainModel.BaseVisit model)
        {
            var VisitResult = this._dbContext.Visit
                                    .Where(this.GetWhereExpression(model))
                                    .Include("VisitTechnicalSpecialist")
                                    .Include("VisitTechnicalSpecialist.TechnicalSpecialist")
                                    .ProjectTo<DomainModel.VisitSearchResults>().ToList();

            return VisitResult;
        }

        public int GetCount(DomainModel.BaseVisit searchModel)
        {
            return _dbContext.Visit
                             .Where(this.GetWhereExpression(searchModel)).Count();
        }

        //TODO : Remove all the hard coded value
        private Expression<Func<DbModel.Visit, bool>> GetWhereExpression(DomainModel.BaseVisit searchModel)
        {
            Expression<Func<DbModel.Visit, bool>> expression = x => (searchModel.VisitFutureDays == null || x.FromDate <= DateTime.Now.AddDays(Convert.ToInt32(searchModel.VisitFutureDays))) &&
                                                 ((string.IsNullOrEmpty(searchModel.VisitStatus) && x.VisitStatus != "A" && x.VisitStatus != "D") || x.VisitStatus == searchModel.VisitStatus) &&
                                                 (searchModel.VisitAssignmentNumber <= 0 || x.Assignment.AssignmentNumber == searchModel.VisitAssignmentNumber) &&
                                                 (String.IsNullOrEmpty(searchModel.VisitContractCompany) || x.Assignment.ContractCompany.Name == searchModel.VisitContractCompany) &&
                                                 (String.IsNullOrEmpty(searchModel.VisitContractCompanyCode) || x.Assignment.ContractCompany.Code == searchModel.VisitContractCompanyCode) &&
                                                 (String.IsNullOrEmpty(searchModel.VisitContractCoordinator) || x.Assignment.ContractCompanyCoordinator.Name == searchModel.VisitContractCoordinator) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitContractNumber) || x.Assignment.Project.Contract.ContractNumber == searchModel.VisitContractNumber) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitCustomerCode) || x.Assignment.Project.Contract.Customer.Code == searchModel.VisitCustomerCode) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitCustomerContractNumber) || x.Assignment.Project.Contract.CustomerContractNumber == searchModel.VisitCustomerContractNumber) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitCustomerName) || x.Assignment.Project.Contract.Customer.Name == searchModel.VisitCustomerName) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitCustomerProjectName) || x.Assignment.Project.CustomerProjectName == searchModel.VisitCustomerProjectName) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitOperatingCompany) || x.Assignment.OperatingCompany.Name == searchModel.VisitOperatingCompany || x.Assignment.ContractCompany.Name == searchModel.VisitOperatingCompany) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitOperatingCompanyCode) || x.Assignment.OperatingCompany.Code == searchModel.VisitOperatingCompanyCode || x.Assignment.ContractCompany.Code == searchModel.VisitOperatingCompanyCode) &&
                                                 ((searchModel.VisitProjectNumber == null) || (searchModel.VisitProjectNumber == x.Assignment.Project.ProjectNumber)) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitOperatingCompanyCoordinator) || x.Assignment.OperatingCompanyCoordinator.Name == searchModel.VisitOperatingCompanyCoordinator || x.Assignment.ContractCompanyCoordinator.Name == searchModel.VisitOperatingCompanyCoordinator) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitOperatingCompanyCoordinatorSamAcctName) || x.Assignment.OperatingCompanyCoordinator.SamaccountName == searchModel.VisitOperatingCompanyCoordinatorSamAcctName || x.Assignment.ContractCompanyCoordinator.SamaccountName == searchModel.VisitOperatingCompanyCoordinatorSamAcctName) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitSupplier) || x.Supplier.SupplierName == searchModel.VisitSupplier) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitSupplierPONumber) || x.Assignment.SupplierPurchaseOrder.SupplierPonumber == searchModel.VisitSupplierPONumber) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitNotificationReference) || x.NotificationReference == searchModel.VisitNotificationReference) &&
                                                 (string.IsNullOrEmpty(searchModel.VisitReportNumber) || x.ReportNumber == searchModel.VisitReportNumber) &&
                                                 ((searchModel.TechSpecialists == null) || x.VisitTechnicalSpecialist.Any(ts => searchModel.TechSpecialists.Any(t => t.Pin == ts.TechnicalSpecialist.Pin)));

            return expression;
        }

        public List<DomainModel.BaseVisit> GetVisit(DomainModel.BaseVisit searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = Filter(searchModel);
            var expression = dbSearchModel.ToExpression();
            List<DomainModel.BaseVisit> filteredVisit = new List<DomainModel.BaseVisit>();
            if (expression == null)
                filteredVisit = whereClause.ProjectTo<DomainModel.BaseVisit>().ToList();
            else
                filteredVisit = whereClause.Where(expression).ProjectTo<DomainModel.BaseVisit>().ToList();

            var visit = whereClause?.Include("VisitTechnicalSpecialist.TechnicalSpecialist").ToList();

            if (filteredVisit != null && filteredVisit.Count > 0)
            {
                filteredVisit.ForEach(x =>
                {
                    x.TechSpecialists = visit?.SelectMany(x1 => x1.VisitTechnicalSpecialist).Where(x2 => x2.VisitId == x.VisitId)?.Select(x3 => x3.TechnicalSpecialist)?.Select(x4 => new DomainModel.TechnicalSpecialist
                    {
                        FirstName = x4.FirstName,
                        LastName = x4.LastName,
                        Pin = x4.Pin
                    })?.ToList();
                });
            }

            return filteredVisit;
        }

        static bool CheckAllFields<TInput, TValue>(TInput input, TValue value, bool alsoCheckProperties)
        {
            Type t = typeof(TInput);
            foreach (FieldInfo info in t.GetFields().Where(x => x.FieldType == typeof(TValue)))
            {
                if (!info.GetValue(input).Equals(value))
                {
                    return false;
                }
            }
            if (alsoCheckProperties)
            {
                foreach (PropertyInfo info in t.GetProperties().Where(x => x.PropertyType == typeof(TValue)))
                {
                    if (!info.GetValue(input, null).Equals(value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public DomainModel.Visit GetVisitByID(DomainModel.BaseVisit searchModel)
        {
            DomainModel.Visit visit = new DomainModel.Visit();
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = VisitFilter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                visit = whereClause.ProjectTo<DomainModel.Visit>().FirstOrDefault();
            else
                visit = whereClause.Where(expression).ProjectTo<DomainModel.Visit>().FirstOrDefault();
            return visit;
        }


        public DomainModel.Visit GetVisitByID1(DbModel.Visit visits)
        {
            DomainModel.Visit visit = new DomainModel.Visit();

            DomainModel.BaseVisit searchModel = new DomainModel.BaseVisit
            {
                VisitId = visits.Id
            };
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = VisitFilter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                visit = whereClause.ProjectTo<DomainModel.Visit>().FirstOrDefault();
            else
                visit = whereClause.Where(expression).ProjectTo<DomainModel.Visit>().FirstOrDefault();
            return visit;
        }


        public DomainModel.Visit GetVisitByID1(DomainModel.BaseVisit searchModel)
        {
            DomainModel.Visit visit = new DomainModel.Visit();
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = VisitFilter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                visit = whereClause.ProjectTo<DomainModel.Visit>().FirstOrDefault();
            else
                visit = whereClause.Where(expression).ProjectTo<DomainModel.Visit>().FirstOrDefault();
            return visit;
        }


        public List<DbModel.SystemSetting> MailTemplateForVisitInterCompanyAmendment()
        {
            return _dbContext.SystemSetting.Where(x => x.KeyName == EmailKey.EmailVisitInterCompanyAmendmentReason.ToString()).ToList();
        }
        public List<DomainModel.Visit> GetHistoricalVisits(DomainModel.BaseVisit searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = Filter(searchModel);
            List<DomainModel.Visit> visits = null;
            whereClause
                .Include("VisitTechnicalSpecialist")
                .Include("VisitTechnicalSpecialist.TechnicalSpecialist");
            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                visits = whereClause.ProjectTo<DomainModel.Visit>().ToList();
            else
                visits = whereClause.Where(expression).ProjectTo<DomainModel.Visit>().ToList();

            return visits;
        }

        public IQueryable<DbModel.Visit> GetVisitForDocumentApproval(DomainModel.BaseVisit searchModel)
        {
            return VisitDocumentFilter(searchModel);
        }

        public IList<TechSpecIntertekWorkHistory> GetIntertekWorkHistoryReport(int epin)
        {
            IList<TechSpecIntertekWorkHistory> reportData = new List<TechSpecIntertekWorkHistory>();
            Exception exception = null;
            try
            {
                var visitReportData = _dbContext.Visit
                     .Join(_dbContext.VisitTechnicalSpecialist,
                         vs => new { vs.Id },
                         vst => new { Id = vst.VisitId },
                         (vs, vst) => new { vs, vst }
                     )
                     .GroupJoin(_dbContext.AssignmentTaxonomy,
                         vsa => vsa.vs.AssignmentId,
                         ast => ast.AssignmentId,
                         (vsa, ast) => new { vsa, ast }
                     ).SelectMany(at => at.ast.DefaultIfEmpty(), (v, at) => new { v.vsa, at })
                     .Join(_dbContext.SupplierPurchaseOrder,
                         vss => new { vss.vsa.vs.Assignment.SupplierPurchaseOrderId },
                         spo => new { SupplierPurchaseOrderId = (int?)spo.Id },
                         (vss, spo) => new { vss, spo }
                     ).Where(x => x.vss.vsa.vst.TechnicalSpecialist.Pin == epin)
                     .Select(x => new
                     {
                         x.vss.vsa.vs.AssignmentId,
                         x.vss.vsa.vs.Assignment.AssignmentNumber, //D1130 (Issue 6.2)
                         x.vss.vsa.vs.Assignment.Project.ProjectNumber,
                         Client = x.vss.vsa.vs.Assignment.Project.Contract.Customer.Name,
                         InspectedEquipment = x.spo.MaterialDescription,
                         x.vss.vsa.vs.Supplier.SupplierName,
                         SupplierCity = x.vss.vsa.vs.Supplier.City.Name,
                         SupplierCounty = x.vss.vsa.vs.Supplier.County.Name,
                         SupplierCountry = x.vss.vsa.vs.Supplier.Country.Name,
                         SupplierPostalCode = x.vss.vsa.vs.Supplier.PostalCode,
                         Category = x.vss.at.TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name,
                         SubCategory = x.vss.at.TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName,
                         Service = x.vss.at.TaxonomyService.TaxonomyServiceName
                     }).GroupBy(x => new { x.AssignmentId, x.AssignmentNumber, x.ProjectNumber, x.Client, x.InspectedEquipment, x.SupplierName, x.SupplierCity, x.SupplierCountry, x.SupplierCounty, x.SupplierPostalCode, x.Category, x.SubCategory, x.Service })
                     .Select(x => new TechSpecIntertekWorkHistory
                     {
                         AssignmentId = x.Key.AssignmentId,
                         AssignmentNumber = x.Key.AssignmentNumber,
                         ProjectNumber = x.Key.ProjectNumber,
                         Client = x.Key.Client,
                         InspectedEquipment = x.Key.InspectedEquipment,
                         SupplierName = x.Key.SupplierName,
                         SupplierCity = x.Key.SupplierCity,
                         SupplierCounty = x.Key.SupplierCounty,
                         SupplierCountry = x.Key.SupplierCountry,
                         SupplierPostalCode = x.Key.SupplierPostalCode,
                         Category = x.Key.Category,
                         SubCategory = x.Key.SubCategory,
                         Service = x.Key.Service
                     }).ToList();
                //Def 1349 : included timesheet related work history data.
                var timesheetReportData = _dbContext.Timesheet
                    .Join(_dbContext.TimesheetTechnicalSpecialist,
                        tims => new { tims.Id },
                        vst => new { Id = vst.TimesheetId },
                        (tims, vst) => new { tims, vst }
                    )
                    .GroupJoin(_dbContext.AssignmentTaxonomy,
                        Timsa => Timsa.tims.AssignmentId,
                        ast => ast.AssignmentId,
                        (timsa, ast) => new { timsa, ast }
                    ).SelectMany(at => at.ast.DefaultIfEmpty(), (v, at) => new { v.timsa, at })
                    .Where(x => x.timsa.vst.TechnicalSpecialist.Pin == epin)
                    .Select(x => new
                    {
                        x.timsa.tims.AssignmentId,
                        x.timsa.tims.Assignment.AssignmentNumber, //D1130 (Issue 6.2)
                        x.timsa.tims.Assignment.Project.ProjectNumber,
                        Client = x.timsa.tims.Assignment.Project.Contract.Customer.Name,
                        Category = x.at.TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name,
                        SubCategory = x.at.TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName,
                        Service = x.at.TaxonomyService.TaxonomyServiceName
                    }).GroupBy(x => new { x.AssignmentId, x.AssignmentNumber, x.ProjectNumber, x.Client, x.Category, x.SubCategory, x.Service })
                    .Select(x => new TechSpecIntertekWorkHistory
                    {
                        AssignmentId = x.Key.AssignmentId,
                        AssignmentNumber = x.Key.AssignmentNumber,
                        ProjectNumber = x.Key.ProjectNumber,
                        Client = x.Key.Client,
                        Category = x.Key.Category,
                        SubCategory = x.Key.SubCategory,
                        Service = x.Key.Service
                    }).ToList();
                if (visitReportData != null && visitReportData.Any())
                {
                    reportData.AddRange(visitReportData);
                }
                if (timesheetReportData != null && timesheetReportData.Any())
                {
                    reportData.AddRange(timesheetReportData);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), epin);
            }
            return reportData?.OrderBy(x => x.AssignmentNumber)?.ThenBy(x => x.ProjectNumber)?.ThenBy(x => x.Client).ToList();
        }

        public IList<TechSpecIntertekWorkHistory> GetStandardCVIntertekWorkHistoryReport(int epin)
        {
            IList<TechSpecIntertekWorkHistory> reportData = new List<TechSpecIntertekWorkHistory>();
            Exception exception = null;
            try
            {
                var visitReportData = _dbContext.Visit
                     .Join(_dbContext.VisitTechnicalSpecialist,
                         vs => new { vs.Id },
                         vst => new { Id = vst.VisitId },
                         (vs, vst) => new { vs, vst }
                     )
                     .Join(_dbContext.SupplierPurchaseOrder,
                         vss => new { vss.vs.Assignment.SupplierPurchaseOrderId },
                         spo => new { SupplierPurchaseOrderId = (int?)spo.Id },
                         (vss, spo) => new { vss, spo }
                     ).Where(x => x.vss.vst.TechnicalSpecialist.Pin == epin && (x.vss.vs.Assignment.IsInternalAssignment == false || x.vss.vs.Assignment.IsInternalAssignment == null))
                     .Select(x => new
                     {
                         Client = x.vss.vs.Assignment.Project.Contract.Customer.Name,
                         InspectedEquipment = x.spo.MaterialDescription,
                         x.vss.vs.Supplier.SupplierName,
                         SupplierCity = x.vss.vs.Supplier.City.Name,
                         SupplierCounty = x.vss.vs.Supplier.County.Name,
                         SupplierCountry = x.vss.vs.Supplier.Country.Name,
                         SupplierPostalCode = x.vss.vs.Supplier.PostalCode,
                     }).GroupBy(x => new { x.SupplierName, x.SupplierCity, x.SupplierCountry, x.SupplierCounty, x.SupplierPostalCode, x.InspectedEquipment, x.Client })
                     .Select(x => new TechSpecIntertekWorkHistory
                     {
                         SupplierName = x.Key.SupplierName,
                         SupplierCity = x.Key.SupplierCity,
                         SupplierCounty = x.Key.SupplierCounty,
                         SupplierCountry = x.Key.SupplierCountry,
                         SupplierPostalCode = x.Key.SupplierPostalCode,
                         Client = x.Key.Client,
                         InspectedEquipment = x.Key.InspectedEquipment
                     }).OrderBy(x => x.SupplierName).ThenBy(x => x.SupplierCity).ThenBy(x => x.SupplierCounty).ThenBy(x => x.SupplierPostalCode).ThenBy(x => x.SupplierCountry).ThenBy(x => x.InspectedEquipment).ThenBy(x => x.Client).ToList();
                //Def 1349 : included timesheet related work history data.
                var timesheetReportData = _dbContext.Timesheet
                    .Join(_dbContext.TimesheetTechnicalSpecialist,
                    tmTs => tmTs.Id,
                    tts => tts.TimesheetId,
                    (tmTs, tts) => new { tmTs, tts })
                    .Join(_dbContext.TechnicalSpecialist,
                    tmTech => tmTech.tts.TechnicalSpecialistId,
                    tech => tech.Id,
                    (tmTech, tech) => new { tmTech, tech })
                    .Where(x => x.tech.Pin == epin)
                     .Select(x => new
                     {
                         Client = x.tmTech.tmTs.Assignment.Project.Contract.Customer.Name,
                     }).GroupBy(x => new { x.Client })
                    .Select(x => new TechSpecIntertekWorkHistory
                    {
                        Client = x.Key.Client,
                    }).OrderBy(x => x.Client).ToList();

                if (visitReportData != null && visitReportData.Any())
                {
                    reportData.AddRange(visitReportData);
                }
                if (timesheetReportData != null && timesheetReportData.Any())
                {
                    reportData.AddRange(timesheetReportData);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), epin);
            }
            return reportData;
        }

        private List<HeaderList> GetHeaderList()
        {
            List<HeaderList> headerData = new List<HeaderList>
            {
                new HeaderList { Label = "Visit Number", Key = "visitNumber" },
                new HeaderList { Label = "Customer", Key = "visitCustomerName" },
                new HeaderList { Label = "Contract No", Key = "visitContractNumber" },
                new HeaderList { Label = "Project No", Key = "visitProjectNumber" },
                new HeaderList { Label = "Customer Contact No", Key = "visitCustomerContractNumber" },
                new HeaderList { Label = "Assignment No", Key = "visitAssignmentNumber" },
                new HeaderList { Label = "Customer Project Name", Key = "visitCustomerProjectName" },
                new HeaderList { Label = "Customer Project No", Key = "visitCustomerProjectNumber" },
                new HeaderList { Label = "CH Coordinator Name", Key = "visitContractCoordinator" },
                new HeaderList { Label = "OC Coordinator Name", Key = "visitOperatingCompanyCoordinator" },
                new HeaderList { Label = "Supplier/Sub-Supplier", Key = "visitSupplier" },
                new HeaderList { Label = "Visit Status", Key = "visitStatus" },
                new HeaderList { Label = "Supplier PO Number", Key = "visitSupplierPONumber" },
                new HeaderList { Label = "Material Description", Key = "visitMaterialDescription" },
                new HeaderList { Label = "Visit Date", Key = "visitStartDate" },
                new HeaderList { Label = "Sent To Customer", Key = "visitReportSentToCustomerDate" },
                new HeaderList { Label = "Report Number", Key = "visitReportNumber" },
                new HeaderList { Label = "Resource(s)", Key = "techSpecialists.FullName" },
                new HeaderList { Label = "Contract Company", Key = "visitContractCompany" },
                new HeaderList { Label = "Operating Company", Key = "visitOperatingCompany" },
                new HeaderList { Label = "Notification Reference", Key = "visitNotificationReference" }
            };

            return headerData;
        }

        //Added extra to check Search functionality
        public Result SearchVisits(DomainModel.VisitSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            Result result = new Result();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                #region Visit
                IQueryable<DbModel.Visit> whereClauseVisit = _dbContext.Visit.AsNoTracking();
                if (searchModel.AssignmentId != null)
                    whereClauseVisit = whereClauseVisit.Where(x => x.AssignmentId == searchModel.AssignmentId);

                if (searchModel.VisitIds?.Count > 0)
                    whereClauseVisit = whereClauseVisit.Where(x => searchModel.VisitIds.Contains(x.Id));

                if (searchModel.SupplierId > 0)
                    whereClauseVisit = whereClauseVisit.Where(x => x.SupplierId == searchModel.SupplierId);

                if (searchModel.NotificationReference.HasEvoWildCardChar())
                    whereClauseVisit = whereClauseVisit.WhereLike(x => x.NotificationReference, searchModel.NotificationReference, '*');
                else if (!string.IsNullOrEmpty(searchModel.NotificationReference))
                    whereClauseVisit = whereClauseVisit.Where(x => x.NotificationReference == searchModel.NotificationReference);

                if (searchModel.ReportNumber.HasEvoWildCardChar())
                    whereClauseVisit = whereClauseVisit.WhereLike(x => x.ReportNumber, searchModel.ReportNumber, '*');
                else if (!string.IsNullOrEmpty(searchModel.ReportNumber))
                    whereClauseVisit = whereClauseVisit.Where(x => x.ReportNumber == searchModel.ReportNumber);

                if (searchModel.FromDate != null || searchModel.ToDate != null)
                {
                    if (searchModel.FromDate != null && searchModel.ToDate != null)
                        whereClauseVisit = whereClauseVisit.Where(x => x.FromDate >= searchModel.FromDate.Value.Date && x.FromDate <= searchModel.ToDate.Value.Date);
                    else if (searchModel.ToDate == null)
                        whereClauseVisit = whereClauseVisit.Where(x => x.FromDate >= searchModel.FromDate.Value.Date);
                    else if (searchModel.FromDate == null)
                        whereClauseVisit = whereClauseVisit.Where(x => x.FromDate <= searchModel.ToDate.Value.Date);
                }

                if (searchModel.TechnicalSpecialist.HasEvoWildCardChar())
                {
                    searchModel.TechnicalSpecialist = searchModel.TechnicalSpecialist.Trim('*');
                    whereClauseVisit = whereClauseVisit.Where(x => x.VisitTechnicalSpecialist.Any(ts => ts.TechnicalSpecialist.LastName.StartsWith(searchModel.TechnicalSpecialist)));
                }
                else if (!string.IsNullOrEmpty(searchModel.TechnicalSpecialist))
                    whereClauseVisit = whereClauseVisit.Where(x => string.IsNullOrEmpty(searchModel.TechnicalSpecialist) || x.VisitTechnicalSpecialist.Any(ts => searchModel.TechnicalSpecialist == ts.TechnicalSpecialist.LastName));
                #endregion

                #region Assignment
                IQueryable<DbModel.Assignment> whereClauseAssignment = _dbContext.Assignment.AsNoTracking();
                if (searchModel.ProjectNumber > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.ProjectId == searchModel.ProjectNumber);
                if (searchModel.AssignmentNubmer > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.AssignmentNumber == searchModel.AssignmentNubmer);

                if (searchModel.IsOnlyViewVisit && searchModel.LoggedInCompanyId > 0)
                    whereClauseVisit = whereClauseVisit.Where(x => (x.Assignment.ContractCompanyId == searchModel.LoggedInCompanyId || x.Assignment.OperatingCompanyId == searchModel.LoggedInCompanyId));
                else
                {
                    if (searchModel.ContractCompanyId > 0 || searchModel.OperatingCompanyId > 0)
                    {
                        whereClauseVisit = whereClauseVisit.Where(x => (x.Assignment.ContractCompanyId == searchModel.LoggedInCompanyId
                            || x.Assignment.OperatingCompanyId == searchModel.LoggedInCompanyId
                            || x.Assignment.Project.Contract.ParentContract.ContractHolderCompanyId == searchModel.LoggedInCompanyId));
                    }
                    if (searchModel.ContractCompanyId > 0)
                        whereClauseAssignment = whereClauseAssignment.Where(x => x.ContractCompanyId == searchModel.ContractCompanyId);

                    if (searchModel.OperatingCompanyId > 0)
                        whereClauseAssignment = whereClauseAssignment.Where(x => x.OperatingCompanyId == searchModel.OperatingCompanyId);
                }

                if (searchModel.ContractCoordinatorId > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.ContractCompanyCoordinatorId == searchModel.ContractCoordinatorId);

                if (searchModel.OperatingCoordinatorId > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.OperatingCompanyCoordinatorId == searchModel.OperatingCoordinatorId);

                if (searchModel.CategoryId > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.AssignmentTaxonomy.Any(x1 => x1.TaxonomyServiceId == searchModel.ServiceId));

                if (searchModel.SubCategoryId > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.AssignmentTaxonomy.Any(x1 => x1.TaxonomyService.TaxonomySubCategoryId == searchModel.SubCategoryId));

                if (searchModel.ServiceId > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.AssignmentTaxonomy.Any(x1 => x1.TaxonomyService.TaxonomySubCategory.TaxonomyCategoryId == searchModel.CategoryId));

                #endregion

                #region SupplierPurchaseOrder
                IQueryable<DbModel.SupplierPurchaseOrder> whereClauseSupplierPO = _dbContext.SupplierPurchaseOrder.AsNoTracking();
                if (searchModel.SupplierPONumber.HasEvoWildCardChar())
                {
                    whereClauseSupplierPO = whereClauseSupplierPO.WhereLike(x => x.SupplierPonumber, searchModel.SupplierPONumber, '*');
                    whereClauseAssignment = whereClauseAssignment.Where(x => whereClauseSupplierPO.Any(x1 => x1.Id == x.SupplierPurchaseOrderId));
                }
                else if (!string.IsNullOrEmpty(searchModel.SupplierPONumber))
                {
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.SupplierPonumber == searchModel.SupplierPONumber);
                    whereClauseAssignment = whereClauseAssignment.Where(x => whereClauseSupplierPO.Any(x1 => x1.Id == x.SupplierPurchaseOrderId));
                }

                if (searchModel.MaterialDescription.HasEvoWildCardChar())
                {
                    whereClauseSupplierPO = whereClauseSupplierPO.WhereLike(x => x.MaterialDescription, searchModel.MaterialDescription, '*');
                    whereClauseAssignment = whereClauseAssignment.Where(x => whereClauseSupplierPO.Any(x1 => x1.Id == x.SupplierPurchaseOrderId));
                }
                else if (!string.IsNullOrEmpty(searchModel.MaterialDescription))
                {
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.MaterialDescription == searchModel.MaterialDescription);
                    whereClauseAssignment = whereClauseAssignment.Where(x => whereClauseSupplierPO.Any(x1 => x1.Id == x.SupplierPurchaseOrderId));
                }

                #endregion

                #region Contract
                IQueryable<DbModel.Contract> whereClauseContract = _dbContext.Contract.AsNoTracking();
                if (searchModel.ContractNumber.HasEvoWildCardChar())
                    whereClauseContract = whereClauseContract.WhereLike(x => x.ContractNumber, searchModel.ContractNumber, '*');
                else if (!string.IsNullOrEmpty(searchModel.ContractNumber))
                    whereClauseContract = whereClauseContract.Where(x => x.ContractNumber == searchModel.ContractNumber);

                if (searchModel.CustomerContractNumber.HasEvoWildCardChar())
                    whereClauseContract = whereClauseContract.WhereLike(x => x.CustomerContractNumber, searchModel.CustomerContractNumber, '*');
                else if (!string.IsNullOrEmpty(searchModel.CustomerContractNumber))
                    whereClauseContract = whereClauseContract.Where(x => x.CustomerContractNumber == searchModel.CustomerContractNumber);

                if (searchModel.CustomerId > 0)
                    whereClauseContract = whereClauseContract.Where(x => x.CustomerId == searchModel.CustomerId);
                #endregion

                #region Project
                IQueryable<DbModel.Project> whereClauseProject = _dbContext.Project.AsNoTracking();
                if (searchModel.CustomerProjectName.HasEvoWildCardChar())
                    whereClauseProject = whereClauseProject.WhereLike(x => x.CustomerProjectName, searchModel.CustomerProjectName, '*');
                else if (!string.IsNullOrEmpty(searchModel.CustomerProjectName))
                    whereClauseProject = whereClauseProject.Where(x => x.CustomerProjectName == searchModel.CustomerProjectName);
                #endregion

                whereClauseProject = whereClauseProject.Where(x => whereClauseContract.Any(x1 => x1.Id == x.ContractId));
                whereClauseAssignment = whereClauseAssignment.Where(x => whereClauseProject.Any(x1 => x1.Id == x.ProjectId));
                whereClauseVisit = whereClauseVisit?.Where(x => whereClauseAssignment.Any(x1 => x1.Id == x.AssignmentId));

                if (whereClauseVisit != null && searchModel.TotalCount <= 0)
                    searchModel.TotalCount = whereClauseVisit.AsNoTracking().Count();

                if (searchModel.TotalCount > 0)
                {
                    IList<BaseVisit> baseVisit = new List<BaseVisit>();
                    if (searchModel.IsExport)
                    {
                        for (int i = 0; i <= searchModel.TotalCount; i += searchModel.FetchCount)
                        {
                            var dbData = whereClauseVisit.AsNoTracking().OrderBy(x => searchModel.OrderBy).Skip(i).Take(searchModel.FetchCount);
                            var domData = MapData(dbData, searchModel.TotalCount);
                            baseVisit.AddRange(domData);
                        }
                    }
                    else
                    {
                        if (searchModel.ModuleName != "VST")
                            whereClauseVisit = whereClauseVisit.AsNoTracking()?.OrderBy(x => searchModel.OrderBy).Skip(searchModel.OffSet).Take(searchModel.FetchCount);
                        var domData = MapData(whereClauseVisit, searchModel.TotalCount);
                        baseVisit.AddRange(domData);
                    }

                    if (baseVisit?.Any() == true)
                    {
                        result.Header = GetHeaderList();
                        result.BaseVisit = baseVisit;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), searchModel);
            }
            finally
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
            return result;
        }

        private List<DomainModel.BaseVisit> MapData(IQueryable<DbModel.Visit> dbData, int totalCount)
        {
            var domData = dbData?.Select(x => new DomainModel.BaseVisit
            {
                VisitId = x.Id,
                VisitAssignmentId = x.AssignmentId,
                VisitNumber = x.VisitNumber,
                VisitStartDate = x.FromDate,
                VisitReportSentToCustomerDate = x.ReportSentToCustomerDate,
                VisitReportNumber = x.ReportNumber,
                VisitCustomerName = x.Assignment.Project.Contract.Customer.Name,
                VisitContractNumber = x.Assignment.Project.Contract.ContractNumber,
                VisitProjectNumber = x.Assignment.Project.ProjectNumber,
                VisitCustomerContractNumber = x.Assignment.Project.Contract.CustomerContractNumber,
                VisitAssignmentNumber = x.Assignment.AssignmentNumber,
                VisitCustomerProjectName = x.Assignment.Project.CustomerProjectName,
                VisitCustomerProjectNumber = x.Assignment.Project.CustomerProjectNumber,
                VisitContractCompany = x.Assignment.ContractCompany.Name,
                VisitContractCompanyCode = x.Assignment.ContractCompany.Code,
                VisitOperatingCompany = x.Assignment.OperatingCompany.Name,
                VisitStatus = x.VisitStatus,
                VisitSupplierPONumber = x.Assignment.SupplierPurchaseOrder.SupplierPonumber,
                VisitMaterialDescription = x.Assignment.SupplierPurchaseOrder.MaterialDescription,
                TechSpecialists = x.VisitTechnicalSpecialist.Select(x1 => new DomainModel.TechnicalSpecialist
                {
                    VisitId = x1.VisitId,
                    FirstName = x1.TechnicalSpecialist.FirstName,
                    LastName = x1.TechnicalSpecialist.LastName,
                    Pin = x1.TechnicalSpecialist.Pin,
                    LoginName = x1.TechnicalSpecialist.LogInName
                }).ToList(),
                VisitNotificationReference = x.NotificationReference,
                VisitSupplier = x.Supplier.SupplierName,
                VisitContractCoordinator = x.Assignment.ContractCompanyCoordinator.Name,
                VisitOperatingCompanyCoordinator = x.Assignment.OperatingCompanyCoordinator.Name,
                FinalVisit = x.IsFinalVisit.ToYesNo(),
                TotalCount = totalCount,
                SupplierId = (int)x.SupplierId
            })?.ToList();

            return domData;
        }

        private IQueryable<DbModel.Visit> SearchFilter(DomainModel.VisitSearch searchModel)
        {
            IQueryable<DbModel.Visit> whereClause = GetAll().AsNoTracking();
            if (searchModel.AssignmentId != null)
                whereClause = whereClause.Where(x => x.Assignment.Id == searchModel.AssignmentId);
            if (searchModel.VisitIds?.Count > 0)
            {
                var visitID = searchModel.VisitIds.Select(x => x).ToList();
                whereClause = whereClause.Where(x => visitID.Contains(x.Id));
            }

            //Condition for Only View Visit Rights with No CH and OC Company got selected. - ITK D - 669
            if (searchModel.IsOnlyViewVisit && !string.IsNullOrEmpty(searchModel.LoggedInCompanyCode))
            {
                //Contract Holding Company Code or Operating Company Code search with Logged in Company Code
                whereClause = whereClause.Where(x => (x.Assignment.ContractCompany.Code == searchModel.LoggedInCompanyCode || x.Assignment.OperatingCompany.Code == searchModel.LoggedInCompanyCode));
            }
            else
            {
                if (!string.IsNullOrEmpty(searchModel.ContractHoldingCompanyCode) || !string.IsNullOrEmpty(searchModel.OperatingCompanyCode))
                {
                    whereClause = whereClause.Where(x => (x.Assignment.ContractCompany.Code == searchModel.LoggedInCompanyCode
                        || x.Assignment.OperatingCompany.Code == searchModel.LoggedInCompanyCode
                        || x.Assignment.Project.Contract.ParentContract.ContractHolderCompany.Code == searchModel.LoggedInCompanyCode));
                }
                if (!string.IsNullOrEmpty(searchModel.ContractHoldingCompanyCode))
                    whereClause = whereClause.Where(x => x.Assignment.ContractCompany.Code == searchModel.ContractHoldingCompanyCode);

                if (!string.IsNullOrEmpty(searchModel.OperatingCompanyCode))
                    whereClause = whereClause.Where(x => x.Assignment.OperatingCompany.Code == searchModel.OperatingCompanyCode);
            }

            if (searchModel.CustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.Customer.Name, searchModel.CustomerName, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.CustomerName))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerName) || x.Assignment.Project.Contract.Customer.Name == searchModel.CustomerName);
            }
            if (searchModel.AssignmentNubmer != null)
                whereClause = whereClause.Where(x => x.Assignment.AssignmentNumber == searchModel.AssignmentNubmer);

            if (searchModel.CustomerNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.Customer.Code, searchModel.CustomerNumber, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.CustomerNumber))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerNumber) || x.Assignment.Project.Contract.Customer.Code == searchModel.CustomerNumber);
            }
            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.ContractNumber))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Assignment.Project.Contract.ContractNumber == searchModel.ContractNumber);
            }
            if (searchModel.ReportNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.ReportNumber, searchModel.ReportNumber, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ReportNumber) || x.ReportNumber == searchModel.ReportNumber);

            if (!string.IsNullOrEmpty(searchModel.CHCoordinatorCode))
                whereClause = whereClause.Where(x => x.Assignment.ContractCompanyCoordinator.SamaccountName == searchModel.CHCoordinatorCode);

            if (searchModel.CustomerContractNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.CustomerContractNumber, searchModel.CustomerContractNumber, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.CustomerContractNumber))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerContractNumber) || x.Assignment.Project.Contract.CustomerContractNumber == searchModel.CustomerContractNumber);
            }
            if (searchModel.FromDate != null || searchModel.ToDate != null)
            {
                if (searchModel.ToDate == null)
                {
                    whereClause = whereClause.Where(x => x.FromDate >= searchModel.FromDate.Value.Date);
                }
                else if (searchModel.FromDate == null)
                {
                    whereClause = whereClause.Where(x => x.FromDate <= searchModel.ToDate.Value.Date);
                }
                else
                {
                    whereClause = whereClause.Where(x => x.FromDate >= searchModel.FromDate.Value.Date && x.FromDate <= searchModel.ToDate.Value.Date);
                }
            }

            if (searchModel.ProjectNumber > 0)
                whereClause = whereClause.Where(x => x.Assignment.Project.ProjectNumber == searchModel.ProjectNumber);

            if (!string.IsNullOrEmpty(searchModel.OCCoordinatorCode))
                whereClause = whereClause.Where(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName == searchModel.OCCoordinatorCode);

            if (searchModel.CustomerProjectName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.CustomerProjectName, searchModel.CustomerProjectName, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.CustomerProjectName))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerProjectName) || x.Assignment.Project.CustomerProjectName == searchModel.CustomerProjectName);
            }
            if (searchModel.SupplierPONumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.SupplierPurchaseOrder.SupplierPonumber, searchModel.SupplierPONumber, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.SupplierPONumber))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPONumber) || x.Assignment.SupplierPurchaseOrder.SupplierPonumber == searchModel.SupplierPONumber);
            }
            if (!string.IsNullOrEmpty(searchModel.TechnicalSpecialist))
            {
                if (searchModel.TechnicalSpecialist.HasEvoWildCardChar())
                {
                    searchModel.TechnicalSpecialist = searchModel.TechnicalSpecialist.Trim('*');
                    whereClause = whereClause.Where(x => x.VisitTechnicalSpecialist.Any(ts => ts.TechnicalSpecialist.LastName.StartsWith(searchModel.TechnicalSpecialist)));
                }
                else
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TechnicalSpecialist) || x.VisitTechnicalSpecialist.Any(ts => searchModel.TechnicalSpecialist == ts.TechnicalSpecialist.LastName));
            }

            if (searchModel.SupplierSubSupplier.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Supplier.SupplierName, searchModel.SupplierSubSupplier, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.SupplierSubSupplier))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierSubSupplier) || x.Supplier.SupplierName == searchModel.SupplierSubSupplier);
            }
            if (searchModel.NotificationReference.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.NotificationReference, searchModel.NotificationReference, '*');
            else
            {
                if (!string.IsNullOrEmpty(searchModel.NotificationReference))
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.NotificationReference) || x.NotificationReference == searchModel.NotificationReference);
            }
            return whereClause;
        }

        private IQueryable<DbModel.Visit> VisitDocumentFilter(DomainModel.BaseVisit searchModel)
        {
            if (searchModel.VisitAssignmentId > 0)
                return _dbContext.Visit.Where(x => x.AssignmentId == searchModel.VisitAssignmentId);

            return null;
        }

        private IQueryable<DbModel.Visit> VisitFilter(DomainModel.BaseVisit searchModel)
        {
            IQueryable<DbModel.Visit> whereClause = _dbContext.Visit;
            if (searchModel.VisitId > 0)
                whereClause = whereClause.Where(x => x.Id == searchModel.VisitId);
            else
                whereClause = whereClause.Where(x => searchModel.VisitId > 0 || x.Id == searchModel.VisitId);

            if (searchModel.VisitAssignmentId > 0)
                whereClause = _dbContext.Visit.Where(x => x.AssignmentId == searchModel.VisitAssignmentId);

            return whereClause;
        }

        private IQueryable<DbModel.Visit> Filter(DomainModel.BaseVisit searchModel)
        {
            IQueryable<DbModel.Visit> whereClause = null;
            if (searchModel.VisitAssignmentId > 0)
                whereClause = _dbContext.Visit.Where(x => x.AssignmentId == searchModel.VisitAssignmentId);

            else
                whereClause = _dbContext.Visit.Include("VisitTechnicalSpecialist").Include("VisitTechnicalSpecialist.TechnicalSpecialist").Where(x => searchModel.VisitAssignmentId > 0 || x.AssignmentId == searchModel.VisitAssignmentId);

            return whereClause;
        }

        public DbModel.Visit GetDBVisitByID(DomainModel.BaseVisit searchModel)
        {
            return _dbContext.Visit.Where(x => x.Id == searchModel.VisitId).FirstOrDefault();
        }

        

        public List<DomainModel.Visit> GetSupplierList(DomainModel.BaseVisit searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = Filter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Visit>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Visit>().ToList();
        }

        public List<DomainModel.Visit> GetTechnicalSpecialistList(DomainModel.BaseVisit searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = Filter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Visit>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Visit>().ToList();
        }

        public int DeleteVisit(long visitId)
        {
            int count = 0;
            try
            {
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.Visit_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, visitId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "VisitId=" + visitId);
            }

            return count;
        }

        public long? GetFinalVisitId(DomainModel.BaseVisit searchModel)
        {
            try
            {
                IQueryable<DbModel.Visit> whereClause = Filter(searchModel);
                var validationResult = whereClause.ToList();
                if (validationResult != null && validationResult.Count > 0)
                {
                    return validationResult.Where(x => x.IsFinalVisit == true).Select(x => x.Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return null;
        }

        public VisitValidationData GetVisitValidationData(DomainModel.BaseVisit searchModel)
        {
            VisitValidationData visitValidationData = new VisitValidationData();

            try
            {
                long? visitId = searchModel.VisitId ?? 0;
                searchModel.VisitId = null;
                var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
                IQueryable<DbModel.Visit> whereClause = Filter(searchModel);
                IQueryable<DbModel.Visit> WhereQuearable = whereClause;
                var validationResult = whereClause.ToList();
                var visitList = WhereQuearable.ProjectTo<DomainModel.BaseVisit>().OrderBy(x => x.Id).ToList();
                if (validationResult != null && validationResult.Count > 0)
                {
                    visitValidationData.FirstVisitId = validationResult.OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefault();
                    visitValidationData.LastVisitId = validationResult.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                    visitValidationData.LastVisitNumber = validationResult.OrderByDescending(x => x.Id).Select(x => x.VisitNumber).FirstOrDefault();
                    visitValidationData.HasFinalVisit = validationResult.Where(x => x.IsFinalVisit == true).Count() > 0 ? true : false;
                    visitValidationData.FinalVisitId = validationResult.Where(x => x.IsFinalVisit == true).Select(x => x.Id).FirstOrDefault();
                    visitValidationData.HasTBAStatusVisit = validationResult.Where(x => x.VisitStatus == "U").Count() > 0 ? true : false;
                    visitValidationData.AwaitingApprovalVisitId = validationResult.Where(x => x.VisitStatus == "C").OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                    visitValidationData.VisitAssignmentDates = visitList;
                    visitValidationData.HasRateUnitFinalVisit = false;
                    visitValidationData.Visits = visitList;
                    string[] visitStaus = { "Q", "T", "U", "W" };
                    List<long?> visitIds = visitList.Where(x => visitStaus.Contains(x.VisitStatus)).Select(x => x.VisitId).ToList();
                    visitValidationData.TechnicalSpecialists = _visitTechnicalSpecialistsAccountRepository.GetTechSpecForAssignment(visitIds);

                    visitValidationData.SupplierPerformanceNCRDate = new List<DomainModel.BaseVisit>();
                    var VisitIds = validationResult.Select(x => x.Id).ToList();
                    var filteredvisitSupplierPerformance = _dbContext.VisitSupplierPerformance.Where(x => x.NcrcloseOutDate == null && VisitIds.Contains(x.VisitId)).Select(x => x.VisitId).ToList();

                    if (filteredvisitSupplierPerformance != null && filteredvisitSupplierPerformance.Count() > 0)
                    {
                        visitValidationData.SupplierPerformanceNCRDate = visitList.Where(x => filteredvisitSupplierPerformance.Contains((long)x.VisitId)).ToList<DomainModel.BaseVisit>();
                    }

                    if (visitValidationData.VisitAssignmentDates != null && visitValidationData.VisitAssignmentDates.Count() > 0)
                    {
                        List<string> VisitStatus = new List<string> { "C", "Q", "T", "U" };
                        var FilteredVisits = visitValidationData.VisitAssignmentDates.Where(x => VisitStatus.Contains(x.VisitStatus) && x.VisitId > visitId).Select(x => x.VisitId).ToList();

                        if (FilteredVisits != null && FilteredVisits.Count() > 0)
                        {
                            visitValidationData.HasRateUnitFinalVisit = _dbContext.VisitTechnicalSpecialistAccountItemTime?.Where(x => FilteredVisits.Contains(x.VisitId)
                                        && ((x.ChargeTotalUnit > 0 && x.ChargeRate > 0) || (x.PayTotalUnit > 0 && x.PayRate > 0))).Count() > 0 ? true : false;
                            if (!visitValidationData.HasRateUnitFinalVisit)
                            {
                                visitValidationData.HasRateUnitFinalVisit = _dbContext.VisitTechnicalSpecialistAccountItemExpense?.Where(x => FilteredVisits.Contains(x.VisitId)
                                            && ((x.ChargeTotalUnit > 0 && x.ChargeRate > 0) || (x.PayTotalUnit > 0 && x.PayRate > 0))).Count() > 0 ? true : false;
                            }
                            if (!visitValidationData.HasRateUnitFinalVisit)
                            {
                                visitValidationData.HasRateUnitFinalVisit = _dbContext.VisitTechnicalSpecialistAccountItemTravel?.Where(x => FilteredVisits.Contains(x.VisitId)
                                            && ((x.ChargeTotalUnit > 0 && x.ChargeRate > 0) || (x.PayTotalUnit > 0 & x.PayRate > 0))).Count() > 0 ? true : false;
                            }
                            if (!visitValidationData.HasRateUnitFinalVisit)
                            {
                                visitValidationData.HasRateUnitFinalVisit = _dbContext.VisitTechnicalSpecialistAccountItemConsumable?.Where(x => FilteredVisits.Contains(x.VisitId)
                                            && ((x.ChargeTotalUnit > 0 && x.ChargeRate > 0) || (x.PayTotalUnit > 0 && x.PayRate > 0))).Count() > 0 ? true : false;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return visitValidationData;
        }


       

        public List<DomainModel.Visit> GetVisitsByAssignment(DomainModel.BaseVisit searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Visit>(searchModel);
            IQueryable<DbModel.Visit> whereClause = Filter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Visit>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Visit>().ToList();
        }

        //Update Assignment status completed if it is final visit
        public bool UpdateAssignmentFinalVisit(int assignmentId, string assignmentStatus)
        {
            bool bResult = false;
            try
            {
                DbModel.Assignment assignment = _dbContext.Assignment.Where(x => x.Id == assignmentId).FirstOrDefault();
                assignment.AssignmentStatus = assignmentStatus;
                _dbContext.Assignment.Update(assignment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "AssignmentId=" + assignmentId);
            }
            return bResult;
        }

        public string GetTemplate(string companyCode, CompanyMessageType companyMessageType, string emailKey)
        {
            string emailContent = string.Empty;
            if (!string.IsNullOrEmpty(companyCode))
                emailContent = _dbContext.CompanyMessage.Where(x => x.Company.Code == companyCode && x.MessageTypeId == Convert.ToInt32(companyMessageType))?
                                   .FirstOrDefault()?.Message;
            if (!string.IsNullOrEmpty(emailKey) && string.IsNullOrEmpty(emailContent))
                emailContent = _dbContext.SystemSetting.Where(x => x.KeyName == emailKey)?.FirstOrDefault()?.KeyValue;
            return emailContent;
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            var dbVisit = _dbContext.Visit.FromSql("SELECT TOP 1 Id, Evoid FROM visit.Visit with(nolock) ORDER By ID DESC")?.AsNoTracking()?.Select(x => new { x.Id, x.Evoid });
            //var dbVisit  =_dbContext.Visit.OrderByDescending(x => x.Id)?.Select(x => new { Id = x.Id, Evoid = x.Evoid });
            long? visitId = dbVisit?.FirstOrDefault()?.Evoid ?? 0;
            //var dbTimesheet = _dbContext.Timesheet.OrderByDescending(x => x.Id)?.Select(x => new { Id = x.Id, Evoid = x.Evoid });
            var dbTimesheet = _dbContext.Timesheet.FromSql("SELECT TOP 1 Id, Evoid FROM timesheet.Timesheet with(nolock) ORDER By ID DESC")?.AsNoTracking()?.Select(x => new { x.Id, x.Evoid });
            long? timesheetId = dbTimesheet?.FirstOrDefault()?.Evoid ?? 0;

            if (visitId == 0 && timesheetId == 0)
                return dbTimesheet?.FirstOrDefault()?.Id;

            if (visitId > timesheetId)
                return visitId;
            else
                return timesheetId;
        }

        public IList<DbModel.Visit> FetchVisits(IList<long> lstVisitId, params string[] includes)
        {
            IList<DbModel.Visit> dbVisits = null;
            IQueryable<DbModel.Visit> whereClause = _dbContext.Visit;
            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            dbVisits = whereClause.Where(x => lstVisitId.Contains(x.Id)).ToList();
            return dbVisits;
        }

        public IList<DbModel.Assignment> GetDBVisitAssignments(IList<int> assignmentId, params string[] includes)
        {
            IList<DbModel.Assignment> dbAssignments = null;

            IQueryable<DbModel.Assignment> whereClause = _dbContext.Assignment;

            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            dbAssignments = whereClause.Where(x => assignmentId.Contains(x.Id)).ToList();

            return dbAssignments;
        }

        public List<DbModel.Visit> GetSupplierPoVisitIds(int? supplierPOId)
        {
            List<DbModel.Visit> visitInfo = _dbContext.Visit.Where(x => x.Assignment.SupplierPurchaseOrderId == supplierPOId).Select(x1 => new DbModel.Visit { Id = x1.Id, ReportNumber = x1.ReportNumber }).ToList();
            return visitInfo;
        }

        public List<DbModel.Visit> GetAssignmentVisitIds(int? assignmentId)
        {
            List<DbModel.Visit> visitInfo = _dbContext.Visit.Where(x => x.AssignmentId == assignmentId).Select(x1 => new DbModel.Visit { Id = x1.Id, ReportNumber = x1.ReportNumber }).ToList();
            return visitInfo;
        }

       
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
        public void AddVisitHistory(long visitId, int? historyItemId, string changedBy)
        {
            try
            {
                DbModel.VisitHistory visitHistory = new DbModel.VisitHistory
                {
                    VisitId = visitId,
                    VisitHistoryDateTime = DateTime.Now,
                    HistoryItemId = historyItemId ?? 0,
                    ChangedBy = string.IsNullOrEmpty(changedBy) ? string.Empty : changedBy,
                    LastModification = DateTime.Now
                };
                _dbContext.VisitHistory.Add(visitHistory);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }
    }
}
