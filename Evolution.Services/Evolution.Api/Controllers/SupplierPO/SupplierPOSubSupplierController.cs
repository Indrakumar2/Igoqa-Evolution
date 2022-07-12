using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.Api.Controllers.SupplierPO
{
    [Route("api/supplierpos/{SupplierPOId}/subsuppliers")]
    [Produces("application/json")]
    public class SupplierPOSubSupplierController : BaseController
    {
        private readonly ISupplierPOSubSupplierService _service = null;
        private readonly IAppLogger<SupplierPOSubSupplierController> _logger = null;

        public SupplierPOSubSupplierController(ISupplierPOSubSupplierService service, IAppLogger<SupplierPOSubSupplierController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]int supplierPOId, [FromQuery] DomainModel.SupplierPOSubSupplier searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.SupplierPOId = supplierPOId;
                return _service.Get(searchModel);
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

        [HttpPost]
        public Response Post([FromRoute] int SupplierPOId, [FromBody] IList<DomainModel.SupplierPOSubSupplier> subSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(subSupplier, ValidationType.Add);
                return _service.Add(SupplierPOId, subSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] int SupplierPOId, [FromBody] IList<DomainModel.SupplierPOSubSupplier> subSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(subSupplier, ValidationType.Update);
                return _service.Modify(SupplierPOId, subSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] int SupplierPOId, [FromBody] IList<DomainModel.SupplierPOSubSupplier> subSupplier)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(subSupplier, ValidationType.Delete);
                return _service.Delete(SupplierPOId, subSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSupplier);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModel.SupplierPOSubSupplier> subSupplier, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(subSupplier, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(subSupplier, "ModifiedBy", UserName);
        }
    }
}