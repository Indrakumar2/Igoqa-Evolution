using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/CommodityEquipmenKnowledges")]
    [ApiController]
    public class TechnicalSpecialistCommodityEquipKnowledgeController : BaseController
    {
        private readonly ITechnicalSpecialCommodityEquipmentKnowledgeService _technicalSpecialCommodityEquipmentKnowledgeService = null;
        private readonly IAppLogger<TechnicalSpecialistCommodityEquipKnowledgeController> _logger = null;

        public TechnicalSpecialistCommodityEquipKnowledgeController(ITechnicalSpecialCommodityEquipmentKnowledgeService technicalSpecialCommodityEquipmentKnowledgeService, IAppLogger<TechnicalSpecialistCommodityEquipKnowledgeController> logger)
        {
            _technicalSpecialCommodityEquipmentKnowledgeService = technicalSpecialCommodityEquipmentKnowledgeService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistCommodityEquipmentKnowledgeInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialCommodityEquipmentKnowledgeService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> commodityEquipKnowledge)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(commodityEquipKnowledge, ValidationType.Add);
                return this._technicalSpecialCommodityEquipmentKnowledgeService.Add(SetEPin(commodityEquipKnowledge, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, commodityEquipKnowledge });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> commodityEquipKnowledge)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(commodityEquipKnowledge, ValidationType.Update);
                return this._technicalSpecialCommodityEquipmentKnowledgeService.Modify(SetEPin(commodityEquipKnowledge, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, commodityEquipKnowledge });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> commodityEquipKnowledge)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(commodityEquipKnowledge, ValidationType.Delete);
                return this._technicalSpecialCommodityEquipmentKnowledgeService.Delete(SetEPin(commodityEquipKnowledge, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, commodityEquipKnowledge });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> SetEPin(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> comdtyEquipKnowledge, int ePin)
        {
            comdtyEquipKnowledge = comdtyEquipKnowledge?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return comdtyEquipKnowledge;
        }
        private void AssignValues(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}