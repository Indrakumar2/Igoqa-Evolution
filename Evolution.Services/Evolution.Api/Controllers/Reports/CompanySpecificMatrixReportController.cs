using Evolution.Api.Controllers.Base;
using Evolution.Common.Models.Responses;
using Evolution.Reports.Domain.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Evolution.Api.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySpecificMatrixReportController : BaseController
    {
        private ICompanySpecificMatrixService _companySpecificMatrixService = null;

        public CompanySpecificMatrixReportController(ICompanySpecificMatrixService companySpecificMatrixService)
        {
            _companySpecificMatrixService = companySpecificMatrixService;
        }

        [HttpPost]
        public Response CompanySpecificMatrix([FromBody] Dictionary<string, object> Params)
        {
            object companyCode = null;
            object isByResource = null;
            if (Params.TryGetValue("companyCode", out companyCode) && Params.TryGetValue("isByResource", out isByResource))
            {
                string[] companyID = companyCode.ToString().Split(',');
                if (Convert.ToBoolean(isByResource))
                    return _companySpecificMatrixService.GetByResource(companyID);
                else
                    return _companySpecificMatrixService.GetByTaxonomyService(companyID);
            }
            else
              return new Response("500", "Invalid Params", null);
        }

        [HttpPost]
        [Route("Export")]
        public object Export([FromBody] Dictionary<string, object> Params)
        {
            object companyCode = null;
            if (Params.TryGetValue("companyCode", out companyCode))
            {
                string[] companyID = companyCode.ToString().Split(',');
                return _companySpecificMatrixService.ExportReport(companyID);
            }
            else
                return new { message = "Invalid Params", code = 500 };
        }
    }
}
