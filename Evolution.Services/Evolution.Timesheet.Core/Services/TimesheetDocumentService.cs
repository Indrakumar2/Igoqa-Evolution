using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Timesheet.Core.Services
{
    public class TimesheetDocumentService : ITimesheetDocumentService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetDocumentService> _logger = null;
        private readonly ITimesheetRepository _repository = null;
        private readonly JObject _messages = null;
        private IDocumentService _service = null;

        public TimesheetDocumentService(IMapper mapper, IAppLogger<TimesheetDocumentService> logger, ITimesheetRepository repository, IDocumentService service, JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._service = service;
            this._messages = messages;

        }

       public Response GetAssignmentTimesheetDocuments(int? assignmentId)
        {
            Exception exception = null;
            List<DbModel.Timesheet> result = null;
            object response = null;
            List<string> strTimesheetIDs = new List<string>();
            try
            {
                result = this._repository.GetAssignmentTimesheetIds(assignmentId);
                if (result?.Count > 0)
                {
                    strTimesheetIDs = result?.ConvertAll<string>(x => x.Id.ToString());
                }
                if (strTimesheetIDs?.Count > 0)
                {
                    response = _service.Get(ModuleCodeType.TIME, strTimesheetIDs).Result.Populate<List<ModuleDocument>>();
                    List<ModuleDocument> timesheetDocuments = _service.Get(ModuleCodeType.TIME, strTimesheetIDs).Result.Populate<List<ModuleDocument>>();
                    response = timesheetDocuments?.Join(result,
                        td => new { Id = td.ModuleRefCode },
                        t => new { Id = t.Id.ToString() },
                        (td, t) => new { td, t })
                        .Select(x => new
                        {
                            x.td.Id,
                            x.td.DocumentName,
                            x.td.DocumentType,
                            x.td.DocumentSize,
                            x.td.IsVisibleToTS,
                            x.td.IsVisibleToCustomer,
                            x.td.IsVisibleOutOfCompany,
                            x.td.Status,
                            x.td.DocumentUniqueName,
                            x.td.ModuleCode,
                            x.td.ModuleRefCode,
                            x.td.SubModuleRefCode,
                            x.td.CreatedOn,
                            x.td.CreatedBy,
                            x.td.DisplayOrder,
                            x.td.Comments,
                            x.td.ExpiryDate,
                            x.td.IsForApproval,
                            x.td.ApprovalDate,
                            x.td.ApprovedBy,
                            x.td.CoordinatorName,
                            x.td.DocumentTitle,
                            x.td.FilePath,
                            x.t.TimesheetDescription
                        });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, response, exception);
        }
    }
}
