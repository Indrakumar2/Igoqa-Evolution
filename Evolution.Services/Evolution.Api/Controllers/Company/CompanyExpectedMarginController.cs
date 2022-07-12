﻿using Evolution.Api.Controllers.Base;
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
    [Route("api/companies/{companyCode}/expectedMargins")]
    [ApiController]
    public class CompanyExpectedMarginController : BaseController
    {
        ICompanyExpectedMarginService _service;
        private readonly IAppLogger<CompanyExpectedMarginController> _logger;
        public CompanyExpectedMarginController(ICompanyExpectedMarginService service, IAppLogger<CompanyExpectedMarginController> logger)
        {
            this._service = service;
            this._logger=logger;
        }
        
        [HttpGet]
        public Response Get([FromRoute] string companyCode, [FromQuery]CompanyExpectedMargin model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               model.CompanyCode = companyCode;
               return _service.GetCompanyExpectedMargin(model);
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
        public Response Post([FromRoute]string companyCode, [FromBody]IList<CompanyExpectedMargin> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return _service.SaveCompanyExpectedMargin(companyCode, model);
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
                
        [HttpPut]
        public Response Put([FromRoute]string companyCode, [FromBody]IList<CompanyExpectedMargin> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return _service.ModifyCompanyExpectedMargin(companyCode, model);
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
        public Response Delete([FromRoute]string companyCode, [FromBody]IList<CompanyExpectedMargin> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return _service.DeleteCompanyExpectedMargin(companyCode, model);
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

        private void AssignValues(IList<CompanyExpectedMargin> companyExpectedMargin, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyExpectedMargin, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyExpectedMargin, "ModifiedBy", UserName);
        }
    }
}