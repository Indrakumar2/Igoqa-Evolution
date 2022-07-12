using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Domain = Evolution.Assignment.Domain.Models.Assignments;
namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments/{assignmentId}/subsuppliers/techspecialists")]
    [ApiController]
    public class AssignmentSubSupplierTSController : BaseController
    {
        private readonly IAssignmentSubSupplierTSService _assignmentSubSupplierTSService;
        private readonly IAppLogger<AssignmentSubSupplierTSController> _logger;

        public AssignmentSubSupplierTSController(IAssignmentSubSupplierTSService service, IAppLogger<AssignmentSubSupplierTSController> logger)
        {
            this._assignmentSubSupplierTSService = service;
            _logger = logger;
        }

        // GET api/assignment
        [HttpGet]
        public Response Get([FromQuery] Domain.AssignmentSubSupplierTechnicalSpecialist model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._assignmentSubSupplierTSService.Get(model);
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
        public Response Post([FromRoute] int assignmentId, [FromBody]IList<Domain.AssignmentSubSupplierTechnicalSpecialist> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return _assignmentSubSupplierTSService.Add(model);
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
        public Response Put([FromRoute] int assignmentId, [FromBody]IList<Domain.AssignmentSubSupplierTechnicalSpecialist> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return _assignmentSubSupplierTSService.Modify(model);
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
        public Response Delete([FromRoute] int assignmentId, [FromBody]IList<Domain.AssignmentSubSupplierTechnicalSpecialist> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return _assignmentSubSupplierTSService.Delete(model);
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

        private void AssignValues(IList<Domain.AssignmentSubSupplierTechnicalSpecialist> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}