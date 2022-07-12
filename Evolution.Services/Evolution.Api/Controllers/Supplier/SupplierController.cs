using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Api.Controllers.Supplier
{
    [Route("api/suppliers")]
    [ApiController]

    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService = null;
        private readonly IAppLogger<SupplierController> _logger = null;

        public SupplierController(ISupplierService supplierService, IAppLogger<SupplierController> logger)
        {
            this._supplierService = supplierService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]DomainModel.SupplierSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return Task.Run<Response>(async () => await this._supplierService.Get(searchModel)).Result;
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
        public Response Post([FromBody]IList<DomainModel.Supplier> supplierModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierModel, ValidationType.Add);
                return this._supplierService.Add(supplierModel);
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
        public Response Put([FromBody]IList<DomainModel.Supplier> supplierModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierModel, ValidationType.Update);
                return this._supplierService.Modify(supplierModel);
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
        public Response Delete([FromBody]IList<DomainModel.Supplier> supplierModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierModel, ValidationType.Delete);
                return this._supplierService.Delete(supplierModel);
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

        private void AssignValues(IList<DomainModel.Supplier> supplierModel, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(supplierModel, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(supplierModel, "ModifiedBy", UserName);
        }

    }
}