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
    [Route("api/technicalSpecialists/{ePin}/Certifications")]
    [ApiController]
    public class TechnicalSpecialistCertificationController : BaseController
    {
        private readonly ITechnicalSpecialistCertificationService _technicalSpecialistCertificationService = null;
        private readonly IAppLogger<TechnicalSpecialistCertificationController> _logger = null;

        public TechnicalSpecialistCertificationController(ITechnicalSpecialistCertificationService technicalSpecialistCertificationService, IAppLogger<TechnicalSpecialistCertificationController> logger)
        {
            _technicalSpecialistCertificationService = technicalSpecialistCertificationService;
            _logger = logger;
        }
        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistCertification searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return _technicalSpecialistCertificationService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistCertification> tsCertifications)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsCertifications.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsCertifications, ValidationType.Add);
                return _technicalSpecialistCertificationService.Add(tsCertifications);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsCertifications });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistCertification> tsCertifications)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsCertifications.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsCertifications, ValidationType.Update);
                return _technicalSpecialistCertificationService.Modify(tsCertifications);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsCertifications });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }
        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistCertification> tsCertifications)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsCertifications.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsCertifications, ValidationType.Delete);
                return _technicalSpecialistCertificationService.Delete(tsCertifications);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , tsCertifications });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<TechnicalSpecialistCertification> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }


    }
}
