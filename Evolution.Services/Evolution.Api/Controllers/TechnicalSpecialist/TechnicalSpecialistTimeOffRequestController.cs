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
    [Route("api/technicalSpecialists/{ePin}/TimeOffRequests")]
    public class TechnicalSpecialistTimeOffRequestController : BaseController
    {
        private readonly ITechnicalSpecialistTimeOffRequestService _technicalSpecialistTimeOffRequestService = null;
        private readonly IAppLogger<TechnicalSpecialistTimeOffRequestController> _logger = null;

        public TechnicalSpecialistTimeOffRequestController(ITechnicalSpecialistTimeOffRequestService technicalSpecialistTimeOffRequestService, IAppLogger<TechnicalSpecialistTimeOffRequestController> logger)
        {
            _logger = logger;
            _technicalSpecialistTimeOffRequestService = technicalSpecialistTimeOffRequestService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery] TechnicalSpecialistTimeOffRequest searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistTimeOffRequestService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistTimeOffRequest> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValueFromToken(ref model);
                AssignValues(model, ValidationType.Add);
                return this._technicalSpecialistTimeOffRequestService.Add(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValueFromToken(ref IList<TechnicalSpecialistTimeOffRequest> model)
        {
            foreach (var m in model)
            {
                m.UserTypes = UserType.Split(',');
                // m.CompanyCode = UserCompanyCode; //def 978 
                m.RequestedBy = UserName;
                m.RequestedOn = DateTime.UtcNow;
            } 
        }
        private void AssignValues(IList<TechnicalSpecialistTimeOffRequest> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
