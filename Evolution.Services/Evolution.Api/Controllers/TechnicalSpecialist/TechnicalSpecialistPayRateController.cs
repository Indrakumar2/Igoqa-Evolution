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

namespace Evolution.Api.Controllers.TechnicalSpecialist.GRM
{
    [Route("api/technicalSpecialists/{ePin}/PayRateSchedules/{PayScheduleId}/PayRates")]
    [ApiController]
    public class TechnicalSpecialistPayRateController : BaseController
    {

        private readonly ITechnicalSpecialistPayRateService _technicalSpecialistPayRateService = null;
        private readonly IAppLogger<TechnicalSpecialistPayRateController> _logger = null;

        public TechnicalSpecialistPayRateController(ITechnicalSpecialistPayRateService technicalSpecialistPayRateService, IAppLogger<TechnicalSpecialistPayRateController> logger)
        {
            _logger = logger;
            _technicalSpecialistPayRateService = technicalSpecialistPayRateService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromRoute] int PayScheduleId, [FromQuery] TechnicalSpecialistPayRateInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                if (PayScheduleId > 0)
                {
                    searchModel.PayScheduleId = PayScheduleId;
                }
                return _technicalSpecialistPayRateService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromRoute] int PayScheduleId, [FromBody] IList<TechnicalSpecialistPayRateInfo> tsPayRateInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                SetEPinAndPayScheduleId(ref tsPayRateInfos, ePin, PayScheduleId);
                AssignValues(tsPayRateInfos, ValidationType.Add);
                return _technicalSpecialistPayRateService.Add(tsPayRateInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, PayScheduleId, tsPayRateInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromRoute] int PayScheduleId, [FromBody] IList<TechnicalSpecialistPayRateInfo> tsPayRateInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                SetEPinAndPayScheduleId(ref tsPayRateInfos, ePin, PayScheduleId);
                AssignValues(tsPayRateInfos, ValidationType.Update);
                return _technicalSpecialistPayRateService.Modify(tsPayRateInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, PayScheduleId, tsPayRateInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromRoute] int PayScheduleId, [FromBody] IList<TechnicalSpecialistPayRateInfo> tsPayRateInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                SetEPinAndPayScheduleId(ref tsPayRateInfos, ePin, PayScheduleId);
                AssignValues(tsPayRateInfos, ValidationType.Delete);
                return _technicalSpecialistPayRateService.Delete(tsPayRateInfos);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, PayScheduleId, tsPayRateInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void SetEPinAndPayScheduleId(ref IList<TechnicalSpecialistPayRateInfo> tsPayRateInfos, int ePin, int payScheduleId)
        {
            tsPayRateInfos.ToList().ForEach(x =>
            {
                x.Epin = ePin;
                if (payScheduleId > 0)
                    x.PayScheduleId = payScheduleId;
            });

        }
        private void AssignValues(IList<TechnicalSpecialistPayRateInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
