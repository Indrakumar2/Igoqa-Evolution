using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Linq;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Api.Controllers.Base;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/ContactInfos")]
    [ApiController]
    public class TechnicalSpecialistContactController : BaseController
    {
        private readonly ITechnicalSpecialistContactService _technicalSpecialistContactService = null;
        private readonly IAppLogger<TechnicalSpecialistContactController> _logger = null;

        public TechnicalSpecialistContactController(ITechnicalSpecialistContactService technicalSpecialistContactService, IAppLogger<TechnicalSpecialistContactController> logger)
        {
            _technicalSpecialistContactService = technicalSpecialistContactService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  DomainModel.TechnicalSpecialistContactInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistContactService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistContactInfo> contactInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(contactInfos, ValidationType.Add);
                return this._technicalSpecialistContactService.Add(SetEPin(contactInfos, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, contactInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistContactInfo> contactInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(contactInfos, ValidationType.Update);
                return this._technicalSpecialistContactService.Modify(SetEPin(contactInfos, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, contactInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]IList<DomainModel.TechnicalSpecialistContactInfo> contactInfos)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(contactInfos, ValidationType.Delete);
                return this._technicalSpecialistContactService.Delete(SetEPin(contactInfos, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, contactInfos });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private IList<DomainModel.TechnicalSpecialistContactInfo> SetEPin(IList<DomainModel.TechnicalSpecialistContactInfo> contacts, int ePin)
        {
            contacts = contacts?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return contacts;

        }
        private void AssignValues(IList<DomainModel.TechnicalSpecialistContactInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}