using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;


namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/CodeAndStandards")]
    public class TechnicalSpecialistCodeAndStandardController : BaseController
    {
        private readonly ITechnicalSpecialistCodeAndStandardService _technicalSpecialistCodeAndStandardService = null;
        private readonly IAppLogger<TechnicalSpecialistCodeAndStandardController> _logger = null;

        public TechnicalSpecialistCodeAndStandardController(ITechnicalSpecialistCodeAndStandardService technicalSpecialistCodeAndStandardService, IAppLogger<TechnicalSpecialistCodeAndStandardController> logger)
        {
            _technicalSpecialistCodeAndStandardService = technicalSpecialistCodeAndStandardService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, DomainModel.TechnicalSpecialistCodeAndStandardinfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistCodeAndStandardService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistCodeAndStandardinfo> codeAndStandards)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(codeAndStandards, ValidationType.Add);
                return this._technicalSpecialistCodeAndStandardService.Add(SetEPin(codeAndStandards, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, codeAndStandards });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistCodeAndStandardinfo> codeAndStandards)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(codeAndStandards, ValidationType.Update);
                return this._technicalSpecialistCodeAndStandardService.Modify(SetEPin(codeAndStandards, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, codeAndStandards });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistCodeAndStandardinfo> codeAndStandards)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(codeAndStandards, ValidationType.Delete);
                return this._technicalSpecialistCodeAndStandardService.Delete(SetEPin(codeAndStandards, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, codeAndStandards });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }


        private IList<DomainModel.TechnicalSpecialistCodeAndStandardinfo> SetEPin(IList<DomainModel.TechnicalSpecialistCodeAndStandardinfo> codes, int ePin)
        {
            codes = codes?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return codes;
        }
        private void AssignValues(IList<DomainModel.TechnicalSpecialistCodeAndStandardinfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
