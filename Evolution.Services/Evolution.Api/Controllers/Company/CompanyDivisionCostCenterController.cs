using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using DomainModel= Evolution.Company.Domain.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Api.Controllers.Base;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Company
{
    [Route("api/companies/{companyCode}/divisions/{divisionName}/costcenters")]
    [ApiController]
    public class CompanyDivisionCostCenterController : BaseController
    {
        ICompanyDivisionCostCenterService _service;
        private readonly IAppLogger<CompanyDivisionCostCenterController> _logger;

        public CompanyDivisionCostCenterController(ICompanyDivisionCostCenterService service, IAppLogger<CompanyDivisionCostCenterController> logger)
        {
            this._service = service;
            this._logger=logger;
        }
              
        [HttpGet]
        public Response Get([FromRoute]string companyCode, [FromQuery]string division, [FromQuery]DomainModel.CompanyDivisionCostCenter searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                    searchModel.CompanyCode = companyCode;
                    searchModel.Division = division;
                    return this._service.GetCompanyCostCenter(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
               // //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);            
        }
        
        [HttpPost]
        public Response Post([FromRoute]string companyCode, [FromRoute]string divisionName, [FromBody]IList<DomainModel.CompanyDivisionCostCenter> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                    AssignValues(model, ValidationType.Add);
                    return _service.SaveCompanyCostCenter(companyCode, divisionName, model);
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
        public Response Put([FromRoute]string companyCode, [FromRoute]string divisionName, [FromBody]IList<DomainModel.CompanyDivisionCostCenter> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                    AssignValues(model, ValidationType.Update);
                    return _service.ModifyCompanyCostCenter(companyCode, divisionName, model);
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

        /// <summary>
        /// Delete the Cost Center
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="divisionName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response Delete([FromRoute]string companyCode, [FromRoute]string divisionName, [FromBody]IList<DomainModel.CompanyDivisionCostCenter> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                    AssignValues(model, ValidationType.Delete);
                    return _service.DeleteCompanyCostCenter(companyCode, divisionName, model);
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


        private void AssignValues(IList<DomainModel.CompanyDivisionCostCenter> companyDivisionCostCenter, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyDivisionCostCenter, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyDivisionCostCenter, "ModifiedBy", UserName);
        }
    }
}