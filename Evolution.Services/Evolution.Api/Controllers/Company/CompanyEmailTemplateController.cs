using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DbModels=Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Company
{
    [Route("api/companies/{companyCode}/email/template")]
    [ApiController]
    public class CompanyEmailTemplateController : BaseController
    {

        private readonly ICompanyEmailTemplateService _companyEmailTemplateService;
        private readonly IAppLogger<CompanyEmailTemplateController> _logger;
        public CompanyEmailTemplateController(ICompanyEmailTemplateService service, IAppLogger<CompanyEmailTemplateController> logger)
        {
            _companyEmailTemplateService = service;
            _logger=logger;
        }

        [HttpGet]
        public Response Get([FromRoute]string companyCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._companyEmailTemplateService.GetCompanyEmailTemplate(companyCode);
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
        public Response Post([FromRoute]string companyCode, [FromBody]CompanyEmailTemplate companyEmailTemplate)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(companyEmailTemplate, ValidationType.Add);
                IList<DbModels.CompanyMessage> msgToBeInsert=null; IList<DbModels.CompanyMessage> msgToBeUpdate=null; IList<DbModels.CompanyMessage> msgToBeDelete=null;
                return this._companyEmailTemplateService.AddCompanyEmailTemplate(companyCode, companyEmailTemplate,ref msgToBeInsert,ref msgToBeUpdate,ref msgToBeDelete);
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
        public Response Update([FromRoute]string companyCode, [FromBody]CompanyEmailTemplate companyEmailTemplate)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(companyEmailTemplate, ValidationType.Update);
                 IList<DbModels.CompanyMessage> msgToBeInsert=null; IList<DbModels.CompanyMessage> msgToBeUpdate=null; IList<DbModels.CompanyMessage> msgToBeDelete=null;
                 return this._companyEmailTemplateService.ModifyCompanyEmailTemplate(companyCode, companyEmailTemplate,ref msgToBeInsert,ref msgToBeUpdate,ref msgToBeDelete);
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

        private void AssignValues(CompanyEmailTemplate companyEmailTemplate, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyEmailTemplate, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyEmailTemplate, "ModifiedBy", UserName);
        }
    }
}