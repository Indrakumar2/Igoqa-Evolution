using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evolution.Logging.Interfaces;
using System;
using Domain = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Api.Controllers.Project
{
    [Produces("application/json")]
    [Route("api/projects")]
    public class ProjectController : BaseController
    {
        private IProjectService _projectService = null;
        private readonly IAppLogger<ProjectController> _logger = null;

        public ProjectController(IProjectService service,IAppLogger<ProjectController> logger)
        {
            _projectService = service;
            _logger = logger;
        }
         
        [HttpGet]
        public Response Get([FromQuery] Domain.ProjectSearch searchModel, [FromQuery] AdditionalFilter additionalFilter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return Task.Run<Response>(async () => await this._projectService.GetProjects(searchModel, additionalFilter)).Result;
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);   
        }

        [HttpGet]
        [Route("GetSelectiveProjectData")]
        public Response Get([FromQuery]Domain.ProjectSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            { 
                return Task.Run<Response>(async () => await this._projectService.SearchProjects(searchModel)).Result;
            }
            catch(Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);   
        }

        [HttpGet]
        [Route("GetProject")]
        public Response Get([FromQuery]int projectNumber)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return  _projectService.GetProjects(projectNumber);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNumber);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);   
        }

        [HttpPost]
        public Response Post([FromBody]IList<Domain.Project> projectModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                long? eventId = null;
                AssignValues(projectModel, ValidationType.Add);
                return this._projectService.SaveProjects(projectModel,ref eventId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetProjectBasedOnStatus")]
        public Response GetProjectBasedOnStatus([FromQuery]string contractNumber, int ContractHolderCompanyId, bool isApproved, bool isVisit, bool isOperating, bool isNDT, int? CoordinatorId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _projectService.GetProjectBasedOnStatus(contractNumber, ContractHolderCompanyId, isApproved, isVisit, isOperating, isNDT, CoordinatorId);
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

        [HttpGet]
        [Route("GetProjectKPI")]
        public Response GetProjectKPI([FromQuery]string contractNumber, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _projectService.GetProjectKPI(contractNumber, ContractHolderCompanyId, isVisit, isOperating);
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
        public Response Put([FromBody]IList<Domain.Project> projectModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                long? eventId = null;
                AssignValues(projectModel, ValidationType.Update);
                return this._projectService.ModifyProjects(projectModel,ref eventId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]IList<Domain.Project> projectModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                long? eventId = null;
                AssignValues(projectModel, ValidationType.Delete);
                return this._projectService.DeleteProjects(projectModel,ref eventId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("budgets")]
        public Response Get([FromQuery]string companyCode = null, [FromQuery] string userName = null, [FromQuery]  ContractStatus contractStatus = ContractStatus.All, [FromQuery] bool showMyAssignmentsOnly = true)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _projectService.GetProjectBudgetDetails(companyCode, userName, contractStatus, showMyAssignmentsOnly);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractStatus);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        
        private void AssignValues(IList<Domain.Project> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
