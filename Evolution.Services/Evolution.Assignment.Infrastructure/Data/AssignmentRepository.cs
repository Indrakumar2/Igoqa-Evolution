using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Enums;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentRepository : GenericRepository<DbModel.Assignment>, IAssignmentRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private IAppLogger<AssignmentRepository> _logger = null;

        public AssignmentRepository(DbModel.EvolutionSqlDbContext dbContext, IAppLogger<AssignmentRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            _logger = logger;
        }

        public IList<DomainModel.AssignmentDashboard> Search(DomainModel.AssignmentSearch searchModel)
        {
            var dbAssignments = _dbContext.Assignment.Where(GetWhereExpression(searchModel));
            var domAssignment = dbAssignments.ProjectTo<DomainModel.AssignmentDashboard>().ToList();
            {
                var isVisit = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId > 0)?.Any(); ;
                var isTimesheet = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId == null || x.AssignmentSupplierPurchaseOrderId == 0)?.Any();
                var assignmentVisitTimesheet = this.GetAssignmentVisitTimesheetByGroup(dbAssignments, isVisit, isTimesheet);

                if (assignmentVisitTimesheet != null)
                    domAssignment = domAssignment?.GroupJoin(assignmentVisitTimesheet,
                            ass => (int)ass.AssignmentId,
                            vtAss => vtAss.AssignmentId,
                            (ass, vtAss) => new { ass, vtAss })
                        .Select(x =>
                        {
                            x.ass.AssignmentLastVisitFromDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.LastVisitFromDate : x.ass?.AssignmentFirstVisitDate; //If visit is null, we need to assign AssignmentFirstVisitDate
                            x.ass.AssignmentFirstVisitDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.FirstVisitDate : x.ass?.AssignmentFirstVisitDate;
                            x.ass.AssignmentLastVisitDate = x.vtAss?.FirstOrDefault()?.LastVisitDate;
                            x.ass.AssignmentExpectedCompleteDate = x.vtAss?.FirstOrDefault()?.ExpectedCompleteDate;
                            x.ass.AssignmentPercentageCompleted = x.vtAss?.FirstOrDefault()?.PercentageCompleted;
                            return x.ass;
                        }).ToList();

                return searchModel.IsInactiveAssignmentOnly ? this.GetInactiveAssignment<DomainModel.AssignmentDashboard>(domAssignment) : domAssignment;
            }
        }

        /* This endpoint is used to load dropdown values of Document Approval*/
        public IQueryable<DbModel.Assignment> GetAssignment(DomainModel.AssignmentSearch searchModel)
        {
            string[] includes = new string[] { };
            return Filter(searchModel, includes);
        }

        public Int32 GetCount(DomainModel.AssignmentSearch searchModel)
        {
            var dbAssignments = _dbContext.Assignment.Where(GetWhereExpression(searchModel))?.Select(x => new DbModel.Assignment { Id = x.Id, FirstVisitTimesheetStartDate = x.FirstVisitTimesheetStartDate });
            if (searchModel.IsInactiveAssignmentOnly)
            {
                var assignmentVisitTimesheet = this.GetAssignmentVisitTimesheetByGroup(dbAssignments, true, true);
                var dbSpecificAssignmentValue = dbAssignments?.Select(x => new DomainModel.Assignment()
                {
                    AssignmentId = x.Id,
                    AssignmentFirstVisitDate = x.FirstVisitTimesheetStartDate //If visit is null, we need this property to find InActive Assignment
                })?.ToList();
                if (assignmentVisitTimesheet != null)
                {
                    var assignments = dbSpecificAssignmentValue?.GroupJoin(assignmentVisitTimesheet,
                        ass => (int)ass.AssignmentId,
                        vtAss => vtAss.AssignmentId,
                        (ass, vtAss) => new { ass, vtAss })
                    .Select(x =>
                    {
                        x.ass.AssignmentLastVisitFromDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.LastVisitFromDate : x.ass.AssignmentFirstVisitDate; //If visit is null, we need to assign AssignmentFirstVisitDate
                        return x.ass;
                    }).ToList();

                    return this.GetInactiveAssignmentCount<DomainModel.Assignment>(assignments);
                }
                return 0;
            }

            return dbAssignments != null ? dbAssignments.Count() : 0;
        }

        private List<dynamic> GetAssignmentVisitTimesheetByGroup(IQueryable<DbModel.Assignment> dbAssignments, bool? isVisit = false, bool? isTimesheet = false)
        {
            try
            {
                IQueryable<AssignmentVisitTimesheet> dbVisits = null;
                IQueryable<AssignmentVisitTimesheet> dbTimesheets = null;
                if ((bool)isVisit)
                {
                    dbVisits = dbAssignments.Join(_dbContext.Visit,
                                    dbAssignment => new { AssignmentId = dbAssignment.Id },
                                    dbVisit => new { dbVisit.AssignmentId },
                                (dbAssignment, dbVisit) => new { dbAssignment, dbVisit })?
                                .Select(x => new
                                {
                                    AssignmentId = x.dbAssignment.Id,
                                    VisitId = x.dbVisit.Id,
                                    x.dbVisit.FromDate,
                                    x.dbVisit.ToDate,
                                    x.dbVisit.PercentageCompleted,
                                    x.dbVisit.ExpectedCompleteDate

                                })?
                                .GroupBy(x => x.AssignmentId)?
                                .Select(x => new AssignmentVisitTimesheet
                                {
                                    AssignmentId = x.Key,
                                    FirstVisitDate = x.Min(x1 => x1.FromDate),
                                    LastVisitFromDate = x.Max(x1 => x1.FromDate),
                                    LastVisitDate = x.FirstOrDefault(x1 => x1.FromDate == x.Max(x2 => x2.FromDate)).ToDate,
                                    PercentageCompleted = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.VisitId).FirstOrDefault().PercentageCompleted,
                                    ExpectedCompleteDate = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.VisitId).FirstOrDefault().ExpectedCompleteDate,
                                })?.AsNoTracking();
                }

                //var hhj=  dbVisits.ToList();
                if ((bool)isTimesheet)
                {
                    dbTimesheets = dbAssignments.Join(_dbContext.Timesheet,
                                    dbAssignment => new { AssignmentId = dbAssignment.Id },
                                    dbTimesheet => new { dbTimesheet.AssignmentId },
                               (dbAssignment, dbTimesheet) => new { dbAssignment, dbTimesheet })?
                               .Select(x => new
                               {
                                   AssignmentId = x.dbAssignment.Id,
                                   TimesheetId = x.dbTimesheet.Id,
                                   x.dbTimesheet.FromDate,
                                   x.dbTimesheet.ToDate,
                                   x.dbTimesheet.PercentageCompleted,
                                   x.dbTimesheet.ExpectedCompleteDate
                               })?
                               .GroupBy(x => x.AssignmentId)?
                               .Select(x => new AssignmentVisitTimesheet
                               {
                                   AssignmentId = x.Key,
                                   FirstVisitDate = x.Min(x1 => x1.FromDate),
                                   LastVisitFromDate = x.Max(x1 => x1.FromDate),
                                   LastVisitDate = x.FirstOrDefault(x1 => x1.FromDate == x.Max(x2 => x2.FromDate)).ToDate,
                                   PercentageCompleted = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.TimesheetId).FirstOrDefault().PercentageCompleted,
                                   ExpectedCompleteDate = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.TimesheetId).FirstOrDefault().ExpectedCompleteDate
                               })?.AsNoTracking();

                }

                return dbVisits != null && dbTimesheets != null ? dbVisits?.Union(dbTimesheets)?.ToList<object>() :
                    dbVisits != null ? dbVisits?.ToList<object>() : dbTimesheets?.ToList<object>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            //var fd = dbTimesheets.ToList();
            return null;
        }

        private Expression<Func<DbModel.Assignment, bool>> GetWhereExpression(DomainModel.AssignmentSearch searchModel)
        {
            Expression<Func<DbModel.Assignment, bool>> expression = x => (string.IsNullOrEmpty(searchModel.AssignmentStatus) || x.AssignmentStatus == searchModel.AssignmentStatus.ToAssignmentStatus()) &&
                                                //Added for Project Number- Document Approval                  
                                                (searchModel.AssignmentProjectNumber == null || x.ProjectId == searchModel.AssignmentProjectNumber) &&
                                                (
                                                    string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompanyCode) ||
                                                    x.ContractCompany.Code == searchModel.AssignmentContractHoldingCompanyCode ||
                                                    x.OperatingCompany.Code == searchModel.AssignmentContractHoldingCompanyCode
                                                ) &&
                                                //Contract Holding Company Name
                                                (
                                                    string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompany) ||
                                                    x.ContractCompany.Name == searchModel.AssignmentContractHoldingCompany ||
                                                     x.OperatingCompany.Name == searchModel.AssignmentOperatingCompany
                                                ) &&
                                                (
                                                   string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompanyCoordinator) ||
                                                   x.ContractCompanyCoordinator.Name == searchModel.AssignmentContractHoldingCompanyCoordinator ||
                                                   x.OperatingCompanyCoordinator.Name == searchModel.AssignmentContractHoldingCompanyCoordinator
                                                ) &&
                                                (
                                                   string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompanyCoordinatorSamAcctName) ||
                                                   x.ContractCompanyCoordinator.SamaccountName == searchModel.AssignmentContractHoldingCompanyCoordinatorSamAcctName ||
                                                   x.OperatingCompanyCoordinator.SamaccountName == searchModel.AssignmentContractHoldingCompanyCoordinatorSamAcctName
                                                ) &&

                                                //Operating Company Code
                                                (
                                                    string.IsNullOrEmpty(searchModel.AssignmentOperatingCompanyCode) ||
                                                    x.OperatingCompany.Code == searchModel.AssignmentOperatingCompanyCode ||
                                                    x.ContractCompany.Code == searchModel.AssignmentOperatingCompanyCode
                                                ) &&
                                                //Operating Company Name
                                                (
                                                    string.IsNullOrEmpty(searchModel.AssignmentOperatingCompany) ||
                                                    x.OperatingCompany.Name == searchModel.AssignmentOperatingCompany ||
                                                     x.ContractCompany.Name == searchModel.AssignmentOperatingCompany
                                                ) &&
                                                (
                                                    string.IsNullOrEmpty(searchModel.AssignmentOperatingCompanyCoordinator) ||
                                                    x.OperatingCompanyCoordinator.Name == searchModel.AssignmentOperatingCompanyCoordinator ||
                                                    x.ContractCompanyCoordinator.Name == searchModel.AssignmentOperatingCompanyCoordinator
                                                );

            return expression;
        }

        //private List<dynamic> GetAssignmentVisitByGroup(List<int> assignmentIds)
        //{
        //    var visits = _dbContext.Visit
        //                  .Where(x => assignmentIds.Contains(x.AssignmentId)).ToList();

        //    var finalVisits = visits?
        //                    .GroupBy(x => x.AssignmentId)
        //                    .Select(x => new
        //                    {
        //                        AssignmentId = x.Key,
        //                        FirstVisitDate = x.Min(x1 => x1.FromDate),//Changes for DM issue(D725)
        //                        LastVisitFromDate = x.Max(x1 => x1.FromDate),
        //                        LastVisitDate = x.FirstOrDefault(x1 => x1.FromDate == x.Max(x2 => x2.FromDate)).ToDate,
        //                        PercentageCompleted = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.Id).FirstOrDefault().PercentageCompleted,
        //                        ExpectedCompleteDate = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.Id).FirstOrDefault().ExpectedCompleteDate
        //                    });

        //    var timesheets = _dbContext.Timesheet
        //                    .Where(x => assignmentIds.Contains(x.AssignmentId))
        //                    .GroupBy(x => x.AssignmentId)
        //                    .Select(x => new
        //                    {
        //                        AssignmentId = x.Key,
        //                        FirstVisitDate = x.Min(x1 => x1.FromDate),//Changes for DM issue(D725)
        //                        LastVisitFromDate = x.Max(x1 => x1.FromDate),
        //                        LastVisitDate = x.FirstOrDefault(x1 => x1.FromDate == x.Max(x2 => x2.FromDate)).ToDate,
        //                        PercentageCompleted = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.Id).FirstOrDefault().PercentageCompleted,
        //                        ExpectedCompleteDate = x.OrderByDescending(x1 => x1.FromDate).ThenByDescending(x1 => x1.Id).FirstOrDefault().ExpectedCompleteDate
        //                    });

        //    return finalVisits?.Union(timesheets)?.ToList<object>();
        //}

        private int GetInactiveAssignmentCount<T>(List<T> models) where T : DomainModel.Assignment
        {
            /*Add Days changed to Add Months*/
            var value = models.Where(x => x.AssignmentLastVisitFromDate != null && ((DateTime)x.AssignmentLastVisitFromDate).Date.AddMonths(3) <= DateTime.Now.Date);
            if (value != null)
                return value.Count();
            else return 0;
        }

        private IList<T> GetInactiveAssignment<T>(List<T> models) where T : DomainModel.Assignment
        {
            /*Add Days changed to Add Months*/
            return models != null ?
            models.Where(x => x.AssignmentLastVisitFromDate != null && ((DateTime)x.AssignmentLastVisitFromDate).Date.AddMonths(3) <= DateTime.Now.Date).ToList().ConvertAll(x => (T)x)
            : new List<T>();
        }

        //This is called from Resource Module
        public DbModel.Assignment SearchAssignment(int assignmentId, params string[] includes)
        {
            IQueryable<DbModel.Assignment> whereClause = _dbContext.Assignment;

            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            return whereClause.FirstOrDefault(x => x.Id == assignmentId);
        }

        public List<DbModel.Assignment> GetAssignmentData(int supplierPOId)
        {
            List<DbModel.Assignment> assignemntList = _dbContext.Assignment.Where(a => a.SupplierPurchaseOrderId == supplierPOId ).ToList();
            return assignemntList;
        }

        //This is called from Assignment Search to filter records based on parameters
        private IQueryable<DbModel.Assignment> Filter(DomainModel.AssignmentSearch searchModel, params string[] includes)
        {
            IQueryable<DbModel.Assignment> whereClause = _dbContext.Assignment;

            if (includes != null && includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            //Condition for Only View Assignment Rights with No CH and OC Company got selected. - ITK D - 689
            if (searchModel.IsOnlyViewAssignment && !string.IsNullOrEmpty(searchModel.LoggedInCompanyCode))
            {
                //Contract Holding Company Code or Operating Company Code search with Logged in Company Code
                whereClause = whereClause.Where(x => (x.ContractCompany.Code == searchModel.LoggedInCompanyCode || x.OperatingCompany.Code == searchModel.LoggedInCompanyCode));

            }
            else
            {
                //Wildcard Search for Contract Holding Company Code
                whereClause = searchModel.AssignmentContractHoldingCompanyCode.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.ContractCompany.Code, searchModel.AssignmentContractHoldingCompanyCode, '*') : whereClause.Where(x => (string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompanyCode) || x.ContractCompany.Code == searchModel.AssignmentContractHoldingCompanyCode));

                //Wildcard Search for Contract Holding Company
                whereClause = searchModel.AssignmentContractHoldingCompany.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.ContractCompany.Name, searchModel.AssignmentContractHoldingCompany, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompany) || x.ContractCompany.Name == searchModel.AssignmentContractHoldingCompany);
                // || x.OperatingCompany.Name == searchModel.AssignmentContractHoldingCompany);

                //Wildcard Search for Operating Company Code
                whereClause = searchModel.AssignmentOperatingCompanyCode.HasEvoWildCardChar() ? whereClause.WhereLikeOr(x => x.OperatingCompany.Code, y => y.ContractCompany.Name, searchModel.AssignmentOperatingCompanyCode, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentOperatingCompanyCode) || x.OperatingCompany.Code == searchModel.AssignmentOperatingCompanyCode);
                //|| x.ContractCompany.Code==searchModel.AssignmentOperatingCompanyCode);

                //Wildcard Search for Operating Company Name
                whereClause = searchModel.AssignmentOperatingCompany.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.OperatingCompany.Name, searchModel.AssignmentOperatingCompany, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentOperatingCompany) || x.OperatingCompany.Name == searchModel.AssignmentOperatingCompany);
                // || x.ContractCompany.Code == searchModel.AssignmentOperatingCompanyCode);
            }

            //Wildcard Search for Contract Holding Cordinator
            whereClause = searchModel.AssignmentContractHoldingCompanyCoordinator.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.ContractCompanyCoordinator.Name, searchModel.AssignmentContractHoldingCompanyCoordinator, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompanyCoordinator) || x.ContractCompanyCoordinator.Name == searchModel.AssignmentContractHoldingCompanyCoordinator);

            //Wildcard Search for Operating Company Coordinator
            whereClause = searchModel.AssignmentOperatingCompanyCoordinator.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.OperatingCompanyCoordinator.Name, searchModel.AssignmentOperatingCompanyCoordinator, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentOperatingCompanyCoordinator) || x.OperatingCompanyCoordinator.Name == searchModel.AssignmentOperatingCompanyCoordinator);

            //Wildcard Search for Customer Code
            whereClause = searchModel.AssignmentCustomerCode.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.Project.Contract.Customer.Code, searchModel.AssignmentCustomerCode, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentCustomerCode) || x.Project.Contract.Customer.Code == searchModel.AssignmentCustomerCode);

            //Wildcard Search for Customer Name
            whereClause = searchModel.AssignmentCustomerName.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.Project.Contract.Customer.Name, searchModel.AssignmentCustomerName, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentCustomerName) || x.Project.Contract.Customer.Name == searchModel.AssignmentCustomerName);

            //Wildcard Search for Assignment Customer Contact Name
            whereClause = searchModel.AssignmentCustomerAssigmentContact.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.CustomerAssignmentContact.ContactName, searchModel.AssignmentCustomerAssigmentContact, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentCustomerAssigmentContact) || x.CustomerAssignmentContact.ContactName == searchModel.AssignmentCustomerAssigmentContact);

            //Wildcard Search for Assignment Contract Number
            whereClause = searchModel.AssignmentContractNumber.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.Project.Contract.ContractNumber, searchModel.AssignmentContractNumber, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentContractNumber) || x.Project.Contract.ContractNumber == searchModel.AssignmentContractNumber);

            //Wildcard Search for Project 
            if (searchModel.AssignmentProjectNumber != 0 && searchModel.AssignmentProjectNumber != null)
                whereClause = whereClause.Where(x => x.Project.ProjectNumber == searchModel.AssignmentProjectNumber);

            // Wildcard Search for Tech Specialist lastName  
            if (searchModel.TechnicalSpecialistName.HasEvoWildCardChar())
            {
                //-- ITK DEF 182 fix 
                string searchName = searchModel.TechnicalSpecialistName.Replace("*", "");

                if (searchModel.TechnicalSpecialistName.StartsWith("*") && searchModel.TechnicalSpecialistName.EndsWith("*"))
                {
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(x1 => x1.TechnicalSpecialist.LastName.ToLower().Contains(searchName))); // Removed First name search - ITK D - 867
                }
                else if (searchModel.TechnicalSpecialistName.EndsWith("*"))
                {
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(x1 => x1.TechnicalSpecialist.LastName.ToLower().StartsWith(searchName))); // Removed First name search - ITK D - 867
                }
                else if (searchModel.TechnicalSpecialistName.StartsWith("*"))
                {
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(x1 => x1.TechnicalSpecialist.LastName.ToLower().EndsWith(searchName))); // Removed First name search - ITK D - 867
                }
            }
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TechnicalSpecialistName) || x.AssignmentTechnicalSpecialist.Any(ts => searchModel.TechnicalSpecialistName == ts.TechnicalSpecialist.LastName)); // Removed First name search - ITK D - 867

            //Wildcard Search for Tech Specialist
            if (searchModel.TechSpecialists != null)
                whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(ts => searchModel.TechSpecialists.Any(t => t.Pin == ts.TechnicalSpecialist.Pin)));

            //Wildcard Search for Supplier Purchase Number
            whereClause = searchModel.AssignmentSupplierPurchaseOrderNumber.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.SupplierPurchaseOrder.SupplierPonumber, searchModel.AssignmentSupplierPurchaseOrderNumber, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentSupplierPurchaseOrderNumber) || x.SupplierPurchaseOrder.SupplierPonumber == searchModel.AssignmentSupplierPurchaseOrderNumber);

            //WildCard Search for AssignmentRef
            whereClause = searchModel.AssignmentReference.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.AssignmentReference, searchModel.AssignmentReference, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentReference) || x.AssignmentReference == searchModel.AssignmentReference);

            //Search for AssignmentNumber
            if (searchModel.AssignmentNumber != null)
                whereClause = whereClause.Where(x => x.AssignmentNumber == searchModel.AssignmentNumber);

            //Search for AssignmentStatus
            if (searchModel.AssignmentStatus != null)
                whereClause = whereClause.Where(x => x.AssignmentStatus == searchModel.AssignmentStatus);

            if (searchModel.WorkFlowTypeIn != null)
            {
                var workFlowTypeIn = searchModel.WorkFlowTypeIn.Split(",");
                whereClause = whereClause.Where(x => workFlowTypeIn.Contains(x.Project.WorkFlowType.Trim()));
            }
            //search for supplierpoID
            if (searchModel.AssignmentSupplierPurchaseOrderId != null)
                whereClause = whereClause.Where(x => x.SupplierPurchaseOrderId == searchModel.AssignmentSupplierPurchaseOrderId);

            return whereClause;
        }

        //This is called from Assignment Edit
        public IList<DomainModel.Assignment> AssignmentSearch(int flag, DomainModel.AssignmentSearch searchModel, params string[] includes)
        {
            var dbAssignments = flag > 0 ? Filter(searchModel, includes)?.AsNoTracking()
                                        : _dbContext.Assignment.Where(x => x.Id == searchModel.AssignmentId)?.AsNoTracking();
            if (includes.Any())
                dbAssignments = includes.Aggregate(dbAssignments, (current, include) => current.Include(include));
            var domAssignment = dbAssignments.ProjectTo<DomainModel.Assignment>().ToList();
            {
                var isVisit = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId > 0)?.Any(); 
                var isTimesheet = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId == null || x.AssignmentSupplierPurchaseOrderId == 0)?.Any();
                var assignmentVisitTimesheet = this.GetAssignmentVisitTimesheetByGroup(dbAssignments, isVisit, isTimesheet);
               
                if (assignmentVisitTimesheet != null)
                    domAssignment = domAssignment.GroupJoin(assignmentVisitTimesheet,
                        ass => (int)ass.AssignmentId,
                        vtAss => vtAss.AssignmentId,
                        (ass, vtAss) => new { ass, vtAss })
                    .Select(x =>
                    {
                        x.ass.AssignmentLastVisitFromDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.LastVisitFromDate : x.ass?.AssignmentFirstVisitDate; //If visit is null, we need to assign AssignmentFirstVisitDate
                        x.ass.AssignmentFirstVisitDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.FirstVisitDate : x.ass?.AssignmentFirstVisitDate;
                        x.ass.AssignmentLastVisitDate = x.vtAss?.FirstOrDefault()?.LastVisitDate;
                        x.ass.AssignmentExpectedCompleteDate = x.vtAss?.FirstOrDefault()?.ExpectedCompleteDate;
                        x.ass.AssignmentPercentageCompleted = x.vtAss?.FirstOrDefault()?.PercentageCompleted;
                        x.ass.IsInternalAssignment =(bool) dbAssignments?.ToList().Select(y => y.IsInternalAssignment).Single();
                        return x.ass;
                    }).ToList();

                return domAssignment;
            }
        }

        //This is called from Edit option of Assignment Search to filter records based on parameters
        private IQueryable<DbModel.Assignment> EditFilter(DomainModel.AssignmentEditSearch searchModel)
        {
            IQueryable<DbModel.Assignment> whereClause = _dbContext.Assignment;

            //Condition for Only View Assignment Rights with No CH and OC Company got selected. - ITK D - 689
            if (searchModel.IsOnlyViewAssignment && searchModel.LoggedInCompanyId > 0)
                //Contract Holding Company Code or Operating Company Code search with Logged in Company Code
                whereClause = whereClause.Where(x => (x.ContractCompanyId == searchModel.LoggedInCompanyId || x.OperatingCompanyId == searchModel.LoggedInCompanyId));
            else
            {
                if (searchModel.AssignmentContractHoldingCompanyId > 0)
                    whereClause = whereClause.Where(x => x.ContractCompanyId == searchModel.AssignmentContractHoldingCompanyId);

                if (searchModel.AssignmentOperatingCompanyId > 0)
                    whereClause = whereClause.Where(x => x.OperatingCompanyId == searchModel.AssignmentOperatingCompanyId);
            }

            if (searchModel.AssignmentIDs?.Count > 0)
            {
                var assignmentId = searchModel.AssignmentIDs.Select(x => x).ToList();
                whereClause = whereClause.Where(x => assignmentId.Contains(x.Id));
            }

            //Wildcard Search for Customer Name
            //if (searchModel.AssignmentCustomerName != null)
            //    whereClause = searchModel.AssignmentCustomerName.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.Project.Contract.Customer.Name, searchModel.AssignmentCustomerName, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentCustomerName) || x.Project.Contract.Customer.Name == searchModel.AssignmentCustomerName);

            //Wildcard Search for Contract Holding Cordinator
            if (searchModel.AssignmentContractHoldingCompanyCoordinator != null)
                whereClause = searchModel.AssignmentContractHoldingCompanyCoordinator.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.ContractCompanyCoordinator.Name, searchModel.AssignmentContractHoldingCompanyCoordinator, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentContractHoldingCompanyCoordinator) || x.ContractCompanyCoordinator.Name == searchModel.AssignmentContractHoldingCompanyCoordinator);

            //Wildcard Search for Operating Company Coordinator
            if (searchModel.AssignmentOperatingCompanyCoordinator != null)
                whereClause = searchModel.AssignmentOperatingCompanyCoordinator.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.OperatingCompanyCoordinator.Name, searchModel.AssignmentOperatingCompanyCoordinator, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentOperatingCompanyCoordinator) || x.OperatingCompanyCoordinator.Name == searchModel.AssignmentOperatingCompanyCoordinator);

            if (searchModel.AssignmentCustomerId > 0)
                whereClause = whereClause.Where(x => x.Project.Contract.CustomerId == searchModel.AssignmentCustomerId);

            if (searchModel.AssignmentContractHoldingCompanyCoordinatorId > 0)
                whereClause = whereClause.Where(x => x.ContractCompanyCoordinatorId == searchModel.AssignmentContractHoldingCompanyCoordinatorId);

            if (searchModel.AssignmentOperatingCompanyCoordinatorId > 0)
                whereClause = whereClause.Where(x => x.OperatingCompanyCoordinatorId == searchModel.AssignmentOperatingCompanyCoordinatorId);

            if (searchModel.AssignmentProjectNumber != 0 && searchModel.AssignmentProjectNumber != null)
                whereClause = whereClause.Where(x => x.ProjectId == searchModel.AssignmentProjectNumber);

            // Wildcard Search for Tech Specialist lastName  
            if (searchModel.TechnicalSpecialistName.HasEvoWildCardChar())
            {
                //-- ITK DEF 182 fix 
                string searchName = searchModel.TechnicalSpecialistName.Replace("*", "");

                if (searchModel.TechnicalSpecialistName.StartsWith("*") && searchModel.TechnicalSpecialistName.EndsWith("*"))
                {
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(x1 => x1.TechnicalSpecialist.LastName.ToLower().Contains(searchName))); // Removed First name search - ITK D - 867
                }
                else if (searchModel.TechnicalSpecialistName.EndsWith("*"))
                {
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(x1 => x1.TechnicalSpecialist.LastName.ToLower().StartsWith(searchName))); // Removed First name search - ITK D - 867
                }
                else if (searchModel.TechnicalSpecialistName.StartsWith("*"))
                {
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(x1 => x1.TechnicalSpecialist.LastName.ToLower().EndsWith(searchName))); // Removed First name search - ITK D - 867
                }
            }
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TechnicalSpecialistName) || x.AssignmentTechnicalSpecialist.Any(ts => searchModel.TechnicalSpecialistName == ts.TechnicalSpecialist.LastName)); // Removed First name search - ITK D - 867

            //Wildcard Search for Tech Specialist
            if (searchModel.TechSpecialists != null)
                whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.Any(ts => searchModel.TechSpecialists.Any(t => t.Pin == ts.TechnicalSpecialist.Pin)));

            //WildCard Search for AssignmentRef
            if (searchModel.AssignmentReference != null)
                whereClause = searchModel.AssignmentReference.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.AssignmentReference, searchModel.AssignmentReference, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentReference) || x.AssignmentReference == searchModel.AssignmentReference);

            //Search for AssignmentNumber
            if (searchModel.AssignmentNumber != null)
                whereClause = whereClause.Where(x => x.AssignmentNumber == searchModel.AssignmentNumber);

            //Search for AssignmentStatus
            if (searchModel.AssignmentStatus != null)
                whereClause = whereClause.Where(x => x.AssignmentStatus == searchModel.AssignmentStatus);

            //Added for Visit/Timesheet assignment in create mode
            if (searchModel.WorkFlowTypeIn != null)
            {
                var workFlowTypeIn = searchModel.WorkFlowTypeIn.Split(",");
                whereClause = whereClause.Where(x => workFlowTypeIn.Contains(x.Project.WorkFlowType.Trim()));
            }
            //search for supplierpoID
            if (searchModel.AssignmentSupplierPurchaseOrderId != null)
                whereClause = whereClause.Where(x => x.SupplierPurchaseOrderId == searchModel.AssignmentSupplierPurchaseOrderId);

            //search for CategoryId - D-1367
            if (searchModel.CategoryId > 0)
                whereClause = whereClause.Where(x => x.AssignmentTaxonomy.Any(x1 => x1.TaxonomyServiceId == searchModel.ServiceId));

            //search for SubCategoryId - D-1367
            if (searchModel.SubCategoryId > 0)
                whereClause = whereClause.Where(x => x.AssignmentTaxonomy.Any(x1 => x1.TaxonomyService.TaxonomySubCategoryId == searchModel.SubCategoryId));

            //search for ServiceId - D-1367
            if (searchModel.ServiceId > 0)
                whereClause = whereClause.Where(x => x.AssignmentTaxonomy.Any(x1 => x1.TaxonomyService.TaxonomySubCategory.TaxonomyCategoryId == searchModel.CategoryId));

            //Wildcard Search for Supplier Purchase Number
            if (searchModel.AssignmentSupplierPurchaseOrderNumber != null)
                whereClause = searchModel.AssignmentSupplierPurchaseOrderNumber.HasEvoWildCardChar() ? whereClause.WhereLike(x => x.SupplierPurchaseOrder.SupplierPonumber, searchModel.AssignmentSupplierPurchaseOrderNumber, '*') : whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignmentSupplierPurchaseOrderNumber) || x.SupplierPurchaseOrder.SupplierPonumber == searchModel.AssignmentSupplierPurchaseOrderNumber);

            //Wildcard Search for Supplier Purchase Order Material Description - D-1367
            if (searchModel.MaterialDescription.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.SupplierPurchaseOrder.MaterialDescription, searchModel.MaterialDescription, '*');
            else if (!string.IsNullOrEmpty(searchModel.MaterialDescription))
                whereClause = whereClause.Where(x => x.SupplierPurchaseOrder.MaterialDescription.Equals(searchModel.MaterialDescription));

            return whereClause;
        }

        //This is called from Assignment Edit Search Screen -(Modified)
        public IList<DomainModel.AssignmentEditSearch> AssignmentSearch(DomainModel.AssignmentEditSearch searchModel)
        {
            var dbAssignments = EditFilter(searchModel)?.AsNoTracking();
            searchModel.TotalCount = dbAssignments.Count();
            if (searchModel.ModuleName != "ASGMNT")
                dbAssignments = dbAssignments.AsNoTracking().OrderBy(x => searchModel.OrderBy).Skip(searchModel.OffSet).Take(searchModel.FetchCount);
            var domAssignment = dbAssignments.ProjectTo<DomainModel.AssignmentEditSearch>().ToList();
            {

                var isVisit = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId > 0)?.Any(); ;
                var isTimesheet = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId == null || x.AssignmentSupplierPurchaseOrderId == 0)?.Any();
                var assignmentVisitTimesheet = this.GetAssignmentVisitTimesheetByGroup(dbAssignments, isVisit, isTimesheet);
                if (assignmentVisitTimesheet != null)
                    domAssignment = domAssignment.GroupJoin(assignmentVisitTimesheet,
                        ass => (int)ass.AssignmentId,
                        vtAss => vtAss.AssignmentId,
                        (ass, vtAss) => new { ass, vtAss })
                    .Select(x =>
                    {
                        x.ass.AssignmentLastVisitFromDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.LastVisitFromDate : x.ass?.AssignmentFirstVisitDate; //If visit is null, we need to assign AssignmentFirstVisitDate
                        x.ass.AssignmentFirstVisitDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.FirstVisitDate : x.ass?.AssignmentFirstVisitDate;
                        x.ass.AssignmentLastVisitDate = x.vtAss?.FirstOrDefault()?.LastVisitDate;
                        x.ass.AssignmentExpectedCompleteDate = x.vtAss?.FirstOrDefault()?.ExpectedCompleteDate;
                        x.ass.AssignmentPercentageCompleted = x.vtAss?.FirstOrDefault()?.PercentageCompleted;
                        x.ass.TotalCount = searchModel.TotalCount;
                        return x.ass;
                    }).ToList();
                if (domAssignment != null && domAssignment.Count > 0)
                    domAssignment.FirstOrDefault().TotalCount = searchModel.TotalCount;
                return domAssignment;
            }
        }

        ////This is called from Assignment Search
        //public IList<DomainModel.AssignmentSearch> AssignmentSearch(DomainModel.AssignmentSearch searchModel, params string[] includes)
        //{
        //    var dbAssignments = Filter(searchModel, includes)?.AsNoTracking();
        //    if (includes.Any())
        //        dbAssignments = includes.Aggregate(dbAssignments, (current, include) => current.Include(include));
        //    var domAssignment = dbAssignments.ProjectTo<DomainModel.AssignmentSearch>().ToList();
        //    {
        //        var isVisit = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId > 0)?.Any(); ;
        //        var isTimesheet = domAssignment?.ToList()?.Where(x => x.AssignmentSupplierPurchaseOrderId == null || x.AssignmentSupplierPurchaseOrderId == 0)?.Any();
        //        var assignmentVisitTimesheet = this.GetAssignmentVisitTimesheetByGroup(dbAssignments, isVisit, isTimesheet);
        //        if (assignmentVisitTimesheet != null)
        //            domAssignment = domAssignment.GroupJoin(assignmentVisitTimesheet,
        //                    ass => (int)ass.AssignmentId,
        //                    vtAss => vtAss.AssignmentId,
        //                    (ass, vtAss) => new { ass, vtAss })
        //                .Select(x =>
        //                {
        //                    x.ass.AssignmentLastVisitFromDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.LastVisitFromDate : x.ass?.AssignmentFirstVisitDate; //If visit is null, we need to assign AssignmentFirstVisitDate
        //                    x.ass.AssignmentFirstVisitDate = (x.vtAss != null && x.vtAss.Count() > 0) ? x.vtAss?.FirstOrDefault()?.FirstVisitDate : x.ass?.AssignmentFirstVisitDate;
        //                    x.ass.AssignmentLastVisitDate = x.vtAss?.FirstOrDefault()?.LastVisitDate;
        //                    x.ass.AssignmentExpectedCompleteDate = x.vtAss?.FirstOrDefault()?.ExpectedCompleteDate;
        //                    x.ass.AssignmentPercentageCompleted = x.vtAss?.FirstOrDefault()?.PercentageCompleted;
        //                    return x.ass;
        //                }).ToList();

        //        return domAssignment;
        //    }
        //}

        public int DeleteAssignment(int assignmentId)
        {
            var count = 0;
            try
            {
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.Assignment_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, assignmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "AssignmentId=" + assignmentId);
            }

            return count;
        }
        
            public List<DbModel.SystemSetting> MailTemplateForInterCompanyAmendment()
        {
            return _dbContext.SystemSetting.Where(x => x.KeyName == EmailKey.EmailInterCompanyDiscountAmendmentReason.ToString()).ToList();
        }
        public List<DbModel.SystemSetting> MailTemplate()
        {
            return _dbContext.SystemSetting.Where(x => x.KeyName == EmailKey.EmailInterCompanyAssignmentToCoordinator.ToString()).ToList();
        }

        public string GetCompanyMessage(string companyCode)
        {
            //CompanyMessageType.EmailInterCompanyAssignmentToCoordinator=11
            return _dbContext.CompanyMessage.FirstOrDefault(x => x.Company.Code == companyCode && x.MessageTypeId == 11)?.Message;
        }

        public IList<DbModel.Data> GetMasterData(IList<string> names, IList<string> description, IList<int> types, IList<string> codes)
        {
            IList<DbModel.Data> result;
            if (names?.Count > 0 && types?.Count > 0)
            {
                result = _dbContext.Data.Where(x => x.MasterDataTypeId == (int)(MasterType.ExpenseType) || (types.Any(x1 => x1 == x.MasterDataTypeId) && (names.Any(x1 => x1 == x.Name) || description.Any(x1 => x1 == (x.Description))) || codes.Any(x1 => x1 == (x.Code)))).AsNoTracking()
                        .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type, Hour = x.Hour }).AsNoTracking().ToList();
            }
            else
                result = _dbContext.Data.Where(x => types.Any(x1 => x1 == x.MasterDataTypeId)).AsNoTracking()
                        .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type, Hour = x.Hour }).AsNoTracking().ToList();

            return result;
        }

        public IList<DbModel.Country> GetCountry(IList<string> names, IList<string> cities)
        {
            IList<DbModel.Country> result = null;
            if (names?.Count > 0 & cities?.Count > 0)
                result = _dbContext.Country?.Join(_dbContext.County,
                                                    country => country.Id,
                                                    county => county.CountryId,
                                                    (country, county) => new { country, county })?
                                            .Join(_dbContext.City,
                                                    county => county.county.Id,
                                                    city => city.CountyId,
                                                    (county, city) => new { county, city })
                                   .Where(x => names.Contains(x.county.country.Name.Trim()))?
                                      .Select(x => new DbModel.Country
                                      {
                                          Id = x.county.country.Id,
                                          Code = x.county.country.Code,
                                          Name = x.county.country.Name,
                                          County = new List<DbModel.County> { new DbModel.County { Id = x.county.county.Id, Name = x.county.county.Name, City = x.county.county.City } }
                                      })?.ToList();
            else
                result = _dbContext.Country?.Join(_dbContext.County,
                                                    country => country.Id,
                                                    county => county.CountryId,
                                                    (country, county) => new { country, county })
                                   .Where(x => names.Contains(x.country.Name.Trim()))?
                                      .Select(x => new DbModel.Country
                                      {
                                          Id = x.country.Id,
                                          Code = x.country.Code,
                                          Name = x.country.Name,
                                          County = new List<DbModel.County> { new DbModel.County { Id = x.county.Id, Name = x.county.Name } }
                                      })?.ToList();
            return result;
        }

        public IList<DbModel.User> GetUser(IList<string> names)
        {
            IList<DbModel.User> result = null;
            if (names?.Count > 0)
                result = _dbContext.User.Where(x => names.Contains(x.SamaccountName)).AsNoTracking()
                                       .Select(x => new DbModel.User { Id = x.Id, Name = x.Name, SamaccountName = x.SamaccountName }).AsNoTracking().ToList();   //Sanity Defect-173
            return result;
        }

        //public IList<DomainModel.ProjectCollection> ProjectCollection(IList<int> projectNumber)
        //{
        //    var results = _dbContext.Project
        //        .Include("Contract.ContractSchedule.ContractRate")
        //        .Include("Contract.Customer.CustomerAddress.CustomerContact")
        //        .Include("SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier.Supplier.SupplierContact")
        //        .Where(x => projectNumber.Contains((int)x.ProjectNumber))
        //        .Select(x => new DomainModel.ProjectCollection
        //        {
        //            Project = new DbModel.Project { Id = x.Id, ProjectNumber = x.ProjectNumber },
        //            Contract = new DbModel.Contract { Id = x.ContractId, ContractNumber = x.Contract.ContractNumber },
        //            Customer = new DbModel.Customer { Id = x.Contract.CustomerId, Code = x.Contract.Customer.Code, Name = x.Contract.Customer.Name },
        //            ContractSchedule = x.Contract.ContractSchedule.Select(x1 => new DbModel.ContractSchedule { Id = x1.Id, ContractId = x1.ContractId, Name = x1.Name, Currency = x1.Currency }).ToList(),
        //            ContractRate = x.Contract.ContractSchedule.SelectMany(x1 => x1.ContractRate).ToList(),
        //            CustomerAddress = x.Contract.Customer.CustomerAddress.Select(x1 => new DbModel.CustomerAddress { Id = x1.Id, CustomerId = x1.CustomerId, Address = x1.Address }).ToList(),
        //            CustomerContact = x.Contract.Customer.CustomerAddress.SelectMany(x1 => x1.CustomerContact).Select(x1 => new DbModel.CustomerContact { Id = x1.Id, CustomerAddressId = x1.CustomerAddressId, ContactName = x1.ContactName }).ToList(),
        //            SupplierPurchaseOrder = x.SupplierPurchaseOrder.Select(x1 => new DbModel.SupplierPurchaseOrder { Id = x1.Id, SupplierPonumber = x1.SupplierPonumber }).ToList(),
        //            SupplierPurchaseOrderSubSupplier = x.SupplierPurchaseOrder.SelectMany(x1 => x1.SupplierPurchaseOrderSubSupplier).ToList(),
        //            Supplier = x.SupplierPurchaseOrder.SelectMany(x1 => x1.SupplierPurchaseOrderSubSupplier).Select(x2 => x2.Supplier).ToList(),
        //            SupplierContact = x.SupplierPurchaseOrder.SelectMany(x1 => x1.SupplierPurchaseOrderSubSupplier).SelectMany(x2 => x2.Supplier.SupplierContact.Select(x3 => new DbModel.SupplierContact { Id = x3.Id, SupplierContactName = x3.SupplierContactName, SupplierId = x3.SupplierId })).ToList()
        //        })?.ToList();

        //    return results;
        // //}
        public void Update(int assignmentId, IList<KeyValuePair<string, object>> updateValueProps, Expression<Func<DbModel.Assignment, object>>[] updatedProperties)
        {
            DbModel.Assignment assignment = _dbContext.Assignment.Where(x => x.Id == assignmentId).FirstOrDefault();
            ObjectExtension.SetPropertyValue(assignment, updateValueProps);
            base.Update(assignment, updatedProperties);
        }


        public void AddAssignmentHistory(int assignmentId, IList<DbModel.Data> dbMasterData, string changedBy)
        {
            try
            {
                //var masterData = _dbContext.Data.Where(x => x.MasterDataTypeId == (int)(MasterType.HistoryTable) && x.Code == historyItemCode)
                //                .Select(x => new DbModel.Data { Id = x.Id, Name = x.Name, Code = x.Code, MasterDataTypeId = x.MasterDataTypeId, Description = x.Description, Type = x.Type }).ToList();

                if (dbMasterData?.Count > 0)
                {
                    DbModel.AssignmentHistory assignmentHistory = new DbModel.AssignmentHistory
                    {
                        AssignmentId = assignmentId,
                        AssignmentHistoryDateTime = DateTime.Now,
                        HistoryItemId = dbMasterData[0].Id,
                        ChangedBy = string.IsNullOrEmpty(changedBy) ? string.Empty : changedBy,
                        LastModification = DateTime.Now
                    };
                    _dbContext.AssignmentHistory.Add(assignmentHistory);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        public IList<DbModel.SqlauditModule> GetAuditModule()
        {
            var moduleNames = new List<string>() {
            SqlAuditModuleType.Assignment.ToString(),
            SqlAuditModuleType.AssignmentAdditionalExpense.ToString(),
            SqlAuditModuleType.AssignmentContractSchedule.ToString(),
            SqlAuditModuleType.AssignmentContributionCalculation.ToString(),
            SqlAuditModuleType.AssignmentContributionRenuveCost.ToString(),
            SqlAuditModuleType.AssignmentDocument.ToString(),
            SqlAuditModuleType.AssignmentInstructions.ToString(),
            SqlAuditModuleType.AssignmentInterCo.ToString(),
            SqlAuditModuleType.AssignmentNote.ToString(),
            SqlAuditModuleType.AssignmentReference.ToString(),
            SqlAuditModuleType.AssignmentSpecialistSubSupplier.ToString(),
            SqlAuditModuleType.AssignmentSubSupplier.ToString(),
            SqlAuditModuleType.AssignmentTechnicalSpecialist.ToString(),
            SqlAuditModuleType.AssignmentTaxonomy.ToString(),
            SqlAuditModuleType.AssignmentTechnicalSchedule.ToString(),
            SqlAuditModuleType.Timesheet.ToString(),
            SqlAuditModuleType.TimesheetReference.ToString(),
            SqlAuditModuleType.TimesheetTechnicalSpecialist.ToString(),
            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable.ToString(),
            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime.ToString(),
            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense.ToString(),
            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel.ToString(),
            SqlAuditModuleType.Visit.ToString(),
            SqlAuditModuleType.VisitReference.ToString(),
            SqlAuditModuleType.VisitSpecialistAccount.ToString(),
            SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable.ToString(),
            SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense.ToString(),
            SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime.ToString(),
            SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel.ToString(),
            SqlAuditModuleType.ResourceSearch.ToString()
            };
            return _dbContext.SqlauditModule.Where(x => moduleNames.Contains(x.ModuleName))?.ToList();
        }

        public bool GetTsVisible()
        {
            return _dbContext.ModuleDocumentType.FirstOrDefault(x => x.Module.Code == "ASGMNT" && x.DocumentType.Name == "Evolution Email")?.Tsvisible ?? false;
        }

        //Added for D-1304 -Start
        public string ValidateAssignmentBudget(DomainModel.Assignment assignments, DbModel.Project dbProjects)
        {
            IList<DbModel.Assignment> dbAssignments = _dbContext.Assignment.Include(a => a.Project).Where(x => x.ProjectId == dbProjects.Id && x.AssignmentNumber != assignments.AssignmentNumber)
                                                .Select(x1 => new DbModel.Assignment { Id = x1.Id, BudgetValue = x1.BudgetValue, BudgetHours = x1.BudgetHours }).ToList();

            decimal? existingAssignmentBudget = dbAssignments?.Sum(x => x.BudgetValue);
            decimal? existingBudgetHours = dbAssignments?.Sum(x => x.BudgetHours);

            decimal? totalBudgetValues = existingAssignmentBudget + assignments.AssignmentBudgetValue;
            decimal? totalBudgetHours = existingBudgetHours + assignments.AssignmentBudgetHours;
            string message = null;
            if (dbProjects.Budget == 0 && dbProjects.BudgetHours == 0)
                message = null;
            else if (dbProjects.Budget != 0 && dbProjects.BudgetHours != 0)
            {
                if (totalBudgetValues > dbProjects.Budget)
                    message = MessageType.AssignmentBudgetValueExceedsProject.ToId();
                else if ((totalBudgetHours > dbProjects.BudgetHours) && (totalBudgetValues < dbProjects.Budget))
                    message = MessageType.AssignmentBudgetHoursExceedsProject.ToId();
            }
            else if (dbProjects.Budget == 0 || dbProjects.BudgetHours == 0)
            {
                if (dbProjects.Budget != 0 && dbProjects.BudgetHours == 0 && (totalBudgetValues > dbProjects.Budget))
                    message = MessageType.AssignmentBudgetValueExceedsProject.ToId();
                else if (dbProjects.Budget == 0 && dbProjects.BudgetHours != 0 && (totalBudgetHours > dbProjects.BudgetHours))
                    message = MessageType.AssignmentBudgetHoursExceedsProject.ToId();
            }
            return message;
        }
        //Added for D-1304 -End

        public void Dispose()
        {
            _dbContext.Dispose();
            _logger = null;
        }

    }

    public class AssignmentVisitTimesheet
    {
        public int AssignmentId { get; set; }
        public DateTime FirstVisitDate { get; set; }
        public DateTime LastVisitFromDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public int? PercentageCompleted { get; set; }
        public DateTime? ExpectedCompleteDate { get; set; }
    }
}