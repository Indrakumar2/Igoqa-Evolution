using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain = Evolution.Assignment.Domain.Models.Assignments;
namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments")]
    [ApiController]
    public class AssignmentController : BaseController
    {
        private readonly IAssignmentService _assignmentService = null;
        private readonly IAppLogger<AssignmentController> _logger = null;

        public AssignmentController(IAssignmentService service, IAppLogger<AssignmentController> logger)
        {
            _assignmentService = service;
            _logger = logger;
        }

        // GET api/assignment - dashboard
        [HttpGet]
        public Response Get([FromQuery] Domain.AssignmentSearch model, [FromQuery] AdditionalFilter filter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return Task.Run<Response>(async () => await this._assignmentService.GetAssignment(model, filter)).Result;
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

        //getting called for complete data of Edit
        [HttpGet]
        [Route("GetAssignments")]
        public Response Get([FromQuery] Domain.AssignmentSearch model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _assignmentService.GetAssignment(model);
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

        //getting called from Document Approval
        [HttpGet]
        [Route("DocumentApproval")]
        public Response GetDocumentAssignment([FromQuery] Domain.AssignmentDashboard model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _assignmentService.GetDocumentAssignment(model);
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
        [Route("budgets")]
        public Response Get([FromQuery] string companyCode = null, [FromQuery] string userName = null, [FromQuery]  ContractStatus contractStatus = ContractStatus.All, [FromQuery] bool showMyAssignmentsOnly = true)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _assignmentService.GetAssignmentBudgetDetails(companyCode, userName, contractStatus, showMyAssignmentsOnly);
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

        //getting called for Search Edit option
        [HttpGet]
        [Route("Search")]
        public Response GetAssignment([FromQuery]Domain.AssignmentEditSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return Task.Run<Response>(async () => await this._assignmentService.SearchAssignment(searchModel))?.Result;
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
        public Response Post([FromBody]IList<Domain.Assignment> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return _assignmentService.Add(model);
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

        [HttpPut]
        public Response Put([FromBody]IList<Domain.Assignment> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return _assignmentService.Modify(model);
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

        [HttpDelete]
        public Response Delete([FromBody]IList<Domain.Assignment> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return _assignmentService.Delete(model);
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

        private void AssignValues(IList<Domain.Assignment> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}