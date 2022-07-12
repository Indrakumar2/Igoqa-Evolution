using Evolution.Common.Models.Responses;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Company
{
    [Route("api/companies/{companyCode}/qualifications")]
    [ApiController]
    public class CompanyQualificationController : ControllerBase
    {
        private readonly ICompanyQualificationService _companyQualificationService;
        private readonly IAppLogger<CompanyQualificationController> _logger;
        public CompanyQualificationController(ICompanyQualificationService service, IAppLogger<CompanyQualificationController> logger)
        {
            this._companyQualificationService = service;
            this._logger = logger;
        }

       
        [HttpGet]
        public Response Get([FromRoute] string companyCode, [FromQuery]CompanyQualification model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model.CompanyCode = companyCode;
                return this._companyQualificationService.GetCompanyQualification(model);
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
      
    }
}