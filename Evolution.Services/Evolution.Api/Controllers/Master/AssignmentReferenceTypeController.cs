using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/assignments/referencetypes")]
    public class AssignmentReferenceTypeController : Controller
    {
        private readonly IAssignmentReferenceType _service = null;
        private readonly IAppLogger<AssignmentReferenceTypeController> _logger = null;

        public AssignmentReferenceTypeController(IAssignmentReferenceType service, IAppLogger<AssignmentReferenceTypeController> logger)
        {
            this._service = service;
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
                return this._service.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}
