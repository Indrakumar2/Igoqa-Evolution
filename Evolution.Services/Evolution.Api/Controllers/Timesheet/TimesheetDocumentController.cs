using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Timesheet.Domain.Models.Timesheets;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;

namespace Evolution.Api.Controllers.Timesheet
{
    [Produces("application/json")]
    [Route("api/timesheets/documents")]
    public class TimesheetDocumentController : Controller
    {
        private readonly IDocumentService _service = null;
        private ITimesheetDocumentService _timesheetDocumentService = null;
        private readonly IAppLogger<TimesheetDocumentController> _logger = null;

        public TimesheetDocumentController(IDocumentService service,ITimesheetDocumentService timesheetDocumentService, IAppLogger<TimesheetDocumentController> logger)
        {
            _service = service;
            _timesheetDocumentService = timesheetDocumentService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAssignmentTimesheetDocuments")]
        public Response GetListOfTimesheetDocuments([FromQuery] int assignmentId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _timesheetDocumentService.GetAssignmentTimesheetDocuments(assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}