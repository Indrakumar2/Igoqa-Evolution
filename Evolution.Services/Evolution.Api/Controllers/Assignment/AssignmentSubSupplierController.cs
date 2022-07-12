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
    [Route("api/assignments/{assignmentId}/subsuppliers")]
    public class AssignmentSubSupplierController : BaseController
    {
        private readonly IAssignmentSubSupplierService _assignmentSubSupplierService;
        private readonly IAppLogger<AssignmentSubSupplierController> _logger;

        public AssignmentSubSupplierController(IAssignmentSubSupplierService assignmentSubSupplierService, IAppLogger<AssignmentSubSupplierController> logger)
        {
            _assignmentSubSupplierService = assignmentSubSupplierService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int assignmentId,[FromQuery] DomainModel.AssignmentSubSupplier assignmentSubSupplier) 
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                assignmentSubSupplier.AssignmentId = assignmentId;
            return this._assignmentSubSupplierService.Get(assignmentSubSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute] int assignmentId,[FromBody] IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentSubSupplier, ValidationType.Add);
            return this._assignmentSubSupplierService.Add(assignmentSubSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int assignmentId,[FromBody] IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentSubSupplier, ValidationType.Update);
            return this._assignmentSubSupplierService.Modify(assignmentSubSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int assignmentId,[FromBody] IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(assignmentSubSupplier, ValidationType.Delete);
            return this._assignmentSubSupplierService.Delete(assignmentSubSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetSubSupplierForVisit")]
        public Response GetSubSupplierForVisit([FromRoute] int assignmentId,[FromQuery] DomainModel.AssignmentSubSupplierVisit assignmentSubSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                assignmentSubSupplier.AssignmentId = assignmentId;
            return this._assignmentSubSupplierService.GetSubSupplierForVisit(assignmentSubSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.AssignmentSubSupplier> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
