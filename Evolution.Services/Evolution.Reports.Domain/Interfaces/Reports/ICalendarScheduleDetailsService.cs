using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using ReportModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Domain.Interfaces.Reports
{
    public interface ICalendarScheduleDetailsService
    {
        Response GetCalendarScheduleDetails(ReportModel.CalendarScheduleDetailSearch searchDetails);
    }
}

