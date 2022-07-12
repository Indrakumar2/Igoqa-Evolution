using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;

namespace Evolution.AuthorizationService.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    { 
        private readonly IAppLogger<ErrorController> _logger = null;
        private readonly JObject _messages = null;

        public ErrorController(IAppLogger<ErrorController> logger, JObject messages)
        {
            _logger = logger;
            _messages = messages;
        }

        [Route("error")]
        public Response Error()
        { 
            ResponseType responseType = ResponseType.Exception;
            try
            {
                IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();
                Exception exception = context.Error; // global exception 
                _logger.LogError(ResponseType.Exception.ToId(), exception.ToFullString());
                Console.WriteLine(exception.ToFullString());
            }
            catch (Exception ex)
            { 
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, null, null, null, null, new Exception(_messages[MessageType.Exception.ToId()].ToString()));
        }

    }
}
