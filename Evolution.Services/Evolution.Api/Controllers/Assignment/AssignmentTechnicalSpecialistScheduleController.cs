using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments/{assignmentid}/technicalSpecialists/schedules")]
    [ApiController]
    public class AssignmentTechnicalSpecialistScheduleController : BaseController
    {
        private readonly IAssignmentTechnicalSpecialistScheduleService _assignmentTechnicalSpecialistScheduleService;
        private readonly IAppLogger<AssignmentTechnicalSpecialistScheduleController> _logger;

        public AssignmentTechnicalSpecialistScheduleController(IAssignmentTechnicalSpecialistScheduleService assignmentTechnicalSpecialistScheduleService, IAppLogger<AssignmentTechnicalSpecialistScheduleController> logger)
        {
            _assignmentTechnicalSpecialistScheduleService = assignmentTechnicalSpecialistScheduleService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]DomainModel.AssignmentTechnicalSpecialistSchedule searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _assignmentTechnicalSpecialistScheduleService.Get(searchModel);
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
        public Response Post([FromBody]IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentTechnicalSpecialistSchedules, ValidationType.Add);
            return _assignmentTechnicalSpecialistScheduleService.Add(assignmentTechnicalSpecialistSchedules);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentTechnicalSpecialistSchedules, ValidationType.Update);
            return _assignmentTechnicalSpecialistScheduleService.Modify(assignmentTechnicalSpecialistSchedules);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentTechnicalSpecialistSchedules, ValidationType.Delete);
            return _assignmentTechnicalSpecialistScheduleService.Delete(assignmentTechnicalSpecialistSchedules);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialistSchedules);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

        [HttpGet]
        [Route("GetTechSpecRateSchedule")]
        public Response Get([FromRoute]int assignmentId, [FromQuery]int epin)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._assignmentTechnicalSpecialistScheduleService.Get(assignmentId, epin);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}