using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments/{assignmentId}/intercompanydiscounts")]
    [ApiController]

    public class AssignmentInterCompanyDiscountController : BaseController
    {
        private readonly IAssignmentInterCompanyDiscountService _assignmentInterCompanyDiscountService;
        private readonly IAppLogger<AssignmentInterCompanyDiscountController> _logger;
        public AssignmentInterCompanyDiscountController(IAssignmentInterCompanyDiscountService service, IAppLogger<AssignmentInterCompanyDiscountController> logger)
        {
            _assignmentInterCompanyDiscountService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int assignmentId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _assignmentInterCompanyDiscountService.Get(assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute] int assignmentId, [FromBody] DomainModel.AssignmentInterCoDiscountInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return this._assignmentInterCompanyDiscountService.Add(model);
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
        public Response Put([FromRoute] int assignmentId, [FromBody] DomainModel.AssignmentInterCoDiscountInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return this._assignmentInterCompanyDiscountService.Modify(model);
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
        public Response Delete([FromRoute] int assignmentId, [FromBody]DomainModel.AssignmentInterCoDiscountInfo model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return this._assignmentInterCompanyDiscountService.Delete(model);
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

        private void AssignValues(DomainModel.AssignmentInterCoDiscountInfo model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}