using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/unusedReason")]
    public class UnusedReasonController : Controller
    {
        private readonly IAppLogger<UnusedReasonController> _logger;
        private readonly IUnusedReasonService _unusedReasonService;


        public UnusedReasonController(IUnusedReasonService unusedReasonsService, IAppLogger<UnusedReasonController> logger)
        {
            _unusedReasonService = unusedReasonsService;
            _logger = logger;
        }


        [HttpGet]
        public Response Get(UnusedReason searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _unusedReasonService.UnusedReasonSearch(searchModel);
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
    }
}