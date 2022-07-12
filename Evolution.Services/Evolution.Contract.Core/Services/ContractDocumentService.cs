using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Evolution.Contract.Core.Services
{
    public class ContractDocumentService :IContractDocumentService
    {
        private readonly IAppLogger<ContractDocumentService> _logger = null;
        private readonly IContractRepository _repository = null;
        private IDocumentService _service = null;

        public ContractDocumentService(IAppLogger<ContractDocumentService> logger, IContractRepository repository, IDocumentService service)
        {
            this._logger = logger;
            this._repository = repository;
            this._service = service;
        }
        public Response GetCustomerContractDocuments(int? customerId)
        {
            Exception exception = null;
            List<string> result = null;
            List<ModuleDocument> response = null;
            List<string> strContractIds = new List<string>();
            try
            {
                result = this._repository.GetCustomerContractNumbers(customerId);
                if (result?.Count > 0)
                {
                    strContractIds = result?.ConvertAll<string>(x => Convert.ToString(x));
                }
                if (strContractIds?.Count > 0)
                    response = _service.Get(ModuleCodeType.CNT, strContractIds).Result.Populate<List<ModuleDocument>>();
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