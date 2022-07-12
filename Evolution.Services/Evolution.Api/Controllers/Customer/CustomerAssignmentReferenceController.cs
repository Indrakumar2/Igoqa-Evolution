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
    [Route("api/customers/{customerCode}/assignmentreferences")]
    [ApiController]
    public class CustomerAssignmentReferenceController : ControllerBase
    {
        private readonly ICustomerAssignmentReferenceService _customerAssignmentReferenceService = null;
        private readonly IAppLogger<CustomerAssignmentReferenceController> _logger = null;

        public CustomerAssignmentReferenceController(ICustomerAssignmentReferenceService customerAssignmentReferenceService, IAppLogger<CustomerAssignmentReferenceController> logger)
        {
            this._customerAssignmentReferenceService = customerAssignmentReferenceService;
            _logger = logger;
        }

        // GET api/customers/A00004/assignmentreference
        ///<summary>
        ///  Get list of Customer assignment reference based on different search conditions
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">Customer assignment reference model with filter conditions.</param>
        /// <returns>List of Customer assignment references.</returns>
        [HttpGet]
        public Response Get([FromRoute]string customerCode, [FromQuery] CustomerAssignmentReference model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAssignmentReferenceService.GetCustomerAssignmentReference(customerCode, model);
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

        // POST api/customers/A00004/assignmentreference
        /// <summary>
        /// Add new customer assignment reference
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model">List of new customer assignment reference data that need to be added.</param>
        [HttpPost]
        public Response Post([FromRoute]string customerCode, [FromBody] IList<CustomerAssignmentReference> model)
        {

            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAssignmentReferenceService.SaveCustomerAssignmentReference(customerCode, model);
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

        // PUT api/customers/A00004/assignmentreference
        /// <summary>
        /// modify existing customer  assignment reference
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer assignment reference data that need to be modified.</param>
        [HttpPut]
        public Response Put([FromRoute]string customerCode, [FromBody] IList<CustomerAssignmentReference> model)
        {

            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAssignmentReferenceService.ModifyCustomerAssignmentReference(customerCode, model);
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

        // DELETE api/customers/A00004/assignmentreference
        /// <summary>
        ///  Delete existing customer assignment reference
        /// </summary>
        /// <param name="customerCode">Unique Customer Code</param>
        /// <param name="model"> List of customer assignment reference data that need to be deleted.</param>
        [HttpDelete]
        public Response Delete([FromRoute]string customerCode, [FromBody] IList<CustomerAssignmentReference> model)
        {

            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerAssignmentReferenceService.DeleteCustomerAssignmentReference(customerCode, model);
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