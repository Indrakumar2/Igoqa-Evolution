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
    [Route("api/technicalSpecialists/{ePin}/PayRateSchedules")]
    [ApiController]
    public class TechnicalSpecialistPayRateScheduleController : BaseController
    {
        private readonly ITechnicalSpecialistPayScheduleService _technicalSpecialistPayScheduleService = null;
        private readonly IAppLogger<TechnicalSpecialistPayRateScheduleController> _logger = null;

        public TechnicalSpecialistPayRateScheduleController(ITechnicalSpecialistPayScheduleService technicalSpecialistPayScheduleService, IAppLogger<TechnicalSpecialistPayRateScheduleController> logger)
        {
            _logger = logger;
            _technicalSpecialistPayScheduleService = technicalSpecialistPayScheduleService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery] TechnicalSpecialistPayScheduleInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistPayScheduleService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistPayScheduleInfo> tsPayScheduleInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsPayScheduleInfos.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsPayScheduleInfos, ValidationType.Add);
                return this._technicalSpecialistPayScheduleService.Add(tsPayScheduleInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsPayScheduleInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistPayScheduleInfo> tsPayScheduleInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsPayScheduleInfos.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsPayScheduleInfos, ValidationType.Update);
                return this._technicalSpecialistPayScheduleService.Modify(tsPayScheduleInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsPayScheduleInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistPayScheduleInfo> tsPayScheduleInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsPayScheduleInfos.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsPayScheduleInfos, ValidationType.Delete);
                return this._technicalSpecialistPayScheduleService.Delete(tsPayScheduleInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , tsPayScheduleInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<TechnicalSpecialistPayScheduleInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
