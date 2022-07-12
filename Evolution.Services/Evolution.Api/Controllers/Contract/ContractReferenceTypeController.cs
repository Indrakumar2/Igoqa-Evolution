using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Domain = Evolution.Contract.Domain.Models.Contracts;
using Evolution.Logging.Interfaces;
using System;


namespace Evolution.Api.Controllers.Contract
{
    [Route("api/contracts/referencetypes")]
    [ApiController]
    public class ContractReferenceTypeController : BaseController
    {
        private IContractInvoiceReferenceTypeService _service = null;
        private readonly IAppLogger<ContractReferenceTypeController> _logger = null;

        public ContractReferenceTypeController(IContractInvoiceReferenceTypeService service,IAppLogger<ContractReferenceTypeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]string contractNumber, [FromQuery] Domain.ContractInvoiceReferenceType searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                searchModel.ContractNumber = contractNumber;
                return _service.GetContractInvoiceReferenceType(searchModel);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }

        [HttpPost]
        public Response Post([FromQuery]string contractNumber, [FromBody] IList<Domain.ContractInvoiceReferenceType> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Add);
                return _service.SaveContractInvoiceReferenceType(contractNumber, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);  
        }

        [HttpPut]
        public Response Put([FromQuery]string contractNumber, [FromBody] IList<Domain.ContractInvoiceReferenceType> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Update);
                return _service.ModifyContractInvoiceReferenceType(contractNumber, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);  
        }

        [HttpDelete]
        public Response Delete([FromQuery]string contractNumber, [FromBody] IList<Domain.ContractInvoiceReferenceType> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Delete);
                return _service.DeleteContractInvoiceReferenceType(contractNumber, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);  
        }

        private void AssignValues(IList<Domain.ContractInvoiceReferenceType> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}