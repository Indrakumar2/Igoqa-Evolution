using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Customer.Domain.Models.Customers;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Evolution.Api.Controllers.Customer
{
    [Route("api/customers/{customerCode}/addresses")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressService _service = null;
        private readonly IAppLogger<CustomerAddressController> _logger = null;

        public CustomerAddressController(ICustomerAddressService service, IAppLogger<CustomerAddressController> logger)
        {
            this._service = service;
            _logger = logger;
        }

        // GET api/customers/A00004/Address
        /// <summary>
        ///  Get list of Customer address based on different search conditions
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">Customer address model with filter conditions.</param>
        /// <returns>List of Customer addresses.</returns>
        [HttpGet]
        public Response Get([FromRoute] string customerCode, [FromQuery] CustomerAddress model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model.CustomerCode = customerCode;
                return this._service.GetCustomerAddress(model);
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

        [Route("GetAddress")]
        [HttpGet]
        public Response Get([FromRoute] string customerCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.GetCustomerAddress(customerCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        // POST api/customers/A00004/Address
        /// <summary>
        /// Add new customer address
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">List of new customer address data that need to be added.</param>
        [HttpPost]
        public Response Post([FromRoute]string customerCode, [FromBody] IList<CustomerAddress> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.SaveCustomerAddress(customerCode, model);
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

        // PUT api/customers/A00004/Address
        /// <summary>
        /// modify existing customer address
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer address data that need to be modified.</param>
        [HttpPut]
        public Response Put([FromRoute]string customerCode, [FromBody] IList<CustomerAddress> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.ModifyCustomerAddress(customerCode, model);
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

        // DELETE api/customers/A00004/Address
        /// <summary>
        ///  Delete existing customer address
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer address data that need to be deleted.</param>
        [HttpDelete]
        public Response Delete([FromRoute]string customerCode, [FromBody] IList<CustomerAddress> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.DeleteCustomerAddress(customerCode, model);
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