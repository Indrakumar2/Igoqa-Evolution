using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.AuditLog.Domain.Enums;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Company.Domain.Enums;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Models.Timesheets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Infrastructure.Data
{
    public class TimesheetRepository : GenericRepository<DbModel.Timesheet>, ITimesheetRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetRepository> _logger = null;
        private readonly ITimesheetTechnicalSpecialistRepository _timesheetTechnicalSpecialistRepository = null;

        public TimesheetRepository(DbModel.EvolutionSqlDbContext dbContext,
                                    IMapper mapper,
                                    IAppLogger<TimesheetRepository> logger,
                                    ITimesheetTechnicalSpecialistRepository timesheetTechnicalSpecialistRepository) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
            _timesheetTechnicalSpecialistRepository = timesheetTechnicalSpecialistRepository;
        }

        public IList<DomainModel.BaseTimesheet> Search(BaseTimesheet searchModel)
        {
            var timesheetResult = this._dbContext.Timesheet
                                .Where(this.GetWhereExpression(searchModel))
                                 .Include("Assignment")
                                 .Include("Assignment.Project")
                                 .Include("Assignment.Project.Contract")
                                 .Include("Assignment.Project.Contract.Customer")
                                 .Include("Assignment.ContractCompanyCoordinator")
                                 .Include("Assignment.OperatingCompanyCoordinator")
                                 .Include("Assignment.ContractCompany")
                                 .Include("Assignment.OperatingCompany")
                                 .Include("TimesheetTechnicalSpecialist")
                                 .Include("TimesheetTechnicalSpecialist.TechnicalSpecialist")
                                .ProjectTo<DomainModel.BaseTimesheet>().ToList();
            return timesheetResult;
        }

        public IQueryable<DbModel.Timesheet> GetTimesheetForDocumentApproval(DomainModel.BaseTimesheet searchModel)
        {
            return Filter(searchModel);
        }

        public int GetCount(BaseTimesheet searchModel)
        {
            return _dbContext.Timesheet
                             .Where(this.GetWhereExpression(searchModel)).Count();
        }

        public IList<DomainModel.TimesheetSearch> GetSearchTimesheet(DomainModel.TimesheetSearch searchModel, params string[] includes)
        {
            List<DomainModel.TimesheetSearch> timesheets = null;
            IQueryable<DbModel.Timesheet> dbTimeSheets = GetAll();
            SearchFilter(searchModel, ref dbTimeSheets);
            if (includes?.Count() > 0)
            {
                includes.ToList().ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                        dbTimeSheets = dbTimeSheets.Include(x);
                });
            }

            timesheets = dbTimeSheets.ProjectTo<DomainModel.TimesheetSearch>().ToList();
            return timesheets;
        }

        private Expression<Func<DbModel.Timesheet, bool>> GetWhereExpression(BaseTimesheet searchModel)
        {
            Expression<Func<DbModel.Timesheet, bool>> expression = x => (searchModel.TimesheetAssignmentNumber == null || x.Assignment.AssignmentNumber == searchModel.TimesheetAssignmentNumber) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetContractCompany) || x.Assignment.ContractCompany.Name == searchModel.TimesheetContractCompany) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetContractCompanyCode) || x.Assignment.ContractCompany.Code == searchModel.TimesheetContractCompanyCode) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetContractCoordinator) || x.Assignment.ContractCompanyCoordinator.Name == searchModel.TimesheetContractCoordinator) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetContractNumber) || x.Assignment.Project.Contract.ContractNumber == searchModel.TimesheetContractNumber) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetCustomerCode) || x.Assignment.Project.Contract.Customer.Code == searchModel.TimesheetCustomerCode) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetCustomerName) || x.Assignment.Project.Contract.Customer.Name == searchModel.TimesheetCustomerName) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetOperatingCompany) || x.Assignment.OperatingCompany.Name == searchModel.TimesheetOperatingCompany || x.Assignment.ContractCompany.Name == searchModel.TimesheetOperatingCompany) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetOperatingCompanyCode) || x.Assignment.OperatingCompany.Code == searchModel.TimesheetOperatingCompanyCode || x.Assignment.ContractCompany.Code == searchModel.TimesheetOperatingCompanyCode) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetOperatingCoordinator) || x.Assignment.OperatingCompanyCoordinator.Name == searchModel.TimesheetOperatingCoordinator || x.Assignment.ContractCompanyCoordinator.Name == searchModel.TimesheetOperatingCoordinator) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetOperatingCoordinatorSamAcctName) || x.Assignment.OperatingCompanyCoordinator.SamaccountName == searchModel.TimesheetOperatingCoordinatorSamAcctName || x.Assignment.ContractCompanyCoordinator.SamaccountName == searchModel.TimesheetOperatingCoordinatorSamAcctName) &&
                                (searchModel.TimesheetProjectNumber == null || x.Assignment.Project.ProjectNumber == searchModel.TimesheetProjectNumber) &&
                                (searchModel.TechSpecialists == null || x.TimesheetTechnicalSpecialist.Any(ts => searchModel.TechSpecialists.Any(t => t.Pin == ts.TechnicalSpecialist.Pin))) &&
                                (string.IsNullOrEmpty(searchModel.TimesheetDescription) || x.TimesheetDescription == searchModel.TimesheetDescription) &&
                                ((string.IsNullOrEmpty(searchModel.TimesheetStatus) && x.TimesheetStatus != "A" && x.TimesheetStatus !="E")   || x.TimesheetStatus == searchModel.TimesheetStatus) &&
                                (searchModel.TimesheetFutureDays == null || x.FromDate <= DateTime.Now.AddDays(Convert.ToInt32(searchModel.TimesheetFutureDays)));
            return expression;
        }

        public List<DomainModel.BaseTimesheet> GetTimesheet(DomainModel.BaseTimesheet searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Timesheet>(searchModel);
            IQueryable<DbModel.Timesheet> whereClause = Filter(searchModel);

            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsApprovedByContractCompany) }));
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.BaseTimesheet>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.BaseTimesheet>().ToList();
        }

        public List<DomainModel.Timesheet> Get(DomainModel.Timesheet searchModel, params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DbModel.Timesheet>(searchModel);
            List<DomainModel.Timesheet> timesheets = null;
            IQueryable<DbModel.Timesheet> whereClause = null;
            SearchFilter(searchModel, ref whereClause);
            if (includes?.Count() > 0)
            {
                includes.ToList().ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                        whereClause = whereClause.Include(x);
                });
            }
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsApprovedByContractCompany) }));
            if (expression == null)
                timesheets = whereClause.ProjectTo<DomainModel.Timesheet>().ToList();
            else
                timesheets = whereClause.Where(expression).ProjectTo<DomainModel.Timesheet>().ToList();

            //Fetching all the techspecialist data
            if (timesheets != null && timesheets?.Count > 0)
            {
                var timesheetIds = timesheets.Select(x => x.TimesheetId).ToList();
                var assignment = timesheets.Select(x => new { x.TimesheetAssignmentId, x.TimesheetId }).FirstOrDefault();

                var assignmentTechSpecialists = _dbContext.AssignmentTechnicalSpecialist
                                                   .Where(x => x.AssignmentId == assignment.TimesheetAssignmentId)?
                                                   .Select(x => new DomainModel.TechnicalSpecialist
                                                   {
                                                       FirstName = x.TechnicalSpecialist.FirstName,
                                                       LastName = x.TechnicalSpecialist.LastName,
                                                       Pin = x.TechnicalSpecialist.Pin,
                                                       TimesheetId = assignment.TimesheetId,
                                                       ProfileStatus = x.TechnicalSpecialist.ProfileStatus.Name
                                                   }).ToList();

                timesheets = timesheets.Select(x =>
                  {
                      x.assignmentTechSpecialists = assignmentTechSpecialists;
                      return x;
                  }).ToList();
            }
            return timesheets;
        }

        private IQueryable<DbModel.Timesheet> Filter(DomainModel.BaseTimesheet searchModel)
        {
            IQueryable<DbModel.Timesheet> whereClause = null;
            if (searchModel.TimesheetAssignmentId > 0)
                whereClause = _dbContext.Timesheet.Where(x => x.AssignmentId == searchModel.TimesheetAssignmentId);
            else
                whereClause = _dbContext.Timesheet.Where(x => searchModel.TimesheetAssignmentId > 0 || x.AssignmentId == searchModel.TimesheetAssignmentId);

            return whereClause;
        }

        private List<HeaderList> GetHeaderList()
        {
            List<HeaderList> headerData = new List<HeaderList>
            {
                new HeaderList { Label = "Timesheet Number", Key = "timesheetNumber" },
                new HeaderList { Label = "Customer", Key = "timesheetCustomerName" },
                new HeaderList { Label = "Contract No", Key = "timesheetContractNumber" },
                new HeaderList { Label = "Project No", Key = "timesheetProjectNumber" },
                new HeaderList { Label = "TimesheetDescription", Key = "timesheetDescription" },
                new HeaderList { Label = "Assignment No", Key = "timesheetAssignmentNumber" },
                new HeaderList { Label = "CH Coordinator Name", Key = "timesheetContractHolderCoordinator" },
                new HeaderList { Label = "OC Coordinator Name", Key = "timesheetOperatingCompanyCoordinator" },
                new HeaderList { Label = "Timesheet Status", Key = "timesheetStatus" },
                new HeaderList { Label = "Timesheet Date", Key = "timesheetStartDate" },
                new HeaderList { Label = "Customer Project Name", Key = "customerProjectName" },
                new HeaderList { Label = "Report Number", Key = "visitReportNumber" },
                new HeaderList { Label = "Resource(s)", Key = "techSpecialists.FullName" },
                new HeaderList { Label = "Contract Company", Key = "timesheetContractHolderCompany" },
                new HeaderList { Label = "Operating Company", Key = "timesheetOperatingCompany" },
                new HeaderList { Label = "Notification Reference", Key = "visitNotificationReference" }
            };

            return headerData;
        }

        //Added extra to check Search functionality
        public Result SearchTimesheets(DomainModel.TimesheetSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            Result result = new Result();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                #region Visit
                IQueryable<DbModel.Timesheet> whereClauseTimesheet = _dbContext.Timesheet.AsNoTracking();
                if (searchModel.TimesheetAssignmentId != null)
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => x.AssignmentId == searchModel.TimesheetAssignmentId);

                if (searchModel.TimesheetIds?.Count > 0)
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => searchModel.TimesheetIds.Contains(x.Id));

                if (!string.IsNullOrEmpty(searchModel.TimesheetStatus))
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => x.TimesheetStatus == searchModel.TimesheetStatus);

                if (searchModel.TimesheetDescription.HasEvoWildCardChar())
                    whereClauseTimesheet = whereClauseTimesheet.WhereLike(x => x.TimesheetDescription, searchModel.TimesheetDescription, '*');
                else if (!string.IsNullOrEmpty(searchModel.TimesheetDescription))
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => x.TimesheetDescription == searchModel.TimesheetDescription);

                if (searchModel.TimesheetStartDate != null || searchModel.TimesheetEndDate != null)
                {
                    if (searchModel.TimesheetStartDate != null && searchModel.TimesheetEndDate != null)
                        whereClauseTimesheet = whereClauseTimesheet.Where(x => x.FromDate >= searchModel.TimesheetStartDate.Value.Date && x.FromDate <= searchModel.TimesheetEndDate.Value.Date);
                    else if (searchModel.TimesheetEndDate == null)
                        whereClauseTimesheet = whereClauseTimesheet.Where(x => x.FromDate >= searchModel.TimesheetStartDate.Value.Date);
                    else if (searchModel.TimesheetStartDate == null)
                        whereClauseTimesheet = whereClauseTimesheet.Where(x => x.FromDate <= searchModel.TimesheetEndDate.Value.Date);
                }

                if (searchModel.TechnicalSpecialistName.HasEvoWildCardChar())
                {
                    searchModel.TechnicalSpecialistName = searchModel.TechnicalSpecialistName.Trim('*');
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => x.TimesheetTechnicalSpecialist.Any(ts => ts.TechnicalSpecialist.LastName.StartsWith(searchModel.TechnicalSpecialistName)));
                }
                else if (!string.IsNullOrEmpty(searchModel.TechnicalSpecialistName))
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => string.IsNullOrEmpty(searchModel.TechnicalSpecialistName) || x.TimesheetTechnicalSpecialist.Any(ts => searchModel.TechnicalSpecialistName == ts.TechnicalSpecialist.LastName));
                #endregion

                #region Assignment
                IQueryable<DbModel.Assignment> whereClauseAssignment = _dbContext.Assignment.AsNoTracking();
                if (searchModel.TimesheetProjectNumber > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.ProjectId == searchModel.TimesheetProjectNumber);
                if (searchModel.TimesheetAssignmentNumber > 0)
                    whereClauseAssignment = whereClauseAssignment.Where(x => x.AssignmentNumber == searchModel.TimesheetAssignmentNumber);

                if (searchModel.IsOnlyViewTimesheet && searchModel.LoggedInCompanyId > 0)
                    whereClauseTimesheet = whereClauseTimesheet.Where(x => (x.Assignment.ContractCompanyId == searchModel.LoggedInCompanyId || x.Assignment.OperatingCompanyId == searchModel.LoggedInCompanyId));
                else
                {
                    if (searchModel.ContractCompanyId > 0 || searchModel.OperatingCompanyId > 0)
                    {
                        whereClauseTimesheet = whereClauseTimesheet.Where(x => (x.Assignment.ContractCompanyId == searchModel.LoggedInCompanyId
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

                #region Contract
                IQueryable<DbModel.Contract> whereClauseContract = _dbContext.Contract.AsNoTracking();
                if (searchModel.TimesheetContractNumber.HasEvoWildCardChar())
                    whereClauseContract = whereClauseContract.WhereLike(x => x.ContractNumber, searchModel.TimesheetContractNumber, '*');
                else if (!string.IsNullOrEmpty(searchModel.TimesheetContractNumber))
                    whereClauseContract = whereClauseContract.Where(x => x.ContractNumber == searchModel.TimesheetContractNumber);

                if (searchModel.CustomerId > 0)
                    whereClauseContract = whereClauseContract.Where(x => x.CustomerId == searchModel.CustomerId);
                #endregion

                #region Project
                IQueryable<DbModel.Project> whereClauseProject = _dbContext.Project.AsNoTracking();
                #endregion

                whereClauseProject = whereClauseProject.Where(x => whereClauseContract.Any(x1 => x1.Id == x.ContractId));
                whereClauseAssignment = whereClauseAssignment.Where(x => whereClauseProject.Any(x1 => x1.Id == x.ProjectId));
                whereClauseTimesheet = whereClauseTimesheet?.Where(x => whereClauseAssignment.Any(x1 => x1.Id == x.AssignmentId));

                if (whereClauseTimesheet != null && searchModel.TotalCount <= 0)
                    searchModel.TotalCount = whereClauseTimesheet.AsNoTracking().Count();

                if (searchModel.TotalCount > 0)
                {
                    IList<TimesheetSearch> timesheetSearches = new List<TimesheetSearch>();
                    if (searchModel.IsExport == true)
                    {
                        for (int i = 0; i <= searchModel.TotalCount; i += searchModel.FetchCount)
                        {
                            var dbData = whereClauseTimesheet.AsNoTracking().OrderBy(x => searchModel.OrderBy).Skip(i).Take(searchModel.FetchCount);
                            var domData = MapData(dbData, searchModel.TotalCount);
                            timesheetSearches.AddRange(domData);
                        }
                    }
                    else
                    {
                        if (searchModel.ModuleName != "TIME")
                            whereClauseTimesheet = whereClauseTimesheet.AsNoTracking()?.OrderBy(x => searchModel.OrderBy).Skip(searchModel.OffSet).Take(searchModel.FetchCount);
                        var domData = MapData(whereClauseTimesheet, searchModel.TotalCount);
                        timesheetSearches.AddRange(domData);
                    }

                    if (timesheetSearches?.Any() == true)
                    {
                        result.Header = GetHeaderList();
                        result.TimesheetSearch = timesheetSearches;
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

        private List<DomainModel.TimesheetSearch> MapData(IQueryable<DbModel.Timesheet> dbData, int totalCount)
        {
            var domData = dbData?.Select(x => new DomainModel.TimesheetSearch
            {
                TimesheetId = x.Id,
                TimesheetAssignmentId = x.AssignmentId,
                TimesheetNumber = x.TimesheetNumber,
                TimesheetStartDate = x.FromDate,
                TimesheetDescription = x.TimesheetDescription,
                TimesheetCustomerName = x.Assignment.Project.Contract.Customer.Name,
                TimesheetContractNumber = x.Assignment.Project.Contract.ContractNumber,
                TimesheetProjectNumber = x.Assignment.Project.ProjectNumber,
                TimesheetAssignmentNumber = x.Assignment.AssignmentNumber,
                CustomerProjectName = x.Assignment.Project.CustomerProjectName,
                TimesheetContractHolderCompany = x.Assignment.ContractCompany.Name,
                TimesheetContractHolderCompanyCode = x.Assignment.ContractCompany.Code,
                TimesheetOperatingCompany = x.Assignment.OperatingCompany.Name,
                TimesheetStatus = x.TimesheetStatus,
                TechSpecialists = x.TimesheetTechnicalSpecialist.Select(x1 => new DomainModel.TechnicalSpecialist
                {
                    TimesheetId = x1.TimesheetId,
                    FirstName = x1.TechnicalSpecialist.FirstName,
                    LastName = x1.TechnicalSpecialist.LastName,
                    Pin = x1.TechnicalSpecialist.Pin,
                    LoginName = x1.TechnicalSpecialist.LogInName
                }).ToList(),
                TimesheetContractHolderCoordinator = x.Assignment.ContractCompanyCoordinator.Name,
                TimesheetOperatingCompanyCoordinator = x.Assignment.OperatingCompanyCoordinator.Name,
                TotalCount = totalCount
            })?.ToList();

            return domData;
        }

        //Used in Searching from Edit records before changing code - Scrapped
        private void SearchFilter(DomainModel.TimesheetSearch searchModel, ref IQueryable<DbModel.Timesheet> whereClause)
        {
            if (whereClause == null)
                whereClause = _dbContext.Timesheet.Where(x => searchModel.TimesheetId > 0 || x.Id == searchModel.TimesheetId);

            if (searchModel.TimesheetIds?.Count > 0)
            {
                var timesheetID = searchModel.TimesheetIds.Select(x => x).ToList();
                whereClause = whereClause.Where(x => timesheetID.Contains(x.Id));
            }

            //Condition for Only View Visit Rights with No CH and OC Company got selected. - ITK D - 669
            if (searchModel.IsOnlyViewTimesheet && !string.IsNullOrEmpty(searchModel.LoggedInCompanyCode))
            {
                //Contract Holding Company Code or Operating Company Code search with Logged in Company Code
                whereClause = whereClause.Where(x => (x.Assignment.ContractCompany.Code == searchModel.LoggedInCompanyCode || x.Assignment.OperatingCompany.Code == searchModel.LoggedInCompanyCode));
            }
            else
            {
                if (!string.IsNullOrEmpty(searchModel.TimesheetContractHolderCompany) || !string.IsNullOrEmpty(searchModel.TimesheetOperatingCompany))
                {
                    whereClause = whereClause.Where(x => (x.Assignment.ContractCompany.Code == searchModel.LoggedInCompanyCode
                        || x.Assignment.OperatingCompany.Code == searchModel.LoggedInCompanyCode
                        || x.Assignment.Project.Contract.ParentContract.ContractHolderCompany.Code == searchModel.LoggedInCompanyCode));
                }
                if (!string.IsNullOrEmpty(searchModel.TimesheetContractHolderCompany))
                    whereClause = whereClause.Where(x => x.Assignment.ContractCompany.Code == searchModel.TimesheetContractHolderCompany);

                if (!string.IsNullOrEmpty(searchModel.TimesheetOperatingCompany))
                    whereClause = whereClause.Where(x => x.Assignment.OperatingCompany.Code == searchModel.TimesheetOperatingCompany);
            }

            //Wildcard Search for Customer Code
            if (searchModel.TimesheetCustomerCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.Customer.Code, searchModel.TimesheetCustomerCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TimesheetCustomerCode) || x.Assignment.Project.Contract.Customer.Code == searchModel.TimesheetCustomerCode);

            if (searchModel.TimesheetCustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.Customer.Name, searchModel.TimesheetCustomerName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TimesheetCustomerName) || x.Assignment.Project.Contract.Customer.Name == searchModel.TimesheetCustomerName);

            if (searchModel.TimesheetStartDate != null || searchModel.TimesheetEndDate != null)
            {
                if (searchModel.TimesheetStartDate != null && searchModel.TimesheetEndDate != null)
                {
                    whereClause = whereClause.Where(x => x.FromDate >= searchModel.TimesheetStartDate && x.FromDate <= searchModel.TimesheetEndDate);
                }
                else if (searchModel.TimesheetStartDate != null && searchModel.TimesheetEndDate == null)
                {
                    whereClause = whereClause.Where(x => x.FromDate >= searchModel.TimesheetStartDate);
                }
                else
                {
                    whereClause = whereClause.Where(x => x.FromDate <= searchModel.TimesheetEndDate);
                }
            }

            if (!string.IsNullOrEmpty(searchModel.TimesheetStatus))
                whereClause = whereClause.Where(x => x.TimesheetStatus == searchModel.TimesheetStatus.ToTimeSheetStatus());

            if (!string.IsNullOrEmpty(searchModel.TimesheetDescription))
                whereClause = whereClause.WhereLike(x => x.TimesheetDescription, searchModel.TimesheetDescription, '*');

            if (!string.IsNullOrEmpty(searchModel.TimesheetContractNumber))
                whereClause = whereClause.Where(x => x.Assignment.Project.Contract.ContractNumber == searchModel.TimesheetContractNumber);

            if (searchModel.TimesheetProjectNumber != 0 && searchModel.TimesheetProjectNumber != null)
                whereClause = whereClause.Where(x => x.Assignment.Project.ProjectNumber == searchModel.TimesheetProjectNumber);

            if (searchModel.TimesheetAssignmentNumber != 0 && searchModel.TimesheetAssignmentNumber != null)
                whereClause = whereClause.Where(x => x.Assignment.AssignmentNumber == searchModel.TimesheetAssignmentNumber);


            if (!string.IsNullOrEmpty(searchModel.TimesheetContractHolderCoordinatorCode))
                whereClause = whereClause.Where(x => x.Assignment.ContractCompanyCoordinator.SamaccountName == searchModel.TimesheetContractHolderCoordinatorCode);

            if (!string.IsNullOrEmpty(searchModel.TimesheetOperatingCompanyCoordinatorCode))
                whereClause = whereClause.Where(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName == searchModel.TimesheetOperatingCompanyCoordinatorCode);

            //Wildcard Search for Tech Specialist
            if (!string.IsNullOrEmpty(searchModel.TechnicalSpecialistName))
            {
                if (searchModel.TechnicalSpecialistName.HasEvoWildCardChar())
                {
                    searchModel.TechnicalSpecialistName = searchModel.TechnicalSpecialistName.Trim('*');
                    whereClause = whereClause.Where(x => x.TimesheetTechnicalSpecialist.Any(ts => ts.TechnicalSpecialist.LastName.StartsWith(searchModel.TechnicalSpecialistName)));
                }
                else
                    whereClause = whereClause.Where(x => x.TimesheetTechnicalSpecialist.Any(ts => searchModel.TechnicalSpecialistName == ts.TechnicalSpecialist.LastName));
            }

            //Wildcard Search for Tech Specialist
            if (searchModel.TechSpecialists != null)
                whereClause = whereClause.Where(x => x.TimesheetTechnicalSpecialist.Any(ts => searchModel.TechSpecialists.Any(t => t.Pin == ts.TechnicalSpecialist.Pin)));

            if (searchModel.TimesheetId > 0)
                whereClause = whereClause.Where(x => x.Id == searchModel.TimesheetId);

            if (searchModel.TimesheetAssignmentId != null && searchModel.TimesheetAssignmentId > 0)
                whereClause = whereClause.Where(x => x.Assignment.Id == searchModel.TimesheetAssignmentId);
        }

        private void SearchFilter(DomainModel.Timesheet searchModel, ref IQueryable<DbModel.Timesheet> whereClause)
        {
            if (whereClause == null)
                whereClause = _dbContext.Timesheet.Where(x => searchModel.TimesheetId > 0 || x.Id == searchModel.TimesheetId);

            //Wildcard Search for Customer Code
            if (searchModel.TimesheetCustomerCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.Customer.Code, searchModel.TimesheetCustomerCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TimesheetCustomerCode) || x.Assignment.Project.Contract.Customer.Code == searchModel.TimesheetCustomerCode);

            if (searchModel.TimesheetCustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Assignment.Project.Contract.Customer.Name, searchModel.TimesheetCustomerName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TimesheetCustomerName) || x.Assignment.Project.Contract.Customer.Name == searchModel.TimesheetCustomerName);

            if (searchModel.TimesheetStartDate != null || searchModel.TimesheetEndDate != null)
            {
                if (searchModel.TimesheetStartDate != null && searchModel.TimesheetEndDate != null)
                {
                    whereClause = whereClause.Where(x => x.FromDate >= searchModel.TimesheetStartDate && x.FromDate <= searchModel.TimesheetEndDate);
                }
                else if (searchModel.TimesheetStartDate != null && searchModel.TimesheetEndDate == null)
                {
                    whereClause = whereClause.Where(x => x.FromDate >= searchModel.TimesheetStartDate);
                }
                else
                {
                    whereClause = whereClause.Where(x => x.FromDate <= searchModel.TimesheetEndDate);
                }
            }

            if (!string.IsNullOrEmpty(searchModel.TimesheetStatus))
                whereClause = whereClause.Where(x => x.TimesheetStatus == searchModel.TimesheetStatus.ToTimeSheetStatus());

            if (!string.IsNullOrEmpty(searchModel.TimesheetDescription))
                whereClause = whereClause.WhereLike(x => x.TimesheetDescription, searchModel.TimesheetDescription, '*');

            if (!string.IsNullOrEmpty(searchModel.TimesheetContractNumber))
                whereClause = whereClause.Where(x => x.Assignment.Project.Contract.ContractNumber == searchModel.TimesheetContractNumber);

            if (searchModel.TimesheetProjectNumber != 0 && searchModel.TimesheetProjectNumber != null)
                whereClause = whereClause.Where(x => x.Assignment.Project.ProjectNumber == searchModel.TimesheetProjectNumber);

            if (searchModel.TimesheetAssignmentNumber != 0 && searchModel.TimesheetAssignmentNumber != null)
                whereClause = whereClause.Where(x => x.Assignment.AssignmentNumber == searchModel.TimesheetAssignmentNumber);

            if (!string.IsNullOrEmpty(searchModel.TimesheetOperatingCompany))
                whereClause = whereClause.Where(x => x.Assignment.OperatingCompany.Name == searchModel.TimesheetOperatingCompany);

            if (!string.IsNullOrEmpty(searchModel.TimesheetOperatingCompanyCode))
                whereClause = whereClause.Where(x => x.Assignment.ContractCompany.Code == searchModel.TimesheetOperatingCompanyCode);

            //Wildcard Search for Tech Specialist
            if (searchModel.TechSpecialists != null)
                whereClause = whereClause.Where(x => x.TimesheetTechnicalSpecialist.Any(ts => searchModel.TechSpecialists.Any(t => t.Pin == ts.TechnicalSpecialist.Pin)));

            if (searchModel.TimesheetId > 0)
                whereClause = whereClause.Where(x => x.Id == searchModel.TimesheetId);
        }

        public int DeleteTimesheet(long timesheetId)
        {
            int count = 0;
            try
            {
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.Timesheet_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, timesheetId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "TimesheetId=" + timesheetId);
            }

            return count;
        }

        public TimesheetValidationData GetTimesheetValidationData(DomainModel.BaseTimesheet searchModel)
        {
            TimesheetValidationData timesheetValidationData = new TimesheetValidationData();

            try
            {
                var dbSearchModel = _mapper.Map<DbModel.Timesheet>(searchModel);
                IQueryable<DbModel.Timesheet> whereClause = Filter(searchModel);

                if (whereClause != null && whereClause.Count() > 0)
                {
                    timesheetValidationData.TimesheetAssignmentDates = whereClause.ProjectTo<DomainModel.BaseTimesheet>().OrderBy(x => x.Id).ToList();
                    string[] timesheetStaus = { "Q", "T", "U", "W", "N" };
                    List<long?> timesheetIds = timesheetValidationData.TimesheetAssignmentDates.Where(x => timesheetStaus.Contains(x.TimesheetStatus)).Select(x => x.TimesheetId).ToList();
                    timesheetValidationData.TechnicalSpecialists = _timesheetTechnicalSpecialistRepository.GetTechSpecForAssignment(timesheetIds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return timesheetValidationData;
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

        //public List<DbModel.SystemSetting> MailTemplate(string emailKey)
        //{
        //    return _dbContext.SystemSetting.Where(x => x.KeyName == emailKey).ToList();
        //}

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            // long? visitId = _dbContext.Visit.FromSql("SELECT TOP 1 * FROM visit.Visit ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
            var dbVisit = _dbContext.Visit.FromSql("SELECT TOP 1 Id,EvoId FROM visit.Visit with(NOLOCK) ORDER By ID DESC")?.AsNoTracking().Select(x => new DbModel.Visit() { Id = x.Id, Evoid = x.Evoid });
            long? visitId = dbVisit?.FirstOrDefault()?.Evoid ?? 0;
            var dbTimesheet = _dbContext.Timesheet.FromSql("SELECT TOP 1 Id,EvoId FROM timesheet.Timesheet with(NOLOCK) ORDER By ID DESC")?.AsNoTracking().Select(x => new DbModel.Timesheet() { Id = x.Id, Evoid = x.Evoid })?.FirstOrDefault();
            long? timesheetId = dbTimesheet?.Evoid ?? 0;

            if (visitId == 0 && timesheetId == 0)
                return dbTimesheet?.Id;
            if (visitId > timesheetId)
                return visitId;
            else
                return timesheetId;
        }

        public IList<DbModel.Timesheet> FetchTimesheets(IList<long> lstTimesheetId, params string[] includes)
        {
            IList<DbModel.Timesheet> dbTimesheets = null;
            IQueryable<DbModel.Timesheet> whereClause = _dbContext.Timesheet;
            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            dbTimesheets = whereClause.Where(x => lstTimesheetId.Contains(x.Id))?.ToList();
            return dbTimesheets;
        }

        public IList<DbModel.Assignment> GetDBTimesheetAssignments(IList<int> assignmentId, params string[] includes)
        {
            IList<DbModel.Assignment> dbAssignments = null;

            IQueryable<DbModel.Assignment> whereClause = _dbContext.Assignment;

            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            dbAssignments = whereClause.Where(x => assignmentId.Contains(x.Id)).ToList();

            return dbAssignments;
        }

        public IList<DbModel.TimesheetInterCompanyDiscount> GetAssignmentInterCompanyDiscounts(int assignmentId, long timesheetId)
        {
            var timesheetInterCompanyDiscounts = _dbContext.AssignmentInterCompanyDiscount.Where(x => x.AssignmentId == assignmentId).
                                                Select(x => new DbModel.TimesheetInterCompanyDiscount
                                                {
                                                    TimesheetId = timesheetId,
                                                    DiscountType = x.DiscountType,
                                                    CompanyId = x.CompanyId,
                                                    Description = x.Description,
                                                    Percentage = x.Percentage
                                                }).ToList();

            return timesheetInterCompanyDiscounts;
        }

        public List<DbModel.Timesheet> GetAssignmentTimesheetIds(int? assignmentId)
        {
            List<DbModel.Timesheet> timesheetInfo = _dbContext.Timesheet.Where(x => x.AssignmentId == assignmentId).Select(x1 => new DbModel.Timesheet { Id = x1.Id, TimesheetDescription = x1.TimesheetDescription }).ToList();
            return timesheetInfo;
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddTimesheetHistory(long timesheetId, int? historyItemId, string changedBy)
        {
            try
            {
                DbModel.TimesheetHistory timesheetHistory = new DbModel.TimesheetHistory
                {
                    TimesheetId = timesheetId,
                    TimesheetHistoryDateTime = DateTime.Now,
                    HistoryItemId = historyItemId ?? 0,
                    ChangedBy = string.IsNullOrEmpty(changedBy) ? string.Empty : changedBy,
                    LastModification = DateTime.Now
                };
                _dbContext.TimesheetHistory.Add(timesheetHistory);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }
    }
}



