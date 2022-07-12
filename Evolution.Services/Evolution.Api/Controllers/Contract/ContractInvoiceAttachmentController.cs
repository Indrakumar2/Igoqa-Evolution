using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;
using Domain= Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Api.Controllers.Contract
{
    // [Route("api/contracts/{contractNumber}/invoice/attachments")]
    [Route("api/contracts/invoice/attachments")]
    [ApiController]
    public class ContractInvoiceAttachmentController : BaseController
    {
        private IContractInvoiceAttachmentService _service = null;
        private readonly IAppLogger<ContractInvoiceAttachmentController> _logger = null;

        public ContractInvoiceAttachmentController(IContractInvoiceAttachmentService service,IAppLogger<ContractInvoiceAttachmentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]string contractNumber, [FromQuery] Domain.ContractInvoiceAttachment searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                searchModel.ContractNumber = contractNumber;
                return _service.GetContractInvoiceAttachment(searchModel);
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
        public Response Post([FromQuery]string contractNumber, [FromBody] IList<Domain.ContractInvoiceAttachment> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Add);
                return _service.SaveContractInvoiceAttachment(contractNumber, model);
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
        public Response Put([FromQuery]string contractNumber, [FromBody] IList<Domain.ContractInvoiceAttachment> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Update);
                return _service.ModifyContractInvoiceAttachment(contractNumber, model);
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
        public Response Delete([FromQuery]string contractNumber, [FromBody] IList<Domain.ContractInvoiceAttachment> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Delete);
                return _service.DeleteContractInvoiceAttachment(contractNumber, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        
        private void AssignValues(IList<Domain.ContractInvoiceAttachment> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}