using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Domain = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Api.Controllers.Customer
{
    /// <summary>
    /// This endpoint is responsible for  iConnect Integration
    /// </summary>
    [Route("api/iconnect")]
    [ApiController]
    public class CustomerIConnectController : Controller
    {
        private readonly ICustomerService _customerService = null;
        private readonly IAppLogger<CustomerIConnectController> _logger = null;

        public CustomerIConnectController(ICustomerService customerService, IAppLogger<CustomerIConnectController> logger)
        {
            this._customerService = customerService;
            _logger = logger;
        }

        // POST api/iconnect
        /// <summary>
        /// Amend Customers
        /// </summary>
        /// <param name="model">List of customer data that needs to be amended.</param>
        [HttpPost]
        public Response Post([FromBody] IList<Domain.Customer> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.IConnectIntegration(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

    }
}
