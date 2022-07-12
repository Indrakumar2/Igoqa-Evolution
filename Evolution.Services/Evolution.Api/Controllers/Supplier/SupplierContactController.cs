using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.SupplierContacts.Domain.Interfaces.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Domain = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Api.Controllers.SupplierContact
{
    [Route("api/suppliers/{supplierNumber}/contacts")]
    [ApiController]

    public class SupplierContactController : BaseController
    {
        private readonly ISupplierContactService _supplierContactService = null;
        private readonly IAppLogger<SupplierContactController> _logger = null;

        public SupplierContactController(ISupplierContactService supplierContactService, IAppLogger<SupplierContactController> logger)
        {
            this._supplierContactService = supplierContactService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]int supplierNumber, [FromQuery]Domain.SupplierContact searchModel, [FromQuery] AdditionalFilter additionalFilter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.SupplierId = supplierNumber;
            return this._supplierContactService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute]string supplierNumber, [FromBody]IList<Domain.SupplierContact> supplierContacts)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierContacts,ValidationType.Add);
            return this._supplierContactService.Add(supplierContacts);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute]string supplierNumber, [FromBody]IList<Domain.SupplierContact> supplierContacts)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierContacts,ValidationType.Update);
            return this._supplierContactService.Modify(supplierContacts);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute]string supplierNumber, [FromBody]IList<Domain.SupplierContact> supplierContacts)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierContacts,ValidationType.Delete);
            return this._supplierContactService.Delete(supplierContacts);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<Domain.SupplierContact> supplierContacts,ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(supplierContacts, "ActionByUser", UserName);
            if(validationType!=ValidationType.Add)
            ObjectExtension.SetPropertyValue(supplierContacts, "ModifiedBy", UserName);
        }
    }
}