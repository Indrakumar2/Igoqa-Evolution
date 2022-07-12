using AutoMapper;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Reports.Domain.Models.Reports;
using Evolution.Common.Extensions;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Reports.Infrastructure.Data
{
    public class CalendarScheduleDetailsRepository : ICalendarScheduleDetailsRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public CalendarScheduleDetailsRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public string GetSupplierLocation(DbModel.City City, string CountryName, string State, string PostalCode)
        {
            string location = string.Empty;
            string SCity = string.Empty;
            if (City != null)
            {
                SCity = City.Name;
            }
            if (!string.IsNullOrEmpty(SCity)) //Changes for D1385
                location += SCity + ", ";
            if (!string.IsNullOrEmpty(State))
                location += State + ", ";
            if (!string.IsNullOrEmpty(CountryName))
                location += CountryName + ", ";
            if (!string.IsNullOrEmpty(PostalCode))
                location += PostalCode;
            else
                location = location.Trim().TrimEnd(',');
            return location;
        }
        public IList<CalendarScheduleDetail> GetCalendarScheduleDetails(CalendarScheduleDetailSearch searchDetails)
        {
            List<int> companyID = Array.ConvertAll(searchDetails.CompanyID.Split(','), s => int.Parse(s)).ToList();
            Tuple<DateTime, DateTime> _tuple = GetStartAndEndDate(searchDetails.Month, searchDetails.Year);
            List<CalendarScheduleDetail> calendarScheduleDetails = new List<CalendarScheduleDetail>();
            var technicalSpecialistCalendar = _dbContext.TechnicalSpecialistCalendar;
            var visitData = _dbContext.Visit.Include(include => include.Assignment).Include(include => include.Assignment.Project).Include(include => include.VisitTechnicalSpecialist)
                .Include(include => include.Assignment.Project.Contract).Include(include => include.Assignment.Project.Contract.Customer);

            var visitItems = visitData.Join(_dbContext.VisitTechnicalSpecialist, visit => visit.Id, tech => tech.VisitId, (visit, tech) => new { visit, tech });

            var visitFinalRecords = visitItems.GroupJoin(technicalSpecialistCalendar,
                 visitFinal => new
                 {
                     techid = visitFinal.tech.TechnicalSpecialistId,
                     col2 = "VISIT",
                     col3 = (long?)visitFinal.visit.Id,
                     col4 = true
                 },
                 calendarFinal => new
                 {
                     techid = calendarFinal.TechnicalSpecialistId,
                     col2 = calendarFinal.CalendarType,
                     col3 = calendarFinal.CalendarRefCode,
                     col4 = calendarFinal.IsActive
                 },
                 (visitFinal, calendarFinal) => new { visitFinal, calendarFinal })
                 .Join(_dbContext.Data, visit => visit.visitFinal.visit.VisitStatus, master => master.Code, (visit, master)
                 => new { visit, master })
                 .Where(item => companyID.Contains(item.visit.visitFinal.visit.Assignment.OperatingCompanyId)
                 && companyID.Contains(item.visit.visitFinal.tech.TechnicalSpecialist.CompanyId)
                 && item.visit.visitFinal.visit.FromDate >= _tuple.Item1 && item.visit.visitFinal.visit.FromDate <= _tuple.Item2
                 && item.visit.visitFinal.tech.TechnicalSpecialist.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active
                 ).Select(item =>
                 new CalendarScheduleDetail()
                 {
                     TechSpecID = item.visit.visitFinal.tech.TechnicalSpecialistId,
                     Company = item.visit.visitFinal.visit.Assignment.OperatingCompany.Name,
                     CHCoordinator = item.visit.visitFinal.visit.Assignment.ContractCompanyCoordinator.Name,
                     FirstName = item.visit.visitFinal.tech.TechnicalSpecialist.FirstName,
                     VisitTimesheetStatus = item.master.Name,
                     LastName = item.visit.visitFinal.tech.TechnicalSpecialist.LastName,
                     OCCoordinator = item.visit.visitFinal.visit.Assignment.OperatingCompanyCoordinator.Name,
                     EVONo = string.Format("{0}-{1}", item.visit.visitFinal.visit.Assignment.Project.ProjectNumber, item.visit.visitFinal.visit.Assignment.AssignmentNumber),
                     ProjectNo = item.visit.visitFinal.visit.Assignment.Project.ProjectNumber,
                     AssignmentNo = item.visit.visitFinal.visit.Assignment.AssignmentNumber,
                     AssignmentCreationDate = item.visit.visitFinal.visit.Assignment.CreatedDate,
                     FirstVisitTimesheetStartDate = item.visit.visitFinal.visit.Assignment.FirstVisitTimesheetStartDate,
                     EndDateTime = item.visit.calendarFinal.FirstOrDefault().EndDateTime,
                     StartDateTime = item.visit.calendarFinal.FirstOrDefault().StartDateTime,
                     CustomerName = item.visit.visitFinal.visit.Assignment.Project.Contract.Customer.Name,
                     FromDate = item.visit.visitFinal.visit.FromDate,
                     ToDate = item.visit.visitFinal.visit.ToDate,
                     SupplierID = item.visit.visitFinal.visit.SupplierId,
                     SupplierName = item.visit.visitFinal.visit.Supplier.SupplierName,
                     SupplierLocation = GetSupplierLocation(item.visit.visitFinal.visit.Supplier.City, item.visit.visitFinal.visit.Supplier.Country.Name, item.visit.visitFinal.visit.Supplier.County.Name, item.visit.visitFinal.visit.Supplier.PostalCode),
                     EPIN = item.visit.visitFinal.tech.TechnicalSpecialist.Pin,
                     EmploymentStatus = item.visit.calendarFinal.FirstOrDefault().CalendarStatus,
                     SubDivision = item.visit.visitFinal.tech.TechnicalSpecialist.SubDivision.Name,
                     ActualDate = item.visit.calendarFinal.FirstOrDefault().StartDateTime,
                     Notes = item.visit.calendarFinal.FirstOrDefault().Description,
                     EmploymentType = item.visit.visitFinal.tech.TechnicalSpecialist.EmploymentType != null ?
                     item.visit.visitFinal.tech.TechnicalSpecialist.EmploymentType.Name : ""
                 })?.Distinct()?.ToList();

            var timeSheetData = _dbContext.Timesheet.Include(include => include.Assignment).Include(include => include.Assignment.Project).Include(include => include.TimesheetTechnicalSpecialist)
                .Include(include => include.Assignment.Project.Contract).Include(include => include.Assignment.Project.Contract.Customer);

            var timeSheetItems = timeSheetData.Join(_dbContext.VisitTechnicalSpecialist, timeSheet => timeSheet.Id, tech => tech.VisitId, (timeSheet, tech) => new { timeSheet, tech });

            var timeSheetFinalRecords = timeSheetItems.GroupJoin(technicalSpecialistCalendar,
                 timeSheetFinal => new
                 {
                     techid = timeSheetFinal.tech.TechnicalSpecialistId,
                     col2 = "TIMESHEET",
                     col3 = (long?)timeSheetFinal.timeSheet.Id,
                     col4 = true
                 },
                 calendarFinal => new
                 {
                     techid = calendarFinal.TechnicalSpecialistId,
                     col2 = calendarFinal.CalendarType,
                     col3 = calendarFinal.CalendarRefCode,
                     col4 = calendarFinal.IsActive
                 },
                 (timeSheetFinal, calendarFinal) => new { timeSheetFinal, calendarFinal })
                 .Join(_dbContext.Data, timeSheet => timeSheet.timeSheetFinal.timeSheet.TimesheetStatus, master => master.Code, (timeSheet, master)
                 => new { timeSheet, master })
                 .Where(item => companyID.Contains(item.timeSheet.timeSheetFinal.timeSheet.Assignment.OperatingCompanyId)
                 && companyID.Contains(item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.CompanyId)
                 && item.timeSheet.timeSheetFinal.timeSheet.FromDate >= _tuple.Item1 && item.timeSheet.timeSheetFinal.timeSheet.FromDate <= _tuple.Item2
                 && item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active
                 ).Select(item =>
                 new CalendarScheduleDetail()
                 {
                     TechSpecID = item.timeSheet.timeSheetFinal.tech.TechnicalSpecialistId,
                     Company = item.timeSheet.timeSheetFinal.timeSheet.Assignment.OperatingCompany.Name,
                     CHCoordinator = item.timeSheet.timeSheetFinal.timeSheet.Assignment.ContractCompanyCoordinator.Name,
                     FirstName = item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.FirstName,
                     VisitTimesheetStatus = item.master.Name,
                     LastName = item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.LastName,
                     OCCoordinator = item.timeSheet.timeSheetFinal.timeSheet.Assignment.OperatingCompanyCoordinator.Name,
                     EVONo = string.Format("{0}-{1}", item.timeSheet.timeSheetFinal.timeSheet.Assignment.Project.ProjectNumber, item.timeSheet.timeSheetFinal.timeSheet.Assignment.AssignmentNumber),
                     ProjectNo = item.timeSheet.timeSheetFinal.timeSheet.Assignment.Project.ProjectNumber,
                     AssignmentNo = item.timeSheet.timeSheetFinal.timeSheet.Assignment.AssignmentNumber,
                     AssignmentCreationDate = item.timeSheet.timeSheetFinal.timeSheet.Assignment.CreatedDate,
                     FirstVisitTimesheetStartDate = item.timeSheet.timeSheetFinal.timeSheet.Assignment.FirstVisitTimesheetStartDate,
                     EndDateTime = item.timeSheet.calendarFinal.FirstOrDefault().EndDateTime,
                     StartDateTime = item.timeSheet.calendarFinal.FirstOrDefault().StartDateTime,
                     CustomerName = item.timeSheet.timeSheetFinal.timeSheet.Assignment.Project.Contract.Customer.Name,
                     FromDate = item.timeSheet.timeSheetFinal.timeSheet.FromDate,
                     ToDate = item.timeSheet.timeSheetFinal.timeSheet.ToDate,
                     SupplierID = null,
                     SupplierName = "",
                     SupplierLocation = "",
                     EPIN = item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.Pin,
                     EmploymentStatus = item.timeSheet.calendarFinal.FirstOrDefault().CalendarStatus,
                     SubDivision = item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.SubDivision.Name,
                     ActualDate = item.timeSheet.calendarFinal.FirstOrDefault().StartDateTime,
                     Notes = item.timeSheet.calendarFinal.FirstOrDefault().Description,
                     EmploymentType = item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.EmploymentType != null ?
                     item.timeSheet.timeSheetFinal.tech.TechnicalSpecialist.EmploymentType.Name : ""
                 })?.Distinct().ToList();

            var preAssignmentRecords = (from _resourceSearch in _dbContext.ResourceSearch
                                        join _calendar in _dbContext.TechnicalSpecialistCalendar on new
                                        { column1 = "PRE", column2 = (long?)_resourceSearch.Id, column3 = true }
                                        equals new
                                        { column1 = _calendar.CalendarType, column2 = _calendar.CalendarRefCode, column3 = _calendar.IsActive }
                                        into _finalCalendar
                                        from _finalCalendarData in _finalCalendar.DefaultIfEmpty()
                                        join _tech in _dbContext.TechnicalSpecialist on _finalCalendarData.TechnicalSpecialistId equals _tech.Id
                                        join _company in _dbContext.Company on _tech.CompanyId equals _company.Id
                                        join _subdivision in _dbContext.Data on new { col1 = _tech.SubDivisionId, col2 = 44 } equals new { col1 = _subdivision.Id, col2 = _subdivision.MasterDataTypeId }
                                        join _master in _dbContext.Data on new { column1 = _tech.EmploymentTypeId, column2 = 46 } equals new { column1 = (int?)_master.Id, column2 = _master.MasterDataTypeId }
                                        into _finalMaster
                                        from _finalData in _finalMaster.DefaultIfEmpty()
                                        join _user in _dbContext.User on new { Col1 = JObject.Parse(_resourceSearch.SerilizableObject)["chCoordinatorLogOnName"].ToString(), Col2 = JObject.Parse(_resourceSearch.SerilizableObject)["opCoordinatorLogOnName"].ToString() } equals new { Col1 = _user.SamaccountName, Col2 = _user.SamaccountName }
                                        where companyID.Contains(_tech.CompanyId)
                                         && _finalCalendarData.StartDateTime >= _tuple.Item1 && _finalCalendarData.StartDateTime <= _tuple.Item2 && _tech.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active //&& _tech.EmploymentType.Name != ResourceSearchConstants.Employment_Type_OfficeStaff
                                        select new CalendarScheduleDetail()
                                        {
                                            Company = _company.Name,
                                            CHCoordinator = _user.Name,
                                            FirstName = _tech.FirstName,
                                            VisitTimesheetStatus = JObject.Parse(_resourceSearch.SerilizableObject)["firstVisitStatus"].ToString(),
                                            LastName = _tech.LastName,
                                            OCCoordinator = _user.Name,
                                            ProjectNo = (int?)JObject.Parse(_resourceSearch.SerilizableObject)["projectNumber"],
                                            AssignmentNo = (int?)JObject.Parse(_resourceSearch.SerilizableObject)["assignmentNumber"],
                                            EVONo = (int?)JObject.Parse(_resourceSearch.SerilizableObject)["projectNumber"] != null &&
                                             (int?)JObject.Parse(_resourceSearch.SerilizableObject)["assignmentNumber"] != null ?
                                             string.Format("{0}-{1}", JObject.Parse(_resourceSearch.SerilizableObject)["projectNumber"], (int?)JObject.Parse(_resourceSearch.SerilizableObject)["assignmentNumber"]) : null,
                                            FirstVisitTimesheetStartDate = Convert.ToDateTime(JObject.Parse(_resourceSearch.SerilizableObject)["firstVisitFromDate"]).Date,
                                            AssignmentCreationDate = _resourceSearch.CreatedOn,
                                            EndDateTime = _finalCalendarData.EndDateTime,
                                            StartDateTime = _finalCalendarData.StartDateTime,
                                            CustomerName = JObject.Parse(_resourceSearch.SerilizableObject)["customerName"].ToString(),
                                            FromDate = _finalCalendarData.StartDateTime,
                                            ToDate = _finalCalendarData.EndDateTime,
                                            SupplierID = (int?)JObject.Parse(_resourceSearch.SerilizableObject)["firstVisitSupplierId"],
                                            SupplierName = JObject.Parse(_resourceSearch.SerilizableObject)["supplier"].ToString(),
                                            SupplierLocation = JObject.Parse(_resourceSearch.SerilizableObject)["supplierLocation"].ToString(),
                                            EPIN = _tech.Pin,
                                            EmploymentStatus = "Pre-Assignment",
                                            SubDivision = _subdivision.Name,
                                            ActualDate = _finalCalendarData.StartDateTime,
                                            Notes = _finalCalendarData.Description,
                                            EmploymentType = _tech.EmploymentType != null ? _tech.EmploymentType.Name : null
                                        }
                         ).ToList();

            var ptoRecords = (from _timeOffRequest in _dbContext.TechnicalSpecialistTimeOffRequest
                              join _tech in _dbContext.TechnicalSpecialist on _timeOffRequest.TechnicalSpecialistId equals _tech.Id
                              join _calendar in _dbContext.TechnicalSpecialistCalendar on new
                              { column1 = "PTO", column2 = (long?)_timeOffRequest.Id, column3 = true }
                              equals new
                              { column1 = _calendar.CalendarType, column2 = _calendar.CalendarRefCode, column3 = _calendar.IsActive }
                              into _finalCalendar
                              from _finalCalendarData in _finalCalendar.DefaultIfEmpty()
                              join _company in _dbContext.Company on _tech.CompanyId equals _company.Id
                              join _subdivision in _dbContext.Data on new { col1 = _tech.SubDivisionId, col2 = 44 } equals new { col1 = _subdivision.Id, col2 = _subdivision.MasterDataTypeId }
                              join _master in _dbContext.Data on new { column1 = _tech.EmploymentTypeId, column2 = 46 } equals new { column1 = (int?)_master.Id, column2 = _master.MasterDataTypeId }
                              into _finalMaster
                              from _finalData in _finalMaster.DefaultIfEmpty()
                              where companyID.Contains(_tech.CompanyId)
                               && _finalCalendarData.StartDateTime >= _tuple.Item1 && _finalCalendarData.StartDateTime <= _tuple.Item2 && _tech.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active //&& _tech.EmploymentType.Name != ResourceSearchConstants.Employment_Type_OfficeStaff
                              select new CalendarScheduleDetail()
                              {
                                  Company = _company.Name,
                                  CHCoordinator = null,
                                  FirstName = _tech.FirstName,
                                  VisitTimesheetStatus = null,
                                  LastName = _tech.LastName,
                                  OCCoordinator = null,
                                  EVONo = null,
                                  FirstVisitTimesheetStartDate = null,
                                  EndDateTime = _finalCalendarData.EndDateTime,
                                  StartDateTime = _finalCalendarData.StartDateTime,
                                  CustomerName = null,
                                  FromDate = _finalCalendarData.StartDateTime,
                                  ToDate = _finalCalendarData.EndDateTime,
                                  SupplierID = null,
                                  SupplierName = null,
                                  SupplierLocation = null,
                                  EPIN = _tech.Pin,
                                  EmploymentStatus = _finalCalendarData.CalendarStatus,
                                  SubDivision = _subdivision.Name,
                                  ActualDate = _finalCalendarData.StartDateTime,
                                  Notes = _finalCalendarData.Description,
                                  EmploymentType = _tech.EmploymentType != null ? _tech.EmploymentType.Name : null
                              }
                            ).ToList();
            var data1 = visitFinalRecords.Union(timeSheetFinalRecords).ToList();
            var data2 = data1.Union(preAssignmentRecords).ToList();
            calendarScheduleDetails = data2.Union(ptoRecords).ToList();

            if (!string.IsNullOrEmpty(searchDetails.CustomerName))
                calendarScheduleDetails = calendarScheduleDetails.Where(a => a.CustomerName != null && a.CustomerName.Contains(searchDetails.CustomerName)).ToList();

            if (!string.IsNullOrEmpty(searchDetails.SupplierName))
                calendarScheduleDetails = calendarScheduleDetails.Where(a => a.SupplierName != null && a.SupplierName.Contains(searchDetails.SupplierName)).ToList();

            if (!string.IsNullOrEmpty(searchDetails.ProjectNo))
            {
                int lintProjectNo = Convert.ToInt32(searchDetails.ProjectNo);
                calendarScheduleDetails = calendarScheduleDetails.Where(a => a.ProjectNo != null && a.ProjectNo == lintProjectNo).ToList();
            }

            if (!string.IsNullOrEmpty(searchDetails.AssignmentNo))
            {
                int lintAssignmentNo = Convert.ToInt32(searchDetails.AssignmentNo);
                calendarScheduleDetails = calendarScheduleDetails.Where(a => a.AssignmentNo != null && a.AssignmentNo == lintAssignmentNo).ToList();
            }

            if (searchDetails.OCCoordinator != null && searchDetails.OCCoordinator.Count > 0)
                calendarScheduleDetails = calendarScheduleDetails.Where(a => a.OCCoordinator != null && searchDetails.OCCoordinator.Contains(a.OCCoordinator)).ToList();

            if (searchDetails.CHCoordinator != null && searchDetails.CHCoordinator.Count > 0)
                calendarScheduleDetails = calendarScheduleDetails.Where(a => a.CHCoordinator != null && searchDetails.CHCoordinator.Contains(a.CHCoordinator)).ToList();

            if (searchDetails.ResourceEpins != null && searchDetails.ResourceEpins.Count > 0)
                calendarScheduleDetails = calendarScheduleDetails.Where(x => searchDetails.ResourceEpins.Contains(x.EPIN)).ToList();

            if (searchDetails.EPins != null && searchDetails.EPins.Count > 0)
                calendarScheduleDetails = calendarScheduleDetails.Where(x => searchDetails.EPins.Contains(x.EPIN)).ToList();
            List<CalendarScheduleDetail> filteredData = new List<CalendarScheduleDetail>();
            CalendarScheduleDetail calendarScheduleDetail = new CalendarScheduleDetail();

            calendarScheduleDetails?.ForEach(x =>
            {
                var numberofDays = Convert.ToDateTime(x.EndDateTime).Date - Convert.ToDateTime(x.StartDateTime).Date;
                if (numberofDays.Days > 0)
                {
                    for (int i = 0; i <= numberofDays.Days; i++)
                    {
                        calendarScheduleDetail = ObjectExtension.Clone(x);
                        calendarScheduleDetail.ActualDate = Convert.ToDateTime(x.StartDateTime).Date.AddDays(i);
                        filteredData.Add(calendarScheduleDetail);
                    }
                }
                else
                {
                    filteredData.Add(x);
                }
            });
            return filteredData?.Distinct()?.OrderBy(a => a.Company)?.ThenBy(a => a.LastName).ToList(); //Changes for D1385

        }

        private Tuple<DateTime, DateTime> GetStartAndEndDate(int month, int year)
        {
            DateTime startDate = new DateTime(year, month, 1);
            return new Tuple<DateTime, DateTime>(startDate, startDate.AddMonths(1).AddSeconds(-1)); //Changes for D1385
        }
    }
}