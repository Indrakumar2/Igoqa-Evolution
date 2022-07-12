using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Company.Core.Services;
using Evolution.Company.Domain.Interfaces.Companies;
using DomainModel= Evolution.Company.Domain.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Api.Controllers.Base;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Company
{
    [Route("api/companies/{companyCode}/divisions")]
    [ApiController]
    public class CompanyDivisionController : BaseController
    {
        ICompanyDivisionService _service;
        private readonly IAppLogger<CompanyDivisionController> _logger;

        public CompanyDivisionController(ICompanyDivisionService service , IAppLogger<CompanyDivisionController> logger)
        {
            this._service = service;
            this._logger=logger;
        }
        
        [HttpGet]
        public Response Get([FromRoute]string companyCode, [FromQuery]DomainModel.CompanyDivision model)
        {            
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                   model.CompanyCode = companyCode;
                   return _service.GetCompanyDivision(model);
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
        public Response Post([FromRoute]string companyCode, [FromBody]IList<DomainModel.CompanyDivision> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                   AssignValues(model, ValidationType.Add);
                   return _service.SaveCompanyDivision(companyCode, model);
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
        public Response Put([FromRoute]string companyCode, [FromBody]IList<DomainModel.CompanyDivision> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(model, ValidationType.Update);
                 return _service.ModifyCompanyDivision(companyCode, model);
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
        public Response Delete([FromRoute]string companyCode, [FromBody]IList<DomainModel.CompanyDivision> model)
            
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                  AssignValues(model, ValidationType.Delete);
                  return _service.DeleteCompanyDivision(companyCode, model);
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

        private void AssignValues(IList<DomainModel.CompanyDivision> CompanyDivisions, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(CompanyDivisions, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(CompanyDivisions, "ModifiedBy", UserName);
        }
    }
}