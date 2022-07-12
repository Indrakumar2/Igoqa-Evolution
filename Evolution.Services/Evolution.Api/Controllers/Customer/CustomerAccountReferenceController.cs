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
    [Route("api/customers/{customerCode}/accountreferences")]
    [ApiController]
    public class CustomerAccountReferenceController : ControllerBase
    {
        private readonly ICustomerAccountReferenceService _customerAccountReferenceService = null;
        private readonly IAppLogger<CustomerAccountReferenceController> _logger = null;

        public CustomerAccountReferenceController(ICustomerAccountReferenceService customerAccountReferenceService, IAppLogger<CustomerAccountReferenceController> logger)
        {
            this._customerAccountReferenceService = customerAccountReferenceService;
            _logger = logger;
        }

        // GET api/customers/A00004/accountreference
        /// <summary>
        ///  Get list of Customer account reference based on different search conditions
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">Customer account reference model with filter conditions.</param>
        /// <returns>List of Customer account references.</returns>
        [HttpGet]
        public Response Get([FromRoute]string customerCode, [FromQuery] CustomerCompanyAccountReference model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAccountReferenceService.GetCustomerAccountReference(customerCode, model);
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

        // POST api/customers/A00004/accountreference
        /// <summary>
        /// Add new customer account reference
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">List of new customer account reference data that need to be added.</param>
        [HttpPost]
        public Response Post([FromRoute]string customerCode, [FromBody] IList<CustomerCompanyAccountReference> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAccountReferenceService.SaveCustomerAccountReference(customerCode, model);
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

        // PUT api/customers/A00004/accountreference
        /// <summary>
        /// modify existing customer account reference
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer account reference data that need to be modified.</param>
        [HttpPut]
        public Response Put([FromRoute]string customerCode, [FromBody] IList<CustomerCompanyAccountReference> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAccountReferenceService.ModifyCustomerAccountReference(customerCode, model);
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

        // DELETE api/customers/A00004/accountreference
        /// <summary>
        ///  Delete existing customer account reference
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer account reference data that need to be deleted.</param>
        [HttpDelete]
        public Response Delete([FromRoute]string customerCode, [FromBody] IList<CustomerCompanyAccountReference> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAccountReferenceService.DeleteCustomerAccountReference(customerCode, model);
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