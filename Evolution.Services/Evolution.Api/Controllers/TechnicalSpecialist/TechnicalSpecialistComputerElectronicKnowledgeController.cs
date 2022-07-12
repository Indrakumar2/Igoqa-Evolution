using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;


namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/ComputerElectronicKnowledges")]
    public class TechnicalSpecialistComputerElectronicKnowledgeController : BaseController
    {
        private readonly ITechnicalSpecialistComputerElectronicKnowledgeService _technicalSpecialistComputerElectronicKnowledgeService = null;
        private readonly IAppLogger<TechnicalSpecialistComputerElectronicKnowledgeController> _logger = null;

        public TechnicalSpecialistComputerElectronicKnowledgeController(ITechnicalSpecialistComputerElectronicKnowledgeService technicalSpecialistComputerElectronicKnowledgeService, IAppLogger<TechnicalSpecialistComputerElectronicKnowledgeController> logger)
        {
            _technicalSpecialistComputerElectronicKnowledgeService = technicalSpecialistComputerElectronicKnowledgeService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]int ePin, TechnicalSpecialistComputerElectronicKnowledgeInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistComputerElectronicKnowledgeService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPost]
        public Response Post([FromRoute]int ePin, [FromBody] IList<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo> computerElectronicKnowledgeInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(computerElectronicKnowledgeInfo, ValidationType.Add);
                return this._technicalSpecialistComputerElectronicKnowledgeService.Add(SetEPin(computerElectronicKnowledgeInfo, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, computerElectronicKnowledgeInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPut]
        public Response Put([FromRoute]int ePin, [FromBody] IList<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo> computerElectronicKnowledgeInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(computerElectronicKnowledgeInfo, ValidationType.Update);
                return this._technicalSpecialistComputerElectronicKnowledgeService.Modify(SetEPin(computerElectronicKnowledgeInfo, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, computerElectronicKnowledgeInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody] IList<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo> computerElectronicKnowledgeInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(computerElectronicKnowledgeInfo, ValidationType.Delete);
                return this._technicalSpecialistComputerElectronicKnowledgeService.Delete(SetEPin(computerElectronicKnowledgeInfo, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, computerElectronicKnowledgeInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private IList<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo> SetEPin(IList<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo> codes, int ePin)
        {
            codes = codes?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return codes;
        }
        private void AssignValues(IList<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}

