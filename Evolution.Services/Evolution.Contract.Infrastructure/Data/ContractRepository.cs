using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.AuditLog.Domain.Enums;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Budget;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;
using Constants = Evolution.Common.Constants;

namespace Evolution.Contract.Infrastructure.Data
{
    public class ContractRepository : GenericRepository<DbModel.Contract>, IContractRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractRepository> _logger = null;

        public ContractRepository(EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<ContractRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public int GetCount(DomainModel.ContractSearch searchModel)
        {
            IQueryable<DbModel.Contract> whereClause = Filter(searchModel);
            var dbSearchModel = _mapper.Map<DbModel.Contract>(searchModel);
            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.Count();
            else
                return whereClause.Where(expression).Count();
        }

        public IList<DomainModel.Contract> Search(DomainModel.ContractSearch searchModel)
        {
            //def 590
            List<string> includes = new List<string> {
                    "ParentContract",
                    "ParentContract.DefaultSalesTax",
                    "ParentContract.DefaultWithholdingTax" ,
                    "ParentContract.DefaultRemittanceText",
                    "ParentContract.DefaultFooterText",
                };
            var dbSearchModel = _mapper.Map<DbModel.Contract>(searchModel);
            IQueryable<DbModel.Contract> whereClause = Filter(searchModel);

            if (searchModel.ContractNumbers?.Count > 0)
                whereClause = whereClause.Where(x => searchModel.ContractNumbers.Contains(x.ContractNumber));

            //Search for Not in ContractType
            if (searchModel.ContractTypeNotIn != null)
            {
                var notInContractTypes = searchModel.ContractTypeNotIn.Split(",");
                whereClause = whereClause.Where(x => !notInContractTypes.Contains(x.ContractType));
            }
            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsUseFixedExchangeRates), nameof(dbSearchModel.IsCrmstatus), nameof(dbSearchModel.IsUseInvoiceDetailsFromParentContract), nameof(dbSearchModel.IsRemittanceText) }));
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Contract>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Contract>().ToList();

        }

        //Added on 19th Oct 2020 for Performance (As per Thiru's email)
        public IList<DomainModel.BaseContract> FetchCompanyContract(DomainModel.BaseContract searchModel)
        {
            IQueryable<DbModel.Contract> result = null;
            if (!string.IsNullOrEmpty(searchModel.ContractStatus))
                result = _dbContext.Contract.Where(x => x.ContractHolderCompany.Code == searchModel.ContractHoldingCompanyCode && x.Status == searchModel.ContractStatus);
            else if (string.IsNullOrEmpty(searchModel.ContractStatus))
                result = _dbContext.Contract.Where(x => x.ContractHolderCompany.Code == searchModel.ContractHoldingCompanyCode);

            return result?.ProjectTo<DomainModel.BaseContract>()?.ToList();
        }

        public IList<DomainModel.BaseContract> SearchBaseContract(DomainModel.ContractSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.Contract>(searchModel);
            IQueryable<DbModel.Contract> whereClause = Filter(searchModel);

            if (searchModel.ContractNumbers?.Count > 0)
                whereClause = whereClause.Where(x => searchModel.ContractNumbers.Contains(x.ContractNumber));

            //Search for Not in ContractType
            if (searchModel.ContractTypeNotIn != null)
            {
                var notInContractTypes = searchModel.ContractTypeNotIn.Split(",");
                whereClause = whereClause.Where(x => !notInContractTypes.Contains(x.ContractType));
            }

            var expression = dbSearchModel.ToExpression((new List<string> {
                nameof(dbSearchModel.IsUseFixedExchangeRates),
                nameof(dbSearchModel.IsCrmstatus),
                nameof(dbSearchModel.IsUseInvoiceDetailsFromParentContract),
                nameof(dbSearchModel.IsRemittanceText)
            }));

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.BaseContract>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.BaseContract>().ToList();
        }

        public IList<BudgetAccountItem> GetBudgetAccountItemDetails(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true)
        {
            var companyId = string.IsNullOrEmpty(companyCode) ? null : _dbContext.Company.FirstOrDefault(x => x.Code == companyCode)?.Id;
            var contractIdsInString = contractIds?.Count > 0 ? string.Join(",", contractIds?.ToArray()) : string.Empty;
            var status = contractStatus == ContractStatus.All ? string.Empty : contractStatus.FirstChar();

            return _mapper.Map<IList<BudgetAccountItem>>(_dbContext.GetAccountItemDetailResult.FromSql("spGetAccountItemDetail  @p0,@p1,@p2,@p3,@p4", companyId, contractIdsInString, status, userName, showMyAssignmentsOnly)?.ToList());
        }

        private IQueryable<DbModel.Contract> Filter(DomainModel.ContractSearch searchModel)
        {
            IQueryable<DbModel.Contract> whereClause = null;
            // var contracts = _dbContext.Contract;

            //Wildcard Search for Contract Holding Company Code
            if (searchModel.ContractHoldingCompanyCode.HasEvoWildCardChar())
                whereClause = _dbContext.Contract.WhereLike(x => x.ContractHolderCompany.Code, searchModel.ContractHoldingCompanyCode, '*');
            else
                whereClause = _dbContext.Contract.Where(x => string.IsNullOrEmpty(searchModel.ContractHoldingCompanyCode) || x.ContractHolderCompany.Code == searchModel.ContractHoldingCompanyCode);

            //Wildcard Search for Contract Holding Company
            if (searchModel.ContractHoldingCompanyName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.ContractHolderCompany.Name, searchModel.ContractHoldingCompanyName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractHoldingCompanyName) || x.ContractHolderCompany.Name == searchModel.ContractHoldingCompanyName);

            //Wildcard Search for Customer Code
            if (searchModel.ContractCustomerCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Customer.Code, searchModel.ContractCustomerCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractCustomerCode) || x.Customer.Code == searchModel.ContractCustomerCode);

            //Wildcard Search for Customer Name
            if (searchModel.ContractCustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Customer.Name, searchModel.ContractCustomerName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractCustomerName) || x.Customer.Name == searchModel.ContractCustomerName);

            //Search for Contract Future days
            if (searchModel.ContractFutureDays != null && searchModel.ContractFutureDays > 0)
                whereClause = whereClause.Where(x => (searchModel.ContractFutureDays == null || x.EndDate <= DateTime.Now.AddDays(searchModel.ContractFutureDays.Value)));

            if (!string.IsNullOrEmpty(searchModel.ParentContractNumber))
                whereClause = whereClause.Where(x => x.ParentContract.ContractNumber == searchModel.ParentContractNumber);


            return whereClause;
        }

        public IList<DomainModel.ContractWithId> GetApprovedContractByCustomer(string customerCode, int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {
            int? customerId = _dbContext.Customer.Where(a => a.Code == customerCode)?.FirstOrDefault()?.Id;
            if (customerId.HasValue)
            {
                return GetApprovedContracts(customerId.Value, ContractHolderCompanyId, isVisit, isNDT);
            }
            return new List<DomainModel.ContractWithId>();
        }

        public IList<DomainModel.ContractWithId> GetUnApprovedContractByCustomer(string customerCode, int? CoordinatorId, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            int? customerId = _dbContext.Customer.Where(a => a.Code == customerCode)?.FirstOrDefault()?.Id;
            if (customerId.HasValue)
            {
                return GetUnApprovedContracts(customerId.Value, CoordinatorId, ContractHolderCompanyId, isVisit, isOperating);
            }
            return new List<DomainModel.ContractWithId>();
        }

        public IList<DomainModel.ContractWithId> GetvisitTimesheetKPIContracts(string customerCode, int ContractHolderCompanyId, bool isOperating, bool isVisit)
        {
            int? customerId = _dbContext.Customer.Where(a => a.Code == customerCode)?.FirstOrDefault()?.Id;
            if (customerId.HasValue)
            {
                if (isVisit)
                {
                    return GetVisitKPIContracts(customerId.Value, ContractHolderCompanyId, isOperating, isVisit);
                }
                return GetTimesheetKPIContracts(customerId.Value, ContractHolderCompanyId, isOperating, isVisit);
            }
            return new List<DomainModel.ContractWithId>();
        }

        private IList<DomainModel.ContractWithId> GetVisitKPIContracts(int customerId, int ContractHolderCompanyId, bool isOperating, bool isVisit)
        {
            IQueryable<DbModel.Visit> visits = _dbContext.Visit.Include(a => a.Assignment).Include(a => a.Assignment.Project).Include(a => a.Assignment.Project.Contract)
                .GroupJoin(_dbContext.Contract, visit => visit.Assignment.Project.Contract.ParentContractId, cont => cont.ParentContract.Id, (visit, cont) => new { visit, cont })
                .Where(a => a.visit.Assignment.Project.Contract.Customer.Id == customerId)
                .Select(a => a.visit);

            if (isOperating)
                visits = visits?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
            else
                visits = visits?.Where(a => (a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId));
            return visits?.Select(a => new DomainModel.ContractWithId()
            {
                Id = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.Id : a.Assignment.Project.Contract.Id,
                CustomerContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.CustomerContractNumber : a.Assignment.Project.Contract.CustomerContractNumber,
                ContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.ContractNumber : a.Assignment.Project.Contract.ContractNumber,
            })?.GroupBy(b => new { b.ContractNumber, b.CustomerContractNumber, b.Id, b.ContractCustomerCode })?.Select(c => new DomainModel.ContractWithId()
            { 
                Id = c.Key.Id,
                CustomerContractNumber = c.Key.CustomerContractNumber,
                ContractNumber = c.Key.ContractNumber
            })?.Distinct().OrderBy(x =>x.ContractNumber)?.ToList();
        }

        private IList<DomainModel.ContractWithId> GetTimesheetKPIContracts(int customerId, int ContractHolderCompanyId, bool isOperating, bool isVisit)
        {
            IQueryable<DbModel.Timesheet> timesheets = _dbContext.Timesheet.Include(a => a.Assignment).Include(a => a.Assignment.Project).Include(a => a.Assignment.Project.Contract);
            timesheets = timesheets.Where(a => a.Assignment.Project.Contract.Customer.Id == customerId);
            if (isOperating)
                timesheets = timesheets?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
            else
                timesheets = timesheets?.Where(a => (a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId));

            return timesheets?.Select(a => new DomainModel.ContractWithId()
            {
                Id = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.Id : a.Assignment.Project.Contract.Id,
                CustomerContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.CustomerContractNumber : a.Assignment.Project.Contract.CustomerContractNumber,
                ContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.ContractNumber : a.Assignment.Project.Contract.ContractNumber,
            })?.Distinct().OrderBy(x => x.ContractNumber)?.ToList();
        }

        private IList<DomainModel.ContractWithId> GetUnApprovedContracts(int customerId, int? CoordinatorId, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            List<DomainModel.ContractWithId> finalContracts = new List<DomainModel.ContractWithId>();
            List<string> status = null;
            try
            {
                if (this._dbContext == null)
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");

                if (isVisit)
                {
                    status = !isOperating ? new List<string>() { "C", "N", "O", "J", "R" } : new List<string>() { "C", "N", "J", "R" };
                    var visit = _dbContext.Visit.Include(a => a.Assignment).Include(a => a.Assignment.Project).Where(a => status.Contains(a.VisitStatus)
                        && a.Assignment.Project.Contract.Customer.Id == customerId);
                    if (isOperating)
                        visit = visit?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
                    else
                        visit = visit?.Where(a => a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId);

                    if (CoordinatorId.HasValue)
                       visit = visit?.Where(a => a.Assignment.OperatingCompanyCoordinatorId == CoordinatorId.Value || a.Assignment.ContractCompanyCoordinatorId == CoordinatorId.Value);
                    
                    finalContracts = visit.Select(a => new DomainModel.ContractWithId()
                    {
                        Id = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.Id : a.Assignment.Project.Contract.Id,
                        CustomerContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.CustomerContractNumber : a.Assignment.Project.Contract.CustomerContractNumber,
                        ContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.ContractNumber : a.Assignment.Project.Contract.ContractNumber,
                    }).Distinct().ToList();
                }
                else if (!isVisit)
                {
                    status = !isOperating ? new List<string>() { "C", "O", "J", "R" } : new List<string>() { "C", "J", "R" };
                    var timesheet = _dbContext.Timesheet.Include(a => a.Assignment).Include(a => a.Assignment.Project).Where(a => status.Contains(a.TimesheetStatus) && a.Assignment.Project.Contract.Customer.Id == customerId);

                    if (isOperating)
                        timesheet = timesheet?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
                    else
                        timesheet = timesheet?.Where(a => a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId);

                    if (CoordinatorId.HasValue)
                       timesheet = timesheet?.Where(a => a.Assignment.OperatingCompanyCoordinatorId == CoordinatorId.Value || a.Assignment.ContractCompanyCoordinatorId == CoordinatorId.Value);
                    

                    finalContracts = timesheet.Select(a => new DomainModel.ContractWithId()
                    {
                        Id = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.Id : a.Assignment.Project.Contract.Id,
                        CustomerContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.CustomerContractNumber : a.Assignment.Project.Contract.CustomerContractNumber,
                        ContractNumber = a.Assignment.Project.Contract.ContractType == "CHD" && a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.Assignment.Project.Contract.ParentContract.ContractNumber : a.Assignment.Project.Contract.ContractNumber,
                    }).Distinct().ToList();
                }
            }
            catch (System.Exception)
            {
                return finalContracts;
            }
            return finalContracts.Distinct().ToList();
        }

        private IList<DomainModel.ContractWithId> GetApprovedContracts(int customerId, int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {
            List<DomainModel.ContractWithId> finalCustomer = new List<DomainModel.ContractWithId>();
            string workFlowType = string.Empty;
            if (isVisit)
            {
                var TechnicalSpecialistAccountItem = _dbContext.VisitTechnicalSpecialistAccountItemTime.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId,
                }).Distinct().ToList();

                var TechnicalSpecialistAccountItemExpense = _dbContext.VisitTechnicalSpecialistAccountItemExpense.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId,
                }).Distinct().ToList();

                var TechnicalSpecialistAccountItemConsumable = _dbContext.VisitTechnicalSpecialistAccountItemConsumable.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId,
                }).Distinct().ToList();

                var TechnicalSpecialistAccountItemTravel = _dbContext.VisitTechnicalSpecialistAccountItemTravel.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                }).Distinct().ToList();
                
                finalCustomer = TechnicalSpecialistAccountItem.Union(TechnicalSpecialistAccountItemTravel).Union(TechnicalSpecialistAccountItemExpense)
                    .Union(TechnicalSpecialistAccountItemConsumable).Distinct().ToList();

                if (isNDT)
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id == Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
                else
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id != Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
            }
            else
            {
                var TechnicalSpecialistAccountItem = _dbContext.TimesheetTechnicalSpecialistAccountItemTime.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                }).Distinct().ToList();

                var TechnicalSpecialistAccountItemExpense = _dbContext.TimesheetTechnicalSpecialistAccountItemExpense.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                }).Distinct().ToList();

                var TechnicalSpecialistAccountItemConsumable = _dbContext.TimesheetTechnicalSpecialistAccountItemConsumable.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                }).Distinct().ToList();

                var TechnicalSpecialistAccountItemTravel = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => a.con.Contract.CustomerId == customerId && (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                new DomainModel.ContractWithId
                {
                    Id = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.Id : a.con.VTI.Project.Contract.Id,
                    CustomerContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.CustomerContractNumber : a.con.VTI.Project.Contract.CustomerContractNumber,
                    ContractNumber = a.con.VTI.Project.Contract.ContractType == "CHD" && a.con.VTI.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId ? a.con.VTI.Project.Contract.ParentContract.ContractNumber : a.con.VTI.Project.Contract.ContractNumber,
                    ContractType = a.con.VTI.Project.WorkFlowType.Trim(),
                    Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                    MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                }).Distinct().ToList();
                
                finalCustomer = TechnicalSpecialistAccountItem.Union(TechnicalSpecialistAccountItemTravel).Union(TechnicalSpecialistAccountItemExpense)
                    .Union(TechnicalSpecialistAccountItemConsumable).Distinct().ToList();

                if (isNDT)
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id == Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
                else
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id != Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
            }

            return finalCustomer?.GroupBy(a => new { a.Id, a.ContractCustomerCode, a.ContractCustomerName, a.CustomerContractNumber, a.ContractNumber })?.Select(b => new DomainModel.ContractWithId()
            {
                Id = b.Key.Id,
                CustomerContractNumber = b.Key.CustomerContractNumber,
                ContractNumber = b.Key.ContractNumber,
            })?.Distinct()?.ToList();
        }

        public int DeleteContract(int contractId, string contractNum)
        {
            int count = 0;
            try
            {
                int contractID = contractId;
                string contractNumber = contractNum; //Changes for IGO D947
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.Contract_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, contractID, contractNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "ContractId=" + contractId);
            }

            return count;
        }

        public int DeleteContract(IList<DbModel.Contract> contracts)
        {
            var contractIds = contracts?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteContract(contractIds);
        }

        public int DeleteContract(List<int> contractIds)
        {
            int count = 0;
            try
            {
                if (contractIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Contract, SQLModuleActionType.Delete), string.Join(",", contractIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "ContractIds=" + contractIds.ToString<int>());
            }

            return count;
        }

        public List<string> GetCustomerContractNumbers(int? customerId)
        {
            List<string> contractIds = _dbContext.Contract.Where(x => x.CustomerId == customerId)?.Select(x1 => x1.ContractNumber).ToList();
            return contractIds;
        }

        public bool CheckUseExchange(DomainModel.Contract contract, DbModel.Contract dbContracts, ValidationType validationType)
        {
            if (contract.ContractExchange?.Select(x => x.RecordStatus.IsRecordStatusNew())?.Count() > 0 && validationType == ValidationType.Add)
                return true;
            if (contract.ContractExchange?.Count > 0 && validationType == ValidationType.Update)
            {
                var contractExchangeRates = _dbContext.ContractExchangeRate.Where(x => x.Contract.ContractNumber == contract.ContractNumber)?.ToList();
                if (contractExchangeRates == null || contractExchangeRates.Count <= 0)
                    return true;
                if (contract.ContractExchange?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList()?.Count == contractExchangeRates?.Count)
                    return false;
                return (bool)dbContracts?.IsUseFixedExchangeRates;
            }
            if (contract.ContractExchange == null && validationType == ValidationType.Update && dbContracts?.IsUseFixedExchangeRates == true)
                return true;
            return false;
        }

        public string ValidateProjectBudget(DomainModel.Contract contract)
        {
            IList<DbModel.Project> dbProjects = _dbContext.Project.Where(x => x.ContractId == contract.Id)?.Select(x1 => new DbModel.Project { Id = x1.Id, Budget = x1.Budget, BudgetHours = x1.BudgetHours }).ToList();
            decimal? existingProjectBudget = dbProjects?.Sum(x => x.Budget);
            decimal? existingProjectBudgetHours = dbProjects?.Sum(x => x.BudgetHours);

            if (contract.ContractBudgetMonetaryValue != 0 && contract.ContractBudgetMonetaryValue < existingProjectBudget)
            {
                string code = MessageType.ContractBudgetValueBelowProject.ToId();
                return code;
            }
            if (contract.ContractBudgetHours != 0 && contract.ContractBudgetHours < existingProjectBudgetHours)
            {
                string code = MessageType.ContractBudgetHoursBelowProject.ToId();
                return code;
            }
            else
            {
                return null;
            }
        }

    }
}
