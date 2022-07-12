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
    [Route("api/timesheets/{timesheetId}/technicalSpecialistAccountItemTime")]
    public class TimesheetTechnicalSpecialistTimeController : BaseController
    {
        private readonly ITechSpecAccountItemTimeService _service = null;
        private readonly IAppLogger<TimesheetTechnicalSpecialistTimeController> _logger = null;
        public TimesheetTechnicalSpecialistTimeController(ITechSpecAccountItemTimeService service, IAppLogger<TimesheetTechnicalSpecialistTimeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]long timesheetId, [FromQuery]DomainModel.TimesheetSpecialistAccountItemTime searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.TimesheetId = timesheetId;
                return this._service.Get(searchModel);
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
        public Response Post([FromRoute]long timesheetId, [FromBody]IList<DomainModel.TimesheetSpecialistAccountItemTime> searchModel)

        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Add);
                return this._service.Add(searchModel);
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

        [HttpPut]
        public Response Put([FromRoute]long timesheetId, [FromBody]IList<DomainModel.TimesheetSpecialistAccountItemTime> searchModel)

        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Update);
                return this._service.Modify(searchModel);
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

        [HttpDelete]
        public Response Delete([FromRoute]long timesheetId, [FromBody]IList<DomainModel.TimesheetSpecialistAccountItemTime> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Delete);
                return this._service.Delete(searchModel);
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
        private void AssignValues(IList<DomainModel.TimesheetSpecialistAccountItemTime> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}