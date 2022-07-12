using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Contract
{
    [Route("api/contracts/notes")]
    [ApiController]
    public class ContractNoteController : BaseController
    {
        private readonly IContractNoteService _contractNoteService = null;
        private readonly IAppLogger<ContractNoteController> _logger = null;

        public ContractNoteController(IContractNoteService contractNoteService,IAppLogger<ContractNoteController> logger)
        {
            this._contractNoteService = contractNoteService;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]string contractNumber,[FromQuery]ContractNote searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                searchModel.ContractNumber = contractNumber;
                return this._contractNoteService.GetContractNote(searchModel);
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
        public Response Post([FromQuery]string contractNumber,[FromBody]IList<ContractNote> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                AssignValues(searchModel, ValidationType.Add);
                return this._contractNoteService.SaveContractNote(contractNumber,searchModel,true);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);        
        }

         private void AssignValues(IList<ContractNote> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}