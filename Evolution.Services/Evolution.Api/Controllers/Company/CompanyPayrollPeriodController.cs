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
    [Route("api/companies/{companyCode}/payrolls/{payrollName}/periods")]
    [ApiController]
    public class CompanyPayrollPeriodController : BaseController
    {
        private readonly ICompanyPayrollPeriodService _companypayrollPeriodService;
        private readonly IAppLogger<CompanyPayrollPeriodController> _logger;

        public CompanyPayrollPeriodController(ICompanyPayrollPeriodService service, IAppLogger<CompanyPayrollPeriodController> logger)
        {
            this._companypayrollPeriodService = service;
            this._logger=logger;
        }

        [HttpGet]
        public Response Get([FromRoute] string companyCode, [FromRoute]string payrollName, [FromQuery]CompanyPayrollPeriod model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model.CompanyCode = companyCode;
                model.PayrollType = payrollName; 
                return this._companypayrollPeriodService.GetCompanyPayrollPeriod(model);
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
        public Response Post([FromRoute]string companyCode, [FromRoute]string payrollName,[FromRoute]int? payrollId, [FromBody]IList<CompanyPayrollPeriod> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return _companypayrollPeriodService.SaveCompanyPayrollPeriod(companyCode, payrollName, payrollId, model);
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
        public Response Put([FromRoute]string companyCode, [FromRoute]string payrollName, [FromRoute]int? payrollId, [FromBody]IList<CompanyPayrollPeriod> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return _companypayrollPeriodService.ModifyCompanyPayrollPeriod(companyCode, payrollName, payrollId, model);
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
        public Response Delete([FromRoute]string companyCode, [FromRoute]string payrollName, [FromRoute]int? payrollId, [FromBody]IList<CompanyPayrollPeriod> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(model, ValidationType.Delete);
                 return _companypayrollPeriodService.DeleteCompanyPayrollPeriod(companyCode, payrollName, payrollId, model); 
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
        private void AssignValues(IList<CompanyPayrollPeriod> companyPayrollPeroid, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyPayrollPeroid, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyPayrollPeroid, "ModifiedBy", UserName);
        }
    }
}