using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/Competencies")]
    public class TechnicalSpecialistCompetencyController : BaseController
    {
        private readonly ITechnicalSpecialistCompetencyService _technicalSpecialistCompetencyService = null;
        private readonly IAppLogger<TechnicalSpecialistCompetencyController> _logger = null;

        public TechnicalSpecialistCompetencyController(ITechnicalSpecialistCompetencyService technicalSpecialistCompetencyService, IAppLogger<TechnicalSpecialistCompetencyController> logger)
        {
            _technicalSpecialistCompetencyService = technicalSpecialistCompetencyService;
            _logger = logger;
        }
        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  DomainModel.TechnicalSpecialistCompetency searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistCompetencyService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]  IList<DomainModel.TechnicalSpecialistCompetency> tsCompetencies)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsCompetencies, ValidationType.Add);
                return this._technicalSpecialistCompetencyService.Add(tsCompetencies);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsCompetencies });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]  IList<DomainModel.TechnicalSpecialistCompetency> tsCompetencies)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsCompetencies, ValidationType.Update);
                return this._technicalSpecialistCompetencyService.Modify(tsCompetencies);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsCompetencies });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]  IList<DomainModel.TechnicalSpecialistCompetency> tsCompetencies)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsCompetencies, ValidationType.Delete);
                return this._technicalSpecialistCompetencyService.Delete(tsCompetencies);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsCompetencies });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<DomainModel.TechnicalSpecialistCompetency> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
