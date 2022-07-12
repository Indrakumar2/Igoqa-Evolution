using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Project.Domain.Interfaces.Projects;
using Microsoft.AspNetCore.Mvc;
using System;
using Model = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Api.Controllers.Project
{
    [Route("api/projects/{projectNumber}/detail")]
    public class ProjectDetailController : BaseController
    {
        private readonly IProjectDetailService _service = null;
        private readonly IAppLogger<ProjectDetailController> _logger = null;

        public ProjectDetailController(IProjectDetailService service,IAppLogger<ProjectDetailController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpPost]
        public Response Post([FromBody]Model.ProjectDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Add);
                //Thread.Sleep(100);
                return _service.SaveProjectDetail( model );
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }

        [HttpPut]
        public Response Put([FromBody]Model.ProjectDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Update);
                return _service.UpdateProjectDetail(model );
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
        public Response Delete([FromBody]Model.ProjectDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Delete);
                return _service.DeleteProjectDetail(model );
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }

        private void AssignValues(Model.ProjectDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.ProjectInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ProjectInvoiceAttachments, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ProjectInvoiceReferences, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ProjectNotes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ProjectNotifications, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ProjectDocuments, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ProjectInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ProjectInvoiceAttachments, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ProjectInvoiceReferences, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ProjectNotifications, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ProjectDocuments, "ModifiedBy", UserName);
            }
        }
    }
}