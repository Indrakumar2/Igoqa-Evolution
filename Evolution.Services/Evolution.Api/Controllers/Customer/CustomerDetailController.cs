using Evolution.Api.Controllers.Base;
using Evolution.Api.Filters;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Customers;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Api.Controllers.Customer
{
    [Route("api/customers/detail")]
    [ApiController]
    public class CustomerDetailController : BaseController
    {
        private readonly ICustomerDetailService _customerDetailService = null;
        private readonly IAppLogger<CustomerDetailController> _logger = null;

        public CustomerDetailController(ICustomerDetailService customerDetailService, IAppLogger<CustomerDetailController> logger)
        {
          
            this._customerDetailService = customerDetailService;
            _logger = logger;
        }

        /// <summary>
        /// Add new customer details
        /// </summary>
        /// <param name="customerDetailModels">List of customer details</param>
        /// <returns> Returns Response with Code,Messages and Results list. </returns>
        [HttpPost]
        [AuthorisationFilter(SecurityModule.Customer, SecurityPermission.V00001,SecurityPermission.M00001)]
        public Response Post([FromBody] IList<Domain.CustomerDetail> customerDetailModels)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(customerDetailModels?.FirstOrDefault(), ValidationType.Add);
            return this._customerDetailService.SaveCustomerDetails(customerDetailModels);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDetailModels);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        /// <summary>
        /// update customer details
        /// </summary>
        /// <param name="customerDetailModels">List of customer details</param>
        /// <returns> Returns Response with Code,Messages and Results list. </returns>
        [HttpPut]
        [AuthorisationFilter(SecurityModule.Customer, SecurityPermission.V00001, SecurityPermission.M00001)]
        public Response Put([FromBody] IList<Domain.CustomerDetail> customerDetailModels)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(customerDetailModels?.FirstOrDefault(), ValidationType.Update);
            return this._customerDetailService.SaveCustomerDetails(customerDetailModels);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), customerDetailModels);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(Domain.CustomerDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                model.ActionByUser = UserName;
                ObjectExtension.SetPropertyValue(model.Detail, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.AccountReferences, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.AssignmentReferences, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.Documents, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.Notes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.Addresses, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.ActionByUser, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.Detail, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.AccountReferences, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                {
                    ObjectExtension.SetPropertyValue(model.Addresses, "ModifiedBy", UserName);
                    if (model.Addresses != null)//D801 - Added to avoid for no addresses
                    {
                        model.Addresses.ToList().ForEach(x =>
                        {
                            ObjectExtension.SetPropertyValue(x.Contacts, "ModifiedBy", UserName);
                        });
                    }
                }
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.AssignmentReferences, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.Documents, "ModifiedBy", UserName);
            }
          
        }

    }
}