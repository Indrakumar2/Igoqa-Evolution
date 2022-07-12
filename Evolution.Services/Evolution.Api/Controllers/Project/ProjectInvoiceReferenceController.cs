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
    [Route("api/projects/{projectNumber}/invoice/referencetypes")]
    [ApiController]
    public class ProjectInvoiceReferenceController : BaseController
    {
        private IProjectInvoiceReferenceService _service = null;
        private readonly IAppLogger<ProjectInvoiceReferenceController> _logger = null;

        public ProjectInvoiceReferenceController(IProjectInvoiceReferenceService service, IAppLogger<ProjectInvoiceReferenceController> logger)
        {
            _service = service;
            _logger = logger;

        }

        [HttpGet]
        public Response Get([FromRoute]int projectNumber, [FromQuery] DomModel.ProjectInvoiceReference searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.ProjectNumber = projectNumber;
                return _service.GetProjectInvoiceReferences(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute]int projectNumber, [FromBody] IList<DomModel.ProjectInvoiceReference> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return _service.SaveProjectInvoiceReferences(projectNumber, model);
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
        public Response Put([FromRoute]int projectNumber, [FromBody] IList<DomModel.ProjectInvoiceReference> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return _service.ModifyProjectInvoiceReferences(projectNumber, model);
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
        public Response Delete([FromRoute]int projectNumber, [FromBody] IList<DomModel.ProjectInvoiceReference> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return _service.DeleteProjectInvoiceReferences(projectNumber, model);
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

        private void AssignValues(IList<DomModel.ProjectInvoiceReference> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}