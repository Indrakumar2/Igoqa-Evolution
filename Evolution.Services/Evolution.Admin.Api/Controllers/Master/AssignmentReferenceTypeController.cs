using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/assignments/referencetypes")]
    public class AssignmentReferenceTypeController : Controller
    {
        private readonly IAppLogger<AssignmentReferenceTypeController> _logger;
        private readonly IAssignmentReferenceType _service;

        public AssignmentReferenceTypeController(IAssignmentReferenceType service,
            IAppLogger<AssignmentReferenceTypeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/master/assignment/referencetype//
        [HttpGet]
        public Response Get(AssignmentReferenceType search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}