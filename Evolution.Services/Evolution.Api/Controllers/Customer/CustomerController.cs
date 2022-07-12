using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Api.Controllers.Customer
{
    /// <summary>
    /// This endpoint is going to take care of CRUD functioanlity for Customer only
    /// </summary>
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService = null;
        private readonly IAppLogger<CustomerController> _logger = null;

        public CustomerController(ICustomerService customerService, IAppLogger<CustomerController> logger)
        {
            this._customerService = customerService;
            _logger = logger;
        }

        // GET api/customer
        /// <summary>
        /// Get list of Customer based on different search conditions
        /// </summary>
        /// <param name="model">Customer model with filter conditions.</param>
        /// <returns>List of Customers.</returns>
        [HttpGet]
        public Response Get([FromQuery] Domain.CustomerSearch model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return Task.Run<Response>(async () => await this._customerService.GetCustomerAsync(model)).Result;
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

        ///GET api/visitTimesheetCustomers
        /// <summary>
        /// Get list of Customer based on Logged in user and Approved or UnApproved Visit and Timesheet.
        /// </summary>
        /// <param name="ContractHolderCompanyId"></param>
        /// <param name="isVisit"></param>
        /// <param name="isNDT"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("visitTimesheetCustomers")]
        public Response GetApprovedVistCustomers(int ContractHolderCompanyId, bool isVisit, bool isNDT)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.GetApprovedVistCustomers(ContractHolderCompanyId, isVisit, isNDT);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ContractHolderCompanyId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("visitTimesheetUnapprovedCustomers")]
        public Response GetUnApprovedVistCustomers(int ContractHolderCompanyId, int? CoordinatorId, bool isVisit, bool isOperating)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.GetUnApprovedVisitCustomers(ContractHolderCompanyId, CoordinatorId, isVisit, isOperating);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ContractHolderCompanyId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("visitTimesheetKPICustomers")]
        public Response GetVisitTimesheetKPICustomers(int ContractHolderCompanyId, bool isVisit, bool isOperating)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.GetVisitTimesheetKPICustomers(ContractHolderCompanyId, isVisit, isOperating);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ContractHolderCompanyId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("customerCoordinator")]
        public Response GetCustomers([FromQuery] Domain.CustomerSearch model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.GetCustomerBasedOnCoordinator(model);
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

        // POST api/customer
        /// <summary>
        /// Add new Customers
        /// </summary>
        /// <param name="model">List of new customer data that need to be added.</param>
        [HttpPost]
        public Response Post([FromBody] IList<Domain.Customer> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.SaveCustomer(model);
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

        // PUT api/customer 
        /// <summary>
        /// Update an existing customer
        /// </summary>
        /// <param name="model">List of customer data that need to be modified.</param>
        [HttpPut]
        public Response Put([FromBody] IList<Domain.Customer> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._customerService.ModifyCustomer(model);
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
