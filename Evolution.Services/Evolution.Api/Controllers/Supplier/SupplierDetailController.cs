using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Model = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Api.Controllers.Supplier
{
    [Route("api/suppliers/{supplierNumber}/detail")]
    public class SupplierPODetailController : BaseController
    {
        private readonly ISupplierDetailService _service = null;
        private readonly IAppLogger<SupplierPODetailController> _logger = null;

        public SupplierPODetailController(ISupplierDetailService service, IAppLogger<SupplierPODetailController> logger)
        {
            this._service = service;
            _logger = logger;
        }

        [HttpPost]
        public Response Post([FromBody]Model.SupplierDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                return _service.Add(model);
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
        public Response Put([FromBody]Model.SupplierDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                return _service.Modify(model);
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
        public Response Delete([FromBody]Model.SupplierDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return _service.Delete(model);
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

        private void AssignValues(Model.SupplierDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.SupplierInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.SupplierContacts, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.SupplierNotes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.SupplierDocuments, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierContacts, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierNotes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierDocuments, "ModifiedBy", UserName);
            }
        }
    }
}