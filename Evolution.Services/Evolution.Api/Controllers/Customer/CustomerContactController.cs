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
    [Route("api/customers/{customerCode}")]
    [ApiController]
    public class CustomerContactController : ControllerBase
    {
        private readonly ICustomerContactService _customerContactService = null;
        private readonly IAppLogger<CustomerAssignmentReferenceController> _logger = null;

        public CustomerContactController(ICustomerContactService customerContactService, IAppLogger<CustomerAssignmentReferenceController> logger)
        {
            this._customerContactService = customerContactService;
            _logger = logger;
        }

        // GET api/customers/A00007/addresses/12/Contacts
        // GET api/customers/A00007/Contacts
        /// <summary>
        ///  Get list of Customer Contact based on different search conditions.
        ///  If address Id is null , will return all contacs belonging to a customer.
        /// </summary>
        /// <param name="customerCode">Customer code to which address and contact belongs.</param>
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="searchModel">Customer Contact model with filter conditions.</param>
        /// <returns>List of Customer Contacts.</returns>
        [Route("contacts")]
        [Route("addresses/{addressId}/contacts")]
        [HttpGet]
        public Response Get([FromRoute] string customerCode,[FromRoute]int? addressId, [FromQuery] Contact searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerContactService.GetCustomerContact(customerCode,addressId, searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [Route("GetContacts")]
        [HttpGet]
        public Response Get([FromRoute] string customerCode)
        {
            return this._customerContactService.GetCustomerContact(customerCode);
        }

        // POST api/customers/A00007/addresses/12/Contacts
        /// <summary>
        /// Add new customer Contact
        /// </summary>
        /// <param name="customerCode">Customer code to which address and contact belongs.</param>
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="contactModel">List of new customer Contact data that need to be added.</param>
        [Route("addresses/{addressId}/contacts")]
        [HttpPost]
        public Response Post([FromRoute] string customerCode,[FromRoute]int addressId, [FromBody] IList<Contact> contactModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerContactService.SaveCustomerContact(customerCode,addressId, contactModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contactModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        // PUT api/customers/A00007/addresses/12/Contacts
        /// <summary>
        /// modify existing customer Contact
        /// </summary>
        /// <param name="customerCode">Customer code to which address and contact belongs.</param>
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="contactModel"> List of customer Contact data that need to be modified.</param>
        [Route("addresses/{addressId}/contacts")]
        [HttpPut]
        public Response Put([FromRoute] string customerCode, [FromRoute]int addressId, [FromBody] IList<Contact> contactModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerContactService.ModifyCustomerContact(customerCode,addressId, contactModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contactModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        // DELETE api/customers/A00007/addresses/12/Contacts
        /// <summary>
        ///  Delete existing customer Contact
        /// </summary>
        /// <param name="customerCode">Customer code to which address and contact belongs.</param>
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="contactModel"> List of customer Contact data that need to be deleted.</param>
        [Route("addresses/{addressId}/contacts")]
        [HttpDelete]
        public Response Delete([FromRoute] string customerCode, [FromRoute]int addressId, [FromBody] IList<Contact> contactModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerContactService.DeleteCustomerContact(customerCode,addressId, contactModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contactModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}