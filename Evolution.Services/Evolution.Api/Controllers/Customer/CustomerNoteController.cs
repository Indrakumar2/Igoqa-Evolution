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
    [Route("api/customers/{customerCode}/notes")]
    [ApiController]
    public class CustomerNoteController : ControllerBase
    {

        private readonly ICustomerNoteService _customerNoteService = null;
        private readonly IAppLogger<CustomerNoteController> _logger = null;

        public CustomerNoteController(ICustomerNoteService customerNoteService, IAppLogger<CustomerNoteController> logger)
        {
            this._customerNoteService = customerNoteService;
            _logger = logger;
        }

        // GET api/customers/A00004/note
        /// <summary>
        ///  Get list of Customer note based on different search conditions
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">Customer note model with filter conditions.</param>
        /// <returns>List of Customer notes.</returns>
        [HttpGet]
        public Response Get([FromRoute]string customerCode, [FromQuery] CustomerNote model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerNoteService.GetCustomerNote(customerCode, model);
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

        // POST api/customers/A00004/note
        /// <summary>
        /// Add new customer note
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">List of new customer note data that need to be added.</param>
        [HttpPost]
        public Response Post([FromRoute]string customerCode, [FromBody] IList<CustomerNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerNoteService.SaveCustomerNote(customerCode, model);
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
        // PUT api/customers/A00004/note
        /// <summary>
        /// modify existing customer note
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer note data that need to be modified.</param>
        [HttpPut]
       public Response Put([FromRoute]string customerCode, [FromBody] IList<CustomerNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerNoteService.ModifyCustomerNote(customerCode, model);
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

        // DELETE api/customers/A00004/note
        /// <summary>
        ///  Delete existing customer note
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer note data that need to be deleted.</param>
        [HttpDelete]
        public Response Delete([FromRoute]string customerCode, [FromBody] IList<CustomerNote> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerNoteService.DeleteCustomerNote(customerCode, model);
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