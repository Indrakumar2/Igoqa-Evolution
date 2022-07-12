using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evolution.Logging.Interfaces;
using System;
using Domain = Evolution.Contract.Domain.Models.Contracts;
namespace Evolution.Api.Controllers.Contract
{
    [Produces("application/json")]
    [Route("api/contracts")]
    public class ContractController : BaseController
    {
        private readonly IContractService _contractService = null;
        private readonly IAppLogger<ContractController> _logger = null;


        public ContractController(IContractService contractService,IAppLogger<ContractController> logger)
        {
            this._contractService = contractService;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] Domain.ContractSearch searchModel, [FromQuery] AdditionalFilter additionalFilter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return Task.Run<Response>(async () => await this._contractService.GetContract(searchModel, additionalFilter)).Result;
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }


        [HttpGet]
        [Route("customerContracts")]
        public Response GetCustomers([FromQuery] Domain.ContractSearch model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
            return this._contractService.GetContractBasedOnCustomers(model);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("visitTimesheetContracts")]
        public Response GetApprovedVisitContracts([FromQuery]string customerCode, int ContractHolderCompanyId ,bool isVisit, bool isNDT)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._contractService.GetApprovedVisitContracts(customerCode, ContractHolderCompanyId, isVisit, isNDT);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), isVisit);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }

        [HttpGet]
        [Route("visitTimesheetUnApprovedContracts")]
        public Response GetUnApprovedVisitContracts([FromQuery]string customerCode,int? CoordinatorId, int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._contractService.GetUnApprovedVisitContracts(customerCode, CoordinatorId, ContractHolderCompanyId, isVisit, isOperating);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), isVisit);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("visitTimesheetKPIContracts")]
        public Response GetvisitTimesheetKPIContracts([FromQuery]string customerCode, int ContractHolderCompanyId, bool isOperating, bool isVisit)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._contractService.GetvisitTimesheetKPIContracts(customerCode, ContractHolderCompanyId, isOperating, isVisit);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), isVisit);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("searchContracts")]
        public Response Get([FromQuery] Domain.ContractSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
            return Task.Run<Response>(async () => await this._contractService.GetBaseContract(searchModel)).Result;
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("searchCompanyContracts")]
        public Response Get([FromQuery] Domain.BaseContract searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _contractService.FetchCompanyContracts(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("budgets")]
        public Response Get([FromQuery] string companyCode = null, [FromQuery] string userName = null, [FromQuery]  ContractStatus contractStatus = ContractStatus.All, [FromQuery] bool showMyAssignmentsOnly = true)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
            return _contractService.GetContractBudgetDetails(companyCode, userName, contractStatus, showMyAssignmentsOnly);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]IList<Domain.Contract> contractModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;

            try{
                long? eventId = null;
                AssignValues(contractModel, ValidationType.Add);
                return this._contractService.SaveContract(contractModel,ref eventId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]IList<Domain.Contract> contractModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;

            try{
                long? eventId = null;
                AssignValues(contractModel, ValidationType.Update);
                return this._contractService.ModifyContract(contractModel,ref eventId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]IList<Domain.Contract> contractModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;

            try{
                long? eventId = null;
                AssignValues(contractModel, ValidationType.Delete);
                return this._contractService.DeleteContract(contractModel,ref eventId);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<Domain.Contract> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
