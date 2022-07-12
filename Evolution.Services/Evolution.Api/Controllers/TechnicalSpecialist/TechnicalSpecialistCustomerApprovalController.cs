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
    [Route("api/technicalSpecialists/{ePin}/CustomerApprovalInfos")]
    [ApiController]
    public class TechnicalSpecialistCustomerApprovalController : BaseController
    {
        private readonly ITechnicalSpecialistCustomerApprovalService _technicalSpecialistCustomerApprovalService = null;
        private readonly IAppLogger<TechnicalSpecialistCustomerApprovalController> _logger = null;

        public TechnicalSpecialistCustomerApprovalController(ITechnicalSpecialistCustomerApprovalService technicalSpecCustomerApprovalService, IAppLogger<TechnicalSpecialistCustomerApprovalController> logger)
        {
            _logger = logger;
            _technicalSpecialistCustomerApprovalService = technicalSpecCustomerApprovalService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery]  TechnicalSpecialistCustomerApprovalInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistCustomerApprovalService.Get(searchModel);
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
        public Response Post([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistCustomerApprovalInfo> customerApprovals)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(customerApprovals, ValidationType.Add);
                return this._technicalSpecialistCustomerApprovalService.Add(SetEPin(customerApprovals, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, customerApprovals });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }
        [HttpPut]
        public Response Put([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistCustomerApprovalInfo> customerApprovals)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(customerApprovals, ValidationType.Update);
                return this._technicalSpecialistCustomerApprovalService.Modify(SetEPin(customerApprovals, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, customerApprovals });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpDelete]
        public Response Delete([FromRoute] int ePin, [FromBody]  IList<TechnicalSpecialistCustomerApprovalInfo> customerApprovals)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(customerApprovals, ValidationType.Delete);
                return this._technicalSpecialistCustomerApprovalService.Delete(SetEPin(customerApprovals, ePin));
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, customerApprovals });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }


        private IList<TechnicalSpecialistCustomerApprovalInfo> SetEPin(IList<TechnicalSpecialistCustomerApprovalInfo> CustomerApproval, int ePin)
        {
            CustomerApproval = CustomerApproval?.Select(x =>
            {
                x.Epin = ePin;
                return x;
            }).ToList();

            return CustomerApproval;
        }
        private void AssignValues(IList<TechnicalSpecialistCustomerApprovalInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }


    }
}