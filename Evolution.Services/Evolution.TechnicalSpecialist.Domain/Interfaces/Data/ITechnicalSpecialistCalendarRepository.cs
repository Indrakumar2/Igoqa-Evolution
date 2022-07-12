using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistCalendarRepository : IGenericRepository<DbModel.TechnicalSpecialistCalendar>
    {
        IList<DbModel.TechnicalSpecialistCalendar> Get(DomainModel.TechnicalSpecialistCalendar technicalSpecialistCalendarModel, params string[] includes);
        //IList<DbModel.TechnicalSpecialistCalendar> CalendarSearchGet(DomainModel.TechnicalSpecialistCalendar technicalSpecialistCalendarModel, 
        //                                                             ref IList<DbModel.Visit> dbVisits,
        //                                                             ref IList<DbModel.Timesheet> dbTimesheets,
        //                                                             ref IList<DbModel.ResourceSearch> dbPreAssignments);

        IList<DbModel.TechnicalSpecialistCalendar> CalendarSearchGet(DomainModel.TechnicalSpecialistCalendar technicalSpecialistCalendarModel, params string[] includes);

        string GetJobReference(string calendarType, long calendarRefCode);
    }
}
