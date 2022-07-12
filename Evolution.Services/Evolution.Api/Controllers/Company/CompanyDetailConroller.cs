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
using Model = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Api.Controllers.Company
{
    [Route("api/companies/detail")]
    public class CompanyDetailConroller : BaseController
    {
        private readonly ICompanyDetailService _service;
        private readonly IAppLogger<CompanyDetailConroller> _logger;

        public CompanyDetailConroller(ICompanyDetailService service, IAppLogger<CompanyDetailConroller> logger)
        {
            this._service = service;
            this._logger=logger;
        }

        [HttpPost]
        public Response Post([FromBody]Model.CompanyDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                  AssignValues(model, ValidationType.Add);
                  return _service.SaveCompanyDetail(new List<CompanyDetail>() { model });
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
        public Response Put([FromBody]Model.CompanyDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 AssignValues(model, ValidationType.Update);
                 return _service.SaveCompanyDetail(new List<CompanyDetail>() { model });
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

        private void AssignValues(Model.CompanyDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.CompanyInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyDivisionCostCenters, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyDivisions, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyDocuments, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyEmailTemplates, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyExpectedMargins, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyInvoiceInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyNotes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyOffices, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyPayrollPeriods, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyPayrolls, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyQualifications, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.CompanyTaxes, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyDivisionCostCenters, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyDivisions, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyDocuments, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyEmailTemplates, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyExpectedMargins, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyInvoiceInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyNotes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyOffices, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyPayrollPeriods, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyPayrolls, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyQualifications, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.CompanyTaxes, "ModifiedBy", UserName);
            }
        }
    }
}
