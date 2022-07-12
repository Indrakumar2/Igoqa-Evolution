using System;
using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainTsModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetDetail
    {
        public Timesheet TimesheetInfo { get; set; }
        public IList<TimesheetTechnicalSpecialist> TimesheetTechnicalSpecialists { get; set; }
        public IList<TimesheetSpecialistAccountItemTime> TimesheetTechnicalSpecialistTimes { get; set; }
        public IList<TimesheetSpecialistAccountItemTravel> TimesheetTechnicalSpecialistTravels { get; set; }
        public IList<TimesheetSpecialistAccountItemExpense> TimesheetTechnicalSpecialistExpenses { get; set; }
        public IList<TimesheetSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistConsumables { get; set; }
        public IList<TimesheetReferenceType> TimesheetReferences { get; set; }
        public IList<TimesheetNote> TimesheetNotes { get; set; }
        public IList<ModuleDocument> TimesheetDocuments { get; set; }
        public IList<DomainTsModel.TechnicalSpecialistCalendar> TechnicalSpecialistCalendarList { get; set; }
    }

    public class DbTimesheet : IDisposable
    {
        bool disposed = false;
        public IList<DbModel.Timesheet> DbTimesheets = null;
        public IList<DbModel.Assignment> DbAssignments =null;
        public IList<DbModel.Project> DbProjects =null;
        public IList<DbModel.Contract> DbContracts =null;
        public IList<DbModel.TimesheetTechnicalSpecialist> DbTimesheetTechSpecialists =null;
        public IList<DbModel.TimesheetReference> DbTimesheetReference =null;
        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> DbTimesheetTechSpecConsumables =null;
        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> DbTimesheetTechSpecExpenses =null;
        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> DbTimesheetTechSpecTimes =null;
        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> DbTimesheetTechSpecTravels =null;
        public IList<DbModel.TimesheetNote> DbTimesheetNotes =null;
        public IList<DbModel.TimesheetDocument> DbTimesheetDocuments =null;
        public IList<DbModel.Data> DbChargeExpenses =null;
        public IList<DbModel.Data> DbPayExpenses =null;
        public IList<DbModel.Data> DbData = null;
        public IList<DbModel.TechnicalSpecialist> DbTechnicalSpecialists = null;
        public IList<DomainTsModel.TechnicalSpecialistCalendar> filteredAddTSCalendarInfo = null;
        public IList<DomainTsModel.TechnicalSpecialistCalendar> filteredModifyTSCalendarInfo = null;
        public IList<DomainTsModel.TechnicalSpecialistCalendar> filteredDeleteTSCalendarInfo = null;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                DbTimesheets?.Clear();
                DbTimesheets = null;

                DbAssignments?.Clear();
                DbAssignments = null;

                DbProjects?.Clear();
                DbProjects = null;

                DbContracts?.Clear();
                DbContracts = null;

                DbTimesheetTechSpecialists?.Clear();
                DbTimesheetTechSpecialists = null;

                DbTimesheetReference?.Clear();
                DbTimesheetReference = null;

                DbTimesheetTechSpecConsumables?.Clear();
                DbTimesheetTechSpecConsumables = null;

                DbTimesheetTechSpecExpenses?.Clear();
                DbTimesheetTechSpecExpenses = null;

                DbTimesheetTechSpecTimes?.Clear();
                DbTimesheetTechSpecTimes = null;

                DbTimesheetTechSpecTravels?.Clear();
                DbTimesheetTechSpecTravels = null;

                DbTimesheetNotes?.Clear();
                DbTimesheetNotes = null;

                DbTimesheetDocuments?.Clear();
                DbTimesheetDocuments = null;

                DbChargeExpenses?.Clear();
                DbChargeExpenses = null;

                DbPayExpenses?.Clear();
                DbPayExpenses = null;

                DbData?.Clear();
                DbData = null;

                DbTechnicalSpecialists?.Clear();
                DbTechnicalSpecialists = null;

                filteredAddTSCalendarInfo?.Clear();
                filteredAddTSCalendarInfo = null;

                filteredModifyTSCalendarInfo?.Clear();
                filteredModifyTSCalendarInfo = null;

                filteredDeleteTSCalendarInfo?.Clear();
                filteredDeleteTSCalendarInfo = null;
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
