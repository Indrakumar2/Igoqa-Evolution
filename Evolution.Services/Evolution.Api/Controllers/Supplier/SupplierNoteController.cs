using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Domain = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Api.Controllers.Supplier
{
    [Route("api/suppliers/{suppliernumber}/notes")]
    [ApiController]

    public class SupplierNoteController : BaseController
    {
        private readonly ISupplierNoteService _supplierNoteService = null;
        private readonly IAppLogger<SupplierNoteController> _logger = null;

        public SupplierNoteController(ISupplierNoteService supplierNoteService, IAppLogger<SupplierNoteController> logger)
        {
            this._supplierNoteService = supplierNoteService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]int supplierNumber, [FromQuery]Domain.SupplierNote searchModel, [FromQuery] AdditionalFilter additionalFilter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.SupplierId = supplierNumber;
            return this._supplierNoteService.Get(searchModel);
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
        public Response Post([FromRoute] int supplierNumber, [FromBody] IList<Domain.SupplierNote> supplierNotemodel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(supplierNotemodel, ValidationType.Add);
                return this._supplierNoteService.Add(supplierNotemodel);
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

        private void AssignValues(IList<Domain.SupplierNote> supplierNote,ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(supplierNote, "ActionByUser", UserName);
            //if(validationType!=ValidationType.Add)
            //ObjectExtension.SetPropertyValue(supplierNote, "ModifiedBy", UserName);
        }
    }
}