using Evolution.Api.Controllers.Base;
using Evolution.Common.Models.Responses;
using Evolution.Reports.Domain.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Reports.Domain.Models.Reports;
namespace Evolution.Api.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxonomyReportController : BaseController
    {
        private ITaxonomyService _taxonomyService = null;

        public TaxonomyReportController(ITaxonomyService taxonomyService)
        {
            _taxonomyService = taxonomyService;
        }
        [HttpPost]
        public Response TaxonomyReport(DomainModel.Taxonomy searchModel)
        {
            return _taxonomyService.Get(searchModel);
        }
    }
}