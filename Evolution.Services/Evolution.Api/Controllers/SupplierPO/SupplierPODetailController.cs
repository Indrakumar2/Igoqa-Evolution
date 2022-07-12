using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Microsoft.AspNetCore.Mvc;
using System;
using Model = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.Api.Controllers.SupplierPO
{
    [Route("api/supplierPOs/{SupplierPOId}/detail")]
    public class SupplierPODetailController : BaseController
    {
        private readonly ISupplierPODetailService _service = null;
        private readonly IAppLogger<SupplierPODetailController> _logger = null;

        public SupplierPODetailController(ISupplierPODetailService service, IAppLogger<SupplierPODetailController> logger)
        {
            this._service = service;
            _logger = logger;
        }

        [HttpPost]
        public Response Post([FromBody]Model.SupplierPODetail model)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]Model.SupplierPODetail model)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]Model.SupplierPODetail model)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(Model.SupplierPODetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.SupplierPOInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.SupplierPOSubSupplier, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.SupplierPONotes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.SupplierPODocuments, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierPOInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierPOSubSupplier, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierPONotes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.SupplierPODocuments, "ModifiedBy", UserName);
            }
        }
    }
}