using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.Project.Domain.Enums;
using Evolution.Project.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Constants = Evolution.Common.Constants;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Infrastructure.Data
{
    public class ProjectRepository : GenericRepository<DbModel.Project>, IProjectRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ProjectRepository> _logger = null;

        public ProjectRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<ProjectRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public int GetCount(DomainModel.ProjectSearch searchModel)
        {
            IQueryable<DbModel.Project> whereClause = Filter(searchModel);
            var dbSearchModel = _mapper.Map<DbModel.Project>(searchModel);
            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.Count();
            else
                return whereClause.Where(expression).Count();
        }

        public IList<DomainModel.ProjectSearch> SearchProject(DomainModel.ProjectSearch searchModel)
        {
            IList<DomainModel.ProjectSearch> domProject = null;
            var whereClause = Filter(searchModel);
            if (searchModel.ProjectNumbers?.Count > 0)
            {
                var projNumbers = searchModel.ProjectNumbers.Select(x => x.ToString()).ToList();
                whereClause = whereClause.Where(x => projNumbers.Contains(x.ProjectNumber.ToString()));
            }
            if (searchModel.WorkFlowTypeIn != null)
            {
                var workFlowTypeIn = searchModel.WorkFlowTypeIn.Split(",");
                whereClause = whereClause.Where(x => workFlowTypeIn.Contains(x.WorkFlowType.Trim()));
            }
            if (whereClause?.Count() > 0)
            {
                searchModel.TotalCount = whereClause.Count();
                domProject = whereClause.OrderBy(x => searchModel.OrderBy).Skip(searchModel.OffSet).Take(searchModel.FetchCount).ProjectTo<DomainModel.ProjectSearch>().ToList();
                if (domProject != null && domProject.Count>0)
                    domProject.FirstOrDefault().TotalCount = searchModel.TotalCount;
            }
            return domProject;
        }

        public IList<DomainModel.Project> GetProjects(int projectNumber)
        {
            return _dbContext.Project.Where(x => x.Id == projectNumber)?.Select(x => new DomainModel.Project
            {
                Id = x.Id,
                ProjectType = x.ProjectType.Name,
                ProjectBudgetCurrency = x.Contract.BudgetCurrency,
                ProjectBudgetWarning = x.BudgetWarning ?? 0,
                ProjectBudgetHoursWarning = x.BudgetHoursWarning ?? 0,
                ContractCustomerCode = x.Contract.Customer.Code,
                ContractCustomerName = x.Contract.Customer.Name,
                ContractHoldingCompanyCode = x.Contract.ContractHolderCompany.Code,
                ContractHoldingCompanyName = x.Contract.ContractHolderCompany.Name,
                ContractNumber = x.Contract.ContractNumber,
                ProjectNumber = x.ProjectNumber,
                ProjectClientReportingRequirement = x.ProjectMessage.FirstOrDefault(m => m.ProjectId == x.Id && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.ReportingRequirements)).Message,
                ProjectCoordinatorName = x.Coordinator.Name,
                ProjectCoordinatorCode = x.Coordinator.SamaccountName,
                ProjectCustomerContact = x.CustomerProjectContact.ContactName,
                ProjectCustomerContactAddress = x.CustomerProjectAddress.Address,
                WorkFlowType = x.WorkFlowType,
                CustomerProjectName = x.CustomerProjectName,
                AssignmentParentContractCompany = x.Contract.ParentContract.ContractHolderCompany.Name,
                AssignmentParentContractCompanyCode = x.Contract.ParentContract.ContractHolderCompany.Code,
                AssignmentParentContractDiscount = x.Contract.ParentContractDiscountPercentage,
                ProjectAssignmentOperationNotes = x.ProjectMessage.FirstOrDefault(m => m.ProjectId == x.Id && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.OperationalNotes)).Message,
                ProjectStartDate = x.StartDate,
                ProjectEndDate = x.EndDate,
                ProjectBudgetValue = x.Budget,
                ProjectBudgetHoursUnit = x.BudgetHours,
                IsProjectForNewFacility = x.IsNewFacility,
                LastModification = x.LastModification,
                ModifiedBy = x.ModifiedBy,
                UpdateCount = x.UpdateCount

            })?.ToList();
        }

        public IList<DomainModel.Project> Search(DomainModel.ProjectSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Project>(searchModel);
            IQueryable<DbModel.Project> whereClause = Filter(searchModel);

            if (searchModel.ProjectNumbers?.Count > 0)
            {
                var projNumbers = searchModel.ProjectNumbers.Select(x => x.ToString()).ToList();
                whereClause = whereClause.Where(x => projNumbers.Contains(x.ProjectNumber.ToString()));
            }

            if (searchModel.WorkFlowTypeIn != null)
            {
                var workFlowTypeIn = searchModel.WorkFlowTypeIn.Split(",");
                whereClause = whereClause.Where(x => workFlowTypeIn.Contains(x.WorkFlowType.Trim()));
            }

            var expression = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.IsNewFacility), nameof(dbSearchModel.IsManagedServices), nameof(dbSearchModel.IsExtranetSummaryVisibleToClient), nameof(dbSearchModel.IsEreportProjectMapped) });

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Project>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Project>().ToList();
        }

        private IQueryable<DbModel.Project> Filter(DomainModel.ProjectSearch searchModel)
        {
            IQueryable<DbModel.Project> whereClause = _dbContext.Project;
            if (!string.IsNullOrEmpty(searchModel.ProjectStatus))
                whereClause = whereClause.Where(x => x.Status == searchModel.ProjectStatus);

            if (searchModel.ProjectNumber > 0)
                whereClause = whereClause.Where(x => x.ProjectNumber == searchModel.ProjectNumber);

            if (searchModel.ProjectNumbers?.Count > 0)
                whereClause = whereClause.Where(x => searchModel.ProjectNumbers.Contains(x.Id));

            if (searchModel.ContractCustomerId > 0)
                whereClause = whereClause.Where(x => x.Contract.CustomerId == searchModel.ContractCustomerId);

            if (searchModel.ContractHoldingCompanyCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Contract.ContractHolderCompany.Code, searchModel.ContractHoldingCompanyCode, '*');
            else if (!string.IsNullOrEmpty(searchModel.ContractHoldingCompanyCode))
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractHoldingCompanyCode) || x.Contract.ContractHolderCompany.Code == searchModel.ContractHoldingCompanyCode);

            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else if (!string.IsNullOrEmpty(searchModel.ContractNumber))
                whereClause = whereClause.Where(x => x.Contract.ContractNumber == searchModel.ContractNumber);

            if (searchModel.ContractHoldingCompanyName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Contract.ContractHolderCompany.Name, searchModel.ContractHoldingCompanyName, '*');
            else if (!string.IsNullOrEmpty(searchModel.ContractHoldingCompanyName))
                whereClause = whereClause.Where(x => x.Contract.ContractHolderCompany.Name == searchModel.ContractHoldingCompanyName);

            if (searchModel.CustomerProjectNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CustomerProjectNumber, searchModel.CustomerProjectNumber, '*');
            else if (!string.IsNullOrEmpty(searchModel.CustomerProjectNumber))
                whereClause = whereClause.Where(x => x.CustomerProjectNumber == searchModel.CustomerProjectNumber);

            if (searchModel.CustomerProjectName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CustomerProjectName, searchModel.CustomerProjectName, '*');
            else if (!string.IsNullOrEmpty(searchModel.CustomerProjectName))
                whereClause = whereClause.Where(x => x.CustomerProjectName == searchModel.CustomerProjectName);

            if (searchModel.ContractCustomerCode.HasEvoWildCardChar() && searchModel.ContractCustomerId <= 0)
                whereClause = whereClause.WhereLike(x => x.Contract.Customer.Code, searchModel.ContractCustomerCode, '*');
            else if (!string.IsNullOrEmpty(searchModel.ContractCustomerCode) && searchModel.ContractCustomerId <= 0)
                whereClause = whereClause.Where(x => x.Contract.Customer.Code == searchModel.ContractCustomerCode);

            if (searchModel.ContractCustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Contract.Customer.Name, searchModel.ContractCustomerName, '*');
            else if (!string.IsNullOrEmpty(searchModel.ContractCustomerName))
                whereClause = whereClause.Where(x => x.Contract.Customer.Name == searchModel.ContractCustomerName);

            if (searchModel.CompanyDivision.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CompanyDivision.Division.Name, searchModel.CompanyDivision, '*');
            else if (!string.IsNullOrEmpty(searchModel.CompanyDivision))
                whereClause = whereClause.Where(x => x.CompanyDivision.Division.Name == searchModel.CompanyDivision);

            if (searchModel.CompanyOffice.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CompanyOffice.OfficeName, searchModel.CompanyOffice, '*');
            else if (!string.IsNullOrEmpty(searchModel.CompanyOffice))
                whereClause = whereClause.Where(x => x.CompanyOffice.OfficeName == searchModel.CompanyOffice);

            if (searchModel.ProjectTypeId > 0)
                whereClause = whereClause.Where(x => x.ProjectTypeId == searchModel.ProjectTypeId);

            if (searchModel.ProjectType.HasEvoWildCardChar() && searchModel.ProjectTypeId <= 0)
                whereClause = whereClause.WhereLike(x => x.ProjectType.Name, searchModel.ProjectType, '*');
            else if (!string.IsNullOrEmpty(searchModel.ProjectType) && searchModel.ProjectTypeId <= 0)
                whereClause = whereClause.Where(x => x.ProjectType.Name == searchModel.ProjectType);

            if (searchModel.ProjectCoordinatorName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Coordinator.Name, searchModel.ProjectCoordinatorName, '*');
            else if (!string.IsNullOrEmpty(searchModel.ProjectCoordinatorName))
                whereClause = whereClause.Where(x => x.Coordinator.Name == searchModel.ProjectCoordinatorName);

            return whereClause;
        }

        public int DeleteProject(int projectId)
        {
            int count = -1;
            try
            {
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.Project_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "ProjectId=" + projectId);
            }
            return count;
        }

        public int DeleteProject(IList<DbModel.Project> projects)
        {
            var projectIds = projects?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteProject(projectIds);
        }

        public int DeleteProject(List<int> projectIds)
        {
            int count = 0;
            try
            {
                if (projectIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Project, SQLModuleActionType.Delete), string.Join(",", projectIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "ProjectIds=" + projectIds.ToString<int>());
            }

            return count;
        }

        public List<DbModel.SystemSetting> MailTemplate()
        {
            return _dbContext.SystemSetting.Where(x => x.KeyName == EmailKey.EmailNewProject.ToString()).ToList();
        }

        public List<int> GetContractProjectIds(string contractNumber)
        {
            var contractId = _dbContext.Contract.Where(x => x.ContractNumber == contractNumber)?.Select(x1 => x1.Id).ToList();
            List<int> projectIds = _dbContext.Project.Where(x => contractId.Contains(x.ContractId))?.Select(x1 => x1.Id).ToList();
            return projectIds;
        }

        public List<int> GetCustomerProjectIds(int? customerId)
        {
            var contractIds = _dbContext.Contract.Where(x => x.CustomerId == customerId)?.Select(x1 => x1.Id).ToList();
            List<int> projectIds = _dbContext.Project.Where(x => contractIds.Contains(x.ContractId))?.Select(x1 => x1.Id).ToList();
            return projectIds;
        }

        // public void Update(DbModel.Project entity, params Expression<Func<DbModel.Project, object>>[] updatedProperties)
        // {
        //     //Ensure only modified fields are updated.
        //     var dbEntityEntry = _dbContext.Entry(entity);
        //     if (updatedProperties != null && updatedProperties.Any())
        //     {
        //         //update explicitly mentioned properties
        //         foreach (var property in updatedProperties)
        //         {
        //             dbEntityEntry.Property(property).IsModified = true;
        //         }
        //         if (this.AutoSave)
        //         {
        //             _dbContext.SaveChanges();
        //         }
        //     }
        //     else
        //     {
        //         base.Update(entity);
        //     }
        // }

        public bool GetTsVisible()
        {
            return _dbContext.ModuleDocumentType.FirstOrDefault(x => x.Module.Code == "PRJ" && x.DocumentType.Name == "Evolution Email")?.Tsvisible ?? false;
        }
        //Added for D-1304 -Start
        public string ValidateContractBudget(DomainModel.Project projects, DbModel.Contract dbContracts)
        {
            IList<DbModel.Project> dbProjects = _dbContext.Project.Where(x => x.ContractId == dbContracts.Id && x.ProjectNumber != projects.ProjectNumber)
                                                .Select(x1 => new DbModel.Project { Id = x1.Id, Budget = x1.Budget, BudgetHours = x1.BudgetHours }).ToList();

            decimal? existingProjectBudget = dbProjects?.Sum(x => x.Budget);
            decimal? existingBudgetHours = dbProjects?.Sum(x => x.BudgetHours);

            decimal? totalBudgetValues = existingProjectBudget + projects.ProjectBudgetValue;
            decimal? totalBudgetHours = existingBudgetHours + projects.ProjectBudgetHoursUnit;
            if (dbContracts.Budget != 0 && totalBudgetValues > dbContracts.Budget)
            {
                string code = MessageType.ProjectBudgetValueExceedsContract.ToId();
                return code;
            }
            else if (dbContracts.BudgetHours != 0 && totalBudgetHours > dbContracts.BudgetHours)
            {
                string code = MessageType.ProjectBudgetHoursExceedsContract.ToId();
                return code;
            }
            else
            {
                return null;
            }
        }
        public string ValidateAssignmentBudget(DomainModel.Project projects)
        {
            IList<DbModel.Assignment> dbAssignments = _dbContext.Assignment.Where(x => x.ProjectId == projects.Id)?.Select(x1 => new DbModel.Assignment { Id = x1.Id, BudgetValue = x1.BudgetValue, BudgetHours = x1.BudgetHours }).ToList();
            decimal? existingAssignmentBudget = dbAssignments?.Sum(x => x.BudgetValue);
            decimal? existingAssignmentBudgetHours = dbAssignments?.Sum(x => x.BudgetHours);

            if (projects.ProjectBudgetValue != 0 && projects.ProjectBudgetValue < existingAssignmentBudget)
            {
                string code = MessageType.ProjectBudgetValueBelowAssignment.ToId();
                return code;
            }
            if (projects.ProjectBudgetHoursUnit != 0 && projects.ProjectBudgetHoursUnit < existingAssignmentBudgetHours)
            {
                string code = MessageType.ProjectBudgetHoursBelowAssignment.ToId();
                return code;
            }
            else
            {
                return null;
            }
        }
        //Added for D-1304 -End

        public List<DomainModel.Project> GetProjectBasedOnStatus(string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isVisit, bool isOperating, bool isNDT, int? CoordinatorId)
        {
            if (isVisit)
                return GetProjectBasedOnVisit(contractNumber, ContractHolderCompanyId, isApproved, isOperating, isNDT, CoordinatorId);
            else
                return GetProjectBasedOnTimeSheet(contractNumber, ContractHolderCompanyId, isApproved, isOperating, isNDT, CoordinatorId);
        }

        public IList<DomainModel.Project> GetProjectKPI(string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            if (isVisit)
                return GetVisitProjectKPI(contractNumber, ContractHolderCompanyId, isVisit, isOperating);
            return GetTimesheetProjectKPI(contractNumber, ContractHolderCompanyId, isVisit, isOperating);
        }

        private List<DomainModel.Project> GetVisitProjectKPI(string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            int contractID = _dbContext.Contract.FirstOrDefault(a => a.ContractNumber == contractNumber).Id;
            var ProjectData = (from visit in _dbContext.Visit
                               join assignment in _dbContext.Assignment on visit.AssignmentId equals assignment.Id
                               join project in _dbContext.Project on assignment.ProjectId equals project.Id into projectDetails
                               from finalProjects in projectDetails.DefaultIfEmpty()
                               join contract in _dbContext.Contract on finalProjects.ContractId equals contract.Id
                               where contract.Id == contractID || contract.ParentContractId == contractID
                               select new { visit, contract, finalProjects });

            ProjectData = ProjectData?.Where(a => ((a.contract.ContractHolderCompanyId == ContractHolderCompanyId
            || a.contract.InvoicingCompanyId == ContractHolderCompanyId) || a.visit.Assignment.OperatingCompanyId == ContractHolderCompanyId));

            List<DomainModel.Project> projects = ProjectData?.Select(a => new DomainModel.Project()
            {
                Id = a.finalProjects.Id,
                ProjectNumber = a.finalProjects.ProjectNumber,
                CustomerProjectName = a.finalProjects.CustomerProjectName,
                CustomerProjectNumber = a.finalProjects.CustomerProjectNumber
            })?.Distinct().ToList();
            return projects;
        }

        private List<DomainModel.Project> GetTimesheetProjectKPI(string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            int contractID = _dbContext.Contract.FirstOrDefault(a => a.ContractNumber == contractNumber).Id;
            var ProjectData = (from visit in _dbContext.Timesheet
                               join assignment in _dbContext.Assignment on visit.AssignmentId equals assignment.Id
                               join project in _dbContext.Project on assignment.ProjectId equals project.Id into projectDetails
                               from finalProjects in projectDetails.DefaultIfEmpty()
                               join contract in _dbContext.Contract on finalProjects.ContractId equals contract.Id
                               where contract.Id == contractID || contract.ParentContractId == contractID
                               select new { visit, contract, finalProjects });

            ProjectData = ProjectData?.Where(a => ((a.contract.ContractHolderCompanyId == ContractHolderCompanyId
            || a.contract.InvoicingCompanyId == ContractHolderCompanyId)
            || a.visit.Assignment.OperatingCompanyId == ContractHolderCompanyId));

            List<DomainModel.Project> projects = ProjectData?.Select(a => new DomainModel.Project()
            {
                Id = a.finalProjects.Id,
                ProjectNumber = a.finalProjects.ProjectNumber,
                CustomerProjectName = a.finalProjects.CustomerProjectName,
                CustomerProjectNumber = a.finalProjects.CustomerProjectNumber
            })?.Distinct().ToList();
            return projects;
        }

        private List<DomainModel.Project> GetProjectBasedOnTimeSheet(string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isOperating, bool isNDT, int? CoordinatorId)
        {
            List<DomainModel.Project> projectData = new List<DomainModel.Project>();
            var projects = _dbContext.Timesheet.Include("Assignment").Include("Assignment.Project");
            if (isApproved)
            {
                int contractID = _dbContext.Contract.FirstOrDefault(a => a.ContractNumber == contractNumber).Id;
                var TechnicalSpecialistAccountItem = _dbContext.TimesheetTechnicalSpecialistAccountItemTime.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                 visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                 .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0) && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && a.Visit.TimesheetStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Project()
                 {
                     Id = a.Visit.Assignment.Project.Id,
                     ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                     CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                     CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                     Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId,
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemExpense = _dbContext.TimesheetTechnicalSpecialistAccountItemExpense.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0) && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && a.Visit.TimesheetStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Project()
                 {
                     Id = a.Visit.Assignment.Project.Id,
                     ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                     CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                     CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                     Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId,
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemConsumable = _dbContext.TimesheetTechnicalSpecialistAccountItemConsumable.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID) && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0) && a.Visit.TimesheetStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                  new DomainModel.Project()
                  {
                      Id = a.Visit.Assignment.Project.Id,
                      ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                      CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                      CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                      Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                      MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId,
                  }).Distinct().ToList();

                var TechnicalSpecialistAccountItemTravel = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID) && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0) && a.Visit.TimesheetStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Project()
                 {
                     Id = a.Visit.Assignment.Project.Id,
                     ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                     CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                     CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                     Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId,
                 }).Distinct().ToList();

                projectData = TechnicalSpecialistAccountItem.Union(TechnicalSpecialistAccountItemTravel).Union(TechnicalSpecialistAccountItemExpense)
                   .Union(TechnicalSpecialistAccountItemConsumable).Distinct().ToList();

                if (isNDT)
                    projectData = projectData?.Where(a => a.Evolution1Id == Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
                else
                    projectData = projectData?.Where(a => a.Evolution1Id != Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();

                return projectData?.GroupBy(a => new { a.Id, a.ProjectNumber, a.CustomerProjectName, a.CustomerProjectNumber })?.Select(b => new DomainModel.Project()
                {
                    Id = b.Key.Id,
                    ProjectNumber = b.Key.ProjectNumber,
                    CustomerProjectName = b.Key.CustomerProjectName,
                    CustomerProjectNumber = b.Key.CustomerProjectNumber,
                })?.Distinct()?.ToList();
            }
            else
            {
                List<string> status = !isOperating ? new List<string>() { "C", "O", "J", "R" } : new List<string>() { "C", "J", "R" };
                int contractID = _dbContext.Contract.FirstOrDefault(a => a.ContractNumber == contractNumber).Id;

                projects = projects?.Where(a => (a.Assignment.Project.Contract.Id == contractID
                || a.Assignment.Project.Contract.ParentContractId == contractID) && status.Contains(a.TimesheetStatus));

                if (isOperating)
                    projects = projects?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
                else
                    projects = projects?.Where(a => a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId);

                if (CoordinatorId.HasValue)
                    projects = projects?.Where(a => a.Assignment.OperatingCompanyCoordinatorId == CoordinatorId.Value || a.Assignment.ContractCompanyCoordinatorId == CoordinatorId.Value);

                projectData = projects?.Select(a => new DomainModel.Project()
                {
                    Id = a.Assignment.Project.Id,
                    ProjectNumber = a.Assignment.Project.ProjectNumber,
                    CustomerProjectName = a.Assignment.Project.CustomerProjectName,
                    CustomerProjectNumber = a.Assignment.Project.CustomerProjectNumber
                }).Distinct().ToList();
            }
            return projectData;
        }

        private List<DomainModel.Project> GetProjectBasedOnVisit(string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isOperating, bool isNDT, int? CoordinatorId)
        {
            List<DomainModel.Project> projectData = new List<DomainModel.Project>();
            var projects = _dbContext.Visit.Include("Assignment").Include("Assignment.Project");
            if (isApproved)
            {
                int contractID = _dbContext.Contract.FirstOrDefault(a => a.ContractNumber == contractNumber).Id;

                var TechnicalSpecialistAccountItem = _dbContext.VisitTechnicalSpecialistAccountItemTime.Join(_dbContext.Visit, VTI => VTI.VisitId,
                 visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                 .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0)
                 && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && a.Visit.VisitStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Project()
                 {
                     Id = a.Visit.Assignment.Project.Id,
                     ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                     CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                     CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                     Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId,
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemExpense = _dbContext.VisitTechnicalSpecialistAccountItemExpense.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0)
                 && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && a.Visit.VisitStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Project()
                 {
                     Id = a.Visit.Assignment.Project.Id,
                     ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                     CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                     CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                     Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemConsumable = _dbContext.VisitTechnicalSpecialistAccountItemConsumable.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID)
                 && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0) && a.Visit.VisitStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                  new DomainModel.Project()
                  {
                      Id = a.Visit.Assignment.Project.Id,
                      ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                      CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                      CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                      Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                      MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId
                  }).Distinct().ToList();

                var TechnicalSpecialistAccountItemTravel = _dbContext.VisitTechnicalSpecialistAccountItemTravel.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract })
                .Where(a => (a.Contract.Id == contractID || a.Contract.ParentContractId == contractID)
                 && (a.VTI.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.VTI.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                 && ((a.VTI.ChargeRate * a.VTI.ChargeTotalUnit) != 0) && a.Visit.VisitStatus == "A" && a.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Project()
                 {
                     Id = a.Visit.Assignment.Project.Id,
                     ProjectNumber = a.Visit.Assignment.Project.ProjectNumber,
                     CustomerProjectName = a.Visit.Assignment.Project.CustomerProjectName,
                     CustomerProjectNumber = a.Visit.Assignment.Project.CustomerProjectNumber,
                     Evolution1Id = a.Visit.Assignment.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.Visit.Assignment.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                projectData = TechnicalSpecialistAccountItem.Union(TechnicalSpecialistAccountItemTravel).Union(TechnicalSpecialistAccountItemExpense)
                   .Union(TechnicalSpecialistAccountItemConsumable).Distinct().ToList();

                if (isNDT)
                    projectData = projectData?.Where(a => a.Evolution1Id == Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
                else
                    projectData = projectData?.Where(a => a.Evolution1Id != Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();

                return projectData?.GroupBy(a => new { a.Id, a.ProjectNumber, a.CustomerProjectName, a.CustomerProjectNumber })?.Select(b => new DomainModel.Project()
                {
                    Id = b.Key.Id,
                    ProjectNumber = b.Key.ProjectNumber,
                    CustomerProjectName = b.Key.CustomerProjectName,
                    CustomerProjectNumber = b.Key.CustomerProjectNumber,
                })?.Distinct()?.ToList();
            }
            else
            {
                List<string> status = !isOperating ? new List<string>() { "C", "N", "O", "J", "R" } : new List<string>() { "C", "N", "J", "R" };
                int contractID = _dbContext.Contract.FirstOrDefault(a => a.ContractNumber == contractNumber).Id;

                projects = projects?.Where(a => (a.Assignment.Project.Contract.Id == contractID
                || a.Assignment.Project.Contract.ParentContractId == contractID) && status.Contains(a.VisitStatus));

                if (isOperating)
                    projects = projects?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
                else
                    projects = projects?.Where(a => a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId);

                projects = projects?.Where(a => (a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId) || a.Assignment.OperatingCompanyId == ContractHolderCompanyId);

                if (CoordinatorId.HasValue)
                    projects = projects?.Where(a => a.Assignment.OperatingCompanyCoordinatorId == CoordinatorId.Value || a.Assignment.ContractCompanyCoordinatorId == CoordinatorId.Value);


                projectData = projects?.Select(a => new DomainModel.Project()
                {
                    Id = a.Assignment.Project.Id,
                    ProjectNumber = a.Assignment.Project.ProjectNumber,
                    CustomerProjectName = a.Assignment.Project.CustomerProjectName,
                    CustomerProjectNumber = a.Assignment.Project.CustomerProjectNumber
                })?.Distinct()?.ToList();
            }
            return projectData;
        }

    }
}
