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

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/InternalTrainings")]
    public class TechnicalSpecialistInternalTrainingController : BaseController
    {
        private readonly ITechnicalSpecialistInternalTrainingService _technicalSpecialistInternalTrainingService = null;
        private readonly IAppLogger<TechnicalSpecialistInternalTrainingController> _logger = null;

        public TechnicalSpecialistInternalTrainingController(ITechnicalSpecialistInternalTrainingService technicalSpecialistInternalTrainingService, IAppLogger<TechnicalSpecialistInternalTrainingController> logger)
        {
            _logger = logger;
            _technicalSpecialistInternalTrainingService = technicalSpecialistInternalTrainingService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistInternalTraining searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistInternalTrainingService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistInternalTraining> tsInternalTrainings)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsInternalTrainings, ValidationType.Add);
                return this._technicalSpecialistInternalTrainingService.Add(tsInternalTrainings);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsInternalTrainings });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistInternalTraining> tsInternalTrainings)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsInternalTrainings, ValidationType.Update);
                return this._technicalSpecialistInternalTrainingService.Modify(tsInternalTrainings);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsInternalTrainings });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistInternalTraining> tsInternalTrainings)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(tsInternalTrainings, ValidationType.Delete);
                return this._technicalSpecialistInternalTrainingService.Delete(tsInternalTrainings);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , tsInternalTrainings });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<TechnicalSpecialistInternalTraining> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }


    }
}
