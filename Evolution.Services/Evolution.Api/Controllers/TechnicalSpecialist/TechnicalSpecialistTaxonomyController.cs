using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.AspNetCore.Mvc;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Linq;
using Evolution.Common.Enums;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{ 
    [Route("api/technicalSpecialists/{ePin}/Taxonomies")]
    [ApiController]
    public class TechnicalSpecialistTaxonomyController : BaseController
    {
        private readonly ITechnicalSpecialistTaxonomyService _taxonomyService = null;
        private readonly IAppLogger<TechnicalSpecialistTaxonomyController> _logger = null;

        public TechnicalSpecialistTaxonomyController(ITechnicalSpecialistTaxonomyService taxonomyService, IAppLogger<TechnicalSpecialistTaxonomyController> logger)
        {
            _logger = logger;
            _taxonomyService = taxonomyService;
        }
        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  DomainModel.TechnicalSpecialistTaxonomyInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._taxonomyService.Get(searchModel);
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
        //D684
        [HttpGet]
        [Route("IsTaxonomyHistoryExists")]
        public Response IsTaxonomyHistoryExists([FromRoute] int ePin)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._taxonomyService.IsTaxonomyHistoryExists(ePin);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPost]
        public Response Post([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistTaxonomyInfo> taxonomy)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._taxonomyService.Add(SetEPin(taxonomy, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, taxonomy });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistTaxonomyInfo> taxonomy)

        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._taxonomyService.Modify(SetEPin(taxonomy, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, taxonomy });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody] IList<DomainModel.TechnicalSpecialistTaxonomyInfo> taxonomy)

        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._taxonomyService.Delete(SetEPin(taxonomy, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, taxonomy });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private IList<DomainModel.TechnicalSpecialistTaxonomyInfo> SetEPin(IList<DomainModel.TechnicalSpecialistTaxonomyInfo> taxonomy, int ePin)
        {
            taxonomy = taxonomy?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return taxonomy;
        }
        private void AssignValues(IList<DomainModel.TechnicalSpecialistTaxonomyInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
