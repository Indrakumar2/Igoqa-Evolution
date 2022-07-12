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

    [Route("api/technicalSpecialists/{ePin}/StampInfos")]
    [ApiController]
    public class TechnicalSpecialistStampController : BaseController
    {
        private readonly ITechnicalSpecialistStampInfoService _technicalSpecialistStampService = null;
        private readonly IAppLogger<TechnicalSpecialistStampController> _logger = null;

        public TechnicalSpecialistStampController(ITechnicalSpecialistStampInfoService technicalSpecialistStampService, IAppLogger<TechnicalSpecialistStampController> logger)
        {
            _logger = logger;
            _technicalSpecialistStampService = technicalSpecialistStampService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery] TechnicalSpecialistStampInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistStampService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistStampInfo> stamps)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(stamps, ValidationType.Add);
                return this._technicalSpecialistStampService.Add(SetEPin(stamps, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, stamps });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistStampInfo> stamps)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(stamps, ValidationType.Update);
                return this._technicalSpecialistStampService.Modify(SetEPin(stamps, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, stamps });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistStampInfo> stamps)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(stamps, ValidationType.Delete);
                return this._technicalSpecialistStampService.Delete(SetEPin(stamps, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , stamps });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistStampInfo> SetEPin(IList<TechnicalSpecialistStampInfo> stamps, int ePin)
        {
            stamps = stamps?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return stamps;
        }
        private void AssignValues(IList<TechnicalSpecialistStampInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
