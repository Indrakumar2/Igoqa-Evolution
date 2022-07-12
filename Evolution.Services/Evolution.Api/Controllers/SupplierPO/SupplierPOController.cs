using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.Api.Controllers.SupplierPO
{
    [Route("api/supplierPOs/")]
    [ApiController]

    public class SupplierPOController : BaseController
    {
        public ISupplierPOService _service = null;
        private readonly IAppLogger<SupplierPOController> _logger = null;

        public SupplierPOController(ISupplierPOService service, IAppLogger<SupplierPOController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]DomainModels.SupplierPOSearch searchModel)
        {
            return Task.Run<Response>(async () => await _service.GetAsync(searchModel)).Result;
        }

        [HttpGet]
        [Route("GetSupplierPO")]
        public Response Get([FromQuery]DomainModels.SupplierPOSearch searchModel, [FromQuery]AdditionalFilter filter = null)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Get(searchModel, filter);
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

        /*Added for Assignment Clean up*/
        [HttpGet]
        [Route("GetAssignmentSupplierPO")]
        public Response GetAssignmentSupplierPO([FromQuery]DomainModels.SupplierPOSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetSupplierPO(searchModel);
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
        public Response Post([FromBody] IList<DomainModels.SupplierPO> models)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(models, ValidationType.Add);
                return _service.Add(models);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody] IList<DomainModels.SupplierPO> models)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(models, ValidationType.Update);
                return _service.Modify(models);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody] IList<DomainModels.SupplierPO> models)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(models, ValidationType.Delete);
                return _service.Delete(models);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<DomainModels.SupplierPO> supplierPO, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(supplierPO, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(supplierPO, "ModifiedBy", UserName);
        }
    }
}