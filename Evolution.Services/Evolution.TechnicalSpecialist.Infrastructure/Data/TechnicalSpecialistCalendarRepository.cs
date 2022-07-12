using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Common.Enums;
using Evolution.Logging.Interfaces;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistCalendarRepository : GenericRepository<DbModel.TechnicalSpecialistCalendar>, ITechnicalSpecialistCalendarRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ITechnicalSpecialistCalendarRepository> _logger = null;

        #region Constructor 

        public TechnicalSpecialistCalendarRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<ITechnicalSpecialistCalendarRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region Get 

        public IList<DbModel.TechnicalSpecialistCalendar> Get(DomainModel.TechnicalSpecialistCalendar technicalSpecialistCalendarModel, params string[] includes)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistCalendar>(technicalSpecialistCalendarModel);
            IQueryable<DbModel.TechnicalSpecialistCalendar> tsAsQueryable = _dbContext.TechnicalSpecialistCalendar;
            if (dbSearchModel != null)
            {
                IList<string> excludeProperties = new List<string>() { "StartDateTime", "EndDateTime" };
                var defWhereExpr = dbSearchModel.ToExpression(excludeProperties);
                if (defWhereExpr != null)
                    tsAsQueryable = tsAsQueryable.Where(defWhereExpr);
                if (technicalSpecialistCalendarModel.StartDateTime != null && technicalSpecialistCalendarModel.EndDateTime != null)
                {
                    tsAsQueryable = tsAsQueryable.Where(x => (Convert.ToDateTime(x.StartDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date && Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                    ((Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && Convert.ToDateTime(x.EndDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                    (((Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)) &&
                    (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)));
                }
                if (technicalSpecialistCalendarModel.ResourceName != null)
                {
                    tsAsQueryable = tsAsQueryable.Where(x => x.TechnicalSpecialist.LogInName == technicalSpecialistCalendarModel.ResourceName);
                }

            }
            if (includes.Any())
                tsAsQueryable = includes.Aggregate(tsAsQueryable, (current, include) => current.Include(include));
            return tsAsQueryable.ToList();
        }

        //public IList<DbModel.TechnicalSpecialistCalendar> CalendarSearchGet(DomainModel.TechnicalSpecialistCalendar technicalSpecialistCalendarModel,
        //                                                                        ref IList<DbModel.Visit> dbVisits,
        //                                                                        ref IList<DbModel.Timesheet> dbTimesheets,
        //                                                                        ref IList<DbModel.ResourceSearch> dbPreAssignments)

        public IList<DbModel.TechnicalSpecialistCalendar> CalendarSearchGet(DomainModel.TechnicalSpecialistCalendar technicalSpecialistCalendarModel, params string[] includes)

        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistCalendar>(technicalSpecialistCalendarModel);
            IQueryable<DbModel.TechnicalSpecialistCalendar> tsAsQueryable = _dbContext.TechnicalSpecialistCalendar;
            var visitCalendarData = new List<DbModel.TechnicalSpecialistCalendar>();
            var timesheetCalendarData = new List<DbModel.TechnicalSpecialistCalendar>();
            var preAssignmentCalendarData = new List<DbModel.TechnicalSpecialistCalendar>();
            var ptoCalendarData = new List<DbModel.TechnicalSpecialistCalendar>();

            var resultCalendarData = new List<DbModel.TechnicalSpecialistCalendar>();
            if (dbSearchModel != null)
            {
                IList<string> excludeProperties = new List<string>() { "StartDateTime", "EndDateTime" };
                var defWhereExpr = dbSearchModel.ToExpression(excludeProperties);

                if (technicalSpecialistCalendarModel.StartDateTime != null && technicalSpecialistCalendarModel.EndDateTime != null && technicalSpecialistCalendarModel.JobReferenceNumber==null)
                {
                    //Filter Visit data
                    var tempVisitCalendarData = _dbContext.TechnicalSpecialistCalendar
                                                .Where(defWhereExpr)
                                                .Where(x => (Convert.ToDateTime(x.StartDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date && Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                                                ((Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && Convert.ToDateTime(x.EndDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                                                (((Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)) &&
                                                (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)))
                                                .Where(calendar => calendar.CalendarType == CalendarType.VISIT.ToString())
                                                .Join(_dbContext.Visit,
                                          dbCalendar => new { VisitId = (long)dbCalendar.CalendarRefCode },
                                          dbVisit => new { VisitId = dbVisit.Id },
                                          (dbCalendar, dbVisit) => new { dbCalendar, dbVisit });
                    //?.Select(x=> new {  x.dbCalendar, x.dbVisit, Assignment = new DbModel.Assignment { AssignmentNumber = x.dbVisit.Assignment.AssignmentNumber, ProjectId = x.dbVisit.Assignment.ProjectId } })
                    //?.ToList();

                    var tempTimesheetCalendarData = _dbContext.TechnicalSpecialistCalendar.Where(defWhereExpr)
                                                    .Where(x => (Convert.ToDateTime(x.StartDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date && Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                                                    ((Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && Convert.ToDateTime(x.EndDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                                                    (((Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)) &&
                                                    (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)))
                                                    .Where(calendar => calendar.CalendarType == CalendarType.TIMESHEET.ToString()).Join(_dbContext.Timesheet,
                                         dbCalendar => new { timesheetId = (long)dbCalendar.CalendarRefCode },
                                         dbTimesheet => new { timesheetId = dbTimesheet.Id },
                                          (dbCalendar, dbTimesheet) => new { dbCalendar, dbTimesheet });
                    //(dbCalendar, dbTimesheet) => new { timesheetCalendarData = dbCalendar, dbTimesheets = new DbModel.Timesheet { TimesheetNumber = dbTimesheet.TimesheetNumber, Assignment = new DbModel.Assignment { AssignmentNumber = dbTimesheet.Assignment.AssignmentNumber, ProjectId = dbTimesheet.Assignment.ProjectId } } })
                    //?.ToList();

                    var tempPreAssignmentCalendarData = _dbContext.TechnicalSpecialistCalendar.Where(defWhereExpr)
                                                       .Where(x => (Convert.ToDateTime(x.StartDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date && Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                                                        ((Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && Convert.ToDateTime(x.EndDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                                                        (((Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)) &&
                                                        (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && (Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date)))
                                                       .Where(calendar => calendar.CalendarType == CalendarType.PRE.ToString()).Join(_dbContext.ResourceSearch,
                                          dbCalendar => new { timesheetId = dbCalendar.CalendarRefCode },
                                          dbResourceSearch => new { timesheetId = (long?)dbResourceSearch.Id },
                                           (dbCalendar, dbResourceSearch) => new { dbCalendar, dbResourceSearch });
                    //(dbCalendar, dbResourceSearch) => new { preAssignmentCalendarData = dbCalendar, dbPreAssignments = dbResourceSearch })
                    //                      ?.ToList();

                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.ResourceName))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(x => x.dbCalendar.TechnicalSpecialist.FirstName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || x.dbCalendar.TechnicalSpecialist.LastName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || string.Format("{0} {1}", x.dbCalendar.TechnicalSpecialist.LastName ?? x.dbCalendar.TechnicalSpecialist.LastName.Trim(), x.dbCalendar.TechnicalSpecialist.FirstName ?? x.dbCalendar.TechnicalSpecialist.FirstName.Trim()).Contains(technicalSpecialistCalendarModel.ResourceName.Trim(), StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = tempTimesheetCalendarData.Where(x => x.dbCalendar.TechnicalSpecialist.FirstName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || x.dbCalendar.TechnicalSpecialist.LastName.Contains(technicalSpecialistCalendarModel.ResourceName) || string.Format("{0} {1}", x.dbCalendar.TechnicalSpecialist.LastName ?? x.dbCalendar.TechnicalSpecialist.LastName.Trim(), x.dbCalendar.TechnicalSpecialist.FirstName ?? x.dbCalendar.TechnicalSpecialist.FirstName.Trim()).Contains(technicalSpecialistCalendarModel.ResourceName.Trim(), StringComparison.OrdinalIgnoreCase));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(x => x.dbCalendar.TechnicalSpecialist.FirstName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || x.dbCalendar.TechnicalSpecialist.LastName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || string.Format("{0} {1}", x.dbCalendar.TechnicalSpecialist.LastName ?? x.dbCalendar.TechnicalSpecialist.LastName.Trim(), x.dbCalendar.TechnicalSpecialist.FirstName ?? x.dbCalendar.TechnicalSpecialist.FirstName.Trim()).Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.CustomerName))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => filtedData.dbVisit.Assignment.Project.Contract.Customer.Name.Contains(technicalSpecialistCalendarModel.CustomerName, StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = tempTimesheetCalendarData.Where(filtedData => filtedData.dbTimesheet.Assignment.Project.Contract.Customer.Name.Contains(technicalSpecialistCalendarModel.CustomerName, StringComparison.OrdinalIgnoreCase));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(filtedData => filtedData.dbResourceSearch.SerilizableObject.Contains(string.Format("\"{0}\":\"{1}", "customerName", technicalSpecialistCalendarModel.CustomerName), StringComparison.OrdinalIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.SupplierName))
                    {
                        //var dd= tempVisitCalendarData.Where(filtedData => filtedData.dbVisits.Supplier.SupplierName.Contains(technicalSpecialistCalendarModel.SupplierName, StringComparison.OrdinalIgnoreCase))?.ToList();

                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => filtedData.dbVisit.Supplier.SupplierName.Contains(technicalSpecialistCalendarModel.SupplierName, StringComparison.OrdinalIgnoreCase));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(filtedData => filtedData.dbResourceSearch.SerilizableObject.Contains(string.Format("\"{0}\":\"{1}", "supplier", technicalSpecialistCalendarModel.SupplierName), StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = null;
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.SupplierLocation))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => (filtedData.dbVisit.Supplier != null) && (filtedData.dbVisit.Supplier.City != null) && (filtedData.dbVisit.Supplier.City.Name.Trim().Contains(technicalSpecialistCalendarModel.SupplierLocation, StringComparison.OrdinalIgnoreCase)));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(filtedData => filtedData.dbResourceSearch.SerilizableObject.Contains(string.Format("\"{0}\":\"{1}", "supplierLocation", technicalSpecialistCalendarModel.SupplierLocation), StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = null;
                    }
                    visitCalendarData = tempVisitCalendarData.Select(x => x.dbCalendar).ToList();
                    timesheetCalendarData = tempTimesheetCalendarData?.Select(x => x.dbCalendar).ToList();
                    preAssignmentCalendarData = tempPreAssignmentCalendarData.Select(x => x.dbCalendar).ToList();

                    //dbVisits = tempVisitCalendarData.Select(x => x.dbVisit).ToList();
                    //dbTimesheets = tempTimesheetCalendarData?.Select(x => x.dbTimesheet).ToList();
                    //dbPreAssignments = tempPreAssignmentCalendarData.Select(x => x.dbResourceSearch).ToList();

                    //Filter PTO data
                    ptoCalendarData = _dbContext.TechnicalSpecialistCalendar.Where(defWhereExpr)
                   .Where(x => (Convert.ToDateTime(x.StartDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date && Convert.ToDateTime(x.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                    ((Convert.ToDateTime(x.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date) && Convert.ToDateTime(x.EndDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date))
                    .Where(calendar => calendar.CalendarType == CalendarType.PTO.ToString()).ToList();

                }
                else
                {
                    var tempVisitCalendarData = _dbContext.TechnicalSpecialistCalendar
                    .Where(defWhereExpr)
                    .Where(calendar => calendar.CalendarType == CalendarType.VISIT.ToString())
                    .Join(_dbContext.Visit,
                          dbCalendar => new { VisitId = (long)dbCalendar.CalendarRefCode },
                          dbVisit => new { VisitId = dbVisit.Id },
                          (dbCalendar, dbVisit) => new { dbCalendar, dbVisit });

                    var tempTimesheetCalendarData = _dbContext.TechnicalSpecialistCalendar.Where(defWhereExpr)
              .Where(calendar => calendar.CalendarType == CalendarType.TIMESHEET.ToString()).Join(_dbContext.Timesheet,
                     dbCalendar => new { timesheetId = (long)dbCalendar.CalendarRefCode },
                     dbTimesheet => new { timesheetId = dbTimesheet.Id },
                     (dbCalendar, dbTimesheet) => new { dbCalendar, dbTimesheet });

                    var tempPreAssignmentCalendarData = _dbContext.TechnicalSpecialistCalendar.Where(defWhereExpr)
                   .Where(calendar => calendar.CalendarType == CalendarType.PRE.ToString()).Join(_dbContext.ResourceSearch,
                          dbCalendar => new { timesheetId = dbCalendar.CalendarRefCode },
                          dbResourceSearch => new { timesheetId = (long?)dbResourceSearch.Id },
                          (dbCalendar, dbResourceSearch) => new { dbCalendar, dbResourceSearch });

                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.ResourceName))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(x => x.dbCalendar.TechnicalSpecialist.FirstName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || x.dbCalendar.TechnicalSpecialist.LastName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || string.Format("{0} {1}", x.dbCalendar.TechnicalSpecialist.LastName ?? x.dbCalendar.TechnicalSpecialist.LastName.Trim(), x.dbCalendar.TechnicalSpecialist.FirstName ?? x.dbCalendar.TechnicalSpecialist.FirstName.Trim()).Contains(technicalSpecialistCalendarModel.ResourceName.Trim(), StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = tempTimesheetCalendarData.Where(x => x.dbCalendar.TechnicalSpecialist.FirstName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || x.dbCalendar.TechnicalSpecialist.LastName.Contains(technicalSpecialistCalendarModel.ResourceName) || string.Format("{0} {1}", x.dbCalendar.TechnicalSpecialist.LastName ?? x.dbCalendar.TechnicalSpecialist.LastName.Trim(), x.dbCalendar.TechnicalSpecialist.FirstName ?? x.dbCalendar.TechnicalSpecialist.FirstName.Trim()).Contains(technicalSpecialistCalendarModel.ResourceName.Trim(), StringComparison.OrdinalIgnoreCase));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(x => x.dbCalendar.TechnicalSpecialist.FirstName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || x.dbCalendar.TechnicalSpecialist.LastName.Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase) || string.Format("{0} {1}", x.dbCalendar.TechnicalSpecialist.LastName ?? x.dbCalendar.TechnicalSpecialist.LastName.Trim(), x.dbCalendar.TechnicalSpecialist.FirstName ?? x.dbCalendar.TechnicalSpecialist.FirstName.Trim()).Contains(technicalSpecialistCalendarModel.ResourceName, StringComparison.OrdinalIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.CustomerName))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => filtedData.dbVisit.Assignment.Project.Contract.Customer.Name.Contains(technicalSpecialistCalendarModel.CustomerName));
                        tempTimesheetCalendarData = tempTimesheetCalendarData.Where(filtedData => filtedData.dbTimesheet.Assignment.Project.Contract.Customer.Name.Contains(technicalSpecialistCalendarModel.CustomerName));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(filtedData => filtedData.dbResourceSearch.SerilizableObject.Contains(string.Format("\"{0}\":\"{1}", "customerName", technicalSpecialistCalendarModel.CustomerName), StringComparison.OrdinalIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.SupplierName))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => filtedData.dbVisit.Supplier.SupplierName.Contains(technicalSpecialistCalendarModel.SupplierName));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(filtedData => filtedData.dbResourceSearch.SerilizableObject.Contains(string.Format("\"{0}\":\"{1}", "supplier", technicalSpecialistCalendarModel.SupplierName), StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = null;
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.SupplierLocation))
                    {
                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => (filtedData.dbVisit.Supplier != null) && (filtedData.dbVisit.Supplier.City != null) && (filtedData.dbVisit.Supplier.City.Name.Trim().Contains(technicalSpecialistCalendarModel.SupplierLocation, StringComparison.OrdinalIgnoreCase)));
                        tempPreAssignmentCalendarData = tempPreAssignmentCalendarData.Where(filtedData => filtedData.dbResourceSearch.SerilizableObject.Contains(string.Format("\"{0}\":\"{1}", "supplierLocation", technicalSpecialistCalendarModel.SupplierLocation), StringComparison.OrdinalIgnoreCase));
                        tempTimesheetCalendarData = null;
                    }
                    if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.JobReferenceNumber))
                    {
                        string JRB = technicalSpecialistCalendarModel.JobReferenceNumber;
                        var jobrefNum = JRB.Split('-');
                        int? projectid = Convert.ToInt32(jobrefNum[0]);
                        int? assignmentNum = Convert.ToInt32(jobrefNum[1]);
                        int? visitNumber = Convert.ToInt32(jobrefNum[2]);
                        tempVisitCalendarData = tempVisitCalendarData.Where(filtedData => filtedData.dbVisit.Assignment.ProjectId == projectid && filtedData.dbVisit.VisitNumber == visitNumber && filtedData.dbVisit.Assignment.AssignmentNumber == assignmentNum);
                        tempTimesheetCalendarData = tempTimesheetCalendarData.Where(filtedData => filtedData.dbTimesheet.Assignment.ProjectId == projectid && filtedData.dbTimesheet.TimesheetNumber == visitNumber && filtedData.dbTimesheet.Assignment.AssignmentNumber == assignmentNum);
                    }
                    visitCalendarData = tempVisitCalendarData.Select(x => x.dbCalendar).ToList();
                    timesheetCalendarData = tempTimesheetCalendarData?.Select(x => x.dbCalendar).ToList();
                    preAssignmentCalendarData = tempPreAssignmentCalendarData.Select(x => x.dbCalendar).ToList();

                    //dbVisits = tempVisitCalendarData.Select(x => x.dbVisit).ToList();
                    //dbTimesheets = tempTimesheetCalendarData?.Select(x => x.dbTimesheet).ToList();
                    //dbPreAssignments = tempPreAssignmentCalendarData.Select(x => x.dbResourceSearch).ToList();

                    //Filter pto data
                    ptoCalendarData = _dbContext.TechnicalSpecialistCalendar.Where(defWhereExpr)
                    .Where(calendar => calendar.CalendarType == CalendarType.PTO.ToString()).ToList();
                }

                resultCalendarData.AddRange(visitCalendarData);
                if (timesheetCalendarData != null && timesheetCalendarData.Count > 0)
                    resultCalendarData.AddRange(timesheetCalendarData);

                    resultCalendarData.AddRange(preAssignmentCalendarData);
                    resultCalendarData.AddRange(ptoCalendarData);
            }
            return resultCalendarData;
        }

        public string GetJobReference(string calendarType, long calendarRefCode)
        {
            string jobReference = string.Empty;
            try
            {
                if (calendarType == "VISIT")
                {
                    var value = _dbContext.Visit
                                   .Join(_dbContext.Assignment,
                                   dbVisit => new { dbVisit.AssignmentId },
                                   dbAssignment => new { AssignmentId = dbAssignment.Id },
                                   (dbVisit, dbAssignment) => new { dbVisit, dbAssignment })
                                   ?.Where(x => x.dbVisit.Id == calendarRefCode)
                                   ?.Select(x => x.dbAssignment != null && x.dbVisit != null ? new { JobReference = x.dbAssignment.ProjectId + "-" + x.dbAssignment.AssignmentNumber + "-" + x.dbVisit.VisitNumber } : null)
                                   ?.FirstOrDefault();
                    jobReference = value?.JobReference;
                }
                else if (calendarType == "TIMESHEET")
                {
                    var value = _dbContext.Timesheet
                                .Join(_dbContext.Assignment,
                                dbTimesheet => new { dbTimesheet.AssignmentId },
                                dbAssignment => new { AssignmentId = dbAssignment.Id },
                                (dbTimesheet, dbAssignment) => new { dbTimesheet, dbAssignment })
                                ?.Where(x => x.dbTimesheet.Id == calendarRefCode)
                                ?.Select(x => x.dbAssignment != null && x.dbTimesheet != null ? new { JobReference = x.dbAssignment.ProjectId + "-" + x.dbAssignment.AssignmentNumber + "-" + x.dbTimesheet.TimesheetNumber } : null)
                                ?.FirstOrDefault();

                    jobReference = value?.JobReference;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), calendarType, calendarRefCode);
            }
            return jobReference;
        }

        #endregion
    }
}
