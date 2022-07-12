using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Responses;
using Evolution.Reports.Domain.Interfaces.Reports;
using Evolution.Reports.Domain.Models.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Api.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarScheduleDetailsController : ControllerBase
    {
        private ICalendarScheduleDetailsService _calendarScheduleDetailsService = null;

        public CalendarScheduleDetailsController(ICalendarScheduleDetailsService calendarScheduleDetailsService)
        {
            _calendarScheduleDetailsService = calendarScheduleDetailsService;
        }

        [HttpPost]
        public Response CalendarScheduleDetails([FromBody] CalendarScheduleDetailSearch Params)
        {
            return _calendarScheduleDetailsService.GetCalendarScheduleDetails(Params); 
        }
    }
}
