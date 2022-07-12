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
    [Route("api/technicalSpecialists/{ePin}/Trainings")]
    public class TechnicalSpecialistTrainingController : BaseController
    {
        private readonly ITechnicalSpecialistTrainingService _technicalSpecialistTrainingService = null;
        private readonly IAppLogger<TechnicalSpecialistTrainingController> _logger = null;

        public TechnicalSpecialistTrainingController(ITechnicalSpecialistTrainingService technicalSpecialistTrainingService, IAppLogger<TechnicalSpecialistTrainingController> logger)
        {
            _logger = logger;
            _technicalSpecialistTrainingService = technicalSpecialistTrainingService;
        }
        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery] TechnicalSpecialistTraining searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return _technicalSpecialistTrainingService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistTraining> tsTrainings)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsTrainings.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsTrainings, ValidationType.Add);
                return _technicalSpecialistTrainingService.Add(tsTrainings);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsTrainings });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistTraining> tsTrainings)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsTrainings.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsTrainings, ValidationType.Update);
                return _technicalSpecialistTrainingService.Modify(tsTrainings);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsTrainings });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistTraining> tsTrainings)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                tsTrainings.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(tsTrainings, ValidationType.Delete);
                return _technicalSpecialistTrainingService.Delete(tsTrainings);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, tsTrainings });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<TechnicalSpecialistTraining> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }



    }
}
