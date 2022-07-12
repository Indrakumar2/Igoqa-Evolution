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
    [Route("api/companies/{companyCode}/invoicedetail")]
    [ApiController]
    public class CompanyInvoiceController : BaseController
    {
        private readonly ICompanyInvoiceService _companyInvoiceService;
        private readonly IAppLogger<CompanyInvoiceController> _logger;

        public CompanyInvoiceController(ICompanyInvoiceService service, IAppLogger<CompanyInvoiceController> logger)
        {
            _companyInvoiceService = service;
            _logger=logger;
        }
        
        [HttpGet]
        public Response Get([FromRoute]string companyCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               return this._companyInvoiceService.GetCompanyInvoice(companyCode);
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
        public Response Post([FromRoute]string companyCode, [FromBody]CompanyInvoice companyInvoices,List<DbModels.CompanyMessage> msgToInsert,
                             List<DbModels.CompanyMessage> msgToUpdate,List<DbModels.CompanyMessage> msgToDelete)
       {
           Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               AssignValues(companyInvoices, ValidationType.Add);
            return this._companyInvoiceService.AddCompanyInvoice(companyCode,companyInvoices,ref msgToInsert,ref msgToUpdate,ref msgToDelete);
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
        public Response Update([FromRoute]string companyCode, [FromBody]CompanyInvoice companyInvoices,List<DbModels.CompanyMessage> msgToInsert,
                             List<DbModels.CompanyMessage> msgToUpdate,List<DbModels.CompanyMessage> msgToDelete)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
               AssignValues(companyInvoices, ValidationType.Update);
               return this._companyInvoiceService.ModifyCompanyInvoice(companyCode, companyInvoices,ref msgToInsert,ref msgToUpdate,ref msgToDelete);
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

        private void AssignValues(CompanyInvoice companyInvoice, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(companyInvoice, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(companyInvoice, "ModifiedBy", UserName);
        }
    }
}