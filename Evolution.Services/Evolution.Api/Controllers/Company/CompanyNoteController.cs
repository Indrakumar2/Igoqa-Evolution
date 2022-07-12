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
    [Route("api/companies/{companyCode}/notes")]
    [ApiController]
    public class CompanyNoteController : BaseController
    {
        private readonly ICompanyNoteService _service;
        private readonly IAppLogger<CompanyNoteController> _logger;
        public CompanyNoteController(ICompanyNoteService service, IAppLogger<CompanyNoteController> logger)
        {
            this._service = service;
            this._logger = logger;
        }
       
        [HttpGet]
        public Response Get([FromRoute]string companyCode,[FromQuery]CompanyNote model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               model.CompanyCode = companyCode;
               return this._service.GetCompanyNote(model);
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
        public Response Post([FromRoute]string companyCode, [FromBody]IList<CompanyNote> model, bool commitChange)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               AssignValues(model, ValidationType.Add);
               return this._service.SaveCompanyNote(companyCode, model,true);
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
        public Response Put([FromRoute]string companyCode, [FromBody]IList<CompanyNote> model,bool commitChange)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return this._service.ModifyCompanyNote(companyCode, model,true);
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

        public Response Delete([FromRoute]string companyCode, [FromBody]IList<CompanyNote> model,bool commitChange)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return this._service.DeleteCompanyNote(companyCode, model,true);
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

        private void AssignValues(IList<CompanyNote> companyNote, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyNote, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyNote, "ModifiedBy", UserName);
        }
    }
}