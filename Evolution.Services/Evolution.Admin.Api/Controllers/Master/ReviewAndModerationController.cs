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
    [Route("api/master/reviewandmoderation/process")]
    public class ReviewAndModerationController : Controller
    {
        private readonly IAppLogger<ReviewAndModerationController> _logger;

        private readonly IReviewAndModerationService _reviewModeartionService;

        public ReviewAndModerationController(IReviewAndModerationService reviewModeartionService,
            IAppLogger<ReviewAndModerationController> logger)
        {
            _reviewModeartionService = reviewModeartionService;
            _logger = logger;
        }


        // GET: api/values
        [HttpGet]
        public Response Get(ReviewAndModeration reviewAndModeration)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _reviewModeartionService.Search(reviewAndModeration);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), reviewAndModeration);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}