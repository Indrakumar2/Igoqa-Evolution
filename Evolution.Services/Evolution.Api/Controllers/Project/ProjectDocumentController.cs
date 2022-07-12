using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Project.Domain.Interfaces.Projects;
using Evolution.Logging.Interfaces;
using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
namespace Evolution.Api.Controllers.Project
{
    [Route("api/projects/documents")]
    [ApiController]
    public class ProjectDocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService = null;
        private IProjectDocumentService _projectDocumentService = null;
        private readonly IAppLogger<ProjectDocumentController> _logger = null;

        public ProjectDocumentController(IDocumentService documentService,IProjectDocumentService projectDocumentService,IAppLogger<ProjectDocumentController> logger)
        {
            this._documentService = documentService;
            this._projectDocumentService = projectDocumentService;
            this._logger = logger;
        }

        [HttpPost]
        [Route("GetProjectSpecificDocuments")]
        public Response GetListOfProjectDocuments([FromBody]List<int> projectNumbers)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                IList<ModuleDocument> searchModel = new List<ModuleDocument>();
                projectNumbers.ForEach(x =>
                { 
                    searchModel.Add(new ModuleDocument { ModuleCode = "PRJ", ModuleRefCode = x.ToString() });

                });
                return _documentService.Get(searchModel);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNumbers);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpGet]
        [Route("GetContractProjectDocuments")]
        public Response GetListOfContractProjectDocuments([FromQuery] string contractNumber)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _projectDocumentService.GetContractProjectDocuments(contractNumber);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNumber);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpGet]
        [Route("GetCustomerProjectDocuments")]
        public Response GetListOfCustomerProjectDocuments([FromQuery] int? customerId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _projectDocumentService.GetCustomerProjectDocuments(customerId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerId);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}
