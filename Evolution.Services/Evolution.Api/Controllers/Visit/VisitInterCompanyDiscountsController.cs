using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using System;
using Domain = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Api.Controllers.Visit
{
    [Produces("application/json")]
    [Route("api/visits/{visitId}/intercompanydiscounts")]
    public class VisitInterCompanyDiscountsController : Controller
    {
        IVisitInterCompanyService _service = null;
        private readonly IAppLogger<VisitInterCompanyDiscountsController> _logger = null;

        public VisitInterCompanyDiscountsController(IVisitInterCompanyService service, IAppLogger<VisitInterCompanyDiscountsController> logger)
        {
            this._service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] long visitId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetVisitInterCompany(visitId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        //[HttpPost]
        //public Response Post([FromQuery]IList<Domain.VisitInterCompanyDiscounts> postModel)
        //{
        //    return _service.SaveVisitInterCompany(postModel);
        //}

        //[HttpPut]
        //public Response Put([FromQuery]IList<Domain.VisitInterCompanyDiscounts> putModel)
        //{
        //    return _service.ModifyVisitInterCompany(putModel);
        //}
    }
}
