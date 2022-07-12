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
    [Route("api/assignments/{assignmentId}/technicalSpecialists")]
    public class AssignmentTechnicalSpecialistController : BaseController
    {
        private readonly IAssignmentTechnicalSpecilaistService _assignmentTechnicalSpecialistService;
        private readonly IAppLogger<AssignmentTechnicalSpecialistController> _logger;

        public AssignmentTechnicalSpecialistController(IAssignmentTechnicalSpecilaistService assignmentTechnicalSpecialistService, IAppLogger<AssignmentTechnicalSpecialistController> logger)
        {
            _assignmentTechnicalSpecialistService = assignmentTechnicalSpecialistService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]int assignmentId, [FromQuery] DomainModel.AssignmentTechnicalSpecialist searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.AssignmentId = assignmentId;
                return _assignmentTechnicalSpecialistService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute]int assignmentId, [FromBody] IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentTechnicalSpecialists, ValidationType.Add);
                return _assignmentTechnicalSpecialistService.Add(assignmentTechnicalSpecialists);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute]int assignmentId, [FromBody] IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentTechnicalSpecialists, ValidationType.Update);
                return _assignmentTechnicalSpecialistService.Modify(assignmentTechnicalSpecialists);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute]int assignmentId, [FromBody] IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentTechnicalSpecialists, ValidationType.Delete);
                return _assignmentTechnicalSpecialistService.Delete(assignmentTechnicalSpecialists);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.AssignmentTechnicalSpecialist> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
