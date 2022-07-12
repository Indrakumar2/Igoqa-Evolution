using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Models.Responses;
using Evolution.Reports.Domain.Interfaces.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Api.Controllers.Reports
{
    [Route("api/reports")]
    [ApiController]
    public class CustomerApprovalController : BaseController
    {
        private ICustomerApprovalService _customerApprovalService = null;

        public CustomerApprovalController(ICustomerApprovalService customerApprovalService)
        {
            _customerApprovalService = customerApprovalService;
        }

        [HttpGet]
        [Route("customerApproval")]
        public Response Get([FromQuery] DomainModel.CustomerApproval searchModel)
        {
            return _customerApprovalService.Get(searchModel);
        }
    }
}