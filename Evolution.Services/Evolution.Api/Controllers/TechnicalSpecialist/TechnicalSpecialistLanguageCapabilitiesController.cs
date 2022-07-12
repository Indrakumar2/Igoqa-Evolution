using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Evolution.Common.Enums;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.TechnicalSpecialist.GRM
{
    [Route("api/technicalSpecialists/{ePin}/LanguageCapabilities")]
    [ApiController]
    public class TechnicalSpecialistLanguageCapabilitiesController : BaseController
    {
        private readonly ITechnicalSpecialistLanguageCapabilityService _technicalSpecialistLanguageCapabilityService = null;
        private readonly IAppLogger<TechnicalSpecialistLanguageCapabilitiesController> _logger = null;

        public TechnicalSpecialistLanguageCapabilitiesController(ITechnicalSpecialistLanguageCapabilityService technicalSpecialistLanguageCapabilityService, IAppLogger<TechnicalSpecialistLanguageCapabilitiesController> logger)
        {
            _logger = logger;
            _technicalSpecialistLanguageCapabilityService = technicalSpecialistLanguageCapabilityService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistLanguageCapabilityInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistLanguageCapabilityService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistLanguageCapabilityInfo> languageCapabilities)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(languageCapabilities, ValidationType.Add);
                return this._technicalSpecialistLanguageCapabilityService.Add(SetEPin(languageCapabilities, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, languageCapabilities });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistLanguageCapabilityInfo> languageCapabilities)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(languageCapabilities, ValidationType.Update);
                return this._technicalSpecialistLanguageCapabilityService.Modify(SetEPin(languageCapabilities, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, languageCapabilities });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistLanguageCapabilityInfo> languageCapabilities)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(languageCapabilities, ValidationType.Delete);
                return this._technicalSpecialistLanguageCapabilityService.Delete(SetEPin(languageCapabilities, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, languageCapabilities });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private IList<TechnicalSpecialistLanguageCapabilityInfo> SetEPin(IList<TechnicalSpecialistLanguageCapabilityInfo> LanguageCapability, int ePin)
        {
            LanguageCapability = LanguageCapability?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return LanguageCapability;
        }
        private void AssignValues(IList<TechnicalSpecialistLanguageCapabilityInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
