using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Interfaces.Projects;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;
using DomModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Api.Controllers.Project
{
    [Route("api/projects/{ProjectNumber}/notes")]
    [ApiController]
    public class ProjectNoteController : BaseController
    {
        private readonly IProjectNotesService _service = null;
        private readonly IAppLogger<ProjectNoteController> _logger = null;

        public ProjectNoteController(IProjectNotesService service,IAppLogger<ProjectNoteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int projectNumber,[FromQuery] DomModel.ProjectNote model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                model.ProjectNumber = projectNumber;
                return this._service.GetProjectNotes(model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute] int projectNumber, [FromBody] IList<DomModel.ProjectNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Add);
                return this._service.SaveProjectNotes(projectNumber, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int projectNumber, [FromBody] IList<DomModel.ProjectNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Delete);
                return this._service.DeleteProjectNotes(projectNumber, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomModel.ProjectNote> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
