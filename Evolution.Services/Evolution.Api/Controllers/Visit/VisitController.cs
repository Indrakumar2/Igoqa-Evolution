using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Enums;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain = Evolution.Visit.Domain.Models.Visits;
namespace Evolution.Api.Controllers.Visit
{
    [Produces("application/json")]
    [Route("api/visits")]
    public class VisitController : BaseController
    {
        IVisitService _service = null;
        private readonly IAppLogger<VisitController> _logger = null;
        public VisitController(IVisitService service, IAppLogger<VisitController> logger)
        {
            this._service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]Domain.Visit search, [FromQuery] AdditionalFilter filter)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetVisit(search, filter);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetVisits")]
        public Response GetVisit([FromQuery]Domain.BaseVisit searchModel)
        {

            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetVisitData(searchModel);
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

        [HttpGet]
        [Route("GetVisitForDocumentApproval")]
        public Response GetVisitForDocumentApproval([FromQuery]Domain.BaseVisit searchModel)

        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetVisitForDocumentApproval(searchModel);
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

        [HttpGet]
        [Route("GetSearchVisits")]
        public Response GetSearchVisits([FromQuery]Domain.VisitSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return Task.Run<Response>(async () => await this._service.GetSearchVisits(searchModel)).Result;
            }
            //return _service.GetSearchVisits(searchModel);
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetVisitByID")]
        public Response GetVisitByID([FromQuery]Domain.BaseVisit searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetVisitByID(searchModel);
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

        [HttpGet]
        [Route("GetHistoricalVisits")]
        public Response GetHistoricalVisits([FromQuery]Domain.BaseVisit searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetHistoricalVisits(searchModel);
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

        [HttpGet]
        [Route("GetTemplate")]
        public Response Get(string companyCode, CompanyMessageType companyMessageType, EmailKey emailKey)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetTemplate(companyCode, companyMessageType, emailKey);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("SupplierList")]
        public Response GetSupplierList([FromQuery]Domain.BaseVisit searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetSupplierList(searchModel);
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

        [HttpGet]
        [Route("TechnicalSpecialistList")]
        public Response GetTechnicalSpecialistList([FromQuery]Domain.BaseVisit searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetTechnicalSpecialistList(searchModel);
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

        [HttpGet]
        [Route("GetFinalVisitId")]
        public Response GetFinalVisitId([FromQuery]Domain.BaseVisit searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetFinalVisitId(searchModel);
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

        [HttpGet]
        [Route("GetVisitValidationData")]
        public Response GetVisitValidationData([FromQuery]Domain.BaseVisit searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetVisitValidationData(searchModel);
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

        [HttpGet]
        [Route("GetIntertekWorkHistoryReport")]
        public Response GetIntertekWorkHistoryReport([FromQuery] int epin)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _service.GetIntertekWorkHistoryReport(epin);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), epin);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(IList<Domain.BaseVisit> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

        //[HttpPost]
        //public Response Post([FromBody]IList<Domain.Visit> visitModel)
        //{
        //    return this._service.SaveVisit(visitModel);
        //}

        //[HttpPut]
        //public Response Put([FromBody]IList<Domain.Visit> visitModel)
        //{
        //    return this._service.ModifyVisit(visitModel);
        //}

        //[HttpDelete]
        //public Response Delete([FromBody]IList<Domain.Visit> visitModel)
        //{
        //    return this._service.DeleteVisit(visitModel);
        //}
    }
}
