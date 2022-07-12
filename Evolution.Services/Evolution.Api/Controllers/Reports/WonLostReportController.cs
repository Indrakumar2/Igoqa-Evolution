using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Models.Responses;
using Evolution.Reports.Domain.Interfaces.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Api.Controllers.Reports
{
    [Route("api/reports")]
    [ApiController]
    public class WonLostController : BaseController
    {
        private IWonLostService _wonLostReportService = null;

        public WonLostController(IWonLostService customerApprovalService)
        {
            _wonLostReportService = customerApprovalService;
        }

        [HttpGet]
        [Route("wonLost")]
        public Response Get([FromQuery] DomainModel.WonLost searchModel)
        {
            return _wonLostReportService.Get(searchModel);
        }
    }
}