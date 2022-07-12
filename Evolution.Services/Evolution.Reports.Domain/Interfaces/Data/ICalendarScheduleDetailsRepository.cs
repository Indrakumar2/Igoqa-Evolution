using System;
using System.Collections.Generic;
using System.Text;
using ReportModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Domain.Interfaces.Data
{
    public interface ICalendarScheduleDetailsRepository
    {
        IList<ReportModel.CalendarScheduleDetail> GetCalendarScheduleDetails(ReportModel.CalendarScheduleDetailSearch searchDetails);
    }
}

