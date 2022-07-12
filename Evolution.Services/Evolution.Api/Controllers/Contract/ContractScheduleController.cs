using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Domain=Evolution.Contract.Domain.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Contract
{
    [Route("api/contracts/schedules")]
    [ApiController]
    public class ContractScheduleController : BaseController
    {
        private readonly IContractScheduleService _contractScheduleService = null;
        private readonly IContractScheduleRateService _contractScheduleRateService = null;
        private readonly IAppLogger<ContractScheduleController> _logger = null;

        public ContractScheduleController(IContractScheduleService contractScheduleService, IContractScheduleRateService contractScheduleRateService,IAppLogger<ContractScheduleController> logger)
        {
            this._contractScheduleService = contractScheduleService;
            this._contractScheduleRateService = contractScheduleRateService;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]string contractNumber, [FromQuery]Domain.ContractSchedule searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                searchModel.ContractNumber = contractNumber;
                return this._contractScheduleService.GetContractSchedule(searchModel);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);        
        }

        // D-714
        [HttpGet]
        [Route("rates")]
        public Response Get([FromQuery]Domain.ContractScheduleRate searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._contractScheduleRateService.GetContractScheduleRate(searchModel);
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
        public Response Post([FromQuery] string contractNumber, [FromBody]IList<Domain.ContractSchedule> contractSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(contractSchedules, ValidationType.Add);
                return _contractScheduleService.SaveContractSchedule(contractNumber, contractSchedules);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);  
        }

        [HttpPut]
        public Response Put([FromQuery] string contractNumber, [FromBody]IList<Domain.ContractSchedule> contractSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(contractSchedules, ValidationType.Update);
                return _contractScheduleService.ModifyContractSchedule(contractNumber, contractSchedules);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);  
        }
        /*AddScheduletoRF*/
        [HttpPut]
        [Route("CopyCStoRFC")]
        public Response Put([FromBody]IList<Domain.ContractSchedule> contractSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _contractScheduleService.CopyCStoRFC(contractSchedules);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }

        // Check RelatedFrameworkExists
        [HttpGet]
        [Route("RFCExists")]
        public Response RfcExists([FromQuery] int contractId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return _contractScheduleService.IsRelatedFrameworkContractExists(contractId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractId);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }

        [HttpGet]
        [Route("DuplicateFrameworkSchedules")]
        public Response DuplicateFrameworkContractSchedules([FromQuery] int contractId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _contractScheduleService.IsDuplicateFrameworkSchedulesExists(contractId);
            }
            catch(Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractId);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete ([FromQuery] string contractNumber, [FromBody]IList<Domain.ContractSchedule> contractSchedules)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(contractSchedules, ValidationType.Delete);
                return _contractScheduleService.DeleteContractSchedule(contractNumber, contractSchedules);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<Domain.ContractSchedule> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}