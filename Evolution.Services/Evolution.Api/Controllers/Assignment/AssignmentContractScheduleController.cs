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
    [Route("api/assignments/{assignmentId}/contractschedules")]
    [ApiController]
    public class AssignmentContractScheduleController : BaseController
    {
        private readonly IAssignmentContractRateScheduleService _assignmentContractRateScheduleService = null;
        private readonly IAppLogger<AssignmentContractScheduleController> _logger = null;

        public AssignmentContractScheduleController(IAssignmentContractRateScheduleService assignmentContractRateScheduleService, IAppLogger<AssignmentContractScheduleController> logger)
        {
            _assignmentContractRateScheduleService = assignmentContractRateScheduleService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]int assignmentId, [FromQuery]DomainModel.AssignmentContractRateSchedule searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.AssignmentId = assignmentId;
                return _assignmentContractRateScheduleService.Get(searchModel);
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
        public Response Post([FromRoute]int assignmentId, [FromBody]IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentContractRateSchedules, ValidationType.Add);
                return _assignmentContractRateScheduleService.Add(assignmentContractRateSchedules, assignmentId: assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute]int assignmentId, [FromBody]IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentContractRateSchedules, ValidationType.Update);
                return _assignmentContractRateScheduleService.Modify(assignmentContractRateSchedules, assignmentId: assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute]int assignmentId, [FromBody]IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentContractRateSchedules, ValidationType.Delete);
                return _assignmentContractRateScheduleService.Delete(assignmentContractRateSchedules, assignmentId: assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.AssignmentContractRateSchedule> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}