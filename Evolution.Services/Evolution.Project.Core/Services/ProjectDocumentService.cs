using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Projects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Evolution.Project.Core.Services
{
    public class ProjectDocumentService : IProjectDocumentService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ProjectDocumentService> _logger = null;
        private readonly IProjectRepository _repository = null;
        private readonly JObject _messages = null;
        private IDocumentService _service = null;

        public ProjectDocumentService(IMapper mapper, IAppLogger<ProjectDocumentService> logger, IProjectRepository repository, IDocumentService service, JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._service = service;
            this._messages = messages;

        }
        public Response GetContractProjectDocuments(string contractNumber)
        {
            Exception exception = null;
            List<int> result = null;
            List<ModuleDocument> response = null;
            List<string> strProjectIds = new List<string>();
            try
            {
                result = this._repository.GetContractProjectIds(contractNumber);
                if (result?.Count > 0)
                {
                    strProjectIds = result?.ConvertAll<string>(x => x.ToString());
                }
                if (strProjectIds?.Count > 0)
                    response = _service.Get(ModuleCodeType.PRJ, strProjectIds).Result.Populate<List<ModuleDocument>>();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, response, exception);
        }

        public Response GetCustomerProjectDocuments(int? customerId)
        {
            Exception exception = null;
            List<int> result = null;
            List<ModuleDocument> response = null;
            List<string> strProjectIds = new List<string>();
            try
            {
                result = this._repository.GetCustomerProjectIds(customerId);
                if (result?.Count > 0)
                {
                    strProjectIds = result?.ConvertAll<string>(x => x.ToString());
                }
                if (strProjectIds?.Count > 0)
                    response = _service.Get(ModuleCodeType.PRJ, strProjectIds).Result.Populate<List<ModuleDocument>>();
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