using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/certificates")]
    public class CertificationsController : Controller
    {
        private readonly IAppLogger<CertificationsController> _logger;
        private readonly ICertificationsService _service;

        public CertificationsController(ICertificationsService service, IAppLogger<CertificationsController> logger)
        {
            _service = service;
            _logger = logger;
        }


        // GET: api/<controller>
        [HttpGet]
        public Response Get(Certifications search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Search(search);
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
    }
}