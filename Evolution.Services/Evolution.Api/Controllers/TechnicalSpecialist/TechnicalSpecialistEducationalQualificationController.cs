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

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/EducationalQualifications")]
    [ApiController]
    public class TechnicalSpecialistEducationalQualificationController : BaseController
    {
        private readonly ITechnicalSpecialistEducationalQualificationService _technicalSpecialistEQService = null;
        private readonly IAppLogger<TechnicalSpecialistEducationalQualificationController> _logger = null;

        public TechnicalSpecialistEducationalQualificationController(ITechnicalSpecialistEducationalQualificationService technicalSpecialistEducationalQualificationService, IAppLogger<TechnicalSpecialistEducationalQualificationController> logger)
        {
            _logger = logger;
            _technicalSpecialistEQService = technicalSpecialistEducationalQualificationService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistEducationalQualificationInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistEQService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistEducationalQualificationInfo> tsEducationalInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsEducationalInfo, ValidationType.Add);
                return this._technicalSpecialistEQService.Add(SetEPin(tsEducationalInfo, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsEducationalInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistEducationalQualificationInfo> tsEducationalInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsEducationalInfo, ValidationType.Update);
                return this._technicalSpecialistEQService.Modify(SetEPin(tsEducationalInfo, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsEducationalInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistEducationalQualificationInfo> tsEducationalInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsEducationalInfo, ValidationType.Delete);
                return this._technicalSpecialistEQService.Delete(SetEPin(tsEducationalInfo, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsEducationalInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private IList<TechnicalSpecialistEducationalQualificationInfo> SetEPin(IList<TechnicalSpecialistEducationalQualificationInfo> EduQulification, int ePin)
        {
            EduQulification = EduQulification?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return EduQulification;
        }
        private void AssignValues(IList<TechnicalSpecialistEducationalQualificationInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}