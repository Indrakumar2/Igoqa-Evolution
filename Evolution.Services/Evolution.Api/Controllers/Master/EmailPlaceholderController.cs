using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/email/placeholders")]
    public class EmailPlaceholderController : Controller
    {
        private readonly IEmailPlaceholderService _service = null;
        private readonly IAppLogger<EmailPlaceholderController> _logger = null;

        public EmailPlaceholderController(IEmailPlaceholderService service, IAppLogger<EmailPlaceholderController> logger)
        {
            this._service = service;
            this._logger=logger;
        }
        
        [HttpGet]
        public Response Get(EmailPlaceholder search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
            
        }        
    }
}
