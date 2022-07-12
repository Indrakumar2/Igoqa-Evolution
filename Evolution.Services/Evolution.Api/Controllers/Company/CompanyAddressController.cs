using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Company
{
    [Route("api/companies/{companyCode}/offices")]
    [ApiController]
    public class CompanyAddressController : BaseController
    {
        private readonly ICompanyOfficeService _companyOfficeservice;
        private readonly IAppLogger<CompanyAddressController> _logger;
        public CompanyAddressController(ICompanyOfficeService service, IAppLogger<CompanyAddressController> logger)
        {
            this._companyOfficeservice = service;
            this._logger = logger;
        }

     
        [HttpGet]
        public Response Get([FromRoute] string companyCode, [FromQuery] CompanyAddress model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 model.CompanyCode = companyCode;
                 return this._companyOfficeservice.GetCompanyAddress(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
           
        }

        [HttpPost]
        public Response Post([FromRoute]string companyCode, [FromBody]IList<CompanyAddress> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                  AssignValues(model, ValidationType.Add);
                  return _companyOfficeservice.SaveCompanyAddress(companyCode, model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
               // //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
           
        }

        [HttpPut]
        public Response Put([FromRoute]string companyCode, [FromBody]IList<CompanyAddress> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                  AssignValues(model, ValidationType.Update);
                  return _companyOfficeservice.ModifyCompanyAddress(companyCode, model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);            
        }

        [HttpDelete]
        public Response Delete([FromRoute]string companyCode, [FromBody]IList<CompanyAddress> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(model, ValidationType.Delete);
                 return _companyOfficeservice.DeleteCompanyAddress(companyCode, model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
               // //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);           
        }
        private void AssignValues(IList<CompanyAddress> companyAddress, ValidationType validationType)
        {        
            ObjectExtension.SetPropertyValue(companyAddress, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyAddress, "ModifiedBy", UserName);
        }
    }
}