using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Api.Controllers.Company
{
    /// <summary>
    /// This endpoint will perform CRUD functioanlity for Company 
    /// </summary>
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyservice;
        private readonly IAppLogger<CompanyController> _logger;
        public CompanyController(ICompanyService service, IAppLogger<CompanyController> logger)
        {
            this._companyservice = service;
            this._logger=logger;
        }

      
        [HttpGet]
       // [ResponseCache(Duration = 3, VaryByQueryKeys = new string[] { "IsActive" })]
        public Response Get([FromQuery]Model.CompanySearch model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 return Task.Run<Response>(async () => await this._companyservice.GetCompanyAsync(model)).Result; 
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
                       
        }

        [HttpGet]
        [Route("GetCompanyList")]
        public Response GetCompanyList([FromQuery]Model.CompanySearch model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _companyservice.GetCompanyList(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }

        [HttpPost]
        public Response Post([FromBody]IList<Model.Company> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                  AssignValues(model, ValidationType.Add);
                  return _companyservice.SaveCompany(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);               
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);           
        }

  
        [HttpPut]
        public Response Put([FromBody]IList<Model.Company> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {                  
                AssignValues(model, ValidationType.Update);
                return _companyservice.ModifyCompany(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                ////Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);               
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<Model.Company> company, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(company, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(company, "ModifiedBy", UserName);
        }
    }
}