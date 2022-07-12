using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Logging.Interfaces;
using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;


namespace Evolution.Api.Controllers.Contract
{
    [Route("api/contracts/documents")]
    [ApiController]
    public class ContractDocumentController : ControllerBase
    {
        private IDocumentService _service = null;
        private IContractDocumentService _contractDocumentService = null;
        private readonly IAppLogger<ContractDocumentController> _logger = null;

        public ContractDocumentController(IDocumentService service,IContractDocumentService contractDocumentService,IAppLogger<ContractDocumentController> logger)
        {
            _service = service;
            _contractDocumentService = contractDocumentService;
            _logger = logger;
        }
          
        [HttpPost]
        [Route("GetContractSpecificDocuments")]
        public Response GetListOfContractDocuments([FromBody]List<string> contractNumber)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                 IList<ModuleDocument> searchModel = new List<ModuleDocument>();
                    contractNumber.ForEach(x =>
                {
                    searchModel.Add(new ModuleDocument { ModuleCode = "CNT", ModuleRefCode = x });

                }); 
                return _service.Get(searchModel);    
            } 
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);         
        } 
        [HttpGet]
        [Route("GetCustomerContractDocuments")]
        public Response GetListOfCustomerContractDocuments([FromQuery] int? customerId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _contractDocumentService.GetCustomerContractDocuments(customerId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}