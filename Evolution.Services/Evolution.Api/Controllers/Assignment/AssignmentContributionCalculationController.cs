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
    [Route("api/assignments/{assignmentId}/contributionCalculations")]
    [ApiController]
    public class AssignmentContributionCalculationController : BaseController
    {
        private readonly IAssignmentContributionCalculationService _service = null;
        private readonly IAppLogger<AssignmentContributionCalculationController> _logger = null;

        public AssignmentContributionCalculationController(IAssignmentContributionCalculationService service, IAppLogger<AssignmentContributionCalculationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int assignmentId,
                            [FromQuery]DomainModel.AssignmentContributionCalculation searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.AssignmentId = assignmentId;
                return _service.Get(searchModel);
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
        public Response Post([FromRoute] int assignmentd,
                             [FromBody] IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentContributionCalculations, ValidationType.Add);
                return _service.Add(assignmentContributionCalculations, true, true, assignmentd);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculations);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int assignmentId,
                            [FromBody] IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentContributionCalculations, ValidationType.Update);
                return _service.Modify(assignmentContributionCalculations, true, true, assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculations);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int assignmentId,
                               [FromBody] IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentContributionCalculations, ValidationType.Delete);
                return _service.Delete(assignmentContributionCalculations, true, true, assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculations);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.AssignmentContributionCalculation> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
