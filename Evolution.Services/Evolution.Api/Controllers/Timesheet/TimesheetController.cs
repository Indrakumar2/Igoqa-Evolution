using Evolution.Common.Models.Responses;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Domain = Evolution.Timesheet.Domain.Models.Timesheets;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Models.Filters;
using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Api.Controllers.Base;
using System.Threading.Tasks;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Timesheet
{
    [Produces("application/json")]
    [Route("api/timesheets")]
    public class TimesheetController : BaseController
    {
        private readonly ITimesheetService _timesheetService = null;
        private readonly IAppLogger<TimesheetController> _logger = null;

        public TimesheetController(ITimesheetService service, IAppLogger<TimesheetController> logger)
        {
            _timesheetService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] Domain.BaseTimesheet timesheet, [FromQuery] AdditionalFilter filter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetService.GetTimesheets(timesheet, filter);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetTimesheets")]
        public Response GetVisit([FromQuery]Domain.BaseTimesheet searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetService.GetTimesheetData(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetTimesheetDetail")]
        public Response GetVisit([FromQuery]Domain.Timesheet searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetTimesheetForDocumentApproval")]
        public Response GetTimesheetForDocumentApproval([FromQuery]Domain.BaseTimesheet searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetService.GetTimesheetForDocumentApproval(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("SearchTimesheets")]
        public Response GetTimesheet([FromQuery]Domain.TimesheetSearch searchModel)
        {

            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return Task.Run<Response>(async () => await this._timesheetService.GetTimesheet(searchModel)).Result;
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
            //return this._timesheetService.GetTimesheet(searchModel);
        }

        [HttpPost]
        public Response Post([FromBody]IList<Domain.Timesheet> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null;
                AssignValues(model, ValidationType.Add);
                return _timesheetService.Add(model, ref eventId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]IList<Domain.Timesheet> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null;
                AssignValues(model, ValidationType.Update);
                return _timesheetService.Modify(model, ref eventId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]IList<Domain.Timesheet> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                long? eventId = null;
                AssignValues(model, ValidationType.Delete);
                return _timesheetService.Delete(model, ref eventId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetTimesheetValidationData")]
        public Response GetTimesheetValidationData([FromQuery]Domain.BaseTimesheet searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetService.GetTimesheetValidationData(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("GetExpenseChargeExchangeRates")]
        public Response GetExpenseLineItemChargeExchangeRates([FromBody]ExpenseExchangeRate expenseExchangeRate)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetService.GetExpenseLineItemChargeExchangeRates(expenseExchangeRate.ExchangeRates, expenseExchangeRate.ContractNumber);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), expenseExchangeRate);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<Domain.Timesheet> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }


    }
}