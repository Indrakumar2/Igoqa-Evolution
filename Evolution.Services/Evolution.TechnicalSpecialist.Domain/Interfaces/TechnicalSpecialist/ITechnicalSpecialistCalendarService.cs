using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistCalendarService
    {
        Response Get(TechnicalSpecialistCalendar searchModel, bool isView = true);

        Response SearchGet(TechnicalSpecialistCalendar searchModel, bool isView);

        Response GetCalendarByTechnicalSpecialistId(TechnicalSpecialistCalendar searchModel, bool isCalendarDataOnly = false);

        Response Save(IList<TechnicalSpecialistCalendar> searchModel, bool commitChange = true, bool isVisitSave = false, IList<DbModel.Company> dbCompanies = null);

        Response Save(IList<DbModel.TechnicalSpecialistCalendar> technicalSpecialistCalendars, bool commitChange = true);

        Response Update(IList<TechnicalSpecialistCalendar> searchModel, bool commitChange = true, bool isVisitSave = false, IList<DbModel.Company> dbCompanies = null);

        Response Delete(IList<TechnicalSpecialistCalendar> searchModel, bool commitChange = true, bool isVisitSave = false);

        bool CheckRecordValidForProcess(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, ValidationType validationType, ref IList<TechnicalSpecialistCalendar> filteredTsInfos, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<ValidationMessage> validationMessages, ref IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo, ref IList<DbModel.Company> dbCompanies);

        string GetJobReference(string calendarType, long calendarRefCode);

        IList<DbModel.TechnicalSpecialistCalendar> GetCalendar(TechnicalSpecialistCalendar technicalSpecialistCalendarModel);

        Response UpdateCalendar(IList<DbModel.TechnicalSpecialistCalendar> dbTechSpecialistCalender, bool commitChange = true);

    }
}
