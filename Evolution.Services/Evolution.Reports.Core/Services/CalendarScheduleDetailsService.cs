using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Reports.Domain.Interfaces.Reports;
using Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Core.Services
{
    public class CalendarScheduleDetailsService : ICalendarScheduleDetailsService
    {
        private ICalendarScheduleDetailsRepository _repository = null;
        private IAppLogger<CalendarScheduleDetailsService> _logger = null;
        public CalendarScheduleDetailsService(ICalendarScheduleDetailsRepository repository, IAppLogger<CalendarScheduleDetailsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public Response GetCalendarScheduleDetails(CalendarScheduleDetailSearch searchDetails)
        {
            Exception exception = null;
            IList<CalendarScheduleDetail> result = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                //                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _repository.GetCalendarScheduleDetails(searchDetails);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //int count = result != null ? (result as ).Count : 0;
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}

