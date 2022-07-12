using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Api.Controllers.Timesheet
{
    [Route("api/timesheets/{timesheetId}/notes")]
    public class TimesheetNoteController : BaseController
    {
        private readonly ITimesheetNoteService _service = null;
        private readonly IAppLogger<TimesheetNoteController> _logger = null;

        public TimesheetNoteController(ITimesheetNoteService service, IAppLogger<TimesheetNoteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]long timesheetId, [FromQuery]DomainModel.TimesheetNote model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model.TimesheetId = timesheetId;
                return this._service.Get(model);
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

        [HttpPost]
        public Response Post([FromRoute]long timesheetId, [FromBody]IList<DomainModel.TimesheetNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return this._service.Add(model);
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
        private void AssignValues(IList<DomainModel.TimesheetNote> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            //if (validationType != ValidationType.Add)
            //    ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}