using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Api.Controllers.Visit
{
    [Route("api/visits/{visitid}/technicalSpecialistAccountItemTime")]
    public class VisitTechnicalSpecialistTimeController : BaseController
    {
        private readonly IVisitTechnicalSpecialistTimeService _service = null;
        private readonly IAppLogger<VisitTechnicalSpecialistTimeController> _logger = null;
        public VisitTechnicalSpecialistTimeController(IVisitTechnicalSpecialistTimeService service, IAppLogger<VisitTechnicalSpecialistTimeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]long visitId, [FromQuery]DomainModel.VisitSpecialistAccountItemTime searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.VisitId = visitId;
                return this._service.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute]long visitId, [FromBody]IList<DomainModel.VisitSpecialistAccountItemTime> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Add);
                return this._service.Add(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute]long visitId, [FromBody]IList<DomainModel.VisitSpecialistAccountItemTime> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Update);
                return this._service.Modify(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute]long visitId, [FromBody]IList<DomainModel.VisitSpecialistAccountItemTime> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Delete);
                return this._service.Delete(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.VisitSpecialistAccountItemTime> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}
