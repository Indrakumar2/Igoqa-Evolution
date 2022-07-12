using AutoMapper;
using AutoMapper.QueryableExtensions;
using Constants = Evolution.Common.Constants;
using Evolution.Common.Extensions;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models;

namespace Evolution.Customer.Infrastructure.Data
{
    public class CustomerRepository : GenericRepository<DbModel.Customer>, ICustomerRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CustomerRepository(IMapper mapper, DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.Customers.Customer> Search(DomainModel.Customers.CustomerSearch searchModel)
        {
            try
            {
                if (this._dbContext == null)
                {
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");
                }

                var customers = _dbContext.Customer;
                IQueryable<DbModel.Customer> whereExpression = null;

                //Wildcard Search for Customer Code
                if (searchModel.CustomerCode.HasEvoWildCardChar())
                {
                    whereExpression = customers.WhereLike(x => x.Code, searchModel.CustomerCode.Replace('*', '%'), '%');
                }
                else
                {
                    whereExpression = customers.Where(x => string.IsNullOrEmpty(searchModel.CustomerCode) || x.Code == searchModel.CustomerCode);
                }

                //Wildcard Search for Customer NAme
                if (searchModel.CustomerName.HasEvoWildCardChar())
                {
                    whereExpression = whereExpression.WhereLike(x => x.Name, searchModel.CustomerName.Replace('*', '%'), '%');
                }
                else
                {
                    whereExpression = whereExpression.Where(x => string.IsNullOrEmpty(searchModel.CustomerName) || x.Name == searchModel.CustomerName);
                }

                //Wildcard Search for Customer Parent Company Name
                if (searchModel.ParentCompanyName.HasEvoWildCardChar())
                {
                    whereExpression = whereExpression.WhereLike(x => x.ParentName, searchModel.ParentCompanyName.Replace('*', '%'), '%');
                }
                else
                {
                    whereExpression = whereExpression.Where(x => string.IsNullOrEmpty(searchModel.ParentCompanyName) || x.ParentName == searchModel.ParentCompanyName);
                }

                if (searchModel.CustomerCodes?.Count > 0)
                {
                    whereExpression = whereExpression.Where(x => searchModel.CustomerCodes.Contains(x.Code));
                }

                return whereExpression.Where(x => (searchModel.MIIWAId <= 0 || x.Miiwaid == searchModel.MIIWAId)
                                                    && (searchModel.MIIWAParentId <= 0 || x.MiiwaparentId == searchModel.MIIWAParentId)
                                                    && (string.IsNullOrEmpty(searchModel.OperatingCountry) || x.CustomerAddress.Any(a => a.City.County.Country.Name == searchModel.OperatingCountry))
                                                    && (string.IsNullOrEmpty(searchModel.Active) || x.IsActive == searchModel.Active.ToTrueFalse()))
                                    .Select(x => _mapper.Map<DomainModel.Customers.Customer>(x)).OrderBy(t => t.CustomerName).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IList<DomainModel.Customers.Customer> SearchCustomersBasedOnCoordinators(DomainModel.Customers.CustomerSearch searchModel)
        {
            try
            {
                if (this._dbContext == null)
                {
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");
                }

                List<string> includes = new List<string> { "Assignment.Project.Contract.Customer" };

                IQueryable<DbModel.Visit> whereExpression = _dbContext.Visit;
                whereExpression = whereExpression
                    .Where(x => (searchModel.ContractHolderCompanyId == 0 || (new List<string> { "C", "N", "O", "J", "R" }.Contains(x.VisitStatus) && (x.Assignment.Project.Contract.ContractHolderCompanyId == searchModel.ContractHolderCompanyId || x.Assignment.Project.Contract.InvoicingCompanyId == searchModel.ContractHolderCompanyId)))
                && (searchModel.OperatingCompanyId == 0 || (new List<string> { "C", "N", "J", "R" }.Contains(x.VisitStatus) && (x.Assignment.OperatingCompanyId == searchModel.OperatingCompanyId)))
                && (searchModel.CoordinatorId == 0 || x.Assignment.ContractCompanyCoordinatorId == searchModel.CoordinatorId || x.Assignment.OperatingCompanyCoordinatorId == searchModel.CoordinatorId));

                if (includes?.Count > 0)
                {
                    whereExpression = includes.Aggregate(whereExpression, (current, include) => current.Include(include));
                }

                //var test = whereExpression.GroupBy(x=>x.Assignment.Project.Contract.Customer.Name);
                return whereExpression.Select(x => x.Assignment.Project.Contract.Customer).ProjectTo<DomainModel.Customers.Customer>().ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IList<DomainModel.Customers.CustomerSearch> SearchApprovedVisitCustomers(int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {
            return this.GetApprovedVisitOrTimeSheetCustomers(ContractHolderCompanyId, isVisit, isNDT);
        }

        public IList<DomainModel.Customers.CustomerSearch> SearchUnApprovedVisitCustomers(int ContractHolderCompanyId, int? CoordinatorId, bool isVisit, bool isOperating)
        {
            return this.GetUnApprovedVisitOrTimeSheetCustomers(ContractHolderCompanyId, CoordinatorId, isVisit, isOperating);
        }

        public IList<DomainModel.Customers.CustomerSearch> GetVisitTimesheetKPICustomers(int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            if (isVisit)
            {
                return GetKPIVisitCustomers(ContractHolderCompanyId, isOperating);
            }
            return GetKPITimesheetCustomers(ContractHolderCompanyId, isOperating);
        }

        private IList<DomainModel.Customers.CustomerSearch> GetKPITimesheetCustomers(int ContractHolderCompanyId, bool isOperating)
        {
            IQueryable<DbModel.Timesheet> timesheet = _dbContext.Timesheet.Include(a => a.Assignment).Include(a => a.Assignment.Project).Include(a => a.Assignment.Project.Contract);
            if (isOperating)
                timesheet = timesheet?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
            else
                timesheet = timesheet?.Where(a => (a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId));
            return timesheet?.Select(a => new DomainModel.Customers.CustomerSearch()
            {
                CustomerName = a.Assignment.Project.Contract.Customer.Name,
                CustomerCode = a.Assignment.Project.Contract.Customer.Code,
                CustomerId = a.Assignment.Project.Contract.Customer.Id,
            })?.Distinct()?.OrderBy(a => a.CustomerName).ToList();
        }

        private IList<DomainModel.Customers.CustomerSearch> GetKPIVisitCustomers(int ContractHolderCompanyId, bool isOperating)
        {
            IQueryable<DbModel.Visit> visit = _dbContext.Visit.Include(a => a.Assignment).Include(a => a.Assignment.Project).Include(a => a.Assignment.Project.Contract);
            if (isOperating)
                visit = visit?.Where(a => a.Assignment.OperatingCompanyId == ContractHolderCompanyId);
            else
                visit = visit?.Where(a => (a.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId));
            return visit?.Select(a => new DomainModel.Customers.CustomerSearch()
            {
                CustomerName = a.Assignment.Project.Contract.Customer.Name,
                CustomerCode = a.Assignment.Project.Contract.Customer.Code,
                CustomerId = a.Assignment.Project.Contract.Customer.Id,
            })?.Distinct()?.OrderBy(a => a.CustomerName).ToList();
        }

        public int? GetCityIdForName(string cityName, string countyName)
        {
            try
            {
                if (this._dbContext == null)
                {
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");
                }

                if (countyName != null)
                {
                    return _dbContext.City.FirstOrDefault(x => x.Name == cityName && x.County.Name == countyName)?.Id;
                }
                else
                {
                    return _dbContext.City.FirstOrDefault(x => x.Name == cityName)?.Id;

                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public int GetAssignmentReferenceIdForAssignmentRefferenceType(string AssignmentReferenceType)
        {
            try
            {
                if (this._dbContext == null)
                {
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");
                }

                return Convert.ToInt32(_dbContext.Data.FirstOrDefault(x => x.MasterDataTypeId == 30 && x.Name == AssignmentReferenceType)?.Id);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public int GetCompanyIdForCompanyCode(string companyCode)
        {
            try
            {
                if (this._dbContext == null)
                {
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");
                }

                return Convert.ToInt32(_dbContext.Company.FirstOrDefault(x => x.Code == companyCode)?.Id);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public bool GetIsPortalUser(string logonName) //Changes for IGO - D905
        {
            try
            {
                if (!string.IsNullOrEmpty(logonName))
                {
                    return _dbContext.User.Where(x => x.SamaccountName.Equals(logonName) && x.IsActive == true).Select(x => (bool)x.IsActive).FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }

        private List<DomainModel.Customers.CustomerSearch> GetApprovedVisitOrTimeSheetCustomers(int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {
            List<DomainModel.Customers.CustomerSearch> finalCustomer = new List<DomainModel.Customers.CustomerSearch>();
            if (isVisit)
            {
                var TechnicalSpecialistAccountItem = _dbContext.VisitTechnicalSpecialistAccountItemTime.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Customers.CustomerSearch
                 {
                     CustomerId = a.con.Contract.Customer.Id,
                     CustomerName = a.con.Contract.Customer.Name,
                     CustomerCode = a.con.Contract.Customer.Code,
                     Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemExpense = _dbContext.VisitTechnicalSpecialistAccountItemExpense.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Customers.CustomerSearch
                 {
                     CustomerId = a.con.Contract.Customer.Id,
                     CustomerName = a.con.Contract.Customer.Name,
                     CustomerCode = a.con.Contract.Customer.Code,
                     Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemConsumable = _dbContext.VisitTechnicalSpecialistAccountItemConsumable.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                  new DomainModel.Customers.CustomerSearch
                  {
                      CustomerId = a.con.Contract.Customer.Id,
                      CustomerName = a.con.Contract.Customer.Name,
                      CustomerCode = a.con.Contract.Customer.Code,
                      Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                      MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                  }).Distinct().ToList();

                var TechnicalSpecialistAccountItemTravel = _dbContext.VisitTechnicalSpecialistAccountItemTravel.Join(_dbContext.Visit, VTI => VTI.VisitId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.VisitStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Customers.CustomerSearch
                 {
                     CustomerId = a.con.Contract.Customer.Id,
                     CustomerName = a.con.Contract.Customer.Name,
                     CustomerCode = a.con.Contract.Customer.Code,
                     Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                finalCustomer = TechnicalSpecialistAccountItem.Union(TechnicalSpecialistAccountItemTravel).Union(TechnicalSpecialistAccountItemExpense)
                    .Union(TechnicalSpecialistAccountItemConsumable).Distinct().ToList();

                //Modified the NDT filter Condition based on 1364 Defect and mail conversation with Sarah.
                if (isNDT)
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id == Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
                else
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id != Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
            }
            else
            {
                var TechnicalSpecialistAccountItem = _dbContext.TimesheetTechnicalSpecialistAccountItemTime.Join(_dbContext.Timesheet,
                VTI => VTI.TimesheetId, visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Customers.CustomerSearch
                 {
                     CustomerId = a.con.Contract.Customer.Id,
                     CustomerName = a.con.Contract.Customer.Name,
                     CustomerCode = a.con.Contract.Customer.Code,
                     Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemExpense = _dbContext.TimesheetTechnicalSpecialistAccountItemExpense.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Customers.CustomerSearch
                 {
                     CustomerId = a.con.Contract.Customer.Id,
                     CustomerName = a.con.Contract.Customer.Name,
                     CustomerCode = a.con.Contract.Customer.Code,
                     Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                     MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                 }).Distinct().ToList();

                var TechnicalSpecialistAccountItemConsumable = _dbContext.TimesheetTechnicalSpecialistAccountItemConsumable.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                  new DomainModel.Customers.CustomerSearch
                  {
                      CustomerId = a.con.Contract.Customer.Id,
                      CustomerName = a.con.Contract.Customer.Name,
                      CustomerCode = a.con.Contract.Customer.Code,
                      Evolution1Id = a.con.VTI.Project.ProjectType.Evolution1Id,
                      MasterDataTypeId = a.con.VTI.Project.ProjectType.MasterDataTypeId
                  }).Distinct().ToList();

                var TechnicalSpecialistAccountItemTravel = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel.Join(_dbContext.Timesheet, VTI => VTI.TimesheetId,
                visit => visit.Id, (VTI, visit) => new { VTI, Visit = visit, VTI.Contract }).
                GroupJoin(_dbContext.Contract, con => con.VTI.Contract.Id, cont => cont.ParentContract.Id, (con, cont) => new { con, cont })
                .Where(a => (a.con.Contract.ContractHolderCompanyId == ContractHolderCompanyId || a.con.Contract.InvoicingCompanyId == ContractHolderCompanyId)
                && ((a.con.VTI.ChargeRate * a.con.VTI.ChargeTotalUnit) != 0)
                && a.con.Visit.TimesheetStatus == "A" && a.con.VTI.InvoicingStatus == "N").Select(a =>
                 new DomainModel.Customers.CustomerSearch
                 {
                     CustomerId = a.con.Contract.Customer.Id,
                     CustomerName = a.con.Contract.Customer.Name,
                     CustomerCode = a.con.Contract.Customer.Code,
                     RecordStatus = a.con.VTI.Project.WorkFlowType.Trim()
                 }).Distinct().ToList();

                finalCustomer = TechnicalSpecialistAccountItem.Union(TechnicalSpecialistAccountItemTravel).Union(TechnicalSpecialistAccountItemExpense)
                    .Union(TechnicalSpecialistAccountItemConsumable).Distinct().ToList();

                //Modified the NDT filter Condition based on 1364 Defect and mail conversation with Sarah.
                if (isNDT)
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id == Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
                else
                    finalCustomer = finalCustomer?.Where(a => a.Evolution1Id != Constants.ProjectConstants.EvolutionId1 && a.MasterDataTypeId == Constants.ProjectConstants.MasterDataTypeId)?.ToList();
            }
            finalCustomer = finalCustomer.GroupBy(a => new { a.CustomerId, a.CustomerName, a.CustomerCode })
                   .OrderBy(a => a.Key.CustomerName).Select(a => new DomainModel.Customers.CustomerSearch { CustomerId = a.Key.CustomerId, CustomerName = a.Key.CustomerName, CustomerCode = a.Key.CustomerCode }).ToList();

            return finalCustomer?.GroupBy(a => new { a.CustomerId, a.CustomerName, a.CustomerCode })
                .Select(a => new DomainModel.Customers.CustomerSearch()
                {
                    CustomerId = a.Key.CustomerId,
                    CustomerName = a.Key.CustomerName,
                    CustomerCode = a.Key.CustomerCode

                })?.ToList();
        }

        private List<DomainModel.Customers.CustomerSearch> GetUnApprovedVisitOrTimeSheetCustomers(int ContractHolderCompanyId, int? CoordinatorId, bool isVisit, bool isOperating)
        {
            List<DomainModel.Customers.CustomerSearch> customers = new List<DomainModel.Customers.CustomerSearch>();
            List<string> status = null;
            try
            {
                if (this._dbContext == null)
                    throw new System.InvalidOperationException("Datacontext is not intitialized.");

                if (isVisit)
                {
                    status = !isOperating ? new List<string>() { "C", "N", "O", "J", "R" } : new List<string>() { "C", "N", "J", "R" };
                    var customerList = _dbContext.Visit.Include(a => a.Assignment).Include(a => a.Assignment.Project).
                        Include(a => a.Assignment.Project.Contract).Where(visit => status.Contains(visit.VisitStatus));
                    if (isOperating)
                        customerList = customerList?.Where(visit => visit.VisitStatus!="D" && visit.Assignment.OperatingCompanyId == ContractHolderCompanyId);
                    else
                        customerList = customerList?.Where(visit => visit.VisitStatus!="D" && (visit.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || visit.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId));

                    if (CoordinatorId.HasValue)
                        customerList = customerList?.Where(a => a.Assignment.OperatingCompanyCoordinatorId == CoordinatorId.Value || a.Assignment.ContractCompanyCoordinatorId == CoordinatorId.Value);

                    customers = customerList?.Select(visit => new DomainModel.Customers.CustomerSearch()
                    {
                        CustomerName = visit.Assignment.Project.Contract.Customer.Name,
                        CustomerCode = visit.Assignment.Project.Contract.Customer.Code,
                        CustomerId = visit.Assignment.Project.Contract.Customer.Id,
                    }).ToList();
                }
                else if (!isVisit)
                {
                    status = !isOperating ? new List<string>() { "C", "O", "J", "R" } : new List<string>() { "C", "J", "R" };
                    var customerList = _dbContext.Timesheet.Include(a => a.Assignment).Include(a => a.Assignment.Project).
                         Include(a => a.Assignment.Project.Contract).Where(timesheet => status.Contains(timesheet.TimesheetStatus));
                    if (isOperating)
                        customerList = customerList?.Where(timesheet => timesheet.TimesheetStatus!="E" && timesheet.Assignment.OperatingCompanyId == ContractHolderCompanyId);
                    else
                        customerList = customerList?.Where(timesheet => timesheet.TimesheetStatus!="E" && (timesheet.Assignment.Project.Contract.ContractHolderCompanyId == ContractHolderCompanyId || timesheet.Assignment.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId));

                    if (CoordinatorId.HasValue)
                        customerList = customerList?.Where(a => a.Assignment.OperatingCompanyCoordinatorId == CoordinatorId.Value || a.Assignment.ContractCompanyCoordinatorId == CoordinatorId.Value);

                    customers = customerList?.Select(timesheet => new DomainModel.Customers.CustomerSearch()
                    {
                        CustomerName = timesheet.Assignment.Project.Contract.Customer.Name,
                        CustomerCode = timesheet.Assignment.Project.Contract.Customer.Code,
                        CustomerId = timesheet.Assignment.Project.Contract.Customer.Id
                    }).ToList();
                }
                return customers.Distinct().OrderBy(a => a.CustomerName).ToList();
            }
            catch (System.Exception)
            {
                return customers;
            }
        }
    }
}