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
using Domain = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.Api.Controllers.SupplierPO
{
    [Produces("application/json")]
    [Route("api/supplierpos/{SupplierPOId}/notes")]

    public class SupplierPONoteController : BaseController
    {
        private readonly ISupplierPONoteService _supplierPONoteService = null;
        private readonly IAppLogger<SupplierPONoteController> _logger = null;

        public SupplierPONoteController(ISupplierPONoteService supplierPONoteService, IAppLogger<SupplierPONoteController> logger)
        {
            _supplierPONoteService = supplierPONoteService;
            _logger = logger;
        }


        [HttpGet]
        public Response Get([FromRoute]int SupplierPOId, [FromQuery]Domain.SupplierPONote searchModel, [FromQuery] AdditionalFilter additionalFilter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.SupplierPOId = SupplierPOId;
                return this._supplierPONoteService.Get(searchModel);
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
        public Response Post([FromRoute]int SupplierPOId, [FromBody]IList<Domain.SupplierPONote> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(searchModel, ValidationType.Add);
                return this._supplierPONoteService.Add(searchModel);
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

        private void AssignValues(IList<Domain.SupplierPONote> supplierPONote, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(supplierPONote, "ActionByUser", UserName);
            //if (validationType != ValidationType.Add)
            //    ObjectExtension.SetPropertyValue(supplierPONote, "ModifiedBy", UserName);
        }
    }
}