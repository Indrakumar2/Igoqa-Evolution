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

namespace Evolution.Api.Controllers.TechnicalSpecialist
{

    [Route("api/technicalSpecialists/{ePin}/WorkHistoryInfos")]
    [ApiController]
    public class TechnicalSpecialistWorkHistoryController : BaseController
    {
        private readonly ITechnicalSpecialistWorkHistoryService _technicalSpecialistWorkHistoryService = null;
        private readonly IAppLogger<TechnicalSpecialistWorkHistoryController> _logger = null;

        public TechnicalSpecialistWorkHistoryController(ITechnicalSpecialistWorkHistoryService technicalSpecialistWorkHistoryService, IAppLogger<TechnicalSpecialistWorkHistoryController> logger)
        {
            _logger = logger;
            _technicalSpecialistWorkHistoryService = technicalSpecialistWorkHistoryService;
        }
        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistWorkHistoryInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistWorkHistoryService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin,searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistWorkHistoryInfo> workHistoryInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(workHistoryInfos, ValidationType.Add);
                return this._technicalSpecialistWorkHistoryService.Add(SetEPin(workHistoryInfos, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, workHistoryInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistWorkHistoryInfo> workHistoryInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(workHistoryInfos, ValidationType.Update);
                return this._technicalSpecialistWorkHistoryService.Modify(SetEPin(workHistoryInfos, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, workHistoryInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistWorkHistoryInfo> workHistoryInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(workHistoryInfos, ValidationType.Delete);
                return this._technicalSpecialistWorkHistoryService.Delete(SetEPin(workHistoryInfos, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , workHistoryInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private IList<TechnicalSpecialistWorkHistoryInfo> SetEPin(IList<TechnicalSpecialistWorkHistoryInfo> Workhistorys, int ePin)
        {
            Workhistorys = Workhistorys?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return Workhistorys;
        }
        private void AssignValues(IList<TechnicalSpecialistWorkHistoryInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
