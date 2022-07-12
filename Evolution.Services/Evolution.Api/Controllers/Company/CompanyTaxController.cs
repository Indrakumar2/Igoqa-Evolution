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
    [Route("api/companies/{companyCode}/taxes")]
    [ApiController]
    public class CompanyTaxController : BaseController
    {
        private readonly ICompanyTaxService _companyTaxService = null;
        private readonly IAppLogger<CompanyTaxController> _logger = null;
        public CompanyTaxController(ICompanyTaxService service, IAppLogger<CompanyTaxController> logger)
        {
            this._companyTaxService = service;
            this._logger = logger;
        }
        
        [HttpGet]
        public Response Get([FromRoute] string companyCode, [FromQuery]CompanyTax searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.CompanyCode = companyCode;
                return this._companyTaxService.GetCompanyTax(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }

        [HttpPost]
        public Response Post([FromRoute]string companyCode, [FromBody]IList<CompanyTax> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(model, ValidationType.Add);
                 return _companyTaxService.SaveCompanyTax(companyCode, model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);            
        }

        [HttpPut]
        public Response Put([FromRoute]string companyCode, [FromBody]IList<CompanyTax> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(model, ValidationType.Update);
                 return _companyTaxService.ModifyCompanyTax(companyCode, model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);            
        }

        [HttpDelete]
        public Response Delete([FromRoute]string companyCode, [FromBody]IList<CompanyTax> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {  
                AssignValues(model, ValidationType.Delete);
                return _companyTaxService.DeleteCompanyTax(companyCode, model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
        }
        private void AssignValues(IList<CompanyTax> companyTax, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyTax, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyTax, "ModifiedBy", UserName);
        }
    }
}