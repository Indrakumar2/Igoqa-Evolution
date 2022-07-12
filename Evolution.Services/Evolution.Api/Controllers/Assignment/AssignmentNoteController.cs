using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Model = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments/{assignmentId}/notes")]
    [ApiController]

    public class AssignmentNoteController : BaseController
    {
        private readonly IAssignmentNoteService _assignmentNoteService;
        private readonly IAppLogger<AssignmentNoteController> _logger;

        public AssignmentNoteController(IAssignmentNoteService service, IAppLogger<AssignmentNoteController> logger)
        {
            this._assignmentNoteService = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int assignmentId, [FromQuery]Model.AssignmentNote model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model.AssignmentId = assignmentId;
                return this._assignmentNoteService.Get(model);
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
        public Response Post([FromRoute] int assignmentId, [FromBody]IList<Model.AssignmentNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model);
                return this._assignmentNoteService.Add(model);
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

        private void AssignValues(IList<Model.AssignmentNote> model)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            //if (validationType != ValidationType.Add)
            //    ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}