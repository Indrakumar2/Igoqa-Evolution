using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;

using Model = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Api.Controllers.Contract
{
    [Route("api/contracts/detail")]
    public class ContractDetailController : BaseController
    {
        private readonly IContractDetailService _service = null;
        private readonly IAppLogger<ContractDetailController> _logger = null;

        public ContractDetailController(IContractDetailService service,IAppLogger<ContractDetailController> logger)
        {            
            this._service = service;
            this._logger = logger;
        }

        [HttpPost]
        public Response Post([FromBody]Model.ContractDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Add);
                return _service.SaveContractDetail( model );
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
        public Response Put([FromBody]Model.ContractDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;

            try{
                AssignValues(model, ValidationType.Update);
                return _service.UpdateContractDetail(model );
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
        public Response Delete([FromBody]Model.ContractDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;

            try{
                AssignValues(model, ValidationType.Delete);
                return _service.DeleteContractDetail(model );
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(Model.ContractDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.ContractInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractExchangeRates, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractInvoiceAttachments, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractInvoiceReferences, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractSchedules, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractScheduleRates, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractDocuments, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ContractNotes, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractInfo, "ModifiedBy", UserName);                    
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractExchangeRates, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractInvoiceAttachments, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractInvoiceReferences, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractSchedules, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractScheduleRates, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractDocuments, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.ContractNotes, "ModifiedBy", UserName);
            }
        }
    }
}