using System;
using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DomainModels = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Api.Controllers.Admin
{
    [Route("api/announcements")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAppLogger<AnnouncementController> _logger;
        private readonly IAnnouncementService _service;

        public AnnouncementController(IAnnouncementService service, IAppLogger<AnnouncementController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery] DomainModels.Announcement searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetAnnouncement(searchModel);
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