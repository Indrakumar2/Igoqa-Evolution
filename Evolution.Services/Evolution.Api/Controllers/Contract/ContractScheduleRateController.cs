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
    //[Route("api/contracts/{contractNumber}/schedules/{scheduleName}/rates")]
    [Route("api/contracts/schedules/{scheduleName}/rates")]
    [ApiController]
    public class ContractScheduleRateController : BaseController
    {
        private readonly IContractScheduleRateService _contractSchRateService = null;
        private readonly IAppLogger<ContractScheduleRateController> _logger = null;

        public ContractScheduleRateController(IContractScheduleRateService contractSchRateService,IAppLogger<ContractScheduleRateController> logger)
        {
            this._contractSchRateService = contractSchRateService;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]string contractNumber, [FromRoute]string scheduleName, [FromQuery]Domain.ContractScheduleRate searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                searchModel.ContractNumber = contractNumber;
                searchModel.ScheduleName = scheduleName;
                return this._contractSchRateService.GetContractScheduleRate(searchModel);
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
        public Response Post([FromQuery]string contractNumber, [FromRoute]string scheduleName, [FromBody] IList<Domain.ContractScheduleRate> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Add);
                return _contractSchRateService.SaveContractScheduleRate(contractNumber, scheduleName, model, ValidationType.Add);
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
        public Response Put([FromQuery]string contractNumber, [FromRoute]string scheduleName, [FromBody] IList<Domain.ContractScheduleRate> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Update);
                return _contractSchRateService.ModifyContractScheduleRate(contractNumber, scheduleName, model);
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
        public Response Delete([FromQuery]string contractNumber, [FromRoute]string scheduleName, [FromBody] IList<Domain.ContractScheduleRate> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(model, ValidationType.Delete);
                return _contractSchRateService.DeleteContractScheduleRate(contractNumber, scheduleName, model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<Domain.ContractScheduleRate> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}